// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToggleDropDownDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XToggleDropDownDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XToggleDropDown showcase page.
/// </summary>
public sealed class XToggleDropDownDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XToggleDropDown";

    /// <inheritdoc/>
    public override string Description => "Demonstrates toggle drop-down menus with checked icons, checked appearance, size, placement and custom popup content.";


    /// <inheritdoc/>
    public override string XamlCode => """
<via:XToggleDropDown
    Content="Export"
    DropDownPlacement="Bottom"
    UncheckedIcon="{via:MaterialIcon Kind=ExportVariant}"
    CheckedIcon="{via:MaterialIcon Kind=ChevronUp}"
    Variant="Primary">
    <via:XToggleDropDown.DropDownContent>
        <via:XStackPanel MinWidth="220" Margin="12" Spacing="8">
            <via:XButton Content="Export as PDF" Icon="{via:MaterialIcon Kind=FilePdfBox}" Appearance="Ghost" />
            <via:XButton Content="Export as Excel" Icon="{via:MaterialIcon Kind=FileExcelOutline}" Appearance="Ghost" />
        </via:XStackPanel>
    </via:XToggleDropDown.DropDownContent>
</via:XToggleDropDown>
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XToggleDropDown dropDown = new()
{
    Content = "Export",
    Variant = XControlVariant.Primary,
    Appearance = XControlAppearance.Solid,
    MaxDropDownHeight = 320d,
};
""";
    #endregion
}
#endregion
