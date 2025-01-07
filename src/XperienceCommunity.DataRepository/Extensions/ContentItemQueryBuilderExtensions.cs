using CMS.ContentEngine;

namespace XperienceCommunity.DataRepository.Extensions;

public static class ContentItemQueryBuilderExtensions
{

    /// <summary>
    /// Conditionally applies an action to the <see cref="ContentItemQueryBuilder"/> if the specified condition is true.
    /// </summary>
    /// <param name="source">The <see cref="ContentItemQueryBuilder"/> to apply the action to.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="action">The action to apply if the condition is true.</param>
    /// <returns>The original <see cref="ContentItemQueryBuilder"/> with the action applied if the condition is true; otherwise, the original <see cref="ContentItemQueryBuilder"/>.</returns>
    public static ContentItemQueryBuilder When(this ContentItemQueryBuilder source, bool condition, Action<ContentItemQueryBuilder> action)
    {
        if (condition)
        {
            action(source);
            return source;
        }

        return source;
    }
}
