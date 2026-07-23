// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMenuButton.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace VIA.WPF.Controls;

#region ### Class XMenuButton ###
/// <summary>
/// Represents an <see cref="XButton"/> that opens an assigned context menu.
/// </summary>
public class XMenuButton : XButton
{
    #region ### Dependency Properties ###
    public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(
        nameof(Menu), typeof(ContextMenu), typeof(XMenuButton), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty MenuPlacementProperty = DependencyProperty.Register(
        nameof(MenuPlacement), typeof(PlacementMode), typeof(XMenuButton), new FrameworkPropertyMetadata(PlacementMode.Bottom));
    #endregion

    #region ### Public Properties ###
    /// <summary>Gets or sets the menu opened by the button.</summary>
    public ContextMenu? Menu
    {
        get => (ContextMenu?)this.GetValue(MenuProperty);
        set => this.SetValue(MenuProperty, value);
    }

    /// <summary>Gets or sets the placement of the menu.</summary>
    public PlacementMode MenuPlacement
    {
        get => (PlacementMode)this.GetValue(MenuPlacementProperty);
        set => this.SetValue(MenuPlacementProperty, value);
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnClick()
    {
        base.OnClick();

        if (this.Menu is not { } menu)
        {
            return;
        }

        menu.PlacementTarget = this;
        menu.Placement = this.MenuPlacement;
        menu.IsOpen = true;
    }
    #endregion
}
#endregion
