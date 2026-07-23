# VIA.WPF.Icons.Fluent

Optional Fluent icon controls and markup extensions for VIA.WPF.

## Package reference

```xml
<PackageReference Include="VIA.WPF.Icons.Fluent" Version="1.0.0" />
```

This package references only:

```text
FluentIcons.Wpf 2.1.325
VIA.WPF.Icons.Core
```

Use this package when an application needs this icon pack without referencing the full `VIA.WPF.Icons` compatibility package.

## XAML namespace

```xml
xmlns:via="http://schemas.via.dev/wpf"
```

## Usage

```xml
<via:XFluentIcon Icon="Add" />
```

Inside VIA controls:

```xml
<via:XButton
    Content="Save"
    Icon="{via:FluentIcon Add}" />
```

Do not reference this optional package together with the full `VIA.WPF.Icons` package unless you intentionally need both during migration.
