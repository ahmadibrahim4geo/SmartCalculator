# SmartCalculator

A premium handheld-style scientific calculator for Windows Desktop.

Built with WPF and .NET 8 — offline, portable, calculator-only.

## Requirements

- Windows 7 or later
- No installation required (portable EXE)
- No internet connection needed
- .NET 8 Desktop Runtime required (for framework-dependent package)

## How to Build

```powershell
dotnet build
```

Build output: `bin\Debug\net8.0-windows\SmartCalculator.exe`

## How to Publish

### Framework-dependent (current default)

```powershell
dotnet publish -c Release
```

Published EXE: `bin\Release\net8.0-windows\publish\SmartCalculator.exe`

### Self-contained single-file (recommended for distribution)

```powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

Portable EXE: `bin\Release\net8.0-windows\win-x64\publish\SmartCalculator.exe`

## Features

### Modes
- **COMP** — Basic computation (arithmetic, scientific functions, parentheses)
- **CMPLX** — Complex arithmetic (+, -, ×, ÷, abs, conj)
- **STAT** — One-variable statistics (n, Σx, Σx², mean, sx, σx, min, max)
- **BASE-N** — Base conversion (DEC/HEX/BIN/OCT) with bitwise operations (AND, OR, XOR, NOT)
- **EQN** — Equation solving via `solve(equation, guess)` (numeric, Newton-Raphson + bisection)
- **MATRIX** — Matrix calculations (2×2, 3×3: add, sub, mul, det, transpose, inv)
- **TABLE** — Function table (f(X) with start/end/step, max 100 rows)
- **VECTOR** — Vector calculations (2D/3D: +, -, ×, dot, cross, mag, unit)

### Core Features
- Basic arithmetic (+, -, ×, ÷, %, ^)
- Scientific functions (sin, cos, tan, log, ln, √, ∛, x², xʸ, x⁻¹, x!)
- Angle modes (DEG/RAD/GRAD)
- Hyperbolic trig (sinh, cosh, tanh) via HYP toggle
- Constants (π, e) and memory (Ans, PreAns, A–F, X, Y, M)
- STO/RCL, M+/M− memory operations
- Parentheses and operator precedence
- Engineering notation, decimal ↔ fraction toggle (S⇔D)
- DMS conversion, Pol/Rec, Ran#, RanInt, GCD, LCM, Int, Intg
- Numeric integration (∫), summation (Σ), derivative (d/dx)
- nPr, nCr permutations and combinations
- Unit conversions (SHIFT+8) and scientific constants (SHIFT+7)
- Keyboard input support
- Dark handheld-style UI with LCD display
- Error-safe evaluation

## Limitations

### Deferred (Future)
- VERIFY mode, INEQ mode, DIST mode
- ʸ√x (y-th root) via SHIFT+xʸ
- Polar/rectangular conversion for vectors
- angle() function for vectors
- Two-function table (f(X) and g(X))
- Graphical plotting
- Regression analysis in STAT mode
- Frequency input support (value:frequency)
- Polynomial exact solving (quadratic formula)

### Known
- 4×4+ matrices not supported (MATRIX mode)
- Inverse for 3×3 uses adjugate method (precision limits for near-singular)
- Complex matrices not supported (CMPLX mode)
- SOLVE is numeric only (no symbolic algebra)
- Systems of equations not supported
- Must use `X` variable in SOLVE and TABLE functions
- Implicit multiplication (e.g., `2X`) not supported — use `2*X`
- BASE-N is integer only, no fractions or decimals
- Negative results in BIN/OCT/HEX show unsigned absolute value with leading `-`

## Notes

- Calculator-only application. No data, no settings, no internet.
- No external trademarks, logos, or branded assets are used.
- App icon is a custom design — not derived from any calculator brand.
- Licensed under MIT License — Free & Open Source.
- Copyright © 2026 Ahmad Ibrahim.

## Output Paths

| Configuration | Path |
|---------------|------|
| Debug Build | `bin\Debug\net8.0-windows\SmartCalculator.exe` |
| Release Build | `bin\Release\net8.0-windows\SmartCalculator.exe` |
| Published (framework-dependent) | `bin\Release\net8.0-windows\publish\SmartCalculator.exe` |
| Published (self-contained) | `bin\Release\net8.0-windows\win-x64\publish\SmartCalculator.exe` |
