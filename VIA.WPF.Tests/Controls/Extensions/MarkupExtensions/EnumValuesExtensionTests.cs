// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumValuesExtensionTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Extensions;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Extensions.MarkupExtensions;

#region ### Class EnumValuesExtensionTests ###
/// <summary>
/// Contains tests for the <see cref="EnumValuesExtension"/> class.
/// </summary>
public sealed class EnumValuesExtensionTests
{
    #region ### Public Methods ###
    /// <summary>
    /// Ensures that enum values are returned when the configured type is an enum.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldReturnEnumValues()
    {
        EnumValuesExtension extension = new(typeof(TestEnum));

        object value = extension.ProvideValue(new XamlServiceProviderStub());

        TestEnum[] values = Assert.IsType<TestEnum[]>(value);
        Assert.Equal([TestEnum.First, TestEnum.Second], values);
    }

    /// <summary>
    /// Ensures that obsolete enum values are excluded by default.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldExcludeObsoleteValuesByDefault()
    {
        EnumValuesExtension extension = new(typeof(TestEnumWithObsolete));

        object value = extension.ProvideValue(new XamlServiceProviderStub());

        TestEnumWithObsolete[] values = Assert.IsType<TestEnumWithObsolete[]>(value);
        Assert.Equal([TestEnumWithObsolete.First, TestEnumWithObsolete.Second], values);
    }

    /// <summary>
    /// Ensures that obsolete enum values can be included explicitly.
    /// </summary>
    [Fact]
    [Obsolete("Dieser Test demonstriert explizit die Einbeziehung veralteter Enum-Werte f�r Testzwecke.")]
    public void ProvideValue_ShouldIncludeObsoleteValuesWhenConfigured()
    {
        EnumValuesExtension extension = new(typeof(TestEnumWithObsolete))
        {
            IncludeObsolete = true
        };

        object value = extension.ProvideValue(new XamlServiceProviderStub());

        TestEnumWithObsolete[] values = Assert.IsType<TestEnumWithObsolete[]>(value);
        Assert.Equal([TestEnumWithObsolete.First, TestEnumWithObsolete.Old, TestEnumWithObsolete.Second], values);
    }

    /// <summary>
    /// Ensures that nullable enum types are unwrapped.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldSupportNullableEnumTypes()
    {
        EnumValuesExtension extension = new(typeof(TestEnum?));

        object value = extension.ProvideValue(new XamlServiceProviderStub());

        TestEnum[] values = Assert.IsType<TestEnum[]>(value);
        Assert.Equal([TestEnum.First, TestEnum.Second], values);
    }

    /// <summary>
    /// Ensures that a non-enum type is rejected.
    /// </summary>
    [Fact]
    public void ProvideValue_ShouldThrowForNonEnumType()
    {
        EnumValuesExtension extension = new(typeof(string));

        Assert.Throws<InvalidOperationException>(() => extension.ProvideValue(new XamlServiceProviderStub()));
    }
    #endregion

    #region ### Nested Types ###
    private enum TestEnum
    {
        First,
        Second
    }

    private enum TestEnumWithObsolete
    {
        First,

        [Obsolete]
        Old,

        Second
    }
    #endregion
}
#endregion
