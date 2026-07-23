// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSecurePasswordBoxDemoViewModel.cs.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XSecurePasswordBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XSecurePasswordBox showcase page.
/// </summary>
public sealed class XSecurePasswordBoxDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XSecurePasswordBox";

    /// <inheritdoc/>
    public override string Description => "Demonstrates secure password input with header, description, placeholder, reveal button, icons, sizes and disabled state.";


    /// <inheritdoc/>
    public override string XamlCode => """
<via:XSecurePasswordBox
    Width="340"
    Header="Password"
    Description="The password value stays inside the control."
    HasRevealButton="True"
    LeadingIcon="{via:MaterialIcon Kind=LockOutline}"
    Placeholder="Enter password" />

<via:XSecurePasswordBox
    Width="340"
    Header="Large password"
    LeadingIcon="{via:MaterialIcon Kind=ShieldKeyOutline}"
    Placeholder="Large"
    Size="Large" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XSecurePasswordBox passwordBox = new()
{
    Width = 340d,
    Header = "Password",
    Description = "The password value stays inside the control.",
    Placeholder = "Enter password",
    HasRevealButton = true,
};

passwordBox.PasswordChanged += (_, _) =>
{
    int length = passwordBox.PasswordLength;
    bool hasPassword = passwordBox.HasPassword;
};
""";
    #endregion
}
#endregion
