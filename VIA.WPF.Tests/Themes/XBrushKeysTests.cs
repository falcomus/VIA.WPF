// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XBrushKeysTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using VIA.WPF.Themes;

namespace VIA.WPF.Tests.Themes;

#region ### Class XBrushKeysTests ###
/// <summary>
/// Tests strongly typed brush resource keys.
/// </summary>
public sealed class XBrushKeysTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that every public brush key property returns a component resource key owned by <see cref="XBrushKeys"/>.
    /// </summary>
    [Fact]
    public void PublicBrushKeyProperties_ShouldReturnComponentResourceKeysOwnedByXBrushKeys()
    {
        PropertyInfo[] properties = GetBrushKeyProperties();

        Assert.NotEmpty(properties);

        foreach (PropertyInfo property in properties)
        {
            ComponentResourceKey key = Assert.IsType<ComponentResourceKey>(property.GetValue(null));

            Assert.Equal(typeof(XBrushKeys), key.TypeInTargetAssembly);
            Assert.Equal(property.Name, key.ResourceId);
        }
    }

    /// <summary>
    /// Verifies that all public brush key properties expose unique resource keys.
    /// </summary>
    [Fact]
    public void PublicBrushKeyProperties_ShouldExposeUniqueKeys()
    {
        ComponentResourceKey[] keys = GetBrushKeyProperties()
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
    private static PropertyInfo[] GetBrushKeyProperties()
    {
        return typeof(XBrushKeys)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(property => property.PropertyType == typeof(ComponentResourceKey))
            .OrderBy(property => property.Name)
            .ToArray();
    }
    #endregion
}
#endregion
