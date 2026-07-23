// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindingAndResourceDictionaryExtensionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VIA.WPF.Extensions;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class BindingAndResourceDictionaryExtensionsTests ###
/// <summary>
/// Provides tests for binding and resource dictionary extension helpers.
/// </summary>
public sealed class BindingAndResourceDictionaryExtensionsTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that binding helpers set properties fluently and return the same binding instance.
    /// </summary>
    [Fact]
    public void BindingExtensions_ShouldConfigureBindingFluently()
    {
        Binding binding = new Binding("Name");
        TestValueConverter converter = new TestValueConverter();
        CultureInfo culture = CultureInfo.InvariantCulture;

        Binding result = binding
            .WithMode(BindingMode.TwoWay)
            .WithUpdateSourceTrigger(UpdateSourceTrigger.PropertyChanged)
            .WithFallbackValue("Fallback")
            .WithTargetNullValue("Null")
            .WithConverter(converter, "Parameter", culture)
            .WithIsAsync();

        Assert.Same(binding, result);
        Assert.Equal(BindingMode.TwoWay, binding.Mode);
        Assert.Equal(UpdateSourceTrigger.PropertyChanged, binding.UpdateSourceTrigger);
        Assert.Equal("Fallback", binding.FallbackValue);
        Assert.Equal("Null", binding.TargetNullValue);
        Assert.Same(converter, binding.Converter);
        Assert.Equal("Parameter", binding.ConverterParameter);
        Assert.Same(culture, binding.ConverterCulture);
        Assert.True(binding.IsAsync);
    }

    /// <summary>
    /// Ensures that binding helpers reject null arguments where required.
    /// </summary>
    [Fact]
    public void BindingExtensions_ShouldRejectNullArguments()
    {
        Binding? binding = null;
        Binding validBinding = new Binding();

        Assert.Throws<ArgumentNullException>(() => binding!.WithMode(BindingMode.OneWay));
        Assert.Throws<ArgumentNullException>(() => binding!.WithUpdateSourceTrigger(UpdateSourceTrigger.PropertyChanged));
        Assert.Throws<ArgumentNullException>(() => binding!.WithFallbackValue("Fallback"));
        Assert.Throws<ArgumentNullException>(() => binding!.WithTargetNullValue("Null"));
        Assert.Throws<ArgumentNullException>(() => binding!.WithConverter(new TestValueConverter()));
        Assert.Throws<ArgumentNullException>(() => validBinding.WithConverter(null!));
        Assert.Throws<ArgumentNullException>(() => binding!.WithIsAsync());
    }

    /// <summary>
    /// Ensures that resources can be resolved from direct and merged dictionaries.
    /// </summary>
    [Fact]
    public void ResourceDictionaryExtensions_TryGetResource_ShouldResolveDirectAndMergedResources()
    {
        ResourceDictionary root = new ResourceDictionary();
        ResourceDictionary firstMerged = new ResourceDictionary();
        ResourceDictionary secondMerged = new ResourceDictionary();
        firstMerged["Name"] = "First";
        secondMerged["Name"] = "Second";
        root["Direct"] = 42;
        root.MergedDictionaries.Add(firstMerged);
        root.MergedDictionaries.Add(secondMerged);

        Assert.True(root.TryGetResource("Direct", out int directValue));
        Assert.Equal(42, directValue);
        Assert.True(root.TryGetResource("Name", out string? mergedValue));
        Assert.Equal("Second", mergedValue);
        Assert.False(root.TryGetResource("Missing", out string? missingValue));
        Assert.Null(missingValue);

        ResourceDictionary? nullDictionary = null;
        Assert.False(nullDictionary.TryGetResource("Direct", out int _));
    }

    /// <summary>
    /// Ensures that color and brush resources are set and frozen when requested.
    /// </summary>
    [Fact]
    public void ResourceDictionaryExtensions_SetColorAndSetBrush_ShouldStoreResources()
    {
        ResourceDictionary dictionary = new ResourceDictionary();

        dictionary.SetColor("Color", Colors.Red);
        dictionary.SetBrush("FrozenBrush", Colors.Blue);
        dictionary.SetBrush("MutableBrush", Colors.Green, freeze: false);

        Assert.Equal(Colors.Red, dictionary["Color"]);
        SolidColorBrush frozenBrush = Assert.IsType<SolidColorBrush>(dictionary["FrozenBrush"]);
        SolidColorBrush mutableBrush = Assert.IsType<SolidColorBrush>(dictionary["MutableBrush"]);
        Assert.Equal(Colors.Blue, frozenBrush.Color);
        Assert.True(frozenBrush.IsFrozen);
        Assert.Equal(Colors.Green, mutableBrush.Color);
        Assert.False(mutableBrush.IsFrozen);
    }

    /// <summary>
    /// Ensures that merged dictionaries can be found, replaced and removed recursively.
    /// </summary>
    [Fact]
    public void ResourceDictionaryExtensions_ShouldFindReplaceAndRemoveMergedDictionariesRecursively()
    {
        ResourceDictionary root = new ResourceDictionary();
        ResourceDictionary child = CreateDictionary("child");
        ResourceDictionary nested = CreateDictionary("nested");
        ResourceDictionary keep = CreateDictionary("keep");
        child.MergedDictionaries.Add(nested);
        root.MergedDictionaries.Add(child);
        root.MergedDictionaries.Add(keep);

        Assert.Same(nested, root.FindMergedDictionary(dictionary => Equals(dictionary["Id"], "nested")));

        int replaced = root.ReplaceMergedDictionaries(
            dictionary => Equals(dictionary["Id"], "nested"),
            () => CreateDictionary("replacement"));

        Assert.Equal(1, replaced);
        Assert.NotNull(root.FindMergedDictionary(dictionary => Equals(dictionary["Id"], "replacement")));
        Assert.Null(root.FindMergedDictionary(dictionary => Equals(dictionary["Id"], "nested")));

        int removed = root.RemoveMergedDictionaries(dictionary => Equals(dictionary["Id"], "replacement"));

        Assert.Equal(1, removed);
        Assert.Null(root.FindMergedDictionary(dictionary => Equals(dictionary["Id"], "replacement")));
        Assert.Same(keep, root.FindMergedDictionary(dictionary => Equals(dictionary["Id"], "keep")));
    }

    /// <summary>
    /// Ensures that resource dictionary helpers reject null arguments where required.
    /// </summary>
    [Fact]
    public void ResourceDictionaryExtensions_ShouldRejectNullArguments()
    {
        ResourceDictionary? dictionary = null;
        ResourceDictionary validDictionary = new ResourceDictionary();

        Assert.Throws<ArgumentNullException>(() => dictionary!.SetColor("Key", Colors.Red));
        Assert.Throws<ArgumentNullException>(() => validDictionary.SetColor(null!, Colors.Red));
        Assert.Throws<ArgumentNullException>(() => dictionary!.SetBrush("Key", Colors.Red));
        Assert.Throws<ArgumentNullException>(() => validDictionary.SetBrush(null!, Colors.Red));
        Assert.Throws<ArgumentNullException>(() => validDictionary.FindMergedDictionary(null!));
        Assert.Throws<ArgumentNullException>(() => dictionary!.ReplaceMergedDictionaries(_ => true, () => new ResourceDictionary()));
        Assert.Throws<ArgumentNullException>(() => validDictionary.ReplaceMergedDictionaries(null!, () => new ResourceDictionary()));
        Assert.Throws<ArgumentNullException>(() => validDictionary.ReplaceMergedDictionaries(_ => true, null!));
        Assert.Throws<ArgumentNullException>(() => dictionary!.RemoveMergedDictionaries(_ => true));
        Assert.Throws<ArgumentNullException>(() => validDictionary.RemoveMergedDictionaries(null!));
    }
    #endregion

    #region ### Private Methods ###
    private static ResourceDictionary CreateDictionary(string id)
    {
        ResourceDictionary dictionary = new ResourceDictionary();
        dictionary["Id"] = id;
        return dictionary;
    }
    #endregion

    #region ### Test Types ###
    private sealed class TestValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    #endregion
}
#endregion
