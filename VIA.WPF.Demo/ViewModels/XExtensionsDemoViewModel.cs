// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XExtensionsDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XExtensionsDemoViewModel ###
/// <summary>
/// Represents the demo view model for extensions and utility services.
/// </summary>
public sealed partial class XExtensionsDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "Extensions & Services";

    /// <inheritdoc/>
    public override string Description => "Summarizes reusable VIA.WPF helper extensions, markup extensions, focus helpers, adorner helpers, clipboard helpers and service abstractions.";

    /// <summary>
    /// Gets the extension groups shown in the demo.
    /// </summary>
    public ObservableCollection<XExtensionDemoGroup> ExtensionGroups { get; } =
    [
        new("DependencyObject / FrameworkElement", "Visual tree search, descendants, ancestors, template parts, delayed focus and hit testing."),
        new("Dispatcher / Task", "InvokeIfRequired, async dispatcher calls and safe fire-and-forget helper methods."),
        new("Collections / Trees", "EmptyIfNull, ReplaceWith, AddRange, Flatten, depth-first traversal and tree search."),
        new("Strings / Enums", "Search text normalization, case-insensitive comparison, DisplayName and Description lookup."),
        new("Markup Extensions", "EnumValues, AppThemeBinding, ThemeResource, ResourceOrDefault and icon markup extensions."),
        new("Services", "Focus navigation, clipboard abstraction, message boxes, notifications and weak event helpers."),
    ];

    /// <summary>
    /// Gets or sets the demo log text.
    /// </summary>
    [ObservableProperty]
    private string _demoLog = "Use the buttons to trigger small service-style actions.";

    /// <inheritdoc/>
    public override string XamlCode => """
<TextBlock
    Foreground="{via:AppThemeBinding Light=Black, Dark=White}"
    Text="Theme-aware value" />

<via:XComboBox ItemsSource="{via:EnumValues {x:Type viewmodels:OrderState}}" />

<via:XButton
    Command="{Binding CopySampleCommand}"
    Content="Copy sample" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
DependencyObject? parent = element.FindVisualParent<Grid>();
IEnumerable<TextBox> inputs = root.GetVisualDescendants<TextBox>();

await dispatcher.InvokeIfRequiredAsync(() => viewModel.Refresh());

IEnumerable<Node> flatNodes = nodes.Flatten(static node => node.Children);
string normalized = text.NormalizeSearchText();
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Copies a small demo text to the clipboard.
    /// </summary>
    [RelayCommand]
    private void CopySample()
    {
        Clipboard.SetText("VIA.WPF infrastructure demo");
        this.DemoLog = "Copied a sample text to the clipboard.";
    }

    /// <summary>
    /// Simulates a service notification.
    /// </summary>
    [RelayCommand]
    private void ShowNotification()
    {
        this.DemoLog = $"Notification service placeholder executed at {DateTime.Now:T}.";
    }
    #endregion
}
#endregion

#region ### Enum OrderState ###
/// <summary>
/// Represents a small demo enum used by the EnumValues markup extension sample.
/// </summary>
public enum OrderState
{
    /// <summary>
    /// The order is still open.
    /// </summary>
    Open,

    /// <summary>
    /// The order is currently being processed.
    /// </summary>
    Processing,

    /// <summary>
    /// The order has been completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The order has been cancelled.
    /// </summary>
    Cancelled,
}
#endregion

#region ### Record XExtensionDemoGroup ###
/// <summary>
/// Represents an extension category in the demo.
/// </summary>
/// <param name="Name">The category name.</param>
/// <param name="Description">The category description.</param>
public sealed record XExtensionDemoGroup(string Name, string Description);
#endregion
