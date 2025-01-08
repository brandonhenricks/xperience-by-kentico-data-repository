using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

using XperienceCommunity.DataRepository.Models;

namespace XperienceCommunity.DataRepository;

public abstract class BaseRepository
{
    protected readonly int CacheMinutes;
    protected readonly IProgressiveCache Cache;
    protected readonly IContentQueryExecutor Executor;
    protected readonly IWebsiteChannelContext WebsiteChannelContext;

    protected BaseRepository(IProgressiveCache cache,
        IContentQueryExecutor executor, IWebsiteChannelContext websiteChannelContext, RepositoryOptions options)
    {
        Cache = cache;
        Executor = executor;
        WebsiteChannelContext = websiteChannelContext;
        CacheMinutes = options.CacheMinutes;
    }


    protected ContentQueryExecutionOptions GetQueryExecutionOptions()
    {
        var queryOptions = new ContentQueryExecutionOptions { ForPreview = WebsiteChannelContext.IsPreview };

        queryOptions.IncludeSecuredItems = queryOptions.IncludeSecuredItems || WebsiteChannelContext.IsPreview;

        return queryOptions;
    }

    protected async Task<IEnumerable<T>> ExecutePageQuery<T>(ContentItemQueryBuilder builder, Func<CMSCacheDependency> dependencyFunc, CancellationToken cancellationToken = default,
        params object[] cacheNameParts)
    {
        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedWebPageResult<T>(builder, queryOptions, cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes, cacheNameParts);

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedWebPageResult<T>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = dependencyFunc.Invoke();

            return result;
        }, cacheSettings, cancellationToken);
    }
    protected async Task<IEnumerable<T>> ExecuteContentQuery<T>(ContentItemQueryBuilder builder, Func<CMSCacheDependency> dependencyFunc, CancellationToken cancellationToken = default,
        params object[] cacheNameParts)
    {
        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedWebPageResult<T>(builder, queryOptions, cancellationToken);
        }

        var cacheSettings =
            new CacheSettings(CacheMinutes, cacheNameParts);

        return await Cache.LoadAsync(async (cs, ct) =>
        {
            var result = (await Executor.GetMappedResult<T>(builder, queryOptions,
                cancellationToken: ct))?.ToList() ?? [];

            cs.BoolCondition = result.Count > 0;

            if (!cs.Cached)
            {
                return result;
            }

            cs.CacheDependency = dependencyFunc.Invoke();

            return result;
        }, cacheSettings, cancellationToken);
    }

    public virtual string CachePrefix => "base|data";
}
