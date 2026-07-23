# VIA.WPF.Demo

Demo application for the VIA.WPF WPF library.

The demo is used to manually inspect controls, templates, themes, validation behavior, navigation, DataGrid behavior and windowing.

## Recommended manual smoke test

After larger library changes, open the demo and check:

```text
- theme switching
- button variants
- text input controls
- validation demo
- validation popup
- XDataGrid search
- XDataGrid filters
- XDataGrid sorting
- XDataGrid action buttons
- XDataGrid view-state restore
- XTreeView behavior
- XViewContainer navigation
- XWindow behavior
```

## Notes

The demo is not a replacement for automated tests. It is the fastest way to catch visual regressions and WPF template and other issues.

## Localization demo

The `Localization` page verifies the complete language workflow before it is adopted by consuming applications:

```text
XLanguageSelector with compact vector flags
XLoc resource bindings
DE/EN satellite resources
culture-aware date and number formatting
localized code messages
synchronization with the XWindow title bar selector
```

