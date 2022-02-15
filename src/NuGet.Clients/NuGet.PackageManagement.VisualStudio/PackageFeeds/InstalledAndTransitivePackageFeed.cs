// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class InstalledAndTransitivePackageFeed : PlainPackageFeedBase
    {
        private readonly IEnumerable<PackageCollectionItem> _installedPackages;
        private readonly IEnumerable<PackageCollectionItem> _transitivePackages;
        private readonly IPackageMetadataProvider _metadataProvider;

        public InstalledAndTransitivePackageFeed(IEnumerable<PackageCollectionItem> installedPackages, IEnumerable<PackageCollectionItem> transitivePackages, IPackageMetadataProvider metadataProvider)
        {
            if (installedPackages == null)
            {
                throw new ArgumentNullException(nameof(installedPackages));
            }

            if (transitivePackages == null)
            {
                throw new ArgumentNullException(nameof(transitivePackages));
            }

            if (metadataProvider == null)
            {
                throw new ArgumentNullException(nameof(metadataProvider));
            }

            _installedPackages = installedPackages;
            _transitivePackages = transitivePackages;
            _metadataProvider = metadataProvider;
        }


        /// <inheritdoc cref="IPackageFeed.ContinueSearchAsync(ContinuationToken, CancellationToken)" />
        public override async Task<SearchResult<IPackageSearchMetadata>> ContinueSearchAsync(ContinuationToken continuationToken, CancellationToken cancellationToken)
        {
            var searchToken = continuationToken as FeedSearchContinuationToken;
            if (searchToken == null)
            {
                throw new InvalidOperationException(Strings.Exception_InvalidContinuationToken);
            }

            // Remove foreign transitive packages
            IEnumerable<PackageCollectionItem> pkgsWithOrigins = _transitivePackages
                .Where(t => t.PackageReferences.Any(x => x is ITransitivePackageReferenceContextInfo y && y.TransitiveOrigins.Any()));

            IEnumerable<PackageCollectionItem> pkgs = _installedPackages.Concat(pkgsWithOrigins);

            PackageCollectionItem[] allPkgs = pkgs
                .Where(p => p.Id.IndexOf(searchToken.SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                .OrderBy(p => p.Id)
                .Skip(searchToken.StartIndex)
                .ToArray();

            IEnumerable<IPackageSearchMetadata> items = await TaskCombinators.ThrottledAsync(
                allPkgs,
                (p, t) => GetPackageMetadataAsync(p, searchToken.SearchFilter.IncludePrerelease, t),
                cancellationToken);

            //  The packages were originally sorted which is important because we Skip based on that sort
            //  however the asynchronous execution has randomly reordered the set. So we need to resort. 
            SearchResult<IPackageSearchMetadata> result = SearchResult.FromItems(items.OrderBy(p => p.Identity.Id).ToArray());

            var loadingStatus = allPkgs.Length == 0 ? LoadingStatus.NoItemsFound : LoadingStatus.NoMoreItems;
            result.SourceSearchStatus = new Dictionary<string, LoadingStatus>
            {
                { "Installed", loadingStatus }
            };

            return result;
        }

        internal async Task<IPackageSearchMetadata> GetPackageMetadataAsync(PackageCollectionItem identity, bool includePrerelease, CancellationToken cancellationToken)
        {
            // first we try and load the metadata from a local package
            var packageMetadata = await _metadataProvider.GetLocalPackageMetadataAsync(identity, includePrerelease, cancellationToken);
            if (packageMetadata == null)
            {
                // and failing that we go to the network
                packageMetadata = await _metadataProvider.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken);
            }

            IEnumerable<IPackageReferenceContextInfo> transPRs = identity.PackageReferences.Where(t => t is ITransitivePackageReferenceContextInfo);

            IPackageReferenceContextInfo transPR = transPRs.OrderByDescending(x => x.Identity.Version).FirstOrDefault();

            var transPRObject = transPR as TransitivePackageReferenceContextInfo;

            IReadOnlyCollection<PackageIdentity> res = Array.Empty<PackageIdentity>();
            if (transPRObject != null)
            {
                res = transPRObject.TransitiveOrigins?.Select(to => to.Identity)?.ToArray() ?? Array.Empty<PackageIdentity>();
            }

            if (res.Any())
            {
                var ts = new TransitivePackageSearchMetadata(packageMetadata, res);

                return ts;
            }

            return packageMetadata;
        }
    }
}
