// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBadgeDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XBadgeDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XBadge showcase page.
/// </summary>
public sealed class XBadgeDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the demo page title.
    /// </summary>
    public override string Title => "XBadge";

    /// <summary>
    /// Gets the demo page description.
    /// </summary>
    public override string Description => "Demonstrates compact status badges with variants, appearances, sizes and elevation.";

    /// <summary>
    /// Gets the XAML code shown on the demo page.
    /// </summary>
    public override string XamlCode => """
<via:XBadge Content="Default" />
<via:XBadge Content="Primary" Variant="Primary" />
<via:XBadge Content="Success" Variant="Success" />
<via:XBadge Content="Warning" Variant="Warning" />
<via:XBadge Content="Danger" Variant="Danger" />
<via:XBadge Content="Info" Variant="Info" />

<via:XBadge Content="Solid" Variant="Primary" Appearance="Solid" />
<via:XBadge Content="Outline" Variant="Primary" Appearance="Outline" />
<via:XBadge Content="Subtle" Variant="Primary" Appearance="Subtle" />
<via:XBadge Content="Very subtle" Variant="Primary" Appearance="VerySubtle" />
<via:XBadge Content="Ghost" Variant="Primary" Appearance="Ghost" />

<via:XBadge Content="Low" Variant="Info" Elevation="Low" />
<via:XBadge Content="Medium" Variant="Info" Elevation="Medium" />
<via:XBadge Content="High" Variant="Info" Elevation="High" />
""";

    /// <summary>
    /// Gets the C# code shown on the demo page.
    /// </summary>
    public override string CSharpCode => """
// XBadge is a pure view control.
// Bind Content, Variant, Appearance, Size or Elevation when the state is dynamic.
""";
    #endregion
}
#endregion
