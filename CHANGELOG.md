# Changelog

All notable changes to VIA.WPF are tracked here.

This project follows the spirit of semantic versioning. Public API breaks should be documented clearly before a public release.

## 1.0.0 - In development

### Added

- Modular VIA.WPF package structure:
  - `VIA.WPF`
  - `VIA.WPF.Controls`
  - `VIA.WPF.MVVM`
  - `VIA.WPF.Themes`
  - `VIA.WPF.Icons`
  - `VIA.WPF.Windowing`
  - `VIA.WPF.Tests`
  - `VIA.WPF.Demo`
- Modern WPF control set with shared XAML namespace.
- Theme infrastructure with brush keys and presets.
- Icon controls and icon-pack wrappers.
- Optional icon-pack packages:
  - `VIA.WPF.Icons.Core`
  - `VIA.WPF.Icons.MaterialDesign`
  - `VIA.WPF.Icons.Material`
  - `VIA.WPF.Icons.Bootstrap`
  - `VIA.WPF.Icons.FontAwesome`
  - `VIA.WPF.Icons.File`
  - `VIA.WPF.Icons.Modern`
  - `VIA.WPF.Icons.Phosphor`
  - `VIA.WPF.Icons.Fluent`
- Tests for strongly typed icon controls and markup extension constructor arguments.
- Windowing support.
- Dynamic application localization infrastructure with `XLocalizationService`, `XLocExtension` and bindable localized strings.
- Optional German/English `XLanguageSelector`, including an opt-in `XWindow` title bar integration next to the theme selector.
- Compact vector flags for the built-in German and English language selector items and a complete localization demo page.
- MVVM infrastructure.
- Validation system based on `INotifyDataErrorInfo`.
- Severity-aware validation messages.
- Parameterized validation texts.
- Resource-based validation localization.
- Async validation with cancellation and stale-result protection.
- Validation hint popup and summary UI.
- XDataGrid with search, filtering, sorting, action column and view-state persistence.
- Unit tests for validation, controls, converters, behaviours, extensions, themes, navigation, windowing and selected specialized controls.
- GitHub Actions CI workflow for restore, release build and test execution.
- CI status badge in the root README.
- MIT license file and NuGet license metadata.
- Shared NuGet package metadata in `Directory.Build.props`.
- Fluent validation builder comparison rules for non-nullable value properties.

### Changed

- Validation moved away from classic WPF `ValidationRule`-centric design toward reusable MVVM validation.
- WPF default validation adorners are suppressed where VIA.WPF validation chrome/hints are responsible for presentation.
- XDataGrid may be split into partial files for maintainability, without changing behavior.
- NuGet package descriptions were improved for the combined package and all modular packages.
- Package authors and copyright metadata were aligned with the MIT license owner.
- The demo project now uses the combined `VIA.WPF` project reference instead of referencing all package projects directly.
- Strongly typed icon controls now share a common internal base implementation without changing public control names.
- Strongly typed icon markup extensions now share a common implementation and support constructor-based kind assignment.
- `VIA.WPF.Icons` remains the full compatibility package while optional icon-pack packages can be referenced individually.
- `XValidationResult.Success()` now returns a shared successful result instance, and empty message sequences reuse it.

### Fixed

- Validation disable behavior clears current messages and suppresses explicit validation.
- Async validation avoids applying stale results after newer validation runs.
- Test parallelization should be disabled when tests modify global localization settings.
- The combined `VIA.WPF` package now depends on `VIA.WPF.Controls`, so the complete package actually brings in the control library.
- Theme infrastructure resource loading was corrected for CI and package verification.
- NuGet package inspection confirmed README inclusion, package metadata and combined package dependencies.
- Validation severity snapshots are cached with the current message snapshot to avoid repeated LINQ filtering in bound UI.

### Verification

Current release-hardening baseline:

```text
Build: green
GitHub Actions: green
Tests: 1865/1865 passing
NuGet package metadata: present
NuGet license metadata: MIT
Package README files: included
```

### Known remaining work

- XDataGrid performance baseline.
- Accessibility audit.
- Package icon and optional release assets.
- Public documentation expansion.
- Final demo review.
- Manual smoke test in a real consuming application.
- Release candidate package inspection.