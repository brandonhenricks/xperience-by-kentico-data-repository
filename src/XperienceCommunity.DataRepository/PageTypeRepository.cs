using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

using XperienceCommunity.DataRepository.Extensions;
using XperienceCommunity.DataRepository.Helpers;
using XperienceCommunity.DataRepository.Interfaces;
using XperienceCommunity.DataRepository.Models;

#pragma warning disable S1121
namespace XperienceCommunity.DataRepository;

public sealed class PageTypeRepository<TEntity>(IProgressiveCache cache, IContentQueryExecutor executor,
    IWebsiteChannelContext websiteChannelContext, RepositoryOptions options, ICacheDependencyBuilder builder) : BaseRepository(cache, executor,
    websiteChannelContext, options, builder), IPageRepository<TEntity>
    where TEntity : class, IWebPageFieldsSource
{
    private readonly string? contentType = typeof(TEntity)?.GetContentTypeName() ?? string.Empty;

    public override string CachePrefix => $"data|{contentType}|{WebsiteChannelContext.WebsiteChannelName}";

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder()
            .ForContentType<TEntity>(
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), languageName ?? string.Empty, contentType, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Guid> nodeGuid, string? languageName,
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var guidList = nodeGuid?.ToArray() ?? [];

        if (guidList.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder()
            .ForContentType<TEntity>(
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(where => where.WhereIn(nameof(IWebPageContentQueryDataContainer.WebPageItemGUID),
                                guidList)))
            .When(!string.IsNullOrEmpty(languageName), options => options.InLanguage(languageName));

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), guidList, languageName ?? string.Empty, contentType, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<int> itemIds, string? languageName,
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        int[] itemIdList = itemIds?.ToArray() ?? [];

        if (itemIdList.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder()
            .ForContentType<TEntity>(
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .Where(where =>
                            where.WhereIn(nameof(IWebPageFieldsSource.SystemFields.WebPageItemID), itemIdList)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), itemIdList, languageName ?? string.Empty, contentType, maxLinkedItems);

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
            .WithWebPageData()
            .ForWebsite(WebsiteChannelContext.WebsiteChannelName))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var query = await Executor.GetMappedResult<TSchema>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return query;
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes, "data", nameof(GetAllBySchema), schemaName, languageName, maxLinkedItems);

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<TSchema>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = CacheHelper.GetCacheDependency(
                $"{schemaName}|all");

            return result;
        }, cacheSettings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByGuidAsync(Guid itemGuid, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder()
            .ForContentType(contentType,
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(predicate =>
                            predicate.WhereEquals(nameof(IWebPageFieldsSource.SystemFields.WebPageItemGUID),
                                itemGuid))
                        .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByGuidAsync), itemGuid, contentType, maxLinkedItems);

        return result.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdAsync(int id, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder()
            .ForContentType(contentType,
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(predicate =>
                            predicate.WhereEquals(nameof(IWebPageFieldsSource.SystemFields.WebPageItemID), id))
                        .TopN(1))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByIdAsync), id, contentType, maxLinkedItems);

        return result.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetByPathAsync(string path, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder()
            .ForContentType(contentType,
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Single(path)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc ?? (() => CacheDependencyHelper.CreateWebPageItemTypeCacheDependency([contentType], WebsiteChannelContext.WebsiteChannelName)),
            cancellationToken, CachePrefix, nameof(GetByPathAsync), path, contentType, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2>(string path, string? languageName,
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        string?[] contentTypes =
        [
            typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName()
        ];

        if (contentTypes.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder()
            .ForContentTypes(
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .OfContentType(contentTypes)
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Single(path)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecutePageQuery<IWebPageFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByPathAsync), path, contentTypes, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2, T3>(string path, string? languageName,
        int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        string?[] contentTypes =
        [
            typeof(T1).GetContentTypeName(), typeof(T2).GetContentTypeName(), typeof(T3).GetContentTypeName()
        ];

        if (contentTypes.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder()
            .ForContentTypes(
                config =>
                    config
                        .OfContentType(contentTypes)
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Single(path)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecutePageQuery<IWebPageFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByPathAsync), path, contentTypes, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2, T3, T4>(string path,
        string? languageName, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
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

        var builder = new ContentItemQueryBuilder()
            .ForContentTypes(
                config =>
                    config
                        .OfContentType(contentTypes)
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Single(path)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var result = await ExecutePageQuery<IWebPageFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByPathAsync), path, contentTypes, maxLinkedItems);

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetByTagsAsync(string columnName, IEnumerable<Guid> tagIdentifiers,
                                                int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(columnName);
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var guidList = tagIdentifiers?.ToArray() ?? [];

        if (guidList.Length == 0)
        {
            return [];
        }

        var builder = new ContentItemQueryBuilder()
            .ForContentType(contentType,
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                            linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(where => where.WhereContainsTags(columnName,
                                guidList)));

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByTagsAsync), columnName, guidList, maxLinkedItems);

        return result;
    }
}
