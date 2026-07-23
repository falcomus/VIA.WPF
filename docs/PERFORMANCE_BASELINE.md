# VIA.WPF performance baseline

This document defines the planned performance baseline work.

## Priority area

XDataGrid is the first component that needs a measured baseline.

## Test sizes

```text
1,000 rows
10,000 rows
50,000 rows optional
```

## Scenarios

Measure:

- initial grid load
- auto-generated column metadata
- filter popup opening
- filter value generation
- applying one column filter
- clearing filters
- free text search
- sorting
- view-state save
- view-state restore

## Rules

- Measure Debug only for development hints.
- Measure Release for real baseline values.
- Use realistic DTO/view-model shapes.
- Include string, numeric, date and nullable values.
- Do not optimize based on guesswork alone.

## Current design expectation

XDataGrid keeps filtering and search on the WPF collection view. Expensive operations should stay batched, lazy or debounced.

For very large datasets, server-side filtering/searching remains the preferred future option.

## Result template

```text
Dataset:
Build:
Machine:
Scenario:
Rows:
Columns:
Time:
Memory:
Notes:
```
