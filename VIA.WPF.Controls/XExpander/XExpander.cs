// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XExpander.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XExpander ###
/// <summary>
/// Represents a themed expander control of VIA.WPF.
/// </summary>
[TemplatePart(Name = PartHeaderButton, Type = typeof(Button))]
public class XExpander : HeaderedContentControl
{
    #region ### Constants ###
    /// <summary>
    /// The header button template part name.
    /// </summary>
    private const string PartHeaderButton = "PART_HeaderButton";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="IsExpanded"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
        nameof(IsExpanded),
        typeof(bool),
        typeof(XExpander),
        new FrameworkPropertyMetadata(
            false,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Identifies the <see cref="ExpandDirection"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(
        nameof(ExpandDirection),
        typeof(XExpandDirection),
        typeof(XExpander),
        new FrameworkPropertyMetadata(XExpandDirection.Down));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XExpander),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="HeaderPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
        nameof(HeaderPadding),
        typeof(Thickness),
        typeof(XExpander),
        new FrameworkPropertyMetadata(new Thickness(14d, 10d, 14d, 10d)));

    /// <summary>
    /// Identifies the <see cref="ContentPadding"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
        nameof(ContentPadding),
        typeof(Thickness),
        typeof(XExpander),
        new FrameworkPropertyMetadata(new Thickness(14d, 0d, 14d, 14d)));

    /// <summary>
    /// Identifies the <see cref="HeaderIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderIconProperty = DependencyProperty.Register(
        nameof(HeaderIcon),
        typeof(object),
        typeof(XExpander),
        new FrameworkPropertyMetadata(null, OnHeaderIconChanged));

    /// <summary>
    /// Identifies the <see cref="HeaderIconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderIconSizeProperty = DependencyProperty.Register(
        nameof(HeaderIconSize),
        typeof(double),
        typeof(XExpander),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnHeaderIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="HeaderIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderIconTemplateProperty = DependencyProperty.Register(
        nameof(HeaderIconTemplate),
        typeof(DataTemplate),
        typeof(XExpander),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowIndicator"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowIndicatorProperty = DependencyProperty.Register(
        nameof(ShowIndicator),
        typeof(bool),
        typeof(XExpander),
        new FrameworkPropertyMetadata(true));
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// The current header button.
    /// </summary>
    private Button? headerButton;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XExpander"/> class.
    /// </summary>
    static XExpander()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XExpander),
            new FrameworkPropertyMetadata(typeof(XExpander)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets a value indicating whether the expander content is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => (bool)this.GetValue(IsExpandedProperty);
        set => this.SetValue(IsExpandedProperty, value);
    }

    /// <summary>
    /// Gets or sets the expand direction.
    /// </summary>
    public XExpandDirection ExpandDirection
    {
        get => (XExpandDirection)this.GetValue(ExpandDirectionProperty);
        set => this.SetValue(ExpandDirectionProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the expander.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding applied to the header area.
    /// </summary>
    public Thickness HeaderPadding
    {
        get => (Thickness)this.GetValue(HeaderPaddingProperty);
        set => this.SetValue(HeaderPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding applied to the content area.
    /// </summary>
    public Thickness ContentPadding
    {
        get => (Thickness)this.GetValue(ContentPaddingProperty);
        set => this.SetValue(ContentPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional icon shown in the header.
    /// </summary>
    public object? HeaderIcon
    {
        get => this.GetValue(HeaderIconProperty);
        set => this.SetValue(HeaderIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the optional header icon.
    /// </summary>
    public double HeaderIconSize
    {
        get => (double)this.GetValue(HeaderIconSizeProperty);
        set => this.SetValue(HeaderIconSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the template used for the header icon.
    /// </summary>
    public DataTemplate? HeaderIconTemplate
    {
        get => (DataTemplate?)this.GetValue(HeaderIconTemplateProperty);
        set => this.SetValue(HeaderIconTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the indicator glyph is shown.
    /// </summary>
    public bool ShowIndicator
    {
        get => (bool)this.GetValue(ShowIndicatorProperty);
        set => this.SetValue(ShowIndicatorProperty, value);
    }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (this.headerButton is not null)
        {
            this.headerButton.Click -= this.OnHeaderButtonClick;
        }

        this.headerButton = this.GetTemplateChild(PartHeaderButton) as Button;

        if (this.headerButton is not null)
        {
            this.headerButton.Click += this.OnHeaderButtonClick;
        }

        this.ApplyCurrentHeaderIconSize();
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Handled)
        {
            return;
        }

        if (e.Key is Key.Space or Key.Enter)
        {
            this.IsExpanded = !this.IsExpanded;
            e.Handled = true;
        }
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Applies the configured header icon size to the current header icon object.
    /// </summary>
    private void ApplyCurrentHeaderIconSize()
    {
        ApplyIconSize(this.HeaderIcon, this.HeaderIconSize);
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
    /// Handles header button clicks.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnHeaderButtonClick(object sender, RoutedEventArgs e)
    {
        this.IsExpanded = !this.IsExpanded;
    }

    /// <summary>
    /// Handles changes to <see cref="HeaderIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnHeaderIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XExpander expander)
        {
            ApplyIconSize(eventArgs.NewValue, expander.HeaderIconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="HeaderIconSize"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnHeaderIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XExpander expander)
        {
            expander.ApplyCurrentHeaderIconSize();
        }
    }
    #endregion
}
#endregion
