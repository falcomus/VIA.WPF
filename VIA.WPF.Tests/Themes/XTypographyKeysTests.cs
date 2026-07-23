// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTypographyKeysTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XTypographyKeysTests ###
/// <summary>
/// Tests strongly typed typography resource keys.
/// </summary>
public sealed class XTypographyKeysTests
{
    #region ### Tests ###

    /// <summary>
    /// Verifies that every public typography key property returns a component resource key owned by <see cref="XTypographyKeys"/>.
    /// </summary>
    [Fact]
    public void PublicTypographyKeyProperties_ShouldReturnComponentResourceKeysOwnedByXTypographyKeys()
    {
        PropertyInfo[] properties = GetTypographyKeyProperties();

        Assert.NotEmpty(properties);

        foreach (PropertyInfo property in properties)
        {
            ComponentResourceKey key = Assert.IsType<ComponentResourceKey>(property.GetValue(null));

            Assert.Equal(typeof(XTypographyKeys), key.TypeInTargetAssembly);
            Assert.Equal(property.Name, key.ResourceId);
        }
    }

    /// <summary>
    /// Verifies that all public typography key properties expose unique resource keys.
    /// </summary>
    [Fact]
    public void PublicTypographyKeyProperties_ShouldExposeUniqueKeys()
    {
        ComponentResourceKey[] keys = GetTypographyKeyProperties()
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

    private static PropertyInfo[] GetTypographyKeyProperties()
    {
        return typeof(XTypographyKeys)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(property => property.PropertyType == typeof(ComponentResourceKey))
            .OrderBy(property => property.Name)
            .ToArray();
    }

    #endregion
}
#endregion
