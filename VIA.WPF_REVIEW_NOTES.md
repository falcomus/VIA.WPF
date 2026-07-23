# VIA.WPF Review Notes

This document summarizes the most important findings from external review discussions and the resulting action plan.

## Strong points

- The validation module is a real differentiator.
- `INotifyDataErrorInfo` integration is clean.
- Multi-property validation is supported.
- Async validation handles cancellation and stale results.
- Validation severities make the UX richer than classic WPF errors.
- Validation popup and inline hints solve a real WPF pain point.
- Package separation is sensible.
- Theme, icon, control and windowing modules form a coherent library direction.

## Main risks

- Public release trust depends on documentation, tests, CI and package hygiene.
- XDataGrid is powerful but must stay maintainable and measurable.
- Static/global localization state must be documented clearly.
- WPF control behavior needs manual and automated smoke testing.
- Accessibility must be checked before a public release.
- NuGet metadata must not contain placeholders.

## Decisions

### Validation localization

`XValidationLocalization` uses global static state. This is acceptable for the current library design, but must be documented.

Implications:

- Resource manager is process-wide.
- Culture is process-wide.
- Missing-resource behavior is process-wide.
- Tests must capture and restore settings.
- Parallel tests that modify localization settings should not run concurrently.

### XValidatableObject threading

`XValidatableObject` is designed for WPF/MVVM usage and should be owned by the UI or view-model owner thread.

Implications:

- Async validation may run asynchronous work.
- Final state is still intended to be consumed by the UI owner.
- It is not advertised as a general-purpose fully concurrent validation store.

### XValidationHintPopup

The popup supports:

- `XValidatableObject`
- `IEnumerable<XValidationError>`
- foreign `INotifyDataErrorInfo` sources

Warnings and information are VIA.WPF extensions. Classic `INotifyDataErrorInfo` only models errors.

### XDataGrid

XDataGrid should be refactored into partial files for maintainability, but only mechanically:

- no public API changes
- no behavior changes
- no XAML changes
- no performance rewrite during the partial split

## Remaining work

- Broader tests outside validation.
- WPF control smoke tests.
- XDataGrid performance baseline.
- Accessibility checklist.
- CI/CD.
- NuGet metadata finalization.
- Documentation site or handbook.
