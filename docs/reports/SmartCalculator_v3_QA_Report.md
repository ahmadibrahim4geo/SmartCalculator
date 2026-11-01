# SmartCalculator v3.0.0 — Final QA Report

Date: 2026-11-01

## Final status

| Check | Result |
|---|---|
| Debug build | Passed — 0 errors, 0 compiler warnings |
| Release build | Passed — 0 errors, 0 compiler warnings |
| Self-contained win-x64 publish | Passed |
| Automated checks | 822 passed, 0 failed |
| Startup runtime verification | Passed (no unhandled exceptions) |
| Active themes | 130 |
| Active skins | 20 |
| Duplicate active IDs, names, paths, or core palettes | 0 |
| Missing or invalid theme XAML files | 0 |
| Orphan theme files | 0 |
| Portable ZIP | Built and verified (runs cleanly) |
| Inno Setup installer | Compiled successfully |
| SHA-256 checksums | Recomputed and verified |

The restricted build environment emitted `NU1900` because the NuGet vulnerability-audit endpoint was unreachable; this is an external network warning, not a compiler or application warning.

## Startup Crash Fix Verification

- **Root Cause**: `OvalButtonTemplate` in `Styles/Skins/SkinBase.xaml` attempted to bind `Border.BorderThickness` to `SkinKeyStrokeWidth` (which evaluated to a double `1.0`), throwing `System.InvalidOperationException: '1' is not a valid value for property 'BorderThickness'` in WPF during initial layout measurement of `SHIFT`, `ALPHA`, `MODE`, and `ON` keys. Additionally, `Path.StrokeThickness` was bound to `SkinInnerBorderThickness` (a `Thickness`).
- **Remediation**: Corrected `BorderThickness` to `{DynamicResource SkinKeyBorderThickness}` and `StrokeThickness` to `{DynamicResource SkinInnerStrokeWidth}`.
- **Verification**: Both `bin\Debug\net8.0-windows\SmartCalculator.exe` and `artifacts\SmartCalculator_Portable_v3.0.0\SmartCalculator.exe` (extracted from the portable ZIP) were launched and verified running with healthy PIDs and zero exceptions in the Windows Event Log.

## Developer Calligraphy Signature ("أحمد إبراهيم")

- Prominently enlarged by over 300% (from 24px height to 76px height in Full mode, 52px in Compact mode) spanning the entire central header between the brand title and window controls.
- Vector paths converted to WPF `CombinedGeometry` with `GeometryCombineMode="Exclude"`:
  - Background is 100% transparent.
  - Calligraphy grooves/highlights are physical vector cutout holes that allow the underlying theme surface to shine through cleanly.
  - Styled in crisp metallic platinum (`#E2E8F0`) with 0.92 default opacity.
  - Hover trigger brightens fill to pure white (`#FFFFFF`) with hand cursor.
  - Click interaction triggers the `ABOUT` dialog.

## Compact mode & Persistent Subtitle Verification

- "Scientific Display • by: Ahmad Ibrahim" (`BrandSubtitle`) remains clearly visible below `SmartCalculator` title in both Full mode and Compact mode, resolving the previous issue where it collapsed during mode switch.
- Balanced header layout cleanly organizes window actions on top and accessory tools on bottom flanking the central calligraphy logo.
- Distinct, expressive glyphs for Pin (`⌖`), Compact (`⤡`/`⤢`), Close (`✕` with red hover), Theme (`🎨`), Skin (`🔲`), About (`ℹ`), and Help (`❓`).
- Complete uniform 4×6 keypad in compact mode featuring `(`, `)`, `√`, `%`, `AC`, `DEL`, `S⇔D`, `÷`, numbers, basic operators, `Ans`, and `=`.
- Global shortcut `Ctrl+M` instantly toggles Compact / Full mode.
- Automatic restoration of Compact mode and Always-on-Top state on next launch.

## Theme verification

- All 130 entries come from one `ThemeCatalog` and use stable string IDs.
- Every resource URI resolves to an existing XAML file.
- Every theme contains the same 61 required resource keys as the baseline theme.
- Every theme parses as valid XML.
- Core color signatures are unique across the active catalog.
- Fifty unused legacy theme files were removed from the build.
- Numeric selection supports three-digit entries through 130.

## Settings verification

- Preferences are stored under `%LocalAppData%\SmartCalculator\settings.json`.
- Preferences include: ThemeId, SkinId, IsCompactMode, and IsAlwaysOnTop.
- The app no longer writes user settings into the installation directory.
- The legacy v2 integer preference can be migrated from `theme_setting.txt`.
- Preferences are written atomically through a temporary file.
- Theme and skin IDs are saved only after their resources load successfully.
- Saving any appearance or window state preference preserves the other values.

## Skin and interface verification

- Twenty stable, uniquely named skin definitions have twenty unique visual signatures.
- Skins use physically authentic calculator profiles (classic scientific, modern ClassWiz soft, raised 3D, graphing pro, precision chamfer, mechanical, glass aero, metallic bevel, retro desktop, chiclet, and high-contrast).
- Secondary SHIFT and ALPHA labels maintain 5px safety margins with zero clipping across all 20 skins.
- The D-Pad navigator geometries (Pill, Ellipse, Round, Octagon, Bow, Hex, Cross, Diamond, Squircle, Chiclet) completely enclose the 60×42 NAV button without any corner spillage.
- The classic LCD colors are defined at window scope and remain unchanged while themes and skins are switched.
- `MODE → second page → 5:SKIN` opens the skin list; direct two-digit selection through 20 works.
- `MODE → second page → 6:COMPACT` toggles Compact mode.

## Key and calculation audit

- Every declared SHIFT and ALPHA action has a matching key-map entry.
- `SHIFT+xʸ` inserts `root(value,index)` and the operation evaluates correctly.
- `SHIFT+log` inserts `10^(`.
- `SHIFT+ENG` moves the cursor left.
- `SHIFT+S⇔D` clears SHIFT and performs the documented fraction toggle.
- `SHIFT+AC` closes the application.
- Negative cube roots use the real cube root.
- Unknown expression characters produce `Syntax Error` rather than being ignored.
- Polar and rectangular helpers keep their second result in variable `Y`.
- Deferred VERIFY, INEQ, and DIST entries state clearly that they are unavailable.

## Release artifacts

- `release/v3.0.0/SmartCalculator_Setup_v3.0.0.exe`
- `release/v3.0.0/SmartCalculator_Portable_v3.0.0.zip`
- `release/v3.0.0/SHA256SUMS_v3.0.0.txt`
- `release/v3.0.0/RELEASE_NOTES_v3.0.0.md`

Both packages use the self-contained Windows x64 publish and require no separate .NET runtime.

Verified SHA-256 values:
- Setup: `1b9221170e9e76a3f7189acfe75e220993cbbc20f4a0b25065439355ebeaf974`
- Portable: `00eb92c0893ad7aecddc936f8f9864a05411289c4a7e0cf4c4ddd2c9af4c851e`
