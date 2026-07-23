// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XControlDependencyPropertySmokeTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using VIA.WPF.Controls;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.Controls.Smoke;

#region ### Class XControlDependencyPropertySmokeTests ###
/// <summary>
/// Provides broad smoke tests for public VIA.WPF WPF controls and their own dependency properties.
/// </summary>
public sealed class XControlDependencyPropertySmokeTests
{
    #region ### Private Fields ###
    private static readonly Type[] PublicDependencyObjectTypes = LoadPublicDependencyObjectTypes();
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets all public dependency-object control types with a public parameterless constructor.
    /// </summary>
    /// <returns>The theory data.</returns>
    public static TheoryData<Type> GetPublicDependencyObjectTypes()
    {
        TheoryData<Type> data = [];

        foreach (Type controlType in PublicDependencyObjectTypes)
        {
            data.Add(controlType);
        }

        return data;
    }

    /// <summary>
    /// Gets all dependency properties declared directly by public VIA.WPF dependency-object control types.
    /// </summary>
    /// <returns>The theory data.</returns>
    public static TheoryData<Type, string> GetDeclaredDependencyPropertyFields()
    {
        TheoryData<Type, string> data = [];

        foreach (Type controlType in PublicDependencyObjectTypes)
        {
            foreach (string fieldName in GetDeclaredDependencyPropertyFieldNames(controlType))
            {
                data.Add(controlType, fieldName);
            }
        }

        return data;
    }

    /// <summary>
    /// Ensures that the reflection smoke suite covers a meaningful number of public controls.
    /// </summary>
    [Fact]
    public void PublicDependencyObjectTypes_ShouldContainVIAControls()
    {
        Assert.Contains(typeof(XButton), PublicDependencyObjectTypes);
        Assert.Contains(typeof(XIconButton), PublicDependencyObjectTypes);
        Assert.Contains(typeof(XNumberBox), PublicDependencyObjectTypes);
        Assert.True(PublicDependencyObjectTypes.Length >= 20);
    }

    /// <summary>
    /// Ensures that public dependency-object control types can be constructed on a WPF STA thread.
    /// </summary>
    /// <param name="controlType">The control type.</param>
    [Theory]
    [MemberData(nameof(GetPublicDependencyObjectTypes))]
    public void PublicDependencyObjectTypes_WithDefaultConstructor_ShouldCreateInstance(Type controlType)
    {
        WpfTestHelper.Run(
            () =>
            {
                object? instance = Activator.CreateInstance(controlType);

                Assert.NotNull(instance);
                Assert.IsType<DependencyObject>(instance, exactMatch: false);
            });
    }

    /// <summary>
    /// Ensures that declared dependency properties expose compatible metadata and default values.
    /// </summary>
    /// <param name="ownerType">The owner type.</param>
    /// <param name="dependencyPropertyFieldName">The dependency property field name.</param>
    [Theory]
    [MemberData(nameof(GetDeclaredDependencyPropertyFields))]
    public void DeclaredDependencyProperties_ShouldExposeCompatibleDefaults(Type ownerType, string dependencyPropertyFieldName)
    {
        WpfTestHelper.Run(
            () =>
            {
                DependencyProperty dependencyProperty = ResolveDeclaredDependencyProperty(ownerType, dependencyPropertyFieldName);
                DependencyObject instance = CreateDependencyObject(ownerType);
                PropertyMetadata metadata = dependencyProperty.GetMetadata(ownerType);
                object? currentValue = instance.GetValue(dependencyProperty);

                Assert.NotNull(metadata);
                AssertValueIsCompatible(dependencyProperty.PropertyType, metadata.DefaultValue, ownerType, dependencyProperty.Name, "metadata default value");
                AssertValueIsCompatible(dependencyProperty.PropertyType, currentValue, ownerType, dependencyProperty.Name, "current value");
            });
    }

