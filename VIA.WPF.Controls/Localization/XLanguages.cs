// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLanguages.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Globalization;

namespace VIA.WPF.Localization;

#region ### Class XLanguages ###
/// <summary>
/// Provides the built-in language definitions used by the VIA.WPF language selector.
/// </summary>
public static class XLanguages
{
    #region ### Public Static Properties ###
    /// <summary>
    /// Gets the built-in German language definition.
    /// </summary>
    public static XLanguage German { get; } = new("de-DE", "Deutsch", "DE", "🇩🇪");

    /// <summary>
    /// Gets the built-in English language definition.
    /// </summary>
    public static XLanguage English { get; } = new("en-GB", "English", "EN", "🇬🇧");

    /// <summary>
    /// Gets the default German and English language list.
    /// </summary>
    public static ReadOnlyCollection<XLanguage> Default { get; } = Array.AsReadOnly(
        new[]
        {
            German,
            English
        });
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Finds the best matching language for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to match.</param>
    /// <param name="languages">The available languages.</param>
    /// <returns>The matching language, the built-in English language when available, or the first language.</returns>
    public static XLanguage? FindBestMatch(
        CultureInfo culture,
        IEnumerable<XLanguage> languages)
    {
        ArgumentNullException.ThrowIfNull(culture);
        ArgumentNullException.ThrowIfNull(languages);

        XLanguage[] languageArray = languages
            .Where(language => language is not null)
            .ToArray();

        XLanguage? exactMatch = languageArray.FirstOrDefault(
            language => StringComparer.OrdinalIgnoreCase.Equals(
                language.CultureName,
                culture.Name));

        if (exactMatch is not null)
        {
            return exactMatch;
        }

        XLanguage? neutralMatch = languageArray.FirstOrDefault(
            language => StringComparer.OrdinalIgnoreCase.Equals(
                language.Culture.TwoLetterISOLanguageName,
                culture.TwoLetterISOLanguageName));

        if (neutralMatch is not null)
        {
            return neutralMatch;
        }

        return languageArray.FirstOrDefault(
                   language => StringComparer.OrdinalIgnoreCase.Equals(
                       language.Culture.TwoLetterISOLanguageName,
                       English.Culture.TwoLetterISOLanguageName))
               ?? languageArray.FirstOrDefault();
    }
    #endregion
}
#endregion
