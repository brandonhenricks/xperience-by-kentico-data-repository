using System.Collections.Immutable;

using CMS.MediaLibrary;

namespace XperienceCommunity.DataRepository.Interfaces;

/// <summary>
/// Interface for media file repository to handle media file operations.
/// </summary>
public interface IMediaFileRepository
{

    /// <summary>
    /// Retrieves a media library by its identifier.
    /// </summary>
    /// <param name="mediaLibraryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represent the asynchronous operation. The task result contains a <see cref="MediaLibraryInfo"/>.</returns>
    Task<MediaLibraryInfo?> GetMediaLibraryByIdAsync(int mediaLibraryId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of media files based on the provided GUIDs.
    /// </summary>
    /// <param name="mediaFileGuids">The GUIDs of the media files to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an immutable list of <see cref="MediaFileInfo"/>.</returns>
    Task<ImmutableList<MediaFileInfo>> GetMediaFilesAsync(IEnumerable<Guid> mediaFileGuids,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of media files based on the provided related items.
    /// </summary>
    /// <param name="items">The related items to retrieve media files from.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an immutable list of <see cref="MediaFileInfo"/>.</returns>
    Task<ImmutableList<MediaFileInfo>> GetAssetsFromRelatedItemsAsync(IEnumerable<AssetRelatedItem> items,
        CancellationToken cancellationToken = default);
}
