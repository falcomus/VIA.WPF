// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTextBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.Input;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XTextBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XTextBox showcase page.
/// </summary>
public sealed class XTextBoxDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    private string searchQuery = "TextBox";
    private string commandText = "dotnet build VIA.WPF.Demo";
    private string resetFeedback = "No reset command executed yet.";
    private int resetCount;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XTextBoxDemoViewModel"/> class.
    /// </summary>
    public XTextBoxDemoViewModel()
    {
        this.ClearSearchQueryCommand = new RelayCommand(this.ClearSearchQuery);
        this.ClearCommandTextCommand = new RelayCommand(this.ClearCommandText);
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XTextBox";

    /// <inheritdoc />
    public override string Description => "Demonstrates the themed XTextBox with sizes, placeholder, header, description, leading and trailing icons, clear/reset behavior, command binding, multiline usage and field states.";

    /// <summary>
    /// Gets or sets the live search query used by the reset-command sample.
    /// </summary>
    public string SearchQuery
    {
        get => this.searchQuery;
        set => this.SetProperty(ref this.searchQuery, value);
    }

    /// <summary>
    /// Gets or sets the command text used by the binding sample.
    /// </summary>
    public string CommandText
    {
        get => this.commandText;
        set => this.SetProperty(ref this.commandText, value);
    }

    /// <summary>
    /// Gets the command used to clear the live search query.
    /// </summary>
    public IRelayCommand ClearSearchQueryCommand { get; }

    /// <summary>
    /// Gets the command used to clear the command text sample.
    /// </summary>
    public IRelayCommand ClearCommandTextCommand { get; }

    /// <summary>
    /// Gets or sets the number of executed reset commands.
    /// </summary>
    public int ResetCount
    {
        get => this.resetCount;
        set => this.SetProperty(ref this.resetCount, value);
    }

    /// <summary>
    /// Gets or sets the latest reset feedback text.
    /// </summary>
    public string ResetFeedback
    {
        get => this.resetFeedback;
        set => this.SetProperty(ref this.resetFeedback, value);
    }

    /// <inheritdoc />
    public override string XamlCode => """
<via:XTextBox
    Width="260"
    Header="Project name"
    HeaderFontSize="14"
    HeaderFontWeight="Bold"
    Placeholder="Enter project name"
    Text="VIA.WPF Pro" />

<via:XTextBox
    Width="300"
    Header="Package id"
    Description="Short helper descriptions stay aligned with the field."
    Placeholder="Company.Product.Package"
    Text="VIA.WPF.Controls" />

<via:XTextBox
    Width="260"
    Header="Compact filter"
    Placeholder="Filter rows"
    Size="Small"
    Text="active" />

<via:XTextBox
    Width="320"
    Header="Search with both icon slots"
    HasClearButton="True"
    LeadingIconSize="17"
    Placeholder="Search controls..."
    Text="TextBox"
    TrailingIconSize="16">
    <via:XTextBox.LeadingIcon>
        <via:XIcon Kind="Magnify" />
    </via:XTextBox.LeadingIcon>
    <via:XTextBox.TrailingIcon>
        <via:XIcon Kind="TuneVariant" />
    </via:XTextBox.TrailingIcon>
</via:XTextBox>

<via:XTextBox
    Width="320"
    Header="Template-driven icons"
    LeadingIcon="FolderOutline"
    LeadingIconSize="18"
    LeadingIconTemplate="{StaticResource TextBoxIconTemplate}"
    Placeholder="Repository path"
    Text="src/VIA.WPF.Controls"
    TrailingIcon="OpenInNew"
    TrailingIconSize="15"
    TrailingIconTemplate="{StaticResource TextBoxIconTemplate}" />

<via:XTextBox
    Width="320"
    Header="Live search"
    Placeholder="Type to update the view model"
    ResetCommand="{Binding ClearSearchQueryCommand}"
    ResetIcon="CloseCircleOutline"
    ShowResetButton="True"
    Text="{Binding SearchQuery, UpdateSourceTrigger=PropertyChanged}">
    <via:XTextBox.LeadingIcon>
        <via:XIcon Kind="Magnify" />
    </via:XTextBox.LeadingIcon>
</via:XTextBox>

<via:XTextBox
    Width="320"
    Header="Reset styling"
    Placeholder="Custom reset icon and size"
    ResetButtonForeground="{DynamicResource {x:Static via:XBrushKeys.DangerText}}"
    ResetButtonHeight="22"
    ResetButtonWidth="22"
    ResetIcon="Close"
    ResetIconTemplate="{StaticResource ResetIconTemplate}"
    ShowResetButton="True"
    Text="Clear me" />

<via:XTextBox
    Width="320"
    Header="Read-only"
    IsReadOnly="True"
    Text="Read-only value">
    <via:XTextBox.LeadingIcon>
        <via:XIcon Kind="LockOutline" />
    </via:XTextBox.LeadingIcon>
</via:XTextBox>

<via:XTextBox
    Width="320"
    Height="128"
    AcceptsReturn="True"
    Header="Multiline notes"
    Placeholder="Write release notes..."
    ShowResetButton="True"
    Text="XTextBox can be used for notes, descriptions and longer comments."
    TextWrapping="Wrap"
    VerticalContentAlignment="Top"
    VerticalScrollBarVisibility="Auto" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
using CommunityToolkit.Mvvm.Input;

public sealed class XTextBoxDemoViewModel : DemoPageViewModel
{
    private string searchQuery = "TextBox";
    private string commandText = "dotnet build VIA.WPF.Demo";
    private string resetFeedback = "No reset command executed yet.";
    private int resetCount;

    public XTextBoxDemoViewModel()
    {
        ClearSearchQueryCommand = new RelayCommand(ClearSearchQuery);
        ClearCommandTextCommand = new RelayCommand(ClearCommandText);
    }

    public string SearchQuery
    {
        get => searchQuery;
        set => SetProperty(ref searchQuery, value);
    }

    public string CommandText
    {
        get => commandText;
        set => SetProperty(ref commandText, value);
    }

    public int ResetCount
    {
        get => resetCount;
        set => SetProperty(ref resetCount, value);
    }

    public string ResetFeedback
    {
        get => resetFeedback;
        set => SetProperty(ref resetFeedback, value);
    }

    public IRelayCommand ClearSearchQueryCommand { get; }
    public IRelayCommand ClearCommandTextCommand { get; }

    private void ClearSearchQuery()
    {
        SearchQuery = string.Empty;
        ResetCount++;
        ResetFeedback = "Search query was cleared by ResetCommand.";
    }

    private void ClearCommandText()
    {
        CommandText = string.Empty;
        ResetCount++;
        ResetFeedback = "Command text was cleared by ResetCommand.";
    }
}
""";
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Clears the live search query and updates the reset feedback.
    /// </summary>
    private void ClearSearchQuery()
    {
        this.SearchQuery = string.Empty;
        this.ResetCount++;
        this.ResetFeedback = "Search query was cleared by ResetCommand.";
    }

    /// <summary>
    /// Clears the command text and updates the reset feedback.
    /// </summary>
    private void ClearCommandText()
    {
        this.CommandText = string.Empty;
        this.ResetCount++;
        this.ResetFeedback = "Command text was cleared by ResetCommand.";
    }
    #endregion
}
#endregion
