// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XDataGridTextColumn.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VIA.WPF.Controls;

#region ### Class XDataGridTextColumn ###
/// <summary>
/// Represents a text column for <see cref="XDataGrid" /> that can highlight search matches.
/// </summary>
public class XDataGridTextColumn : DataGridTextColumn
{
    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        XHighlightTextBlock textBlock = new()
        {
            VerticalAlignment = VerticalAlignment.Center
        };

        if (ElementStyle is not null)
        {
            textBlock.Style = ElementStyle;
        }

        ApplyTextBinding(textBlock);
        ApplyHighlightBindings(textBlock, cell);

        return textBlock;
    }
    #endregion

    #region ### Private Methods ###
    private void ApplyTextBinding(XHighlightTextBlock textBlock)
    {
        if (Binding is not null)
        {
            BindingOperations.SetBinding(textBlock, XHighlightTextBlock.DisplayTextProperty, Binding);
        }
    }

    private static void ApplyHighlightBindings(XHighlightTextBlock textBlock, DependencyObject source)
    {
        XDataGrid? grid = XDataGrid.FindVisualParent<XDataGrid>(source);

        if (grid is not null)
        {
            ApplyHighlightBindings(textBlock, grid);
            return;
        }

        textBlock.Loaded += OnHighlightTextBlockLoaded;
    }

    private static void ApplyHighlightBindings(XHighlightTextBlock textBlock, XDataGrid grid)
    {
        textBlock.SetBinding(
            XHighlightTextBlock.HighlightTextProperty,
            new Binding(nameof(XDataGrid.SearchTerm))
            {
                Source = grid,
                Mode = BindingMode.OneWay
            });

        textBlock.SetBinding(
            XHighlightTextBlock.HighlightRenderModeProperty,
            new Binding(nameof(XDataGrid.SearchMatchHighlightRenderMode))
            {
                Source = grid,
                Mode = BindingMode.OneWay
            });

        textBlock.SetBinding(
            XHighlightTextBlock.HighlightBrushProperty,
            new Binding(nameof(XDataGrid.SearchMatchHighlightBrush))
            {
                Source = grid,
                Mode = BindingMode.OneWay
            });

        textBlock.SetBinding(
            XHighlightTextBlock.HighlightForegroundProperty,
            new Binding(nameof(XDataGrid.SearchMatchHighlightForeground))
            {
                Source = grid,
                Mode = BindingMode.OneWay
            });
    }

    private static void OnHighlightTextBlockLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not XHighlightTextBlock textBlock)
        {
            return;
        }

        textBlock.Loaded -= OnHighlightTextBlockLoaded;

        XDataGrid? grid = XDataGrid.FindVisualParent<XDataGrid>(textBlock);
        if (grid is not null)
        {
            ApplyHighlightBindings(textBlock, grid);
        }
    }

    #endregion
}
#endregion
