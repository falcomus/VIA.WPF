# VIA.WPF.MVVM

MVVM infrastructure for VIA.WPF-based WPF applications.

## Target framework

```text
.NET 9
```

## Package reference

```xml
<PackageReference Include="VIA.WPF.MVVM" Version="1.0.0" />
```

## Main areas

- Observable view-model base types.
- Editor view-model infrastructure.
- Validation infrastructure.
- Severity-aware validation messages.
- Async validation helpers.
- Localization support for validation texts.

## Validation

The validation system is based on `INotifyDataErrorInfo` and provides VIA.WPF-specific additions:

- property validation
- multi-property validation
- async validation
- validation severities
- resource-based texts
- parameterized texts
- message collection for UI presentation

## Example

```csharp
protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
{
    context.Required(this.Name, nameof(this.Name), XValidationText.Text("Name is required."));

    context.Range(
        this.Age,
        0,
        120,
        nameof(this.Age),
        XValidationText.Text("Age must be between {0} and {1}.", 0, 120));

    return Task.CompletedTask;
}
```

## XValidatableObject

`XValidatableObject` is designed for WPF/MVVM usage. Instances should be owned and observed by the UI or view-model owner thread.

Async validation protects against stale results, but the type is not advertised as a fully concurrent general-purpose validation store.

Important behavior:

- `ValidateAllAsync` validates explicitly.
- `ValidateOnPropertyChanged` enables automatic validation.
- `ValidationDelay` debounces automatic validation.
- `IsValidationEnabled = false` clears current messages and suppresses further validation.
- `ValidationMessages` contains all severities.
- `ValidationErrors` contains only error severity messages.

## XValidationLocalization

`XValidationLocalization` uses global static settings.

Global state:

```text
ResourceManager
Culture
ThrowOnMissingResource
```

This is intentional for the current VIA.WPF design, but it has test implications:

- Capture settings before changing them.
- Restore settings after the test.
- Do not run localization-setting tests in parallel.

## Recommended test pattern

```csharp
XValidationLocalizationSettings settings = XValidationLocalization.Capture();

try
{
    // Change localization settings for this test.
}
finally
{
    XValidationLocalization.Restore(settings);
}
```

## Editor infrastructure for CRUD detail dialogs

`XEditorViewModelBase` is intended as the base class for form/detail editors. It provides dirty tracking, read-only state and save validation.

Recommended save flow:

```text
Open editor
    -> load values with WithoutDirtyTracking(...)
    -> MarkClean()
Save
    -> ValidateForSaveAsync()
    -> keep dialog open when errors exist
    -> persist data
    -> MarkClean()
    -> close CRUD context
```

### External validation errors

Server-side or persistence-layer errors can be pushed into an editor without mixing API logic into the view:

```csharp
editor.SetExternalValidationErrors(
[
    XExternalValidationError.FromText(
        "This code already exists.",
        nameof(CategoryEditorViewModel.Code),
        code: "DuplicateCode")
]);
```

External validation errors participate in `ValidateForSaveAsync()` and are exposed through the normal `INotifyDataErrorInfo` pipeline.

### Composite editors

Use `XCompositeEditorViewModelBase` when a detail editor owns child editors such as addresses, contacts or nested settings.

```csharp
public sealed class SupplierEditorViewModel : XCompositeEditorViewModelBase
{
    public SupplierEditorViewModel()
    {
        this.MainAddress = this.RegisterChildEditor(new AddressEditorViewModel());
    }

    public AddressEditorViewModel MainAddress { get; }
}
```

`ValidateForSaveAsync()` validates the root editor and all registered child editors. `MarkClean()` also marks all registered children clean.

## CRUD page infrastructure

CRUD pages that use `XViewContainer` can derive from `XCrudEditorPageViewModelBase<TEntity, TEditor, TKey>` in `VIA.WPF.Controls.Navigation`.

The base class provides:

- `Items` and `SelectedItem`
- `XCrudContext`
- `NewCommand`, `ViewCommand`, `EditCommand`, `DeleteCommand`
- `SaveDetailCommand`
- toolbar command wiring
- a standard save pipeline based on `ValidateForSaveAsync()`

The application still owns entity mapping and persistence by overriding the abstract methods.
