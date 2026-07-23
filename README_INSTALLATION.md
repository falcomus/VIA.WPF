# VIA.WPF NuGet README setup

Each NuGet package README belongs directly inside the matching project folder.

## Expected package README locations

```text
VIA.WPF/README.md
VIA.WPF.Controls/README.md
VIA.WPF.MVVM/README.md
VIA.WPF.Themes/README.md
VIA.WPF.Icons/README.md
VIA.WPF.Windowing/README.md
```

## Do not place package READMEs here

```text
VIA.WPF.Controls/Properties/README.md
VIA.WPF.MVVM/Properties/README.md
VIA.WPF.Themes/Properties/README.md
VIA.WPF.Icons/Properties/README.md
VIA.WPF.Windowing/Properties/README.md
```

The `.csproj` files expect `README.md` next to the project file.

## Metadata checklist before public release

Check every package project:

```text
PackageId
Version
Authors
Description
PackageTags
RepositoryUrl
PackageProjectUrl
PackageLicenseExpression
PackageReadmeFile
```

Do not write placeholder values for license, repository URL or project URL. Add them only when the public release decision is final.

## Recommended verification

```text
1. Rebuild solution.
2. Run tests.
3. Pack NuGet packages.
4. Inspect generated packages.
5. Confirm each package contains the correct README.md.
```
