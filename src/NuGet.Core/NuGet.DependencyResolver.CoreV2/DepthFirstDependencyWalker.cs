// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.RuntimeModel;
using GraphPath = System.Collections.Generic.Stack<NuGet.LibraryModel.LibraryDependency>;

namespace NuGet.DependencyResolverV2
{
    internal class DepthFirstDependencyWalker
    {
        private readonly string _runtimeName;
        private readonly RuntimeGraph _runtimeGraph;
        private readonly bool _recursive;

        // The names of the dependencies on the path to the current item.
        // Used to detect cycles.
        private readonly HashSet<string> _onPath = new(StringComparer.OrdinalIgnoreCase);

        public GraphPath Path { get; } = new();

        // The dependencies of each ancestor along the path to the current
        // item, excluding the dependencies that are on the path itself.
        //
        // For example, if the path is A -> B -> C -> D, then the dependencies
        // marked -*> will be in the collection.
        //
        // A --> B --> C --> D
        //               -*> E
        //         -*> E
        //   -*> C --> F
        //   -*> G
        //
        // Used to detect potential downgrades.
        //
        // We also keep track of each dependency's distance from the root
        // so that we can reconstruct the path to that dependency.
        // In the example above, we would have
        //
        // ["E"] => [3, 2]
        // ["C"] => [1]
        // ["G"] => [1]
        public Dictionary<string, Stack<(LibraryDependency LibraryDependency, int Depth)>> AncestorDependencies { get; } = new(StringComparer.OrdinalIgnoreCase);

        public Action<LibraryDependency> Cycle { get; set; }

        public Action<(LibraryDependency Dependency, LibraryDependency ConflictingDependency)> PotentialDowngrade { get; set; }

        public Func<LibraryDependency, bool> PreVisit { get; set; }

        public Action<LibraryDependency> PostVisit { get; set; }

        public DepthFirstDependencyWalker(string runtimeName, RuntimeGraph runtimeGraph, bool recursive)
        {
            _runtimeName = runtimeName;
            _runtimeGraph = runtimeGraph;
            _recursive = recursive;
        }

        public void Traverse(
            LibraryRange libraryRange,
            Func<LibraryRange, GraphItem<RemoteResolveResult>> getItem)
        {
            Traverse(new LibraryDependency { LibraryRange = libraryRange }, getItem);
        }

        private void Traverse(
            LibraryDependency dependency,
            Func<LibraryRange, GraphItem<RemoteResolveResult>> getItem)
        {
            if (PreVisit?.Invoke(dependency) == false)
            {
                return;
            }

            // Push on path to avoid cycles.
            _onPath.Add(dependency.Name);

            Path.Push(dependency);

            var childDependencies = GetDependencies(dependency, getItem);

            foreach (var childDependency in childDependencies)
            {
                if (!AncestorDependencies.TryGetValue(childDependency.Name, out var dependencies))
                {
                    dependencies = new();
                    AncestorDependencies.Add(childDependency.Name, dependencies);
                }

                dependencies.Push((childDependency, Path.Count));
            }

            foreach (var childDependency in childDependencies)
            {
                var dependencies = AncestorDependencies[childDependency.Name];
                dependencies.Pop();

                if (ShouldVisit(childDependency))
                {
                    Traverse(childDependency, getItem);
                }

                dependencies.Push((childDependency, Path.Count));
            }

            foreach (var childDependency in childDependencies)
            {
                AncestorDependencies[childDependency.Name].Pop();
            }

            Path.Pop();

            _onPath.Remove(dependency.Name);

            PostVisit?.Invoke(dependency);
        }

        public async Task TraverseAsync(
            LibraryRange libraryRange,
            Func<LibraryRange, Task<GraphItem<RemoteResolveResult>>> getItem)
        {
            await TraverseAsync(new LibraryDependency { LibraryRange = libraryRange }, getItem);
        }

        private async Task TraverseAsync(
            LibraryDependency dependency,
            Func<LibraryRange, Task<GraphItem<RemoteResolveResult>>> getItem)
        {
            if (PreVisit?.Invoke(dependency) == false)
            {
                return;
            }

            // Push on path to avoid cycles.
            _onPath.Add(dependency.Name);

            Path.Push(dependency);

            var childDependencies = await GetDependenciesAsync(dependency, getItem);

            foreach (var childDependency in childDependencies)
            {
                if (!AncestorDependencies.TryGetValue(childDependency.Name, out var dependencies))
                {
                    dependencies = new();
                    AncestorDependencies.Add(childDependency.Name, dependencies);
                }

                dependencies.Push((childDependency, Path.Count));
            }

            foreach (var childDependency in childDependencies)
            {
                var dependencies = AncestorDependencies[childDependency.Name];
                dependencies.Pop();

                if (ShouldVisit(childDependency))
                {
                    await TraverseAsync(childDependency, getItem);
                }

                dependencies.Push((childDependency, Path.Count));
            }

            foreach (var childDependency in childDependencies)
            {
                AncestorDependencies[childDependency.Name].Pop();
            }

            Path.Pop();

            _onPath.Remove(dependency.Name);

            PostVisit?.Invoke(dependency);
        }

