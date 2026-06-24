# Changelog

## v1.0.0 (2026-06-24)

### Initial stable release

### Features
- **Core**: Arithmetic, scientific functions, parentheses, operator precedence, error-safe evaluation
- **SHIFT/ALPHA**: Two-level key mapping system (gold secondary + red tertiary labels)
- **BASE-N**: Decimal/Hex/Binary/Octal with bitwise AND, OR, XOR, NOT
- **STAT**: One-variable statistics (n, Σx, Σx², mean, sx, σx, min, max)
- **CMPLX**: Complex arithmetic (+, -, ×, ÷, abs, conj)
- **MATRIX**: 2×2 / 3×3 matrix operations (add, sub, mul, det, transpose, inv)
- **VECTOR**: 2D / 3D vector operations (+, -, ×, dot, cross, mag, unit)
- **TABLE**: Guided function table generation (f(X), start/end/step, max 100 rows)
- **EQN/SOLVE**: One-variable numeric equation solving via `solve(equation, guess)`
- **Additional**: Engineering notation, DMS conversion, fraction toggle, integration, summation, derivative, constants, unit conversions, memory (Ans/PreAns/A–F/X/Y/M), STO/RCL, M+/M−

### Packaging
- Framework-dependent (requires .NET 8 Desktop Runtime)
- Self-contained portable (recommended, no runtime required)

### Test Coverage
- 174 regression tests covering all modes

### Known Limitations
- ʸ√x, VERIFY, INEQ, DIST — deferred
- Advanced regression, graphing, symbolic algebra — deferred
- See RELEASE_NOTES_v1.0.0.md for full details.
