// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCheckerBoardDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XCheckerBoardDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XCheckerBoard showcase page.
/// </summary>
public sealed class XCheckerBoardDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <inheritdoc/>
    public override string Title => "XCheckerBoard";

    /// <inheritdoc/>
    public override string Description => "Shows checkerboard transparency previews and inherited XBorder features such as border, corner radius, variant, appearance and elevation.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XCheckerBoard
    Width="260"
    Height="135"
    Padding="16"
    BorderBrush="{DynamicResource {x:Static via:XBrushKeys.Border}}"
    BorderThickness="1"
    CheckerSize="10"
    CornerRadius="12"
    Elevation="Low">
    <Border
        Width="110"
        Height="60"
        Background="#802D6CDF"
        CornerRadius="10" />
</via:XCheckerBoard>

<via:XCheckerBoard
    CheckerLightBrush="#F8FAFC"
    CheckerDarkBrush="#DDE7F3"
    CheckerSize="6" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XCheckerBoard checkerBoard = new()
{
    Width = 260d,
    Height = 135d,
    CheckerSize = 10d,
    CornerRadius = new CornerRadius(12d),
    BorderThickness = new Thickness(1d),
    Padding = new Thickness(16d),
    Elevation = XElevation.Low,
};

checkerBoard.CheckerLightBrush = Brushes.White;
checkerBoard.CheckerDarkBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224));
""";
    #endregion
}
#endregion
