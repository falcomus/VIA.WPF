// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSearchBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XSearchBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XSearchBox showcase page.
/// </summary>
public sealed class XSearchBoxDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    private string searchText = "button";
    private string commandSearchText = "navigation";
    private string lastResetInfo = "No reset command executed yet.";
    private int resetCount;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XSearchBoxDemoViewModel"/> class.
    /// </summary>
    public XSearchBoxDemoViewModel()
    {
        this.ResetSearchCommand = new RelayCommand(this.ResetCommandSearch);
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XSearchBox";

    /// <inheritdoc />
    public override string Description => "Demonstrates the compact XSearchBox with placeholder, search icon, reset icon, reset command, clear-button compatibility, read-only state and live two-way binding.";

    /// <summary>
    /// Gets or sets the live search text.
    /// </summary>
    public string SearchText
    {
        get => this.searchText;
        set => this.SetProperty(ref this.searchText, value);
    }

    /// <summary>
    /// Gets or sets the command search text.
    /// </summary>
    public string CommandSearchText
    {
        get => this.commandSearchText;
        set => this.SetProperty(ref this.commandSearchText, value);
    }

    /// <summary>
    /// Gets or sets the latest reset feedback text.
    /// </summary>
    public string LastResetInfo
    {
        get => this.lastResetInfo;
        set => this.SetProperty(ref this.lastResetInfo, value);
    }

    /// <summary>
    /// Gets or sets the number of executed reset commands.
    /// </summary>
    public int ResetCount
    {
        get => this.resetCount;
        set => this.SetProperty(ref this.resetCount, value);
    }

    /// <summary>
    /// Gets the command used by the reset-command sample.
    /// </summary>
    public IRelayCommand ResetSearchCommand { get; }

    /// <inheritdoc />
    public override string XamlCode => """
<via:XSearchBox
    Width="260"
    Placeholder="Search controls..."
    Text="{Binding SearchText, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<via:XSearchBox
    Width="260"
    Placeholder="Custom search icon"
    SearchIcon="{via:MaterialIcon Kind=Magnify}"
    SearchIconSize="16"
    Text="theme" />

<via:XSearchBox
    Width="280"
    Placeholder="Custom reset icon"
    ResetButtonHeight="24"
    ResetButtonWidth="24"
    ResetIcon="{via:MaterialIcon Kind=CloseCircleOutline}"
    Text="clear me" />

<via:XSearchBox
    Width="280"
    Placeholder="Reset command"
    ResetSearchCommand="{Binding ResetSearchCommand}"
    Text="{Binding CommandSearchText, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<via:XSearchBox
    Width="260"
    HasClearButton="False"
    Placeholder="No reset button"
    Text="locked filter" />

<via:XSearchBox
    Width="260"
    IsReadOnly="True"
    Placeholder="Read-only"
    Text="read-only search" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
using CommunityToolkit.Mvvm.Input;

public sealed class XSearchBoxDemoViewModel : DemoPageViewModel
{
    private string searchText = "button";
    private string commandSearchText = "navigation";
    private string lastResetInfo = "No reset command executed yet.";
    private int resetCount;

    public XSearchBoxDemoViewModel()
    {
        ResetSearchCommand = new RelayCommand(ResetCommandSearch);
    }

    public string SearchText
    {
        get => searchText;
        set => SetProperty(ref searchText, value);
    }

    public string CommandSearchText
    {
        get => commandSearchText;
        set => SetProperty(ref commandSearchText, value);
    }

    public string LastResetInfo
    {
        get => lastResetInfo;
        set => SetProperty(ref lastResetInfo, value);
    }

    public int ResetCount
    {
        get => resetCount;
        set => SetProperty(ref resetCount, value);
    }

    public IRelayCommand ResetSearchCommand { get; }

    private void ResetCommandSearch()
    {
        CommandSearchText = string.Empty;
        ResetCount++;
        LastResetInfo = "Search reset by ResetSearchCommand.";
    }
}
""";
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Resets the command search sample and updates feedback.
    /// </summary>
    private void ResetCommandSearch()
    {
        this.CommandSearchText = string.Empty;
        this.ResetCount++;
        this.LastResetInfo = "Search reset by ResetSearchCommand.";
    }
    #endregion
}
#endregion
