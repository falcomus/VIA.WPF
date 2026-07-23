// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationListItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XNavigationListItem ###
/// <summary>
/// Represents a navigation item inside an <see cref="XNavigationList"/>.
/// </summary>
public class XNavigationListItem : ListBoxItem
{
    #region ### Dependency Properties ###
    private static readonly DependencyPropertyKey HasSubTitlePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasSubTitle),
        typeof(bool),
        typeof(XNavigationListItem),
        new FrameworkPropertyMetadata(false));

    private static readonly DependencyPropertyKey HasBadgeContentPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasBadgeContent),
        typeof(bool),
        typeof(XNavigationListItem),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="Title"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(XNavigationListItem),
            new PropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="SubTitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SubTitleProperty =
        DependencyProperty.Register(
            nameof(SubTitle),
            typeof(string),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnSubTitleChanged));

    /// <summary>
    /// Identifies the read-only <see cref="HasSubTitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasSubTitleProperty = HasSubTitlePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="Icon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(
            nameof(Icon),
            typeof(object),
            typeof(XNavigationListItem),
            new PropertyMetadata(null, OnIconChanged));

    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(18d, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="IconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconTemplateProperty =
        DependencyProperty.Register(
            nameof(IconTemplate),
            typeof(DataTemplate),
            typeof(XNavigationListItem),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowBadge"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowBadgeProperty =
        DependencyProperty.Register(
            nameof(ShowBadge),
            typeof(bool),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="BadgeContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BadgeContentProperty =
        DependencyProperty.Register(
            nameof(BadgeContent),
            typeof(object),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnBadgeContentChanged));

    /// <summary>
    /// Identifies the read-only <see cref="HasBadgeContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasBadgeContentProperty = HasBadgeContentPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="BadgeVariant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BadgeVariantProperty =
        DependencyProperty.Register(
            nameof(BadgeVariant),
            typeof(XControlVariant),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(XControlVariant.Accent));

    /// <summary>
    /// Identifies the <see cref="ShowEdit"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditProperty =
        DependencyProperty.Register(
            nameof(ShowEdit),
            typeof(bool),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="EditCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandProperty =
        DependencyProperty.Register(
            nameof(EditCommand),
            typeof(ICommand),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EditCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandParameterProperty =
        DependencyProperty.Register(
            nameof(EditCommandParameter),
            typeof(object),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowDelete"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDeleteProperty =
        DependencyProperty.Register(
            nameof(ShowDelete),
            typeof(bool),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="DeleteCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteCommandProperty =
        DependencyProperty.Register(
            nameof(DeleteCommand),
            typeof(ICommand),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DeleteCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteCommandParameterProperty =
        DependencyProperty.Register(
            nameof(DeleteCommandParameter),
            typeof(object),
            typeof(XNavigationListItem),
            new FrameworkPropertyMetadata(null));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes the <see cref="XNavigationListItem"/> class.
    /// </summary>
    static XNavigationListItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XNavigationListItem), new FrameworkPropertyMetadata(typeof(XNavigationListItem)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the title of the navigation item.
    /// </summary>
    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional second text line of the navigation item.
    /// </summary>
    public string? SubTitle
    {
        get => (string?)this.GetValue(SubTitleProperty);
        set => this.SetValue(SubTitleProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the navigation item has a non-empty subtitle.
    /// </summary>
    public bool HasSubTitle => (bool)this.GetValue(HasSubTitleProperty);

    /// <summary>
    /// Gets or sets the icon content of the navigation item.
    /// </summary>
    public object? Icon
    {
        get => this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
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
    /// Gets or sets the template used to render the icon.
    /// </summary>
    public DataTemplate? IconTemplate
    {
        get => (DataTemplate?)this.GetValue(IconTemplateProperty);
        set => this.SetValue(IconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the badge is shown.
    /// </summary>
    public bool ShowBadge
    {
        get => (bool)this.GetValue(ShowBadgeProperty);
        set => this.SetValue(ShowBadgeProperty, value);
    }

    /// <summary>
    /// Gets or sets the badge content.
    /// </summary>
    public object? BadgeContent
    {
        get => this.GetValue(BadgeContentProperty);
        set => this.SetValue(BadgeContentProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the badge contains a meaningful value.
    /// </summary>
    public bool HasBadgeContent => (bool)this.GetValue(HasBadgeContentProperty);

    /// <summary>
    /// Gets or sets the semantic color variant of the badge.
    /// </summary>
    public XControlVariant BadgeVariant
    {
        get => (XControlVariant)this.GetValue(BadgeVariantProperty);
        set => this.SetValue(BadgeVariantProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the edit action is available.
    /// </summary>
    public bool ShowEdit
    {
        get => (bool)this.GetValue(ShowEditProperty);
        set => this.SetValue(ShowEditProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the edit action.
    /// </summary>
    public ICommand? EditCommand
    {
        get => (ICommand?)this.GetValue(EditCommandProperty);
        set => this.SetValue(EditCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used by the edit action.
    /// </summary>
    public object? EditCommandParameter
    {
        get => this.GetValue(EditCommandParameterProperty);
        set => this.SetValue(EditCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the delete action is available.
    /// </summary>
    public bool ShowDelete
    {
        get => (bool)this.GetValue(ShowDeleteProperty);
        set => this.SetValue(ShowDeleteProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed by the delete action.
    /// </summary>
    public ICommand? DeleteCommand
    {
        get => (ICommand?)this.GetValue(DeleteCommandProperty);
        set => this.SetValue(DeleteCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter used by the delete action.
    /// </summary>
    public object? DeleteCommandParameter
    {
        get => this.GetValue(DeleteCommandParameterProperty);
        set => this.SetValue(DeleteCommandParameterProperty, value);
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

    private static bool HasMeaningfulContent(object? value)
    {
        return value switch
        {
            null => false,
            string text => !string.IsNullOrWhiteSpace(text),
            _ => true,
        };
    }

    /// <summary>
    /// Handles changes to <see cref="SubTitle"/>.
    /// </summary>
    private static void OnSubTitleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XNavigationListItem navigationListItem)
        {
            navigationListItem.SetValue(HasSubTitlePropertyKey, HasMeaningfulContent(eventArgs.NewValue));
        }
    }

    /// <summary>
    /// Handles changes to <see cref="BadgeContent"/>.
    /// </summary>
    private static void OnBadgeContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XNavigationListItem navigationListItem)
        {
            navigationListItem.SetValue(HasBadgeContentPropertyKey, HasMeaningfulContent(eventArgs.NewValue));
        }
    }

    /// <summary>
    /// Handles changes to <see cref="Icon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XNavigationListItem navigationListItem)
        {
            ApplyIconSize(eventArgs.NewValue, navigationListItem.IconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="IconSize"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XNavigationListItem navigationListItem)
        {
            navigationListItem.ApplyCurrentIconSize();
        }
    }
    #endregion
}
#endregion
