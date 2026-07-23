// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoPageViewBase.xaml.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using VIA.WPF.Demo.ViewModels;

namespace VIA.WPF.Demo.Views;

#region ### Class DemoPageViewBase ###
/// <summary>
/// Provides the shared layout for all demo pages.
/// </summary>
public partial class DemoPageViewBase : UserControl
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="PreviewContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PreviewContentProperty = DependencyProperty.Register(
        nameof(PreviewContent),
        typeof(object),
        typeof(DemoPageViewBase),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="PreviewSurface"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PreviewSurfaceProperty = DependencyProperty.Register(
        nameof(PreviewSurface),
        typeof(string),
        typeof(DemoPageViewBase),
        new PropertyMetadata("Canvas"));

    /// <summary>
    /// Identifies the <see cref="PreviewWidth"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PreviewWidthProperty = DependencyProperty.Register(
        nameof(PreviewWidth),
        typeof(string),
        typeof(DemoPageViewBase),
        new PropertyMetadata("Fill"));

    /// <summary>
    /// Identifies the <see cref="IsPreviewEnabled"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsPreviewEnabledProperty = DependencyProperty.Register(
        nameof(IsPreviewEnabled),
        typeof(bool),
        typeof(DemoPageViewBase),
        new PropertyMetadata(true));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="DemoPageViewBase"/> class.
    /// </summary>
    public DemoPageViewBase()
    {
        this.InitializeComponent();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the visual preview content.
    /// </summary>
    public object? PreviewContent
    {
        get => this.GetValue(PreviewContentProperty);
        set => this.SetValue(PreviewContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic background used by the preview workspace.
    /// </summary>
    public string PreviewSurface
    {
        get => (string)this.GetValue(PreviewSurfaceProperty);
        set => this.SetValue(PreviewSurfaceProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the preview fills the workspace or uses a constrained reference width.
    /// </summary>
    public string PreviewWidth
    {
        get => (string)this.GetValue(PreviewWidthProperty);
        set => this.SetValue(PreviewWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the preview content is enabled.
    /// </summary>
    public bool IsPreviewEnabled
    {
        get => (bool)this.GetValue(IsPreviewEnabledProperty);
        set => this.SetValue(IsPreviewEnabledProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private void OnCopyXamlClicked(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is DemoPageViewModel viewModel)
        {
            Clipboard.SetText(viewModel.XamlCode ?? string.Empty);
        }
    }

    private void OnCopyCSharpClicked(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is DemoPageViewModel viewModel)
        {
            Clipboard.SetText(viewModel.CSharpCode ?? string.Empty);
        }
    }

    private void OnResetPreviewClicked(object sender, RoutedEventArgs e)
    {
        this.SetCurrentValue(PreviewSurfaceProperty, "Canvas");
        this.SetCurrentValue(PreviewWidthProperty, "Fill");
        this.SetCurrentValue(IsPreviewEnabledProperty, true);
    }
    #endregion
}
#endregion
