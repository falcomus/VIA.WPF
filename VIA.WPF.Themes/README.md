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

## Semantic color roles

VIA.WPF presets use a Material-inspired tonal hierarchy. Applications should select
tokens by purpose rather than by a desired literal color:

| Role | Token | Intended use |
| --- | --- | --- |
| App canvas | `Canvas` | Window, page, and workspace backgrounds |
| Container | `Surface` | Panels, groups, cards, and persistent content regions |
| Raised container | `SurfaceRaised` | Prominent cards, detail panes, and floating regions |
| Overlay | `SurfaceOverlay` | Menus, popups, and transient layers |
| Sunken container | `SurfaceSunken` | Wells, tracks, read-only regions, and recessed areas |
| Subtle separation | `BorderSubtle` / `PanelBorder` | Card and panel outlines or dividers |
| Control separation | `BorderDefault` / `ControlBorder` | Inputs and interactive control outlines |
| Strong separation | `BorderStrong` / `ControlBorderStrong` | Emphasized boundaries |
| Interaction | `StateHover`, `StatePressed`, `StateSelected` | Transient interaction states |
| Brand and status | `Primary`, `Accent`, `Status*` | Actions, selection emphasis, and status meaning |

Every built-in preset supplies the same roles in light and dark mode. The shared
preset factory keeps canvas, containers, inputs, and borders visually ordered while
retaining the hue character of each theme.

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

Controls and applications should use VIA.WPF brush keys instead of hard-coded colors.
Use `DynamicResource` for colors that must react to runtime theme or mode changes.
