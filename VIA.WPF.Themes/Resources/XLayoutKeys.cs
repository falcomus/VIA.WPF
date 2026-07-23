// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLayoutKeys.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Themes;

#region ### Class XLayoutKeys ###
/// <summary>
/// Provides strongly typed resource keys for shared VIA.WPF layout values.
/// </summary>
public static class XLayoutKeys
{
    #region ### Public Properties ###

    /// <summary>
    /// Gets the resource key for two-pixel micro spacing as thickness.
    /// </summary>
    public static ComponentResourceKey SpacingXXSmall { get; } = new(typeof(XLayoutKeys), nameof(SpacingXXSmall));

    /// <summary>
    /// Gets the resource key for two-pixel micro spacing as a scalar value.
    /// </summary>
    public static ComponentResourceKey SpacingValueXXSmall { get; } = new(typeof(XLayoutKeys), nameof(SpacingValueXXSmall));

    /// <summary>
    /// Gets the resource key for the small corner radius.
    /// </summary>
    public static ComponentResourceKey CornerRadiusSmall { get; } = new(typeof(XLayoutKeys), nameof(CornerRadiusSmall));

    /// <summary>
    /// Gets the resource key for the standard corner radius.
    /// </summary>
    public static ComponentResourceKey CornerRadiusStandard { get; } = new(typeof(XLayoutKeys), nameof(CornerRadiusStandard));

    /// <summary>
    /// Gets the resource key for the large corner radius.
    /// </summary>
    public static ComponentResourceKey CornerRadiusLarge { get; } = new(typeof(XLayoutKeys), nameof(CornerRadiusLarge));

    /// <summary>
    /// Gets the resource key for the extra large corner radius.
    /// </summary>
    public static ComponentResourceKey CornerRadiusExtraLarge { get; } = new(typeof(XLayoutKeys), nameof(CornerRadiusExtraLarge));

    /// <summary>
    /// Gets the resource key for a pill-shaped corner radius.
    /// </summary>
    public static ComponentResourceKey CornerRadiusPill { get; } = new(typeof(XLayoutKeys), nameof(CornerRadiusPill));

    /// <summary>
    /// Gets the resource key for extra small spacing as thickness.
    /// </summary>
    public static ComponentResourceKey SpacingXSmall { get; } = new(typeof(XLayoutKeys), nameof(SpacingXSmall));

    /// <summary>
    /// Gets the resource key for small spacing as thickness.
    /// </summary>
    public static ComponentResourceKey SpacingSmall { get; } = new(typeof(XLayoutKeys), nameof(SpacingSmall));

    /// <summary>
    /// Gets the resource key for medium spacing as thickness.
    /// </summary>
    public static ComponentResourceKey SpacingMedium { get; } = new(typeof(XLayoutKeys), nameof(SpacingMedium));

    /// <summary>
    /// Gets the resource key for standard spacing as thickness.
    /// </summary>
    public static ComponentResourceKey SpacingStandard { get; } = new(typeof(XLayoutKeys), nameof(SpacingStandard));

    /// <summary>
    /// Gets the resource key for large spacing as thickness.
    /// </summary>
    public static ComponentResourceKey SpacingLarge { get; } = new(typeof(XLayoutKeys), nameof(SpacingLarge));

    /// <summary>
    /// Gets the resource key for extra large spacing as thickness.
    /// </summary>
    public static ComponentResourceKey SpacingXLarge { get; } = new(typeof(XLayoutKeys), nameof(SpacingXLarge));

    /// <summary>
    /// Gets the resource key for extra small spacing as a scalar value.
    /// </summary>
    public static ComponentResourceKey SpacingValueXSmall { get; } = new(typeof(XLayoutKeys), nameof(SpacingValueXSmall));

    /// <summary>
    /// Gets the resource key for small spacing as a scalar value.
    /// </summary>
    public static ComponentResourceKey SpacingValueSmall { get; } = new(typeof(XLayoutKeys), nameof(SpacingValueSmall));

    /// <summary>
    /// Gets the resource key for medium spacing as a scalar value.
    /// </summary>
    public static ComponentResourceKey SpacingValueMedium { get; } = new(typeof(XLayoutKeys), nameof(SpacingValueMedium));

    /// <summary>
    /// Gets the resource key for standard spacing as a scalar value.
    /// </summary>
    public static ComponentResourceKey SpacingValueStandard { get; } = new(typeof(XLayoutKeys), nameof(SpacingValueStandard));

