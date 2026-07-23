// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMessageBoxService.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Services;

#region ### Class XMessageBoxService ###
/// <summary>
/// Provides default WPF message box operations.
/// </summary>
public sealed class XMessageBoxService : IXMessageBoxService
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the optional message box owner.
    /// </summary>
    public Window? Owner { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public void ShowInfo(string message, string caption = "Information")
    {
        this.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <inheritdoc />
    public void ShowWarning(string message, string caption = "Warning")
    {
        this.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    /// <inheritdoc />
    public void ShowError(string message, string caption = "Error")
    {
        this.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <inheritdoc />
    public bool Confirm(string message, string caption = "Confirm")
    {
        return this.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    /// <inheritdoc />
    public MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage image)
    {
        if (this.Owner is not null)
        {
            return MessageBox.Show(this.Owner, message, caption, button, image);
        }

        return MessageBox.Show(message, caption, button, image);
    }
    #endregion
}
#endregion
