// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCalendarDemoViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;

namespace VIA.WPF.Demo.ViewModels;

#region ### Class XCalendarDemoViewModel ###
/// <summary>
/// Represents the demo page view model for <c>XCalendar</c>.
/// </summary>
public sealed partial class XCalendarDemoViewModel : DemoPageViewModel
{
    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the selected reference date.
    /// </summary>
    [ObservableProperty]
    private DateTime? _selectedDate = new DateTime(2026, 5, 4);

    /// <inheritdoc/>
    public override string Title => "XCalendar";

    /// <inheritdoc/>
    public override string Description => "Demonstrates the VIA.WPF calendar with themed month, year and decade views plus complete interaction states.";

    /// <inheritdoc/>
    public override string XamlCode => """
<via:XCalendar
    DisplayDate="2026-05-01"
    FirstDayOfWeek="Monday"
    SelectedDate="{Binding SelectedDate, Mode=TwoWay}" />

<via:XCalendar
    DisplayMode="Year"
    DisplayDate="2026-05-01" />
""";

    /// <inheritdoc/>
    public override string CSharpCode => """
XCalendar calendar = new()
{
    DisplayDate = new DateTime(2026, 5, 1),
    FirstDayOfWeek = DayOfWeek.Monday,
    SelectedDate = new DateTime(2026, 5, 4),
};

calendar.BlackoutDates.Add(
    new CalendarDateRange(
        new DateTime(2026, 5, 18),
        new DateTime(2026, 5, 20)));
""";
    #endregion
}
#endregion
