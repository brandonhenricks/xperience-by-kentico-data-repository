using System.Collections.Concurrent;
using System.Reflection;

using CMS.ContentEngine;
using CMS.Websites;

namespace XperienceCommunity.DataRepository.Extensions;

/// <summary>
/// Provides extension methods for working with types.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// The name of the static string field that contains the content type name.
    /// </summary>
    private const string ContentTypeFieldName = "CONTENT_TYPE_NAME";

    /// <summary>
    /// The name of the static string field that contains the reusable field schema name.
    /// </summary>
    private const string ReusableFieldSchemaName = "REUSABLE_FIELD_SCHEMA_NAME";

    /// <summary>
    /// Thread safe dictionary for faster content type name lookup.
    /// </summary>
    private static readonly ConcurrentDictionary<string, string> sClassNames = new();

    /// <summary>
    /// Thread safe dictionary for faster schema name lookup.
    /// </summary>
    private static readonly ConcurrentDictionary<string, string> sSchemaNames = new();

    /// <summary>
    /// Determines whether the specified type inherits from <see cref="IWebPageFieldsSource"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool InheritsFromIWebPageFieldsSource(this Type type) => typeof(IWebPageFieldsSource).IsAssignableFrom(type);

    /// <summary>
    /// Determines whether the specified type inherits from <see cref="IContentItemFieldsSource"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool InheritsFromIContentItemFieldsSource(this Type type) => typeof(IContentItemFieldsSource).IsAssignableFrom(type);

    /// <summary>
    /// Gets the value of a static string field from a given type.
    /// </summary>
    /// <param name="type">The type to get the static string field from.</param>
    /// <param name="fieldName">The name of the static string field.</param>
    /// <returns>The value of the static string field if found; otherwise, an empty string.</returns>
    public static string? GetStaticString(this Type type, string fieldName)
    {
        var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        if (field == null || field.FieldType != typeof(string))
        {
            return null;
        }

        return field.GetValue(null) as string ?? null;
    }

    /// <summary>
    /// Gets the reusable field schema name for a given value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to get the reusable field schema name from.</param>
    /// <returns>The reusable field schema name if found; otherwise, null.</returns>
    public static string? GetReusableFieldSchemaName<T>(this T value)
    {
        var type = typeof(T);

        if (!type.IsInterface)
        {
            return null;
        }

        string? interfaceName = type?.Name ?? type?.GetInterfaces().FirstOrDefault()?.Name;

        if (string.IsNullOrEmpty(interfaceName))
        {
            return null;
        }

        if (sSchemaNames.TryGetValue(interfaceName, out string? schemaName))
        {
            return schemaName;
        }

        schemaName = type?.GetStaticString(ReusableFieldSchemaName);

        if (string.IsNullOrEmpty(schemaName))
        {
            return null;
        }

        sSchemaNames.TryAdd(interfaceName, schemaName);

        return schemaName;
    }

    /// <summary>
    /// Gets the content type name for a given type.
    /// </summary>
    /// <returns>The content type name.</returns>
    public static string? GetContentTypeName(this Type? type)
    {
        if (type is null)
        {
            return null;
        }

        if (!type.InheritsFromIWebPageFieldsSource() && !type.InheritsFromIContentItemFieldsSource())
        {
            return null;
        }

        if (string.IsNullOrEmpty(type.FullName))
        {
            return null;
        }

        if (sClassNames.TryGetValue(type.FullName, out string? contentTypeName))
        {
            return contentTypeName;
        }

        contentTypeName = type.GetStaticString(ContentTypeFieldName);

        if (string.IsNullOrEmpty(contentTypeName))
        {
            return null;
        }

        sClassNames.TryAdd(type.FullName, contentTypeName);

        return contentTypeName;
    }
}
