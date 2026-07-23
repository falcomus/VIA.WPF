// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDatePickerDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XDatePickerDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XDatePicker showcase page.
/// </summary>
public sealed class XDatePickerDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    private DateTime? invoiceDate = new(2026, 5, 4);
    private DateTime? releaseDate = new(2026, 6, 18);
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XDatePicker";

    /// <inheritdoc />
    public override string Description => "Demonstrates the themed XDatePicker with sizes, selected date binding, date ranges, custom formatting, reset button, first day of week, leading and trailing icons and read-only states.";

    /// <summary>
    /// Gets or sets the invoice date sample value.
    /// </summary>
    public DateTime? InvoiceDate
    {
        get => this.invoiceDate;
        set => this.SetProperty(ref this.invoiceDate, value);
    }

    /// <summary>
    /// Gets or sets the release date sample value.
    /// </summary>
    public DateTime? ReleaseDate
    {
        get => this.releaseDate;
        set => this.SetProperty(ref this.releaseDate, value);
    }

    /// <inheritdoc />
    public override string XamlCode => """
<via:XDatePicker
    Width="280"
    Header="Invoice date"
    Placeholder="Select date"
    SelectedDate="{Binding InvoiceDate, Mode=TwoWay}" />

<via:XDatePicker
    Width="240"
    Header="Small"
    Placeholder="Compact date"
    Size="Small"
    SelectedDate="2026-05-04" />

<via:XDatePicker
    Width="320"
    Header="Release window"
    Description="Use MinimumDate and MaximumDate to restrict the selectable range."
    LeadingIcon="{via:MaterialIcon Kind=CalendarRange}"
    MaximumDate="2026-12-31"
    MinimumDate="2026-01-01"
    SelectedDate="{Binding ReleaseDate, Mode=TwoWay}" />

<via:XDatePicker
    Width="320"
    DateFormatString="dd.MM.yyyy"
    Header="German display format"
    SelectedDate="2026-05-04" />

<via:XDatePicker
    Width="320"
    Header="Markup extension icons"
    LeadingIcon="{via:MaterialIcon Kind=CalendarClock}"
    SelectedDate="2026-05-04"
    TrailingIcon="{via:MaterialIcon Kind=ChevronDown}" />

<via:XDatePicker
    Width="320"
    Header="Read-only"
    IsReadOnly="True"
    SelectedDate="2026-05-04" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
public sealed class XDatePickerDemoViewModel : DemoPageViewModel
{
    private DateTime? invoiceDate = new(2026, 5, 4);
    private DateTime? releaseDate = new(2026, 6, 18);

    public DateTime? InvoiceDate
    {
        get => invoiceDate;
        set => SetProperty(ref invoiceDate, value);
    }

    public DateTime? ReleaseDate
    {
        get => releaseDate;
        set => SetProperty(ref releaseDate, value);
    }
}

// Useful properties shown by the page:
//
// SelectedDate / Text / DisplayDate
// MinimumDate / MaximumDate
// DateFormatString
// IsDropDownOpen
// ShowResetButton
// Size
// CornerRadius
// Placeholder / Header / Description
// LeadingIcon / LeadingIconSize
// TrailingIcon / TrailingIconSize
// IsReadOnly
// FirstDayOfWeek
""";
    #endregion
}
#endregion
