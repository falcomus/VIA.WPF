// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToggleButton.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace VIA.WPF.Controls;

#region ### Class XToggleButton ###
/// <summary>
/// Represents the standard toggle button control of VIA.WPF.
/// </summary>
public class XToggleButton : ToggleButton
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(XControlVariant.Default));

    /// <summary>
    /// Identifies the <see cref="Appearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(XControlAppearance),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(XControlAppearance.Solid));

    /// <summary>
    /// Identifies the <see cref="CheckedVariant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckedVariantProperty = DependencyProperty.Register(
        nameof(CheckedVariant),
        typeof(XControlVariant),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(XControlVariant.Primary));

    /// <summary>
    /// Identifies the <see cref="CheckedAppearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckedAppearanceProperty = DependencyProperty.Register(
        nameof(CheckedAppearance),
        typeof(XControlAppearance),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(XControlAppearance.Solid));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="UncheckedIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UncheckedIconProperty = DependencyProperty.Register(
        nameof(UncheckedIcon),
        typeof(object),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(null, OnIconChanged));

    /// <summary>
    /// Identifies the <see cref="UncheckedIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UncheckedIconTemplateProperty = DependencyProperty.Register(
        nameof(UncheckedIconTemplate),
        typeof(DataTemplate),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CheckedIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckedIconProperty = DependencyProperty.Register(
        nameof(CheckedIcon),
        typeof(object),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(null, OnIconChanged));

    /// <summary>
    /// Identifies the <see cref="CheckedIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckedIconTemplateProperty = DependencyProperty.Register(
        nameof(CheckedIconTemplate),
        typeof(DataTemplate),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IconPlacement"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
        nameof(IconPlacement),
        typeof(XIconPlacement),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(XIconPlacement.Left));


    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize),
        typeof(double),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(
            XControlSizeMetrics.MediumIconSize,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="StretchContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StretchContentProperty = DependencyProperty.Register(
        nameof(StretchContent),
        typeof(bool),
        typeof(XToggleButton),
        new FrameworkPropertyMetadata(false));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XToggleButton"/> class.
    /// </summary>
    static XToggleButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XToggleButton),
            new FrameworkPropertyMetadata(typeof(XToggleButton)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the corner radius of the button.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic variant of the button.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the appearance of the button.
    /// </summary>
    public XControlAppearance Appearance
    {
        get => (XControlAppearance)this.GetValue(AppearanceProperty);
        set => this.SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic variant applied while the button is checked.
    /// </summary>
    public XControlVariant CheckedVariant
    {
        get => (XControlVariant)this.GetValue(CheckedVariantProperty);
        set => this.SetValue(CheckedVariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the appearance applied while the button is checked.
    /// </summary>
    public XControlAppearance CheckedAppearance
    {
        get => (XControlAppearance)this.GetValue(CheckedAppearanceProperty);
        set => this.SetValue(CheckedAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic size of the button.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon shown while the button is unchecked.
    /// </summary>
    public object? UncheckedIcon
    {
        get => this.GetValue(UncheckedIconProperty);
        set => this.SetValue(UncheckedIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the unchecked icon.
    /// </summary>
    public DataTemplate? UncheckedIconTemplate
    {
        get => (DataTemplate?)this.GetValue(UncheckedIconTemplateProperty);
        set => this.SetValue(UncheckedIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon shown while the button is checked.
    /// </summary>
    public object? CheckedIcon
    {
        get => this.GetValue(CheckedIconProperty);
        set => this.SetValue(CheckedIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the checked icon.
    /// </summary>
    public DataTemplate? CheckedIconTemplate
    {
        get => (DataTemplate?)this.GetValue(CheckedIconTemplateProperty);
        set => this.SetValue(CheckedIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon placement.
    /// </summary>
    public XIconPlacement IconPlacement
    {
        get => (XIconPlacement)this.GetValue(IconPlacementProperty);
        set => this.SetValue(IconPlacementProperty, value);
    }


    /// <summary>
    /// Gets or sets the icon size.
    /// </summary>
    public double IconSize
    {
        get => (double)this.GetValue(IconSizeProperty);
        set => this.SetValue(IconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the inner content stretches horizontally.
    /// </summary>
    public bool StretchContent
    {
        get => (bool)this.GetValue(StretchContentProperty);
        set => this.SetValue(StretchContentProperty, value);
    }
    #endregion


    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        this.ApplyIconSizeToCurrentIcons();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Handles icon changes.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XToggleButton toggleButton)
        {
            toggleButton.ApplyIconSizeToCurrentIcons();
        }
    }

    /// <summary>
    /// Handles icon size changes.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XToggleButton toggleButton)
        {
            toggleButton.ApplyIconSizeToCurrentIcons();
        }
    }

    /// <summary>
    /// Applies the current <see cref="IconSize"/> to the current icon objects, if possible.
    /// </summary>
    private void ApplyIconSizeToCurrentIcons()
    {
        ApplyIconSize(this.UncheckedIcon, this.IconSize);
        ApplyIconSize(this.CheckedIcon, this.IconSize);
    }

    /// <summary>
    /// Applies a numeric icon size to common WPF icon controls.
    /// </summary>
    /// <param name="icon">The icon object.</param>
    /// <param name="iconSize">The icon size.</param>
    private static void ApplyIconSize(object? icon, double iconSize)
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
    /// Sets a dependency property by its static field name if it exists and accepts a <see cref="double"/> value.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="propertyFieldName">The static dependency property field name.</param>
    /// <param name="value">The value to set.</param>
    private static void TrySetDependencyProperty(DependencyObject dependencyObject, string propertyFieldName, double value)
    {
        FieldInfo? fieldInfo = dependencyObject.GetType().GetField(
            propertyFieldName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        if (fieldInfo?.GetValue(null) is not DependencyProperty dependencyProperty
            || dependencyProperty.PropertyType != typeof(double))
        {
            return;
        }

        dependencyObject.SetValue(dependencyProperty, value);
    }
    #endregion
}
#endregion
