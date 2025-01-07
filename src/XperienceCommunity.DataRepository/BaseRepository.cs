using CMS.ContentEngine;
using CMS.Helpers;
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

    public virtual string CachePrefix => "base|data";
}
