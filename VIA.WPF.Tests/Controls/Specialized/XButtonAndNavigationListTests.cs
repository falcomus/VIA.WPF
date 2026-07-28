// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XButtonAndNavigationListTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Specialized;

#region ### Class XButtonAndNavigationListTests ###
/// <summary>
/// Tests semantic button sizing and navigation list surface variants.
/// </summary>
public sealed class XButtonAndNavigationListTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that semantic button sizes remain visible inside stretching grid rows.
    /// </summary>
    [Fact]
    public void XButtonSize_ShouldApplyExactThemeHeightUnlessLocallyOverridden()
    {
        WpfTestHelper.Run(
            () =>
            {
                XButton small = new() { Content = "Small", Size = XControlSize.Small };
                XButton medium = new() { Content = "Medium", Size = XControlSize.Medium };
                XButton large = new() { Content = "Large", Size = XControlSize.Large };
                XButton custom = new() { Content = "Custom", Size = XControlSize.Small, Height = 44d };

                Grid grid = new();
                for (int index = 0; index < 4; index++)
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(56d) });
                }

                Grid.SetRow(small, 0);
                Grid.SetRow(medium, 1);
                Grid.SetRow(large, 2);
                Grid.SetRow(custom, 3);
                grid.Children.Add(small);
                grid.Children.Add(medium);
                grid.Children.Add(large);
                grid.Children.Add(custom);

                Window host = new()
                {
                    Width = 320d,
                    Height = 280d,
                    ShowInTaskbar = false,
                    Content = grid
                };

                try
                {
                    host.Show();
                    host.UpdateLayout();

                    Assert.Equal(XControlSizeMetrics.SmallHeight, small.Height);
                    Assert.Equal(XControlSizeMetrics.MediumHeight, medium.Height);
                    Assert.Equal(XControlSizeMetrics.LargeHeight, large.Height);
                    Assert.Equal(XControlSizeMetrics.SmallHeight, small.ActualHeight);
                    Assert.Equal(XControlSizeMetrics.MediumHeight, medium.ActualHeight);
                    Assert.Equal(XControlSizeMetrics.LargeHeight, large.ActualHeight);
                    Assert.Equal(44d, custom.ActualHeight);
                }
                finally
                {
                    host.Close();
                }
            });
    }

    /// <summary>
    /// Verifies that link buttons retain natural height.
    /// </summary>
    [Fact]
    public void XButtonLinkAppearance_ShouldRetainNaturalHeight()
    {
        WpfTestHelper.Run(
            () =>
            {
                XButton link = new()
                {
                    Appearance = XControlAppearance.Link,
                    Content = "Link",
                    Size = XControlSize.Large
                };

                Window host = new()
                {
                    Width = 240d,
                    Height = 120d,
                    ShowInTaskbar = false,
                    Content = link
                };

                try
                {
                    host.Show();
                    host.UpdateLayout();

                    Assert.True(double.IsNaN(link.Height));
                    Assert.True(link.ActualHeight > 0d);
                }
                finally
                {
                    host.Close();
                }
            });
    }

    /// <summary>
    /// Verifies that navigation variants resolve to the expected theme brush families.
    /// </summary>
    [Fact]
    public void XNavigationListVariant_ShouldApplySurfaceAndDarkBrushFamilies()
    {
        WpfTestHelper.Run(
            () =>
            {
                XNavigationList surface = new() { Variant = XNavigationListVariant.Surface };
                XNavigationList dark = new() { Variant = XNavigationListVariant.Dark };

                StackPanel panel = new();
                panel.Children.Add(surface);
                panel.Children.Add(dark);

                Window host = new()
                {
                    Width = 320d,
                    Height = 240d,
                    ShowInTaskbar = false,
                    Content = panel
                };

                try
                {
                    host.Show();
                    host.UpdateLayout();

                    SolidColorBrush surfaceBackground = Assert.IsType<SolidColorBrush>(surface.Background);
                    SolidColorBrush surfaceForeground = Assert.IsType<SolidColorBrush>(surface.Foreground);
                    SolidColorBrush darkBackground = Assert.IsType<SolidColorBrush>(dark.Background);
                    SolidColorBrush darkForeground = Assert.IsType<SolidColorBrush>(dark.Foreground);

                    Assert.NotEqual(surfaceBackground.Color, darkBackground.Color);
                    Assert.NotEqual(surfaceForeground.Color, darkForeground.Color);
                }
                finally
                {
                    host.Close();
                }
            });
    }
    #endregion
}
#endregion
