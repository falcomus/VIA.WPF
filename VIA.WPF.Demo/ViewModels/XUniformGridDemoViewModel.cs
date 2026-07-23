// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XUniformGridDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XUniformGridDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XUniformGrid</c>.
/// </summary>
public sealed class XUniformGridDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XUniformGrid";

    /// <inheritdoc/>
    public override string Description => "Demonstrates uniform cell layouts with automatic dimensions, fixed rows or columns and independent spacing.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XUniformGrid Spacing="10">
    <via:XButton Content="One" />
    <via:XButton Content="Two" />
    <via:XButton Content="Three" />
    <via:XButton Content="Four" />
</via:XUniformGrid>

<via:XUniformGrid Columns="4" ColumnSpacing="10" RowSpacing="10">
    <via:XButton Content="One" />
    <via:XButton Content="Two" />
    <via:XButton Content="Three" />
    <via:XButton Content="Four" />
</via:XUniformGrid>

<via:XUniformGrid Rows="2" Columns="3" RowSpacing="16" ColumnSpacing="8" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
var automaticGrid = new XUniformGrid
{
    Spacing = 10,
};

var fourColumnGrid = new XUniformGrid
{
    Columns = 4,
    RowSpacing = 10,
    ColumnSpacing = 10,
};

var explicitGrid = new XUniformGrid
{
    Rows = 2,
    Columns = 3,
    RowSpacing = 16,
    ColumnSpacing = 8,
};
""";
    #endregion
}
#endregion
