// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XIconButtonDemoViewModel.cs.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XIconButtonDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XIconButton showcase page.
/// </summary>
public sealed class XIconButtonDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XIconButton";

    /// <inheritdoc/>
    public override string Description => "Demonstrates compact icon-only commands with variants, appearances, sizes, loading and disabled states.";


    /// <inheritdoc/>
    public override string XamlCode => """
<via:XWrapPanel HorizontalSpacing="10" VerticalSpacing="10">
    <via:XIconButton Icon="{via:MaterialIcon Kind=Pencil}" ToolTip="Edit" />
    <via:XIconButton Icon="{via:MaterialIcon Kind=Refresh}" Variant="Primary" ToolTip="Refresh" />
    <via:XIconButton Icon="{via:MaterialIcon Kind=DeleteOutline}" Variant="Danger" ToolTip="Delete" />
</via:XWrapPanel>

<via:XWrapPanel HorizontalSpacing="10" VerticalSpacing="10">
    <via:XIconButton Icon="{via:MaterialIcon Kind=Plus}" Size="Small" ToolTip="Small" />
    <via:XIconButton Icon="{via:MaterialIcon Kind=Plus}" Size="Medium" ToolTip="Medium" />
    <via:XIconButton Icon="{via:MaterialIcon Kind=Plus}" Size="Large" ToolTip="Large" />
</via:XWrapPanel>

<via:XIconButton
    Appearance="Outline"
    Icon="{via:MaterialIcon Kind=OpenInNew}"
    Variant="Info"
    ToolTip="Open" />

<via:XIconButton
    Icon="{via:MaterialIcon Kind=Refresh}"
    IsLoading="True"
    LoadingContent="Loading"
    ToolTip="Loading" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XIconButton refreshButton = new()
{
    Icon = new XMaterialIcon { Kind = PackIconMaterialKind.Refresh },
    Variant = XControlVariant.Primary,
    Size = XControlSize.Medium,
    ToolTip = "Refresh",
};
""";
    #endregion
}
#endregion
