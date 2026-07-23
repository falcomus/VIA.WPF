# VIA.WPF Markdown file map

This ZIP contains the intended Markdown documentation layout for the VIA.WPF solution.

Copy the files into the VIA.WPF solution root so the paths stay exactly as shown here.

## Root documentation

```text
README.md
CHANGELOG.md
ROADMAP.md
VIA.WPF_REVIEW_NOTES.md
README_FILE_MAP.md
```

## Package READMEs

These files belong directly next to their project files and are the intended NuGet package READMEs.

```text
VIA.WPF/README.md
VIA.WPF.Controls/README.md
VIA.WPF.MVVM/README.md
VIA.WPF.Themes/README.md
VIA.WPF.Icons/README.md
VIA.WPF.Windowing/README.md
```

## Support documentation

```text
VIA.WPF/README_INSTALLATION.md
VIA.WPF.Demo/README.md
VIA.WPF.Tests/README.md
docs/ACCESSIBILITY_CHECKLIST.md
docs/MINIMAL_DEMO.md
docs/PERFORMANCE_BASELINE.md
docs/RELEASE_POLICY.md
```

## What should not exist

Delete misplaced Markdown files such as:

```text
VIA.WPF.Controls/Properties/README.md
```

A package README belongs in the project folder, not in `Properties`.

## Safe cleanup workflow

```text
1. Commit or stash the current green state.
2. Delete all existing *.md files inside the VIA.WPF solution.
3. Extract this ZIP into the VIA.WPF solution root.
4. Check git diff.
5. Rebuild all projects.
6. Run all tests.
7. Pack the NuGet packages once to verify README inclusion.
```

Do not delete or replace Markdown files from unrelated solutions such as `iplanLager`.