    /// <summary>
    /// Gets the resource key for large spacing as a scalar value.
    /// </summary>
    public static ComponentResourceKey SpacingValueLarge { get; } = new(typeof(XLayoutKeys), nameof(SpacingValueLarge));

    /// <summary>
    /// Gets the resource key for extra large spacing as a scalar value.
    /// </summary>
    public static ComponentResourceKey SpacingValueXLarge { get; } = new(typeof(XLayoutKeys), nameof(SpacingValueXLarge));

    /// <summary>
    /// Gets the resource key for a hairline border thickness.
    /// </summary>
    public static ComponentResourceKey BorderThicknessHairline { get; } = new(typeof(XLayoutKeys), nameof(BorderThicknessHairline));

    /// <summary>
    /// Gets the resource key for subtle border thickness.
    /// </summary>
    public static ComponentResourceKey BorderThicknessSubtle { get; } = new(typeof(XLayoutKeys), nameof(BorderThicknessSubtle));

    /// <summary>
    /// Gets the resource key for standard border thickness.
    /// </summary>
    public static ComponentResourceKey BorderThicknessStandard { get; } = new(typeof(XLayoutKeys), nameof(BorderThicknessStandard));

    /// <summary>
    /// Gets the resource key for emphasized border thickness.
    /// </summary>
    public static ComponentResourceKey BorderThicknessEmphasis { get; } = new(typeof(XLayoutKeys), nameof(BorderThicknessEmphasis));

    /// <summary>
    /// Gets the resource key for the small control height.
    /// </summary>
    public static ComponentResourceKey ControlHeightSmall { get; } = new(typeof(XLayoutKeys), nameof(ControlHeightSmall));

    /// <summary>
    /// Gets the resource key for the medium control height.
    /// </summary>
    public static ComponentResourceKey ControlHeightMedium { get; } = new(typeof(XLayoutKeys), nameof(ControlHeightMedium));

    /// <summary>
    /// Gets the resource key for the large control height.
    /// </summary>
    public static ComponentResourceKey ControlHeightLarge { get; } = new(typeof(XLayoutKeys), nameof(ControlHeightLarge));

    /// <summary>
    /// Gets the resource key for compact data-grid row height.
    /// </summary>
    public static ComponentResourceKey DataGridRowHeight { get; } = new(typeof(XLayoutKeys), nameof(DataGridRowHeight));

    /// <summary>
    /// Gets the resource key for compact data-grid header height.
    /// </summary>
    public static ComponentResourceKey DataGridHeaderHeight { get; } = new(typeof(XLayoutKeys), nameof(DataGridHeaderHeight));

    /// <summary>
    /// Gets the resource key for navigation item height.
    /// </summary>
    public static ComponentResourceKey NavigationItemHeight { get; } = new(typeof(XLayoutKeys), nameof(NavigationItemHeight));

    /// <summary>
    /// Gets the resource key for integrated command-bar height.
    /// </summary>
    public static ComponentResourceKey CommandBarHeight { get; } = new(typeof(XLayoutKeys), nameof(CommandBarHeight));

    /// <summary>
    /// Gets the resource key for command-bar action height.
    /// </summary>
    public static ComponentResourceKey CommandBarItemHeight { get; } = new(typeof(XLayoutKeys), nameof(CommandBarItemHeight));

    /// <summary>
    /// Gets the resource key for standard page padding.
    /// </summary>
    public static ComponentResourceKey PagePadding { get; } = new(typeof(XLayoutKeys), nameof(PagePadding));

    /// <summary>
    /// Gets the resource key for compact group content padding.
    /// </summary>
    public static ComponentResourceKey GroupPadding { get; } = new(typeof(XLayoutKeys), nameof(GroupPadding));

    /// <summary>
    /// Gets the resource key for standard icon size.
    /// </summary>
    public static ComponentResourceKey IconSizeStandard { get; } = new(typeof(XLayoutKeys), nameof(IconSizeStandard));

    /// <summary>
    /// Gets the resource key for fast animation durations.
    /// </summary>
    public static ComponentResourceKey AnimationDurationFast { get; } = new(typeof(XLayoutKeys), nameof(AnimationDurationFast));

    /// <summary>
    /// Gets the resource key for standard animation durations.
    /// </summary>
    public static ComponentResourceKey AnimationDurationStandard { get; } = new(typeof(XLayoutKeys), nameof(AnimationDurationStandard));

    /// <summary>
    /// Gets the resource key for slow animation durations.
    /// </summary>
    public static ComponentResourceKey AnimationDurationSlow { get; } = new(typeof(XLayoutKeys), nameof(AnimationDurationSlow));

    #endregion
}
#endregion
