using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites.Routing;

using XperienceCommunity.DataRepository.Models;

namespace XperienceCommunity.DataRepository;

public abstract class BaseRepository
{
    protected readonly int _cacheMinutes;
    protected readonly IProgressiveCache _cache;
    protected readonly IContentQueryExecutor _executor;
    protected readonly IWebsiteChannelContext _websiteChannelContext;

    protected BaseRepository(IProgressiveCache cache,
        IContentQueryExecutor executor, IWebsiteChannelContext websiteChannelContext, RepositoryOptions options)
    {
        _cache = cache;
        _executor = executor;
        _websiteChannelContext = websiteChannelContext;
        _cacheMinutes = options.CacheMinutes;
    }


    protected ContentQueryExecutionOptions GetQueryExecutionOptions()
    {
        var queryOptions = new ContentQueryExecutionOptions { ForPreview = _websiteChannelContext.IsPreview };

        queryOptions.IncludeSecuredItems = queryOptions.IncludeSecuredItems || _websiteChannelContext.IsPreview;

        return queryOptions;
    }

    public virtual string CachePrefix => "base|data";
}
