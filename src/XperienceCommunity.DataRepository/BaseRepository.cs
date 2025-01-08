using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

using XperienceCommunity.DataRepository.Models;

namespace XperienceCommunity.DataRepository;

/// <summary>
/// Base repository class providing common functionality for data access.
/// </summary>
public abstract class BaseRepository
{
    /// <summary>
    /// Gets the number of minutes to cache data.
    /// </summary>
    protected readonly int CacheMinutes;

    /// <summary>
    /// Gets the progressive cache instance.
    /// </summary>
    protected readonly IProgressiveCache Cache;

    /// <summary>
    /// Gets the content query executor instance.
    /// </summary>
    protected readonly IContentQueryExecutor Executor;

    /// <summary>
    /// Gets the website channel context instance.
    /// </summary>
    protected readonly IWebsiteChannelContext WebsiteChannelContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository"/> class.
    /// </summary>
    /// <param name="cache">The progressive cache instance.</param>
    /// <param name="executor">The content query executor instance.</param>
    /// <param name="websiteChannelContext">The website channel context instance.</param>
    /// <param name="options">The repository options.</param>
    protected BaseRepository(IProgressiveCache cache,
        IContentQueryExecutor executor, IWebsiteChannelContext websiteChannelContext, RepositoryOptions options)
    {
        Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        WebsiteChannelContext = websiteChannelContext ?? throw new ArgumentNullException(nameof(websiteChannelContext));
        CacheMinutes = options?.CacheMinutes ?? 10;
    }

    /// <summary>
    /// Gets the content query execution options based on the current website channel context.
    /// </summary>
    /// <returns>The content query execution options.</returns>
    protected ContentQueryExecutionOptions GetQueryExecutionOptions()
    {
        var queryOptions = new ContentQueryExecutionOptions { ForPreview = WebsiteChannelContext.IsPreview };

        queryOptions.IncludeSecuredItems = queryOptions.IncludeSecuredItems || WebsiteChannelContext.IsPreview;

        return queryOptions;
    }

    /// <summary>
    /// Executes a page query and returns the result, utilizing caching if not in preview mode.
    /// </summary>
    /// <typeparam name="T">The type of the result items.</typeparam>
    /// <param name="builder">The content item query builder.</param>
    /// <param name="dependencyFunc">The function to create cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="cacheNameParts">The parts of the cache name.</param>
    /// <returns>The result of the query.</returns>
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

    /// <summary>
    /// Executes a content query and returns the result, utilizing caching if not in preview mode.
    /// </summary>
    /// <typeparam name="T">The type of the result items.</typeparam>
    /// <param name="builder">The content item query builder.</param>
    /// <param name="dependencyFunc">The function to create cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="cacheNameParts">The parts of the cache name.</param>
    /// <returns>The result of the query.</returns>
    protected async Task<IEnumerable<T>> ExecuteContentQuery<T>(ContentItemQueryBuilder builder, Func<CMSCacheDependency> dependencyFunc, CancellationToken cancellationToken = default,
        params object[] cacheNameParts)
    {
        var queryOptions = GetQueryExecutionOptions();

        if (WebsiteChannelContext.IsPreview)
        {
            return await Executor.GetMappedResult<T>(builder, queryOptions, cancellationToken);
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

    /// <summary>
    /// Gets the cache prefix.
    /// </summary>
    public virtual string CachePrefix => "base|data";
}
