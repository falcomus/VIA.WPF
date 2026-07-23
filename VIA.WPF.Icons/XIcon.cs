// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XIcon.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace VIA.WPF.Icons;

#region ### Class XIcon ###
/// <summary>
/// Represents the standard icon control of VIA.WPF.
/// </summary>
[TemplatePart(Name = PartIconHost, Type = typeof(ContentControl))]
public class XIcon : Control
{
    #region ### Constants ###
    private const string PartIconHost = "PART_IconHost";
    #endregion

    #region ### Dependency Properties ###
    /// <summary>
    /// Identifies the <see cref="Foreground"/> dependency property.
    /// </summary>
    public static readonly new DependencyProperty ForegroundProperty = Control.ForegroundProperty.AddOwner(
        typeof(XIcon),
        new FrameworkPropertyMetadata(
            SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Identifies the <see cref="Pack"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PackProperty = DependencyProperty.Register(
        nameof(Pack),
        typeof(XIconPack),
        typeof(XIcon),
        new FrameworkPropertyMetadata(
            XIconPack.MaterialDesign,
            OnIconDefinitionPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="Kind"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty KindProperty = DependencyProperty.Register(
        nameof(Kind),
        typeof(object),
        typeof(XIcon),
        new FrameworkPropertyMetadata(
            null,
            OnIconDefinitionPropertyChanged));

    /// <summary>
    /// Identifies the <see cref="Size"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(double),
        typeof(XIcon),
        new FrameworkPropertyMetadata(
            16d,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Identifies the <see cref="Stretch"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
        nameof(Stretch),
        typeof(Stretch),
        typeof(XIcon),
        new FrameworkPropertyMetadata(
            Stretch.Uniform,
            FrameworkPropertyMetadataOptions.AffectsRender));
    #endregion

    #region ### Private Fields ###
    private ContentControl? _iconHost;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XIcon"/> class.
    /// </summary>
    static XIcon()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XIcon),
            new FrameworkPropertyMetadata(typeof(XIcon)));

    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets or sets the icon foreground brush.
    /// </summary>
    public new Brush Foreground
    {
        get => (Brush)this.GetValue(ForegroundProperty);
        set => this.SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon pack.
    /// </summary>
    public XIconPack Pack
    {
        get => (XIconPack)this.GetValue(PackProperty);
        set => this.SetValue(PackProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon kind.
    /// </summary>
    /// <remarks>
    /// This property accepts either a matching MahApps enum value or a string containing the enum member name.
    /// </remarks>
    public object? Kind
    {
        get => this.GetValue(KindProperty);
        set => this.SetValue(KindProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon size.
    /// </summary>
    public double Size
    {
        get => (double)this.GetValue(SizeProperty);
        set => this.SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon stretch mode.
    /// </summary>
    public Stretch Stretch
    {
        get => (Stretch)this.GetValue(StretchProperty);
        set => this.SetValue(StretchProperty, value);
    }
    #endregion

    #region ### Public Methods ###
    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this._iconHost = this.GetTemplateChild(PartIconHost) as ContentControl;
        this.UpdateIconHostContent();
    }
    #endregion

    #region ### Private Methods ###
    /// <summary>
    /// Handles icon definition changes.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="eventArgs">The event arguments.</param>
    private static void OnIconDefinitionPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XIcon icon)
        {
            icon.UpdateIconHostContent();
        }
    }

    /// <summary>
    /// Updates the icon host content.
    /// </summary>
    private void UpdateIconHostContent()
    {
        if (this._iconHost is null)
        {
            return;
        }

        this._iconHost.Content = this.CreateIconElement();
    }

    /// <summary>
    /// Creates the icon element for the current pack and kind.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if no valid icon could be created.</returns>
    private FrameworkElement? CreateIconElement()
    {
        if (this.Kind is null)
        {
            return null;
        }

        return this.Pack switch
        {
            XIconPack.MaterialDesign => this.CreateMaterialDesignIcon(),
            XIconPack.Material => this.CreateMaterialIcon(),
            XIconPack.BootstrapIcons => this.CreateBootstrapIcon(),
            XIconPack.FontAwesome => this.CreateFontAwesomeIcon(),
            XIconPack.FontAwesome6 => this.CreateFontAwesome6Icon(),
            XIconPack.Modern => this.CreateModernIcon(),
            XIconPack.PhosphorIcons => this.CreatePhosphorIcon(),
            XIconPack.FileIcons => this.CreateFileIcon(),
            _ => null
        };
    }

    /// <summary>
    /// Creates a material design icon.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if the icon kind is invalid.</returns>
    private FrameworkElement? CreateMaterialDesignIcon()
    {
        if (!TryResolveKind<PackIconMaterialDesignKind>(this.Kind, out PackIconMaterialDesignKind kind))
        {
            return null;
        }

        PackIconMaterialDesign icon = new()
        {
            Kind = kind
        };

        this.ApplyCommonBindings(icon);
        return icon;
    }

    /// <summary>
    /// Creates a material icon.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if the icon kind is invalid.</returns>
    private FrameworkElement? CreateMaterialIcon()
    {
        if (!TryResolveKind<PackIconMaterialKind>(this.Kind, out PackIconMaterialKind kind))
        {
            return null;
        }

        PackIconMaterial icon = new()
        {
            Kind = kind
        };

        this.ApplyCommonBindings(icon);
        return icon;
    }

    /// <summary>
    /// Creates a bootstrap icon.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if the icon kind is invalid.</returns>
    private FrameworkElement? CreateBootstrapIcon()
    {
        if (!TryResolveKind<PackIconBootstrapIconsKind>(this.Kind, out PackIconBootstrapIconsKind kind))
        {
            return null;
        }

        PackIconBootstrapIcons icon = new()
        {
            Kind = kind
        };

        this.ApplyCommonBindings(icon);
        return icon;
    }

    /// <summary>
    /// Creates a Font Awesome icon.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if the icon kind is invalid.</returns>
    private FrameworkElement? CreateFontAwesomeIcon()
    {
        if (!TryResolveKind<PackIconFontAwesome6Kind>(this.Kind, out PackIconFontAwesome6Kind kind))
        {
            return null;
        }

        PackIconFontAwesome6 icon = new()
        {
            Kind = kind
        };

        this.ApplyCommonBindings(icon);
        return icon;
    }

    /// <summary>
    /// Creates a Font Awesome 6 icon.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if the icon kind is invalid.</returns>
    private FrameworkElement? CreateFontAwesome6Icon()
    {
        if (!TryResolveKind<PackIconFontAwesome6Kind>(this.Kind, out PackIconFontAwesome6Kind kind))
        {
            return null;
        }

        PackIconFontAwesome6 icon = new()
        {
            Kind = kind
        };

        this.ApplyCommonBindings(icon);
        return icon;
    }

    /// <summary>
    /// Creates a modern icon.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if the icon kind is invalid.</returns>
    private FrameworkElement? CreateModernIcon()
    {
        if (!TryResolveKind<PackIconModernKind>(this.Kind, out PackIconModernKind kind))
        {
            return null;
        }

        PackIconModern icon = new()
        {
            Kind = kind
        };

        this.ApplyCommonBindings(icon);
        return icon;
    }

    /// <summary>
    /// Creates a phosphor icon.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if the icon kind is invalid.</returns>
    private FrameworkElement? CreatePhosphorIcon()
    {
        if (!TryResolveKind<PackIconPhosphorIconsKind>(this.Kind, out PackIconPhosphorIconsKind kind))
        {
            return null;
        }

        PackIconPhosphorIcons icon = new()
        {
            Kind = kind
        };

        this.ApplyCommonBindings(icon);
        return icon;
    }

    /// <summary>
    /// Creates a file icon.
    /// </summary>
    /// <returns>The icon element, or <see langword="null"/> if the icon kind is invalid.</returns>
    private FrameworkElement? CreateFileIcon()
    {
        if (!TryResolveKind<PackIconFileIconsKind>(this.Kind, out PackIconFileIconsKind kind))
        {
            return null;
        }

        PackIconFileIcons icon = new()
        {
            Kind = kind
        };

        this.ApplyCommonBindings(icon);
        return icon;
    }

    /// <summary>
    /// Applies common bindings to a MahApps icon control.
    /// </summary>
    /// <param name="icon">The icon control.</param>
    private void ApplyCommonBindings(Control icon)
    {
        BindingOperations.SetBinding(
            icon,
            WidthProperty,
            new Binding(nameof(Size))
            {
                Source = this
            });

        BindingOperations.SetBinding(
            icon,
            HeightProperty,
            new Binding(nameof(Size))
            {
                Source = this
            });

        BindingOperations.SetBinding(
            icon,
            Control.ForegroundProperty,
            new Binding(nameof(Foreground))
            {
                Source = this
            });
    }

    /// <summary>
    /// Tries to resolve a kind value to a specific icon enum.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The raw kind value.</param>
    /// <param name="resolvedValue">The resolved enum value.</param>
    /// <returns><see langword="true"/> if the value could be resolved; otherwise, <see langword="false"/>.</returns>
    private static bool TryResolveKind<TEnum>(object? value, out TEnum resolvedValue)
        where TEnum : struct, Enum
    {
        switch (value)
        {
            case TEnum typedValue:
                resolvedValue = typedValue;
                return true;

            case Enum enumValue when Enum.TryParse(enumValue.ToString(), true, out TEnum parsedEnumValue):
                resolvedValue = parsedEnumValue;
                return true;

            case string textValue when Enum.TryParse(textValue, true, out TEnum parsedTextValue):
                resolvedValue = parsedTextValue;
                return true;

            case not null when Enum.TryParse(value.ToString(), true, out TEnum parsedFallbackValue):
                resolvedValue = parsedFallbackValue;
                return true;

            default:
                resolvedValue = default;
                return false;
        }
    }
    #endregion
}
#endregion