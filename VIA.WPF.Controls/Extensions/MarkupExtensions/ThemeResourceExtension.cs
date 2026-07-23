// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeResourceExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace VIA.WPF.Extensions;

#region ### Class ThemeResourceExtension ###
/// <summary>
/// Resolves an VIA.WPF theme resource key by name and returns it as a dynamic resource.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public sealed class ThemeResourceExtension : MarkupExtension
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeResourceExtension"/> class.
    /// </summary>
    public ThemeResourceExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeResourceExtension"/> class.
    /// </summary>
    /// <param name="keyName">The theme key name.</param>
    public ThemeResourceExtension(string keyName)
    {
        this.KeyName = keyName;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the theme key name, for example <c>SelectionBorder</c>.
    /// </summary>
    [ConstructorArgument("keyName")]
    public string? KeyName { get; set; }

    /// <summary>
    /// Gets or sets the fully qualified key provider type name.
    /// </summary>
    public string KeyProviderTypeName { get; set; } = "VIA.WPF.Themes.XBrushKeys";
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        object? resourceKey = ResolveResourceKey(this.KeyProviderTypeName, this.KeyName);

        if (resourceKey is null)
        {
            return DependencyProperty.UnsetValue;
        }

        return new DynamicResourceExtension(resourceKey).ProvideValue(serviceProvider);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Resolves the resource key from the configured key provider.
    /// </summary>
    /// <param name="keyProviderTypeName">The key provider type name.</param>
    /// <param name="keyName">The key name.</param>
    /// <returns>The resource key or <see langword="null"/>.</returns>
    private static object? ResolveResourceKey(string keyProviderTypeName, string? keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            return null;
        }

        Type? keyProviderType = ResolveType(keyProviderTypeName);
        if (keyProviderType is null)
        {
            return keyName;
        }

        const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        PropertyInfo? propertyInfo = keyProviderType.GetProperty(keyName, flags);
        if (propertyInfo is not null)
        {
            return propertyInfo.GetValue(null);
        }

        FieldInfo? fieldInfo = keyProviderType.GetField(keyName, flags);
        if (fieldInfo is not null)
        {
            return fieldInfo.GetValue(null);
        }

        return keyName;
    }

    /// <summary>
    /// Resolves a type by searching loaded assemblies when <see cref="Type.GetType(string)"/> cannot resolve it directly.
    /// </summary>
    /// <param name="typeName">The type name.</param>
    /// <returns>The resolved type or <see langword="null"/>.</returns>
    private static Type? ResolveType(string typeName)
    {
        Type? directType = Type.GetType(typeName, false);
        if (directType is not null)
        {
            return directType;
        }

        return AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(assembly => assembly.GetType(typeName, false))
            .FirstOrDefault(type => type is not null);
    }
    #endregion
}
#endregion
