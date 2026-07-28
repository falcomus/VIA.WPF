// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDatePickerInteractionTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
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

    [Fact]
    public void XDatePicker_ShouldNavigateMonthsWhenItsCalendarAlreadyHasASelectedDate()
    {
        WpfTestHelper.Run(
            () =>
            {
                XDatePicker datePicker = new() { SelectedDate = new DateTime(2026, 5, 4) };
                Window host = CreateHost(datePicker, new Button { Content = "Next" });

                host.Show();
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                Button openButton = Assert.IsType<Button>(datePicker.Template.FindName("PART_DropDownButton", datePicker));
                openButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Input);

                Calendar calendar = Assert.IsType<XCalendar>(datePicker.Template.FindName("PART_Calendar", datePicker));
                Button previousButton = FindVisualChildren<Button>(calendar)
                    .Single(button => button.Name == "PART_PreviousButton");
                previousButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Input);

                Assert.Equal(2026, calendar.DisplayDate.Year);
                Assert.Equal(4, calendar.DisplayDate.Month);
                Assert.Equal(new DateTime(2026, 5, 4), datePicker.SelectedDate);
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

    /// <summary>
    /// Enumerates visual descendants of a requested type.
    /// </summary>
    /// <typeparam name="T">The visual type to enumerate.</typeparam>
    /// <param name="parent">The visual root.</param>
    /// <returns>The matching descendants.</returns>
    private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent)
        where T : DependencyObject
    {
        for (int index = 0; index < VisualTreeHelper.GetChildrenCount(parent); index++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, index);
            if (child is T match)
            {
                yield return match;
            }

            foreach (T descendant in FindVisualChildren<T>(child))
            {
                yield return descendant;
            }
        }
    }
    #endregion
}
#endregion
