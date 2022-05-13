// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;

namespace NuGet.DependencyResolverV2
{
    public class AnalyzeResult
    {
        public List<DowngradeResult> Downgrades { get; } = new();
        public List<VersionConflictResult> VersionConflicts { get; } = new();
        public List<List<LibraryDependency>> Cycles { get; } = new();

        public void Combine(AnalyzeResult result)
        {
            Downgrades.AddRange(result.Downgrades);
            VersionConflicts.AddRange(result.VersionConflicts);
            Cycles.AddRange(result.Cycles);
        }
    }
}
