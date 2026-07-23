// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueConverterTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using VIA.WPF.Converters;

namespace VIA.WPF.Tests.Controls.Converters;

#region ### Class ValueConverterTests ###
/// <summary>
/// Tests value and object related VIA.WPF converters.
/// </summary>
public sealed class ValueConverterTests
{
    #region ### Private Fields ###
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
    #endregion

    #region ### Tests ###
    /// <summary>
    /// Verifies that <see cref="ColorToBrushConverter" /> creates a solid color brush.
    /// </summary>
    [Fact]
    public void ColorToBrushConverter_ShouldCreateSolidColorBrush()
    {
        ColorToBrushConverter converter = new();

        object result = converter.Convert(Colors.CornflowerBlue, typeof(Brush), null, Culture);

        SolidColorBrush brush = Assert.IsType<SolidColorBrush>(result);
        Assert.Equal(Colors.CornflowerBlue, brush.Color);
        Assert.Equal(Colors.CornflowerBlue, converter.ConvertBack(brush, typeof(Color), null, Culture));
        Assert.Same(Binding.DoNothing, converter.Convert("Blue", typeof(Brush), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="BrushOpacityConverter" /> clones a brush and applies opacity.
    /// </summary>
    [Fact]
    public void BrushOpacityConverter_ShouldCloneBrushAndApplyOpacity()
    {
        BrushOpacityConverter converter = new()
        {
            FreezeBrush = false
        };
        SolidColorBrush sourceBrush = new(Colors.Red)
        {
            Opacity = 0.8
        };

        object result = converter.Convert(sourceBrush, typeof(Brush), "0.25", Culture);

        Brush brush = Assert.IsAssignableFrom<Brush>(result);
        Assert.NotSame(sourceBrush, brush);
        Assert.Equal(0.25, brush.Opacity);
        Assert.Equal(0.8, sourceBrush.Opacity);
        Assert.Same(Binding.DoNothing, converter.ConvertBack(brush, typeof(Brush), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="MultiplyConverter" /> multiplies and divides numeric values.
    /// </summary>
    [Fact]
    public void MultiplyConverter_ShouldMultiplyAndDivideNumericValues()
    {
        MultiplyConverter converter = new()
        {
            Factor = 2
        };

        Assert.Equal(8d, (double)converter.Convert(4, typeof(double), null, Culture));
        Assert.Equal(12d, (double)converter.Convert(4, typeof(double), "3", Culture));
        Assert.Equal(4d, (double)converter.ConvertBack(8, typeof(double), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(8, typeof(double), "0", Culture));
    }

    /// <summary>
    /// Verifies that <see cref="TreeLevelToThicknessConverter" /> maps tree levels to left indentation.
    /// </summary>
    [Fact]
    public void TreeLevelToThicknessConverter_ShouldMapTreeLevelToLeftIndentation()
    {
        TreeLevelToThicknessConverter converter = new()
        {
            IndentSize = 10,
            Top = 1,
            Right = 2,
            Bottom = 3
        };

        Thickness defaultThickness = (Thickness)converter.Convert(2, typeof(Thickness), null, Culture);
        Thickness parameterThickness = (Thickness)converter.Convert(3, typeof(Thickness), "4", Culture);
        Thickness negativeThickness = (Thickness)converter.Convert(-1, typeof(Thickness), null, Culture);

        Assert.Equal(new Thickness(20, 1, 2, 3), defaultThickness);
        Assert.Equal(new Thickness(12, 1, 2, 3), parameterThickness);
        Assert.Equal(new Thickness(0, 1, 2, 3), negativeThickness);
        Assert.Same(Binding.DoNothing, converter.ConvertBack(defaultThickness, typeof(int), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="EnumToBooleanConverter" /> maps enum values to booleans and back.
    /// </summary>
    [Fact]
    public void EnumToBooleanConverter_ShouldMapEnumValuesToBooleanValuesAndBack()
    {
        EnumToBooleanConverter converter = new();

        Assert.True((bool)converter.Convert(TestMode.Edit, typeof(bool), "Edit", Culture));
        Assert.False((bool)converter.Convert(TestMode.Edit, typeof(bool), "View", Culture));
        Assert.Equal(TestMode.Edit, converter.ConvertBack(true, typeof(TestMode), "Edit", Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(false, typeof(TestMode), "Edit", Culture));
    }

    /// <summary>
    /// Verifies that <see cref="ObjectMemberPathValueConverter" /> resolves properties, fields and dictionaries.
    /// </summary>
    [Fact]
    public void ObjectMemberPathValueConverter_ShouldResolveMemberPaths()
    {
        ObjectMemberPathValueConverter converter = new();
        TestContainer source = new();
        Hashtable dictionary = new()
        {
            ["Name"] = "DictionaryValue"
        };

        Assert.Equal("NestedValue", converter.Convert([source, "Nested.Name"], typeof(string), null, Culture));
        Assert.Equal("FieldValue", converter.Convert([source, "PublicField"], typeof(string), null, Culture));
        Assert.Equal("DictionaryValue", converter.Convert([dictionary, "Name"], typeof(string), null, Culture));
        Assert.Equal(source.ToString(), converter.Convert([source, "Missing"], typeof(string), "FallbackToString", Culture));

        object[] result = converter.ConvertBack("Value", [typeof(object), typeof(string)], null, Culture);
        Assert.All(result, item => Assert.Same(Binding.DoNothing, item));
    }

    /// <summary>
    /// Verifies that <see cref="FirstValidationErrorConverter" /> safely returns the first validation error content.
    /// </summary>
    [Fact]
    public void FirstValidationErrorConverter_ShouldReturnFirstValidationErrorContent()
    {
        FirstValidationErrorConverter converter = new();
        ReadOnlyCollection<ValidationError> errors = new(
            new List<ValidationError>
            {
                new(new ExceptionValidationRule(), new object(), "First error", null),
                new(new ExceptionValidationRule(), new object(), "Second error", null)
            });

        Assert.Equal("First error", converter.Convert(errors, typeof(string), null, Culture));
        Assert.Equal(string.Empty, converter.Convert(Array.Empty<ValidationError>(), typeof(string), null, Culture));
        Assert.Equal(string.Empty, converter.Convert(null, typeof(string), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack("Text", typeof(object), null, Culture));
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestContainer
    {
        #region ### Public Fields ###
        /// <summary>
        /// Public field used for member-path tests.
        /// </summary>
        public string PublicField = "FieldValue";
        #endregion

        #region ### Public Properties ###
        /// <summary>
        /// Gets the nested test value.
        /// </summary>
        public TestNestedContainer Nested { get; } = new();
        #endregion
    }

    private sealed class TestNestedContainer
    {
        #region ### Public Properties ###
        /// <summary>
        /// Gets the nested name.
        /// </summary>
        public string Name { get; } = "NestedValue";
        #endregion
    }
    #endregion

    #region ### Private Enums ###
    private enum TestMode
    {
        View,
        Edit
    }
    #endregion
}
#endregion