    /// <summary>
    /// Ensures that declared writable dependency properties accept a safe sample value when one is available.
    /// </summary>
    /// <param name="ownerType">The owner type.</param>
    /// <param name="dependencyPropertyFieldName">The dependency property field name.</param>
    [Theory]
    [MemberData(nameof(GetDeclaredDependencyPropertyFields))]
    public void DeclaredDependencyProperties_WithSafeSampleValue_ShouldAcceptAssignment(Type ownerType, string dependencyPropertyFieldName)
    {
        WpfTestHelper.Run(
            () =>
            {
                DependencyProperty dependencyProperty = ResolveDeclaredDependencyProperty(ownerType, dependencyPropertyFieldName);

                if (dependencyProperty.ReadOnly || !IsSafeForSampleAssignment(ownerType, dependencyPropertyFieldName, dependencyProperty))
                {
                    return;
                }

                if (!TryCreateSampleValue(dependencyProperty.PropertyType, out object? sampleValue))
                {
                    return;
                }

                DependencyObject instance = CreateDependencyObject(ownerType);

                Exception? exception = Record.Exception(() => instance.SetValue(dependencyProperty, sampleValue));

                Assert.Null(exception);
                AssertValueIsCompatible(
                    dependencyProperty.PropertyType,
                    instance.GetValue(dependencyProperty),
                    ownerType,
                    dependencyProperty.Name,
                    "assigned value");
            });
    }
    #endregion

    #region ### Private Methods ###
    private static DependencyObject CreateDependencyObject(Type ownerType)
    {
        object? instance = Activator.CreateInstance(ownerType);

        Assert.NotNull(instance);
        return Assert.IsType<DependencyObject>(instance, exactMatch: false);
    }

