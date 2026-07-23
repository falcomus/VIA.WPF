// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XTextBlock.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Controls;

#region ### Enum XTextBlockRole ###
/// <summary>
/// Defines semantic text roles for <see cref="XTextBlock" />.
/// </summary>
public enum XTextBlockRole
{
    /// <summary>
    /// Standard paragraph text.
    /// </summary>
    Body,

    /// <summary>
    /// Compact label text.
    /// </summary>
    Label,

    /// <summary>
    /// Large display text for prominent branding or hero titles.
    /// </summary>
    Display,

    /// <summary>
    /// Uppercase or letter-spaced auxiliary text.
    /// </summary>
    Overline,

    /// <summary>
    /// Large page or dialog title text.
    /// </summary>
    Title,

    /// <summary>
    /// Major page section or dialog heading text.
    /// </summary>
    LargeHeading,

    /// <summary>
    /// Medium page section or dialog heading text.
    /// </summary>
    Heading,

    /// <summary>
    /// Secondary title text below a title.
    /// </summary>
    Subtitle,

    /// <summary>
    /// Section headline text.
    /// </summary>
    SectionTitle,

    /// <summary>
    /// Secondary explanatory text.
    /// </summary>
    Description,

    /// <summary>
    /// Small secondary text.
    /// </summary>
    Caption,

    /// <summary>
    /// Error or validation text.
    /// </summary>
    Error,

    /// <summary>
    /// Success text.
    /// </summary>
    Success,

    /// <summary>
    /// Warning text.
    /// </summary>
    Warning,

    /// <summary>
    /// Informational text.
    /// </summary>
    Info,

    /// <summary>
    /// Monospace text for code-like content.
    /// </summary>
    Code
}
#endregion

#region ### Class XTextBlock ###
/// <summary>
/// Represents a themed text block with semantic roles, shared sizing and multiline convenience.
/// </summary>
public class XTextBlock : TextBlock
{
    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="TextRole" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty TextRoleProperty = DependencyProperty.Register(
        nameof(TextRole),
        typeof(XTextBlockRole),
        typeof(XTextBlock),
        new FrameworkPropertyMetadata(
            XTextBlockRole.Body,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="Size" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(XControlSize),
        typeof(XTextBlock),
        new FrameworkPropertyMetadata(
            XControlSize.Medium,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="Variant" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty VariantProperty = DependencyProperty.Register(
        nameof(Variant),
        typeof(XControlVariant),
        typeof(XTextBlock),
        new FrameworkPropertyMetadata(
            XControlVariant.Default,
            FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="IsMultiline" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsMultilineProperty = DependencyProperty.Register(
        nameof(IsMultiline),
        typeof(bool),
        typeof(XTextBlock),
        new FrameworkPropertyMetadata(
            false,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XTextBlock" /> class.
    /// </summary>
    static XTextBlock()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XTextBlock),
            new FrameworkPropertyMetadata(typeof(XTextBlock)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XTextBlock"/> class.
    /// </summary>
    public XTextBlock()
    {
        XValidationAdornerHelper.SuppressDefaultErrorTemplate(this);
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the semantic role of the text.
    /// </summary>
    public XTextBlockRole TextRole
    {
        get => (XTextBlockRole)this.GetValue(TextRoleProperty);
        set => this.SetValue(TextRoleProperty, value);
    }

    /// <summary>
    /// Gets or sets the shared text size.
    /// </summary>
    public XControlSize Size
    {
        get => (XControlSize)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the semantic text color variant.
    /// </summary>
    public XControlVariant Variant
    {
        get => (XControlVariant)this.GetValue(VariantProperty);
        set => this.SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether text should wrap by default.
    /// </summary>
    public bool IsMultiline
    {
        get => (bool)this.GetValue(IsMultilineProperty);
        set => this.SetValue(IsMultilineProperty, value);
    }
    #endregion
}
#endregion
