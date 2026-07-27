// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XListBoxItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VIA.WPF.Controls;

#region ### Class XListBoxItem ###
/// <summary>
/// Represents a themed list box item container of VIA.WPF.
/// </summary>
public class XListBoxItem : ListBoxItem
{
    #region ### Dependency Properties ###
    private static readonly DependencyPropertyKey HasSubTitlePropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(HasSubTitle),
            typeof(bool),
            typeof(XListBoxItem),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    private static readonly DependencyPropertyKey HasBadgeContentPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(HasBadgeContent),
            typeof(bool),
            typeof(XListBoxItem),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="Title"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(XListBoxItem),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="SubTitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register(
        nameof(SubTitle),
        typeof(string),
        typeof(XListBoxItem),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnSubTitleChanged));

    /// <summary>
    /// Identifies the <see cref="HasSubTitle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasSubTitleProperty = HasSubTitlePropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="ShowBadge"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowBadgeProperty = DependencyProperty.Register(
        nameof(ShowBadge),
        typeof(bool),
        typeof(XListBoxItem),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="BadgeContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BadgeContentProperty = DependencyProperty.Register(
        nameof(BadgeContent),
        typeof(object),
        typeof(XListBoxItem),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnBadgeContentChanged));

    /// <summary>
    /// Identifies the <see cref="HasBadgeContent"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasBadgeContentProperty = HasBadgeContentPropertyKey.DependencyProperty;

    /// <summary>
    /// Identifies the <see cref="BadgeVariant"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty BadgeVariantProperty = DependencyProperty.Register(
        nameof(BadgeVariant),
        typeof(XControlVariant),
        typeof(XListBoxItem),
        new FrameworkPropertyMetadata(XControlVariant.Accent, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="ShowEdit"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowEditProperty = DependencyProperty.Register(
        nameof(ShowEdit),
        typeof(bool),
        typeof(XListBoxItem),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="EditToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditToolTipProperty = DependencyProperty.Register(
        nameof(EditToolTip),
        typeof(object),
        typeof(XListBoxItem),
        new PropertyMetadata("Edit"));

    /// <summary>
    /// Identifies the <see cref="EditCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(
        nameof(EditCommand),
        typeof(ICommand),
        typeof(XListBoxItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="EditCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty EditCommandParameterProperty = DependencyProperty.Register(
        nameof(EditCommandParameter),
        typeof(object),
        typeof(XListBoxItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="ShowDelete"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowDeleteProperty = DependencyProperty.Register(
        nameof(ShowDelete),
        typeof(bool),
        typeof(XListBoxItem),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Identifies the <see cref="DeleteToolTip"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteToolTipProperty = DependencyProperty.Register(
        nameof(DeleteToolTip),
        typeof(object),
        typeof(XListBoxItem),
        new PropertyMetadata("Delete"));

    /// <summary>
    /// Identifies the <see cref="DeleteCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
        nameof(DeleteCommand),
        typeof(ICommand),
        typeof(XListBoxItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="DeleteCommandParameter"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty DeleteCommandParameterProperty = DependencyProperty.Register(
        nameof(DeleteCommandParameter),
        typeof(object),
        typeof(XListBoxItem),
        new PropertyMetadata(null));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XListBoxItem"/> class.
    /// </summary>
    static XListBoxItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XListBoxItem),
            new FrameworkPropertyMetadata(typeof(XListBoxItem)));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the primary item text.
    /// </summary>
    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional secondary item text.
    /// </summary>
    public string? SubTitle
    {
        get => (string?)this.GetValue(SubTitleProperty);
        set => this.SetValue(SubTitleProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether a visible subtitle is available.
    /// </summary>
    public bool HasSubTitle => (bool)this.GetValue(HasSubTitleProperty);

    /// <summary>
    /// Gets or sets a value indicating whether the badge is enabled.
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
    /// Gets a value indicating whether visible badge content is available.
    /// </summary>
    public bool HasBadgeContent => (bool)this.GetValue(HasBadgeContentProperty);

    /// <summary>
    /// Gets or sets the badge color variant.
    /// </summary>
    public XControlVariant BadgeVariant
    {
        get => (XControlVariant)this.GetValue(BadgeVariantProperty);
        set => this.SetValue(BadgeVariantProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the edit action is enabled.
    /// </summary>
    public bool ShowEdit
    {
        get => (bool)this.GetValue(ShowEditProperty);
        set => this.SetValue(ShowEditProperty, value);
    }

    /// <summary>
    /// Gets or sets the edit action tooltip.
    /// </summary>
    public object? EditToolTip
    {
        get => this.GetValue(EditToolTipProperty);
        set => this.SetValue(EditToolTipProperty, value);
    }

    /// <summary>
    /// Gets or sets the edit command.
    /// </summary>
    public ICommand? EditCommand
    {
        get => (ICommand?)this.GetValue(EditCommandProperty);
        set => this.SetValue(EditCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the edit command parameter.
    /// </summary>
    public object? EditCommandParameter
    {
        get => this.GetValue(EditCommandParameterProperty);
        set => this.SetValue(EditCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the delete action is enabled.
    /// </summary>
    public bool ShowDelete
    {
        get => (bool)this.GetValue(ShowDeleteProperty);
        set => this.SetValue(ShowDeleteProperty, value);
    }

    /// <summary>
    /// Gets or sets the delete action tooltip.
    /// </summary>
    public object? DeleteToolTip
    {
        get => this.GetValue(DeleteToolTipProperty);
        set => this.SetValue(DeleteToolTipProperty, value);
    }

    /// <summary>
    /// Gets or sets the delete command.
    /// </summary>
    public ICommand? DeleteCommand
    {
        get => (ICommand?)this.GetValue(DeleteCommandProperty);
        set => this.SetValue(DeleteCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the delete command parameter.
    /// </summary>
    public object? DeleteCommandParameter
    {
        get => this.GetValue(DeleteCommandParameterProperty);
        set => this.SetValue(DeleteCommandParameterProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static void OnSubTitleChanged(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XListBoxItem listBoxItem)
        {
            listBoxItem.SetValue(
                HasSubTitlePropertyKey,
                eventArgs.NewValue is string value && !string.IsNullOrWhiteSpace(value));
        }
    }

    private static void OnBadgeContentChanged(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XListBoxItem listBoxItem)
        {
            bool hasContent = eventArgs.NewValue is not null
                && (eventArgs.NewValue is not string text || !string.IsNullOrWhiteSpace(text));

            listBoxItem.SetValue(HasBadgeContentPropertyKey, hasContent);
        }
    }
    #endregion
}
#endregion
