namespace XperienceCommunity.DataRepository.Models;

/// <summary>
/// Represents the options for configuring the repository.
/// </summary>
public sealed class RepositoryOptions
{
    /// <summary>
    /// Gets or sets the number of minutes to cache data.
    /// </summary>
    public int CacheMinutes { get; set; }
}
