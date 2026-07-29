// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XColorPickerTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XColorPickerTests ###
/// <summary>
/// Provides tests for picker defaults, template inputs and binding preservation.
/// </summary>
public sealed class XColorPickerTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that a new picker starts with an opaque black color.
    /// </summary>
    [Fact]
    public void Defaults_ShouldBeOpaqueBlack()
    {
        WpfTestHelper.Run(
            () =>
            {
                XColorPicker picker = new();

                Assert.Equal(Colors.Black, picker.SelectedColor);
                Assert.Equal((byte)0, picker.Red);
                Assert.Equal((byte)0, picker.Green);
                Assert.Equal((byte)0, picker.Blue);
                Assert.Equal(byte.MaxValue, picker.Alpha);
                Assert.Equal("#FF000000", picker.Hex);
            });
    }

    /// <summary>
    /// Ensures that the real template controls synchronize color channels and preserve the outer binding.
    /// </summary>
    [Fact]
    public void TemplateInputs_ShouldSynchronizeColorAndPreserveBinding()
    {
        WpfTestHelper.Run(
            () =>
            {
                ColorBindingSource source = new()
                {
                    Color = Colors.Black
                };

                XColorPicker picker = new();

                BindingOperations.SetBinding(
                    picker,
                    XColorPicker.SelectedColorProperty,
                    new Binding(nameof(ColorBindingSource.Color))
                    {
                        Source = source,
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });

                Window host = new()
                {
                    Width = 560d,
                    SizeToContent = SizeToContent.Height,
                    Content = new Border
                    {
                        Padding = new Thickness(20d),
                        Child = picker
                    }
                };

                try
                {
                    host.Show();
                    host.Dispatcher.Invoke(static () => { }, DispatcherPriority.Render);

                    XSlider greenSlider = GetPart<XSlider>(picker, "PART_GreenSlider");
                    XNumberBox redNumberBox = GetPart<XNumberBox>(picker, "PART_RedNumberBox");
                    XNumberBox blueNumberBox = GetPart<XNumberBox>(picker, "PART_BlueNumberBox");
                    XNumberBox alphaNumberBox = GetPart<XNumberBox>(picker, "PART_AlphaNumberBox");
                    XTextBox hexTextBox = GetPart<XTextBox>(picker, "PART_HexTextBox");

                    redNumberBox.ApplyTemplate();
                    TextBox redEditor = Assert.IsType<TextBox>(
                        redNumberBox.Template.FindName("PART_TextBox", redNumberBox));
                    Button spinUpButton = Assert.IsType<Button>(
                        redNumberBox.Template.FindName("PART_SpinUpButton", redNumberBox));

                    Assert.Equal(TextAlignment.Right, redEditor.TextAlignment);
                    Assert.Equal(0d, redNumberBox.MinWidth);
                    Assert.Equal(70d, redNumberBox.Width);

                    redNumberBox.Text = "73";
                    host.Dispatcher.Invoke(static () => { }, DispatcherPriority.DataBind);
                    Assert.Equal(73d, redNumberBox.Value);
                    Assert.Equal((byte)73, picker.Red);
                    Assert.Equal((byte)73, source.Color.R);

                    spinUpButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    host.Dispatcher.Invoke(static () => { }, DispatcherPriority.DataBind);
                    Assert.Equal((byte)74, picker.Red);
                    Assert.Equal((byte)74, source.Color.R);

                    greenSlider.Value = 88d;
                    host.Dispatcher.Invoke(static () => { }, DispatcherPriority.DataBind);
                    Assert.Equal((byte)88, picker.Green);

                    blueNumberBox.Text = "101";
                    host.Dispatcher.Invoke(static () => { }, DispatcherPriority.DataBind);
                    Assert.Equal((byte)101, picker.Blue);

                    alphaNumberBox.Text = "128";
                    host.Dispatcher.Invoke(static () => { }, DispatcherPriority.DataBind);
                    Assert.Equal((byte)128, picker.Alpha);

                    hexTextBox.Text = "#80445566";
                    host.Dispatcher.Invoke(static () => { }, DispatcherPriority.DataBind);
                    Assert.Equal(Color.FromArgb(128, 68, 85, 102), picker.SelectedColor);
                    Assert.Equal(Color.FromArgb(128, 68, 85, 102), source.Color);
                    Assert.NotNull(BindingOperations.GetBindingExpression(picker, XColorPicker.SelectedColorProperty));
                }
                finally
                {
                    host.Close();
                }
            });
    }
    #endregion

    #region ### Private Methods ###
    private static T GetPart<T>(FrameworkElement control, string partName)
        where T : DependencyObject
    {
        return Assert.IsType<T>(control.FindName(partName));
    }

    private sealed class ColorBindingSource : DependencyObject
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color),
            typeof(Color),
            typeof(ColorBindingSource),
            new FrameworkPropertyMetadata(Colors.Black));

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }
    #endregion
}
#endregion
