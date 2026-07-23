# VIA.WPF.Icons.Core

Shared base infrastructure for optional VIA.WPF icon packages.

## Package reference

```xml
<PackageReference Include="VIA.WPF.Icons.Core" Version="1.0.0" />
```

Applications usually do not reference this package directly. It is pulled in by optional packages such as `VIA.WPF.Icons.MaterialDesign` or `VIA.WPF.Icons.Bootstrap`.

## Contains

```text
XIconBase<TKind>
KindIconExtensionBase<TIcon, TKind>
```

The package intentionally has no direct MahApps or FluentIcons dependency.
