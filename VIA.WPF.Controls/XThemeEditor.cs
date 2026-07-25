using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VIA.WPF.Themes;

namespace VIA.WPF.Controls;

/// <summary>
/// Provides an editable light and dark VIA.WPF palette. Applying a palette registers or updates its theme at runtime.
/// Applications can persist <see cref="Palette"/> independently.
/// </summary>
public class XThemeEditor : Control
{
    public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(
        nameof(Palette), typeof(XThemePaletteDefinition), typeof(XThemeEditor), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty SelectedThemeProperty = DependencyProperty.Register(
        nameof(SelectedTheme), typeof(XTheme), typeof(XThemeEditor), new FrameworkPropertyMetadata(null, OnSelectedThemeChanged));
    public static readonly DependencyProperty SelectedColorSeedProperty = DependencyProperty.Register(
        nameof(SelectedColorSeed), typeof(XThemeColorSeed), typeof(XThemeEditor), new FrameworkPropertyMetadata(null));

    public static readonly RoutedUICommand ApplyThemeCommand = new("Apply theme", nameof(ApplyThemeCommand), typeof(XThemeEditor));
    public static readonly RoutedUICommand CreateThemeCommand = new("Create theme", nameof(CreateThemeCommand), typeof(XThemeEditor));

    static XThemeEditor()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(XThemeEditor), new FrameworkPropertyMetadata(typeof(XThemeEditor)));
        CommandManager.RegisterClassCommandBinding(typeof(XThemeEditor), new CommandBinding(ApplyThemeCommand, static (sender, _) => ((XThemeEditor)sender).ApplyPalette()));
        CommandManager.RegisterClassCommandBinding(typeof(XThemeEditor), new CommandBinding(CreateThemeCommand, static (sender, _) => ((XThemeEditor)sender).CreatePalette()));
    }

    public XThemeEditor()
    {
        this.Loaded += this.OnLoaded;
    }

    /// <summary>Gets the registered themes available for selection.</summary>
    public IReadOnlyList<XTheme> AvailableThemes => XThemeService.Registry.Themes;

    /// <summary>Gets or sets the editable palette. Bind this to persist application-defined themes.</summary>
    public XThemePaletteDefinition? Palette
    {
        get => (XThemePaletteDefinition?)GetValue(PaletteProperty);
        set => SetValue(PaletteProperty, value);
    }

    /// <summary>Gets or sets the source theme from which the editor loads its palette.</summary>
    public XTheme? SelectedTheme
    {
        get => (XTheme?)GetValue(SelectedThemeProperty);
        set => SetValue(SelectedThemeProperty, value);
    }

    /// <summary>Gets or sets the palette role currently edited in the light and dark pickers.</summary>
    public XThemeColorSeed? SelectedColorSeed
    {
        get => (XThemeColorSeed?)GetValue(SelectedColorSeedProperty);
        set => SetValue(SelectedColorSeedProperty, value);
    }

    /// <summary>Raised after the palette has been converted, registered and applied.</summary>
    public event EventHandler<XTheme>? ThemeApplied;

    private static void OnSelectedThemeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependencyObject is XThemeEditor editor && eventArgs.NewValue is XTheme theme)
        {
            editor.Palette = XThemePaletteDefinition.FromTheme(theme);
            editor.SelectedColorSeed = editor.Palette.Colors.FirstOrDefault();
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs eventArgs)
    {
        XThemeService.EnsureBuiltInThemesRegistered();

        if (this.SelectedTheme is null)
        {
            this.SelectedTheme = XThemeManager.Current.CurrentTheme;
        }

        this.Palette ??= this.SelectedTheme is null
            ? new XThemePaletteDefinition()
            : XThemePaletteDefinition.FromTheme(this.SelectedTheme);
        this.SelectedColorSeed ??= this.Palette.Colors.FirstOrDefault();
    }

    private void ApplyPalette()
    {
        if (this.Palette is null || string.IsNullOrWhiteSpace(this.Palette.Name))
        {
            return;
        }

        XTheme theme = this.Palette.CreateTheme();
        XThemeService.ApplyOrUpdateTheme(theme);
        this.SelectedTheme = theme;
        this.ThemeApplied?.Invoke(this, theme);
    }

    private void CreatePalette()
    {
        XThemePaletteDefinition source = this.Palette
            ?? (this.SelectedTheme is null
                ? new XThemePaletteDefinition()
                : XThemePaletteDefinition.FromTheme(this.SelectedTheme));

        this.SelectedTheme = null;
        this.Palette = source.Clone($"{source.Name} copy");
        this.SelectedColorSeed = this.Palette.Colors.FirstOrDefault();
    }
}
