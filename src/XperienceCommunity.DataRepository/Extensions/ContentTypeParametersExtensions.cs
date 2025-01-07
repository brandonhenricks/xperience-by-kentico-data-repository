using CMS.ContentEngine;

namespace XperienceCommunity.DataRepository.Extensions;

public static class ContentTypeParametersExtensions
{
    /// <summary>
    /// Conditionally applies an action to the <see cref="ContentTypeQueryParameters"/> instance.
    /// </summary>
    /// <param name="source">The <see cref="ContentTypeQueryParameters"/> instance.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="action">The action to apply if the condition is true.</param>
    /// <returns>The <see cref="ContentTypeQueryParameters"/> instance.</returns>
    public static ContentTypeQueryParameters When(this ContentTypeQueryParameters source, bool condition, Action<ContentTypeQueryParameters> action)
    {
        if (condition)
        {
            action(source);
            return source;
        }

        return source;
    }

    /// <summary>
    /// Conditionally applies an action to the <see cref="ContentTypesQueryParameters"/> instance.
    /// </summary>
    /// <param name="source">The <see cref="ContentTypesQueryParameters"/> instance.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="action">The action to apply if the condition is true.</param>
    /// <returns>The <see cref="ContentTypesQueryParameters"/> instance.</returns>
    public static ContentTypesQueryParameters When(this ContentTypesQueryParameters source, bool condition, Action<ContentTypesQueryParameters> action)
    {
        if (condition)
        {
            action(source);
            return source;
        }

        return source;
    }
}
