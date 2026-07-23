// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XLayoutKeysTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XLayoutKeysTests ###
/// <summary>
/// Tests strongly typed layout resource keys.
/// </summary>
public sealed class XLayoutKeysTests
{
    #region ### Tests ###

    /// <summary>
    /// Verifies that every public layout key property returns a component resource key owned by <see cref="XLayoutKeys"/>.
    /// </summary>
    [Fact]
    public void PublicLayoutKeyProperties_ShouldReturnComponentResourceKeysOwnedByXLayoutKeys()
    {
        PropertyInfo[] properties = GetLayoutKeyProperties();

        Assert.NotEmpty(properties);

        foreach (PropertyInfo property in properties)
        {
            ComponentResourceKey key = Assert.IsType<ComponentResourceKey>(property.GetValue(null));

            Assert.Equal(typeof(XLayoutKeys), key.TypeInTargetAssembly);
            Assert.Equal(property.Name, key.ResourceId);
        }
    }

    /// <summary>
    /// Verifies that all public layout key properties expose unique resource keys.
    /// </summary>
    [Fact]
    public void PublicLayoutKeyProperties_ShouldExposeUniqueKeys()
    {
        ComponentResourceKey[] keys = GetLayoutKeyProperties()
            .Select(property => Assert.IsType<ComponentResourceKey>(property.GetValue(null)))
            .ToArray();

        int uniqueCount = keys
            .Select(key => key.ResourceId)
            .Distinct()
            .Count();

        Assert.Equal(keys.Length, uniqueCount);
    }

    #endregion

    #region ### Private Methods ###

    private static PropertyInfo[] GetLayoutKeyProperties()
    {
        return typeof(XLayoutKeys)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(property => property.PropertyType == typeof(ComponentResourceKey))
            .OrderBy(property => property.Name)
            .ToArray();
    }

    #endregion
}
#endregion
