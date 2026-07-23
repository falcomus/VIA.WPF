// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTreeViewNodeDragDropController.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace VIA.WPF.Controls;

#region ### Class XTreeViewNodeDragDropController ###
/// <summary>
/// Provides native WPF drag-and-drop handling for <see cref="XTreeView"/> nodes.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="XTreeViewNodeDragDropController"/> class.
/// </remarks>
/// <param name="owner">The owning tree view.</param>
internal sealed class XTreeViewNodeDragDropController(XTreeView owner)
{
    #region ### Constants ###
    /// <summary>
    /// The custom data format used for native XTreeView node drags.
    /// </summary>
    private const string NodeDataFormat = "VIA.WPF.Controls.XTreeView.Node";

    /// <summary>
    /// The custom data format used for the source container of native XTreeView node drags.
    /// </summary>
    private const string SourceContainerDataFormat = "VIA.WPF.Controls.XTreeView.Node.SourceContainer";

    /// <summary>
    /// The distance from the top or bottom edge where automatic scrolling starts during node drags.
    /// </summary>
    private const double AutoScrollEdgeSize = 36d;

    /// <summary>
    /// The smallest automatic scroll step during node drags.
    /// </summary>
    private const double AutoScrollMinimumStep = 3d;

    /// <summary>
    /// The largest automatic scroll step during node drags.
    /// </summary>
    private const double AutoScrollMaximumStep = 24d;

    /// <summary>
    /// The automatic scroll timer interval in milliseconds.
    /// </summary>
    private const int AutoScrollIntervalMilliseconds = 35;

    /// <summary>
    /// The maximum vertical gap in which the nearest visible node header is still used as drop target.
    /// </summary>
    private const double HeaderGapTargetTolerance = 24d;

    /// <summary>
    /// The visual child indentation used by the XTreeViewItem template.
    /// </summary>
    private const double ChildIndent = 18d;
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The owning tree view.
    /// </summary>
    private readonly XTreeView owner = owner;

    /// <summary>
    /// The mouse position where the pending drag was armed.
    /// </summary>
    private Point dragStartPoint;

    /// <summary>
    /// The currently armed drag source item container.
    /// </summary>
    private XTreeViewItem? dragStartContainer;

    /// <summary>
    /// The currently armed drag source data item.
    /// </summary>
    private object? dragStartItem;

    /// <summary>
    /// The current drop target adorner.
    /// </summary>
    private XTreeViewDropTargetAdorner? dropTargetAdorner;

    /// <summary>
    /// The overlay host that currently owns the drop target adorner.
    /// </summary>
    private Panel? dropTargetAdornerHost;

    /// <summary>
    /// The timer used for automatic tree scrolling during node drags.
    /// </summary>
    private DispatcherTimer? autoScrollTimer;

    /// <summary>
    /// The latest drag position relative to the owning tree view.
    /// </summary>
    private Point lastDragPosition;

    /// <summary>
    /// The latest valid drop information used to repaint the drop adorner after automatic scrolling.
    /// </summary>
    private XTreeViewNodeDropInfo? lastDropAdornerInfo;

    /// <summary>
    /// Indicates whether a valid drag position is currently known.
    /// </summary>
    private bool hasLastDragPosition;

    /// <summary>
    /// Indicates whether the controller is currently enabled and attached.
    /// </summary>
    private bool isEnabled;
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Enables or disables native node drag-and-drop handling.
    /// </summary>
    /// <param name="enabled">A value indicating whether node drag-and-drop should be enabled.</param>
    public void SetEnabled(bool enabled)
    {
        if (this.isEnabled == enabled)
        {
            return;
        }

        this.isEnabled = enabled;

        if (enabled)
        {
            this.Attach();
            return;
        }

        this.Detach();
        this.ClearPendingDrag();
        this.ClearDropTargetAdorner();
        this.owner.EndNodeDragVisualState();
    }

