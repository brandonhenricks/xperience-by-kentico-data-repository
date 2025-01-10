using CMS.Helpers;

namespace XperienceCommunity.DataRepository.Interfaces
{
    /// <summary>
    /// Defines a method to create a CMS cache dependency for a collection of items.
    /// </summary>
    public interface ICacheDependencyBuilder
    {
        /// <summary>
        /// Creates a CMS cache dependency for the specified collection of items.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="items">The collection of items for which to create the cache dependency.</param>
        /// <returns>A <see cref="CMSCacheDependency"/> representing the cache dependency for the specified items.</returns>
        CMSCacheDependency? Create<T>(IEnumerable<T> items);
    }
}
