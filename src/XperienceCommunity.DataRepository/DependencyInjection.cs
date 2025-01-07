using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using XperienceCommunity.DataRepository.Interfaces;
using XperienceCommunity.DataRepository.Models;

namespace XperienceCommunity.DataRepository;

public static class DependencyInjection
{
    /// <summary>
    /// Adds Xperience data repositories to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the repositories to.</param>
    /// <param name="options">A delegate to configure the repository options.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddXperienceDataRepositories(this IServiceCollection services, Action<RepositoryOptions>? options)
    {
        var repositoryOptions = new RepositoryOptions() { CacheMinutes = 60 };

        if (options != null)
        {
            options(repositoryOptions);

            services.Configure(options);
        }
        else
        {
            services.Configure<RepositoryOptions>(options => options.CacheMinutes = 60);
        }

        services.TryAddScoped(typeof(IContentRepository<>), typeof(ContentTypeRepository<>));

        services.TryAddScoped(typeof(IPageRepository<>), typeof(PageTypeRepository<>));

        return services;
    }
}
