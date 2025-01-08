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
    /// <param name="maxLinkedItems">Maximum Linked Items to Return</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<TEntity>> GetByPathAsync(string path, string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Gets entities by the specified path asynchronously.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum Linked Items to Return</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2>(string path, string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified path asynchronously.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum Linked Items to Return</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2, T3>(string path, string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified path asynchronously.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum Linked Items to Return</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<IWebPageFieldsSource>> GetByPathAsync<T1, T2, T3, T4>(string path, string? languageName, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);
}
