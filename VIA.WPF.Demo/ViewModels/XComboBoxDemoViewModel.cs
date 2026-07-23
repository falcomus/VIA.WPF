// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XComboBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XComboBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XComboBox showcase page.
/// </summary>
public sealed class XComboBoxDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XComboBox";

    /// <inheritdoc/>
    public override string Description => "Demonstrates the themed base combo box with headers, descriptions, editable mode, sizes and states.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XComboBox
    Width="300"
    Header="Status"
    Description="Header and description are part of the control template."
    SelectedIndex="1"
    ShowResetButton="True">
    <via:XComboBoxItem Content="Draft" />
    <via:XComboBoxItem Content="In review" />
    <via:XComboBoxItem Content="Published" />
</via:XComboBox>

<via:XComboBox
    Width="300"
    Header="Owner"
    IsEditable="True"
    SelectedIndex="0">
    <via:XComboBoxItem Content="Anna Schneider" />
    <via:XComboBoxItem Content="Claus Meyer" />
</via:XComboBox>

<via:XComboBox Width="170" Header="Small" Size="Small" />
<via:XComboBox Width="210" Header="Large" Size="Large" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XComboBox comboBox = new()
{
    Width = 300d,
    Header = "Status",
    Description = "Header and description are part of the control template.",
    Size = XControlSize.Medium,
    SelectedIndex = 1,
    ShowResetButton = true,
};

comboBox.Items.Add(new XComboBoxItem { Content = "Draft" });
comboBox.Items.Add(new XComboBoxItem { Content = "In review" });
comboBox.Items.Add(new XComboBoxItem { Content = "Published" });
""";
    #endregion
}
#endregion
