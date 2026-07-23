// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XPasswordBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XPasswordBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XPasswordBox showcase page.
/// </summary>
public sealed class XPasswordBoxDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    private string demoPassword = "VIA.WPF-Pro-2026!";
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XPasswordBox";

    /// <inheritdoc />
    public override string Description => "Demonstrates the themed XPasswordBox with sizes, placeholder, header, description, reveal behavior, leading and trailing icons, icon templates, custom corner radius and field states.";

    /// <summary>
    /// Gets or sets the live password used by the binding sample.
    /// </summary>
    public string DemoPassword
    {
        get => this.demoPassword;
        set => this.SetProperty(ref this.demoPassword, value);
    }

    /// <inheritdoc />
    public override string XamlCode => """
<via:XPasswordBox
    Width="280"
    Header="Password"
    Placeholder="Enter password"
    Password="VIA.WPF-Pro-2026!" />

<via:XPasswordBox
    Width="280"
    Header="Small"
    Placeholder="Compact input"
    Size="Small" />

<via:XPasswordBox
    Width="320"
    Header="Account password"
    Description="Leading and trailing icons help communicate field purpose."
    Placeholder="Enter account password"
    Password="secret"
    LeadingIcon="{via:MaterialIcon Kind=LockOutline}"
    LeadingIconSize="16"
    TrailingIcon="{via:MaterialIcon Kind=ShieldCheckOutline}"
    TrailingIconSize="16" />

<via:XPasswordBox
    Width="320"
    Header="Markup extension icons"
    LeadingIcon="{via:MaterialIcon Kind=KeyVariant}"
    Placeholder="MaterialIcon markup extension"
    TrailingIcon="{via:MaterialIcon Kind=EyeOutline}" />

<via:XPasswordBox
    Width="320"
    Header="Reveal disabled"
    HasRevealButton="False"
    Password="hidden"
    Placeholder="No reveal button" />

<via:XPasswordBox
    Width="320"
    Header="Initially revealed"
    IsPasswordRevealed="True"
    Password="visible-demo"
    Placeholder="Visible on load" />

<via:XPasswordBox
    Width="320"
    Header="Two-way binding"
    Password="{Binding DemoPassword, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
    Placeholder="Bound password" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
public sealed class XPasswordBoxDemoViewModel : DemoPageViewModel
{
    private string demoPassword = "VIA.WPF-Pro-2026!";

    public string DemoPassword
    {
        get => demoPassword;
        set => SetProperty(ref demoPassword, value);
    }
}

// Useful properties shown by the page:
//
// Password
// Placeholder
// Header
// Description
// Size
// CornerRadius
// LeadingIcon / LeadingIconSize
// TrailingIcon / TrailingIconSize
// HasRevealButton
// IsPasswordRevealed
// CaretBrush
// IsEnabled
""";
    #endregion
}
#endregion
