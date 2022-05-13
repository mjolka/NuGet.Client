// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.LibraryModel;

namespace NuGet.DependencyResolverV2
{
    public class DowngradeResult
    {
        public List<LibraryDependency> DowngradedFrom { get; }

        public List<LibraryDependency> DowngradedTo { get; }

        public DowngradeResult(List<LibraryDependency> downgradedFrom, List<LibraryDependency> downgradedTo)
        {
            DowngradedFrom = downgradedFrom;
            DowngradedTo = downgradedTo;
        }
    }
}
