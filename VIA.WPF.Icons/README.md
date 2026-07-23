# VIA.WPF.Icons

Full compatibility icon package for VIA.WPF WPF applications.

This package keeps the existing all-in-one icon behavior. It includes the universal `XIcon`, all strongly typed icon controls and all supported icon-pack wrappers.

## Target framework

```text
.NET 9 / WPF
```

## Package reference

```xml
<PackageReference Include="VIA.WPF.Icons" Version="1.0.0" />
```

## Optional icon packages

Applications that only need a specific icon pack can reference one of the optional packages instead:

```xml
<PackageReference Include="VIA.WPF.Icons.MaterialDesign" Version="1.0.0" />
<PackageReference Include="VIA.WPF.Icons.Bootstrap" Version="1.0.0" />
<PackageReference Include="VIA.WPF.Icons.Fluent" Version="1.0.0" />
```

Available optional packages:

```text
VIA.WPF.Icons.Core
VIA.WPF.Icons.MaterialDesign
VIA.WPF.Icons.Material
VIA.WPF.Icons.Bootstrap
VIA.WPF.Icons.FontAwesome
VIA.WPF.Icons.File
VIA.WPF.Icons.Modern
VIA.WPF.Icons.Phosphor
VIA.WPF.Icons.Fluent
```

Do not reference the full compatibility package and optional icon packages together unless this is intentional during migration.

## XAML namespace

```xml
xmlns:via="http://schemas.via.dev/wpf"
```

## Main areas

- `XIcon`
- icon-specific controls
- icon markup extensions
- wrappers for supported icon packs

## Typical usage

```xml
<via:XIcon Kind="ContentSave" />
```

Inside a button:

```xml
<via:XButton
    Content="Save"
    Icon="{via:MaterialDesignIcon ContentSave}" />
```

## Supported icon areas

The full package is designed to provide one VIA.WPF-facing icon API while wrapping multiple icon sources.

For smaller dependency graphs, prefer the optional icon packages when an application does not need the full icon stack.
