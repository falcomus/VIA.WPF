# VIA.WPF.Windowing

Window chrome and window command support for VIA.WPF WPF applications.

## Target framework

```text
.NET 9 / WPF
```

## Package reference

```xml
<PackageReference Include="VIA.WPF.Windowing" Version="1.0.0" />
```

## Main areas

- `XWindow`
- window commands
- themed window template
- custom window chrome support

## Typical usage

```xml
<via:XWindow
    x:Class="MyApp.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:via="http://schemas.via.dev/wpf"
    Title="My App">
</via:XWindow>
```

## Notes

Windowing behavior should be tested in the real host application because shell integration, DPI behavior and window chrome edge cases depend on the application environment.

## Optional title bar language selector

`XWindow` can host the VIA.WPF language selector next to the theme selector:

```xml
<via:XWindow
    ShowLanguageSelector="True"
    ShowThemeSelector="True" />
```

Relevant properties:

```text
ShowLanguageSelector
AvailableLanguages
SelectedLanguage
ApplyLanguageFormattingCulture
```

The default language list is `XLanguages.Default` with German and English. Host applications remain responsible for their resource files and for persisting the selected language when persistence is required.

