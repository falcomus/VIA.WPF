// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XGrid.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XGrid ###
/// <summary>
/// Represents an extended <see cref="Grid"/> with compact row and column definition syntax,
/// logical spacing and optional named areas.
/// </summary>
/// <remarks>
/// Child placement uses the standard <see cref="Grid.RowProperty"/>, <see cref="Grid.ColumnProperty"/>,
/// <see cref="Grid.RowSpanProperty"/> and <see cref="Grid.ColumnSpanProperty"/> attached properties.
/// Named placement can additionally be declared through <see cref="AreaProperty"/>.
/// </remarks>
public class XGrid : Grid
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Rows"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(
        nameof(Rows),
        typeof(string),
        typeof(XGrid),
        new FrameworkPropertyMetadata(string.Empty, OnStructurePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="Columns"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
        nameof(Columns),
        typeof(string),
        typeof(XGrid),
        new FrameworkPropertyMetadata(string.Empty, OnStructurePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="Areas"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AreasProperty = DependencyProperty.Register(
        nameof(Areas),
        typeof(string),
        typeof(XGrid),
        new FrameworkPropertyMetadata(string.Empty, OnStructurePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="RowSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
        nameof(RowSpacing),
        typeof(double),
        typeof(XGrid),
        new FrameworkPropertyMetadata(0d, OnStructurePropertyChanged));

    /// <summary>
    /// Identifies the <see cref="ColumnSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
        nameof(ColumnSpacing),
        typeof(double),
        typeof(XGrid),
        new FrameworkPropertyMetadata(0d, OnStructurePropertyChanged));

    /// <summary>
    /// Identifies the named area attached property.
    /// </summary>
    public static readonly DependencyProperty AreaProperty = DependencyProperty.RegisterAttached(
        "Area",
        typeof(string),
        typeof(XGrid),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure, OnChildLayoutPropertyChanged));
    #endregion

    #region ### Private Fields ###
    private const int UnsetGridValue = int.MinValue;

    private static readonly DependencyProperty StandardLogicalRowProperty = DependencyProperty.RegisterAttached(
        "StandardLogicalRow",
        typeof(int),
        typeof(XGrid),
        new PropertyMetadata(UnsetGridValue));

    private static readonly DependencyProperty StandardLogicalColumnProperty = DependencyProperty.RegisterAttached(
        "StandardLogicalColumn",
        typeof(int),
        typeof(XGrid),
        new PropertyMetadata(UnsetGridValue));

    private static readonly DependencyProperty StandardLogicalRowSpanProperty = DependencyProperty.RegisterAttached(
        "StandardLogicalRowSpan",
        typeof(int),
        typeof(XGrid),
        new PropertyMetadata(UnsetGridValue));

    private static readonly DependencyProperty StandardLogicalColumnSpanProperty = DependencyProperty.RegisterAttached(
        "StandardLogicalColumnSpan",
        typeof(int),
        typeof(XGrid),
        new PropertyMetadata(UnsetGridValue));

    private static readonly DependencyProperty LastAppliedActualRowProperty = DependencyProperty.RegisterAttached(
        "LastAppliedActualRow",
        typeof(int),
        typeof(XGrid),
        new PropertyMetadata(UnsetGridValue));

    private static readonly DependencyProperty LastAppliedActualColumnProperty = DependencyProperty.RegisterAttached(
        "LastAppliedActualColumn",
        typeof(int),
        typeof(XGrid),
        new PropertyMetadata(UnsetGridValue));

    private static readonly DependencyProperty LastAppliedActualRowSpanProperty = DependencyProperty.RegisterAttached(
        "LastAppliedActualRowSpan",
        typeof(int),
        typeof(XGrid),
        new PropertyMetadata(UnsetGridValue));

    private static readonly DependencyProperty LastAppliedActualColumnSpanProperty = DependencyProperty.RegisterAttached(
        "LastAppliedActualColumnSpan",
        typeof(int),
        typeof(XGrid),
        new PropertyMetadata(UnsetGridValue));

    private static readonly DependencyProperty IsStandardGridPlacementObserverAttachedProperty = DependencyProperty.RegisterAttached(
        "IsStandardGridPlacementObserverAttached",
        typeof(bool),
        typeof(XGrid),
        new PropertyMetadata(false));

    private IReadOnlyDictionary<string, AreaDefinition> _areaDefinitions = new Dictionary<string, AreaDefinition>(StringComparer.OrdinalIgnoreCase);
    private bool _isApplyingChildLayoutMappings;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XGrid"/> class.
    /// </summary>
    public XGrid()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
        this.Loaded += OnLoaded;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the compact row definition string.
    /// </summary>
    public string Rows
    {
        get => (string)this.GetValue(RowsProperty);
        set => this.SetValue(RowsProperty, value);
    }

    /// <summary>
    /// Gets or sets the compact column definition string.
    /// </summary>
    public string Columns
    {
        get => (string)this.GetValue(ColumnsProperty);
        set => this.SetValue(ColumnsProperty, value);
    }

    /// <summary>
    /// Gets or sets the named area matrix.
    /// </summary>
    public string Areas
    {
        get => (string)this.GetValue(AreasProperty);
        set => this.SetValue(AreasProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between logical rows.
    /// </summary>
    public double RowSpacing
    {
        get => (double)this.GetValue(RowSpacingProperty);
        set => this.SetValue(RowSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between logical columns.
    /// </summary>
    public double ColumnSpacing
    {
        get => (double)this.GetValue(ColumnSpacingProperty);
        set => this.SetValue(ColumnSpacingProperty, value);
    }
    #endregion

    #region ### Public Attached Property Accessors ###
    /// <summary>
    /// Sets the named area of a child element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The named area.</param>
    public static void SetArea(UIElement element, string value)
    {
        ArgumentNullException.ThrowIfNull(element);
        element.SetValue(AreaProperty, value);
    }

    /// <summary>
    /// Gets the named area of a child element.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The named area.</returns>
    public static string GetArea(UIElement element)
    {
        ArgumentNullException.ThrowIfNull(element);
        return (string)element.GetValue(AreaProperty);
    }
    #endregion

    #region ### Protected Methods ###
    /// <summary>
    /// Called when visual children are added or removed.
    /// </summary>
    /// <param name="visualAdded">The added visual.</param>
    /// <param name="visualRemoved">The removed visual.</param>
    protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
    {
        if (visualRemoved is UIElement removedElement)
        {
            this.DetachStandardGridPlacementObservers(removedElement);
        }

        if (visualAdded is UIElement addedElement)
        {
            this.AttachStandardGridPlacementObservers(addedElement);
        }

        base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        this.ApplyChildLayoutMappings();
    }

    /// <inheritdoc />
    protected override Size MeasureOverride(Size constraint)
    {
        this.ApplyChildLayoutMappings();
        return base.MeasureOverride(constraint);
    }
    #endregion

    #region ### Private Static Methods ###
    private static void OnStructurePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is XGrid grid)
        {
            grid.RebuildDefinitions();
            grid.ApplyChildLayoutMappings();
        }
    }

    private static void OnChildLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            DependencyObject? parent = LogicalTreeHelper.GetParent(element) ?? VisualTreeHelperEx.GetVisualParent(element);
            if (parent is XGrid grid)
            {
                grid.ApplyChildLayoutMappings();
            }
        }
    }

    private static List<GridLength> ParseDefinitions(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        string[] tokens = value
            .Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return [.. tokens.Select(ParseGridLength)];
    }

    private static GridLength ParseGridLength(string token)
    {
        string normalizedToken = token.Trim();

        if (string.Equals(normalizedToken, "Auto", StringComparison.OrdinalIgnoreCase))
        {
            return GridLength.Auto;
        }

        if (normalizedToken.EndsWith('*'))
        {
            string starValue = normalizedToken[..^1].Trim();
            if (string.IsNullOrEmpty(starValue))
            {
                return new GridLength(1d, GridUnitType.Star);
            }

            if (double.TryParse(starValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double starFactor) && starFactor >= 0d)
            {
                return new GridLength(starFactor, GridUnitType.Star);
            }

            throw new FormatException($"The value '{token}' is not a valid star-sized grid length.");
        }

        if (double.TryParse(normalizedToken, NumberStyles.Float, CultureInfo.InvariantCulture, out double pixelValue) && pixelValue >= 0d)
        {
            return new GridLength(pixelValue, GridUnitType.Pixel);
        }

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(GridLength));
        if (converter.CanConvertFrom(typeof(string)))
        {
            object? convertedValue = converter.ConvertFrom(null, CultureInfo.InvariantCulture, normalizedToken);
            if (convertedValue is GridLength gridLength)
            {
                return gridLength;
            }
        }

        throw new FormatException($"The value '{token}' is not a valid grid length.");
    }

    private static IEnumerable<GridLength> ExpandDefinitionsWithSpacing(List<GridLength> definitions, double spacing)
    {
        if (definitions.Count == 0)
        {
            yield break;
        }

        double normalizedSpacing = Math.Max(0d, spacing);

        for (int index = 0; index < definitions.Count; index++)
        {
            if (index > 0 && normalizedSpacing > 0d)
            {
                yield return new GridLength(normalizedSpacing, GridUnitType.Pixel);
            }

            yield return definitions[index];
        }
    }

    private static Dictionary<string, AreaDefinition> ParseAreas(string? areas, int expectedRowCount, int expectedColumnCount)
    {
        if (string.IsNullOrWhiteSpace(areas))
        {
            return new Dictionary<string, AreaDefinition>(StringComparer.OrdinalIgnoreCase);
        }

        string normalizedAreas = areas.Replace("\r", string.Empty, StringComparison.Ordinal).Trim();
        List<string[]> matrix = [];

        if (normalizedAreas.Contains(';', StringComparison.Ordinal) || normalizedAreas.Contains('\n', StringComparison.Ordinal))
        {
            string[] lines = normalizedAreas
                .Split(['\n', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (string line in lines)
            {
                string[] tokens = line
                    .Split([' ', '\t', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (tokens.Length == 0)
                {
                    continue;
                }

                if (expectedColumnCount > 0 && tokens.Length != expectedColumnCount)
                {
                    throw new FormatException($"Each Areas row must contain exactly {expectedColumnCount} columns.");
                }

                matrix.Add(tokens);
            }
        }
        else
        {
            string[] flatTokens = normalizedAreas
                .Split([' ', '\t', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (flatTokens.Length == 0)
            {
                return new Dictionary<string, AreaDefinition>(StringComparer.OrdinalIgnoreCase);
            }

            if (expectedRowCount <= 0 || expectedColumnCount <= 0)
            {
                throw new FormatException("Areas defined in a single line require matching Rows and Columns definitions or semicolon-separated rows.");
            }

            int expectedTokenCount = expectedRowCount * expectedColumnCount;
            if (flatTokens.Length != expectedTokenCount)
            {
                throw new FormatException($"The Areas definition contains {flatTokens.Length} cells, but {expectedTokenCount} cells are required for {expectedRowCount} rows and {expectedColumnCount} columns.");
            }

            for (int row = 0; row < expectedRowCount; row++)
            {
                string[] rowTokens = [.. flatTokens
                    .Skip(row * expectedColumnCount)
                    .Take(expectedColumnCount)];

                matrix.Add(rowTokens);
            }
        }

        if (matrix.Count == 0)
        {
            return new Dictionary<string, AreaDefinition>(StringComparer.OrdinalIgnoreCase);
        }

        if (expectedRowCount > 0 && matrix.Count != expectedRowCount)
        {
            throw new FormatException($"The Rows definition count ({expectedRowCount}) must match the Areas row count ({matrix.Count}).");
        }

        if (expectedColumnCount > 0 && matrix.Any(row => row.Length != expectedColumnCount))
        {
            throw new FormatException($"The Columns definition count ({expectedColumnCount}) must match the Areas column count.");
        }

        Dictionary<string, List<(int Row, int Column)>> cellsByArea = new(StringComparer.OrdinalIgnoreCase);

        for (int row = 0; row < matrix.Count; row++)
        {
            for (int column = 0; column < matrix[row].Length; column++)
            {
                string token = matrix[row][column];
                if (IsEmptyAreaToken(token))
                {
                    continue;
                }

                if (!cellsByArea.TryGetValue(token, out List<(int Row, int Column)>? cells))
                {
                    cells = [];
                    cellsByArea[token] = cells;
                }

                cells.Add((row, column));
            }
        }

        Dictionary<string, AreaDefinition> result = new(StringComparer.OrdinalIgnoreCase);

        foreach ((string areaName, List<(int Row, int Column)> cells) in cellsByArea)
        {
            int minRow = cells.Min(static cell => cell.Row);
            int maxRow = cells.Max(static cell => cell.Row);
            int minColumn = cells.Min(static cell => cell.Column);
            int maxColumn = cells.Max(static cell => cell.Column);

            for (int row = minRow; row <= maxRow; row++)
            {
                for (int column = minColumn; column <= maxColumn; column++)
                {
                    if (!cells.Contains((row, column)))
                    {
                        throw new FormatException($"The area '{areaName}' must form a contiguous rectangle.");
                    }
                }
            }

            result[areaName] = new AreaDefinition(
                areaName,
                minRow,
                minColumn,
                (maxRow - minRow) + 1,
                (maxColumn - minColumn) + 1);
        }

        return result;
    }

    private static bool IsEmptyAreaToken(string token)
    {
        return string.Equals(token, ".", StringComparison.Ordinal)
            || string.Equals(token, "-", StringComparison.Ordinal)
            || string.Equals(token, "_", StringComparison.Ordinal);
    }

    private static int MapLogicalIndexToActualIndex(int logicalIndex, double spacing)
    {
        return Math.Max(0, spacing > 0d ? logicalIndex * 2 : logicalIndex);
    }

    private static int MapLogicalSpanToActualSpan(int logicalSpan, double spacing)
    {
        if (logicalSpan <= 1)
        {
            return 1;
        }

        return spacing > 0d ? (logicalSpan * 2) - 1 : logicalSpan;
    }

    private static int GetEffectiveRow(UIElement element)
    {
        return GetEffectiveStandardGridValue(element, StandardLogicalRowProperty, LastAppliedActualRowProperty, Grid.GetRow);
    }

    private static int GetEffectiveColumn(UIElement element)
    {
        return GetEffectiveStandardGridValue(element, StandardLogicalColumnProperty, LastAppliedActualColumnProperty, Grid.GetColumn);
    }

    private static int GetEffectiveRowSpan(UIElement element)
    {
        return GetEffectiveStandardGridValue(element, StandardLogicalRowSpanProperty, LastAppliedActualRowSpanProperty, Grid.GetRowSpan);
    }

    private static int GetEffectiveColumnSpan(UIElement element)
    {
        return GetEffectiveStandardGridValue(element, StandardLogicalColumnSpanProperty, LastAppliedActualColumnSpanProperty, Grid.GetColumnSpan);
    }

    private static int GetEffectiveStandardGridValue(UIElement element, DependencyProperty logicalProperty, DependencyProperty lastAppliedActualProperty, Func<UIElement, int> valueAccessor)
    {
        int currentValue = valueAccessor(element);
        int logicalValue = (int)element.GetValue(logicalProperty);
        int lastAppliedActualValue = (int)element.GetValue(lastAppliedActualProperty);

        if (logicalValue == UnsetGridValue || currentValue != lastAppliedActualValue)
        {
            element.SetValue(logicalProperty, currentValue);
            return currentValue;
        }

        return logicalValue;
    }
    #endregion

    #region ### Private Methods ###
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        this.EnsureStandardGridPlacementObservers();
        this.RebuildDefinitions();
        this.ApplyChildLayoutMappings();
    }

    private void EnsureStandardGridPlacementObservers()
    {
        foreach (UIElement child in this.Children.OfType<UIElement>())
        {
            this.AttachStandardGridPlacementObservers(child);
        }
    }

    private void AttachStandardGridPlacementObservers(UIElement element)
    {
        if ((bool)element.GetValue(IsStandardGridPlacementObserverAttachedProperty))
        {
            return;
        }

        DependencyPropertyDescriptor.FromProperty(Grid.RowProperty, typeof(UIElement))?.AddValueChanged(element, this.OnStandardGridRowChanged);
        DependencyPropertyDescriptor.FromProperty(Grid.ColumnProperty, typeof(UIElement))?.AddValueChanged(element, this.OnStandardGridColumnChanged);
        DependencyPropertyDescriptor.FromProperty(Grid.RowSpanProperty, typeof(UIElement))?.AddValueChanged(element, this.OnStandardGridRowSpanChanged);
        DependencyPropertyDescriptor.FromProperty(Grid.ColumnSpanProperty, typeof(UIElement))?.AddValueChanged(element, this.OnStandardGridColumnSpanChanged);

        element.SetValue(IsStandardGridPlacementObserverAttachedProperty, true);
    }

    private void DetachStandardGridPlacementObservers(UIElement element)
    {
        if (!(bool)element.GetValue(IsStandardGridPlacementObserverAttachedProperty))
        {
            return;
        }

        DependencyPropertyDescriptor.FromProperty(Grid.RowProperty, typeof(UIElement))?.RemoveValueChanged(element, this.OnStandardGridRowChanged);
        DependencyPropertyDescriptor.FromProperty(Grid.ColumnProperty, typeof(UIElement))?.RemoveValueChanged(element, this.OnStandardGridColumnChanged);
        DependencyPropertyDescriptor.FromProperty(Grid.RowSpanProperty, typeof(UIElement))?.RemoveValueChanged(element, this.OnStandardGridRowSpanChanged);
        DependencyPropertyDescriptor.FromProperty(Grid.ColumnSpanProperty, typeof(UIElement))?.RemoveValueChanged(element, this.OnStandardGridColumnSpanChanged);

        element.SetValue(IsStandardGridPlacementObserverAttachedProperty, false);
    }

    private void OnStandardGridRowChanged(object? sender, EventArgs e)
    {
        this.OnStandardGridPlacementChanged(sender, StandardLogicalRowProperty, LastAppliedActualRowProperty, Grid.GetRow);
    }

    private void OnStandardGridColumnChanged(object? sender, EventArgs e)
    {
        this.OnStandardGridPlacementChanged(sender, StandardLogicalColumnProperty, LastAppliedActualColumnProperty, Grid.GetColumn);
    }

    private void OnStandardGridRowSpanChanged(object? sender, EventArgs e)
    {
        this.OnStandardGridPlacementChanged(sender, StandardLogicalRowSpanProperty, LastAppliedActualRowSpanProperty, Grid.GetRowSpan);
    }

    private void OnStandardGridColumnSpanChanged(object? sender, EventArgs e)
    {
        this.OnStandardGridPlacementChanged(sender, StandardLogicalColumnSpanProperty, LastAppliedActualColumnSpanProperty, Grid.GetColumnSpan);
    }

    private void OnStandardGridPlacementChanged(object? sender, DependencyProperty logicalProperty, DependencyProperty lastAppliedActualProperty, Func<UIElement, int> valueAccessor)
    {
        if (this._isApplyingChildLayoutMappings)
        {
            return;
        }

        if (sender is not UIElement element)
        {
            return;
        }

        element.SetValue(logicalProperty, valueAccessor(element));
        element.SetValue(lastAppliedActualProperty, UnsetGridValue);
        this.ApplyChildLayoutMappings();
    }

    private void RebuildDefinitions()
    {
        List<GridLength> rowLengths = ParseDefinitions(this.Rows);
        List<GridLength> columnLengths = ParseDefinitions(this.Columns);

        this._areaDefinitions = ParseAreas(this.Areas, rowLengths.Count, columnLengths.Count);

        if (this._areaDefinitions.Count > 0)
        {
            int requiredRows = this._areaDefinitions.Values.Max(static area => area.Row + area.RowSpan);
            int requiredColumns = this._areaDefinitions.Values.Max(static area => area.Column + area.ColumnSpan);

            if (rowLengths.Count > 0 && rowLengths.Count != requiredRows)
            {
                throw new FormatException($"The Rows definition count ({rowLengths.Count}) must match the Areas row count ({requiredRows}).");
            }

            if (columnLengths.Count > 0 && columnLengths.Count != requiredColumns)
            {
                throw new FormatException($"The Columns definition count ({columnLengths.Count}) must match the Areas column count ({requiredColumns}).");
            }
        }

        this.RowDefinitions.Clear();
        this.ColumnDefinitions.Clear();

        foreach (GridLength rowLength in ExpandDefinitionsWithSpacing(rowLengths, this.RowSpacing))
        {
            this.RowDefinitions.Add(new RowDefinition { Height = rowLength });
        }

        foreach (GridLength columnLength in ExpandDefinitionsWithSpacing(columnLengths, this.ColumnSpacing))
        {
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = columnLength });
        }
    }

    private void ApplyChildLayoutMappings()
    {
        if (this._isApplyingChildLayoutMappings)
        {
            return;
        }

        try
        {
            this._isApplyingChildLayoutMappings = true;

            foreach (UIElement child in this.Children.OfType<UIElement>())
            {
                ChildPlacement placement = this.ResolvePlacement(child);

                int actualRow = MapLogicalIndexToActualIndex(placement.Row, this.RowSpacing);
                int actualColumn = MapLogicalIndexToActualIndex(placement.Column, this.ColumnSpacing);
                int actualRowSpan = MapLogicalSpanToActualSpan(placement.RowSpan, this.RowSpacing);
                int actualColumnSpan = MapLogicalSpanToActualSpan(placement.ColumnSpan, this.ColumnSpacing);

                if (Grid.GetRow(child) != actualRow)
                {
                    Grid.SetRow(child, actualRow);
                }

                child.SetValue(LastAppliedActualRowProperty, actualRow);

                if (Grid.GetColumn(child) != actualColumn)
                {
                    Grid.SetColumn(child, actualColumn);
                }

                child.SetValue(LastAppliedActualColumnProperty, actualColumn);

                if (Grid.GetRowSpan(child) != actualRowSpan)
                {
                    Grid.SetRowSpan(child, actualRowSpan);
                }

                child.SetValue(LastAppliedActualRowSpanProperty, actualRowSpan);

                if (Grid.GetColumnSpan(child) != actualColumnSpan)
                {
                    Grid.SetColumnSpan(child, actualColumnSpan);
                }

                child.SetValue(LastAppliedActualColumnSpanProperty, actualColumnSpan);
            }
        }
        finally
        {
            this._isApplyingChildLayoutMappings = false;
        }
    }

    private ChildPlacement ResolvePlacement(UIElement child)
    {
        string area = GetArea(child);
        if (!string.IsNullOrWhiteSpace(area) && this._areaDefinitions.TryGetValue(area.Trim(), out AreaDefinition? areaDefinition))
        {
            return new ChildPlacement(areaDefinition.Row, areaDefinition.Column, areaDefinition.RowSpan, areaDefinition.ColumnSpan);
        }

        return new ChildPlacement(
            Math.Max(0, GetEffectiveRow(child)),
            Math.Max(0, GetEffectiveColumn(child)),
            Math.Max(1, GetEffectiveRowSpan(child)),
            Math.Max(1, GetEffectiveColumnSpan(child)));
    }
    #endregion
}
#endregion

#region ### Class AreaDefinition ###
/// <summary>
/// Represents a named logical area of an <see cref="XGrid"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AreaDefinition"/> class.
/// </remarks>
/// <param name="name">The area name.</param>
/// <param name="row">The logical row.</param>
/// <param name="column">The logical column.</param>
/// <param name="rowSpan">The logical row span.</param>
/// <param name="columnSpan">The logical column span.</param>
internal sealed class AreaDefinition(string name, int row, int column, int rowSpan, int columnSpan)
{
    #region ### Constructors ###
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the area name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the logical row.
    /// </summary>
    public int Row { get; } = row;

    /// <summary>
    /// Gets the logical column.
    /// </summary>
    public int Column { get; } = column;

    /// <summary>
    /// Gets the logical row span.
    /// </summary>
    public int RowSpan { get; } = rowSpan;

    /// <summary>
    /// Gets the logical column span.
    /// </summary>
    public int ColumnSpan { get; } = columnSpan;
    #endregion
}
#endregion

#region ### Struct ChildPlacement ###
/// <summary>
/// Represents the resolved logical placement of a child.
/// </summary>
internal readonly record struct ChildPlacement(int Row, int Column, int RowSpan, int ColumnSpan);
#endregion

#region ### Class VisualTreeHelperEx ###
/// <summary>
/// Provides compatibility helpers for visual tree lookups.
/// </summary>
internal static class VisualTreeHelperEx
{
    #region ### Public Methods ###
    /// <summary>
    /// Gets the visual parent of the specified dependency object if available.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <returns>The visual parent, or <see langword="null"/>.</returns>
    public static DependencyObject? GetVisualParent(DependencyObject dependencyObject)
    {
        return dependencyObject switch
        {
            null => null,
            Visual or System.Windows.Media.Media3D.Visual3D => System.Windows.Media.VisualTreeHelper.GetParent(dependencyObject),
            _ => null,
        };
    }
    #endregion
}
#endregion
