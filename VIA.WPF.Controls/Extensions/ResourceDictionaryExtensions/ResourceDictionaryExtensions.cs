// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceDictionaryExtensions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;

namespace VIA.WPF.Extensions;

#region ### Class ResourceDictionaryExtensions ###
/// <summary>
/// Provides helper methods for working with WPF resource dictionaries.
/// </summary>
public static class ResourceDictionaryExtensions
{
    #region ### Public Methods ###
    /// <summary>
    /// Tries to resolve a resource from the specified dictionary including merged dictionaries.
    /// </summary>
    /// <typeparam name="T">The expected resource type.</typeparam>
    /// <param name="dictionary">The resource dictionary.</param>
    /// <param name="key">The resource key.</param>
    /// <param name="value">The resolved resource value.</param>
    /// <returns><c>true</c> if a resource was found and has the expected type; otherwise, <c>false</c>.</returns>
    public static bool TryGetResource<T>(this ResourceDictionary? dictionary, object key, out T? value)
    {
        if (dictionary is null)
        {
            value = default;
            return false;
        }

        if (dictionary.Contains(key) && dictionary[key] is T directValue)
        {
            value = directValue;
            return true;
        }

        for (int index = dictionary.MergedDictionaries.Count - 1; index >= 0; index--)
        {
            if (dictionary.MergedDictionaries[index].TryGetResource(key, out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Sets or replaces a color resource.
    /// </summary>
    /// <param name="dictionary">The resource dictionary.</param>
    /// <param name="key">The resource key.</param>
    /// <param name="color">The color value.</param>
    public static void SetColor(this ResourceDictionary dictionary, object key, Color color)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(key);

        dictionary[key] = color;
    }

    /// <summary>
    /// Sets or replaces a solid color brush resource.
    /// </summary>
    /// <param name="dictionary">The resource dictionary.</param>
    /// <param name="key">The resource key.</param>
    /// <param name="color">The brush color.</param>
    /// <param name="freeze">A value indicating whether the created brush should be frozen.</param>
    public static void SetBrush(this ResourceDictionary dictionary, object key, Color color, bool freeze = true)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(key);

        SolidColorBrush brush = new(color);

        if (freeze && brush.CanFreeze)
        {
            brush.Freeze();
        }

        dictionary[key] = brush;
    }

    /// <summary>
    /// Finds the first merged dictionary that matches the specified predicate.
    /// </summary>
    /// <param name="dictionary">The root dictionary.</param>
    /// <param name="predicate">The dictionary predicate.</param>
    /// <returns>The matching dictionary or <see langword="null"/>.</returns>
    public static ResourceDictionary? FindMergedDictionary(this ResourceDictionary? dictionary, Predicate<ResourceDictionary> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (dictionary is null)
        {
            return null;
        }

        foreach (ResourceDictionary mergedDictionary in dictionary.MergedDictionaries)
        {
            if (predicate(mergedDictionary))
            {
                return mergedDictionary;
            }

            ResourceDictionary? nestedDictionary = mergedDictionary.FindMergedDictionary(predicate);
            if (nestedDictionary is not null)
            {
                return nestedDictionary;
            }
        }

        return null;
    }

    /// <summary>
    /// Replaces all merged dictionaries that match the specified predicate.
    /// </summary>
    /// <param name="dictionary">The root dictionary.</param>
    /// <param name="predicate">The dictionary predicate.</param>
    /// <param name="replacementFactory">The replacement dictionary factory.</param>
    /// <returns>The number of replaced dictionaries.</returns>
    public static int ReplaceMergedDictionaries(
        this ResourceDictionary dictionary,
        Predicate<ResourceDictionary> predicate,
        Func<ResourceDictionary> replacementFactory)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(replacementFactory);

        int replacedCount = 0;

        for (int index = 0; index < dictionary.MergedDictionaries.Count; index++)
        {
            ResourceDictionary mergedDictionary = dictionary.MergedDictionaries[index];

            if (predicate(mergedDictionary))
            {
                dictionary.MergedDictionaries[index] = replacementFactory();
                replacedCount++;
                continue;
            }

            replacedCount += mergedDictionary.ReplaceMergedDictionaries(predicate, replacementFactory);
        }

        return replacedCount;
    }

    /// <summary>
    /// Removes all merged dictionaries that match the specified predicate.
    /// </summary>
    /// <param name="dictionary">The root dictionary.</param>
    /// <param name="predicate">The dictionary predicate.</param>
    /// <returns>The number of removed dictionaries.</returns>
    public static int RemoveMergedDictionaries(this ResourceDictionary dictionary, Predicate<ResourceDictionary> predicate)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(predicate);

        int removedCount = 0;

        for (int index = dictionary.MergedDictionaries.Count - 1; index >= 0; index--)
        {
            ResourceDictionary mergedDictionary = dictionary.MergedDictionaries[index];

            if (predicate(mergedDictionary))
            {
                dictionary.MergedDictionaries.RemoveAt(index);
                removedCount++;
                continue;
            }

            removedCount += mergedDictionary.RemoveMergedDictionaries(predicate);
        }

        return removedCount;
    }
    #endregion
}
#endregion
