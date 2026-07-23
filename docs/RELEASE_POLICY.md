# VIA.WPF release policy

This document defines the intended release discipline for VIA.WPF.

## Versioning

VIA.WPF should follow semantic versioning once publicly released.

```text
MAJOR.MINOR.PATCH
```

## Patch release

Use for:

- bug fixes
- documentation fixes
- small internal changes
- non-breaking analyzer cleanup

## Minor release

Use for:

- new controls
- new optional APIs
- new theme presets
- new validation helpers
- non-breaking improvements

## Major release

Use for:

- breaking API changes
- removed public members
- changed behavior that can break consumers
- package restructuring that affects references

## Breaking changes

Before removing public APIs:

1. Mark obsolete where possible.
2. Document the replacement.
3. Keep a transition period.
4. Remove only in a major version.

## Public release checklist

Before publishing broadly:

```text
- Rebuild all
- Run all tests
- Run demo smoke test
- Pack all packages
- Inspect package contents
- Verify README files
- Verify license metadata
- Verify repository URL
- Verify project URL
- Verify package tags
- Update CHANGELOG.md
- Update ROADMAP.md if needed
```

## Metadata

Do not publish packages with placeholder metadata.

Set these only when final:

```text
PackageLicenseExpression
RepositoryUrl
PackageProjectUrl
PackageTags
```

## Support statement

Until the public release decision is final, VIA.WPF should be treated as a controlled/internal library.
