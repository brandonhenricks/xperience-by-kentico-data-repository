using System.Collections.ObjectModel;

using CMS.ContentEngine;
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
    IWebsiteChannelContext websiteChannelContext, RepositoryOptions options, ICacheDependencyBuilder cacheDependencyBuilder, IWebPageUrlRetriever webPageUrlRetriever) : BaseRepository(cache, executor,
    websiteChannelContext, options, cacheDependencyBuilder), IPageRepository<TEntity>
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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .OrderByWebPageItemOrder()
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), languageName ?? string.Empty, contentType, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .OrderByWebPageItemOrder()
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(where => where.WhereIn(nameof(IWebPageContentQueryDataContainer.WebPageItemGUID),
                                guidList)))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), guidList, languageName ?? string.Empty, contentType, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .OrderByWebPageItemOrder()
                        .Where(where =>
                            where.WhereIn(nameof(IWebPageFieldsSource.SystemFields.WebPageItemID), itemIdList)))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllAsync), itemIdList, languageName ?? string.Empty, contentType, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
                .WithLinkedItemsAndWebPageData(maxLinkedItems)
            .OfReusableSchema(schemaName)
            .WithWebPageData()
            .ForWebsite(WebsiteChannelContext.WebsiteChannelName))
            .WithLanguage(languageName);

        var result = await ExecuteContentQuery<TSchema>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetAllBySchema), schemaName, languageName ?? string.Empty, maxLinkedItems);

        return result;
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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .OrderByWebPageItemOrder()
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(predicate =>
                            predicate.WhereEquals(nameof(IWebPageFieldsSource.SystemFields.WebPageItemGUID),
                                itemGuid))
                        .TopN(1))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByGuidAsync), itemGuid, contentType, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .OrderByWebPageItemOrder()
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(predicate =>
                            predicate.WhereEquals(nameof(IWebPageFieldsSource.SystemFields.WebPageItemID), id))
                        .TopN(1))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByIdAsync), id, contentType, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .OrderByWebPageItemOrder()
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Single(path)))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc ?? (() => CacheDependencyHelper.CreateWebPageItemTypeCacheDependency([contentType], WebsiteChannelContext.WebsiteChannelName)),
            cancellationToken, CachePrefix, nameof(GetByPathAsync), path, contentType, maxLinkedItems);


        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .OfContentType(contentTypes)
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Single(path)))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<IWebPageFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByPathAsync), path, contentTypes, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Single(path)))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<IWebPageFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByPathAsync), path, contentTypes, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Single(path)))
            .WithLanguage(languageName);

        var result = await ExecutePageQuery<IWebPageFieldsSource>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByPathAsync), path, contentTypes, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, languageName, result, cancellationToken);

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
            .ForContentType<TEntity>(config =>
                    config
                        .WithLinkedItemsAndWebPageData(maxLinkedItems)
                        .OrderByWebPageItemOrder()
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(where => where.WhereContainsTags(columnName,
                                guidList)));

        var result = await ExecutePageQuery<TEntity>(builder, dependencyFunc,
            cancellationToken, CachePrefix, nameof(GetByTagsAsync), columnName, guidList, maxLinkedItems);

        await UpdateWebPageUrls(webPageUrlRetriever, null, result, cancellationToken);

        return result;
    }


    private async Task UpdateWebPageUrls<T>(IWebPageUrlRetriever webPageUrlRetriever, string? languageName, IEnumerable<T> result, CancellationToken cancellationToken) where T : IWebPageFieldsSource
    {
        var webPageGuids = new ReadOnlyCollection<Guid>(result.Select(x => x.SystemFields.WebPageItemGUID).ToArray());

        var webpageLinks = await webPageUrlRetriever.Retrieve(webPageGuids, WebsiteChannelContext.WebsiteChannelName, languageName, WebsiteChannelContext.IsPreview, cancellationToken);

        foreach (var item in result)
        {
            item.SystemFields.WebPageUrlPath = webpageLinks.FirstOrDefault(x => x.Key == item.SystemFields.WebPageItemGUID).Value.AbsoluteUrl;
        }
    }
}
