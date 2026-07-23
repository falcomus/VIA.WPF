// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdornerLayerExtensionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;
using AdornerLayerExtensionMethods = VIA.WPF.Extensions.AdornerLayerExtensions;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class AdornerLayerExtensionsTests ###
/// <summary>
/// Provides tests for adorner layer extension helpers.
/// </summary>
public sealed class AdornerLayerExtensionsTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that adorner helpers handle missing elements and missing layers safely.
    /// </summary>
    [Fact]
    public void AdornerLayerExtensions_ShouldHandleNullAndMissingLayerSafely()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border adornedElement = new();
                TestAdorner adorner = new(adornedElement);
                UIElement? nullElement = null;

                Assert.Null(nullElement.GetAdornerLayerSafe());
                Assert.False(nullElement.AddAdorner(adorner));
                Assert.Empty(nullElement.GetAdorners<TestAdorner>());
                Assert.Equal(0, nullElement.RemoveAdorners<TestAdorner>());
                nullElement.InvalidateAdorners();
                Assert.False(adornedElement.AddAdorner(adorner));
            });
    }

    /// <summary>
    /// Ensures that adorners can be added, found, invalidated and removed when an adorner layer exists.
    /// </summary>
    [Fact]
    public void AdornerLayerExtensions_ShouldAddFindInvalidateAndRemoveAdorners()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border adornedElement = new()
                {
                    Width = 100d,
                    Height = 50d,
                    Background = Brushes.Transparent
                };
                AdornerDecorator decorator = new()
                {
                    Child = adornedElement
                };
                decorator.Measure(new Size(100d, 50d));
                decorator.Arrange(new Rect(0d, 0d, 100d, 50d));
                decorator.UpdateLayout();
                TestAdorner adorner = new(adornedElement);

                bool added = adornedElement.AddAdorner(adorner);
                IReadOnlyList<TestAdorner> adorners = adornedElement.GetAdorners<TestAdorner>();

                adornedElement.InvalidateAdorners();
                int removedCount = adornedElement.RemoveAdorners<TestAdorner>();

                Assert.True(added);
                Assert.Single(adorners);
                Assert.Same(adorner, adorners[0]);
                Assert.Equal(1, removedCount);
                Assert.Empty(adornedElement.GetAdorners<TestAdorner>());
            });
    }

    /// <summary>
    /// Ensures that adorner bounds return an empty rectangle when input is incomplete.
    /// </summary>
    [Fact]
    public void AdornerLayerExtensions_GetBoundsRelativeTo_ShouldReturnEmptyForIncompleteInput()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                Visual? nullVisual = null;

                Assert.Equal(
                    Rect.Empty,
                    AdornerLayerExtensionMethods.GetBoundsRelativeTo((FrameworkElement?)null, element));

                Assert.Equal(
                    Rect.Empty,
                    AdornerLayerExtensionMethods.GetBoundsRelativeTo(element, nullVisual));

                Assert.Equal(
                    Rect.Empty,
                    AdornerLayerExtensionMethods.GetBoundsRelativeTo(element, element));
            });
    }

    /// <summary>
    /// Ensures that adding a null adorner is rejected.
    /// </summary>
    [Fact]
    public void AdornerLayerExtensions_AddAdorner_ShouldRejectNullAdorner()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();

                Assert.Throws<ArgumentNullException>(() => element.AddAdorner(null!));
            });
    }
    #endregion

    #region ### Test Types ###
    private sealed class TestAdorner(UIElement adornedElement) : Adorner(adornedElement)
    {
    }
    #endregion
}
#endregion
