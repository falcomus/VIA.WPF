// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XHeaderGroup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XHeaderGroup ###
/// <summary>
/// Represents a compact labelled group of contextual header commands.
/// </summary>
public class XHeaderGroup : HeaderedContentControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Spacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(XHeaderGroup),
        new FrameworkPropertyMetadata(8d));

    /// <summary>
    /// Identifies the <see cref="IsSeparatorVisible"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsSeparatorVisibleProperty = DependencyProperty.Register(
        nameof(IsSeparatorVisible),
        typeof(bool),
        typeof(XHeaderGroup),
        new FrameworkPropertyMetadata(true));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XHeaderGroup"/> class.
    /// </summary>
    static XHeaderGroup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XHeaderGroup),
            new FrameworkPropertyMetadata(typeof(XHeaderGroup)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the distance between the label and content.
    /// </summary>
    public double Spacing
    {
        get => (double)this.GetValue(SpacingProperty);
        set => this.SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether a separator is shown after the group.
    /// </summary>
    public bool IsSeparatorVisible
    {
        get => (bool)this.GetValue(IsSeparatorVisibleProperty);
        set => this.SetValue(IsSeparatorVisibleProperty, value);
    }
    #endregion
}
#endregion
