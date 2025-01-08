using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites.Routing;

using XperienceCommunity.DataRepository.Extensions;
using XperienceCommunity.DataRepository.Interfaces;
using XperienceCommunity.DataRepository.Models;

#pragma warning disable S1121
#pragma warning disable IDE0055

namespace XperienceCommunity.DataRepository;

public sealed class ContentTypeRepository<TEntity>(
    IProgressiveCache cache,
    IContentQueryExecutor executor,
    IWebsiteChannelContext websiteChannelContext,
    RepositoryOptions options)
    : BaseRepository(cache, executor,
        websiteChannelContext, options), IContentRepository<TEntity>
    where TEntity : class, IContentItemFieldsSource
{
    private readonly string contentType = typeof(TEntity)?.GetContentTypeName() ?? string.Empty;


    public override string CachePrefix => $"data|{contentType}|{WebsiteChannelContext.WebsiteChannelName}";

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Guid> nodeGuid, string? languageName,
        int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var guidList = nodeGuid?.ToArray() ?? [];

        if (guidList.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType(contentType, config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .Where(where => where.WhereIn(nameof(IContentItemFieldsSource.SystemFields.ContentItemGUID),
                guidList))).When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetAllAsync)}|{guidList.GetHashCode()}|{languageName}|{maxLinkedItems}");

        var cacheResult = await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());

            return result;
        }, cacheSettings, cancellationToken);

        return cacheResult ?? [];
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<int> itemIds, string? languageName,
        int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        int[] idList = itemIds?.ToArray() ?? [];

        if (idList.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType(contentType, config => config
                .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                .Where(where => where.WhereIn(nameof(IContentItemFieldsSource.SystemFields.ContentItemID), idList)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetAllAsync)}|{idList.GetHashCode()}|{languageName}|{maxLinkedItems}");

        var cacheResult = await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());

            return result;
        }, cacheSettings, cancellationToken);

        return cacheResult ?? [];
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType(contentType
                , config =>
                    config.When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetAllAsync)}|{languageName}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];


            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());
            return result;
        }, cacheSettings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByGuidAsync(Guid itemGuid, string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType(contentType
                , config => config
                    .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                    .Where(where =>
                        where.WhereEquals(nameof(IContentItemFieldsSource.SystemFields.ContentItemGUID), itemGuid))
                    .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var query = await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return query.FirstOrDefault();
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes, nameof(GetByGuidAsync), languageName, maxLinkedItems);

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct)).FirstOrDefault();

            cs.BoolCondition = result != null;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(
                $"contentitem|byguid|{itemGuid}|{languageName}");


            return result;
        }, cacheSettings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TSchema>> GetAllBySchema<TSchema>(string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        string? schemaName = typeof(TSchema).GetReusableFieldSchemaName();

        ArgumentException.ThrowIfNullOrEmpty(schemaName);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(parameters => parameters
            .When(maxLinkedItems > 0, linkItemOptions => linkItemOptions.WithLinkedItems(maxLinkedItems))
            .OfReusableSchema(schemaName).WithContentTypeFields()).When(!string.IsNullOrEmpty(languageName),
            lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var query = await Executor.GetMappedResult<TSchema>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return query;
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes, nameof(GetAllBySchema), schemaName, languageName, maxLinkedItems);

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TSchema>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency($"{schemaName}|all");

            return result;
        }, cacheSettings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdAsync(int id, string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);
        var builder = new ContentItemQueryBuilder();

        builder.ForContentType(contentType
                , config => config
                    .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                    .Where(where =>
                        where.WhereEquals(nameof(IContentItemFieldsSource.SystemFields.ContentItemID), id))
                    .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var query = await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return query.FirstOrDefault();
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetByIdAsync)}|{id}|{languageName}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct)).FirstOrDefault();

            cs.BoolCondition = result != null;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKey());


            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<TEntity?> GetByIdentifierAsync(Guid id, string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        var builder = new ContentItemQueryBuilder();

        builder.ForContentType(contentType,
                config => config
                    .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                    .Where(where => where
                        .WhereEquals(nameof(IContentQueryDataContainer.ContentItemGUID), id))
                    .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var query = await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return query.FirstOrDefault();
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetByIdentifierAsync)}|{id}|{languageName}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct)).FirstOrDefault();

            cs.BoolCondition = result != null;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKey());


            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<TEntity?> GetByNameAsync(string name, string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        var builder = new ContentItemQueryBuilder();

        builder.ForContentType(contentType, query =>
                query
                    .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                    .Where(where =>
                        where.WhereEquals(nameof(IContentItemFieldsSource.SystemFields.ContentItemName), name))
                    .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var query = await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return query.FirstOrDefault(x =>
                string.Equals(x.SystemFields.ContentItemName, name, StringComparison.OrdinalIgnoreCase));
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetByNameAsync)}|{name}|{languageName}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct)).FirstOrDefault(x =>
                string.Equals(x.SystemFields.ContentItemName, name, StringComparison.OrdinalIgnoreCase));

            cs.BoolCondition = result != null;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(
                $"contentitem|bycontenttype|{contentType}|{languageName}");


            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetBySmartFolderGuidAsync(Guid smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentType));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetBySmartFolderIdAsync)}|{smartFolderId}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetBySmartFolderIdAsync(int smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentType));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetBySmartFolderIdAsync)}|{smartFolderId}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2>(int smartFolderId,
        int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        string?[] contentTypes = [typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName()];

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentTypes)
            .InSmartFolder(smartFolderId));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetBySmartFolderIdAsync)}|{smartFolderId}|{maxLinkedItems}|{string.Join('|', contentTypes)}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<IContentItemFieldsSource>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());
            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3>(int smartFolderId,
        int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        string?[] contentTypes =
        [
            typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName(), typeof(T3).GetContentTypeName()
        ];

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentTypes)
            .InSmartFolder(smartFolderId));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetBySmartFolderIdAsync)}|{smartFolderId}|{maxLinkedItems}|{string.Join('|', contentTypes)}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<IContentItemFieldsSource>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3, T4>(
        int smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        string?[] contentTypes =
        [
            typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName(), typeof(T3).GetContentTypeName(),
            typeof(T4).GetContentTypeName()
        ];

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentTypes)
            .InSmartFolder(smartFolderId));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetBySmartFolderIdAsync)}|{smartFolderId}|{maxLinkedItems}|{string.Join('|', contentTypes)}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<IContentItemFieldsSource>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];


            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3, T4, T5>(
        int smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        string?[] contentTypes =
        [
            typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName(), typeof(T3).GetContentTypeName(),
            typeof(T4).GetContentTypeName(), typeof(T5).GetContentTypeName()
        ];

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentTypes)
            .InSmartFolder(smartFolderId));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetBySmartFolderIdAsync)}|{smartFolderId}|{maxLinkedItems}|{string.Join('|', contentTypes)}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<IContentItemFieldsSource>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetByTagsAsync(string columnName, IEnumerable<Guid> tagIdentifiers,
        int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);
        ArgumentException.ThrowIfNullOrEmpty(columnName);

        var tagIdents = tagIdentifiers?.ToList() ?? [];

        if (tagIdents.Count == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType(contentType, config =>
            config
                .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                    linkOptions => linkOptions.IncludeWebPageData()))
                .Where(where => where.WhereContainsTags(columnName, tagIdents)));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetByTagsAsync)}|{tagIdents.GetHashCode()}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());

            return result;
        }, cacheSettings, cancellationToken);
    }
}
