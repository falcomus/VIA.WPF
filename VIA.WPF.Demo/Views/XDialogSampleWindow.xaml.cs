// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDialogSampleWindow.xaml.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using VIA.WPF.Demo.ViewModels;

namespace VIA.WPF.Demo.Views;

#region ### Class XDialogSampleWindow ###
/// <summary>
/// Represents the modal editor used by the XDialog showcase page.
/// </summary>
public partial class XDialogSampleWindow
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XDialogSampleWindow"/> class.
    /// </summary>
    /// <param name="profile">The editable profile.</param>
    public XDialogSampleWindow(XDialogProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        this.InitializeComponent();
        this.Profile = profile;
        this.DataContext = profile;
        this.Loaded += this.OnLoaded;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the editable profile.
    /// </summary>
    public XDialogProfile Profile { get; }
    #endregion

    #region ### Private Methods ###
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        this.Loaded -= this.OnLoaded;
        this.DisplayNameInput.Focus();
        this.DisplayNameInput.SelectAll();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(this.Profile.DisplayName))
        {
            this.DisplayNameInput.Focus();
            return;
        }

        this.DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
    }
    #endregion
}
#endregion