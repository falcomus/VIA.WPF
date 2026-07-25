# VIA.WPF Theme Color Contract

## Purpose

VIA.WPF owns the colors used by application chrome, controls, interaction states, and reusable workbench infrastructure. Applications consume semantic `XBrushKeys`; they do not define parallel theme palettes or select colors by hexadecimal value.

The contract follows the Fluent 2 separation of neutral, brand, shared, and semantic colors. WCAG 2.2 contrast requirements are the minimum acceptance criteria:

- normal text: 4.5:1;
- large text and meaningful UI indicators: 3:1;
- disabled and purely decorative content is exempt, but must remain visibly intentional.

## Canonical roles

### Surfaces

| Key | Intended use |
| --- | --- |
| `Canvas` | Application or workbench background |
| `Surface` | Standard panels, cards, and content regions |
| `SurfaceRaised` | Visually raised persistent content |
| `SurfaceOverlay` | Menus, flyouts, dialogs, and temporary overlays |
| `SurfaceSunken` | Wells, tracks, recessed regions, and read-only groups |
| `Scrim` | Modal overlay behind temporary content |

### Text and borders

| Key | Intended use |
| --- | --- |
| `TextPrimary` | Main content and labels |
| `TextSecondary` | Supporting content and metadata |
| `TextTertiary` | Placeholders and low-emphasis content |
| `TextDisabled` | Disabled text and icons |
| `BorderSubtle` | Card and panel separation |
| `BorderDefault` | Standard control boundary |
| `BorderStrong` | Emphasized control boundary |
| `Divider` | Separators between adjacent regions |
| `FocusRing` | Outer keyboard focus indicator |
| `FocusRingInner` | Contrasting inner focus indicator |

### Primary and accent

`Primary` identifies the main action and selection family. `Accent` is a complementary highlight family and must not duplicate `Primary` or a status color.

New code uses `PrimaryForeground`, `PrimarySubtle`, `PrimarySubtleHover`, and `PrimaryStrong` instead of mode-ambiguous names such as `PrimaryText`, `PrimaryVeryLight`, `PrimaryLight`, and `PrimaryDark`. Equivalent canonical names exist for `Accent`.

### Status

New code uses:

- `StatusSuccess`, `StatusSuccessForeground`, `StatusSuccessSubtle`;
- `StatusWarning`, `StatusWarningForeground`, `StatusWarningSubtle`;
- `StatusDanger`, `StatusDangerForeground`, `StatusDangerSubtle`;
- `StatusInfo`, `StatusInfoForeground`, `StatusInfoSubtle`.

Status colors communicate meaning and are not decorative accents. Teal and Emerald use a blue information ramp so that `StatusInfo` stays distinct from their primary color.

## Built-in preset seeds

The seed pairs use established stepped color ramps: a darker step for Light Mode and a lighter step for Dark Mode. Neutral backgrounds are deliberately quieter than the brand family.

| Theme | Primary Light / Dark | Accent Light / Dark | Info Light / Dark |
| --- | --- | --- | --- |
| Default | `#355C91` / `#70A2E0` | `#6D5B9E` / `#B7A7E5` | `#007C83` / `#5DD9DF` |
| Amber | `#B45309` / `#FBBF24` | `#4338CA` / `#A5B4FC` | `#007C83` / `#5DD9DF` |
| Azure | `#0369A1` / `#7DD3FC` | `#6D28D9` / `#C4B5FD` | `#007C83` / `#5DD9DF` |
| Crimson | `#BE123C` / `#FB7185` | `#B45309` / `#FBBF24` | `#007C83` / `#5DD9DF` |
| Emerald | `#047857` / `#6EE7B7` | `#4F46E5` / `#A5B4FC` | `#0369A1` / `#7DD3FC` |
| Graphite | `#334155` / `#CBD5E1` | `#0369A1` / `#7DD3FC` | `#007C83` / `#5DD9DF` |
| Indigo | `#4338CA` / `#A5B4FC` | `#B45309` / `#FBBF24` | `#007C83` / `#5DD9DF` |
| Magenta | `#A21CAF` / `#F0ABFC` | `#0369A1` / `#7DD3FC` | `#007C83` / `#5DD9DF` |
| Rose | `#BE123C` / `#FDA4AF` | `#4F46E5` / `#A5B4FC` | `#007C83` / `#5DD9DF` |
| Sandstone | `#78350F` / `#FDBA74` | `#4D7C0F` / `#BEF264` | `#007C83` / `#5DD9DF` |
| Teal | `#0F766E` / `#5EEAD4` | `#6D28D9` / `#C4B5FD` | `#0369A1` / `#7DD3FC` |
| Violet | `#6D28D9` / `#C4B5FD` | `#B45309` / `#FBBF24` | `#007C83` / `#5DD9DF` |

## Application integration

Application-specific aliases may temporarily exist during migration but must directly reference an `XBrushKeys` resource. They may not contain independent colors.

Domain colors are outside this contract when they are user-selected or persisted as document content. Examples include mockup control fills, chart series, and asset tints. Rendering colors that describe application chrome or designer state remain theme colors and must resolve through VIA.WPF.
