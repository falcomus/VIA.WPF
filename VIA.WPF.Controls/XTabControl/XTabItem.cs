// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTabItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XTabItem ###
/// <summary>
/// Represents a themed tab item of VIA.WPF.
/// </summary>
public class XTabItem : TabItem
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Icon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(object),
        typeof(XTabItem),
        new FrameworkPropertyMetadata(null, OnIconChanged));

    /// <summary>
    /// Identifies the <see cref="IconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
        nameof(IconTemplate),
        typeof(DataTemplate),
        typeof(XTabItem),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize),
        typeof(double),
        typeof(XTabItem),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="CanClose"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register(
        nameof(CanClose),
        typeof(bool),
        typeof(XTabItem),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="CloseCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(
        nameof(CloseCommand),
        typeof(ICommand),
        typeof(XTabItem),
        new FrameworkPropertyMetadata(null));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XTabItem"/> class.
    /// </summary>
    static XTabItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XTabItem),
            new FrameworkPropertyMetadata(typeof(XTabItem)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the optional icon content of the tab header.
    /// </summary>
    public object? Icon
    {
        get => this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional icon template of the tab header.
    /// </summary>
    public DataTemplate? IconTemplate
    {
        get => (DataTemplate?)this.GetValue(IconTemplateProperty);
        set => this.SetValue(IconTemplateProperty, value);
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
    /// Gets or sets a value indicating whether the tab can be closed.
    /// </summary>
    public bool CanClose
    {
        get => (bool)this.GetValue(CanCloseProperty);
        set => this.SetValue(CanCloseProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the close button is clicked.
    /// </summary>
    public ICommand? CloseCommand
    {
        get => (ICommand?)this.GetValue(CloseCommandProperty);
        set => this.SetValue(CloseCommandProperty, value);
    }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this.ApplyCurrentIconSize();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Applies the configured icon size to the current icon object.
    /// </summary>
    private void ApplyCurrentIconSize()
    {
        ApplyIconSize(this.Icon, this.IconSize);
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

        if (icon is FrameworkElement frameworkElement && ShouldApplyFrameworkElementSize(frameworkElement))
        {
            frameworkElement.Width = iconSize;
            frameworkElement.Height = iconSize;
        }
    }

    /// <summary>
    /// Gets a value indicating whether width and height should be applied directly to the specified element.
    /// </summary>
    /// <param name="frameworkElement">The framework element to inspect.</param>
    /// <returns><c>true</c> if direct width and height assignment is appropriate; otherwise, <c>false</c>.</returns>
    private static bool ShouldApplyFrameworkElementSize(FrameworkElement frameworkElement)
    {
        if (frameworkElement is ButtonBase)
        {
            return false;
        }

        string typeName = frameworkElement.GetType().Name;
        string? namespaceName = frameworkElement.GetType().Namespace;

        return typeName.Contains("Icon", StringComparison.OrdinalIgnoreCase)
            || namespaceName?.Contains("IconPacks", StringComparison.OrdinalIgnoreCase) == true;
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

    /// <summary>
    /// Handles changes to <see cref="Icon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XTabItem tabItem)
        {
            ApplyIconSize(eventArgs.NewValue, tabItem.IconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="IconSize"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XTabItem tabItem)
        {
            tabItem.ApplyCurrentIconSize();
        }
    }
    #endregion
}
#endregion
