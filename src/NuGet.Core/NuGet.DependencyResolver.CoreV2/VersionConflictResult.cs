// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.LibraryModel;

namespace NuGet.DependencyResolverV2
{
    public class VersionConflictResult
    {
        public VersionConflictResult(List<LibraryDependency> selected, List<LibraryDependency> conflicting)
        {
            Selected = selected;
            Conflicting = conflicting;
        }

        public List<LibraryDependency> Selected { get; }
        public List<LibraryDependency> Conflicting { get; }
    }
}
