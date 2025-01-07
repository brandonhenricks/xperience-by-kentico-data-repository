using CMS.ContentEngine;

namespace XperienceCommunity.DataRepository.Interfaces;

public interface IContentRepository<TEntity> : IRepository<TEntity> where TEntity : class, IContentItemFieldsSource
{
    /// <summary>
    /// Gets entities by the specified name asynchronously.
    /// </summary>
    /// <param name="name">The name of the entities.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">The maximum number of linked items to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<TEntity?> GetByNameAsync(string name, string? languageName, int maxLinkedItems = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by the specified identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">The maximum number of linked items to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    Task<TEntity?> GetByIdentifierAsync(Guid id, string? languageName, int maxLinkedItems = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<TEntity>> GetBySmartFolderIdAsync(int smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2>(int smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3>(int smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3, T4>(int smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <param name="smartFolderId">The ID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<IContentItemFieldsSource>> GetBySmartFolderIdAsync<T1, T2, T3, T4, T5>(int smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities by the specified smart folder ID asynchronously.
    /// </summary>
    /// <param name="smartFolderId">The GUID of the smart folder.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<TEntity>> GetBySmartFolderGuidAsync(Guid smartFolderId, int maxLinkedItems = 0,
        CancellationToken cancellationToken = default);

}
