using CMS.Helpers;
using CMS.Websites;

namespace XperienceCommunity.DataRepository.Interfaces;

/// <summary>
/// Represents a repository for managing web pages.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IPageRepository<TEntity> : IRepository<TEntity> where TEntity : class, IWebPageFieldsSource
{
    /// <summary>
    /// Gets entities by the specified path asynchronously.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty, or path is empty.</exception>
    Task<IEnumerable<TEntity>> GetByPathAsync(string path, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified path asynchronously.
    /// </summary>
    /// <typeparam name="T1">The type of the first entity.</typeparam>
    /// <typeparam name="T2">The type of the second entity.</typeparam>
    /// <param name="path">The path.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty, or path is empty.</exception>
    Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2>(string path, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified path asynchronously.
    /// </summary>
    /// <typeparam name="T1">The type of the first entity.</typeparam>
    /// <typeparam name="T2">The type of the second entity.</typeparam>
    /// <typeparam name="T3">The type of the third entity.</typeparam>
    /// <param name="path">The path.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty, or path is empty.</exception>
    Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2, T3>(string path, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified path asynchronously.
    /// </summary>
    /// <typeparam name="T1">The type of the first entity.</typeparam>
    /// <typeparam name="T2">The type of the second entity.</typeparam>
    /// <typeparam name="T3">The type of the third entity.</typeparam>
    /// <typeparam name="T4">The type of the fourth entity.</typeparam>
    /// <param name="path">The path.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty, or path is empty.</exception>
    Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2, T3, T4>(string path, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);
}
