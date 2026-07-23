# VIA.WPF.Controls

WPF controls, templates, converters, behaviours, validation UI and navigation helpers for VIA.WPF.

## Target framework

```text
.NET 9 / WPF
```

## Package reference

```xml
<PackageReference Include="VIA.WPF.Controls" Version="1.0.0" />
```

## XAML namespace

```xml
xmlns:via="http://schemas.via.dev/wpf"
```

## Control areas

- Buttons and icon buttons.
- Text input controls.
- Combo boxes and lookup controls.
- Panels and layout helpers.
- DataGrid extensions.
- TreeView extensions.
- Navigation helpers.
- Validation UI.
- Converters.
- Behaviours.
- Themed templates.

## Example

```xml
<via:XStackPanel Spacing="12">
    <via:XTextBox
        Header="Name"
        Placeholder="Enter a name"
        Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}" />

    <via:XButton
        Content="Save"
        Variant="Primary"
        Command="{Binding SaveCommand}" />
</via:XStackPanel>
```

## Validation UI

VIA.WPF.Controls contains validation presentation controls that work with the MVVM validation infrastructure.

### XValidationHintPopup

`XValidationHintPopup` displays validation messages from a validation source.

Supported sources:

```text
VIA.WPF.MVVM.XValidatableObject
IEnumerable<XValidationError>
INotifyDataErrorInfo
```

Example:

```xml
<via:XValidationHintPopup
    ValidationSource="{Binding Editor}"
    IncludeWarnings="True"
    IncludeInformation="False" />
```

Behavior:

- With `XValidatableObject`, the popup can show VIA.WPF validation errors, warnings and information messages.
- With `IEnumerable<XValidationError>`, the popup displays the supplied messages directly.
- With foreign `INotifyDataErrorInfo`, errors are wrapped defensively as VIA.WPF validation messages.
- Warnings and information are VIA.WPF extensions and are not part of the classic WPF `INotifyDataErrorInfo` contract.

## XDataGrid

XDataGrid extends WPF `DataGrid` with:

- column filter popups
- free-text search
- sorting
- action column
- keyboard shortcuts
- view-state persistence

For very large datasets, server-side filtering/searching should remain an option. Local filtering is intended for typical client-side WPF grids.

## Notes

This package depends on VIA.WPF.Themes and VIA.WPF.MVVM concepts in several controls. For the full experience, use the combined `VIA.WPF` package or reference the related packages explicitly.

## Localization

VIA.WPF.Controls provides reusable localization infrastructure while application-specific texts remain in the consuming application.

Main types:

```text
XLocalizationService
XLanguage
XLanguages
XLanguageSelector
XLocalizedString
XLocExtension
```

Dynamic XAML resource lookup:

```xml
<TextBlock
    Text="{via:XLoc Save,
                   ResourceManager={x:Static resources:Strings.ResourceManager},
                   Fallback=Save}" />
```

The built-in selector contains German and English definitions by default. German and English are rendered with compact vector flags before the language name. Application-defined languages receive a compact short-code fallback and can replace the selector's `ItemTemplate` when a custom flag presentation is required. Applications can replace `ItemsSource` with their own `XLanguage` collection.

