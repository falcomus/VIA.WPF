# Accessibility checklist

This checklist is intended for manual and automated review before a public VIA.WPF release.

## Goal

VIA.WPF controls should be usable with keyboard, visible focus, readable contrast and basic assistive technology support.

This checklist is a working audit document. Each checked control should get a short result entry.

## Audit status

```text
Status:
- Not started
- In progress
- Passed
- Passed with notes
- Needs work
```

## General checks

For every reviewed control, check:

- Keyboard navigation works.
- Focus is visible.
- Focus order is logical.
- Disabled states are visually clear.
- High contrast mode is usable.
- Text has sufficient contrast.
- Tooltips do not contain the only essential information.
- Popups can be dismissed predictably.
- Mouse-only interaction has a keyboard equivalent where practical.
- Validation and error states are discoverable without relying on color alone.

## Automation properties

Check important controls for meaningful automation information:

```text
AutomationProperties.Name
AutomationProperties.HelpText
AutomationProperties.LabeledBy
AutomationProperties.AutomationId
```

Rules:

- Icon-only buttons need a meaningful accessible name.
- Inputs with visible labels should expose that label through automation.
- Validation messages should be reachable or mirrored in accessible text.
- Decorative icons should not confuse screen readers.

## Priority 1 controls

These controls should be checked before a public release:

```text
XButton
XIconButton
XTextBox
XPasswordBox
XSecurePasswordBox
XNumberBox
XSearchBox
XComboBox
XLookupComboBox
XLookupTreeComboBox
XCheckBox
XRadioButton
XValidationHintPopup
XValidationSummary
XViewContainer
XDataGrid
XTreeView
XWindow
```

## Priority 2 controls

These controls should be checked after the priority 1 group:

```text
XBadge
XBorder
XButtonGroup
XCheckGroup
XDatePicker
XExpander
XListBox
XNavigationList
XNavigationTabControl
XPanel
XProgressBar
XSlider
XSplitView
XTabControl
XTextBlock
XTimePicker
XToggleButton
XToggleDropDown
XToggleSwitch
XToolbarPresenter
```

## Keyboard and focus checks

For each interactive control, check:

- Can the control be reached with `Tab`?
- Can the control be left with `Tab` or `Shift+Tab`?
- Is the focus visual clearly visible?
- Does `Enter` trigger the expected action where appropriate?
- Does `Space` trigger button-like or checkbox-like behavior where appropriate?
- Does `Escape` close popups or overlays where appropriate?
- Do arrow keys work for list, combo, tree, tab and grid controls?
- Does focus return to a sensible place after popup/dialog close?

## High contrast checks

Check with Windows high contrast mode enabled:

- Text remains readable.
- Borders remain visible.
- Focus indicator remains visible.
- Selection states remain visible.
- Disabled states remain understandable.
- Error, warning and information states are still distinguishable.
- Important icons are still visible or have text alternatives.

## Validation UI checks

Check `XValidationHintPopup` and `XValidationSummary`:

- Errors are visible near the affected control or in a summary.
- Warnings and information messages are distinguishable.
- Error color is not the only signal.
- Popup content can be dismissed predictably.
- Validation content is accessible through text, not only icon/color.
- Long validation messages wrap correctly.
- Multiple validation messages are readable.
- Async validation state does not trap focus.

## XDataGrid checks

Check:

- Keyboard navigation between cells and rows.
- Sorting can be triggered or understood by keyboard users.
- Filter buttons can receive focus.
- Filter popup can be opened and closed by keyboard.
- Filter popup has a logical tab order.
- Search input has a meaningful label/name.
- Action buttons have meaningful accessible names.
- Row selection feedback is visible.
- Current cell/focus state is visible.
- Empty state is understandable.
- Screen reader basics are acceptable.
- Large data does not freeze keyboard interaction.

## XTreeView checks

Check:

- Nodes can be expanded and collapsed by keyboard.
- Selection is visible.
- Drag/drop does not remove basic keyboard usability.
- Drop hints are visually clear.
- Drop hint text is understandable.
- Focus is not lost after expand/collapse.

## XWindow checks

Check:

- Window buttons have meaningful accessible names.
- Close, minimize, maximize and restore are keyboard reachable.
- Window drag areas do not block child control interaction.
- Focus starts in a sensible place.
- Focus remains inside modal windows where applicable.
- High contrast title bar is usable.

## Result template

Use this template for each control:

```text
Control:
Date:
Reviewer:
Status:

Scenarios checked:
- Default:
- Disabled:
- Focused:
- Hover:
- Pressed/Selected:
- Validation:
- High contrast:
- Keyboard:
- Screen reader basics:

Findings:
-

Decision:
-

Follow-up:
-
```

## Audit table

| Control | Keyboard | Focus | Automation | High contrast | Validation | Status | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| XButton |  |  |  |  | n/a | Not started |  |
| XIconButton |  |  |  |  | n/a | Not started |  |
| XTextBox |  |  |  |  |  | Not started |  |
| XPasswordBox |  |  |  |  |  | Not started |  |
| XSecurePasswordBox |  |  |  |  |  | Not started |  |
| XNumberBox |  |  |  |  |  | Not started |  |
| XSearchBox |  |  |  |  |  | Not started |  |
| XComboBox |  |  |  |  |  | Not started |  |
| XLookupComboBox |  |  |  |  |  | Not started |  |
| XLookupTreeComboBox |  |  |  |  |  | Not started |  |
| XCheckBox |  |  |  |  | n/a | Not started |  |
| XRadioButton |  |  |  |  | n/a | Not started |  |
| XValidationHintPopup |  |  |  |  |  | Not started |  |
| XValidationSummary |  |  |  |  |  | Not started |  |
| XViewContainer |  |  |  |  | n/a | Not started |  |
| XDataGrid |  |  |  |  |  | Not started |  |
| XTreeView |  |  |  |  | n/a | Not started |  |
| XWindow |  |  |  |  | n/a | Not started |  |