// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDatePickerInteractionTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Smoke;

#region ### Class XDatePickerInteractionTests ###
/// <summary>
/// Verifies the user-facing text-entry behavior of <see cref="XDatePicker"/>.
/// </summary>
public sealed class XDatePickerInteractionTests
{
    #region ### Tests ###
    [Fact]
    public void XDatePicker_ShouldCommitCompactDayMonthInputWhenFocusLeavesEditor()
    {
        WpfTestHelper.Run(
            () =>
            {
                XDatePicker datePicker = new();
                Button nextControl = new() { Content = "Next" };
                Window host = CreateHost(datePicker, nextControl);

                host.Show();
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                TextBox editor = Assert.IsType<TextBox>(datePicker.Template.FindName("PART_TextBox", datePicker));
                editor.Focus();
                editor.Text = "0309";
                nextControl.Focus();
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Input);

                Assert.Equal(new DateTime(DateTime.Today.Year, 9, 3), datePicker.SelectedDate);
                Assert.True(datePicker.IsInputValid);
                host.Close();
            });
    }

    [Fact]
    public void XDatePicker_ShouldKeepInvalidInputVisibleAndPreserveSelectedDate()
    {
        WpfTestHelper.Run(
            () =>
            {
                DateTime selectedDate = new(2026, 5, 4);
                XDatePicker datePicker = new() { SelectedDate = selectedDate };
                Button nextControl = new() { Content = "Next" };
                Window host = CreateHost(datePicker, nextControl);

                host.Show();
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                TextBox editor = Assert.IsType<TextBox>(datePicker.Template.FindName("PART_TextBox", datePicker));
                editor.Focus();
                editor.Text = "not a date";
                nextControl.Focus();
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Input);

                Assert.Equal(selectedDate, datePicker.SelectedDate);
                Assert.Equal("not a date", datePicker.Text);
                Assert.False(datePicker.IsInputValid);
                host.Close();
            });
    }

    [Fact]
    public void XDatePicker_ShouldOpenTheSelectedMonthFromAnyPreviousCalendarView()
    {
        WpfTestHelper.Run(
            () =>
            {
                DateTime selectedDate = new(2026, 5, 4);
                XDatePicker datePicker = new() { SelectedDate = selectedDate };
                Window host = CreateHost(datePicker, new Button { Content = "Next" });

                host.Show();
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                Calendar calendar = Assert.IsType<XCalendar>(datePicker.Template.FindName("PART_Calendar", datePicker));
                calendar.DisplayMode = CalendarMode.Year;

                Button openButton = Assert.IsType<Button>(datePicker.Template.FindName("PART_DropDownButton", datePicker));
                openButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Input);

                Assert.True(datePicker.IsDropDownOpen);
                Assert.Equal(CalendarMode.Month, calendar.DisplayMode);
                Assert.Equal(selectedDate.Year, calendar.DisplayDate.Year);
                Assert.Equal(selectedDate.Month, calendar.DisplayDate.Month);
                host.Close();
            });
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Creates a host that makes focus transitions behave as they do in an application window.
    /// </summary>
    /// <param name="datePicker">The date picker under test.</param>
    /// <param name="nextControl">The control receiving focus after text entry.</param>
    /// <returns>The initialized host window.</returns>
    private static Window CreateHost(XDatePicker datePicker, Button nextControl)
    {
        StackPanel content = new() { Margin = new Thickness(16d) };
        content.Children.Add(datePicker);
        content.Children.Add(nextControl);

        return new Window
        {
            Width = 320d,
            Height = 180d,
            Content = content
        };
    }
    #endregion
}
#endregion
