# VIA.WPF.Themes

Theme infrastructure for VIA.WPF WPF applications.

## Target framework

```text
.NET 9 / WPF
```

## Package reference

```xml
<PackageReference Include="VIA.WPF.Themes" Version="1.0.0" />
```

## Main areas

- Theme models.
- Theme registry.
- Theme manager.
- Brush keys.
- Default brushes.
- Theme presets.
- Theme selector.
- Theme transition helpers.

## Typical usage

Use VIA.WPF theme resources through dynamic resources and brush keys.

```xml
<Border
    Background="{DynamicResource {x:Static via:XBrushKeys.Surface}}"
    BorderBrush="{DynamicResource {x:Static via:XBrushKeys.Border}}" />
```

## Presets

VIA.WPF includes predefined theme presets such as:

```text
Default
Amber
Azure
Crimson
Emerald
Graphite
Indigo
Magenta
Rose
Sandstone
Teal
Violet
```

## Notes

Controls should use VIA.WPF brush keys instead of hard-coded colors where possible. This keeps applications themeable and consistent.
