using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

using XperienceCommunity.DataRepository.Extensions;
using XperienceCommunity.DataRepository.Interfaces;
using XperienceCommunity.DataRepository.Models;
#pragma warning disable S1121

namespace XperienceCommunity.DataRepository;

public sealed class PageTypeRepository<TEntity> : BaseRepository, IPageRepository<TEntity>
    where TEntity : class, IWebPageFieldsSource
{
    private readonly string? contentType = typeof(TEntity)?.GetContentTypeName() ?? string.Empty;


    public PageTypeRepository(IProgressiveCache cache, IContentQueryExecutor executor,
        IWebsiteChannelContext websiteChannelContext, RepositoryOptions options) : base(cache, executor,
        websiteChannelContext, options)
    {
    }

    public override string CachePrefix => $"data|{contentType}|{WebsiteChannelContext.WebsiteChannelName}";

    public async Task<IEnumerable<TEntity>> GetByTagsAsync(string columnName, IEnumerable<Guid> tagIdentifiers,
        int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
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
                        .Where(guidList.Length > 0
                            ? where => where.WhereContainsTags(columnName,
                                guidList)
                            : null));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetByTagsAsync)}|{guidList.GetHashCode()}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            if (cs.Cached = result.Count > 0)
            {
                cs.CacheDependency = CacheHelper.GetCacheDependency(
                    $"webpageitem|bychannel|{WebsiteChannelContext.WebsiteChannelName}|bycontenttype|{contentType}");
            }

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Guid> nodeGuid, string languageName,
        int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var guidList = nodeGuid?.ToArray() ?? [];

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
                        .Where(guidList.Length > 0
                            ? where => where.WhereIn(nameof(IWebPageContentQueryDataContainer.WebPageItemGUID),
                                guidList)
                            : null))
            .When(!string.IsNullOrEmpty(languageName), options => options.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetAllAsync)}|{guidList.GetHashCode()}|{languageName}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            if (cs.Cached = result.Count > 0)
            {
                cs.CacheDependency = CacheHelper.GetCacheDependency(
                    $"webpageitem|bychannel|{WebsiteChannelContext.WebsiteChannelName}|bycontenttype|{contentType}");
            }

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(string languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder()
            .ForContentType(contentType,
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                                linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetAllAsync)}|{languageName}|{maxLinkedItems}");


        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            if (cs.Cached = result.Count > 0)
            {
                cs.CacheDependency = CacheHelper.GetCacheDependency(
                    $"webpageitem|bychannel|{WebsiteChannelContext.WebsiteChannelName}|bycontenttype|{contentType}");
            }

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(int id, string languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
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

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var result = await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return result.FirstOrDefault();
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetByIdAsync)}|{id}|{languageName}|{maxLinkedItems}");


        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result =
                (await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                    cancellationToken: ct))?.FirstOrDefault();

            if (cs.Cached = result != null)
            {
                cs.CacheDependency = CacheHelper.GetCacheDependency(result.GetCacheDependencyKey());
            }

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<TEntity?> GetByGuidAsync(Guid itemGuid, string languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
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

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var result = await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return result.FirstOrDefault();
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetByGuidAsync)}|{itemGuid}|{languageName}|{maxLinkedItems}");


        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result =
                (await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                    cancellationToken: ct))?.FirstOrDefault();

            if (cs.Cached = result != null)
            {
                cs.CacheDependency = CacheHelper.GetCacheDependency($"webpageitem|byguid|{itemGuid}");
            }

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetByPathAsync(string path, string languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
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

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var result = await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return result;
        }


        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetByPathAsync)}|{path}|{languageName}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            if (cs.Cached = result.Count > 0)
            {
                cs.CacheDependency =
                    CacheHelper.GetCacheDependency($"webpageitem|bycontenttype|{contentType}");
            }

            return result;
        }, cacheSettings, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<int> itemIds, string languageName,
        int maxLinkedItems = 0, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        var builder = new ContentItemQueryBuilder()
            .ForContentType(contentType,
                config =>
                    config
                        .When(maxLinkedItems > 0, options => options.WithLinkedItems(maxLinkedItems,
                                linkOptions => linkOptions.IncludeWebPageData()))
                        .OrderBy(OrderByColumn.Asc(nameof(IWebPageFieldsSource.SystemFields.WebPageItemOrder)))
                        .Where(where =>
                            where.WhereIn(nameof(IWebPageFieldsSource.SystemFields.WebPageItemID), itemIds)))
            .When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));

        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            var result = await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: cancellationToken);

            return result;
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes,
                $"{CachePrefix}|{nameof(GetAllAsync)}|{itemIds.GetHashCode()}|{languageName}|{maxLinkedItems}");

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedWebPageResult<TEntity>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            if (cs.Cached = result.Count > 0)
            {
                cs.CacheDependency =
                    CacheHelper.GetCacheDependency(result.GetCacheDependencyKeys());
            }

            return result;
        }, cacheSettings, cancellationToken);
    }


    public async Task<IEnumerable<TSchema>> GetAllBySchema<TSchema>(string languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default)
    {
        string? schemaName = typeof(TSchema).GetReusableFieldSchemaName();

        ArgumentException.ThrowIfNullOrEmpty(schemaName);

        var builder = new ContentItemQueryBuilder();

        builder.ForContentTypes(parameters =>
        {
            parameters.When(maxLinkedItems > 0, linkItemOptions => linkItemOptions.WithLinkedItems(maxLinkedItems));
            // Retrieves all items with the given reusable schema

            parameters
                .OfReusableSchema(schemaName)
                .WithWebPageData()
                .ForWebsite(WebsiteChannelContext.WebsiteChannelName);
        }).When(!string.IsNullOrEmpty(languageName), lang => lang.InLanguage(languageName));
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

            if (cs.Cached = result.Count > 0)
            {
                cs.CacheDependency = CacheHelper.GetCacheDependency(
                    $"{schemaName}|all");
            }

            return result;
        }, cacheSettings, cancellationToken);
    }
}
