# Copy instructions

Copy these files into the VIA.WPF solution root:

```text
THIRD_PARTY_NOTICES.md
README_THIRD_PARTY_SECTION.md
```

Then open the root `README.md` and insert the content of `README_THIRD_PARTY_SECTION.md` near the end, ideally before `## Documentation` or before the license section if one exists.

After that, either delete `README_THIRD_PARTY_SECTION.md` or keep it only temporarily while reviewing the README change.

Recommended git workflow:

```powershell
git add .\THIRD_PARTY_NOTICES.md .\README.md
git commit -m "Add third-party notices"
git push
```

If you also commit the temporary snippet file, use:

```powershell
git add .\THIRD_PARTY_NOTICES.md .\README_THIRD_PARTY_SECTION.md .\README.md
git commit -m "Add third-party notices"
git push
```

Before public release, verify the current dependency list:

```powershell
dotnet list .\VIA.WPF.slnx package --include-transitive
```
