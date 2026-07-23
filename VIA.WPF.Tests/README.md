# VIA.WPF.Tests

Unit tests for VIA.WPF.

## Target

The current priority is validation correctness.

## Important test rule

Some validation localization tests modify global static state. These tests must not run in parallel with other tests that modify the same settings.

Recommended global setting in the test project:

```csharp
[assembly: CollectionBehavior(DisableTestParallelization = true)]
```

## Current focus

- Validation context helpers.
- Validation text formatting.
- Localization capture/restore.
- `XValidatableObject` validation flow.
- Disabled validation behavior.
- Async stale-result handling.

## Future tests

Planned areas:

```text
- converters
- behaviours
- XViewContainer
- XDataGrid view-state
- XTreeView drag/drop
- theme registry
- windowing
```
