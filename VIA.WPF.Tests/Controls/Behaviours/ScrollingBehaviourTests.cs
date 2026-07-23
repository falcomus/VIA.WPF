// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrollingBehaviourTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Controls;
using VIA.WPF.Behaviors;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Behaviours;

#region ### Class ScrollingBehaviourTests ###
/// <summary>
/// Tests scrolling related VIA.WPF behaviors.
/// </summary>
public sealed class ScrollingBehaviourTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that <see cref="ScrollIntoViewBehavior" /> reacts to list box selection changes without errors.
    /// </summary>
    [Fact]
    public void ScrollIntoViewBehavior_ShouldReactToListBoxSelectionChanges()
    {
        WpfTestHelper.Run(
            () =>
            {
                string[] items = ["Alpha", "Beta", "Gamma"];
                ListBox listBox = new()
                {
                    ItemsSource = items
                };

                ScrollIntoViewBehavior.SetIsEnabled(listBox, true);
                listBox.SelectedItem = items[2];
                WpfTestHelper.DoEvents();

                Assert.Same(items[2], listBox.SelectedItem);
            });
    }

    /// <summary>
    /// Verifies that <see cref="ScrollIntoViewBehavior" /> reacts to tree view selection changes without errors.
    /// </summary>
    [Fact]
    public void ScrollIntoViewBehavior_ShouldReactToTreeViewSelectionChanges()
    {
        WpfTestHelper.Run(
            () =>
            {
                TreeViewItem child = new()
                {
                    Header = "Child"
                };
                TreeViewItem root = new()
                {
                    Header = "Root",
                    IsExpanded = true
                };
                root.Items.Add(child);

                TreeView treeView = new();
                treeView.Items.Add(root);

                ScrollIntoViewBehavior.SetIsEnabled(treeView, true);
                child.IsSelected = true;
                WpfTestHelper.DoEvents();

                Assert.True(child.IsSelected);
            });
    }

    /// <summary>
    /// Verifies that <see cref="AutoScrollOnDragOverBehavior" /> keeps its scroll settings within the expected attached property contract.
    /// </summary>
    [Fact]
    public void AutoScrollOnDragOverBehavior_ShouldKeepScrollSettings()
    {
        WpfTestHelper.Run(
            () =>
            {
                ScrollViewer scrollViewer = new();

                AutoScrollOnDragOverBehavior.SetEdgeThreshold(scrollViewer, 12d);
                AutoScrollOnDragOverBehavior.SetScrollStep(scrollViewer, 4d);
                AutoScrollOnDragOverBehavior.SetIsEnabled(scrollViewer, true);

                Assert.Equal(12d, AutoScrollOnDragOverBehavior.GetEdgeThreshold(scrollViewer));
                Assert.Equal(4d, AutoScrollOnDragOverBehavior.GetScrollStep(scrollViewer));
                Assert.True(AutoScrollOnDragOverBehavior.GetIsEnabled(scrollViewer));
                Assert.True(scrollViewer.AllowDrop);
            });
    }
    #endregion
}
#endregion
