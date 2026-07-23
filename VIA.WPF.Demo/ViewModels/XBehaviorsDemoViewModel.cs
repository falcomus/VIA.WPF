// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBehaviorsDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XBehaviorsDemoViewModel ###
/// <summary>
/// Represents the demo view model for attached behaviors.
/// </summary>
public sealed partial class XBehaviorsDemoViewModel : DemoPageViewModel
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XBehaviorsDemoViewModel"/> class.
    /// </summary>
    public XBehaviorsDemoViewModel()
    {
        this.SelectedAction = this.Actions[2];
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "Behaviors";

    /// <inheritdoc/>
    public override string Description => "Demonstrates attached behaviors for focus, text selection, keyboard commands, double-click commands, drag/drop, auto-scroll and window dragging.";

    /// <summary>
    /// Gets or sets the sample input text.
    /// </summary>
    [ObservableProperty]
    private string _sampleText = "Click into this TextBox: the full text is selected automatically.";

    /// <summary>
    /// Gets or sets the command log text.
    /// </summary>
    [ObservableProperty]
    private string _commandLog = "Press Enter or Escape in the command field.";

    /// <summary>
    /// Gets the selectable actions.
    /// </summary>
    public ObservableCollection<string> Actions { get; } =
    [
        "Create article",
        "Edit category",
        "Open stock movement",
        "Print label",
        "Synchronize inventory",
        "Archive document",
        "Export CSV",
        "Send notification",
    ];

    /// <summary>
    /// Gets or sets the selected action.
    /// </summary>
    [ObservableProperty]
    private string? _selectedAction;

    /// <inheritdoc/>
    public override string XamlCode => """
<TextBox
    behaviors:SelectAllTextBoxBehavior.IsEnabled="True"
    behaviors:TextBoxCommitOnEnterBehavior.IsEnabled="True"
    behaviors:TextBoxCommitOnEnterBehavior.MoveFocusAfterCommit="True"
    Text="{Binding SampleText, Mode=TwoWay}" />

<TextBox
    behaviors:KeyCommandBehavior.EnterCommand="{Binding ConfirmCommand}"
    behaviors:KeyCommandBehavior.EscapeCommand="{Binding CancelCommand}" />

<ListBox
    behaviors:MouseDoubleClickCommandBehavior.Command="{Binding OpenActionCommand}"
    ItemsSource="{Binding Actions}" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
[RelayCommand]
private void Confirm()
{
    this.CommandLog = "Enter command executed.";
}

[RelayCommand]
private void Cancel()
{
    this.CommandLog = "Escape command executed.";
}

[RelayCommand]
private void OpenAction(object? action)
{
    this.CommandLog = $"Double-click: {action}";
}
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Handles the Enter demo command.
    /// </summary>
    [RelayCommand]
    private void Confirm()
    {
        this.CommandLog = "Enter command executed.";
    }

    /// <summary>
    /// Handles the Escape demo command.
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        this.CommandLog = "Escape command executed.";
    }

    /// <summary>
    /// Handles the double-click demo command.
    /// </summary>
    /// <param name="action">The selected action.</param>
    [RelayCommand]
    private void OpenAction(object? action)
    {
        this.CommandLog = action is null
            ? "Double-click command executed."
            : $"Double-click command executed for '{action}'.";
    }
    #endregion
}
#endregion
