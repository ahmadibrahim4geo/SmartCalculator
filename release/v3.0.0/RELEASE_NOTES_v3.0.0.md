# SmartCalculator v3.0.0

Release date: 2026-11-01

## Highlights

- **Startup Crash Resolution**: Resolved a startup crash (`System.InvalidOperationException` in `OvalButtonTemplate` due to type mismatch in `BorderThickness` and `StrokeThickness`) ensuring immediate, smooth launch across both Setup installer and Portable ZIP builds.
- **Enlarged Developer Signature Logo (>300%)**: The authentic Arabic calligraphy signature ("أحمد إبراهيم" from `D:\Logo.svg`) is now prominently enlarged by over 300% (from 24px height to 76px height in full mode, 52px in compact mode) with full vector fidelity, transparent background, physical vector cutout grooves (`CombinedGeometry`), metallic platinum finish (`#E2E8F0`), hover brightening, and interactive click to open the About dialog.
- **Persistent Developer Subtitle in Compact Mode**: "Scientific Display • by: Ahmad Ibrahim" (`BrandSubtitle`) remains clearly visible below the application title in both Full mode and Compact mode, ensuring consistent branding.
- **Two-Row Balanced Header**: Cleanly separates window controls (Pin, Compact, Close) and accessory tools (Theme, Skin, About, Help) into a balanced two-stack layout flanking the prominent central calligraphy signature.
- **Complete Compact Mode**: A streamlined mini-calculator featuring a uniform 4×6 keypad with quick functions (`(`, `)`, `√`, `%`), `S⇔D` fraction toggle, clear/delete keys, and full arithmetic. Toggle anytime with **Ctrl+M**, the header icon `⤡`/`⤢`, or `MODE → ▶ → 6:COMPACT`.
- **20 Independent Ergonomic Key & Navigator Skins**: Completely redesigned with authentic physical calculator profiles (classic scientific, modern ClassWiz soft, raised 3D, graphing pro, precision chamfer, mechanical, glass aero, metallic bevel, retro desktop, chiclet, and high-contrast). All secondary SHIFT/ALPHA labels maintain 5px safety margins with zero clipping.
- **130 Unique Visual Themes**, including 30 new premium palettes.
- **Persistent Preferences**: Theme, skin, compact mode, and always-on-top state are stored in `%LocalAppData%\SmartCalculator\settings.json` and restored on startup. Automatic migration from legacy v2 preferences.
- **Classic Fixed LCD**: The calculation display keeps its familiar green scientific appearance across every theme and skin.
- **Corrected Scientific Keys**: `SHIFT+xʸ` inserts `root(value,index)`, `SHIFT+log` inserts `10^(`, and `SHIFT+ENG` correctly moves the cursor left.
- **Safer Calculations**: Negative cube roots return real values and invalid characters produce `Syntax Error` instead of being ignored.
- **Tactile Dpad Sync**: Navigation keys physically animate and depress in real-time with responsive tactile feedback.
- **Built-in Help Manual**: Access via the `❓` Help button or press **F1** to open the user guide.

## Packages

- `SmartCalculator_Setup_v3.0.0.exe` — Windows installer with shortcuts and help files.
- `SmartCalculator_Portable_v3.0.0.zip` — Extract and run; no installation required.
- `SHA256SUMS_v3.0.0.txt` — SHA-256 integrity hashes for both packages.

Both packages are self-contained Windows x64 builds and do not require a separate .NET runtime.

### Checksums (SHA-256)
- Setup: `1b9221170e9e76a3f7189acfe75e220993cbbc20f4a0b25065439355ebeaf974`
- Portable: `00eb92c0893ad7aecddc936f8f9864a05411289c4a7e0cf4c4ddd2c9af4c851e`

## Verification

- Debug build: 0 errors, 0 warnings.
- Release build and publish: Verified during packaging.
- Portable execution: Extracted from ZIP and verified running smoothly without crashes.
- Automated regression, theme, skin, settings, and key-map checks: 822 passed, 0 failed.
- UI validation: Balanced header with enlarged logo (>300%), persistent subtitle in compact mode, compact mode toggle (Ctrl+M / button / MODE), zero label clipping across all 20 skins, and persistent settings.
