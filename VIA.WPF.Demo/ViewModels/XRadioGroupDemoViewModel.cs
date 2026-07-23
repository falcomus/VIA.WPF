// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRadioGroupDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XRadioGroupDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XRadioGroup showcase page.
/// </summary>
public sealed class XRadioGroupDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private RadioGroupOption? _selectedLayoutOption;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XRadioGroupDemoViewModel"/> class.
    /// </summary>
    public XRadioGroupDemoViewModel()
    {
        LayoutOptions.Add(new RadioGroupOption("Grid", "Best for dense data and column filtering."));
        LayoutOptions.Add(new RadioGroupOption("Tree", "Best for hierarchical structures."));
        LayoutOptions.Add(new RadioGroupOption("Cards", "Best for visual overview pages."));

        SelectedLayoutOption = LayoutOptions[0];
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XRadioGroup";

    /// <inheritdoc/>
    public override string Description => "Demonstrates titled single-selection groups with descriptions, variants, shared sizing and MVVM-backed selected item.";

    /// <summary>
    /// Gets the selectable layout options used by the MVVM sample.
    /// </summary>
    public ObservableCollection<RadioGroupOption> LayoutOptions { get; } = new();

    /// <summary>
    /// Gets or sets the selected layout option used by the MVVM sample.
    /// </summary>
    public RadioGroupOption? SelectedLayoutOption
    {
        get => _selectedLayoutOption;
        set
        {
            if (SetProperty(ref _selectedLayoutOption, value))
            {
                OnPropertyChanged(nameof(LayoutSummary));
            }
        }
    }

    /// <summary>
    /// Gets a short summary of the current MVVM selection.
    /// </summary>
    public string LayoutSummary => SelectedLayoutOption is null
        ? "No layout mode is selected."
        : $"Current layout mode: {SelectedLayoutOption.Name}. {SelectedLayoutOption.Description}";

    /// <inheritdoc/>
    public override string XamlCode => """
<!-- Descriptive single-selection group -->
<via:XRadioGroup Header="Default view" ItemSpacing="8" ItemsPadding="12">
    <via:XRadioGroupItem Content="Grid" Description="Best for dense data and column filtering." IsChecked="True" Variant="Primary" />
    <via:XRadioGroupItem Content="Tree" Description="Best for hierarchical structures." Variant="Info" />
    <via:XRadioGroupItem Content="Cards" Description="Best for visual overview pages." Variant="Accent" />
</via:XRadioGroup>

<!-- Compact horizontal selection -->
<via:XRadioGroup Orientation="Horizontal" ShowTitle="False" ItemsPadding="12,5" ItemSpacing="24">
    <via:XRadioGroupItem Content="Small" />
    <via:XRadioGroupItem Content="Medium" IsChecked="True" />
    <via:XRadioGroupItem Content="Large" />
</via:XRadioGroup>

<!-- MVVM selected item -->
<via:XRadioGroup
    Title="Bound layout mode"
    ItemsSource="{Binding LayoutOptions}"
    SelectedItem="{Binding SelectedLayoutOption, Mode=TwoWay}"
    ItemSpacing="8">
    <via:XRadioGroup.ItemTemplate>
        <DataTemplate>
            <StackPanel>
                <TextBlock FontWeight="SemiBold" Text="{Binding Name}" />
                <TextBlock Text="{Binding Description}" TextWrapping="Wrap" />
            </StackPanel>
        </DataTemplate>
    </via:XRadioGroup.ItemTemplate>
</via:XRadioGroup>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
public ObservableCollection<RadioGroupOption> LayoutOptions { get; } = new();

private RadioGroupOption? _selectedLayoutOption;
public RadioGroupOption? SelectedLayoutOption
{
    get => _selectedLayoutOption;
    set
    {
        if (SetProperty(ref _selectedLayoutOption, value))
        {
            OnPropertyChanged(nameof(LayoutSummary));
        }
    }
}

public string LayoutSummary => SelectedLayoutOption is null
    ? "No layout mode is selected."
    : $"Current layout mode: {SelectedLayoutOption.Name}.";

public sealed record RadioGroupOption(string Name, string Description);
""";
    #endregion
}
#endregion

#region ### Record RadioGroupOption ###
/// <summary>
/// Represents one option used by the XRadioGroup MVVM sample.
/// </summary>
/// <param name="Name">The display name.</param>
/// <param name="Description">The secondary description.</param>
public sealed record RadioGroupOption(string Name, string Description);
#endregion
