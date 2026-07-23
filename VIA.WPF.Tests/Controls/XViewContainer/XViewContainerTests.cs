// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XViewContainerTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using VIA.WPF.Controls;
using VIA.WPF.Controls.Navigation;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.XViewContainer;

#region ### Class XViewContainerTests ###
/// <summary>
/// Tests the public contracts of <see cref="VIA.WPF.Controls.XViewContainer" />.
/// </summary>
public sealed class XViewContainerTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies the default dependency property values of <see cref="VIA.WPF.Controls.XViewContainer" />.
    /// </summary>
    [Fact]
    public void Constructor_ShouldExposeExpectedDefaultValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new();

                Assert.Null(container.ListHost);
                Assert.Null(container.TreeHost);
                Assert.Equal(XContentViewMode.Grid, container.ViewMode);
                Assert.Null(container.CrudContext);
                Assert.True(container.RequireCrudContext);
                Assert.Equal(XViewDetailPresentation.Dialog, container.DetailPresentation);
                Assert.Null(container.DetailHost);
                Assert.Null(container.DetailHeader);
                Assert.Null(container.DetailFooter);
                Assert.False(container.IsDetailOpen);
                Assert.Equal(XViewFlyoutPlacement.Top, container.DetailPlacement);
                Assert.True(double.IsNaN(container.DetailWidth));
                Assert.Equal(520d, container.DetailMinWidth);
                Assert.Equal(double.PositiveInfinity, container.DetailMaxWidth);
                Assert.Equal(0d, container.DetailMinHeight);
                Assert.Equal(560d, container.DetailMaxHeight);
                Assert.Equal(new Thickness(24d), container.DetailMargin);
                Assert.Equal(new Thickness(18d), container.DetailPadding);
                Assert.Equal(new Thickness(18d, 14d, 10d, 12d), container.DetailHeaderPadding);
                Assert.Equal(new Thickness(18d, 12d, 18d, 14d), container.DetailFooterPadding);
                Assert.Equal(new CornerRadius(12d), container.DetailCornerRadius);
                Assert.Null(container.DetailBackground);
                Assert.Null(container.DetailBorderBrush);
                Assert.Equal(new Thickness(1d), container.DetailBorderThickness);
                Assert.Equal(HorizontalAlignment.Center, container.DetailHorizontalAlignment);
                Assert.Equal(Color.FromRgb(0, 10, 30), ((SolidColorBrush)container.OverlayBackground).Color);
                Assert.Equal(0.4d, container.OverlayOpacity);
                Assert.Equal(new CornerRadius(4d), container.OverlayCornerRadius);
                Assert.True(container.ShowDetailCloseButton);
                Assert.False(container.CloseOnOverlayClick);
                Assert.True(container.IsModal);
                Assert.True(container.EnableDetailAnimation);
                Assert.Equal(XViewDetailAnimation.SlideZoom, container.DetailAnimation);
                Assert.Equal(18d, container.DetailAnimationOffset);
                Assert.Equal(0.96d, container.DetailAnimationScale);
                Assert.Equal(new Duration(TimeSpan.FromMilliseconds(220d)), container.DetailAnimationDuration);
                Assert.Null(container.CloseDetailCommand);
                Assert.Null(container.CloseDetailCommandParameter);
                Assert.Null(container.PrimaryDetailCommand);
                Assert.Null(container.PrimaryDetailCommandParameter);
                Assert.Equal("OK", container.PrimaryDetailText);
                Assert.Equal("Cancel", container.CancelDetailText);
                Assert.True(container.ShowDefaultDetailFooter);
                Assert.Null(container.ValidationSource);
                Assert.False(container.ShowValidationHint);
                Assert.Null(container.EffectiveValidationSource);
                Assert.False(container.HasTreeHost);
                Assert.False(container.HasDetailHeader);
                Assert.False(container.HasDetailFooter);
            });
    }

    /// <summary>
    /// Verifies that host and template dependency property values are stored unchanged.
    /// </summary>
    [Fact]
    public void HostProperties_ShouldStoreAssignedValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new();
                object listHost = new();
                object treeHost = new();
                object detailHost = new();
                object detailHeader = new();
                object detailFooter = new();
                DataTemplate listTemplate = new();
                DataTemplate treeTemplate = new();
                DataTemplate detailTemplate = new();
                DataTemplate headerTemplate = new();
                DataTemplate footerTemplate = new();

                container.ListHost = listHost;
                container.ListHostTemplate = listTemplate;
                container.TreeHost = treeHost;
                container.TreeHostTemplate = treeTemplate;
                container.DetailHost = detailHost;
                container.DetailHostTemplate = detailTemplate;
                container.DetailHeader = detailHeader;
                container.DetailHeaderTemplate = headerTemplate;
                container.DetailFooter = detailFooter;
                container.DetailFooterTemplate = footerTemplate;

                Assert.Same(listHost, container.ListHost);
                Assert.Same(listTemplate, container.ListHostTemplate);
                Assert.Same(treeHost, container.TreeHost);
                Assert.Same(treeTemplate, container.TreeHostTemplate);
                Assert.Same(detailHost, container.DetailHost);
                Assert.Same(detailTemplate, container.DetailHostTemplate);
                Assert.Same(detailHeader, container.DetailHeader);
                Assert.Same(headerTemplate, container.DetailHeaderTemplate);
                Assert.Same(detailFooter, container.DetailFooter);
                Assert.Same(footerTemplate, container.DetailFooterTemplate);
            });
    }

    /// <summary>
    /// Verifies that layout dependency property values are stored unchanged.
    /// </summary>
    [Fact]
    public void LayoutProperties_ShouldStoreAssignedValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new()
                {
                    ViewMode = XContentViewMode.Tree,
                    DetailPlacement = XViewFlyoutPlacement.Center,
                    DetailWidth = 360d,
                    DetailMinWidth = 200d,
                    DetailMaxWidth = 640d,
                    DetailMinHeight = 120d,
                    DetailMaxHeight = 480d,
                    DetailMargin = new Thickness(1d, 2d, 3d, 4d),
                    DetailPadding = new Thickness(5d, 6d, 7d, 8d),
                    DetailHeaderPadding = new Thickness(9d, 10d, 11d, 12d),
                    DetailFooterPadding = new Thickness(13d, 14d, 15d, 16d),
                    DetailCornerRadius = new CornerRadius(1d, 2d, 3d, 4d),
                    DetailBorderThickness = new Thickness(2d),
                    DetailHorizontalAlignment = HorizontalAlignment.Left
                };

                Assert.Equal(XContentViewMode.Tree, container.ViewMode);
                Assert.Equal(XViewFlyoutPlacement.Center, container.DetailPlacement);
                Assert.Equal(360d, container.DetailWidth);
                Assert.Equal(200d, container.DetailMinWidth);
                Assert.Equal(640d, container.DetailMaxWidth);
                Assert.Equal(120d, container.DetailMinHeight);
                Assert.Equal(480d, container.DetailMaxHeight);
                Assert.Equal(new Thickness(1d, 2d, 3d, 4d), container.DetailMargin);
                Assert.Equal(new Thickness(5d, 6d, 7d, 8d), container.DetailPadding);
                Assert.Equal(new Thickness(9d, 10d, 11d, 12d), container.DetailHeaderPadding);
                Assert.Equal(new Thickness(13d, 14d, 15d, 16d), container.DetailFooterPadding);
                Assert.Equal(new CornerRadius(1d, 2d, 3d, 4d), container.DetailCornerRadius);
                Assert.Equal(new Thickness(2d), container.DetailBorderThickness);
                Assert.Equal(HorizontalAlignment.Left, container.DetailHorizontalAlignment);
            });
    }

    /// <summary>
    /// Verifies that visual option dependency property values are stored unchanged.
    /// </summary>
    [Fact]
    public void VisualProperties_ShouldStoreAssignedValues()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new();
                Brush detailBackground = Brushes.AliceBlue;
                Brush borderBrush = Brushes.CadetBlue;
                Brush overlayBrush = Brushes.DarkGray;

                container.DetailBackground = detailBackground;
                container.DetailBorderBrush = borderBrush;
                container.OverlayBackground = overlayBrush;
                container.OverlayOpacity = 0.42d;
                container.ShowDetailCloseButton = false;
                container.CloseOnOverlayClick = true;
                container.IsModal = true;
                container.EnableDetailAnimation = false;
                container.DetailAnimationOffset = 24d;
                container.DetailAnimationDuration = new Duration(TimeSpan.FromMilliseconds(80d));
                container.ShowValidationHint = true;

                Assert.Same(detailBackground, container.DetailBackground);
                Assert.Same(borderBrush, container.DetailBorderBrush);
                Assert.Same(overlayBrush, container.OverlayBackground);
                Assert.Equal(0.42d, container.OverlayOpacity);
                Assert.False(container.ShowDetailCloseButton);
                Assert.True(container.CloseOnOverlayClick);
                Assert.True(container.IsModal);
                Assert.False(container.EnableDetailAnimation);
                Assert.Equal(24d, container.DetailAnimationOffset);
                Assert.Equal(new Duration(TimeSpan.FromMilliseconds(80d)), container.DetailAnimationDuration);
                Assert.True(container.ShowValidationHint);
            });
    }

    /// <summary>
    /// Verifies that <see cref="VIA.WPF.Controls.XViewContainer.IsDetailOpen" /> binds two-way by default.
    /// </summary>
    [Fact]
    public void IsDetailOpen_ShouldBindTwoWayByDefault()
    {
        FrameworkPropertyMetadata metadata = Assert.IsType<FrameworkPropertyMetadata>(
            VIA.WPF.Controls.XViewContainer.IsDetailOpenProperty.GetMetadata(typeof(VIA.WPF.Controls.XViewContainer)));

        Assert.True(metadata.BindsTwoWayByDefault);
    }

    /// <summary>
    /// Verifies that meaningful tree host values update <see cref="VIA.WPF.Controls.XViewContainer.HasTreeHost" />.
    /// </summary>
    [Fact]
    public void TreeHost_ShouldUpdateHasTreeHost()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new();

                Assert.False(container.HasTreeHost);

                container.TreeHost = "   ";
                Assert.False(container.HasTreeHost);

                container.TreeHost = "Tree";
                Assert.True(container.HasTreeHost);

                container.TreeHost = null;
                Assert.False(container.HasTreeHost);

                container.TreeHost = new object();
                Assert.True(container.HasTreeHost);
            });
    }

    /// <summary>
    /// Verifies that meaningful header values update <see cref="VIA.WPF.Controls.XViewContainer.HasDetailHeader" />.
    /// </summary>
    [Fact]
    public void DetailHeader_ShouldUpdateHasDetailHeader()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new();

                Assert.False(container.HasDetailHeader);

                container.DetailHeader = string.Empty;
                Assert.False(container.HasDetailHeader);

                container.DetailHeader = "Header";
                Assert.True(container.HasDetailHeader);

                container.DetailHeader = null;
                Assert.False(container.HasDetailHeader);

                container.DetailHeader = new TextBlock();
                Assert.True(container.HasDetailHeader);
            });
    }

    /// <summary>
    /// Verifies that meaningful footer values update <see cref="VIA.WPF.Controls.XViewContainer.HasDetailFooter" />.
    /// </summary>
    [Fact]
    public void DetailFooter_ShouldUpdateHasDetailFooter()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new();

                Assert.False(container.HasDetailFooter);

                container.DetailFooter = "\t";
                Assert.False(container.HasDetailFooter);

                container.DetailFooter = "Footer";
                Assert.True(container.HasDetailFooter);

                container.DetailFooter = null;
                Assert.False(container.HasDetailFooter);

                container.DetailFooter = new Button();
                Assert.True(container.HasDetailFooter);
            });
    }

    /// <summary>
    /// Verifies that the effective validation source uses the detail host as fallback.
    /// </summary>
    [Fact]
    public void EffectiveValidationSource_ShouldUseDetailHostWhenValidationSourceIsNull()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new();
                object detailHost = new();

                container.DetailHost = detailHost;

                Assert.Same(detailHost, container.EffectiveValidationSource);
            });
    }

    /// <summary>
    /// Verifies that an explicit validation source overrides the detail host.
    /// </summary>
    [Fact]
    public void EffectiveValidationSource_ShouldPreferExplicitValidationSource()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = new();
                object detailHost = new();
                object validationSource = new();

                container.DetailHost = detailHost;
                container.ValidationSource = validationSource;

                Assert.Same(validationSource, container.EffectiveValidationSource);

                container.ValidationSource = null;

                Assert.Same(detailHost, container.EffectiveValidationSource);
            });
    }

    /// <summary>
    /// Verifies that the minimal control template is applied without requiring theme resources.
    /// </summary>
    [Fact]
    public void OnApplyTemplate_ShouldInitializeClosedDetailState()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = CreateTemplatedContainer();
                container.EnableDetailAnimation = false;
                container.DetailAnimationOffset = 25d;
                container.OverlayOpacity = 0.33d;
                container.IsDetailOpen = false;

                ApplyTemplate(container);

                FrameworkElement detailLayer = GetTemplatePart<FrameworkElement>(container, "PART_DetailLayer");
                FrameworkElement overlay = GetTemplatePart<FrameworkElement>(container, "PART_Overlay");
                FrameworkElement detailBorder = GetTemplatePart<FrameworkElement>(container, "PART_DetailBorder");
                TranslateTransform translateTransform = GetDetailTranslateTransform(detailBorder);

                Assert.Equal(Visibility.Collapsed, detailLayer.Visibility);
                Assert.Equal(0d, overlay.Opacity);
                Assert.Equal(0d, detailBorder.Opacity);
                Assert.Equal(-25d, translateTransform.Y);
            });
    }

    /// <summary>
    /// Verifies that opening the detail area updates the template parts immediately when animation is disabled.
    /// </summary>
    [Fact]
    public void IsDetailOpen_ShouldOpenDetailLayerWhenAnimationIsDisabled()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = CreateTemplatedContainer();
                container.EnableDetailAnimation = false;
                container.DetailAnimationOffset = 25d;
                container.OverlayOpacity = 0.33d;

                ApplyTemplate(container);
                container.IsDetailOpen = true;

                FrameworkElement detailLayer = GetTemplatePart<FrameworkElement>(container, "PART_DetailLayer");
                FrameworkElement overlay = GetTemplatePart<FrameworkElement>(container, "PART_Overlay");
                FrameworkElement detailBorder = GetTemplatePart<FrameworkElement>(container, "PART_DetailBorder");
                TranslateTransform translateTransform = GetDetailTranslateTransform(detailBorder);

                Assert.Equal(Visibility.Visible, detailLayer.Visibility);
                Assert.Equal(0.33d, overlay.Opacity);
                Assert.Equal(1d, detailBorder.Opacity);
                Assert.Equal(0d, translateTransform.Y);
            });
    }

    /// <summary>
    /// Verifies that closing the detail area updates the template parts immediately when animation is disabled.
    /// </summary>
    [Fact]
    public void IsDetailOpen_ShouldCloseDetailLayerWhenAnimationIsDisabled()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = CreateTemplatedContainer();
                container.EnableDetailAnimation = false;
                container.DetailAnimationOffset = 25d;
                container.OverlayOpacity = 0.33d;
                container.IsDetailOpen = true;

                ApplyTemplate(container);
                container.IsDetailOpen = false;

                FrameworkElement detailLayer = GetTemplatePart<FrameworkElement>(container, "PART_DetailLayer");
                FrameworkElement overlay = GetTemplatePart<FrameworkElement>(container, "PART_Overlay");
                FrameworkElement detailBorder = GetTemplatePart<FrameworkElement>(container, "PART_DetailBorder");
                TranslateTransform translateTransform = GetDetailTranslateTransform(detailBorder);

                Assert.Equal(Visibility.Collapsed, detailLayer.Visibility);
                Assert.Equal(0d, overlay.Opacity);
                Assert.Equal(0d, detailBorder.Opacity);
                Assert.Equal(-25d, translateTransform.Y);
            });
    }

    /// <summary>
    /// Verifies that the close button closes the detail area directly when no close command is configured.
    /// </summary>
    [Fact]
    public void CloseButton_ShouldCloseDetailWhenNoCommandIsConfigured()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = CreateTemplatedContainer();
                container.EnableDetailAnimation = false;
                container.IsDetailOpen = true;

                ApplyTemplate(container);
                ButtonBase closeButton = GetTemplatePart<ButtonBase>(container, "PART_CloseButton");

                closeButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, closeButton));

                Assert.False(container.IsDetailOpen);
            });
    }

    /// <summary>
    /// Verifies that the close button delegates closing to <see cref="VIA.WPF.Controls.XViewContainer.CloseDetailCommand" />.
    /// </summary>
    [Fact]
    public void CloseButton_ShouldExecuteCloseDetailCommandWithConfiguredParameter()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = CreateTemplatedContainer();
                TrackingCommand command = new();
                object commandParameter = new();
                container.EnableDetailAnimation = false;
                container.IsDetailOpen = true;
                container.CloseDetailCommand = command;
                container.CloseDetailCommandParameter = commandParameter;

                ApplyTemplate(container);
                ButtonBase closeButton = GetTemplatePart<ButtonBase>(container, "PART_CloseButton");

                closeButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, closeButton));

                Assert.Equal(1, command.ExecuteCount);
                Assert.Same(commandParameter, command.LastParameter);
                Assert.True(container.IsDetailOpen);
            });
    }

    /// <summary>
    /// Verifies that the container instance is used as fallback close command parameter.
    /// </summary>
    [Fact]
    public void CloseButton_ShouldUseContainerAsFallbackCommandParameter()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = CreateTemplatedContainer();
                TrackingCommand command = new();
                container.EnableDetailAnimation = false;
                container.IsDetailOpen = true;
                container.CloseDetailCommand = command;

                ApplyTemplate(container);
                ButtonBase closeButton = GetTemplatePart<ButtonBase>(container, "PART_CloseButton");

                closeButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, closeButton));

                Assert.Equal(1, command.ExecuteCount);
                Assert.Same(container, command.LastParameter);
                Assert.True(container.IsDetailOpen);
            });
    }

    /// <summary>
    /// Verifies that a close command that cannot execute leaves the detail area open.
    /// </summary>
    [Fact]
    public void CloseButton_ShouldKeepDetailOpenWhenCloseCommandCannotExecute()
    {
        WpfTestHelper.Run(
            () =>
            {
                VIA.WPF.Controls.XViewContainer container = CreateTemplatedContainer();
                TrackingCommand command = new()
                {
                    CanExecuteResult = false
                };
                container.EnableDetailAnimation = false;
                container.IsDetailOpen = true;
                container.CloseDetailCommand = command;

                ApplyTemplate(container);
                ButtonBase closeButton = GetTemplatePart<ButtonBase>(container, "PART_CloseButton");

                closeButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, closeButton));

                Assert.Equal(0, command.ExecuteCount);
                Assert.True(container.IsDetailOpen);
            });
    }
    #endregion

    #region ### Private Methods ###
    private static VIA.WPF.Controls.XViewContainer CreateTemplatedContainer()
    {
        return new VIA.WPF.Controls.XViewContainer
        {
            Template = CreateTemplate()
        };
    }

    private static ControlTemplate CreateTemplate()
    {
        const string templateXaml = """
<ControlTemplate
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="clr-namespace:VIA.WPF.Controls;assembly=VIA.WPF.Controls"
    TargetType="{x:Type controls:XViewContainer}">
    <Grid
        x:Name="PART_DetailLayer"
        Visibility="Collapsed">
        <Border
            x:Name="PART_Overlay"
            Opacity="0" />
        <Border
            x:Name="PART_DetailBorder"
            Opacity="0">
            <Border.RenderTransform>
                <TranslateTransform Y="-18" />
            </Border.RenderTransform>
        </Border>
        <Button
            x:Name="PART_CloseButton" />
    </Grid>
</ControlTemplate>
""";

        return (ControlTemplate)XamlReader.Parse(templateXaml);
    }

    private static TranslateTransform GetDetailTranslateTransform(FrameworkElement detailBorder)
    {
        if (detailBorder.RenderTransform is TranslateTransform translateTransform)
        {
            return translateTransform;
        }

        TransformGroup transformGroup = Assert.IsType<TransformGroup>(detailBorder.RenderTransform);

        foreach (Transform transform in transformGroup.Children)
        {
            if (transform is TranslateTransform currentTranslateTransform)
            {
                return currentTranslateTransform;
            }
        }

        throw new InvalidOperationException("The detail border render transform does not contain a TranslateTransform.");
    }

    private static void ApplyTemplate(Control control)
    {
        control.ApplyTemplate();
    }

    private static T GetTemplatePart<T>(Control control, string partName)
    where T : DependencyObject
    {
        object? part = control.Template.FindName(partName, control);

        Assert.NotNull(part);

        T typedPart = Assert.IsType<T>(part, exactMatch: false);
        return typedPart;
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TrackingCommand : ICommand
    {
        #region ### Public Events ###
        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged;
        #endregion

        #region ### Public Properties ###
        /// <summary>
        /// Gets or sets a value indicating whether the command can execute.
        /// </summary>
        public bool CanExecuteResult { get; set; } = true;

        /// <summary>
        /// Gets the number of command executions.
        /// </summary>
        public int ExecuteCount { get; private set; }

        /// <summary>
        /// Gets the last command parameter.
        /// </summary>
        public object? LastParameter { get; private set; }
        #endregion

        #region ### Public Methods ###
        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            return this.CanExecuteResult;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            this.ExecuteCount++;
            this.LastParameter = parameter;
        }
        #endregion

        #region ### Internal Methods ###
        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        internal void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
    #endregion
}
#endregion
