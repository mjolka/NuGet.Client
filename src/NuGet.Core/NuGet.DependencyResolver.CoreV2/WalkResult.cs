// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.RuntimeModel;

namespace NuGet.DependencyResolverV2
{
    public class WalkResult
    {
        private readonly string _runtimeName;
        private readonly RuntimeGraph _runtimeGraph;
        private readonly LibraryRange _libraryRange;
        private readonly Dictionary<LibraryRange, GraphItem<RemoteResolveResult>> _items;

        public WalkResult(string runtimeName, RuntimeGraph runtimeGraph, LibraryRange libraryRange, Dictionary<LibraryRange, GraphItem<RemoteResolveResult>> items)
        {
            _runtimeName = runtimeName;
            _runtimeGraph = runtimeGraph;
            _libraryRange = libraryRange;
            _items = items;
        }

        public GraphItem<RemoteResolveResult> this[LibraryRange range]
        {
            get
            {
                _items.TryGetValue(range, out var item);
                return item;
            }
        }

        public AnalyzeResult Analyze()
        {
            var result = new AnalyzeResult();

            var acceptedItems = new Dictionary<string, List<LibraryDependency>>(StringComparer.OrdinalIgnoreCase);

            CheckCycleAndNearestWins(result.Downgrades, result.Cycles);
            TryResolveConflicts(result.VersionConflicts, acceptedItems);

            // Remove all downgrades that didn't result in selecting the node we actually downgraded to
            result.Downgrades.RemoveAll(d => !IsRelevantDowngrade(d, acceptedItems));

            return result;
        }

