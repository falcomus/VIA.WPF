// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLocExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Resources;
using System.Windows.Data;
using System.Windows.Markup;
using VIA.WPF.Localization;

namespace VIA.WPF.Extensions;

#region ### Class XLocExtension ###
/// <summary>
/// Creates a dynamic localization binding for an application resource key.
/// </summary>
/// <example>
/// <code>
/// Text="{via:XLoc Save, ResourceManager={x:Static resources:Strings.ResourceManager}, Fallback=Save}"
/// </code>
/// </example>
[MarkupExtensionReturnType(typeof(string))]
public sealed class XLocExtension : MarkupExtension
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XLocExtension"/> class.
    /// </summary>
    public XLocExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XLocExtension"/> class.
    /// </summary>
    /// <param name="key">The resource key.</param>
    public XLocExtension(string key)
    {
        this.Key = key;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the resource key.
    /// </summary>
    [ConstructorArgument("key")]
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets the application resource manager.
    /// </summary>
    public ResourceManager? ResourceManager { get; set; }

    /// <summary>
    /// Gets or sets the optional fallback text.
    /// </summary>
    public string? Fallback { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        if (this.ResourceManager is null || string.IsNullOrWhiteSpace(this.Key))
        {
            return this.Fallback ?? this.Key ?? string.Empty;
        }

        XLocalizedString localizedString = new(
            this.ResourceManager,
            this.Key,
            this.Fallback);

        Binding binding = new(nameof(XLocalizedString.Value))
        {
            Source = localizedString,
            Mode = BindingMode.OneWay
        };

        return binding.ProvideValue(serviceProvider);
    }
    #endregion
}
#endregion
