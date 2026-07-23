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
