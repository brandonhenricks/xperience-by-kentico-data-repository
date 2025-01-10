using CMS.ContentEngine;
using CMS.Helpers;

namespace XperienceCommunity.DataRepository.Interfaces;

public interface IContentRepository<TEntity> : IRepository<TEntity> where TEntity : class, IContentItemFieldsSource
{
    /// <summary>
    /// Gets entities by the specified name asynchronously.
    /// </summary>
    /// <param name="name">The name of the entities.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">The maximum number of linked items to retrieve.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty, or name is Empty.</exception>
    Task<TEntity?> GetByNameAsync(string name, string? languageName, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by the specified identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">The maximum number of linked items to retrieve.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<TEntity?> GetByIdentifierAsync(Guid id, string? languageName, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<TEntity>> GetBySmartFolderIdAsync(int smartFolderId, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <typeparam name="T1">The type of the first entity.</typeparam>
    /// <typeparam name="T2">The type of the second entity.</typeparam>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2>(int smartFolderId, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <typeparam name="T1">The type of the first entity.</typeparam>
    /// <typeparam name="T2">The type of the second entity.</typeparam>
    /// <typeparam name="T3">The type of the third entity.</typeparam>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3>(int smartFolderId, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <typeparam name="T1">The type of the first entity.</typeparam>
    /// <typeparam name="T2">The type of the second entity.</typeparam>
    /// <typeparam name="T3">The type of the third entity.</typeparam>
    /// <typeparam name="T4">The type of the fourth entity.</typeparam>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3, T4>(int smartFolderId, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <typeparam name="T1">The type of the first entity.</typeparam>
    /// <typeparam name="T2">The type of the second entity.</typeparam>
    /// <typeparam name="T3">The type of the third entity.</typeparam>
    /// <typeparam name="T4">The type of the fourth entity.</typeparam>
    /// <typeparam name="T5">The type of the fifth entity.</typeparam>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3, T4, T5>(int smartFolderId, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder GUID asynchronously.
    /// </summary>
    /// <param name="smartFolderId">The GUID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<TEntity>> GetBySmartFolderGuidAsync(Guid smartFolderId, int maxLinkedItems = 0, Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);
}
