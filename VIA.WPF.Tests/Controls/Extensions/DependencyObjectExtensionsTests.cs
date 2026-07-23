// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyObjectExtensionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class DependencyObjectExtensionsTests ###
/// <summary>
/// Provides tests for dependency object visual and logical tree extension helpers.
/// </summary>
public sealed class DependencyObjectExtensionsTests
{
    #region ### Private Fields ###
    private static readonly string[] ExpectedDescendantNames = ["RootText", "NestedText"];
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Ensures that parent and ancestor helpers walk the visual and logical tree.
    /// </summary>
    [Fact]
    public void DependencyObjectExtensions_ShouldFindParentsAncestorsAndChildren()
    {
        WpfTestHelper.Run(
            () =>
            {
                Grid root = new();
                StackPanel panel = new();
                Button button = new();
                TextBlock rootText = new() { Text = "RootText" };
                TextBlock nestedText = new() { Text = "NestedText" };

                root.Children.Add(rootText);
                root.Children.Add(panel);
                panel.Children.Add(button);
                panel.Children.Add(nestedText);

                Assert.Same(panel, button.FindVisualParent<StackPanel>());
                Assert.Same(root, button.FindVisualParent<Grid>());
                Assert.Same(button, button.FindVisualAncestorOrSelf<Button>());
                Assert.Same(root, root.FindVisualAncestorOrSelf<Grid>());
                Assert.Same(button, root.FindVisualChild<Button>());
                Assert.Equal(ExpectedDescendantNames, root.GetVisualDescendants<TextBlock>().Select(textBlock => textBlock.Text));
                Assert.Same(root, root.GetVisualSelfAndDescendants<Grid>().Single());
            });
    }

    /// <summary>
    /// Ensures that visual helpers handle null input safely.
    /// </summary>
    [Fact]
    public void DependencyObjectExtensions_ShouldHandleNullInputSafely()
    {
        DependencyObject? dependencyObject = null;

        Assert.Null(dependencyObject.FindVisualParent<Grid>());
        Assert.Null(dependencyObject.FindVisualAncestorOrSelf<Grid>());
        Assert.Null(dependencyObject.FindVisualChild<Grid>());
        Assert.Empty(dependencyObject.GetVisualDescendants<Grid>());
        Assert.Empty(dependencyObject.GetVisualSelfAndDescendants<Grid>());
        Assert.Null(dependencyObject.HitTestVisual<Grid>(new Point(0d, 0d)));
    }

    /// <summary>
    /// Ensures that hit testing can resolve a visual child at the requested point.
    /// </summary>
    [Fact]
    public void DependencyObjectExtensions_HitTestVisual_ShouldReturnVisualAtPoint()
    {
        WpfTestHelper.Run(
            () =>
            {
                Grid root = new()
                {
                    Width = 100d,
                    Height = 100d,
                    Background = Brushes.Transparent
                };
                Border child = new()
                {
                    Width = 50d,
                    Height = 50d,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                root.Children.Add(child);

                root.Measure(new Size(100d, 100d));
                root.Arrange(new Rect(0d, 0d, 100d, 100d));
                root.UpdateLayout();

                Border? result = root.HitTestVisual<Border>(new Point(10d, 10d));

                Assert.Same(child, result);
            });
    }
    #endregion
}
#endregion
