// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using VIA.WPF.Demo.ViewModels;

namespace VIA.WPF.Demo.Models;

#region ### Class DemoItem ###
/// <summary>
/// Represents a single showcase entry.
/// </summary>
public partial class DemoItem : ObservableObject
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the demo title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets or sets the short summary.
    /// </summary>
    public required string Summary { get; init; }

    /// <summary>
    /// Gets or sets the backing view model for the demo content.
    /// </summary>
    public required DemoPageViewModel ViewModel { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the navigation item is selected.
    /// </summary>
    [ObservableProperty]
    private bool isSelected;
    #endregion
}
#endregion