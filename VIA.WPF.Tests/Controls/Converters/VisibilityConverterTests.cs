// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisibilityConverterTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using VIA.WPF.Converters;

namespace VIA.WPF.Tests.Controls.Converters;

#region ### Class VisibilityConverterTests ###
/// <summary>
/// Tests visibility-related VIA.WPF converters.
/// </summary>
public sealed class VisibilityConverterTests
{
    #region ### Private Fields ###
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
    #endregion

    #region ### Tests ###
    /// <summary>
    /// Verifies that <see cref="BooleanToVisibilityConverter" /> maps boolean values to visibility values.
    /// </summary>
    [Fact]
    public void BooleanToVisibilityConverter_ShouldMapBooleanValuesToVisibilityValues()
    {
        VIA.WPF.Converters.BooleanToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Visible, converter.Convert(true, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(false, typeof(Visibility), null, Culture));
        Assert.True((bool)converter.ConvertBack(Visibility.Visible, typeof(bool), null, Culture));
        Assert.False((bool)converter.ConvertBack(Visibility.Collapsed, typeof(bool), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="InverseBooleanToVisibilityConverter" /> maps boolean values to inverted visibility values.
    /// </summary>
    [Fact]
    public void InverseBooleanToVisibilityConverter_ShouldMapBooleanValuesToInvertedVisibilityValues()
    {
        InverseBooleanToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Collapsed, converter.Convert(true, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert(false, typeof(Visibility), null, Culture));
        Assert.True((bool)converter.ConvertBack(Visibility.Collapsed, typeof(bool), null, Culture));
        Assert.False((bool)converter.ConvertBack(Visibility.Visible, typeof(bool), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="AllTrueToVisibilityConverter" /> returns visible only when all values are true.
    /// </summary>
    [Fact]
    public void AllTrueToVisibilityConverter_ShouldReturnVisibleOnlyWhenAllValuesAreTrue()
    {
        AllTrueToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Visible, converter.Convert([true, 1, "true"], typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert([true, 0], typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert([], typeof(Visibility), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="AnyTrueToVisibilityConverter" /> returns visible when any value is true.
    /// </summary>
    [Fact]
    public void AnyTrueToVisibilityConverter_ShouldReturnVisibleWhenAnyValueIsTrue()
    {
        AnyTrueToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Visible, converter.Convert([false, 1], typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert([false, 0, "false"], typeof(Visibility), null, Culture));
    }

    /// <summary>
    /// Verifies that multi-value visibility converters return <see cref="Binding.DoNothing" /> for ConvertBack.
    /// </summary>
    [Fact]
    public void MultiVisibilityConverters_ConvertBack_ShouldReturnDoNothingValues()
    {
        AnyTrueToVisibilityConverter converter = new();

        object[] result = converter.ConvertBack(Visibility.Visible, [typeof(bool), typeof(bool)], null, Culture);

        Assert.Equal(2, result.Length);
        Assert.All(result, item => Assert.Same(Binding.DoNothing, item));
    }

    /// <summary>
    /// Verifies that <see cref="CollectionEmptyToVisibilityConverter" /> maps empty collections to visibility values.
    /// </summary>
    [Fact]
    public void CollectionEmptyToVisibilityConverter_ShouldMapCollectionsByCount()
    {
        CollectionEmptyToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Visible, converter.Convert(Array.Empty<string>(), typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(new[] { "A" }, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert(null, typeof(Visibility), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Visible, typeof(object), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="CollectionNotEmptyToVisibilityConverter" /> maps non-empty collections to visibility values.
    /// </summary>
    [Fact]
    public void CollectionNotEmptyToVisibilityConverter_ShouldMapCollectionsByCount()
    {
        CollectionNotEmptyToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Collapsed, converter.Convert(Array.Empty<string>(), typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert(new[] { "A" }, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(null, typeof(Visibility), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Visible, typeof(object), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="CountToVisibilityConverter" /> maps zero and non-zero counts to visibility values.
    /// </summary>
    [Fact]
    public void CountToVisibilityConverter_ShouldMapCountsToVisibilityValues()
    {
        CountToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Collapsed, converter.Convert(0, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert(3, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert(new[] { 1, 2 }, typeof(Visibility), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Visible, typeof(int), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="NullToVisibilityConverter" /> maps null and non-null values.
    /// </summary>
    [Fact]
    public void NullToVisibilityConverter_ShouldMapNullAndNonNullValues()
    {
        NullToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Collapsed, converter.Convert(null, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert("Text", typeof(Visibility), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Visible, typeof(object), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="EqualityToVisibilityConverter" /> maps comparison results to visibility values.
    /// </summary>
    [Fact]
    public void EqualityToVisibilityConverter_ShouldMapComparisonResultsToVisibilityValues()
    {
        EqualityToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Visible, converter.Convert(42, typeof(Visibility), "42", Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(42, typeof(Visibility), "43", Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Visible, typeof(int), "42", Culture));
    }

    /// <summary>
    /// Verifies that <see cref="EnumToVisibilityConverter" /> maps enum comparison results to visibility values.
    /// </summary>
    [Fact]
    public void EnumToVisibilityConverter_ShouldMapEnumComparisonResultsToVisibilityValues()
    {
        EnumToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Visible, converter.Convert(TestMode.Edit, typeof(Visibility), "Edit", Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(TestMode.Edit, typeof(Visibility), "View", Culture));
        Assert.Equal(TestMode.Edit, converter.ConvertBack(Visibility.Visible, typeof(TestMode), "Edit", Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Collapsed, typeof(TestMode), "Edit", Culture));
    }

    /// <summary>
    /// Verifies that <see cref="NumberGreaterThanToVisibilityConverter" /> maps numbers by threshold.
    /// </summary>
    [Fact]
    public void NumberGreaterThanToVisibilityConverter_ShouldMapNumbersByThreshold()
    {
        NumberGreaterThanToVisibilityConverter converter = new()
        {
            Threshold = 10
        };

        Assert.Equal(Visibility.Visible, converter.Convert(11, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(10, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert(6, typeof(Visibility), "5", Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Visible, typeof(double), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="StringNullOrEmptyToVisibilityConverter" /> maps null, empty and text values.
    /// </summary>
    [Fact]
    public void StringNullOrEmptyToVisibilityConverter_ShouldMapStringValues()
    {
        StringNullOrEmptyToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Collapsed, converter.Convert(null, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(string.Empty, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert("   ", typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert("Text", typeof(Visibility), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Visible, typeof(string), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="StringNullOrWhiteSpaceToVisibilityConverter" /> maps null, white-space and text values.
    /// </summary>
    [Fact]
    public void StringNullOrWhiteSpaceToVisibilityConverter_ShouldMapStringValues()
    {
        StringNullOrWhiteSpaceToVisibilityConverter converter = new();

        Assert.Equal(Visibility.Collapsed, converter.Convert(null, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert("   ", typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Visible, converter.Convert("Text", typeof(Visibility), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(Visibility.Visible, typeof(string), null, Culture));
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
