// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBindExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VIA.WPF.Extensions;

#region ### Class XBindExtension ###
/// <summary>
/// Provides an VIA.WPF binding shortcut with production-ready validation defaults.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public sealed class XBindExtension : MarkupExtension
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XBindExtension"/> class.
    /// </summary>
    public XBindExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XBindExtension"/> class.
    /// </summary>
    /// <param name="path">The binding path.</param>
    public XBindExtension(string path)
    {
        this.Path = path;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the binding path.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the binding mode.
    /// </summary>
    public BindingMode Mode { get; set; } = BindingMode.TwoWay;

    /// <summary>
    /// Gets or sets the update source trigger.
    /// </summary>
    public UpdateSourceTrigger UpdateSourceTrigger { get; set; } = UpdateSourceTrigger.PropertyChanged;

    /// <summary>
    /// Gets or sets a value indicating whether <see cref="System.ComponentModel.INotifyDataErrorInfo"/> validation is enabled.
    /// </summary>
    public bool ValidatesOnNotifyDataErrors { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether <see cref="System.ComponentModel.IDataErrorInfo"/> validation is enabled.
    /// </summary>
    public bool ValidatesOnDataErrors { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether exception validation is enabled.
    /// </summary>
    public bool ValidatesOnExceptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether validation errors raise binding notifications.
    /// </summary>
    public bool NotifyOnValidationError { get; set; } = true;

    /// <summary>
    /// Gets or sets the binding source.
    /// </summary>
    public object? Source { get; set; }

    /// <summary>
    /// Gets or sets the source element name.
    /// </summary>
    public string? ElementName { get; set; }

    /// <summary>
    /// Gets or sets the relative source.
    /// </summary>
    public RelativeSource? RelativeSource { get; set; }

    /// <summary>
    /// Gets or sets the converter.
    /// </summary>
    public IValueConverter? Converter { get; set; }

    /// <summary>
    /// Gets or sets the converter parameter.
    /// </summary>
    public object? ConverterParameter { get; set; }

    /// <summary>
    /// Gets or sets the converter culture.
    /// </summary>
    public CultureInfo? ConverterCulture { get; set; }

    /// <summary>
    /// Gets or sets the fallback value.
    /// </summary>
    public object? FallbackValue { get; set; } = DependencyProperty.UnsetValue;

    /// <summary>
    /// Gets or sets the target null value.
    /// </summary>
    public object? TargetNullValue { get; set; } = DependencyProperty.UnsetValue;

    /// <summary>
    /// Gets or sets the binding delay in milliseconds.
    /// </summary>
    public int Delay { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the binding is evaluated asynchronously.
    /// </summary>
    public bool IsAsync { get; set; }

    /// <summary>
    /// Gets or sets the string format.
    /// </summary>
    public string? StringFormat { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        Binding binding = string.IsNullOrWhiteSpace(this.Path)
            ? new Binding()
            : new Binding(this.Path);

        binding.Mode = this.Mode;
        binding.UpdateSourceTrigger = this.UpdateSourceTrigger;
        binding.ValidatesOnNotifyDataErrors = this.ValidatesOnNotifyDataErrors;
        binding.ValidatesOnDataErrors = this.ValidatesOnDataErrors;
        binding.ValidatesOnExceptions = this.ValidatesOnExceptions;
        binding.NotifyOnValidationError = this.NotifyOnValidationError;
        binding.Delay = Math.Max(0, this.Delay);
        binding.IsAsync = this.IsAsync;

        if (this.Source is not null)
        {
            binding.Source = this.Source;
        }

        if (!string.IsNullOrWhiteSpace(this.ElementName))
        {
            binding.ElementName = this.ElementName;
        }

        if (this.RelativeSource is not null)
        {
            binding.RelativeSource = this.RelativeSource;
        }

        if (this.Converter is not null)
        {
            binding.Converter = this.Converter;
        }

        if (this.ConverterParameter is not null)
        {
            binding.ConverterParameter = this.ConverterParameter;
        }

        if (this.ConverterCulture is not null)
        {
            binding.ConverterCulture = this.ConverterCulture;
        }

        if (this.FallbackValue != DependencyProperty.UnsetValue)
        {
            binding.FallbackValue = this.FallbackValue;
        }

        if (this.TargetNullValue != DependencyProperty.UnsetValue)
        {
            binding.TargetNullValue = this.TargetNullValue;
        }

        if (!string.IsNullOrWhiteSpace(this.StringFormat))
        {
            binding.StringFormat = this.StringFormat;
        }

        return binding.ProvideValue(serviceProvider);
    }
    #endregion
}
#endregion
