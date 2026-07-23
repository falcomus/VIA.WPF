// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XHighlightTextBlock.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using VIA.WPF.Themes;

namespace VIA.WPF.Controls;

#region ### Enum XHighlightRenderMode ###
/// <summary>
/// Defines how matching text fragments are highlighted.
/// </summary>
public enum XHighlightRenderMode
{
    /// <summary>
    /// Does not highlight matching text fragments.
    /// </summary>
    None,

    /// <summary>
    /// Highlights matching text fragments by using lightweight <see cref="Run" /> formatting.
    /// </summary>
    Simple,

    /// <summary>
    /// Highlights matching text fragments by using a rounded badge element.
    /// </summary>
    Badge
}
#endregion

#region ### Class XHighlightTextBlock ###
/// <summary>
/// Displays text and highlights all occurrences of a configured search text.
/// </summary>
public class XHighlightTextBlock : TextBlock
{
    #region ### Fields ###
    private static readonly SolidColorBrush DefaultHighlightBrush = CreateDefaultHighlightBrush();

    private string currentText = string.Empty;
    private bool hasCurrentText;
    private bool isUpdatingInlines;
    private bool usesDisplayTextSource;
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="DisplayText" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(
        nameof(DisplayText),
        typeof(string),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnDisplayTextChanged));

    /// <summary>
    /// Identifies the <see cref="HighlightText" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HighlightTextProperty = DependencyProperty.Register(
        nameof(HighlightText),
        typeof(string),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="HighlightRenderMode" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HighlightRenderModeProperty = DependencyProperty.Register(
        nameof(HighlightRenderMode),
        typeof(XHighlightRenderMode),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(XHighlightRenderMode.Simple, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="HighlightBrush" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register(
        nameof(HighlightBrush),
        typeof(Brush),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(DefaultHighlightBrush, FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="HighlightForeground" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HighlightForegroundProperty = DependencyProperty.Register(
        nameof(HighlightForeground),
        typeof(Brush),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="HighlightCornerRadius" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HighlightCornerRadiusProperty = DependencyProperty.Register(
        nameof(HighlightCornerRadius),
        typeof(CornerRadius),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(new CornerRadius(3), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="HighlightPadding" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HighlightPaddingProperty = DependencyProperty.Register(
        nameof(HighlightPadding),
        typeof(Thickness),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(new Thickness(2, 0, 2, 0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="HighlightMargin" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HighlightMarginProperty = DependencyProperty.Register(
        nameof(HighlightMargin),
        typeof(Thickness),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="HighlightBaselineAlignment" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty HighlightBaselineAlignmentProperty = DependencyProperty.Register(
        nameof(HighlightBaselineAlignment),
        typeof(BaselineAlignment),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(BaselineAlignment.Center, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="IsCaseSensitive" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsCaseSensitiveProperty = DependencyProperty.Register(
        nameof(IsCaseSensitive),
        typeof(bool),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="MinimumHighlightTextLength" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty MinimumHighlightTextLengthProperty = DependencyProperty.Register(
        nameof(MinimumHighlightTextLength),
        typeof(int),
        typeof(XHighlightTextBlock),
        new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnHighlightPropertyChanged));
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XHighlightTextBlock" /> class.
    /// </summary>
    static XHighlightTextBlock()
    {
        TextProperty.OverrideMetadata(
            typeof(XHighlightTextBlock),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnTextChanged));
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the text that should be displayed and highlighted.
    /// </summary>
    public string? DisplayText
    {
        get => (string?)GetValue(DisplayTextProperty);
        set => SetValue(DisplayTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the search text that should be highlighted inside <see cref="TextBlock.Text" />.
    /// </summary>
    public string? HighlightText
    {
        get => (string?)GetValue(HighlightTextProperty);
        set => SetValue(HighlightTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the rendering mode used for matching text fragments.
    /// </summary>
    public XHighlightRenderMode HighlightRenderMode
    {
        get => (XHighlightRenderMode)GetValue(HighlightRenderModeProperty);
        set => SetValue(HighlightRenderModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the background brush used for highlighted text fragments.
    /// </summary>
    public Brush? HighlightBrush
    {
        get => (Brush?)GetValue(HighlightBrushProperty);
        set => SetValue(HighlightBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground brush used for highlighted text fragments.
    /// When unset, the normal text foreground is inherited.
    /// </summary>
    public Brush? HighlightForeground
    {
        get => (Brush?)GetValue(HighlightForegroundProperty);
        set => SetValue(HighlightForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius used for badge-highlighted text fragments.
    /// </summary>
    public CornerRadius HighlightCornerRadius
    {
        get => (CornerRadius)GetValue(HighlightCornerRadiusProperty);
        set => SetValue(HighlightCornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the inner spacing used for badge-highlighted text fragments.
    /// </summary>
    public Thickness HighlightPadding
    {
        get => (Thickness)GetValue(HighlightPaddingProperty);
        set => SetValue(HighlightPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the outer spacing used for badge-highlighted text fragments.
    /// </summary>
    public Thickness HighlightMargin
    {
        get => (Thickness)GetValue(HighlightMarginProperty);
        set => SetValue(HighlightMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets the baseline alignment used for badge-highlighted text fragments.
    /// </summary>
    public BaselineAlignment HighlightBaselineAlignment
    {
        get => (BaselineAlignment)GetValue(HighlightBaselineAlignmentProperty);
        set => SetValue(HighlightBaselineAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether text matching should be case-sensitive.
    /// </summary>
    public bool IsCaseSensitive
    {
        get => (bool)GetValue(IsCaseSensitiveProperty);
        set => SetValue(IsCaseSensitiveProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum search text length required before highlighting is applied.
    /// </summary>
    public int MinimumHighlightTextLength
    {
        get => (int)GetValue(MinimumHighlightTextLengthProperty);
        set => SetValue(MinimumHighlightTextLengthProperty, value);
    }
    #endregion

    #region ### Private Methods ###
    private static SolidColorBrush CreateDefaultHighlightBrush()
    {
        return XBrushFactory.CreateFrozenBrush(0x66, 0xFF, 0xD5, 0x4A);
    }

    private Binding CreateSelfBinding(DependencyProperty dependencyProperty)
    {
        return new Binding
        {
            Path = new PropertyPath(dependencyProperty),
            Source = this,
            Mode = BindingMode.OneWay
        };
    }

    private static void OnDisplayTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not XHighlightTextBlock textBlock)
        {
            return;
        }

        textBlock.usesDisplayTextSource = true;
        textBlock.currentText = eventArgs.NewValue as string ?? string.Empty;
        textBlock.hasCurrentText = true;
        textBlock.UpdateInlines();
    }

    private static void OnTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is not XHighlightTextBlock textBlock)
        {
            return;
        }

        if (textBlock.isUpdatingInlines || textBlock.usesDisplayTextSource)
        {
            return;
        }

        textBlock.currentText = eventArgs.NewValue as string ?? string.Empty;
        textBlock.hasCurrentText = true;
        textBlock.UpdateInlines();
    }

    private static void OnHighlightPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XHighlightTextBlock textBlock)
        {
            textBlock.UpdateInlines();
        }
    }

    private void UpdateInlines()
    {
        if (isUpdatingInlines)
        {
            return;
        }

        if (!hasCurrentText)
        {
            currentText = usesDisplayTextSource
                ? DisplayText ?? string.Empty
                : Text ?? string.Empty;
            hasCurrentText = true;
        }

        isUpdatingInlines = true;

        try
        {
            string text = currentText;
            string searchText = HighlightText?.Trim() ?? string.Empty;
            int minimumLength = Math.Max(1, MinimumHighlightTextLength);

            if (string.IsNullOrEmpty(text))
            {
                ApplyPlainText(string.Empty);
                return;
            }

            if (HighlightRenderMode == XHighlightRenderMode.None || searchText.Length < minimumLength)
            {
                ApplyPlainText(text);
                return;
            }

            ApplyHighlightedTextMode();

            StringComparison comparison = IsCaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
            int currentIndex = 0;

            while (currentIndex < text.Length)
            {
                int matchIndex = text.IndexOf(searchText, currentIndex, comparison);

                if (matchIndex < 0)
                {
                    AddNormalRun(text[currentIndex..]);
                    break;
                }

                if (matchIndex > currentIndex)
                {
                    AddNormalRun(text[currentIndex..matchIndex]);
                }

                AddHighlightedText(text.Substring(matchIndex, searchText.Length));
                currentIndex = matchIndex + searchText.Length;
            }
        }
        finally
        {
            isUpdatingInlines = false;
        }
    }

    private void ApplyPlainText(string value)
    {
        if (Inlines.Count > 0)
        {
            Inlines.Clear();
        }

        if (!string.Equals(Text, value, StringComparison.Ordinal))
        {
            SetCurrentValue(TextProperty, value);
        }
    }

    private void ApplyHighlightedTextMode()
    {
        if (!string.IsNullOrEmpty(Text))
        {
            SetCurrentValue(TextProperty, string.Empty);
        }

        Inlines.Clear();
    }

    private void AddNormalRun(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            Inlines.Add(new Run(value));
        }
    }

    private void AddHighlightedText(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (HighlightRenderMode == XHighlightRenderMode.Badge)
        {
            AddHighlightedBlock(value);
            return;
        }

        AddHighlightedRun(value);
    }

    private void AddHighlightedRun(string value)
    {
        Run run = new(value)
        {
            Background = HighlightBrush
        };

        if (HighlightForeground is not null)
        {
            run.Foreground = HighlightForeground;
        }

        Inlines.Add(run);
    }

    private void AddHighlightedBlock(string value)
    {
        TextBlock textBlock = new()
        {
            Text = value,
            Foreground = HighlightForeground ?? Foreground,
            VerticalAlignment = VerticalAlignment.Center
        };

        textBlock.SetBinding(TextBlock.FontFamilyProperty, CreateSelfBinding(FontFamilyProperty));
        textBlock.SetBinding(TextBlock.FontSizeProperty, CreateSelfBinding(FontSizeProperty));
        textBlock.SetBinding(TextBlock.FontStretchProperty, CreateSelfBinding(FontStretchProperty));
        textBlock.SetBinding(TextBlock.FontStyleProperty, CreateSelfBinding(FontStyleProperty));
        textBlock.SetBinding(TextBlock.FontWeightProperty, CreateSelfBinding(FontWeightProperty));

        Border border = new()
        {
            Child = textBlock,
            SnapsToDevicePixels = true,
            VerticalAlignment = VerticalAlignment.Center
        };

        border.SetBinding(Border.BackgroundProperty, CreateSelfBinding(HighlightBrushProperty));
        border.SetBinding(Border.CornerRadiusProperty, CreateSelfBinding(HighlightCornerRadiusProperty));
        border.SetBinding(Border.PaddingProperty, CreateSelfBinding(HighlightPaddingProperty));
        border.SetBinding(FrameworkElement.MarginProperty, CreateSelfBinding(HighlightMarginProperty));

        InlineUIContainer inline = new(border)
        {
            BaselineAlignment = HighlightBaselineAlignment
        };

        Inlines.Add(inline);
    }
    #endregion
}
#endregion
