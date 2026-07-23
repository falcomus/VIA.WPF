# VIA.WPF Roadmap

## Guiding principle

> Inside hard coconut. Outside soft banana.

VIA.WPF should absorb the hard WPF complexity internally and expose simple, predictable APIs to application developers.

## Current focus

VIA.WPF is now in release-hardening mode. The validation module is stable, the test suite is green, CI/CD is active, and the NuGet package metadata has been brought to a release-ready baseline.

The next priorities are accessibility, XDataGrid performance measurement, documentation expansion and final release candidate verification.

## Phase 1 - Stabilize validation

Status: complete.

- Property and multi-property validation.
- Async validation.
- Validation severities: error, warning, information.
- Parameterized validation texts.
- Resource-based localization.
- `INotifyDataErrorInfo` integration.
- Validation popup and summary UI.
- Unit tests for validation behavior.
- Documentation of global localization state.
- Documentation of UI-thread ownership assumptions.

## Phase 2 - Documentation and release hygiene

Status: mostly complete.

Completed:

- Package READMEs.
- Root README.
- CI badge in root README.
- Changelog.
- Release policy.
- Review notes.
- Minimal demo documentation.
- NuGet metadata checklist.
- Repository URL.
- Project URL.
- NuGet tags.
- MIT license metadata.
- Package authors and copyright alignment.
- Combined package dependency verification.
- Public release readiness checklist.

Remaining:

- Package icon and optional release assets.
- Public documentation expansion.
- Final README review before release.

## Phase 3 - Tests outside validation

Status: mostly complete.

Covered areas include:

- Converter tests.
- Behaviour tests.
- XViewContainer tests.
- XBind / markup extension tests.
- Theme registry and theme manager tests.
- XDataGrid view-state tests.
- XTreeView drag/drop tests.
- Windowing tests.
- Navigation tests.
- Extension tests.
- Service tests.
- Icon tests.
- Dependency property smoke tests.
- Specialized control tests.

Current verification baseline:

```text
1865/1865 tests passing
```

Remaining:

- Add targeted tests only when new features or defects require them.
- Avoid adding broad low-value tests without a concrete risk.

## Phase 4 - WPF control smoke tests

Status: optional future hardening.

Potential test candidates:

- `XValidationHintPopup`
- `XValidationSummary`
- `XViewContainer`
- `XDataGrid`
- `XTreeView`
- main control template smoke tests

Note:

These tests can be useful, but they are also more fragile than pure unit tests. Add them selectively where they reduce real release risk.

## Phase 5 - XDataGrid performance baseline

Status: planned.

Planned measurements:

- 1,000 rows
- 10,000 rows
- optional 50,000 rows
- initial load
- filter popup opening
- filter value generation
- search refresh
- sorting
- view-state save/restore

Potential future improvements:

- Lazy filter value generation.
- Cached property accessors.
- Deferred refresh.
- Optional search highlighting.
- Optional server-side filtering/searching for very large datasets.

Rule:

Do not optimize XDataGrid based on guesswork. Measure first, then decide.

## Phase 6 - Accessibility audit

Status: planned.

Planned checks:

- Keyboard navigation.
- Focus visibility.
- Screen reader basics.
- `AutomationProperties.Name`.
- `AutomationProperties.HelpText`.
- High contrast behavior.
- Tab order.
- Popup usability.
- Validation message discoverability.
- Error color not being the only signal.

Priority controls:

- XButton
- XIconButton
- XTextBox
- XPasswordBox
- XNumberBox
- XSearchBox
- XComboBox
- XCheckBox
- XRadioButton
- XValidationHintPopup
- XValidationSummary
- XViewContainer
- XDataGrid
- XWindow

## Phase 7 - Public release readiness

Status: in progress.

Completed:

- CI build.
- Full test run.
- Changelog.
- SemVer policy.
- MIT license decision.
- Repository URL.
- Project URL.
- NuGet tags.
- Final package metadata baseline.
- Package README inclusion.
- Combined package dependency check.
- Demo project uses combined VIA.WPF reference.
- GitHub Actions verification.

Remaining before a broad public release:

- Package icon decision.
- Final documentation review.
- Demo review.
- Accessibility audit.
- XDataGrid performance baseline.
- Manual smoke test in a real consuming application.
- Release candidate package inspection.
- Release tag.