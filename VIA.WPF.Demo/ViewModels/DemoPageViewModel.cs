// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoPageViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class DemoPageViewModel ###
/// <summary>
/// Represents a generic showcase page.
/// </summary>
public abstract class DemoPageViewModel : ObservableObject
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets the page title.
    /// </summary>
    public abstract string Title { get; }

    /// <summary>
    /// Gets the page description.
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Gets the displayed XAML source.
    /// </summary>
    public abstract string XamlCode { get; }

    /// <summary>
    /// Gets the displayed C# source.
    /// </summary>
    public abstract string CSharpCode { get; }
    #endregion
}
#endregion