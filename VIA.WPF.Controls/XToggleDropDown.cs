// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XToggleDropDown.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace VIA.WPF.Controls;

#region ### Class XToggleDropDown ###
/// <summary>
/// Represents a toggle button with an integrated drop-down popup.
/// </summary>
[TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
public class XToggleDropDown : ToggleButton
{
    #region ### Constants ###
    /// <summary>
    /// The popup template part name.
    /// </summary>
    private const string PopupPartName = "PART_Popup";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumCornerRadius));

    /// <summary>
    /// Identifies the <see cref="Variant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(XControlVariant.Default));

    /// <summary>
    /// Identifies the <see cref="Appearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(XControlAppearance),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(XControlAppearance.Solid));

    /// <summary>
    /// Identifies the <see cref="CheckedVariant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckedVariantProperty = DependencyProperty.Register(
        nameof(CheckedVariant),
        typeof(XControlVariant),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(XControlVariant.Primary));

    /// <summary>
    /// Identifies the <see cref="CheckedAppearance"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckedAppearanceProperty = DependencyProperty.Register(
        nameof(CheckedAppearance),
        typeof(XControlAppearance),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(XControlAppearance.Solid));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(XControlSize.Medium));

    /// <summary>
    /// Identifies the <see cref="UncheckedIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UncheckedIconProperty = DependencyProperty.Register(
        nameof(UncheckedIcon),
        typeof(object),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(null, OnUncheckedIconChanged));

    /// <summary>
    /// Identifies the <see cref="UncheckedIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty UncheckedIconTemplateProperty = DependencyProperty.Register(
        nameof(UncheckedIconTemplate),
        typeof(DataTemplate),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="CheckedIcon"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckedIconProperty = DependencyProperty.Register(
        nameof(CheckedIcon),
        typeof(object),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(null, OnCheckedIconChanged));

    /// <summary>
    /// Identifies the <see cref="CheckedIconTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty CheckedIconTemplateProperty = DependencyProperty.Register(
        nameof(CheckedIconTemplate),
        typeof(DataTemplate),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IconPlacement"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
        nameof(IconPlacement),
        typeof(XIconPlacement),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(XIconPlacement.Left));

    /// <summary>
    /// Identifies the <see cref="IconSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize),
        typeof(double),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(XControlSizeMetrics.MediumIconSize, FrameworkPropertyMetadataOptions.AffectsMeasure, OnIconSizeChanged));

    /// <summary>
    /// Identifies the <see cref="StretchContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StretchContentProperty = DependencyProperty.Register(
        nameof(StretchContent),
        typeof(bool),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="DropDownContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register(
        nameof(DropDownContent),
        typeof(object),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DropDownContentTemplate"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropDownContentTemplateProperty = DependencyProperty.Register(
        nameof(DropDownContentTemplate),
        typeof(DataTemplate),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));

    /// <summary>
    /// Identifies the <see cref="DropDownPlacement"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DropDownPlacementProperty = DependencyProperty.Register(
        nameof(DropDownPlacement),
        typeof(PlacementMode),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(PlacementMode.Bottom));

    /// <summary>
    /// Identifies the <see cref="MaxDropDownHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(
        nameof(MaxDropDownHeight),
        typeof(double),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(320d));

    /// <summary>
    /// Identifies the <see cref="StaysOpen"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register(
        nameof(StaysOpen),
        typeof(bool),
        typeof(XToggleDropDown),
        new FrameworkPropertyMetadata(false));
    #endregion

    #region ### Private Fields ###
    /// <summary>
    /// Prevents recursive synchronization between checked and popup state.
    /// </summary>
    private bool isSynchronizingState;

    /// <summary>
    /// The current popup template part.
    /// </summary>
    private Popup? popup;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XToggleDropDown"/> class.
    /// </summary>
    static XToggleDropDown()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XToggleDropDown),
            new FrameworkPropertyMetadata(typeof(XToggleDropDown)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the corner radius of the control.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic variant of the control.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the appearance of the control.
    /// </summary>
    public XControlAppearance Appearance
    {
        get => (XControlAppearance)this.GetValue(AppearanceProperty);
        set => this.SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic variant applied while checked.
    /// </summary>
    public XControlVariant CheckedVariant
    {
        get => (XControlVariant)this.GetValue(CheckedVariantProperty);
        set => this.SetValue(CheckedVariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the appearance applied while checked.
    /// </summary>
    public XControlAppearance CheckedAppearance
    {
        get => (XControlAppearance)this.GetValue(CheckedAppearanceProperty);
        set => this.SetValue(CheckedAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic size of the control.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon shown while unchecked.
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
    /// Gets or sets the icon shown while checked.
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

    /// <summary>
    /// Gets or sets the drop-down content.
    /// </summary>
    public object? DropDownContent
    {
        get => this.GetValue(DropDownContentProperty);
        set => this.SetValue(DropDownContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the template for the drop-down content.
    /// </summary>
    public DataTemplate? DropDownContentTemplate
    {
        get => (DataTemplate?)this.GetValue(DropDownContentTemplateProperty);
        set => this.SetValue(DropDownContentTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the popup is open.
    /// </summary>
    public bool IsDropDownOpen
    {
        get => (bool)this.GetValue(IsDropDownOpenProperty);
        set => this.SetValue(IsDropDownOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the popup placement.
    /// </summary>
    public PlacementMode DropDownPlacement
    {
        get => (PlacementMode)this.GetValue(DropDownPlacementProperty);
        set => this.SetValue(DropDownPlacementProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum popup height.
    /// </summary>
    public double MaxDropDownHeight
    {
        get => (double)this.GetValue(MaxDropDownHeightProperty);
        set => this.SetValue(MaxDropDownHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the popup remains open until explicitly closed.
    /// </summary>
    public bool StaysOpen
    {
        get => (bool)this.GetValue(StaysOpenProperty);
        set => this.SetValue(StaysOpenProperty, value);
    }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this.popup = this.GetTemplateChild(PopupPartName) as Popup;
        this.ApplyCurrentIconSize();
    }
    #endregion

    #region ### Protected Methods ###
    /// <inheritdoc />
    protected override void OnClick()
    {
        if (this.IsDropDownOpen)
        {
            this.CloseDropDown();
            return;
        }

        base.OnClick();
    }

    /// <inheritdoc />
    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (this.IsDropDownOpen)
        {
            DependencyObject? originalSource = e.OriginalSource as DependencyObject;

            if (this.IsElementWithinPopup(originalSource))
            {
                base.OnPreviewMouseLeftButtonDown(e);
                return;
            }

            this.CloseDropDown();
            e.Handled = true;
            return;
        }

        base.OnPreviewMouseLeftButtonDown(e);
    }

    /// <inheritdoc />
    protected override void OnChecked(RoutedEventArgs e)
    {
        base.OnChecked(e);
        this.SynchronizeDropDownStateFromCheckedState(true);
    }

    /// <inheritdoc />
    protected override void OnUnchecked(RoutedEventArgs e)
    {
        base.OnUnchecked(e);
        this.SynchronizeDropDownStateFromCheckedState(false);
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Closes the drop-down and synchronizes the checked state.
    /// </summary>
    private void CloseDropDown()
    {
        try
        {
            this.isSynchronizingState = true;
            this.SetCurrentValue(IsDropDownOpenProperty, false);
            this.SetCurrentValue(IsCheckedProperty, false);
        }
        finally
        {
            this.isSynchronizingState = false;
        }
    }

    /// <summary>
    /// Applies the configured icon size to the current icon objects.
    /// </summary>
    private void ApplyCurrentIconSize()
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
    /// Synchronizes the popup state from the checked state.
    /// </summary>
    /// <param name="isChecked">The target checked state.</param>
    private void SynchronizeDropDownStateFromCheckedState(bool isChecked)
    {
        if (this.isSynchronizingState)
        {
            return;
        }

        try
        {
            this.isSynchronizingState = true;
            this.SetCurrentValue(IsDropDownOpenProperty, isChecked);
        }
        finally
        {
            this.isSynchronizingState = false;
        }
    }

    /// <summary>
    /// Synchronizes the checked state from the popup state.
    /// </summary>
    /// <param name="isOpen">The target popup state.</param>
    private void SynchronizeCheckedStateFromDropDownState(bool isOpen)
    {
        if (this.isSynchronizingState)
        {
            return;
        }

        try
        {
            this.isSynchronizingState = true;
            this.SetCurrentValue(IsCheckedProperty, isOpen);
        }
        finally
        {
            this.isSynchronizingState = false;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the specified element belongs to the current popup content.
    /// </summary>
    /// <param name="element">The element to inspect.</param>
    /// <returns><c>true</c> if the element belongs to the popup; otherwise, <c>false</c>.</returns>
    private bool IsElementWithinPopup(DependencyObject? element)
    {
        if (this.popup?.Child is not DependencyObject popupChild)
        {
            return false;
        }

        while (element is not null)
        {
            if (ReferenceEquals(element, popupChild) || ReferenceEquals(element, this.popup))
            {
                return true;
            }

            element = GetParent(element);
        }

        return false;
    }

    /// <summary>
    /// Gets the most likely parent of the specified dependency object.
    /// </summary>
    /// <param name="element">The element whose parent is requested.</param>
    /// <returns>The parent element, if available; otherwise, <c>null</c>.</returns>
    private static DependencyObject? GetParent(DependencyObject element)
    {
        if (element is Visual)
        {
            DependencyObject? visualParent = VisualTreeHelper.GetParent(element);
            if (visualParent is not null)
            {
                return visualParent;
            }
        }

        DependencyObject? logicalParent = LogicalTreeHelper.GetParent(element);
        if (logicalParent is not null)
        {
            return logicalParent;
        }

        if (element is FrameworkElement frameworkElement)
        {
            if (frameworkElement.Parent is not null)
            {
                return frameworkElement.Parent;
            }

            if (frameworkElement.TemplatedParent is DependencyObject templatedParent)
            {
                return templatedParent;
            }
        }

        return element is FrameworkContentElement frameworkContentElement
            ? frameworkContentElement.Parent
            : null;
    }

    /// <summary>
    /// Handles changes to <see cref="UncheckedIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnUncheckedIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XToggleDropDown toggleDropDown)
        {
            ApplyIconSize(eventArgs.NewValue, toggleDropDown.IconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="CheckedIcon"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnCheckedIconChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XToggleDropDown toggleDropDown)
        {
            ApplyIconSize(eventArgs.NewValue, toggleDropDown.IconSize);
        }
    }

    /// <summary>
    /// Handles changes to <see cref="IconSize"/>.
    /// </summary>
    /// <param name="dependencyObject">The changed dependency object.</param>
    /// <param name="eventArgs">The change data.</param>
    private static void OnIconSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XToggleDropDown toggleDropDown)
        {
            toggleDropDown.ApplyCurrentIconSize();
        }
    }

    /// <summary>
    /// Handles changes to <see cref="IsDropDownOpen"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    /// <summary>
    /// Handles changes to <see cref="IsDropDownOpen"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    /// <summary>
    /// Handles changes to <see cref="IsDropDownOpen"/>.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The property changed event data.</param>
    private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not XToggleDropDown toggleDropDown || e.NewValue is not bool isOpen)
        {
            return;
        }

        toggleDropDown.SynchronizeCheckedStateFromDropDownState(isOpen);
    }
    #endregion
}
#endregion

