# SmartCalculator v1.0.0

**Final stable release**

| | |
|---|---|
| **App** | SmartCalculator |
| **Version** | v1.0.0 |
| **Developer** | Ahmad Ibrahim |
| **License** | MIT License — Free & Open Source |
| **Copyright** | © 2026 Ahmad Ibrahim |

## Main Features

- **Scientific calculator core** — arithmetic, scientific functions, parentheses, operator precedence
- **SHIFT / ALPHA key system** — mapped secondary (gold) and tertiary (red) labels on every key
- **BASE-N mode** — DEC/HEX/BIN/OCT with bitwise AND, OR, XOR, NOT
- **STAT mode** — one-variable statistics (n, Σx, Σx², mean, sx, σx, min, max)
- **CMPLX mode** — complex arithmetic (+, -, ×, ÷, abs, conj)
- **MATRIX mode** — 2×2 / 3×3 matrix operations (add, sub, mul, det, transpose, inv)
- **VECTOR mode** — 2D / 3D vector operations (+, -, ×, dot, cross, mag, unit)
- **TABLE mode** — guided function table generation (f(X), start/end/step, max 100 rows)
- **EQN / SOLVE** — one-variable numeric equation solving via `solve(equation, guess)`
- Engineering notation, decimal ↔ fraction toggle, DMS conversion
- Numeric integration (∫), summation (Σ), derivative (d/dx)
- Constants (π, e), memory (Ans, PreAns, A–F, X, Y, M), STO/RCL, M+/M−
- Unit conversions (SHIFT+8) and scientific constants (SHIFT+7)

## Known Limitations

- ʸ√x (y-th root via SHIFT+xʸ) — deferred
- VERIFY mode (SHIFT+6), INEQ mode, DIST mode — deferred
- Regression analysis (STAT) — deferred
- Graphical plotting — deferred
- Symbolic algebra — deferred
- MATRIX: 2×2 / 3×3 only, no complex matrices
- VECTOR: angle() / polar-rectangular conversion deferred
- SOLVE: numeric only (no symbolic solving)
- Implicit multiplication (`2X`) not supported — use `2*X`

## Packages

### Framework-dependent (FrameworkDependent)

Requires .NET 8 Desktop Runtime installed on the user machine.
Includes full publish folder with runtime dependencies.

### Self-contained (SelfContained_win-x64)

No runtime required — portable folder with all dependencies for Windows x64.
**Recommended for end users.**
Multi-file (not single-file) — Assets\AppIcon.ico must remain beside the EXE.

## Checksums

SHA256 checksums provided in `SHA256SUMS_v1.0.0.txt`.
