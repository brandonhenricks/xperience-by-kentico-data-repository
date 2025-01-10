using CMS.Helpers;

namespace XperienceCommunity.DataRepository.Interfaces;

/// <summary>
/// Represents a generic repository interface for accessing data.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IRepository<TEntity>
{
    /// <summary>
    /// Gets entities by the specified tags asynchronously.
    /// </summary>
    /// <param name="columnName">The name of the column to filter by tags.</param>
    /// <param name="tagIdentifiers">The collection of tag identifiers.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if columnName is empty.</exception>
    Task<IEnumerable<TEntity>> GetByTagsAsync(string columnName, IEnumerable<Guid> tagIdentifiers,
        int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities asynchronously based on the specified node GUIDs.
    /// </summary>
    /// <param name="nodeGuid">The node GUIDs.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Guid> nodeGuid, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities asynchronously based on the specified item IDs.
    /// </summary>
    /// <param name="itemIds">The item IDs.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<int> itemIds, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities asynchronously.
    /// </summary>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<TEntity>> GetAllAsync(string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<TEntity?> GetByIdAsync(int id, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by its node GUID asynchronously.
    /// </summary>
    /// <param name="itemGuid">The item GUID of the entity.</param>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<TEntity?> GetByGuidAsync(Guid itemGuid, string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities by the specified schema asynchronously.
    /// </summary>
    /// <typeparam name="TSchema">The type of the schema.</typeparam>
    /// <param name="languageName">The language name.</param>
    /// <param name="maxLinkedItems">Maximum linked items to return.</param>
    /// <param name="dependencyFunc">The function to create a cache dependency.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown if content type is empty.</exception>
    Task<IEnumerable<TSchema>> GetAllBySchema<TSchema>(string? languageName, int maxLinkedItems = 0,
        Func<CMSCacheDependency>? dependencyFunc = null,
        CancellationToken cancellationToken = default);
}
