# Third-party notices

VIA.WPF includes, references or wraps functionality from the following third-party projects.

This file is intended as a human-readable overview. The actual license terms are governed by the respective third-party projects and NuGet packages.

Design references, inspiration sources and projects that are only reviewed for comparison are not listed here unless VIA.WPF uses their code, assets or NuGet packages.

## Microsoft .NET and WPF

VIA.WPF targets .NET 9 and WPF on Windows.

Related components:

- .NET
- WPF
- Microsoft.WindowsDesktop.App.WPF
- System.Text.Json

These components are provided by Microsoft and the .NET project contributors.

## CommunityToolkit.Mvvm

VIA.WPF uses CommunityToolkit.Mvvm for MVVM infrastructure, observable objects, commands and messenger-related patterns.

Project owner:

- Microsoft and .NET Community Toolkit contributors

## GongSolutions.WPF.DragDrop

VIA.WPF.Controls uses GongSolutions.WPF.DragDrop for drag-and-drop related functionality.

Project owner:

- GongSolutions.WPF.DragDrop contributors

## FluentIcons

VIA.WPF uses FluentIcons packages for Fluent icon support.

Related packages:

- FluentIcons.Common
- FluentIcons.Wpf

Project owner:

- FluentIcons contributors

## MahApps.Metro.IconPacks

VIA.WPF.Icons wraps selected MahApps.Metro.IconPacks packages.

Related icon packages include:

- MahApps.Metro.IconPacks.BootstrapIcons
- MahApps.Metro.IconPacks.Core
- MahApps.Metro.IconPacks.FileIcons
- MahApps.Metro.IconPacks.FontAwesome
- MahApps.Metro.IconPacks.FontAwesome6
- MahApps.Metro.IconPacks.Material
- MahApps.Metro.IconPacks.MaterialDesign
- MahApps.Metro.IconPacks.Modern
- MahApps.Metro.IconPacks.PhosphorIcons

Project owner:

- MahApps.Metro.IconPacks contributors

## Independence notice

VIA.WPF is an independent project.

VIA.WPF is not affiliated with Microsoft, MahApps, FluentIcons, GongSolutions or any other third-party project.

All trademarks, package names and project names belong to their respective owners.

## Maintenance note

Before each public release, verify this file against the current package graph:

```powershell
dotnet list .\VIA.WPF.slnx package --include-transitive
```
