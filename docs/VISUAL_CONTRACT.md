# VIA.WPF Visual Contract

Status: Phase 1 baseline for the Modern Workbench brushup.

This contract is the source of truth for new VIA.WPF controls, styles, and
composition primitives. It intentionally favors a professional desktop density
over touch-first sizing and decorative card layouts.

## Design direction

- Use a Studio Dense base: compact, neutral, structured, and optimized for
  information-rich desktop applications.
- Use Balanced Fluent command treatment: quiet New, Refresh, Search, and More
  actions integrated into the owning header instead of floating toolbars.
- Use cobalt as a restrained interaction accent. Semantic colors communicate
  status only; they are not decorative fills.
- Use graphite neutrals in dark mode. Dark mode must not read as a navy color
  filter.
- Prefer borders, spacing, and typography for hierarchy. Shadows are reserved
  for transient or genuinely raised layers.

## Layer model

| Layer | Semantic resource | Purpose |
| --- | --- | --- |
| Canvas | `XBrushKeys.Canvas` | Application and page background |
| Surface | `XBrushKeys.Surface` | Standard content surface |
| Raised | `XBrushKeys.SurfaceRaised` | Popups, menus, dialogs, raised panes |
| Sunken | `XBrushKeys.SurfaceSunken` | Recessed work areas and secondary wells |

Do not nest containers only to create visual depth. A page normally has one
canvas, a small number of meaningful groups, and content that sits directly in
those groups.

## Semantic color roles

New code should prefer these roles over concrete palette variants:

| Role | Resource |
| --- | --- |
| Primary text | `XBrushKeys.TextPrimary` |
| Secondary text | `XBrushKeys.TextSecondary` |
| Tertiary/placeholder text | `XBrushKeys.TextTertiary` |
| Subtle separator | `XBrushKeys.BorderSubtle` |
| Standard control border | `XBrushKeys.BorderDefault` |
| Emphasized border | `XBrushKeys.BorderStrong` |
| Keyboard focus | `XBrushKeys.FocusRing` |
| Focus contrast inset | `XBrushKeys.FocusRingInner` |
| Hover | `XBrushKeys.StateHover` |
| Pressed | `XBrushKeys.StatePressed` |
| Selected | `XBrushKeys.StateSelected` |
| Strong selection/accent | `XBrushKeys.StateSelectedStrong` |
| Modal overlay | `XBrushKeys.Scrim` |

The Default preset uses neutral graphite surfaces and cobalt interaction color.
Other presets may change the accent, but they must preserve the same contrast
and layer relationships.

## Density and sizing

The standard desktop metrics are:

| Element | Metric |
| --- | ---: |
| Small control | 26 px |
| Standard control | 30 px |
| Large control | 32 px |
| Data row | 28 px |
| Data header | 30 px |
| Navigation item | 34 px |
| Command bar | 40 px |
| Command item | 32 px |
| Standard icon | 16 px |
| Page padding | 20 px |
| Group padding | 16 px |

Spacing uses the 2, 4, 8, 12, 16, 20, and 24 px scale. Repeated local values
outside this scale need a component-specific reason.

## Shape and elevation

- Interactive controls use 2 to 4 px corner radii.
- Containers use 4 to 6 px corner radii.
- Pill shapes are reserved for tags, status indicators, and intentionally
  capsule-shaped actions.
- Persistent page content has no drop shadow by default.
- Popups, menus, teaching tips, dialogs, and raised overlays may use restrained
  elevation.

## Typography

- Segoe UI is the application typeface; Consolas is used for code.
- Body text is 13 px with a 20 px line height.
- Standard hierarchy uses regular body text, medium labels, and semibold section
  or page titles.
- Avoid oversized dashboard headings and repeated product identity within a
  page.
- Secondary text must remain readable in both modes and must not substitute for
  disabled styling.

## Interaction states

Every interactive control must visibly distinguish:

1. Rest
2. Pointer over
3. Pressed
4. Keyboard focused
5. Selected or checked
6. Disabled
7. Validation error, where applicable

Focus is independent from hover and selection. Keyboard focus uses the focus
ring token and must not rely on color alone. Disabled state changes foreground,
background, and border together and remains legible.

## Composition rules

- `XPage` owns page spacing and the relationship between header and content.
- `XHeaderBar` owns title, subtitle, breadcrumbs, and contextual actions.
- `XHeaderGroup` groups related header actions without ribbon-like framing.
- `XGroup` is the standard titled content container with arbitrary actions,
  optional More menu, content, and footer.
- `XContentStatePresenter` owns Content, Loading, Empty, Error, Offline, and Retry
  presentation.
- Generic containers do not expose fixed CRUD properties or commands.
- Do not create parallel Panel, Card, Section, and Group concepts for the same
  visual role.

## Command treatment

- Common actions such as New and Refresh are quiet commands in the owning header.
- More uses a dedicated overflow button and menu.
- A command bar has no decorative background or border unless it separates
  distinct panes.
- Primary filled buttons are reserved for the page or dialog's main commitment.
- Destructive actions are outlined or quiet by default and become filled only
  during hover, confirmation, or when destructive intent must be unmistakable.

## Navigation and data

- Navigation selection uses a tinted background plus an accent edge; it does not
  use a large solid accent block.
- Data grids use visible but restrained grid lines, a distinct header layer, and
  28 px rows by default.
- Grid actions belong to the data group's header, not to a loose toolbar above
  the grid.
- Empty states sit naturally in the content region. Dashed placeholder cards are
  not part of the standard system.

## Combo box contract

- `MaxVisibleItems` defaults to 10 and the popup is clamped to the work area.
- Standard type-ahead remains available. Full filtering is opt-in through
  `IsSearchEnabled`.
- Clearing a selection is opt-in through `CanClearSelection`; its button appears
  on hover or focus.
- The popup is at least as wide as the control, may be wider for content, and may
  not extend beyond the active screen.
- Empty and no-results states are built in.
- Multiple selection is a separate `XMultiComboBox`, not a mode of `XComboBox`.

## Accessibility and verification

- Text and essential graphics target WCAG AA contrast.
- Selected, error, and focus states use more than color where necessary.
- Every control is checked with keyboard-only navigation and visible focus.
- Light and dark mode are reviewed independently at 100%, 125%, and 150% scale.
- VIA.WPF.Demo is the visual specimen and state matrix.
- ProjectPlanner is the primary composition reference.
- iPlan Lager is the compatibility consumer.

