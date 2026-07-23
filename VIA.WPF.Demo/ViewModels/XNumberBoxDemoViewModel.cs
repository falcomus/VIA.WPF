// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNumberBoxDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XNumberBoxDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XNumberBox showcase page.
/// </summary>
public sealed class XNumberBoxDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    private double? quantity = 24;
    private double? price = 149.9;
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XNumberBox";

    /// <inheritdoc />
    public override string Description => "Demonstrates the themed XNumberBox with sizes, numeric value binding, min/max range, step size, format strings, spinner buttons, leading and trailing icons and read-only states.";

    /// <summary>
    /// Gets or sets the live quantity sample value.
    /// </summary>
    public double? Quantity
    {
        get => this.quantity;
        set => this.SetProperty(ref this.quantity, value);
    }

    /// <summary>
    /// Gets or sets the live price sample value.
    /// </summary>
    public double? Price
    {
        get => this.price;
        set => this.SetProperty(ref this.price, value);
    }

    /// <inheritdoc />
    public override string XamlCode => """
<via:XNumberBox
    Width="260"
    Header="Quantity"
    Minimum="0"
    Maximum="100"
    Step="1"
    Value="24" />

<via:XNumberBox
    Width="220"
    Header="Small"
    Size="Small"
    Value="8" />

<via:XNumberBox
    Width="320"
    Header="Price"
    Description="Formats can be used for currency, percentages or fixed precision."
    FormatString="N2"
    LeadingIcon="{via:MaterialIcon Kind=Cash}"
    Minimum="0"
    Step="0.5"
    Value="149.9" />

<via:XNumberBox
    Width="320"
    Header="Percent"
    FormatString="N1"
    Maximum="100"
    Minimum="0"
    Step="0.25"
    TrailingIcon="{via:MaterialIcon Kind=PercentOutline}"
    Value="42.5" />

<via:XNumberBox
    Width="320"
    Header="No spinner buttons"
    ShowSpinnerButtons="False"
    Value="12" />

<via:XNumberBox
    Width="320"
    Header="Bound quantity"
    Value="{Binding Quantity, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
public sealed class XNumberBoxDemoViewModel : DemoPageViewModel
{
    private double? quantity = 24;
    private double? price = 149.9;

    public double? Quantity
    {
        get => quantity;
        set => SetProperty(ref quantity, value);
    }

    public double? Price
    {
        get => price;
        set => SetProperty(ref price, value);
    }
}

// Useful properties shown by the page:
//
// Value / Text
// Minimum / Maximum
// Step
// FormatString
// Size
// CornerRadius
// Placeholder / Header / Description
// LeadingIcon / LeadingIconSize
// TrailingIcon / TrailingIconSize
// ShowSpinnerButtons
// IsReadOnly
""";
    #endregion
}
#endregion
