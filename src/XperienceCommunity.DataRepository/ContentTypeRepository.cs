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
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var guidList = nodeGuid?.ToArray() ?? [];

        if (guidList.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType<TEntity>(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .Where(where => where.WhereIn(nameof(IContentItemFieldsSource.SystemFields.ContentItemGUID),
                guidList))).When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), guidList, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<int> itemIds, string? languageName,
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        int[] idList = itemIds?.ToArray() ?? [];

        if (idList.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType<TEntity>(config => config
                .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                .Where(where => where.WhereIn(nameof(IContentItemFieldsSource.SystemFields.ContentItemID), idList)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), idList, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType<TEntity>(config =>
                config.When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), contentType, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TSchema>> GetAllBySchema<TSchema>(string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        string? schemaName = typeof(TSchema).GetReusableFieldSchemaName();

        ArgumentException.ThrowIfNullOrEmpty(schemaName);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(parameters => parameters
                .When(maxLinkedItems > 0, linkItemOptions => linkItemOptions.WithLinkedItems(maxLinkedItems))
                .OfReusableSchema(schemaName)
                .WithContentTypeFields())
            .When(!string.IsNullOrEmpty(languageName),
                lang => lang.InLanguage(languageName));

        var result = await ExecuteContentQuery<TSchema>(builder,
            () => CacheHelper.GetCacheDependency($"{schemaName}|all"),
            cancellationToken, CachePrefix, nameof(GetAllBySchema), schemaName, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByGuidAsync(Guid itemGuid, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentType<TEntity>(config => config
                .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                .Where(where =>
                    where.WhereEquals(nameof(IContentItemFieldsSource.SystemFields.ContentItemGUID), itemGuid))
                .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByIdAsync), itemGuid, maxLinkedItems);

        return result.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdAsync(int id, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);
        var builder = new ContentItemQueryBuilder();

        builder.ForContentType<TEntity>(config => config
                .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                .Where(where =>
                    where.WhereEquals(nameof(IContentItemFieldsSource.SystemFields.ContentItemID), id))
                .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByIdAsync), id, maxLinkedItems);

        return result.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdentifierAsync(Guid id, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        var builder = new ContentItemQueryBuilder();

        builder.ForContentType<TEntity>(config => config
                .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                .Where(where => where
                    .WhereEquals(nameof(IContentQueryDataContainer.ContentItemGUID), id))
                .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByIdentifierAsync), id, maxLinkedItems);

        return result.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByNameAsync(string name, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        var builder = new ContentItemQueryBuilder();

        builder.ForContentType<TEntity>(query =>
                query
                    .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
                    .Where(where =>
                        where.WhereEquals(nameof(IContentItemFieldsSource.SystemFields.ContentItemName), name))
                    .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByNameAsync), name, maxLinkedItems);

        return result.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetBySmartFolderGuidAsync(Guid smartFolderId, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentType)
            .InSmartFolder(smartFolderId));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetBySmartFolderGuidAsync), smartFolderId, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetBySmartFolderIdAsync(int smartFolderId, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentType)
            .InSmartFolder(smartFolderId));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetBySmartFolderIdAsync), contentType, smartFolderId,
            maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2>(int smartFolderId,
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default)
    {
        string?[] contentTypes = [typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName()];

        if (contentTypes.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentTypes)
            .InSmartFolder(smartFolderId));

        var result = await ExecuteContentQuery<IContentItemFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetBySmartFolderIdAsync), contentTypes, smartFolderId,
            maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3>(int smartFolderId,
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default)
    {
        string?[] contentTypes =
        [
            typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName(), typeof(T3).GetContentTypeName()
        ];

        if (contentTypes.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentTypes)
            .InSmartFolder(smartFolderId));

        var result = await ExecuteContentQuery<IContentItemFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetBySmartFolderIdAsync), contentTypes, smartFolderId,
            maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3, T4>(
        int smartFolderId, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default)
    {
        string?[] contentTypes =
        [
            typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName(), typeof(T3).GetContentTypeName(),
            typeof(T4).GetContentTypeName()
        ];

        if (contentTypes.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentTypes)
            .InSmartFolder(smartFolderId));

        var result = await ExecuteContentQuery<IContentItemFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetBySmartFolderIdAsync), contentTypes, smartFolderId,
            maxLinkedItems);

        return result;
    }

    public async Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3, T4, T5>(
        int smartFolderId, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default)
    {
        string?[] contentTypes =
        [
            typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName(), typeof(T3).GetContentTypeName(),
            typeof(T4).GetContentTypeName(), typeof(T5).GetContentTypeName()
        ];

        if (contentTypes.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(config => config
            .When(maxLinkedItems > 0, linkOptions => linkOptions.WithLinkedItems(maxLinkedItems))
            .OfContentType(contentTypes)
            .InSmartFolder(smartFolderId));

        var result = await ExecuteContentQuery<IContentItemFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetBySmartFolderIdAsync), contentTypes, smartFolderId,
            maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetByTagsAsync(string columnName, IEnumerable<Guid> tagIdentifiers,
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null,
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

        builder.ForContentType<TEntity>(config =>
            config
                .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                    linkOptions => linkOptions.IncludeWebPageData()))
                .Where(where => where.WhereContainsTags(columnName, tagIdents)));

        var result = await ExecuteContentQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByTagsAsync), contentType, columnName,
            maxLinkedItems);

        return result;
    }
}
