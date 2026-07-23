# VIA.WPF.Icons.Phosphor

Optional Phosphor icon controls and markup extensions for VIA.WPF.

## Package reference

```xml
<PackageReference Include="VIA.WPF.Icons.Phosphor" Version="1.0.0" />
```

This package references only:

```text
MahApps.Metro.IconPacks.PhosphorIcons 6.2.1
VIA.WPF.Icons.Core
```

Use this package when an application needs this icon pack without referencing the full `VIA.WPF.Icons` compatibility package.

## XAML namespace

```xml
xmlns:via="http://schemas.via.dev/wpf"
```

## Usage

```xml
<via:XPhosphorIcon Kind="Regular_Check" />
```

Inside VIA controls:

```xml
<via:XButton
    Content="Save"
    Icon="{via:PhosphorIcon Regular_Check}" />
```

Do not reference this optional package together with the full `VIA.WPF.Icons` package unless you intentionally need both during migration.
