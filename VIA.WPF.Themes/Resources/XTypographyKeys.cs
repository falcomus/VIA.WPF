// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTypographyKeys.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Themes;

#region ### Class XTypographyKeys ###
/// <summary>
/// Provides strongly typed resource keys for shared VIA.WPF typography values.
/// </summary>
public static class XTypographyKeys
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the resource key for body font family.
    /// </summary>
    public static ComponentResourceKey FontFamilyBody { get; } = new(typeof(XTypographyKeys), nameof(FontFamilyBody));

    /// <summary>
    /// Gets the resource key for code font family.
    /// </summary>
    public static ComponentResourceKey FontFamilyCode { get; } = new(typeof(XTypographyKeys), nameof(FontFamilyCode));

    /// <summary>
    /// Gets the resource key for overline font size.
    /// </summary>
    public static ComponentResourceKey FontSizeOverline { get; } = new(typeof(XTypographyKeys), nameof(FontSizeOverline));

    /// <summary>
    /// Gets the resource key for caption font size.
    /// </summary>
    public static ComponentResourceKey FontSizeCaption { get; } = new(typeof(XTypographyKeys), nameof(FontSizeCaption));

    /// <summary>
    /// Gets the resource key for small body font size.
    /// </summary>
    public static ComponentResourceKey FontSizeBodySmall { get; } = new(typeof(XTypographyKeys), nameof(FontSizeBodySmall));

    /// <summary>
    /// Gets the resource key for body font size.
    /// </summary>
    public static ComponentResourceKey FontSizeBody { get; } = new(typeof(XTypographyKeys), nameof(FontSizeBody));

    /// <summary>
    /// Gets the resource key for large body font size.
    /// </summary>
    public static ComponentResourceKey FontSizeBodyLarge { get; } = new(typeof(XTypographyKeys), nameof(FontSizeBodyLarge));

    /// <summary>
    /// Gets the resource key for subtitle font size.
    /// </summary>
    public static ComponentResourceKey FontSizeSubtitle { get; } = new(typeof(XTypographyKeys), nameof(FontSizeSubtitle));

    /// <summary>
    /// Gets the resource key for section title font size.
    /// </summary>
    public static ComponentResourceKey FontSizeSectionTitle { get; } = new(typeof(XTypographyKeys), nameof(FontSizeSectionTitle));

    /// <summary>
    /// Gets the resource key for heading font size.
    /// </summary>
    public static ComponentResourceKey FontSizeHeading { get; } = new(typeof(XTypographyKeys), nameof(FontSizeHeading));

    /// <summary>
    /// Gets the resource key for large heading font size.
    /// </summary>
    public static ComponentResourceKey FontSizeLargeHeading { get; } = new(typeof(XTypographyKeys), nameof(FontSizeLargeHeading));

    /// <summary>
    /// Gets the resource key for title font size.
    /// </summary>
    public static ComponentResourceKey FontSizeTitle { get; } = new(typeof(XTypographyKeys), nameof(FontSizeTitle));

    /// <summary>
    /// Gets the resource key for display font size.
    /// </summary>
    public static ComponentResourceKey FontSizeDisplay { get; } = new(typeof(XTypographyKeys), nameof(FontSizeDisplay));

    /// <summary>
    /// Gets the resource key for code font size.
    /// </summary>
    public static ComponentResourceKey FontSizeCode { get; } = new(typeof(XTypographyKeys), nameof(FontSizeCode));

    /// <summary>
    /// Gets the resource key for overline line height.
    /// </summary>
    public static ComponentResourceKey LineHeightOverline { get; } = new(typeof(XTypographyKeys), nameof(LineHeightOverline));

    /// <summary>
    /// Gets the resource key for caption line height.
    /// </summary>
    public static ComponentResourceKey LineHeightCaption { get; } = new(typeof(XTypographyKeys), nameof(LineHeightCaption));

    /// <summary>
    /// Gets the resource key for small body line height.
    /// </summary>
    public static ComponentResourceKey LineHeightBodySmall { get; } = new(typeof(XTypographyKeys), nameof(LineHeightBodySmall));

    /// <summary>
    /// Gets the resource key for body line height.
    /// </summary>
    public static ComponentResourceKey LineHeightBody { get; } = new(typeof(XTypographyKeys), nameof(LineHeightBody));

    /// <summary>
    /// Gets the resource key for large body line height.
    /// </summary>
    public static ComponentResourceKey LineHeightBodyLarge { get; } = new(typeof(XTypographyKeys), nameof(LineHeightBodyLarge));

    /// <summary>
    /// Gets the resource key for subtitle line height.
    /// </summary>
    public static ComponentResourceKey LineHeightSubtitle { get; } = new(typeof(XTypographyKeys), nameof(LineHeightSubtitle));

    /// <summary>
    /// Gets the resource key for section title line height.
    /// </summary>
    public static ComponentResourceKey LineHeightSectionTitle { get; } = new(typeof(XTypographyKeys), nameof(LineHeightSectionTitle));

    /// <summary>
    /// Gets the resource key for heading line height.
    /// </summary>
    public static ComponentResourceKey LineHeightHeading { get; } = new(typeof(XTypographyKeys), nameof(LineHeightHeading));

    /// <summary>
    /// Gets the resource key for large heading line height.
    /// </summary>
    public static ComponentResourceKey LineHeightLargeHeading { get; } = new(typeof(XTypographyKeys), nameof(LineHeightLargeHeading));

    /// <summary>
    /// Gets the resource key for title line height.
    /// </summary>
    public static ComponentResourceKey LineHeightTitle { get; } = new(typeof(XTypographyKeys), nameof(LineHeightTitle));

    /// <summary>
    /// Gets the resource key for display line height.
    /// </summary>
    public static ComponentResourceKey LineHeightDisplay { get; } = new(typeof(XTypographyKeys), nameof(LineHeightDisplay));

    /// <summary>
    /// Gets the resource key for code line height.
    /// </summary>
    public static ComponentResourceKey LineHeightCode { get; } = new(typeof(XTypographyKeys), nameof(LineHeightCode));

    /// <summary>
    /// Gets the resource key for regular font weight.
    /// </summary>
    public static ComponentResourceKey FontWeightRegular { get; } = new(typeof(XTypographyKeys), nameof(FontWeightRegular));

    /// <summary>
    /// Gets the resource key for medium font weight.
    /// </summary>
    public static ComponentResourceKey FontWeightMedium { get; } = new(typeof(XTypographyKeys), nameof(FontWeightMedium));

    /// <summary>
    /// Gets the resource key for semi-bold font weight.
    /// </summary>
    public static ComponentResourceKey FontWeightSemiBold { get; } = new(typeof(XTypographyKeys), nameof(FontWeightSemiBold));
    #endregion
}
#endregion
