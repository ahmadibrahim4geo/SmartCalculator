# Phase 1.5 — Verification & Regression Test Report

## Build Status
| Configuration | Result |
|--------------|--------|
| Debug        | 0 errors, 0 warnings |
| Release      | 0 errors, 0 warnings |
| Publish (win-x64) | Succeeded |

## Automated Test Results — 75/75 PASS (0 FAIL)

### Key Mapping (39 tests)
- AllKeys: 47 entries (3 mode, 6 digits, 4 ops, AC/DEL/ON/SHIFT/ALPHA/MODE/ABOUT, 6 trig/hyperbolic, 5 memory/special, 12 scientific/math, 5 secondary)
- ShiftMap: 37 entries — all function-to-shift mappings correct
- AlphaMap: 24 entries — all ALPHA letter/variable mappings correct
- Every critical key present in AllKeys: AC, =, +, −, ×, ÷, 0–9, sin/cos/tan, log/ln, SHIFT, ALPHA, RCL, M+, Ans, DEL, ENG, INVERSE, LOGAB, FRAC, √, x², xʸ, (, ), ., ×10^, DMS, HYP, (-), SD, CALC, INTEGRAL
- ShiftMap entries verified: INVERSE→fact, LOGAB→Σ, INTEGRAL→DERIV, FRAC→SD

### Calculator Engine (18 tests)
| Expression | Expected | Result |
|-----------|----------|--------|
| 2+3 | 5 | PASS |
| 10-4 | 6 | PASS |
| 3×5 | 15 | PASS |
| 15÷3 | 5 | PASS |
| 2+3×4 | 14 (precedence) | PASS |
| (2+3)×4 | 20 | PASS |
| 2^10 | 1024 | PASS |
| 0.5+0.25 | 0.75 | PASS |
| π | 3.14159… | PASS |
| e | 2.71828… | PASS |
| √9 | 3 | PASS |
| ∛8 | 2 | PASS |
| sin(90) | 1 | PASS |
| cos(0) | 1 | PASS |
| log(1000) | 3 | PASS |
| ln(e) | 1 | PASS |
| 5! | 120 | PASS |
| abs(-5) | 5 | PASS |

### Error Handling (4 tests)
- Division by zero: Error ✓
- sqrt of negative: Error ✓
- log of zero: Error ✓
- Empty expression: Error ✓

### Scientific Functions (3 tests)
- Factorial(5) = 120 ✓
- Factorial(0) = 1 ✓
- RanInt(1,6) in range ✓

### Memory Service (4 tests)
- Store X=42 ✓
- Overwrite X=100 ✓
- M+ adds ✓
- M- subtracts ✓

### Fraction Value (3 tests)
- 0.75 → 3/4 ✓
- 0.5 → 1/2 ✓
- ToDouble round-trip ✓

### Angle Service (3 tests)
- DEG ToRad(90) = π/2 ✓
- RAD identity ✓
- GRAD ToRad(100) = π/2 ✓

## Phase 1 Feature Coverage

### Keys Wired (25 previously silent)
| Key | Label | Action | Status |
|-----|-------|--------|--------|
| INVERSE | x⁻¹ | 1/result | Implemented |
| LOGAB | log□ | insert "log(" | Partial |
| FRAC | □/□ | toggle fraction | Partial |
| √ shift | ∛ | cube root function | Implemented |
| INTEGRAL shift | d/dx | numeric derivative | Partial |
| LOGAB shift | Σ | summation | Partial |
| INVERSE shift | x! | factorial | Implemented |
| ALPHA . | RanInt | random integer | Implemented |
| ALPHA + | Int | truncation | Implemented |
| ALPHA − | Intg | floor function | Implemented |
| ALPHA √ | i | imaginary unit | Implemented |
| ALPHA FRAC | d/c | display toggle | Partial |

### SHIFT/ALPHA Fixes
- SHIFT + key → mapped action from ShiftMap ✓
- ALPHA + key → variable letter (A-F, X, Y, M) ✓
- ALPHA turns off after every key press ✓
- SHIFT turns off after one mapped action ✓
- S/A indicator LCD overlay via BooleanToOpacityConverter ✓

### STO/RCL Fix
- STO evaluates current expression, activates ALPHA, stores to next letter ✓
- RCL reads from variable ✓

### Keyboard Shortcuts
- `=` key → "=" (enters expression) ✓
- `A`–`F`, `X`, `Y`, `M` → ALPHA variable letters when ALPHA active ✓

## Published Binary
`R:\My Software\SmartCalculator\publish\SmartCalculator.exe` (162 MB, Release single-file)

## Notes
- `sqrt` ASCII keyword is not recognized — use Unicode `√` instead (✓ is recognized)
- Variable letters A/B/C/D/E/F/X/Y/M are accessed via ALPHA+key combos, not as separate keys
- STO and M- are shift/alpha actions, not separate key entries
- Deferred Phase 2 features (STAT, CMPLX, MATRIX, VECTOR, VERIFY, BASE, DEC/HEX/BIN/OCT, SOLVE, ʸ√x) show "N/A" message