        /// <summary>
        /// A downgrade is relevant if the node itself was `Accepted`.
        /// A node that itself wasn't `Accepted`, or has a parent that wasn't accepted is not relevant.
        /// </summary>
        private bool IsRelevantDowngrade(
            DowngradeResult downgradeResult,
            Dictionary<string, List<LibraryDependency>> acceptedItems)
        {
            return IsAccepted(downgradeResult.DowngradedTo[0], acceptedItems) && AreAllParentsAccepted();

            bool AreAllParentsAccepted()
            {
                var downgradedFrom = downgradeResult.DowngradedFrom;

                for (var i = 1; i < downgradedFrom.Count; i++)
                {
                    if (!IsAccepted(downgradedFrom[i], acceptedItems))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private bool IsAccepted(LibraryDependency dependency, Dictionary<string, List<LibraryDependency>> acceptedItems)
        {
            var item = _items[dependency.LibraryRange];

            return acceptedItems.TryGetValue(item.Key.Name, out var acceptedPath) &&
                   item.Equals(_items[acceptedPath[0].LibraryRange]);
        }

        private bool TryResolveConflicts(List<VersionConflictResult> versionConflicts, Dictionary<string, List<LibraryDependency>> acceptedItems)
        {
            // TODO: Recursive?
            var walker = new DepthFirstDependencyWalker(_runtimeName, _runtimeGraph, recursive: true);

            // now we walk the tree as often as it takes to determine 
            // which paths are accepted or rejected, based on conflicts occuring
            // between cousin packages

            var patience = 1000;
            var incomplete = true;

            var rejectedItems = new HashSet<GraphItem<RemoteResolveResult>>();

            while (incomplete && --patience != 0)
            {
                var tracker = new Tracker<RemoteResolveResult>();

                // Create a picture of what has not been rejected yet
                WalkTreeTrackItems(walker, rejectedItems, tracker);

                // Inform tracker of ambiguity beneath nodes that are not resolved yet
                WalkTreeMarkAmbiguousItems(walker, rejectedItems, tracker);

                WalkTreeAcceptOrRejectItems(walker, rejectedItems, acceptedItems, tracker);

                incomplete = IsIncomplete(walker, rejectedItems, acceptedItems);
            }

            WalkTreeDetectConflicts(walker, acceptedItems, versionConflicts);

            return !incomplete;
        }

        private void WalkTreeTrackItems(
            DepthFirstDependencyWalker walker,
            HashSet<GraphItem<RemoteResolveResult>> rejectedItems,
            Tracker<RemoteResolveResult> tracker)
        {
            ForEach(walker, e =>
            {
                var item = this[e.LibraryRange];
                if (rejectedItems.Contains(item))
                {
                    return false;
                }

                tracker.Track(item);
                return true;
            });
        }

        private void WalkTreeMarkAmbiguousItems(
            DepthFirstDependencyWalker walker,
            HashSet<GraphItem<RemoteResolveResult>> rejectedItems,
            Tracker<RemoteResolveResult> tracker)
        {
            var disputed = 0;
            ForEach(walker, e =>
            {
                var item = this[e.LibraryRange];
                if (rejectedItems.Contains(item))
                {
                    return false;
                }

                if (disputed > 0)
                {
                    tracker.MarkAmbiguous(item);
                }

                if (tracker.IsDisputed(item))
                {
                    disputed++;
                }

                return true;
            }, e =>
            {
                if (tracker.IsDisputed(this[e.LibraryRange]))
                {
                    disputed--;
                }
            });
        }

        private void WalkTreeAcceptOrRejectItems(
            DepthFirstDependencyWalker walker,
            HashSet<GraphItem<RemoteResolveResult>> rejectedItems,
            Dictionary<string, List<LibraryDependency>> acceptedItems,
            Tracker<RemoteResolveResult> tracker)
        {
            ForEach(walker, e =>
            {
                var item = this[e.LibraryRange];
                if (rejectedItems.Contains(item))
                {
                    return false;
                }

                if (tracker.IsAmbiguous(item))
                {
                    return false;
                }

                if (tracker.IsBestVersion(item))
                {
                    walker.Path.Push(e);
                    var path = walker.Path.ToList();
                    walker.Path.Pop();
                    // TODO: Rename to acceptedPaths?
                    acceptedItems[item.Key.Name] = path;
                    return true;
                }

                rejectedItems.Add(item);
                return false;
            });
        }

        private bool IsIncomplete(
            DepthFirstDependencyWalker walker,
            HashSet<GraphItem<RemoteResolveResult>> rejectedItems,
            Dictionary<string, List<LibraryDependency>> acceptedItems)
        {
            var isIncomplete = false;

            ForEach(walker, e =>
            {
                if (isIncomplete)
                {
                    // Don't continue to walk the tree once we know it's incomplete.
                    return false;
                }

                var item = this[e.LibraryRange];
                if (rejectedItems.Contains(item))
                {
                    return false;
                }

                if (acceptedItems.ContainsKey(item.Key.Name))
                {
                    return true;
                }

                isIncomplete = true;
                return false;
            });

            return isIncomplete;
        }

        private void WalkTreeDetectConflicts(
            DepthFirstDependencyWalker walker,
            Dictionary<string, List<LibraryDependency>> acceptedItems,
            List<VersionConflictResult> versionConflicts)
        {
            ForEach(walker, e =>
            {
                if (walker.Path.Count == 0)
                {
                    return true;
                }

                // Check if the parent was accepted
                var parentDependency = walker.Path.Peek();
                if (IsAccepted(parentDependency, acceptedItems))
                {
                    var item = _items[e.LibraryRange];
                    if (acceptedItems.TryGetValue(e.Name, out var acceptedItemPath) &&
                        !item.Equals(_items[acceptedItemPath[0].LibraryRange]) &&
                        e.LibraryRange != null &&
                        _items[acceptedItemPath[0].LibraryRange].Key.Version != null)
                    {
                        var acceptedType = LibraryDependencyTargetUtils.Parse(_items[acceptedItemPath[0].LibraryRange].Key.Type);
                        var childType = e.LibraryRange.TypeConstraint;

                        // Skip the check if a project reference override a package dependency
                        // Check the type constraints, if there is any overlap check for conflict
                        if ((acceptedType & (LibraryDependencyTarget.Project | LibraryDependencyTarget.ExternalProject)) == LibraryDependencyTarget.None
                            && (childType & acceptedType) != LibraryDependencyTarget.None)
                        {
                            var versionRange = e.LibraryRange.VersionRange;
                            var checkVersion = _items[acceptedItemPath[0].LibraryRange].Key.Version;

                            if (!versionRange.Satisfies(checkVersion))
                            {
                                walker.Path.Push(e);
                                var conflictingPath = walker.Path.ToList();
                                walker.Path.Pop();

                                versionConflicts.Add(new VersionConflictResult(acceptedItemPath, conflictingPath));
                            }
                        }
                    }
                }

                return true;
            });
        }

        private void ForEach(DepthFirstDependencyWalker walker, Func<LibraryDependency, bool> preVisit)
        {
            walker.PreVisit = preVisit;

            walker.Traverse(_libraryRange, GetItem);

            walker.PreVisit = null;
        }

        private void ForEach(DepthFirstDependencyWalker walker, Func<LibraryDependency, bool> preVisit, Action<LibraryDependency> postVisit)
        {
            walker.PreVisit = preVisit;
            walker.PostVisit = postVisit;

            walker.Traverse(_libraryRange, GetItem);

            walker.PreVisit = null;
            walker.PostVisit = null;
        }

        private GraphItem<RemoteResolveResult> GetItem(LibraryRange range)
        {
            return _items[range];
        }

        public bool Equals(GraphNode<RemoteResolveResult> root)
        {
            var stack = new Stack<GraphNode<RemoteResolveResult>>();
            stack.Push(root);

            // TODO: recursive?
            var walker = new DepthFirstDependencyWalker(_runtimeName, _runtimeGraph, recursive: true);

            var equal = true;
            ForEach(walker, dependency =>
            {
                if (!equal)
                {
                    return false;
                }

                if (stack.Count == 0)
                {
                    equal = false;
                    return false;
                }

                var node = stack.Pop();
                if (!node.Item.Equals(this[dependency.LibraryRange]))
                {
                    equal = false;
                    return false;
                }

                foreach (var innerNode in node.InnerNodes.Reverse())
                {
                    if (innerNode.Disposition != Disposition.Cycle &&
                        innerNode.Disposition != Disposition.PotentiallyDowngraded)
                    {
                        stack.Push(innerNode);
                    }
                }

                return true;
            });

            return equal && stack.Count == 0;
        }

        private void CheckCycleAndNearestWins(List<DowngradeResult> downgrades, List<List<LibraryDependency>> cycles)
        {
            // TODO: Recursive?
            var walker = new DepthFirstDependencyWalker(_runtimeName, _runtimeGraph, recursive: true);

            walker.Cycle = dependency =>
            {
                walker.Path.Push(dependency);
                cycles.Add(walker.Path.ToList());
                walker.Path.Pop();
            };

            var workingDowngrades = new Dictionary<List<LibraryDependency>, List<LibraryDependency>>();

            walker.PotentialDowngrade = dependencies =>
            {
                (LibraryDependency dependency, LibraryDependency _) = dependencies;
                // TODO: Better name for foos.
                if (walker.AncestorDependencies.TryGetValue(dependency.Name, out var foos))
                {
                    walker.Path.Push(dependency);
                    var path = walker.Path.ToList();
                    walker.Path.Pop();

                    // TODO: Better name for foo.
                    foreach (var foo in foos)
                    {
                        (LibraryDependency potentiallyConflictingDependency, var depth) = foo;
                        // Nodes that have no version range should be ignored as potential downgrades e.g. framework reference
                        if (potentiallyConflictingDependency.LibraryRange.VersionRange != null &&
                            dependency.LibraryRange.VersionRange != null &&
                            !RemoteDependencyWalker.IsGreaterThanOrEqualTo(potentiallyConflictingDependency.LibraryRange.VersionRange, dependency.LibraryRange.VersionRange))
                        {
                            // Is the resolved version actually within node's version range? This happen if there
                            // was a different request for a lower version of the library than this version range
                            // allows but no matching library was found, so the library is bumped up into this
                            // version range.
                            if (!_items.TryGetValue(potentiallyConflictingDependency.LibraryRange, out var item))
                            {
                                continue;
                            }

                            var resolvedVersion = item.Data?.Match?.Library?.Version;
                            if (resolvedVersion != null && dependency.LibraryRange.VersionRange.Satisfies(resolvedVersion))
                            {
                                continue;
                            }

                            var downgradedToPath = new List<LibraryDependency>(1 + depth);
                            downgradedToPath.Add(potentiallyConflictingDependency);
                            downgradedToPath.AddRange(walker.Path.Reverse().Take(depth).Reverse());

                            workingDowngrades[path] = downgradedToPath;
                        }
                        else
                        {
                            workingDowngrades.Remove(path);
                        }
                    }
                }
            };

            walker.Traverse(_libraryRange, libraryRange => _items[libraryRange]);

            foreach (var kvp in workingDowngrades)
            {
                downgrades.Add(new DowngradeResult(kvp.Key, kvp.Value));
            }
        }
    }
}
