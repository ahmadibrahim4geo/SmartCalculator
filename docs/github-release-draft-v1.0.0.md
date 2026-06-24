# GitHub Release Draft — SmartCalculator v1.0.0

## Tag
v1.0.0

## Title
SmartCalculator v1.0.0

## Status
Final stable release

## Developer
Ahmad Ibrahim

## License
MIT License — Free & Open Source

## Overview

SmartCalculator is a premium handheld-style scientific calculator for Windows Desktop. Built with WPF and .NET 8 — offline, portable, calculator-only.

## Features

- **Scientific calculator core** — arithmetic, scientific functions (sin, cos, tan, log, ln, √, x², xʸ, x⁻¹, x!), parentheses, operator precedence
- **SHIFT / ALPHA key system** — two-level key mapping with gold secondary and red tertiary labels
- **BASE-N mode** — Decimal/Hex/Binary/Octal with bitwise AND, OR, XOR, NOT
- **STAT mode** — one-variable statistics (n, Σx, Σx², mean, sx, σx, min, max)
- **CMPLX mode** — complex arithmetic (+, -, ×, ÷, abs, conj)
- **MATRIX mode** — 2×2 / 3×3 matrix operations (add, sub, mul, det, transpose, inv)
- **VECTOR mode** — 2D / 3D vector operations (+, -, ×, dot, cross, mag, unit)
- **TABLE mode** — guided function table generation (f(X), start/end/step, max 100 rows)
- **EQN / SOLVE** — one-variable numeric equation solving via `solve(equation, guess)`
- Engineering notation, decimal ↔ fraction toggle, DMS conversion, integration, summation, derivative, constants, unit conversions, memory, STO/RCL, M+/M−

## Download Options

| Asset | Description |
|-------|-------------|
| `SmartCalculator_v1.0.0_SelfContained_win-x64.zip` | Portable package — no installation required, no runtime needed. Extract and run. **Recommended for most users.** |
| `SmartCalculator_v1.0.0_FrameworkDependent.zip` | Smaller package — requires .NET 8 Desktop Runtime to be installed. |
| `SmartCalculator_Setup_v1.0.0.exe` | Windows installer — installs to Program Files, adds Start Menu shortcut, clean uninstall. |
| `SHA256SUMS_v1.0.0.txt` | SHA256 checksums for all assets. |

## Installation Notes

### Setup installer (recommended for beginners)
Run `SmartCalculator_Setup_v1.0.0.exe`. Follow the installer wizard. The app will appear in Start Menu and can be uninstalled via Windows Apps & Features.

### Portable (for advanced users or USB)
Download `SmartCalculator_v1.0.0_SelfContained_win-x64.zip`, extract to any folder, run `SmartCalculator.exe`. No installation or runtime required.

### Framework-dependent (if you already have .NET 8)
Download `SmartCalculator_v1.0.0_FrameworkDependent.zip`, extract, run `SmartCalculator.exe`. Requires .NET 8 Desktop Runtime installed separately.

## SHA256 Verification

```powershell
Get-FileHash -Algorithm SHA256 -Path "path\to\file.zip"
```

Compare the hash against `SHA256SUMS_v1.0.0.txt`.

## Known Limitations

- ʸ√x, VERIFY mode, INEQ mode, DIST mode — deferred to future release
- Advanced regression analysis (STAT) — deferred
- Graphical plotting — deferred
- Symbolic algebra — deferred
- MATRIX: 2×2 / 3×3 only
- SOLVE: numeric only

## License

MIT License — Free & Open Source. See `LICENSE` file for details.