    private static IEnumerable<string> GetDeclaredDependencyPropertyFieldNames(Type ownerType)
    {
        return ownerType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(field => field.FieldType == typeof(DependencyProperty))
            .Select(field => field.Name)
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal);
    }

    private static DependencyProperty ResolveDeclaredDependencyProperty(Type ownerType, string fieldName)
    {
        FieldInfo? field = ownerType.GetField(fieldName, BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        Assert.NotNull(field);

        object? value = field.GetValue(null);

        return Assert.IsType<DependencyProperty>(value, exactMatch: false);
    }

    private static Type[] LoadPublicDependencyObjectTypes()
    {
        return [.. typeof(XButton)
            .Assembly
            .GetExportedTypes()
            .Where(type => type is { IsAbstract: false, IsGenericTypeDefinition: false })
            .Where(type => typeof(DependencyObject).IsAssignableFrom(type))
            .Where(type => type.Namespace is not null && type.Namespace.StartsWith("VIA.WPF.Controls", StringComparison.Ordinal))
            .Where(type => type.GetConstructor(Type.EmptyTypes) is not null)
            .OrderBy(type => type.FullName, StringComparer.Ordinal)];
    }

    private static void AssertValueIsCompatible(Type propertyType, object? value, Type ownerType, string propertyName, string valueDescription)
    {
        if (value is null || ReferenceEquals(value, DependencyProperty.UnsetValue))
        {
            return;
        }

        Type effectivePropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        Assert.True(
            effectivePropertyType.IsInstanceOfType(value),
            $"{ownerType.Name}.{propertyName} has an incompatible {valueDescription}. Expected '{propertyType.FullName}', actual '{value.GetType().FullName}'.");
    }

    private static bool TryCreateSampleValue(Type propertyType, out object? value)
    {
        Type effectiveType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (effectiveType == typeof(string))
        {
            value = "Smoke";
            return true;
        }

        if (effectiveType == typeof(bool))
        {
            value = true;
            return true;
        }

        if (effectiveType == typeof(byte))
        {
            value = (byte)1;
            return true;
        }

        if (effectiveType == typeof(short))
        {
            value = (short)1;
            return true;
        }

        if (effectiveType == typeof(int))
        {
            value = 1;
            return true;
        }

        if (effectiveType == typeof(long))
        {
            value = 1L;
            return true;
        }

        if (effectiveType == typeof(float))
        {
            value = 1f;
            return true;
        }

        if (effectiveType == typeof(double))
        {
            value = 1d;
            return true;
        }

        if (effectiveType == typeof(decimal))
        {
            value = 1m;
            return true;
        }

        if (effectiveType == typeof(object))
        {
            value = "Smoke content";
            return true;
        }

        if (effectiveType == typeof(Type))
        {
            value = typeof(object);
            return true;
        }

        if (effectiveType.IsEnum)
        {
            Array values = Enum.GetValues(effectiveType);
            value = values.Length == 0 ? null : values.GetValue(0);
            return value is not null;
        }

        if (effectiveType == typeof(Thickness))
        {
            value = new Thickness(1d, 2d, 3d, 4d);
            return true;
        }

        if (effectiveType == typeof(CornerRadius))
        {
            value = new CornerRadius(4d);
            return true;
        }

        if (effectiveType == typeof(Rect))
        {
            value = new Rect(0d, 0d, 16d, 16d);
            return true;
        }

        if (effectiveType == typeof(Size))
        {
            value = new Size(16d, 16d);
            return true;
        }

        if (effectiveType == typeof(Point))
        {
            value = new Point(4d, 4d);
            return true;
        }

        if (effectiveType == typeof(GridLength))
        {
            value = new GridLength(1d, GridUnitType.Star);
            return true;
        }

        if (effectiveType == typeof(Color))
        {
            value = Colors.CornflowerBlue;
            return true;
        }

        if (effectiveType == typeof(Brush) || effectiveType.IsAssignableFrom(typeof(SolidColorBrush)))
        {
            value = Brushes.CornflowerBlue;
            return true;
        }

        if (effectiveType == typeof(Geometry) || effectiveType.IsAssignableFrom(typeof(RectangleGeometry)))
        {
            value = new RectangleGeometry(new Rect(0d, 0d, 16d, 16d));
            return true;
        }

        if (effectiveType == typeof(Transform) || effectiveType.IsAssignableFrom(typeof(TranslateTransform)))
        {
            value = new TranslateTransform(1d, 1d);
            return true;
        }

        if (effectiveType == typeof(FontFamily))
        {
            value = new FontFamily("Segoe UI");
            return true;
        }

        if (effectiveType == typeof(FontWeight))
        {
            value = FontWeights.SemiBold;
            return true;
        }

        if (effectiveType == typeof(FontStyle))
        {
            value = FontStyles.Italic;
            return true;
        }

        if (effectiveType == typeof(FontStretch))
        {
            value = FontStretches.Normal;
            return true;
        }

        if (effectiveType == typeof(HorizontalAlignment))
        {
            value = HorizontalAlignment.Center;
            return true;
        }

        if (effectiveType == typeof(VerticalAlignment))
        {
            value = VerticalAlignment.Center;
            return true;
        }

        if (effectiveType == typeof(TextAlignment))
        {
            value = TextAlignment.Center;
            return true;
        }

        if (effectiveType == typeof(FlowDirection))
        {
            value = FlowDirection.LeftToRight;
            return true;
        }

        if (effectiveType == typeof(Orientation))
        {
            value = Orientation.Horizontal;
            return true;
        }

        if (effectiveType == typeof(Visibility))
        {
            value = Visibility.Visible;
            return true;
        }

        if (effectiveType == typeof(SelectionMode))
        {
            value = SelectionMode.Single;
            return true;
        }

        if (effectiveType == typeof(ICommand))
        {
            value = new NoOpCommand();
            return true;
        }

        if (effectiveType == typeof(BindingBase) || effectiveType.IsAssignableFrom(typeof(Binding)))
        {
            value = new Binding();
            return true;
        }

        if (typeof(UIElement).IsAssignableFrom(effectiveType) && effectiveType.IsAssignableFrom(typeof(Border)))
        {
            value = new Border();
            return true;
        }

        value = null;
        return false;
    }

    private static bool IsSafeForSampleAssignment(Type ownerType, string fieldName, DependencyProperty dependencyProperty)
    {
        if (ownerType == typeof(global::VIA.WPF.Controls.XGrid)
            && (fieldName == "RowsProperty"
                || fieldName == "ColumnsProperty"
                || fieldName == "AreasProperty"))
        {
            return false;
        }

        Type propertyType = dependencyProperty.PropertyType;

        if (typeof(FrameworkTemplate).IsAssignableFrom(propertyType))
        {
            return false;
        }

        if (typeof(Style).IsAssignableFrom(propertyType))
        {
            return false;
        }

        if (typeof(DataTemplate).IsAssignableFrom(propertyType))
        {
            return false;
        }

        if (typeof(Uri).IsAssignableFrom(propertyType))
        {
            return false;
        }

        return true;
    }
    #endregion

    #region ### Test Types ###
    private sealed class NoOpCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
        }
    }
    #endregion
}
#endregion