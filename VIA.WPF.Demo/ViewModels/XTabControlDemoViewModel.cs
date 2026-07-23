// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTabControlDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XTabControlDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XTabControl showcase page.
/// </summary>
public sealed partial class XTabControlDemoViewModel : DemoPageViewModel
{
    #region ### Private Fields ###
    private int _nextRuntimeTabNumber = 3;

    [ObservableProperty]
    private string _lastTabAction = "No tab command executed.";

    [ObservableProperty]
    private XTabControlDemoDocument? _selectedRuntimeTab;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XTabControlDemoViewModel"/> class.
    /// </summary>
    public XTabControlDemoViewModel()
    {
        this.RuntimeTabs.Add(this.CreateRuntimeTab("General", "The first runtime tab is created by the view model and can be selected like a regular tab."));
        this.RuntimeTabs.Add(this.CreateRuntimeTab("Closable", "This tab demonstrates XTabItem.CloseCommand through an ItemsSource-driven tab collection."));
        this.SelectedRuntimeTab = this.RuntimeTabs[0];
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XTabControl";

    /// <inheritdoc/>
    public override string Description => "Demonstrates VIA.WPF tab header appearances, icon tabs, add buttons, close buttons and optional border chrome.";

    /// <summary>
    /// Gets the runtime tabs used by the add/close demo.
    /// </summary>
    public ObservableCollection<XTabControlDemoDocument> RuntimeTabs { get; } = [];

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XTabControl
    ItemsSource="{Binding RuntimeTabs}"
    SelectedItem="{Binding SelectedRuntimeTab}"
    ItemHeaderTemplate="{StaticResource RuntimeTabHeaderTemplate}"
    HeaderAppearance="Underlined"
    ShowAddButton="True"
    AddButtonCommand="{Binding AddTabCommand}"
    AddButtonContent="{via:MaterialIcon Kind=Plus}"
    ShowTabCloseButton="True">
    <via:XTabControl.ItemContainerStyle>
        <Style TargetType="{x:Type via:XTabItem}">
            <Setter Property="CanClose" Value="True" />
            <Setter Property="CloseCommand" Value="{Binding CloseCommand}" />
            <Setter Property="ContentTemplate" Value="{StaticResource RuntimeTabContentTemplate}" />
        </Style>
    </via:XTabControl.ItemContainerStyle>
</via:XTabControl>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
public ObservableCollection<XTabControlDemoDocument> RuntimeTabs { get; } = [];

[RelayCommand]
private void AddTab()
{
    XTabControlDemoDocument document = CreateRuntimeTab("New tab", "Added at runtime.");
    RuntimeTabs.Add(document);
    SelectedRuntimeTab = document;
}

private void CloseRuntimeTab(XTabControlDemoDocument document)
{
    RuntimeTabs.Remove(document);
}
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Adds a new runtime tab and selects it.
    /// </summary>
    [RelayCommand]
    private void AddTab()
    {
        string title = $"Draft {this._nextRuntimeTabNumber++}";
        XTabControlDemoDocument document = this.CreateRuntimeTab(
            title,
            "This tab was created by AddButtonCommand and selected immediately after insertion.");

        this.RuntimeTabs.Add(document);
        this.SelectedRuntimeTab = document;
        this.LastTabAction = $"Added and selected '{title}'.";
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Creates a runtime tab document.
    /// </summary>
    /// <param name="title">The document title.</param>
    /// <param name="description">The document description.</param>
    /// <returns>The created document.</returns>
    private XTabControlDemoDocument CreateRuntimeTab(string title, string description)
    {
        return new XTabControlDemoDocument(title, description, this.CloseRuntimeTab);
    }

    /// <summary>
    /// Closes the specified runtime tab.
    /// </summary>
    /// <param name="document">The document to close.</param>
    private void CloseRuntimeTab(XTabControlDemoDocument document)
    {
        if (this.RuntimeTabs.Count <= 1)
        {
            this.LastTabAction = "The demo keeps at least one runtime tab open.";
            return;
        }

        int oldIndex = this.RuntimeTabs.IndexOf(document);
        this.RuntimeTabs.Remove(document);

        if (ReferenceEquals(this.SelectedRuntimeTab, document))
        {
            int newIndex = Math.Clamp(oldIndex, 0, this.RuntimeTabs.Count - 1);
            this.SelectedRuntimeTab = this.RuntimeTabs[newIndex];
        }

        this.LastTabAction = $"Closed '{document.Title}'.";
    }
    #endregion
}
#endregion

#region ### Class XTabControlDemoDocument ###
/// <summary>
/// Represents a runtime tab document for the XTabControl demo.
/// </summary>
public sealed class XTabControlDemoDocument
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XTabControlDemoDocument"/> class.
    /// </summary>
    /// <param name="title">The document title.</param>
    /// <param name="description">The document description.</param>
    /// <param name="closeAction">The action used to close this document.</param>
    public XTabControlDemoDocument(string title, string description, Action<XTabControlDemoDocument> closeAction)
    {
        this.Title = title;
        this.Description = description;
        this.CloseCommand = new RelayCommand(() => closeAction(this));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the document title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the document description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the command used by the tab close button.
    /// </summary>
    public IRelayCommand CloseCommand { get; }
    #endregion
}
#endregion
