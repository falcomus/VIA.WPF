// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLanguage.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace VIA.WPF.Localization;

#region ### Class XLanguage ###
/// <summary>
/// Describes a language that can be presented by an application language selector.
/// </summary>
public sealed class XLanguage : IEquatable<XLanguage>
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLanguage"/> class.
    /// </summary>
    /// <param name="cultureName">The culture name, for example <c>de-DE</c> or <c>en-US</c>.</param>
    /// <param name="displayName">The user-facing language name.</param>
    /// <param name="shortName">The optional compact language name, for example <c>DE</c>.</param>
    /// <param name="flagGlyph">The optional flag glyph shown by a host application.</param>
    public XLanguage(
        string cultureName,
        string displayName,
        string? shortName = null,
        string? flagGlyph = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cultureName);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

        this.Culture = CultureInfo.GetCultureInfo(cultureName);
        this.DisplayName = displayName.Trim();
        this.ShortName = string.IsNullOrWhiteSpace(shortName)
            ? this.Culture.TwoLetterISOLanguageName.ToUpperInvariant()
            : shortName.Trim();
        this.FlagGlyph = string.IsNullOrWhiteSpace(flagGlyph)
            ? null
            : flagGlyph.Trim();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the represented culture.
    /// </summary>
    public CultureInfo Culture { get; }

    /// <summary>
    /// Gets the represented culture name.
    /// </summary>
    public string CultureName => this.Culture.Name;

    /// <summary>
    /// Gets the user-facing language name.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Gets the compact language name.
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// Gets the optional flag glyph.
    /// </summary>
    public string? FlagGlyph { get; }

    /// <summary>
    /// Gets the display text including the optional flag glyph.
    /// </summary>
    public string DisplayText => string.IsNullOrWhiteSpace(this.FlagGlyph)
        ? this.DisplayName
        : $"{this.FlagGlyph} {this.DisplayName}";

    /// <summary>
    /// Gets the compact display text including the optional flag glyph.
    /// </summary>
    public string CompactDisplayText => string.IsNullOrWhiteSpace(this.FlagGlyph)
        ? this.ShortName
        : $"{this.FlagGlyph} {this.ShortName}";
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public bool Equals(XLanguage? other)
    {
        return other is not null
            && StringComparer.OrdinalIgnoreCase.Equals(this.CultureName, other.CultureName);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is XLanguage other && this.Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(this.CultureName);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return this.DisplayName;
    }
    #endregion
}
#endregion