        private bool ShouldVisit(LibraryDependency dependency)
        {
            if (!IsDependencyValidForGraph(dependency))
            {
                return false;
            }

            // Skip dependencies if the dependency edge has 'all' excluded and
            // the node is not a direct dependency.
            if (Path.Count > 1 && dependency.SuppressParent == LibraryIncludeFlags.All)
            {
                return false;
            }

            if (_onPath.Contains(dependency.Name))
            {
                // We've hit a cycle.
                Cycle?.Invoke(dependency);
                return false;
            }

            var (dependencyResult, conflictingDependency) = CalculateDependencyResult(dependency.LibraryRange);

            switch (dependencyResult)
            {
                case DependencyResult.Acceptable:
                    return true;
                case DependencyResult.Eclipsed:
                    return false;
                case DependencyResult.PotentiallyDowngraded:
                    PotentialDowngrade?.Invoke((dependency, conflictingDependency));
                    return false;
                default:
                    throw new InvalidOperationException();
            }
        }

        private List<LibraryDependency> GetDependencies(LibraryDependency dependency, Func<LibraryRange, GraphItem<RemoteResolveResult>> getItem)
        {
            var (libraryRange, dependencies, runtimeDependencies) = DoHack(dependency.LibraryRange);

            var item = getItem(libraryRange);

            return MergeRuntimeDependencies(item, dependencies, runtimeDependencies);
        }

        private async Task<List<LibraryDependency>> GetDependenciesAsync(LibraryDependency dependency, Func<LibraryRange, Task<GraphItem<RemoteResolveResult>>> getItem)
        {
            var (libraryRange, dependencies, runtimeDependencies) = DoHack(dependency.LibraryRange);

            var item = await getItem(libraryRange);

            return MergeRuntimeDependencies(item, dependencies, runtimeDependencies);
        }

        private (LibraryRange LibraryRange, List<LibraryDependency> Dependencies, HashSet<string> RuntimeDependencies) DoHack(LibraryRange libraryRange)
        {
            List<LibraryDependency> dependencies = null;
            HashSet<string> runtimeDependencies = null;

            if (_runtimeGraph != null && !string.IsNullOrEmpty(_runtimeName))
            {
                // HACK(davidfowl): This is making runtime.json support package redirects

                // Look up any additional dependencies for this package
                foreach (var runtimeDependency in _runtimeGraph.FindRuntimeDependencies(_runtimeName, libraryRange.Name))
                {
                    var libraryDependency = new LibraryDependency
                    {
                        LibraryRange = new LibraryRange()
                        {
                            Name = runtimeDependency.Id,
                            VersionRange = runtimeDependency.VersionRange,
                            TypeConstraint = LibraryDependencyTarget.PackageProjectExternal
                        }
                    };

                    if (StringComparer.OrdinalIgnoreCase.Equals(runtimeDependency.Id, libraryRange.Name))
                    {
                        if (libraryRange.VersionRange != null &&
                            runtimeDependency.VersionRange != null &&
                            libraryRange.VersionRange.MinVersion < runtimeDependency.VersionRange.MinVersion)
                        {
                            libraryRange = libraryDependency.LibraryRange;
                        }
                    }
                    else
                    {
                        // Otherwise it's a dependency of this node
                        if (dependencies == null)
                        {
                            // Init dependency lists
                            dependencies = new List<LibraryDependency>(1);
                            runtimeDependencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        }

                        dependencies.Add(libraryDependency);
                        runtimeDependencies.Add(libraryDependency.Name);
                    }
                }
            }

            return (libraryRange, dependencies, runtimeDependencies);
        }

        private static List<LibraryDependency> MergeRuntimeDependencies(GraphItem<RemoteResolveResult> item, List<LibraryDependency> dependencies, HashSet<string> runtimeDependencies)
        {
            // Merge in runtime dependencies
            if (dependencies?.Count > 0)
            {
                foreach (var nodeDep in item.Data.Dependencies)
                {
                    if (!runtimeDependencies.Contains(nodeDep.Name))
                    {
                        dependencies.Add(nodeDep);
                    }
                }

                return dependencies;
            }

            return item.Data.Dependencies;
        }

        private (DependencyResult, LibraryDependency) CalculateDependencyResult(
            LibraryRange childDependencyLibrary)
        {
            if (AncestorDependencies.TryGetValue(childDependencyLibrary.Name, out var dependencies))
            {
                foreach ((LibraryDependency d, int _) in dependencies)
                {
                    if (childDependencyLibrary.IsEclipsedBy(d.LibraryRange))
                    {
                        if (d.LibraryRange.VersionRange != null &&
                            childDependencyLibrary.VersionRange != null &&
                            !RemoteDependencyWalker.IsGreaterThanOrEqualTo(d.LibraryRange.VersionRange, childDependencyLibrary.VersionRange))
                        {
                            return (DependencyResult.PotentiallyDowngraded, d);
                        }

                        return (DependencyResult.Eclipsed, d);
                    }
                }
            }

            return (_recursive ? DependencyResult.Acceptable : DependencyResult.Eclipsed, null);
        }

        /// <summary>
        /// A centrally defined package version has the potential to become a transitive dependency.
        /// Such a dependency is defined by
        ///     ReferenceType == LibraryDependencyReferenceType.None
        /// However do not include them in the graph for the beginning.
        /// </summary>
        private static bool IsDependencyValidForGraph(LibraryDependency dependency)
        {
            return dependency.ReferenceType != LibraryDependencyReferenceType.None;
        }

        private enum DependencyResult
        {
            Acceptable,
            Eclipsed,
            PotentiallyDowngraded,
        }
    }
}
