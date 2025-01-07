using CMS.Websites;

namespace XperienceCommunity.DataRepository.Extensions;

/// <summary>
/// Extension methods for <see cref="IWebPageFieldsSource"/>.
/// </summary>
public static class IWebPageFieldsSourceExtensions
{
    private const string WebPageItemCachePrefix = "webpageitem|byid|";

    /// <summary>
    /// Determines whether the specified content item is secure.
    /// </summary>
    /// <param name="source">The content item fields source.</param>
    /// <returns><c>true</c> if the content item is secure; otherwise, <c>false</c>.</returns>
    public static bool IsSecureItem(this IWebPageFieldsSource? source) => source?.SystemFields?.ContentItemIsSecured ?? false;

    /// <summary>
    /// Determines whether the collection contains any secure content items.
    /// </summary>
    /// <param name="source">The collection of content item fields sources.</param>
    /// <returns><c>true</c> if the collection contains any secure content items; otherwise, <c>false</c>.</returns>
    public static bool HasSecureItems(this IEnumerable<IWebPageFieldsSource>? source) => source?.Any(x => x.SystemFields.ContentItemIsSecured) == true;

    /// <summary>
    /// Gets the cache dependency key for the specified <see cref="IWebPageFieldsSource"/>.
    /// </summary>
    /// <param name="source">The source to get the cache dependency key for.</param>
    /// <returns>An array containing the cache dependency key.</returns>
    public static string[] GetCacheDependencyKey(this IWebPageFieldsSource? source) => source is null ? [] : [$"{WebPageItemCachePrefix}{source.SystemFields.WebPageItemID}"];

    /// <summary>
    /// Gets the cache dependency keys for the specified collection of <see cref="IWebPageFieldsSource"/>.
    /// </summary>
    /// <param name="source">The collection of sources to get the cache dependency keys for.</param>
    /// <returns>An array containing the cache dependency keys.</returns>
    public static string[] GetCacheDependencyKeys(this IEnumerable<IWebPageFieldsSource>? source) => source?.Select(x => $"{WebPageItemCachePrefix}{x.SystemFields.WebPageItemID}")?.ToArray() ?? [];

    /// <summary>
    /// Gets the web page item IDs for the specified collection of <see cref="IWebPageFieldsSource"/>.
    /// </summary>
    /// <param name="source">The collection of sources to get the web page item IDs for.</param>
    /// <returns>An enumerable containing the web page item IDs.</returns>
    public static IEnumerable<int> GetWebPageItemIds(this IEnumerable<IWebPageFieldsSource>? source) => source?.Select(x => x.SystemFields.WebPageItemID) ?? [];

    /// <summary>
    /// Gets the content types for the specified collection of <see cref="IWebPageFieldsSource"/>.
    /// </summary>
    /// <param name="source">The collection of sources to get the content types for.</param>
    /// <returns>An enumerable containing the content types.</returns>
    public static IEnumerable<string?> GetContentTypes<T>(this IEnumerable<T>? source) where T : class, IWebPageFieldsSource, new() => source?.Select(static x => x.GetType().GetContentTypeName()) ?? [];

    /// <summary>
    /// Converts the specified collection of <see cref="IWebPageFieldsSource"/> to a typed list of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to convert the collection to.</typeparam>
    /// <param name="source">The collection of sources to convert.</param>
    /// <returns>A typed list of <typeparamref name="T"/>.</returns>
    public static List<T> ToTypedList<T>(this IEnumerable<T>? source) where T : class, IWebPageFieldsSource, new() => source?.OfType<T>()?.ToList() ?? [];
}
