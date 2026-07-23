// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanConverterTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using VIA.WPF.Converters;

namespace VIA.WPF.Tests.Controls.Converters;

#region ### Class BooleanConverterTests ###
/// <summary>
/// Tests boolean-related VIA.WPF converters.
/// </summary>
public sealed class BooleanConverterTests
{
    #region ### Private Fields ###
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
    #endregion

    #region ### Tests ###
    /// <summary>
    /// Verifies that <see cref="InverseBooleanConverter" /> inverts boolean-like values.
    /// </summary>
    [Fact]
    public void InverseBooleanConverter_ShouldInvertBooleanValues()
    {
        InverseBooleanConverter converter = new();

        Assert.False((bool)converter.Convert(true, typeof(bool), null, Culture));
        Assert.True((bool)converter.Convert(false, typeof(bool), null, Culture));
        Assert.True((bool)converter.Convert(null, typeof(bool), null, Culture));
        Assert.False((bool)converter.ConvertBack(true, typeof(bool), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="BooleanAndConverter" /> returns true only when all relevant values are true.
    /// </summary>
    [Fact]
    public void BooleanAndConverter_ShouldReturnTrueOnlyWhenAllValuesAreTrue()
    {
        BooleanAndConverter converter = new();

        Assert.True((bool)converter.Convert([true, 1, "true"], typeof(bool), null, Culture));
        Assert.False((bool)converter.Convert([true, 0], typeof(bool), null, Culture));
        Assert.False((bool)converter.Convert([], typeof(bool), null, Culture));
        Assert.True((bool)converter.Convert([DependencyProperty.UnsetValue, true], typeof(bool), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="BooleanOrConverter" /> returns true when at least one relevant value is true.
    /// </summary>
    [Fact]
    public void BooleanOrConverter_ShouldReturnTrueWhenAnyValueIsTrue()
    {
        BooleanOrConverter converter = new();

        Assert.True((bool)converter.Convert([false, 1], typeof(bool), null, Culture));
        Assert.False((bool)converter.Convert([false, 0, "false"], typeof(bool), null, Culture));
        Assert.False((bool)converter.Convert([DependencyProperty.UnsetValue], typeof(bool), null, Culture));
    }

    /// <summary>
    /// Verifies that multi-value boolean converters return <see cref="Binding.DoNothing" /> for ConvertBack.
    /// </summary>
    [Fact]
    public void MultiBooleanConverters_ConvertBack_ShouldReturnDoNothingValues()
    {
        BooleanAndConverter converter = new();

        object[] result = converter.ConvertBack(true, [typeof(bool), typeof(bool)], null, Culture);

        Assert.Equal(2, result.Length);
        Assert.All(result, item => Assert.Same(Binding.DoNothing, item));
    }

    /// <summary>
    /// Verifies that <see cref="BooleanToOpacityConverter" /> maps boolean values to opacity values.
    /// </summary>
    [Fact]
    public void BooleanToOpacityConverter_ShouldMapBooleanValuesToOpacityValues()
    {
        BooleanToOpacityConverter converter = new()
        {
            TrueOpacity = 0.9,
            FalseOpacity = 0.2
        };

        Assert.Equal(0.9, (double)converter.Convert(true, typeof(double), null, Culture));
        Assert.Equal(0.2, (double)converter.Convert(false, typeof(double), null, Culture));
        Assert.True((bool)converter.ConvertBack(0.9, typeof(bool), null, Culture));
        Assert.False((bool)converter.ConvertBack(0.2, typeof(bool), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="BooleanToGridLengthConverter" /> maps boolean values to grid lengths.
    /// </summary>
    [Fact]
    public void BooleanToGridLengthConverter_ShouldMapBooleanValuesToGridLengths()
    {
        BooleanToGridLengthConverter converter = new();

        GridLength trueLength = (GridLength)converter.Convert(true, typeof(GridLength), "280|0", Culture);
        GridLength falseLength = (GridLength)converter.Convert(false, typeof(GridLength), "280|0", Culture);

        Assert.Equal(new GridLength(280), trueLength);
        Assert.Equal(new GridLength(0), falseLength);
        Assert.True((bool)converter.ConvertBack(new GridLength(1), typeof(bool), null, Culture));
        Assert.False((bool)converter.ConvertBack(new GridLength(0), typeof(bool), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="BooleanToThicknessConverter" /> maps boolean values to thickness values.
    /// </summary>
    [Fact]
    public void BooleanToThicknessConverter_ShouldMapBooleanValuesToThicknessValues()
    {
        BooleanToThicknessConverter converter = new();

        Thickness trueThickness = (Thickness)converter.Convert(true, typeof(Thickness), "1,2,3,4|5", Culture);
        Thickness falseThickness = (Thickness)converter.Convert(false, typeof(Thickness), "1,2,3,4|5", Culture);

        Assert.Equal(new Thickness(1, 2, 3, 4), trueThickness);
        Assert.Equal(new Thickness(5), falseThickness);
        Assert.Same(Binding.DoNothing, converter.ConvertBack(new Thickness(1), typeof(bool), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="NullToBooleanConverter" /> maps null and non-null values.
    /// </summary>
    [Fact]
    public void NullToBooleanConverter_ShouldMapNullAndNonNullValues()
    {
        NullToBooleanConverter converter = new();

        Assert.True((bool)converter.Convert(null, typeof(bool), null, Culture));
        Assert.False((bool)converter.Convert("Text", typeof(bool), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(true, typeof(object), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="NotNullToBooleanConverter" /> maps null and non-null values.
    /// </summary>
    [Fact]
    public void NotNullToBooleanConverter_ShouldMapNullAndNonNullValues()
    {
        NotNullToBooleanConverter converter = new();

        Assert.False((bool)converter.Convert(null, typeof(bool), null, Culture));
        Assert.True((bool)converter.Convert("Text", typeof(bool), null, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(true, typeof(object), null, Culture));
    }

    /// <summary>
    /// Verifies that <see cref="EqualityToBooleanConverter" /> compares values using parameter conversion.
    /// </summary>
    [Fact]
    public void EqualityToBooleanConverter_ShouldCompareValuesUsingParameterConversion()
    {
        EqualityToBooleanConverter converter = new();

        Assert.True((bool)converter.Convert(42, typeof(bool), "42", Culture));
        Assert.False((bool)converter.Convert(42, typeof(bool), "43", Culture));
        Assert.Equal(42, converter.ConvertBack(true, typeof(int), "42", Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(false, typeof(int), "42", Culture));
    }

    /// <summary>
    /// Verifies that <see cref="EqualityToBooleanConverter" /> supports inverted comparison.
    /// </summary>
    [Fact]
    public void EqualityToBooleanConverter_ShouldSupportInvertedComparison()
    {
        EqualityToBooleanConverter converter = new()
        {
            Invert = true
        };

        Assert.False((bool)converter.Convert("A", typeof(bool), "A", Culture));
        Assert.True((bool)converter.Convert("A", typeof(bool), "B", Culture));
        Assert.Equal("A", converter.ConvertBack(false, typeof(string), "A", Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(true, typeof(string), "A", Culture));
    }

    /// <summary>
    /// Verifies that <see cref="ObjectReferenceEqualsConverter" /> compares references instead of values.
    /// </summary>
    [Fact]
    public void ObjectReferenceEqualsConverter_ShouldCompareObjectReferences()
    {
        object source = new();
        object other = new();
        ObjectReferenceEqualsConverter converter = new();

        Assert.True((bool)converter.Convert(source, typeof(bool), source, Culture));
        Assert.False((bool)converter.Convert(source, typeof(bool), other, Culture));
        Assert.Same(Binding.DoNothing, converter.ConvertBack(true, typeof(object), source, Culture));
    }
    #endregion
}
#endregion
