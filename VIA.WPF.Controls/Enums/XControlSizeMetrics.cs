// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XControlSizeMetrics.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Controls;

#region ### Class XControlSizeMetrics ###
/// <summary>
/// Provides shared size metrics for VIA.WPF controls.
/// </summary>
public static class XControlSizeMetrics
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the small control height.
    /// </summary>
    public static double SmallHeight { get; } = 26d;

    /// <summary>
    /// Gets the medium control height.
    /// </summary>
    public static double MediumHeight { get; } = 30d;

    /// <summary>
    /// Gets the large control height.
    /// </summary>
    public static double LargeHeight { get; } = 32d;

    /// <summary>
    /// Gets the small control corner radius.
    /// </summary>
    public static CornerRadius SmallCornerRadius { get; } = new(2d);

    /// <summary>
    /// Gets the medium control corner radius.
    /// </summary>
    public static CornerRadius MediumCornerRadius { get; } = new(3d);

    /// <summary>
    /// Gets the large control corner radius.
    /// </summary>
    public static CornerRadius LargeCornerRadius { get; } = new(4d);

    /// <summary>
    /// Gets the small icon size.
    /// </summary>
    public static double SmallIconSize { get; } = 14d;

    /// <summary>
    /// Gets the medium icon size.
    /// </summary>
    public static double MediumIconSize { get; } = 16d;

    /// <summary>
    /// Gets the large icon size.
    /// </summary>
    public static double LargeIconSize { get; } = 18d;

    /// <summary>
    /// Gets the small badge height.
    /// </summary>
    public static double SmallBadgeHeight { get; } = 22d;

    /// <summary>
    /// Gets the medium badge height.
    /// </summary>
    public static double MediumBadgeHeight { get; } = 26d;

    /// <summary>
    /// Gets the large badge height.
    /// </summary>
    public static double LargeBadgeHeight { get; } = 30d;

    /// <summary>
    /// Gets the small icon button size.
    /// </summary>
    public static double SmallIconButtonSize { get; } = 26d;

    /// <summary>
    /// Gets the medium icon button size.
    /// </summary>
    public static double MediumIconButtonSize { get; } = 34d;

    /// <summary>
    /// Gets the large icon button size.
    /// </summary>
    public static double LargeIconButtonSize { get; } = 38d;

    /// <summary>
    /// Gets the small icon button corner radius.
    /// </summary>
    public static CornerRadius SmallIconButtonCornerRadius { get; } = new(3d);

    /// <summary>
    /// Gets the medium icon button corner radius.
    /// </summary>
    public static CornerRadius MediumIconButtonCornerRadius { get; } = new(4d);

    /// <summary>
    /// Gets the large icon button corner radius.
    /// </summary>
    public static CornerRadius LargeIconButtonCornerRadius { get; } = new(6d);

    /// <summary>
    /// Gets the small list item height.
    /// </summary>
    public static double SmallListItemHeight { get; } = 26d;

    /// <summary>
    /// Gets the medium list item height.
    /// </summary>
    public static double MediumListItemHeight { get; } = 32d;

    /// <summary>
    /// Gets the large list item height.
    /// </summary>
    public static double LargeListItemHeight { get; } = 38d;

    /// <summary>
    /// Gets the small tree item height.
    /// </summary>
    public static double SmallTreeItemHeight { get; } = 24d;

    /// <summary>
    /// Gets the medium tree item height.
    /// </summary>
    public static double MediumTreeItemHeight { get; } = 28d;

    /// <summary>
    /// Gets the large tree item height.
    /// </summary>
    public static double LargeTreeItemHeight { get; } = 34d;

    /// <summary>
    /// Gets the small inline action button size.
    /// </summary>
    public static double SmallInlineActionSize { get; } = 18d;

    /// <summary>
    /// Gets the medium inline action button size.
    /// </summary>
    public static double MediumInlineActionSize { get; } = 20d;

    /// <summary>
    /// Gets the large inline action button size.
    /// </summary>
    public static double LargeInlineActionSize { get; } = 22d;

    /// <summary>
    /// Gets the small progress bar height.
    /// </summary>
    public static double SmallProgressBarHeight { get; } = 20d;

    /// <summary>
    /// Gets the medium progress bar height.
    /// </summary>
    public static double MediumProgressBarHeight { get; } = 24d;

    /// <summary>
    /// Gets the large progress bar height.
    /// </summary>
    public static double LargeProgressBarHeight { get; } = 28d;

    /// <summary>
    /// Gets the small progress track thickness.
    /// </summary>
    public static double SmallProgressTrackThickness { get; } = 6d;

    /// <summary>
    /// Gets the medium progress track thickness.
    /// </summary>
    public static double MediumProgressTrackThickness { get; } = 8d;

    /// <summary>
    /// Gets the large progress track thickness.
    /// </summary>
    public static double LargeProgressTrackThickness { get; } = 12d;

    /// <summary>
    /// Gets the small slider height.
    /// </summary>
    public static double SmallSliderHeight { get; } = 30d;

    /// <summary>
    /// Gets the medium slider height.
    /// </summary>
    public static double MediumSliderHeight { get; } = 34d;

    /// <summary>
    /// Gets the large slider height.
    /// </summary>
    public static double LargeSliderHeight { get; } = 40d;

    /// <summary>
    /// Gets the small slider track thickness.
    /// </summary>
    public static double SmallSliderTrackThickness { get; } = 3d;

    /// <summary>
    /// Gets the medium slider track thickness.
    /// </summary>
    public static double MediumSliderTrackThickness { get; } = 4d;

    /// <summary>
    /// Gets the large slider track thickness.
    /// </summary>
    public static double LargeSliderTrackThickness { get; } = 6d;

    /// <summary>
    /// Gets the small slider thumb size.
    /// </summary>
    public static double SmallSliderThumbSize { get; } = 14d;

    /// <summary>
    /// Gets the medium slider thumb size.
    /// </summary>
    public static double MediumSliderThumbSize { get; } = 18d;

    /// <summary>
    /// Gets the large slider thumb size.
    /// </summary>
    public static double LargeSliderThumbSize { get; } = 20d;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets the control height for the specified size.
    /// </summary>
    /// <param name="size">The semantic control size.</param>
    /// <returns>The control height in device-independent pixels.</returns>
    public static double GetHeight(XControlSize size)
    {
        return size switch
        {
            XControlSize.Small => SmallHeight,
            XControlSize.Medium => MediumHeight,
            XControlSize.Large => LargeHeight,
            _ => MediumHeight
        };
    }

    /// <summary>
    /// Gets the default corner radius for the specified size.
    /// </summary>
    /// <param name="size">The semantic control size.</param>
    /// <returns>The corner radius.</returns>
    public static CornerRadius GetCornerRadius(XControlSize size)
    {
        return size switch
        {
            XControlSize.Small => SmallCornerRadius,
            XControlSize.Medium => MediumCornerRadius,
            XControlSize.Large => LargeCornerRadius,
            _ => MediumCornerRadius
        };
    }

    /// <summary>
    /// Gets the icon size for the specified size.
    /// </summary>
    /// <param name="size">The semantic control size.</param>
    /// <returns>The icon size in device-independent pixels.</returns>
    public static double GetIconSize(XControlSize size)
    {
        return size switch
        {
            XControlSize.Small => SmallIconSize,
            XControlSize.Medium => MediumIconSize,
            XControlSize.Large => LargeIconSize,
            _ => MediumIconSize
        };
    }
    #endregion
}
#endregion
