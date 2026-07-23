// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceOrDefaultExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Markup;

namespace VIA.WPF.Extensions;

#region ### Class ResourceOrDefaultExtension ###
/// <summary>
/// Resolves a resource if available and returns a fallback value otherwise.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public sealed class ResourceOrDefaultExtension : MarkupExtension
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceOrDefaultExtension"/> class.
    /// </summary>
    public ResourceOrDefaultExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceOrDefaultExtension"/> class.
    /// </summary>
    /// <param name="key">The resource key.</param>
    public ResourceOrDefaultExtension(object key)
    {
        this.Key = key;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the resource key.
    /// </summary>
    [ConstructorArgument("key")]
    public object? Key { get; set; }

    /// <summary>
    /// Gets or sets the fallback value used when the resource cannot be found.
    /// </summary>
    public object? DefaultValue { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        if (this.Key is null)
        {
            return this.DefaultValue;
        }

        object? resource = TryFindResource(serviceProvider, this.Key);

        return resource ?? this.DefaultValue;
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Tries to resolve a resource from the current XAML target context.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="key">The resource key.</param>
    /// <returns>The resolved resource or <see langword="null"/>.</returns>
    private static object? TryFindResource(IServiceProvider serviceProvider, object key)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
        {
            if (provideValueTarget.TargetObject is FrameworkElement frameworkElement)
            {
                object resource = frameworkElement.TryFindResource(key);
                if (resource is not null)
                {
                    return resource;
                }
            }

            if (provideValueTarget.TargetObject is FrameworkContentElement frameworkContentElement)
            {
                object resource = frameworkContentElement.TryFindResource(key);
                if (resource is not null)
                {
                    return resource;
                }
            }
        }

        return Application.Current?.TryFindResource(key);
    }
    #endregion
}
#endregion
