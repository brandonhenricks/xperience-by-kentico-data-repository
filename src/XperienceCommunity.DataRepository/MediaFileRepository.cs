﻿using System.Collections.Immutable;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.MediaLibrary;

using XperienceCommunity.DataRepository.Interfaces;
using XperienceCommunity.DataRepository.Models;

namespace XperienceCommunity.DataRepository;

public sealed class MediaFileRepository : IMediaFileRepository
{
    private readonly IProgressiveCache cache;
    private readonly int cacheMinutes;

    public MediaFileRepository(IProgressiveCache progressiveCache, RepositoryOptions options)
    {
        cache = progressiveCache ?? throw new ArgumentNullException(nameof(progressiveCache));
        cacheMinutes = options?.CacheMinutes ?? 10;
    }

    /// <inheritdoc />
    public async Task<MediaLibraryInfo?> GetMediaLibraryByIdAsync(int mediaLibraryId,
        CancellationToken cancellationToken = default)
    {
        var objectQuery = await new ObjectQuery<MediaLibraryInfo>()
            .WhereEquals(nameof(MediaLibraryInfo.LibraryID), mediaLibraryId)
            .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken);

        return objectQuery?.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<ImmutableList<MediaFileInfo>> GetAssetsFromRelatedItemsAsync(IEnumerable<AssetRelatedItem> items,
        CancellationToken cancellationToken = default)
    {
        var assetItems = items?.ToList() ?? [];

        if (assetItems.Count == 0)
        {
            return [];
        }

        return await cache.LoadAsync(
            async (cacheSettings, ct) =>
            {
                var results = (await new ObjectQuery<MediaFileInfo>()
                        .ForAssets(assetItems)
                        .GetEnumerableTypedResultAsync(cancellationToken: ct))
                    .ToList() ?? [];

                string[] dependencyKeys = results
                    .Select(result => $"mediafile|{result.FileGUID}")
                    .ToArray();

                cacheSettings.CacheDependency = CacheHelper.GetCacheDependency(dependencyKeys);

                return results.ToImmutableList();
            },
            new CacheSettings(
                cacheMinutes: cacheMinutes,
                useSlidingExpiration: true,
                cacheItemNameParts:
                [
                    nameof(MediaFileRepository),
                    nameof(GetAssetsFromRelatedItemsAsync),
                    .. assetItems.OrderBy(item => item.Name).Select(item => item.Name) ?? [],
                ]
            ), cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<ImmutableList<MediaFileInfo>> GetMediaFilesAsync(IEnumerable<Guid> mediaFileGuids,
            CancellationToken cancellationToken = default)
    {
        var guidList = mediaFileGuids?.ToList() ?? [];

        if (guidList.Count == 0)
        {
            return [];
        }

        return await cache.LoadAsync(
            async (cacheSettings, ct) =>
            {
                var results = (await new ObjectQuery<MediaFileInfo>()
                        .WhereIn(nameof(MediaFileInfo.FileGUID), guidList)
                        .GetEnumerableTypedResultAsync(cancellationToken: ct))
                    ?.ToList() ?? [];

                string[] dependencyKeys = guidList
                    .Select(x => $"mediafile|{x}")
                    .ToArray();

                cacheSettings.CacheDependency = CacheHelper.GetCacheDependency(dependencyKeys);

                return results.ToImmutableList();
            },
            new CacheSettings(
                cacheMinutes: cacheMinutes,
                useSlidingExpiration: true,
                cacheItemNameParts:
                [
                    nameof(MediaFileRepository),
                    nameof(GetMediaFilesAsync),
                    guidList.GetHashCode(),
                ]
            ), cancellationToken
        );
    }
}
