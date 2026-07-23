// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBindExtensionTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions.MarkupExtensions;

#region ### Class XBindExtensionTests ###
/// <summary>
/// Contains tests for the <see cref="XBindExtension"/> class.
/// </summary>
public sealed class XBindExtensionTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that the default binding settings match the VIA.WPF validation defaults.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldCreateBindingWithDefaultSettings()
    {
        Binding binding = CreateBinding(new XBindExtension("Name"));

        Assert.Equal("Name", binding.Path.Path);
        Assert.Equal(BindingMode.TwoWay, binding.Mode);
        Assert.Equal(UpdateSourceTrigger.PropertyChanged, binding.UpdateSourceTrigger);
        Assert.True(binding.ValidatesOnNotifyDataErrors);
        Assert.False(binding.ValidatesOnDataErrors);
        Assert.False(binding.ValidatesOnExceptions);
        Assert.True(binding.NotifyOnValidationError);
        Assert.Equal(0, binding.Delay);
        Assert.False(binding.IsAsync);
    }

    /// <summary>
    /// Ensures that an empty path creates a binding without an explicit path.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldCreatePathlessBindingWhenPathIsEmpty()
    {
        XBindExtension extension = new(string.Empty);

        Binding binding = Assert.IsType<Binding>(extension.ProvideValue(null!));

        Assert.Null(binding.Path);
        Assert.Equal(BindingMode.TwoWay, binding.Mode);
        Assert.Equal(UpdateSourceTrigger.PropertyChanged, binding.UpdateSourceTrigger);
    }

    /// <summary>
    /// Ensures that configured binding settings are copied to the generated binding.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldCopyConfiguredBindingSettings()
    {
        object source = new();
        TestValueConverter converter = new();
        CultureInfo culture = CultureInfo.InvariantCulture;
        object converterParameter = new();
        object fallbackValue = "Fallback";
        object targetNullValue = "NullValue";
        XBindExtension extension = new("Name")
        {
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
            ValidatesOnNotifyDataErrors = false,
            ValidatesOnDataErrors = true,
            ValidatesOnExceptions = true,
            NotifyOnValidationError = false,
            Source = source,
            Converter = converter,
            ConverterParameter = converterParameter,
            ConverterCulture = culture,
            FallbackValue = fallbackValue,
            TargetNullValue = targetNullValue,
            Delay = 250,
            IsAsync = true,
            StringFormat = "Value: {0}"
        };

        Binding binding = CreateBinding(extension);

        Assert.Equal("Name", binding.Path.Path);
        Assert.Equal(BindingMode.OneWay, binding.Mode);
        Assert.Equal(UpdateSourceTrigger.LostFocus, binding.UpdateSourceTrigger);
        Assert.False(binding.ValidatesOnNotifyDataErrors);
        Assert.True(binding.ValidatesOnDataErrors);
        Assert.True(binding.ValidatesOnExceptions);
        Assert.False(binding.NotifyOnValidationError);
        Assert.Same(source, binding.Source);
        Assert.Same(converter, binding.Converter);
        Assert.Same(converterParameter, binding.ConverterParameter);
        Assert.Same(culture, binding.ConverterCulture);
        Assert.Same(fallbackValue, binding.FallbackValue);
        Assert.Same(targetNullValue, binding.TargetNullValue);
        Assert.Equal(250, binding.Delay);
        Assert.True(binding.IsAsync);
        Assert.Equal("Value: {0}", binding.StringFormat);
    }

    /// <summary>
    /// Ensures that negative delays are clamped to zero.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldClampNegativeDelayToZero()
    {
        XBindExtension extension = new("Name")
        {
            Delay = -50
        };

        Binding binding = CreateBinding(extension);

        Assert.Equal(0, binding.Delay);
    }

    /// <summary>
    /// Ensures that element name sources are copied to the generated binding.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldCopyElementName()
    {
        XBindExtension extension = new("Name")
        {
            ElementName = "SourceElement"
        };

        Binding binding = CreateBinding(extension);

        Assert.Equal("SourceElement", binding.ElementName);
    }

    /// <summary>
    /// Ensures that relative sources are copied to the generated binding.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldCopyRelativeSource()
    {
        RelativeSource relativeSource = new(RelativeSourceMode.Self);
        XBindExtension extension = new("Name")
        {
            RelativeSource = relativeSource
        };

        Binding binding = CreateBinding(extension);

        Assert.Same(relativeSource, binding.RelativeSource);
    }
    #endregion

    #region ### Private Methods ###
    private static Binding CreateBinding(XBindExtension extension)
    {
        object value = extension.ProvideValue(new XamlServiceProviderStub());
        return Assert.IsType<Binding>(value);
    }
    #endregion

    #region ### Nested Types ###
    private sealed class TestValueConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value ?? DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value ?? DependencyProperty.UnsetValue;
        }
    }
    #endregion
}
#endregion
