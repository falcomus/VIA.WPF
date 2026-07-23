// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XButton.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Class XButton ###
/// <summary>
/// Represents the standard button control of VIA.WPF.
/// </summary>
public class XButton : Button
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XButton),
        new FrameworkPropertyMetadata(new CornerRadius(6d)));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XButton),
        new FrameworkPropertyMetadata(XControlVariant.Default));

    /// <summary>
    /// Identifies the <see cref="Appearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(XControlAppearance),
        typeof(XButton),
        new FrameworkPropertyMetadata(XControlAppearance.Solid));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XButton),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="Elevation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ElevationProperty = DependencyProperty.Register(
        nameof(Elevation),
        typeof(XElevation),
        typeof(XButton),
        new FrameworkPropertyMetadata(XElevation.None));

    /// <summary>
    /// Identifies the <see cref="Icon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(object),
        typeof(XButton),
        new FrameworkPropertyMetadata(null, OnIconChanged));

    /// <summary>
    /// Identifies the <see cref="IconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
        nameof(IconTemplate),
        typeof(DataTemplate),
        typeof(XButton),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IconPlacement"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
        nameof(IconPlacement),
        typeof(XIconPlacement),
        typeof(XButton),
        new FrameworkPropertyMetadata(XIconPlacement.Left));

    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize),
        typeof(double),
        typeof(XButton),
        new FrameworkPropertyMetadata(
            XControlSizeMetrics.MediumIconSize,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
            OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="IsLoading"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
        nameof(IsLoading),
        typeof(bool),
        typeof(XButton),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="LoadingContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.Register(
        nameof(LoadingContent),
        typeof(object),
        typeof(XButton),
        new FrameworkPropertyMetadata("Loading..."));

    /// <summary>
    /// Identifies the <see cref="LoadingContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LoadingContentTemplateProperty = DependencyProperty.Register(
        nameof(LoadingContentTemplate),
        typeof(DataTemplate),
        typeof(XButton),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="StretchContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StretchContentProperty = DependencyProperty.Register(
        nameof(StretchContent),
        typeof(bool),
        typeof(XButton),
        new FrameworkPropertyMetadata(false));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XButton"/> class.
    /// </summary>
    static XButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XButton),
            new FrameworkPropertyMetadata(typeof(XButton)));
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
    /// Gets or sets the size of the button.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the elevation of the button.
    /// </summary>
    public XElevation Elevation
    {
        get => (XElevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon content.
    /// </summary>
    public object? Icon
    {
        get => this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon template.
    /// </summary>
    public DataTemplate? IconTemplate
    {
        get => (DataTemplate?)this.GetValue(IconTemplateProperty);
        set => this.SetValue(IconTemplateProperty, value);
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
    /// Gets or sets a value indicating whether the loading state is active.
    /// </summary>
    public bool IsLoading
    {
        get => (bool)this.GetValue(IsLoadingProperty);
        set => this.SetValue(IsLoadingProperty, value);
    }

    /// <summary>
    /// Gets or sets the content shown while loading.
    /// </summary>
    public object? LoadingContent
    {
        get => this.GetValue(LoadingContentProperty);
        set => this.SetValue(LoadingContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the loading content.
    /// </summary>
    public DataTemplate? LoadingContentTemplate
    {
        get => (DataTemplate?)this.GetValue(LoadingContentTemplateProperty);
        set => this.SetValue(LoadingContentTemplateProperty, value);
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
    /// <inheritdoc/>
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        this.ApplyIconSizeToCurrentIcon();
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
        if (dependencyObject is XButton button)
        {
            button.ApplyIconSizeToCurrentIcon();
        }
    }

    /// <summary>
    /// Handles icon size changes.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XButton button)
        {
            button.ApplyIconSizeToCurrentIcon();
        }
    }

    /// <summary>
    /// Applies the current <see cref="IconSize"/> to the current icon object, if possible.
    /// </summary>
    private void ApplyIconSizeToCurrentIcon()
    {
        XIconAssist.ApplySize(this.Icon, this.IconSize);
    }
    #endregion
}
#endregion

