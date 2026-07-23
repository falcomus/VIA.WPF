// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTimePickerDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XTimePickerDemoViewModel ###
/// <summary>
/// Represents the demo view model for the XTimePicker showcase page.
/// </summary>
public sealed class XTimePickerDemoViewModel : DemoPageViewModel
{
    #region ### Fields ###
    private TimeSpan? reminderTime = new(9, 30, 0);
    private TimeSpan? supportTime = new(13, 45, 0);
    #endregion

    #region ### Public Properties ###
    /// <inheritdoc />
    public override string Title => "XTimePicker";

    /// <inheritdoc />
    public override string Description => "Demonstrates the themed XTimePicker with sizes, selected time binding, minimum and maximum time, minute steps, format strings, reset button, leading and trailing icons and read-only states.";

    /// <summary>
    /// Gets or sets the reminder time sample value.
    /// </summary>
    public TimeSpan? ReminderTime
    {
        get => this.reminderTime;
        set => this.SetProperty(ref this.reminderTime, value);
    }

    /// <summary>
    /// Gets or sets the support time sample value.
    /// </summary>
    public TimeSpan? SupportTime
    {
        get => this.supportTime;
        set => this.SetProperty(ref this.supportTime, value);
    }

    /// <inheritdoc />
    public override string XamlCode => """
<via:XTimePicker
    Width="280"
    Header="Reminder"
    Placeholder="Select time"
    SelectedTime="{Binding ReminderTime, Mode=TwoWay}" />

<via:XTimePicker
    Width="240"
    Header="Small"
    Placeholder="Compact time"
    Size="Small"
    SelectedTime="09:30:00" />

<via:XTimePicker
    Width="320"
    Header="Support hours"
    Description="Only business hours are selectable."
    MaximumTime="18:00:00"
    LeadingIcon="{via:MaterialIcon Kind=ClockCheckOutline}"
    MinimumTime="08:00:00"
    SelectedTime="{Binding SupportTime, Mode=TwoWay}"
    StepMinutes="15" />

<via:XTimePicker
    Width="320"
    Header="24-hour format"
    SelectedTime="18:45:00"
    TimeFormatString="hh\:mm" />

<via:XTimePicker
    Width="320"
    Header="Markup extension icons"
    LeadingIcon="{via:MaterialIcon Kind=ClockOutline}"
    SelectedTime="11:00:00"
    TrailingIcon="{via:MaterialIcon Kind=ChevronDown}" />

<via:XTimePicker
    Width="320"
    Header="Read-only"
    IsReadOnly="True"
    SelectedTime="08:45:00" />
""";

    /// <inheritdoc />
    public override string CSharpCode => """
public sealed class XTimePickerDemoViewModel : DemoPageViewModel
{
    private TimeSpan? reminderTime = new(9, 30, 0);
    private TimeSpan? supportTime = new(13, 45, 0);

    public TimeSpan? ReminderTime
    {
        get => reminderTime;
        set => SetProperty(ref reminderTime, value);
    }

    public TimeSpan? SupportTime
    {
        get => supportTime;
        set => SetProperty(ref supportTime, value);
    }
}

// Useful properties shown by the page:
//
// SelectedTime / Text
// MinimumTime / MaximumTime
// StepMinutes
// TimeFormatString
// IsDropDownOpen
// ShowResetButton
// Size
// CornerRadius
// Placeholder / Header / Description
// LeadingIcon / LeadingIconSize
// TrailingIcon / TrailingIconSize
// IsReadOnly
""";
    #endregion
}
#endregion
