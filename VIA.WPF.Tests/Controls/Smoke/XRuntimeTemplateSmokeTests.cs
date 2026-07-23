// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XRuntimeTemplateSmokeTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using VIA.WPF.Controls;
using VIA.WPF.Icons;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Smoke;

#region ### Class XRuntimeTemplateSmokeTests ###
/// <summary>
/// Verifies templates that WPF only materializes when a control is connected to an HWND.
/// </summary>
public sealed class XRuntimeTemplateSmokeTests
{
    #region ### Tests ###
    [Fact]
    public void XBorder_ShouldMaterializeWithIconContent()
    {
        WpfTestHelper.Run(
            () =>
            {
                Window host = new()
                {
                    Width = 320d,
                    Height = 180d,
                    Content = new XBorder
                    {
                        Content = new XMaterialIcon()
                    }
                };

                host.Show();
                host.Close();
            });
    }

    [Fact]
    public void UpdatedControlFamilies_ShouldMaterializeTheirTemplates()
    {
        WpfTestHelper.Run(
            () =>
            {
                XComboBox comboBox = new()
                {
                    CanClearSelection = true,
                    SelectedIndex = 0
                };
                comboBox.Items.Add("First");
                comboBox.Items.Add("Second");

                XListBox listBox = new();
                listBox.Items.Add(new XListBoxItem { Content = "First", IsSelected = true });
                listBox.Items.Add(new XListBoxItem { Content = "Second" });

                VIA.WPF.Controls.XTreeView treeView = new();
                XTreeViewItem treeItem = new() { Header = "Root", IsExpanded = true, IsSelected = true };
                treeItem.Items.Add(new XTreeViewItem { Header = "Child" });
                treeView.Items.Add(treeItem);

                XTabControl tabControl = new() { ShowTabCloseButton = true };
                tabControl.Items.Add(new XTabItem { Header = "General", Content = "Content", CanClose = true });

                XCheckGroup checkGroup = new() { Title = "Checks" };
                checkGroup.Items.Add(new XCheckGroupItem { Content = "First", IsChecked = true });
                checkGroup.Items.Add(new XCheckGroupItem { Content = "Second" });

                XRadioGroup radioGroup = new() { Title = "Options" };
                radioGroup.Items.Add(new XRadioGroupItem { Content = "First", IsChecked = true });
                radioGroup.Items.Add(new XRadioGroupItem { Content = "Second" });

                VIA.WPF.Controls.XDataGrid dataGrid = new()
                {
                    Height = 120d,
                    ItemsSource = new[]
                    {
                        new { Name = "Alpha", Status = "Ready" },
                        new { Name = "Beta", Status = "Pending" }
                    },
                    SelectedIndex = 0
                };

                StackPanel content = new()
                {
                    Margin = new Thickness(16d)
                };
                content.Children.Add(new XSearchBox { Text = "Search" });
                content.Children.Add(comboBox);
                content.Children.Add(new XNumberBox());
                XDatePicker datePicker = new()
                {
                    IsDropDownOpen = true,
                    SelectedDate = new DateTime(2026, 5, 4)
                };
                content.Children.Add(datePicker);
                XCalendar calendar = new()
                {
                    DisplayDate = new DateTime(2026, 5, 1),
                    SelectedDate = new DateTime(2026, 5, 4)
                };
                content.Children.Add(calendar);
                content.Children.Add(new XTimePicker());
                content.Children.Add(new XPasswordBox());
                content.Children.Add(new XSecurePasswordBox());
                content.Children.Add(new XLookupComboBox());
                content.Children.Add(new XLookupTreeComboBox());
                content.Children.Add(new XCheckBox { Content = "Check", IsChecked = true });
                content.Children.Add(new XRadioButton { Content = "Radio", IsChecked = true });
                content.Children.Add(new XToggleSwitch { Content = "Toggle", IsChecked = true });
                content.Children.Add(new XToggleButton { Content = "Toggle button", IsChecked = true });
                content.Children.Add(new XToggleDropDown { Content = "Toggle menu", DropDownContent = "Menu content" });
                content.Children.Add(new XSlider { Value = 45d });
                content.Children.Add(new XProgressBar { Value = 65d });
                content.Children.Add(new XBadge { Content = "Ready" });
                content.Children.Add(checkGroup);
                content.Children.Add(radioGroup);
                content.Children.Add(new XExpander { Header = "Details", Content = "Expanded content", IsExpanded = true });
                content.Children.Add(listBox);
                content.Children.Add(treeView);
                content.Children.Add(dataGrid);
                content.Children.Add(tabControl);
                content.Children.Add(new XSplitView { FirstContent = "First pane", SecondContent = "Second pane" });
                content.Children.Add(new XRecentItemTree());
                content.Children.Add(new VIA.WPF.Controls.XViewContainer { ListHost = new TextBlock { Text = "List host" } });

                Window host = new()
                {
                    Width = 640d,
                    Height = 760d,
                    Content = new ScrollViewer { Content = content }
                };

                host.Show();
                host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                Assert.IsType<XCalendar>(datePicker.Template.FindName("PART_Calendar", datePicker));

                CalendarDayButton[] visibleDayButtons = FindVisualChildren<CalendarDayButton>(calendar)
                    .Where(button => button.IsVisible)
                    .ToArray();
                Assert.True(visibleDayButtons.Length >= 28);

                Point[] distinctPositions = visibleDayButtons
                    .Select(button => button.TranslatePoint(new Point(), calendar))
                    .Distinct()
                    .ToArray();
                Assert.Equal(visibleDayButtons.Length, distinctPositions.Length);

                host.Close();
            });
    }
    #endregion

    #region ### Private Methods ###
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
