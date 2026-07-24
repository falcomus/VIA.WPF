// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XContextMenuDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VIA.WPF.Controls;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XContextMenuDemoViewModel ###
/// <summary>
/// Represents the view model for the XContextMenu showcase page.
/// </summary>
public sealed partial class XContextMenuDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    [ObservableProperty]
    private string _lastAction = "Select a document and open one of its menus.";

    [ObservableProperty]
    private XContextMenuDemoDocument? _selectedDocument;

    [ObservableProperty]
    private bool _showPreview = true;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XContextMenuDemoViewModel"/> class.
    /// </summary>
    public XContextMenuDemoViewModel()
    {
        this.SelectedDocument = this.Documents[0];
    }
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XContextMenu";

    /// <inheritdoc />
    public override string Description => "Demonstrates themed context menus, XMoreButton integration, semantic variants, checkable items, submenus and placement-target bindings.";

    /// <summary>
    /// Gets the sample documents.
    /// </summary>
    public ObservableCollection<XContextMenuDemoDocument> Documents { get; } =
    [
        new("Product brief", "Updated 12 minutes ago", "Draft", XControlVariant.Warning),
        new("Launch checklist", "18 of 24 tasks completed", "75%", XControlVariant.Primary),
        new("Research notes", "Shared with the design team", "Shared", XControlVariant.Info),
        new("Release summary", "Ready for stakeholder review", "Ready", XControlVariant.Success)
    ];

    /// <inheritdoc />
    public override string XamlCode => """
<via:XListBox
    ItemsSource="{Binding Documents}"
    SelectedItem="{Binding SelectedDocument}">
    <via:XListBox.ContextMenu>
        <via:XContextMenu>
            <via:XMenuItem
                Command="{Binding TargetDataContext.OpenDocumentCommand,
                    RelativeSource={RelativeSource AncestorType={x:Type via:XContextMenu}}}"
                CommandParameter="{Binding TargetItem,
                    RelativeSource={RelativeSource AncestorType={x:Type via:XContextMenu}}}"
                Header="Open"
                Icon="{via:MaterialIcon Kind=FolderOpenOutline}"
                Variant="Primary" />

            <via:XMenuItem
                Header="Show preview"
                IsCheckable="True"
                IsChecked="{Binding TargetDataContext.ShowPreview,
                    Mode=TwoWay,
                    RelativeSource={RelativeSource AncestorType={x:Type via:XContextMenu}}}"
                StaysOpenOnClick="True" />

            <Separator Style="{StaticResource XMenuSeparatorStyle}" />

            <via:XMenuItem
                Command="{Binding TargetDataContext.DeleteDocumentCommand,
                    RelativeSource={RelativeSource AncestorType={x:Type via:XContextMenu}}}"
                CommandParameter="{Binding TargetItem,
                    RelativeSource={RelativeSource AncestorType={x:Type via:XContextMenu}}}"
                Header="Delete"
                Icon="{via:MaterialIcon Kind=DeleteOutline}"
                Variant="Danger" />
        </via:XContextMenu>
    </via:XListBox.ContextMenu>
</via:XListBox>

<via:XMoreButton Tag="{Binding SelectedDocument}">
    <via:XMoreButton.Menu>
        <via:XContextMenu>
            <via:XMenuItem
                Command="{Binding TargetDataContext.OpenDocumentCommand,
                    RelativeSource={RelativeSource AncestorType={x:Type via:XContextMenu}}}"
                CommandParameter="{Binding TargetTag,
                    RelativeSource={RelativeSource AncestorType={x:Type via:XContextMenu}}}"
                Header="Open selected document" />
        </via:XContextMenu>
    </via:XMoreButton.Menu>
</via:XMoreButton>
""";

    /// <inheritdoc />
    public override string CSharpCode => """
[RelayCommand]
private void OpenDocument(XContextMenuDemoDocument? document)
{
    this.LastAction = document is null
        ? "No document selected."
        : $"Opened '{document.Title}'.";
}

[RelayCommand]
private void ReportTarget(object? target)
{
    this.LastAction = $"Placement target tag: {target ?? "(none)"}";
}
""";
    #endregion

    #region ### Commands ###
    /// <summary>
    /// Creates a sample document.
    /// </summary>
    [RelayCommand]
    private void CreateDocument()
    {
        XContextMenuDemoDocument document = new(
            $"Untitled document {this.Documents.Count + 1}",
            "Created just now",
            "New",
            XControlVariant.Accent);

        this.Documents.Add(document);
        this.SelectedDocument = document;
        this.LastAction = $"Created '{document.Title}'.";
    }

    /// <summary>
    /// Opens the selected document.
    /// </summary>
    /// <param name="document">The selected document.</param>
    [RelayCommand]
    private void OpenDocument(XContextMenuDemoDocument? document)
    {
        this.LastAction = document is null
            ? "No document selected."
            : $"Opened '{document.Title}'.";
    }

    /// <summary>
    /// Renames the selected document.
    /// </summary>
    /// <param name="document">The selected document.</param>
    [RelayCommand]
    private void RenameDocument(XContextMenuDemoDocument? document)
    {
        this.LastAction = document is null
            ? "No document selected."
            : $"Rename requested for '{document.Title}'.";
    }

    /// <summary>
    /// Duplicates the selected document.
    /// </summary>
    /// <param name="document">The selected document.</param>
    [RelayCommand]
    private void DuplicateDocument(XContextMenuDemoDocument? document)
    {
        if (document is null)
        {
            this.LastAction = "No document selected.";
            return;
        }

        XContextMenuDemoDocument copy = document with
        {
            Title = $"{document.Title} copy",
            SubTitle = "Duplicated just now",
            BadgeContent = "Copy",
            BadgeVariant = XControlVariant.Accent
        };

        this.Documents.Add(copy);
        this.SelectedDocument = copy;
        this.LastAction = $"Duplicated '{document.Title}'.";
    }

    /// <summary>
    /// Deletes the selected document.
    /// </summary>
    /// <param name="document">The selected document.</param>
    [RelayCommand]
    private void DeleteDocument(XContextMenuDemoDocument? document)
    {
        if (document is null || !this.Documents.Remove(document))
        {
            this.LastAction = "No document selected.";
            return;
        }

        this.SelectedDocument = this.Documents.FirstOrDefault();
        this.LastAction = $"Deleted '{document.Title}'.";
    }

    /// <summary>
    /// Exports the selected document in the requested format.
    /// </summary>
    /// <param name="format">The export format.</param>
    [RelayCommand]
    private void ExportDocument(string? format)
    {
        this.LastAction = this.SelectedDocument is null
            ? "No document selected."
            : $"Exported '{this.SelectedDocument.Title}' as {format ?? "file"}.";
    }

    /// <summary>
    /// Reports information exposed by the placement target.
    /// </summary>
    /// <param name="target">The placement target tag.</param>
    [RelayCommand]
    private void ReportTarget(object? target)
    {
        this.LastAction = $"Placement target tag: {target ?? "(none)"}.";
    }
    #endregion
}
#endregion

#region ### Record XContextMenuDemoDocument ###
/// <summary>
/// Represents a sample document used by the context-menu demo.
/// </summary>
/// <param name="Title">The document title.</param>
/// <param name="SubTitle">The secondary text.</param>
/// <param name="BadgeContent">The badge content.</param>
/// <param name="BadgeVariant">The badge variant.</param>
public sealed record XContextMenuDemoDocument(
    string Title,
    string SubTitle,
    string BadgeContent,
    XControlVariant BadgeVariant)
{
    /// <summary>
    /// Gets a value indicating whether the badge is shown.
    /// </summary>
    public bool ShowBadge => true;
}
#endregion