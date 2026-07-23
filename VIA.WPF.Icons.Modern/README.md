# VIA.WPF.Icons.Modern

Optional Modern icon controls and markup extensions for VIA.WPF.

## Package reference

```xml
<PackageReference Include="VIA.WPF.Icons.Modern" Version="1.0.0" />
```

This package references only:

```text
MahApps.Metro.IconPacks.Modern 6.2.1
VIA.WPF.Icons.Core
```

Use this package when an application needs this icon pack without referencing the full `VIA.WPF.Icons` compatibility package.

## XAML namespace

```xml
xmlns:via="http://schemas.via.dev/wpf"
```

## Usage

```xml
<via:XModernIcon Kind="Save" />
```

Inside VIA controls:

```xml
<via:XButton
    Content="Save"
    Icon="{via:ModernIcon Save}" />
```

Do not reference this optional package together with the full `VIA.WPF.Icons` package unless you intentionally need both during migration.
