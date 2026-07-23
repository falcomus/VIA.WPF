// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XIconAssist.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VIA.WPF.Controls;

#region ### Class XIconAssist ###
/// <summary>
/// Provides shared attached properties and helpers for icon presentation.
/// </summary>
public static class XIconAssist
{
    #region ### Public Fields ###
    /// <summary>
    /// Identifies the Rotation attached dependency property.
    /// </summary>
    public static readonly DependencyProperty RotationProperty = DependencyProperty.RegisterAttached(
        "Rotation",
        typeof(double),
        typeof(XIconAssist),
        new FrameworkPropertyMetadata(
            0d,
            FrameworkPropertyMetadataOptions.AffectsRender,
            OnRotationChanged));

    /// <summary>
    /// Identifies the IsRotationAnimated attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsRotationAnimatedProperty = DependencyProperty.RegisterAttached(
        "IsRotationAnimated",
        typeof(bool),
        typeof(XIconAssist),
        new FrameworkPropertyMetadata(false, OnIsRotationAnimatedChanged));

    /// <summary>
    /// Identifies the RotationAnimationDuration attached dependency property.
    /// </summary>
    public static readonly DependencyProperty RotationAnimationDurationProperty = DependencyProperty.RegisterAttached(
        "RotationAnimationDuration",
        typeof(Duration),
        typeof(XIconAssist),
        new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(900)), OnRotationAnimationDurationChanged));

    /// <summary>
    /// Identifies the Size attached dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.RegisterAttached(
        "Size",
        typeof(double),
        typeof(XIconAssist),
        new FrameworkPropertyMetadata(
            double.NaN,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnSizeChanged));

    /// <summary>
    /// Identifies the Foreground attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(
        "Foreground",
        typeof(Brush),
        typeof(XIconAssist),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsRender,
            OnForegroundChanged));
    #endregion

    #region ### Private Fields ###
    private static readonly DependencyProperty IsManagedRotationTransformProperty = DependencyProperty.RegisterAttached(
        "IsManagedRotationTransform",
        typeof(bool),
        typeof(XIconAssist),
        new PropertyMetadata(false));
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Gets the icon rotation in degrees.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured rotation in degrees.</returns>
    public static double GetRotation(DependencyObject element)
    {
        return (double)element.GetValue(RotationProperty);
    }

    /// <summary>
    /// Sets the icon rotation in degrees.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The rotation in degrees.</param>
    public static void SetRotation(DependencyObject element, double value)
    {
        element.SetValue(RotationProperty, value);
    }

    /// <summary>
    /// Gets whether the icon rotation is animated continuously.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns><c>true</c> when the rotation is animated; otherwise <c>false</c>.</returns>
    public static bool GetIsRotationAnimated(DependencyObject element)
    {
        return (bool)element.GetValue(IsRotationAnimatedProperty);
    }

    /// <summary>
    /// Sets whether the icon rotation is animated continuously.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetIsRotationAnimated(DependencyObject element, bool value)
    {
        element.SetValue(IsRotationAnimatedProperty, value);
    }

    /// <summary>
    /// Gets the rotation animation duration.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured animation duration.</returns>
    public static Duration GetRotationAnimationDuration(DependencyObject element)
    {
        return (Duration)element.GetValue(RotationAnimationDurationProperty);
    }

    /// <summary>
    /// Sets the rotation animation duration.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The duration to set.</param>
    public static void SetRotationAnimationDuration(DependencyObject element, Duration value)
    {
        element.SetValue(RotationAnimationDurationProperty, value);
    }

    /// <summary>
    /// Gets the icon size.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured icon size.</returns>
    public static double GetSize(DependencyObject element)
    {
        return (double)element.GetValue(SizeProperty);
    }

    /// <summary>
    /// Sets the icon size.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The icon size.</param>
    public static void SetSize(DependencyObject element, double value)
    {
        element.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets the icon foreground brush.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <returns>The configured foreground brush.</returns>
    public static Brush? GetForeground(DependencyObject element)
    {
        return (Brush?)element.GetValue(ForegroundProperty);
    }

    /// <summary>
    /// Sets the icon foreground brush.
    /// </summary>
    /// <param name="element">The target element.</param>
    /// <param name="value">The foreground brush.</param>
    public static void SetForeground(DependencyObject element, Brush? value)
    {
        element.SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// Applies a numeric icon size to common WPF icon controls.
    /// </summary>
    /// <param name="icon">The icon object.</param>
    /// <param name="iconSize">The icon size.</param>
    public static void ApplySize(object? icon, double iconSize)
    {
        if (icon is null || double.IsNaN(iconSize) || iconSize <= 0d)
        {
            return;
        }

        if (icon is DependencyObject dependencyObject)
        {
            TrySetDependencyProperty(dependencyObject, "SizeProperty", iconSize);
        }

        if (icon is FrameworkElement frameworkElement)
        {
            frameworkElement.Width = iconSize;
            frameworkElement.Height = iconSize;
        }
    }

    /// <summary>
    /// Applies a foreground brush to common WPF icon controls.
    /// </summary>
    /// <param name="icon">The icon object.</param>
    /// <param name="foreground">The foreground brush.</param>
    public static void ApplyForeground(object? icon, Brush? foreground)
    {
        if (icon is not DependencyObject dependencyObject || foreground is null)
        {
            return;
        }

        TrySetDependencyProperty(dependencyObject, "ForegroundProperty", foreground);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnRotationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not FrameworkElement element || element is XButton)
        {
            return;
        }

        double rotation = e.NewValue is double value ? value : 0d;
        ApplyRotation(element, rotation);
    }

    private static void OnIsRotationAnimatedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        UpdateRotationAnimation(dependencyObject);
    }

    private static void OnRotationAnimationDurationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (GetIsRotationAnimated(dependencyObject))
        {
            UpdateRotationAnimation(dependencyObject);
        }
    }

    private static void OnSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not FrameworkElement element || e.NewValue is not double size)
        {
            return;
        }

        ApplySize(element, size);
    }

    private static void OnForegroundChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not FrameworkElement element || e.NewValue is not Brush foreground)
        {
            return;
        }

        ApplyForeground(element, foreground);
    }

    private static void ApplyRotation(FrameworkElement element, double rotation)
    {
        bool isManagedTransform = (bool)element.GetValue(IsManagedRotationTransformProperty);

        if (Math.Abs(rotation) < double.Epsilon)
        {
            if (isManagedTransform)
            {
                element.RenderTransform = Transform.Identity;
                element.ClearValue(IsManagedRotationTransformProperty);
            }

            return;
        }

        object localTransformValue = element.ReadLocalValue(UIElement.RenderTransformProperty);
        bool hasExternalTransform = !isManagedTransform
            && localTransformValue != DependencyProperty.UnsetValue
            && !ReferenceEquals(element.RenderTransform, Transform.Identity);

        if (hasExternalTransform)
        {
            return;
        }

        element.RenderTransformOrigin = new Point(0.5d, 0.5d);
        element.RenderTransform = new RotateTransform(rotation);
        element.SetValue(IsManagedRotationTransformProperty, true);
    }

    private static void UpdateRotationAnimation(DependencyObject dependencyObject)
    {
        if (dependencyObject is not FrameworkElement element)
        {
            return;
        }

        if (!GetIsRotationAnimated(dependencyObject))
        {
            element.BeginAnimation(RotationProperty, null);
            ApplyRotationAfterAnimationStop(element);
            return;
        }

        Duration duration = GetRotationAnimationDuration(dependencyObject);
        if (!duration.HasTimeSpan || duration.TimeSpan <= TimeSpan.Zero)
        {
            duration = new Duration(TimeSpan.FromMilliseconds(900));
        }

        double startRotation = GetRotation(dependencyObject);
        DoubleAnimation animation = new()
        {
            From = startRotation,
            To = startRotation + 360d,
            Duration = duration,
            RepeatBehavior = RepeatBehavior.Forever,
            EasingFunction = null
        };

        element.BeginAnimation(RotationProperty, animation, HandoffBehavior.SnapshotAndReplace);
    }

    private static void ApplyRotationAfterAnimationStop(FrameworkElement element)
    {
        if (element is XButton)
        {
            return;
        }

        ApplyRotation(element, GetRotation(element));
    }

    private static void TrySetDependencyProperty(DependencyObject dependencyObject, string propertyFieldName, double value)
    {
        TrySetDependencyProperty(dependencyObject, propertyFieldName, value, typeof(double));
    }

    private static void TrySetDependencyProperty(DependencyObject dependencyObject, string propertyFieldName, Brush value)
    {
        TrySetDependencyProperty(dependencyObject, propertyFieldName, value, typeof(Brush));
    }

    private static void TrySetDependencyProperty(DependencyObject dependencyObject, string propertyFieldName, object value, Type expectedPropertyType)
    {
        FieldInfo? fieldInfo = dependencyObject.GetType().GetField(
            propertyFieldName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        if (fieldInfo?.GetValue(null) is not DependencyProperty dependencyProperty
            || !expectedPropertyType.IsAssignableFrom(dependencyProperty.PropertyType))
        {
            return;
        }

        dependencyObject.SetValue(dependencyProperty, value);
    }
    #endregion
}
#endregion