    /// <summary>
    /// Refreshes the active drop target adorner after visual configuration changed.
    /// </summary>
    public void RefreshDropTargetAdorner()
    {
        if (this.lastDropAdornerInfo is not null && this.hasLastDragPosition)
        {
            this.ShowDropTargetAdorner(this.lastDropAdornerInfo, this.lastDragPosition);
            return;
        }

        this.dropTargetAdorner?.InvalidateVisual();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Attaches the native routed event handlers.
    /// </summary>
    private void Attach()
    {
        this.owner.PreviewMouseLeftButtonDown += this.OnPreviewMouseLeftButtonDown;
        this.owner.PreviewMouseMove += this.OnPreviewMouseMove;
        this.owner.PreviewDragEnter += this.OnPreviewDragOver;
        this.owner.PreviewDragOver += this.OnPreviewDragOver;
        this.owner.PreviewDragLeave += this.OnPreviewDragLeave;
        this.owner.PreviewDrop += this.OnPreviewDrop;
    }

    /// <summary>
    /// Detaches the native routed event handlers.
    /// </summary>
    private void Detach()
    {
        this.owner.PreviewMouseLeftButtonDown -= this.OnPreviewMouseLeftButtonDown;
        this.owner.PreviewMouseMove -= this.OnPreviewMouseMove;
        this.owner.PreviewDragEnter -= this.OnPreviewDragOver;
        this.owner.PreviewDragOver -= this.OnPreviewDragOver;
        this.owner.PreviewDragLeave -= this.OnPreviewDragLeave;
        this.owner.PreviewDrop -= this.OnPreviewDrop;
    }

    /// <summary>
    /// Handles the mouse-down event used to arm a possible node drag.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs eventArgs)
    {
        this.ClearPendingDrag();

        if (!this.CanProcessDragDrop() || eventArgs.OriginalSource is not DependencyObject source || IsInteractiveElement(source))
        {
            return;
        }

        XTreeViewItem? sourceContainer = FindAncestor<XTreeViewItem>(source);
        object? sourceItem = GetDataItem(sourceContainer);

        if (sourceContainer is null || sourceItem is null)
        {
            return;
        }

        this.dragStartPoint = eventArgs.GetPosition(this.owner);
        this.dragStartContainer = sourceContainer;
        this.dragStartItem = sourceItem;
    }

    /// <summary>
    /// Handles the mouse-move event used to start a native node drag.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnPreviewMouseMove(object sender, MouseEventArgs eventArgs)
    {
        if (eventArgs.LeftButton != MouseButtonState.Pressed || this.dragStartContainer is null || this.dragStartItem is null)
        {
            this.ClearPendingDragIfMouseReleased(eventArgs);
            return;
        }

        Point currentPosition = eventArgs.GetPosition(this.owner);

        if (Math.Abs(currentPosition.X - this.dragStartPoint.X) < SystemParameters.MinimumHorizontalDragDistance &&
            Math.Abs(currentPosition.Y - this.dragStartPoint.Y) < SystemParameters.MinimumVerticalDragDistance)
        {
            return;
        }

        XTreeViewItem sourceContainer = this.dragStartContainer;
        object sourceItem = this.dragStartItem;

        this.ClearPendingDrag();
        this.StartNativeDrag(sourceContainer, sourceItem);
    }

    /// <summary>
    /// Starts the native WPF drag operation.
    /// </summary>
    /// <param name="sourceContainer">The source item container.</param>
    /// <param name="sourceItem">The source data item.</param>
    private void StartNativeDrag(XTreeViewItem sourceContainer, object sourceItem)
    {
        if (!this.CanProcessDragDrop())
        {
            return;
        }

        DataObject dataObject = new();
        dataObject.SetData(NodeDataFormat, sourceItem);
        dataObject.SetData(SourceContainerDataFormat, sourceContainer);

        try
        {
            this.owner.BeginNodeDragVisualState();
            _ = DragDrop.DoDragDrop(sourceContainer, dataObject, DragDropEffects.Move);
        }
        finally
        {
            this.StopAutoScroll();
            this.ClearDropTargetAdorner();
            this.owner.EndNodeDragVisualState();
        }
    }

    /// <summary>
    /// Handles native drag-over events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnPreviewDragOver(object sender, DragEventArgs eventArgs)
    {
        Point ownerPosition = eventArgs.GetPosition(this.owner);

        if (!this.TryCreateDropInfo(eventArgs, out XTreeViewNodeDropInfo? treeDropInfo))
        {
            eventArgs.Effects = DragDropEffects.None;
            this.ClearDropTargetAdorner();
            this.UpdateAutoScrollState(ownerPosition, false);
            eventArgs.Handled = true;
            return;
        }

        eventArgs.Effects = DragDropEffects.Move;
        this.owner.BeginNodeDragVisualState();
        this.UpdateAutoScrollState(ownerPosition, true);
        this.ShowDropTargetAdorner(treeDropInfo, ownerPosition);
        eventArgs.Handled = true;
    }

    /// <summary>
    /// Handles native drag-leave events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnPreviewDragLeave(object sender, DragEventArgs eventArgs)
    {
        if (!this.owner.IsMouseOver)
        {
            this.StopAutoScroll();
            this.ClearDropTargetAdorner();
            this.owner.EndNodeDragVisualState();
        }
    }

    /// <summary>
    /// Handles native drop events.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnPreviewDrop(object sender, DragEventArgs eventArgs)
    {
        this.StopAutoScroll();
        this.ClearDropTargetAdorner();

        IXTreeViewNodeDropHandler? nodeDropHandler = this.owner.EffectiveNodeDropHandler;

        if (nodeDropHandler is null ||
            !this.TryCreateDropInfo(eventArgs, out XTreeViewNodeDropInfo? treeDropInfo) ||
            !nodeDropHandler.CanDrop(treeDropInfo))
        {
            eventArgs.Effects = DragDropEffects.None;
            eventArgs.Handled = true;
            this.owner.EndNodeDragVisualState();
            return;
        }

        eventArgs.Effects = DragDropEffects.Move;
        eventArgs.Handled = true;

        _ = this.owner.Dispatcher.BeginInvoke(
            DispatcherPriority.ContextIdle,
            new Action(() => this.ExecuteDrop(nodeDropHandler, treeDropInfo)));
    }


    /// <summary>
    /// Determines whether two values represent the same tree item.
    /// </summary>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <returns><c>true</c> if both values represent the same item; otherwise, <c>false</c>.</returns>
    private static bool AreSameItem(object? first, object? second)
    {
        return ReferenceEquals(first, second) || first is not null && first.Equals(second);
    }

    /// <summary>
    /// Executes the specified drop operation after the current WPF drag-and-drop event has completed.
    /// </summary>
    /// <param name="nodeDropHandler">The drop handler that accepted the operation.</param>
    /// <param name="treeDropInfo">The captured drop information.</param>
    private void ExecuteDrop(IXTreeViewNodeDropHandler nodeDropHandler, XTreeViewNodeDropInfo treeDropInfo)
    {
        try
        {
            if (!this.owner.AllowNodeDragDrop || !nodeDropHandler.CanDrop(treeDropInfo))
            {
                return;
            }

            object? selectedDataItemBeforeDrop = this.owner.SelectedDataItem;

            nodeDropHandler.Drop(treeDropInfo);

            if (!AreSameItem(selectedDataItemBeforeDrop, this.owner.SelectedDataItem))
            {
                if (this.owner.SelectedDataItem is not null)
                {
                    this.owner.SelectDroppedDataItemInternal(this.owner.SelectedDataItem);
                }

                return;
            }

            this.owner.SelectDroppedDataItemInternal(treeDropInfo.DraggedItem);
        }
        finally
        {
            this.owner.EndNodeDragVisualState();
        }
    }

    /// <summary>
    /// Tries to create VIA.WPF drop information from native WPF drop information.
    /// </summary>
    /// <param name="eventArgs">The native drag event data.</param>
    /// <param name="treeDropInfo">The created VIA.WPF drop information.</param>
    /// <returns><c>true</c> if the information could be created and is valid; otherwise, <c>false</c>.</returns>
    private bool TryCreateDropInfo(
        DragEventArgs eventArgs,
        [NotNullWhen(true)] out XTreeViewNodeDropInfo? treeDropInfo)
    {
        treeDropInfo = null;

        IXTreeViewNodeDropHandler? nodeDropHandler = this.owner.EffectiveNodeDropHandler;
        if (!this.CanProcessDragDrop() || nodeDropHandler is null || !TryGetDraggedItem(eventArgs.Data, out object? draggedItem))
        {
            return false;
        }

        XTreeViewItem? targetContainer = this.FindTargetContainer(eventArgs);
        object? targetItem = GetDataItem(targetContainer);
        XTreeViewNodeDropPosition position = targetContainer is null
            ? XTreeViewNodeDropPosition.Root
            : this.GetDropPosition(targetContainer, eventArgs);

        if (draggedItem is null || ReferenceEquals(draggedItem, targetItem))
        {
            return false;
        }

        if (!IsPositionAllowed(this.owner.NodeDropMode, position))
        {
            return false;
        }

        XTreeViewItem? draggedContainer = TryGetDraggedContainer(eventArgs.Data) ?? this.owner.FindContainerForDataItemInternal(draggedItem);

        if (draggedContainer is not null &&
            targetContainer is not null &&
            IsVisualAncestor(draggedContainer, targetContainer))
        {
            return false;
        }

        treeDropInfo = new XTreeViewNodeDropInfo
        {
            DraggedItem = draggedItem,
            TargetItem = targetItem,
            Position = position,
            TreeView = this.owner,
            DraggedContainer = draggedContainer,
            TargetContainer = targetContainer,
            OriginalDropInfo = eventArgs
        };

        return nodeDropHandler.CanDrop(treeDropInfo);
    }

    /// <summary>
    /// Shows or updates the drop target adorner.
    /// </summary>
    /// <param name="treeDropInfo">The current drop information.</param>
    /// <param name="mousePosition">The current mouse position relative to the owning tree view.</param>
    private void ShowDropTargetAdorner(XTreeViewNodeDropInfo treeDropInfo, Point mousePosition)
    {
        FrameworkElement? targetElement = this.GetAdornerTargetElement(treeDropInfo);
        Rect targetBounds = targetElement is not null
            ? GetBoundsRelativeToOwner(targetElement, this.owner)
            : this.GetRootDropBounds(mousePosition);

        if (targetBounds.IsEmpty)
        {
            this.ClearDropTargetAdorner();
            return;
        }

        Panel? adornerHost = this.GetDropTargetAdornerHost();
        if (adornerHost is null)
        {
            return;
        }

        if (this.dropTargetAdorner is null || !ReferenceEquals(this.dropTargetAdornerHost, adornerHost))
        {
            this.ClearDropTargetAdorner();
            this.dropTargetAdorner = new XTreeViewDropTargetAdorner(this.owner);
            this.dropTargetAdornerHost = adornerHost;
            adornerHost.Children.Add(this.dropTargetAdorner);
        }

        string? hintText = this.BuildDropHintText(treeDropInfo);
        this.dropTargetAdorner.Update(treeDropInfo.Position, targetBounds, hintText);
        this.lastDropAdornerInfo = treeDropInfo;
    }

    /// <summary>
    /// Builds the drop target hint text.
    /// </summary>
    /// <param name="treeDropInfo">The current drop information.</param>
    /// <returns>The hint text or <see langword="null"/>.</returns>
    private string? BuildDropHintText(XTreeViewNodeDropInfo treeDropInfo)
    {
        if (!this.owner.UseDropTargetHint)
        {
            return null;
        }

        string targetText = this.GetDisplayText(treeDropInfo.TargetItem);
        if (string.IsNullOrWhiteSpace(targetText))
        {
            return treeDropInfo.Position == XTreeViewNodeDropPosition.Root
                ? this.owner.DropHintRootText
                : null;
        }

        return treeDropInfo.Position switch
        {
            XTreeViewNodeDropPosition.Before => $"{this.owner.DropHintBeforeText}: {targetText}",
            XTreeViewNodeDropPosition.Into => $"{this.owner.DropHintIntoText}: {targetText}",
            XTreeViewNodeDropPosition.After => $"{this.owner.DropHintAfterText}: {targetText}",
            XTreeViewNodeDropPosition.Root => this.owner.DropHintRootText,
            _ => targetText
        };
    }

    private string GetDisplayText(object? item)
    {
        if (item is null)
        {
            return string.Empty;
        }

        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);

        foreach (string propertyName in new[] { "Name", "Title", "DisplayName", "Code" })
        {
            PropertyDescriptor? property = properties.Find(propertyName, false);
            object? value = property?.GetValue(item);

            if (value is string text && !string.IsNullOrWhiteSpace(text))
            {
                return text;
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Clears the active drop target adorner.
    /// </summary>
    private void ClearDropTargetAdorner()
    {
        if (this.dropTargetAdorner is null)
        {
            return;
        }

        this.dropTargetAdornerHost?.Children.Remove(this.dropTargetAdorner);
        this.dropTargetAdorner = null;
        this.dropTargetAdornerHost = null;
        this.lastDropAdornerInfo = null;
    }

    /// <summary>
    /// Gets the template overlay host used for native drop target feedback.
    /// </summary>
    /// <returns>The overlay host or <see langword="null"/>.</returns>
    private Panel? GetDropTargetAdornerHost()
    {
        this.owner.ApplyTemplate();
        return this.owner.Template.FindName("PART_DropTargetAdornerHost", this.owner) as Panel;
    }

    /// <summary>
    /// Gets the element that should be highlighted or used for insert-line placement.
    /// </summary>
    /// <param name="treeDropInfo">The current drop information.</param>
    /// <returns>The target element or <see langword="null"/>.</returns>
    private FrameworkElement? GetAdornerTargetElement(XTreeViewNodeDropInfo treeDropInfo)
    {
        XTreeViewItem? targetContainer = treeDropInfo.TargetContainer;
        if (targetContainer is null)
        {
            return null;
        }

        targetContainer.ApplyTemplate();
        return targetContainer.Template.FindName("HeaderSelectionBorder", targetContainer) as FrameworkElement ?? targetContainer;
    }

    /// <summary>
    /// Gets a root drop adorner rectangle for drops below realized items.
    /// </summary>
    /// <param name="mousePosition">The current mouse position relative to the owning tree view.</param>
    /// <returns>The root drop rectangle.</returns>
    private Rect GetRootDropBounds(Point mousePosition)
    {
        if (!this.owner.ShowAlwaysDropTargetAdorner)
        {
            return Rect.Empty;
        }

        double width = Math.Max(0d, this.owner.ActualWidth);
        double y = Math.Min(Math.Max(0d, mousePosition.Y), Math.Max(0d, this.owner.ActualHeight));
        return new Rect(0d, y, width, 0d);
    }

    /// <summary>
    /// Finds the visible node header that is currently used as native drop target.
    /// </summary>
    /// <param name="eventArgs">The native drag event data.</param>
    /// <returns>The target container or <see langword="null"/> for root drops.</returns>
    private XTreeViewItem? FindTargetContainer(DragEventArgs eventArgs)
    {
        Point ownerPosition = eventArgs.GetPosition(this.owner);
        List<(XTreeViewItem Container, Rect Bounds)> headerTargets = [];

        this.CollectVisibleHeaderTargets(this.owner, headerTargets);
        if (headerTargets.Count == 0)
        {
            return null;
        }

        foreach ((XTreeViewItem container, Rect bounds) in headerTargets)
        {
            if (this.CreateFullWidthRowBounds(bounds).Contains(ownerPosition))
            {
                return container;
            }
        }

        return this.FindNearestGapTarget(headerTargets, ownerPosition);
    }

    /// <summary>
    /// Collects realized visible node headers in visual order.
    /// </summary>
    /// <param name="parent">The current item parent.</param>
    /// <param name="headerTargets">The collected header targets.</param>
    private void CollectVisibleHeaderTargets(ItemsControl parent, IList<(XTreeViewItem Container, Rect Bounds)> headerTargets)
    {
        foreach (object item in parent.Items)
        {
            if (parent.ItemContainerGenerator.ContainerFromItem(item) is not XTreeViewItem container)
            {
                continue;
            }

            container.SetOwnerTreeViewInternal(this.owner);

            FrameworkElement headerElement = this.GetHeaderElement(container);
            Rect headerBounds = GetBoundsRelativeToOwner(headerElement, this.owner);
            if (!headerBounds.IsEmpty)
            {
                headerTargets.Add((container, headerBounds));
            }

            if (container.IsExpanded)
            {
                this.CollectVisibleHeaderTargets(container, headerTargets);
            }
        }
    }

    /// <summary>
    /// Finds the nearest target for small gaps between visible node headers.
    /// </summary>
    /// <param name="headerTargets">The visible header targets.</param>
    /// <param name="ownerPosition">The mouse position relative to the owning tree view.</param>
    /// <returns>The nearest gap target or <see langword="null"/>.</returns>
    private XTreeViewItem? FindNearestGapTarget(IReadOnlyList<(XTreeViewItem Container, Rect Bounds)> headerTargets, Point ownerPosition)
    {
        (XTreeViewItem Container, Rect Bounds)? aboveTarget = null;
        (XTreeViewItem Container, Rect Bounds)? belowTarget = null;

        foreach ((XTreeViewItem container, Rect bounds) in headerTargets)
        {
            if (bounds.Bottom <= ownerPosition.Y)
            {
                aboveTarget = (container, bounds);
                continue;
            }

            if (bounds.Top >= ownerPosition.Y)
            {
                belowTarget = (container, bounds);
                break;
            }
        }

        double aboveDistance = aboveTarget is null ? double.PositiveInfinity : ownerPosition.Y - aboveTarget.Value.Bounds.Bottom;
        double belowDistance = belowTarget is null ? double.PositiveInfinity : belowTarget.Value.Bounds.Top - ownerPosition.Y;

        if (aboveDistance > HeaderGapTargetTolerance && belowDistance > HeaderGapTargetTolerance)
        {
            return null;
        }

        if (aboveTarget is not null && belowTarget is not null)
        {
            bool prefersAboveLevel = ownerPosition.X >= aboveTarget.Value.Bounds.Left - ChildIndent / 2d;
            bool prefersBelowLevel = ownerPosition.X >= belowTarget.Value.Bounds.Left - ChildIndent / 2d;

            if (aboveTarget.Value.Bounds.Left > belowTarget.Value.Bounds.Left && prefersAboveLevel)
            {
                return aboveTarget.Value.Container;
            }

            if (belowTarget.Value.Bounds.Left > aboveTarget.Value.Bounds.Left && prefersBelowLevel)
            {
                return belowTarget.Value.Container;
            }

            return aboveDistance <= belowDistance
                ? aboveTarget.Value.Container
                : belowTarget.Value.Container;
        }

        if (aboveTarget is not null && aboveDistance <= HeaderGapTargetTolerance)
        {
            return aboveTarget.Value.Container;
        }

        return belowTarget is not null && belowDistance <= HeaderGapTargetTolerance
            ? belowTarget.Value.Container
            : null;
    }

    /// <summary>
    /// Creates a full-width row rectangle from a visible header rectangle.
    /// </summary>
    /// <param name="headerBounds">The header bounds.</param>
    /// <returns>The full-width row bounds.</returns>
    private Rect CreateFullWidthRowBounds(Rect headerBounds)
    {
        return new Rect(
            0d,
            headerBounds.Top,
            Math.Max(this.owner.ActualWidth, headerBounds.Right),
            headerBounds.Height);
    }

    /// <summary>
    /// Determines the requested drop position for the specified target container.
    /// </summary>
    /// <param name="targetContainer">The target container.</param>
    /// <param name="eventArgs">The native drag event data.</param>
    /// <returns>The requested drop position.</returns>
    private XTreeViewNodeDropPosition GetDropPosition(XTreeViewItem targetContainer, DragEventArgs eventArgs)
    {
        FrameworkElement targetElement = this.GetHeaderElement(targetContainer);
        Point elementPosition = eventArgs.GetPosition(targetElement);
        double height = Math.Max(1d, targetElement.ActualHeight);
        double y = Math.Min(Math.Max(0d, elementPosition.Y), height);

        return this.owner.NodeDropMode switch
        {
            XTreeViewNodeDropMode.Before => XTreeViewNodeDropPosition.Before,
            XTreeViewNodeDropMode.After => XTreeViewNodeDropPosition.After,
            XTreeViewNodeDropMode.Into => XTreeViewNodeDropPosition.Into,
            XTreeViewNodeDropMode.BeforeAfter => y < height / 2d ? XTreeViewNodeDropPosition.Before : XTreeViewNodeDropPosition.After,
            XTreeViewNodeDropMode.BeforeInto => y < height * 0.35d ? XTreeViewNodeDropPosition.Before : XTreeViewNodeDropPosition.Into,
            XTreeViewNodeDropMode.AfterInto => y > height * 0.65d ? XTreeViewNodeDropPosition.After : XTreeViewNodeDropPosition.Into,
            XTreeViewNodeDropMode.BeforeAfterInto => y < height * 0.30d
                ? XTreeViewNodeDropPosition.Before
                : y > height * 0.70d
                    ? XTreeViewNodeDropPosition.After
                    : XTreeViewNodeDropPosition.Into,
            _ => XTreeViewNodeDropPosition.Root
        };
    }

    /// <summary>
    /// Gets the header element of the specified target container.
    /// </summary>
    /// <param name="targetContainer">The target container.</param>
    /// <returns>The header element.</returns>
    private FrameworkElement GetHeaderElement(XTreeViewItem targetContainer)
    {
        targetContainer.ApplyTemplate();
        return targetContainer.Template.FindName("HeaderSelectionBorder", targetContainer) as FrameworkElement ?? targetContainer;
    }

    /// <summary>
    /// Gets a value indicating whether native node drag-and-drop can currently be processed.
    /// </summary>
    /// <returns><c>true</c> if processing is allowed; otherwise, <c>false</c>.</returns>
    private bool CanProcessDragDrop()
    {
        return this.isEnabled && this.owner.AllowNodeDragDrop && this.owner.EffectiveNodeDropHandler is not null;
    }

    /// <summary>
    /// Clears the pending drag if the mouse button is not pressed anymore.
    /// </summary>
    /// <param name="eventArgs">The mouse event data.</param>
    private void ClearPendingDragIfMouseReleased(MouseEventArgs eventArgs)
    {
        if (eventArgs.LeftButton != MouseButtonState.Pressed)
        {
            this.ClearPendingDrag();
        }
    }

    /// <summary>
    /// Clears the pending drag source information.
    /// </summary>
    private void ClearPendingDrag()
    {
        this.dragStartContainer = null;
        this.dragStartItem = null;
        this.dragStartPoint = default;
    }

    /// <summary>
    /// Updates automatic scrolling for the current drag position.
    /// </summary>
    /// <param name="ownerPosition">The current drag position relative to the owning tree view.</param>
    /// <param name="canScroll">A value indicating whether automatic scrolling is currently allowed.</param>
    private void UpdateAutoScrollState(Point ownerPosition, bool canScroll)
    {
        this.lastDragPosition = ownerPosition;
        this.hasLastDragPosition = canScroll;

        if (!canScroll || !this.IsInsideAutoScrollArea(ownerPosition))
        {
            this.StopAutoScroll();
            return;
        }

        this.EnsureAutoScrollTimer().Start();
    }

    /// <summary>
    /// Ensures that the automatic scrolling timer exists.
    /// </summary>
    /// <returns>The automatic scrolling timer.</returns>
    private DispatcherTimer EnsureAutoScrollTimer()
    {
        if (this.autoScrollTimer is not null)
        {
            return this.autoScrollTimer;
        }

        this.autoScrollTimer = new DispatcherTimer(DispatcherPriority.Input, this.owner.Dispatcher)
        {
            Interval = TimeSpan.FromMilliseconds(AutoScrollIntervalMilliseconds)
        };

        this.autoScrollTimer.Tick += this.OnAutoScrollTimerTick;
        return this.autoScrollTimer;
    }

    /// <summary>
    /// Stops automatic scrolling.
    /// </summary>
    private void StopAutoScroll()
    {
        this.hasLastDragPosition = false;
        this.autoScrollTimer?.Stop();
    }

    /// <summary>
    /// Handles automatic scrolling timer ticks.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="eventArgs">The event data.</param>
    private void OnAutoScrollTimerTick(object? sender, EventArgs eventArgs)
    {
        if (!this.hasLastDragPosition || !this.IsInsideAutoScrollArea(this.lastDragPosition))
        {
            this.StopAutoScroll();
            return;
        }

        this.ScrollTreeByDragPosition(this.lastDragPosition);
    }

    /// <summary>
    /// Gets a value indicating whether the specified drag position is inside the automatic scroll edge areas.
    /// </summary>
    /// <param name="ownerPosition">The current drag position relative to the owning tree view.</param>
    /// <returns><c>true</c> if automatic scrolling should run; otherwise, <c>false</c>.</returns>
    private bool IsInsideAutoScrollArea(Point ownerPosition)
    {
        double height = Math.Max(0d, this.owner.ActualHeight);
        return ownerPosition.Y >= 0d && ownerPosition.Y <= height &&
            (ownerPosition.Y < AutoScrollEdgeSize || ownerPosition.Y > height - AutoScrollEdgeSize);
    }

    /// <summary>
    /// Scrolls the tree according to the current drag position.
    /// </summary>
    /// <param name="ownerPosition">The current drag position relative to the owning tree view.</param>
    private void ScrollTreeByDragPosition(Point ownerPosition)
    {
        ScrollViewer? scrollViewer = this.GetScrollViewer();
        if (scrollViewer is null || scrollViewer.ScrollableHeight <= 0d)
        {
            this.StopAutoScroll();
            return;
        }

        double height = Math.Max(0d, this.owner.ActualHeight);
        double delta = 0d;

        if (ownerPosition.Y < AutoScrollEdgeSize)
        {
            delta = -CalculateAutoScrollStep(AutoScrollEdgeSize - ownerPosition.Y);
        }
        else if (ownerPosition.Y > height - AutoScrollEdgeSize)
        {
            delta = CalculateAutoScrollStep(ownerPosition.Y - (height - AutoScrollEdgeSize));
        }

        if (Math.Abs(delta) < 0.1d)
        {
            return;
        }

        double currentOffset = scrollViewer.VerticalOffset;
        double targetOffset = Math.Min(Math.Max(0d, currentOffset + delta), scrollViewer.ScrollableHeight);

        if (Math.Abs(targetOffset - currentOffset) < 0.1d)
        {
            return;
        }

        scrollViewer.ScrollToVerticalOffset(targetOffset);
        scrollViewer.UpdateLayout();

        if (this.lastDropAdornerInfo is not null)
        {
            this.ShowDropTargetAdorner(this.lastDropAdornerInfo, ownerPosition);
        }
    }

    /// <summary>
    /// Calculates the automatic scroll step for the specified edge distance.
    /// </summary>
    /// <param name="distance">The distance inside the automatic scroll edge area.</param>
    /// <returns>The scroll step.</returns>
    private static double CalculateAutoScrollStep(double distance)
    {
        double ratio = Math.Min(1d, Math.Max(0d, distance / AutoScrollEdgeSize));
        return AutoScrollMinimumStep + (AutoScrollMaximumStep - AutoScrollMinimumStep) * ratio;
    }

    /// <summary>
    /// Gets the template scroll viewer used by the owning tree view.
    /// </summary>
    /// <returns>The scroll viewer or <see langword="null"/>.</returns>
    private ScrollViewer? GetScrollViewer()
    {
        this.owner.ApplyTemplate();
        return this.owner.Template.FindName("PART_ScrollViewer", this.owner) as ScrollViewer ??
            FindDescendant<ScrollViewer>(this.owner);
    }

    /// <summary>
    /// Tries to get the dragged data item from the native data object.
    /// </summary>
    /// <param name="dataObject">The native data object.</param>
    /// <param name="draggedItem">The dragged item.</param>
    /// <returns><c>true</c> if the item could be read; otherwise, <c>false</c>.</returns>
    private static bool TryGetDraggedItem(IDataObject dataObject, [NotNullWhen(true)] out object? draggedItem)
    {
        draggedItem = null;

        if (!dataObject.GetDataPresent(NodeDataFormat))
        {
            return false;
        }

        draggedItem = NormalizeDraggedItem(dataObject.GetData(NodeDataFormat));
        return draggedItem is not null;
    }

    /// <summary>
    /// Tries to get the dragged source container from the native data object.
    /// </summary>
    /// <param name="dataObject">The native data object.</param>
    /// <returns>The source container or <see langword="null"/>.</returns>
    private static XTreeViewItem? TryGetDraggedContainer(IDataObject dataObject)
    {
        return dataObject.GetDataPresent(SourceContainerDataFormat)
            ? dataObject.GetData(SourceContainerDataFormat) as XTreeViewItem
            : null;
    }

    /// <summary>
    /// Normalizes the dragged data object.
    /// </summary>
    /// <param name="data">The raw drag data.</param>
    /// <returns>The first dragged item or <see langword="null"/>.</returns>
    private static object? NormalizeDraggedItem(object? data)
    {
        if (data is null or string)
        {
            return data;
        }

        if (data is TreeViewItem treeViewItem)
        {
            return GetDataItem(treeViewItem);
        }

        if (data is IEnumerable enumerable)
        {
            foreach (object? item in enumerable)
            {
                return NormalizeDraggedItem(item);
            }

            return null;
        }

        return data;
    }

    /// <summary>
    /// Gets the data item represented by the specified tree view item.
    /// </summary>
    /// <param name="treeViewItem">The tree view item.</param>
    /// <returns>The represented data item or <see langword="null"/>.</returns>
    private static object? GetDataItem(TreeViewItem? treeViewItem)
    {
        return treeViewItem is null
            ? null
            : treeViewItem.DataContext ?? treeViewItem.Header;
    }

    /// <summary>
    /// Gets a value indicating whether the specified position is allowed by the configured mode.
    /// </summary>
    /// <param name="mode">The configured mode.</param>
    /// <param name="position">The requested position.</param>
    /// <returns><c>true</c> if the position is allowed; otherwise, <c>false</c>.</returns>
    private static bool IsPositionAllowed(XTreeViewNodeDropMode mode, XTreeViewNodeDropPosition position)
    {
        return mode switch
        {
            XTreeViewNodeDropMode.Before => position == XTreeViewNodeDropPosition.Before,
            XTreeViewNodeDropMode.After => position == XTreeViewNodeDropPosition.After,
            XTreeViewNodeDropMode.Into => position is XTreeViewNodeDropPosition.Into or XTreeViewNodeDropPosition.Root,
            XTreeViewNodeDropMode.BeforeAfter => position is XTreeViewNodeDropPosition.Before or XTreeViewNodeDropPosition.After,
            XTreeViewNodeDropMode.BeforeInto => position is XTreeViewNodeDropPosition.Before or XTreeViewNodeDropPosition.Into or XTreeViewNodeDropPosition.Root,
            XTreeViewNodeDropMode.AfterInto => position is XTreeViewNodeDropPosition.After or XTreeViewNodeDropPosition.Into or XTreeViewNodeDropPosition.Root,
            XTreeViewNodeDropMode.BeforeAfterInto => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets a value indicating whether the potential ancestor visually contains the potential descendant.
    /// </summary>
    /// <param name="potentialAncestor">The potential ancestor.</param>
    /// <param name="potentialDescendant">The potential descendant.</param>
    /// <returns><c>true</c> if the ancestor contains the descendant; otherwise, <c>false</c>.</returns>
    private static bool IsVisualAncestor(DependencyObject potentialAncestor, DependencyObject potentialDescendant)
    {
        DependencyObject? current = potentialDescendant;

        while (current is not null)
        {
            if (ReferenceEquals(current, potentialAncestor))
            {
                return true;
            }

            current = GetParent(current);
        }

        return false;
    }

    /// <summary>
    /// Finds the first visual descendant of the specified type.
    /// </summary>
    /// <typeparam name="T">The descendant type.</typeparam>
    /// <param name="dependencyObject">The start object.</param>
    /// <returns>The matching descendant or <see langword="null"/>.</returns>
    private static T? FindDescendant<T>(DependencyObject dependencyObject)
        where T : DependencyObject
    {
        int childCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
        for (int childIndex = 0; childIndex < childCount; childIndex++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, childIndex);
            if (child is T typedChild)
            {
                return typedChild;
            }

            T? descendant = FindDescendant<T>(child);
            if (descendant is not null)
            {
                return descendant;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds the first visual or logical ancestor of the specified type.
    /// </summary>
    /// <typeparam name="T">The ancestor type.</typeparam>
    /// <param name="dependencyObject">The start object.</param>
    /// <returns>The matching ancestor or <see langword="null"/>.</returns>
    private static T? FindAncestor<T>(DependencyObject? dependencyObject)
        where T : DependencyObject
    {
        while (dependencyObject is not null)
        {
            if (dependencyObject is T typedObject)
            {
                return typedObject;
            }

            dependencyObject = GetParent(dependencyObject);
        }

        return null;
    }

    /// <summary>
    /// Gets a value indicating whether the source element belongs to an interactive subtree that should not start node drags.
    /// </summary>
    /// <param name="dependencyObject">The source element.</param>
    /// <returns><c>true</c> if the element should suppress node drag start; otherwise, <c>false</c>.</returns>
    private static bool IsInteractiveElement(DependencyObject dependencyObject)
    {
        DependencyObject? current = dependencyObject;

        while (current is not null)
        {
            if (current is ButtonBase or TextBoxBase or Selector)
            {
                return true;
            }

            if (current is XTreeViewItem)
            {
                return false;
            }

            current = GetParent(current);
        }

        return false;
    }

    /// <summary>
    /// Gets bounds of an element relative to the owner tree view.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="ownerTreeView">The owner tree view.</param>
    /// <returns>The element bounds relative to the owner tree view.</returns>
    private static Rect GetBoundsRelativeToOwner(FrameworkElement element, XTreeView ownerTreeView)
    {
        if (element.ActualWidth <= 0d || element.ActualHeight <= 0d)
        {
            return Rect.Empty;
        }

        try
        {
            GeneralTransform transform = element.TransformToAncestor(ownerTreeView);
            return transform.TransformBounds(new Rect(0d, 0d, element.ActualWidth, element.ActualHeight));
        }
        catch (InvalidOperationException)
        {
            return Rect.Empty;
        }
    }

    /// <summary>
    /// Gets the most likely parent of the specified dependency object.
    /// </summary>
    /// <param name="dependencyObject">The source object.</param>
    /// <returns>The parent object or <see langword="null"/>.</returns>
    private static DependencyObject? GetParent(DependencyObject dependencyObject)
    {
        if (dependencyObject is Visual or Visual3D)
        {
            DependencyObject? visualParent = VisualTreeHelper.GetParent(dependencyObject);
            if (visualParent is not null)
            {
                return visualParent;
            }
        }

        DependencyObject? logicalParent = LogicalTreeHelper.GetParent(dependencyObject);
        if (logicalParent is not null)
        {
            return logicalParent;
        }

        if (dependencyObject is FrameworkElement frameworkElement)
        {
            if (frameworkElement.Parent is not null)
            {
                return frameworkElement.Parent;
            }

            if (frameworkElement.TemplatedParent is DependencyObject templatedParent)
            {
                return templatedParent;
            }
        }

        return dependencyObject is FrameworkContentElement frameworkContentElement
            ? frameworkContentElement.Parent
            : null;
    }
    #endregion
}
#endregion
