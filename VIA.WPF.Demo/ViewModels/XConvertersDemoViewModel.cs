// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XConvertersDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XConvertersDemoViewModel ###
/// <summary>
/// Represents the demo view model for converter infrastructure.
/// </summary>
public sealed partial class XConvertersDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "Converters";

    /// <inheritdoc/>
    public override string Description => "Demonstrates reusable WPF converters for visibility, boolean logic, equality, collections, GridLength, colors, brushes and tree indentation.";

    /// <summary>
    /// Gets or sets a value indicating whether the detail column is visible.
    /// </summary>
    [ObservableProperty]
    private bool _showDetails = true;

    /// <summary>
    /// Gets or sets the selected mode used by equality converters.
    /// </summary>
    [ObservableProperty]
    private string _selectedMode = "Edit";

    /// <summary>
    /// Gets the available demo modes.
    /// </summary>
    public ObservableCollection<string> Modes { get; } = ["View", "Edit", "Admin"];

    /// <summary>
    /// Gets the demo messages used by collection converters.
    /// </summary>
    public ObservableCollection<string> Messages { get; } = ["Missing article number", "Price requires review"];

    /// <summary>
    /// Gets the converter groups shown in the demo.
    /// </summary>
    public ObservableCollection<XConverterDemoGroup> ConverterGroups { get; } =
    [
        new("Boolean / Visibility", "BooleanToVisibility, InverseBooleanToVisibility, AnyTrue, AllTrue, BooleanAnd and BooleanOr."),
        new("Null / String / Collection", "NullToVisibility, StringNullOrWhiteSpace, CollectionEmpty, CollectionNotEmpty and CountToVisibility."),
        new("Equality / Enum", "EqualityToBoolean, EqualityToVisibility, EnumToBoolean and EnumToVisibility."),
        new("Layout / Numeric", "BooleanToGridLength, BooleanToThickness, BooleanToOpacity, Multiply and NumberGreaterThanToVisibility."),
        new("Brushes / Trees", "ColorToBrush, BrushOpacity, ObjectReferenceEquals and TreeLevelToThickness."),
    ];

    /// <inheritdoc/>
    public override string XamlCode => """
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*" />
        <ColumnDefinition Width="{Binding ShowDetails,
            Converter={StaticResource BooleanToGridLengthConverter},
            ConverterParameter='260|0'}" />
    </Grid.ColumnDefinitions>

    <TextBlock
        Text="Edit-only content"
        Visibility="{Binding SelectedMode,
            Converter={StaticResource EqualityToVisibilityConverter},
            ConverterParameter=Edit}" />

    <Border
        Grid.Column="1"
        Visibility="{Binding Messages,
            Converter={StaticResource CollectionNotEmptyToVisibilityConverter}}" />
</Grid>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
[ObservableProperty]
private bool _showDetails = true;

[ObservableProperty]
private string _selectedMode = "Edit";

public ObservableCollection<string> Messages { get; } = [];
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Adds a demo message.
    /// </summary>
    [RelayCommand]
    private void AddMessage()
    {
        this.Messages.Add($"Message {this.Messages.Count + 1}");
        this.OnPropertyChanged(nameof(this.Messages));
    }

    /// <summary>
    /// Clears all demo messages.
    /// </summary>
    [RelayCommand]
    private void ClearMessages()
    {
        this.Messages.Clear();
        this.OnPropertyChanged(nameof(this.Messages));
    }
    #endregion
}
#endregion

#region ### Record XConverterDemoGroup ###
/// <summary>
/// Represents a converter group shown in the demo.
/// </summary>
/// <param name="Name">The group name.</param>
/// <param name="Description">The group description.</param>
public sealed record XConverterDemoGroup(string Name, string Description);
#endregion
