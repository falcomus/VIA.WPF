# VIA.WPF

[![CI](https://github.com/falcomus/VIA.WPF/actions/workflows/ci.yml/badge.svg)](https://github.com/falcomus/VIA.WPF/actions/workflows/ci.yml)

VIA.WPF is a modular .NET 9 WPF library for building modern desktop applications with a consistent control set, theming, icons, windowing support and MVVM infrastructure.

> Inside hard coconut. Outside soft banana.

The goal is simple: hide the hard WPF details inside the library and give application developers a clear, pleasant API.

## Packages

| Package | Purpose |
| --- | --- |
| `VIA.WPF` | Combined package for applications that want the complete VIA.WPF stack. |
| `VIA.WPF.Controls` | WPF controls, templates, converters, behaviours, validation UI and navigation helpers. |
| `VIA.WPF.MVVM` | Observable view models, validation, editor infrastructure and MVVM helpers. |
| `VIA.WPF.Themes` | Theme resources, brush keys, theme presets and theme switching. |
| `VIA.WPF.Icons` | VIA.WPF icon controls and supported icon-pack wrappers. |
| `VIA.WPF.Windowing` | Window chrome and window command support. |

## Target platform

```text
.NET 9
WPF
Windows
```

## Quick start

For applications that should use the complete VIA.WPF stack, reference the combined package:

```xml
<PackageReference Include="VIA.WPF" Version="1.0.0" />
```

For applications that only need selected areas, reference the smaller packages directly:

```xml
<PackageReference Include="VIA.WPF.Controls" Version="1.0.0" />
<PackageReference Include="VIA.WPF.MVVM" Version="1.0.0" />
<PackageReference Include="VIA.WPF.Themes" Version="1.0.0" />
<PackageReference Include="VIA.WPF.Icons" Version="1.0.0" />
<PackageReference Include="VIA.WPF.Windowing" Version="1.0.0" />
```

Use the common XAML namespace:

```xml
xmlns:via="http://schemas.via.dev/wpf"
```

## Highlights

- Modern WPF controls with consistent sizing, variants and theme brushes.
- Theme presets and dynamic theme resources.
- Icon controls and icon-pack wrappers.
- MVVM base types for observable objects, editors and validation.
- Validation built on `INotifyDataErrorInfo`.
- Severity-aware validation messages: errors, warnings and information.
- Async validation with debounce, cancellation and stale-result protection.
- Validation UI with inline hints and compact popup summaries.
- XDataGrid with search, filtering, sorting, action column and view-state persistence.
- Navigation and windowing helpers for application shells.

## Validation status

The validation module is currently the most mature part of VIA.WPF. It supports property validation, multi-property validation, parameterized and resource-based messages, async checks, severity levels, validation hints and popup summaries.

## Public release status

VIA.WPF is suitable for controlled internal use. Before a broad public release, the remaining work is tracked in `ROADMAP.md`:

```text
- XDataGrid performance baseline
- accessibility audit
- package icon and release assets
- public documentation expansion
- possible icon-package split
```

## License

VIA.WPF Community is licensed under the MIT License.

Future VIA.WPF Pro packages may use a separate commercial license.

## Documentation

Start here:

```text
ROADMAP.md
CHANGELOG.md
VIA.WPF_REVIEW_NOTES.md
docs/MINIMAL_DEMO.md
docs/RELEASE_POLICY.md
```