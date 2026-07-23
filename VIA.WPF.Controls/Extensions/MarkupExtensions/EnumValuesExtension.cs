// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumValuesExtension.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Markup;

namespace VIA.WPF.Extensions;

#region ### Class EnumValuesExtension ###
/// <summary>
/// Provides all values of an enum type for XAML bindings.
/// </summary>
[MarkupExtensionReturnType(typeof(Array))]
public sealed class EnumValuesExtension : MarkupExtension
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumValuesExtension"/> class.
    /// </summary>
    public EnumValuesExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumValuesExtension"/> class.
    /// </summary>
    /// <param name="enumType">The enum type.</param>
    public EnumValuesExtension(Type enumType)
    {
        this.EnumType = enumType;
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the enum type.
    /// </summary>
    [ConstructorArgument("enumType")]
    public Type? EnumType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether obsolete enum values should be included.
    /// </summary>
    public bool IncludeObsolete { get; set; }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        Type enumType = Nullable.GetUnderlyingType(this.EnumType ?? typeof(object)) ?? (this.EnumType ?? typeof(object));

        if (!enumType.IsEnum)
        {
            throw new InvalidOperationException($"The type '{enumType.FullName}' is not an enum type.");
        }

        Array values = Enum.GetValues(enumType);

        if (this.IncludeObsolete)
        {
            return values;
        }

        object[] filteredValues = [.. values
            .Cast<object>()
            .Where(value => !IsObsolete(enumType, value))];

        Array result = Array.CreateInstance(enumType, filteredValues.Length);

        for (int index = 0; index < filteredValues.Length; index++)
        {
            result.SetValue(filteredValues[index], index);
        }

        return result;
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Gets a value indicating whether the specified enum value is obsolete.
    /// </summary>
    /// <param name="enumType">The enum type.</param>
    /// <param name="value">The enum value.</param>
    /// <returns><c>true</c> if the value is obsolete; otherwise, <c>false</c>.</returns>
    private static bool IsObsolete(Type enumType, object value)
    {
        string? name = Enum.GetName(enumType, value);
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        return enumType.GetField(name)?.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0;
    }
    #endregion
}
#endregion
