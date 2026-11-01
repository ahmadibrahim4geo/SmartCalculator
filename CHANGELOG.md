# Changelog

## v3.0.0 (2026-11-01)

### Added
- 30 new hand-tuned themes for a total of 130 unique active themes.
- Dynamic theme catalog with stable IDs and automatic validation.
- Persistent preferences in `%LocalAppData%\SmartCalculator\settings.json` (theme, skin, compact mode, and always-on-top state).
- 20 independent ergonomic button skins covering classic scientific, modern soft ClassWiz, raised 3D, graphing pro, precision chamfer, mechanical, glass aero, metallic bevel, retro desktop, chiclet, and high-contrast designs.
- Complete Compact Mode: high-efficiency 4x6 uniform scientific keypad featuring `(`, `)`, `√`, `%`, `AC`, `DEL`, `S⇔D`, `÷`, numbers 0-9, basic operators, `Ans`, and `=`.
- Global shortcut `Ctrl+M` for instant toggling between Full Scientific mode and Compact mode.
- Mode Setup integration with `MODE → second page → 6:COMPACT` alongside `5:SKIN` and `4:THEME`.
- Two-row balanced header layout cleanly distributing application branding, developer credits, centered developer vector calligraphy signature ("أحمد إبراهيم"), window actions (Pin, Compact, Close), and tool actions (Theme, Skin, About, Help).
- Authentic Arabic calligraphy logo integrated with transparent background, vector cutouts, and interactive About trigger.
- Skin selection from the header button or `MODE → second page → 5:SKIN`, with automatic startup restoration.
- Automatic migration from the v2 `theme_setting.txt` preference.
- Working nth-root key (`SHIFT+xʸ`) through `root(value,index)`.
- Reproducible release script for the self-contained portable ZIP, installer, and SHA-256 checksums.

### Fixed
- Fixed fatal startup crash (`System.InvalidOperationException` in `OvalButtonTemplate` caused by type mismatch in `BorderThickness` and `StrokeThickness`) that prevented v3.0.0 from launching.
- Installed copies can now save preferences without writing to `Program Files`.
- Theme count is no longer hard-coded to 100.
- Theme choice is saved only after a resource dictionary loads successfully.
- `SHIFT+log`, `SHIFT+ENG`, and `SHIFT+S⇔D` mappings now match their printed labels.
- Negative cube roots now return the correct real result.
- Invalid expression characters now report `Syntax Error` instead of being ignored.
- Polar/rectangular helpers preserve their second component in variable `Y`.
- File, product, assembly, and installer versions now agree.
- Theme changes no longer alter the classic LCD palette.
- Secondary SHIFT/ALPHA labels now keep a readable high-contrast treatment with 5px margins ensuring zero label clipping across all 20 skins.
- Eliminated geometric distortion and text overflow on all button shapes and D-pad navigation components.

### Changed
- Removed 50 unused legacy theme files from the build.
- Published packages are self-contained win-x64 distributions.
- Header icons updated to clear, expressive, and uncorrupted glyphs with dedicated hover treatments.
- The edit caret and skin-aware navigation control were visually refined.
- Deferred INEQ, VERIFY, and DIST entries now state clearly that they are unavailable.

## v2.0.0 (2026-07-08)

### Added
- 100 visual themes with click-to-cycle and hold-to-select behavior.
- D-pad keyboard animation, built-in help, and expanded tooltips.

## v1.0.0 (2026-06-24)

### Initial stable release
