// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementExtensionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class FrameworkElementExtensionsTests ###
/// <summary>
/// Provides tests for framework element extension helpers.
/// </summary>
public sealed class FrameworkElementExtensionsTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that template part lookup applies the template and returns matching parts.
    /// </summary>
    [Fact]
    public void FrameworkElementExtensions_FindTemplatePart_ShouldReturnNamedPart()
    {
        WpfTestHelper.Run(
            () =>
            {
                Control control = new()
                {
                    Template = CreateTemplateWithNamedBorder()
                };

                Border? border = control.FindTemplatePart<Border>("PART_Border");
                TextBlock? missing = control.FindTemplatePart<TextBlock>("PART_Border");

                Assert.NotNull(border);
                Assert.Null(missing);
            });
    }

    /// <summary>
    /// Ensures that resource lookup returns only resources of the requested type.
    /// </summary>
    [Fact]
    public void FrameworkElementExtensions_TryFindResource_ShouldReturnTypedResource()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                SolidColorBrush brush = Brushes.Red.Clone();
                element.Resources["Brush"] = brush;
                element.Resources["Text"] = "Value";

                bool foundBrush = element.TryFindResource("Brush", out SolidColorBrush? foundResource);
                bool wrongType = element.TryFindResource("Text", out SolidColorBrush? wrongTypeResource);

                Assert.True(foundBrush);
                Assert.Same(brush, foundResource);
                Assert.False(wrongType);
                Assert.Null(wrongTypeResource);
            });
    }

    /// <summary>
    /// Ensures that loaded helpers execute when the Loaded event is raised.
    /// </summary>
    [Fact]
    public void FrameworkElementExtensions_LoadedHelpers_ShouldCompleteAfterLoadedEvent()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new();
                int runCount = 0;

                element.RunWhenLoaded(() => runCount++);
                Task loadedTask = element.WhenLoadedAsync();

                Assert.False(loadedTask.IsCompleted);

                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

                Assert.Equal(1, runCount);
                Assert.True(loadedTask.IsCompletedSuccessfully);
            });
    }

    /// <summary>
    /// Ensures that deferred helpers queue their operations on the dispatcher.
    /// </summary>
    [Fact]
    public void FrameworkElementExtensions_DeferredHelpers_ShouldQueueDispatcherOperations()
    {
        WpfTestHelper.Run(
            () =>
            {
                Border element = new()
                {
                    Focusable = true,
                    Width = 20d,
                    Height = 20d
                };

                element.FocusLater(DispatcherPriority.Background);
                element.BringIntoViewLater(DispatcherPriority.Background);

                WpfTestHelper.DoEvents();
            });
    }

    /// <summary>
    /// Ensures that typed data context lookup returns matching contexts only.
    /// </summary>
    /// <summary>
    /// Ensures that typed data contexts are returned only when the type matches.
    /// </summary>
    [Fact]
    public void FrameworkElementExtensions_GetDataContext_ShouldReturnTypedContext()
    {
        WpfTestHelper.Run(
            () =>
            {
                object context = new();
                FrameworkElement element = new()
                {
                    DataContext = context
                };

                Assert.Same(context, element.GetDataContext<object>());
                Assert.Null(element.GetDataContext<string>());
            });
    }

    /// <summary>
    /// Ensures that framework element helpers reject null arguments where required.
    /// </summary>
    [Fact]
    public void FrameworkElementExtensions_ShouldRejectNullArguments()
    {
        WpfTestHelper.Run(
async () =>
            {
                Control? control = null;
                Border? element = null;
                Border validElement = new();
                Control validControl = new();

                Assert.Throws<ArgumentNullException>(() => control!.FindTemplatePart<Border>("PART"));
                Assert.Throws<ArgumentException>(() => validControl.FindTemplatePart<Border>(string.Empty));
                Assert.Throws<ArgumentNullException>(() => element!.TryFindResource("Key", out object? _));
                Assert.Throws<ArgumentNullException>(() => validElement.TryFindResource<object>(null!, out _));
                Assert.Throws<ArgumentNullException>(() => element!.RunWhenLoaded(() => { }));
                Assert.Throws<ArgumentNullException>(() => validElement.RunWhenLoaded(null!));
                await Assert.ThrowsAsync<ArgumentNullException>(() => element!.WhenLoadedAsync());
                Assert.Throws<ArgumentNullException>(() => element!.FocusLater());
                Assert.Throws<ArgumentNullException>(() => element!.BringIntoViewLater());
            });
    }
    #endregion

    #region ### Private Methods ###
    private static ControlTemplate CreateTemplateWithNamedBorder()
    {
        FrameworkElementFactory borderFactory = new(typeof(Border))
        {
            Name = "PART_Border"
        };

        return new ControlTemplate(typeof(Control))
        {
            VisualTree = borderFactory
        };
    }
    #endregion

    #region ### Test Types ###
    private sealed class TestViewModel(string name)
    {
        public string Name { get; } = name;
    }
    #endregion
}
#endregion
