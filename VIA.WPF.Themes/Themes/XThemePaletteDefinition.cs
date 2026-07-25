using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace VIA.WPF.Themes;

/// <summary>
/// Editable seed palette from which VIA.WPF derives a complete light and dark theme.
/// Persist this model in an application when user-defined themes must survive a restart.
/// </summary>
public sealed class XThemePaletteDefinition : INotifyPropertyChanged
{
    private string name;

    public XThemePaletteDefinition(string name = "Custom theme")
    {
        this.name = name;
        this.Colors =
        [
            new("Primary", Color.FromRgb(53, 92, 145), Color.FromRgb(104, 163, 224)),
            new("Accent", Color.FromRgb(109, 91, 158), Color.FromRgb(185, 165, 232)),
            new("Success", Color.FromRgb(16, 124, 65), Color.FromRgb(84, 176, 84)),
            new("Warning", Color.FromRgb(138, 90, 0), Color.FromRgb(244, 191, 79)),
            new("Danger", Color.FromRgb(196, 43, 28), Color.FromRgb(255, 153, 164)),
            new("Info", Color.FromRgb(0, 124, 131), Color.FromRgb(93, 217, 223)),
            new("Background", Color.FromRgb(241, 244, 247), Color.FromRgb(24, 26, 29)),
            new("Surface", System.Windows.Media.Colors.White, Color.FromRgb(37, 40, 44)),
            new("Navigation", Color.FromRgb(32, 37, 43), Color.FromRgb(21, 24, 28))
        ];
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Name
    {
        get => this.name;
        set
        {
            if (this.name == value)
            {
                return;
            }

            this.name = value;
            this.OnPropertyChanged();
        }
    }

    public IReadOnlyList<XThemeColorSeed> Colors { get; }

    /// <summary>Creates a complete theme with all derived states and tokens.</summary>
    public XTheme CreateTheme()
    {
        return XThemePresetFactory.Create(new XThemePalette
        {
            Name = this.Name,
            PrimaryLight = this["Primary"].Light,
            PrimaryDark = this["Primary"].Dark,
            AccentLight = this["Accent"].Light,
            AccentDark = this["Accent"].Dark,
            SuccessLight = this["Success"].Light,
            SuccessDark = this["Success"].Dark,
            WarningLight = this["Warning"].Light,
            WarningDark = this["Warning"].Dark,
            DangerLight = this["Danger"].Light,
            DangerDark = this["Danger"].Dark,
            InfoLight = this["Info"].Light,
            InfoDark = this["Info"].Dark,
            BackgroundLight = this["Background"].Light,
            BackgroundDark = this["Background"].Dark,
            SurfaceLight = this["Surface"].Light,
            SurfaceDark = this["Surface"].Dark,
            NavigationLight = this["Navigation"].Light,
            NavigationDark = this["Navigation"].Dark
        });
    }

    /// <summary>Creates an editable palette from an existing complete theme.</summary>
    public static XThemePaletteDefinition FromTheme(XTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        XThemePaletteDefinition result = new(theme.Name);
        result.Set("Primary", theme.Primary.Light, theme.Primary.Dark);
        result.Set("Accent", theme.Accent.Light, theme.Accent.Dark);
        result.Set("Success", theme.Success.Light, theme.Success.Dark);
        result.Set("Warning", theme.Warning.Light, theme.Warning.Dark);
        result.Set("Danger", theme.Danger.Light, theme.Danger.Dark);
        result.Set("Info", theme.Info.Light, theme.Info.Dark);
        result.Set("Background", theme.Background.Light, theme.Background.Dark);
        result.Set("Surface", theme.Surface.Light, theme.Surface.Dark);
        result.Set("Navigation", theme.NavigationPanelBackground.Light, theme.NavigationPanelBackground.Dark);
        return result;
    }

    /// <summary>Creates an independent copy suitable for a new user theme.</summary>
    public XThemePaletteDefinition Clone(string? name = null)
    {
        XThemePaletteDefinition result = new(name ?? this.Name);
        foreach (XThemeColorSeed color in this.Colors)
        {
            result.Set(color.Name, color.Light, color.Dark);
        }

        return result;
    }

    private XThemeColorSeed this[string name] => this.Colors.Single(color => color.Name == name);

    private void Set(string name, Color light, Color dark)
    {
        XThemeColorSeed color = this[name];
        color.Light = light;
        color.Dark = dark;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

/// <summary>Editable light/dark color pair in a theme palette.</summary>
public sealed class XThemeColorSeed : INotifyPropertyChanged
{
    private Color dark;
    private Color light;

    internal XThemeColorSeed(string name, Color light, Color dark)
    {
        this.Name = name;
        this.light = light;
        this.dark = dark;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public string Name { get; }

    public Color Light
    {
        get => this.light;
        set { if (this.light != value) { this.light = value; this.OnPropertyChanged(); this.OnPropertyChanged(nameof(this.LightHex)); this.OnPropertyChanged(nameof(this.LightBrush)); } }
    }

    public Color Dark
    {
        get => this.dark;
        set { if (this.dark != value) { this.dark = value; this.OnPropertyChanged(); this.OnPropertyChanged(nameof(this.DarkHex)); this.OnPropertyChanged(nameof(this.DarkBrush)); } }
    }

    public string LightHex { get => this.Light.ToString(); set { if (TryParse(value, out Color color)) this.Light = color; } }
    public string DarkHex { get => this.Dark.ToString(); set { if (TryParse(value, out Color color)) this.Dark = color; } }
    public Brush LightBrush => new SolidColorBrush(this.Light);
    public Brush DarkBrush => new SolidColorBrush(this.Dark);

    private static bool TryParse(string? value, out Color color)
    {
        try { color = (Color)ColorConverter.ConvertFromString(value ?? string.Empty); return true; }
        catch (FormatException) { color = default; return false; }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
