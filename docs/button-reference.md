# SmartCalculator — Button Reference

## Oval Buttons (Top Row)

| Button | Action | Status |
|--------|--------|--------|
| **SHIFT** | Toggles shift mode (gold labels). Affects next key press, then turns off. | Working |
| **ALPHA** | Toggles alpha mode (red labels). Affects next key press, then turns off. | Working |
| **NAV ▲** | Show direction, navigate history up / menu up | Working |
| **NAV ▼** | Show direction, navigate history down / menu down | Working |
| **NAV ◀** | Move cursor left in expression | Working |
| **NAV ▶** | Move cursor right in expression | Working |
| **MODE** | Opens/closes SETUP menu inside LCD. Use number keys or ◀▶ to navigate pages. | Working |
| **ON** | Clears all (same as AC). | Working |

## Command Strip (4 keys)

| Key | Shift | Alpha | Function | Status |
|-----|-------|-------|----------|--------|
| **CALC** | SOLVE | — | Evaluate expression (=) / SHIFT inserts `solve(` | Working |
| **∫** (integral) | **d/dx** | • | Insert `∫(f(X),a,b)` / `d/dx(f(X),a)` (shift) | Working (numeric integral/derivative) |
| **x⁻¹** | **x!** | — | Compute 1/result / Insert `!` (shift) | Working |
| **log□** | **Σ** | ▥ | Insert `log(` / `Σ(f(X),start,end)` (shift) | Working |

## Scientific Grid Row 1

| Key | Shift | Alpha | Function | Status |
|-----|-------|-------|----------|--------|
| **□/□** (fraction) | a b/c | d/c | Toggle fraction display (same as S⇔D) | Partial (fraction entry deferred) |
| **√** | **∛** | i | Insert `√(` / `∛(` (shift) | Working |
| **x²** | x³ | DEC | Insert `^2` / `^3` (shift) / Switch base to DEC (alpha, in BASE-N mode) | Working |
| **xʸ** | ʸ√x | HEX | Insert ` ^ ` / Switch base to HEX (alpha, in BASE-N mode) | Working (ʸ√x deferred) |
| **log** | 10ˣ | BIN | Insert `log(` / `×10^` (shift) / Switch base to BIN (alpha, in BASE-N mode) | Working |
| **ln** | eˣ | OCT | Insert `ln(` / `exp(` (shift) / Switch base to OCT (alpha, in BASE-N mode) | Working |

## Scientific Grid Row 2

| Key | Shift | Alpha | Function | Status |
|-----|-------|-------|----------|--------|
| **−** (unary) | ← | A | Insert `-` / cursor left (shift) / variable A (alpha) | Working |
| **DMS** | FACT | B | Convert to DMS / insert `!` (shift) / variable B (alpha) | Working |
| **hyp** | Abs | C | Toggle HYP mode / insert `abs(` (shift) / variable C (alpha) | Working |
| **sin** | sin⁻¹ | D | Insert `sin(` / `asin(` (shift) / variable D (alpha) | Working |
| **cos** | cos⁻¹ | E | Insert `cos(` / `acos(` (shift) / variable E (alpha) | Working |
| **tan** | tan⁻¹ | F | Insert `tan(` / `atan(` (shift) / variable F (alpha) | Working |

## Scientific Grid Row 3

| Key | Shift | Alpha | Function | Status |
|-----|-------|-------|----------|--------|
| **RCL** | **STO** | — | Insert `Ans` (recall) / Store value then ALPHA+letter (shift) | Working |
| **ENG** | ← | i | Toggle engineering notation / cursor left (shift) / imaginary unit i (alpha) | Working |
| **(** | % | X | Insert `(` / ` % ` (shift) / variable X (alpha) | Working |
| **)** | , | Y | Insert `)` / `,` (shift) / variable Y (alpha) | Working |
| **S⇔D** | a b/c | d/c | Cycle decimal ↔ fraction display | Working |
| **M+** | M− | M | Add to memory M / subtract from M (shift) / variable M (alpha) | Working |

## Numeric Grid Row 1

| Key | Shift | Alpha | Function | Status |
|-----|-------|-------|----------|--------|
| **7** | **CONST** | — | Digit 7 / constants menu (shift) | Working |
| **8** | **CONV** | — | Digit 8 / conversions menu (shift) | Working |
| **9** | **CLR** | — | Digit 9 / clear all (shift) | Working |
| **DEL** | **INS** | — | Delete character before cursor / toggle insert/overwrite (shift) | Working |
| **AC** | **OFF** | — | Clear all / off (shift, same as AC) | Working |

## Numeric Grid Row 2

| Key | Shift | Alpha | Function | Status |
|-----|-------|-------|----------|--------|
| **4** | MATRIX | — | Digit 4 / Activate MATRIX mode (shift) | Working |
| **5** | VECTOR | — | Digit 5 / Activate VECTOR mode (shift) | Working |
| **6** | VERIFY | — | Digit 6 | Working (VERIFY → info, Phase 2) |
| **×** | nPr | GCD | Multiply / ` nPr ` (shift) / ` gcd(` (alpha) | Working |
| **÷** | nCr | LCM | Divide / ` nCr ` (shift) / ` lcm(` (alpha) | Working |

## Numeric Grid Row 3

| Key | Shift | Alpha | Function | Status |
|-----|-------|-------|----------|--------|
| **1** | STAT | — | Digit 1 / Activate STAT mode (shift) | Working |
| **2** | CMPLX | — | Digit 2 / Activate CMPLX mode (shift) | Working |
| **3** | BASE | — | Digit 3 / Activate BASE-N mode (shift) | Working |
| **+** | Pol | Int | Insert ` + ` / `Pol(` (shift) / `Int(` (alpha) | Working |
| **−** | Rec | Intg | Insert ` - ` / `Rec(` (shift) / `Intg(` (alpha) | Working |

## Numeric Grid Row 4

| Key | Shift | Alpha | Function | Status |
|-----|-------|-------|----------|--------|
| **0** | **Rnd** | — | Digit 0 / round result (shift) | Working |
| **.** | **Ran#** | **RanInt** | Decimal / random 0–1 (shift) / `RanInt(a,b)` (alpha) | Working |
| **×10ˣ** | **π** | **e** | Insert `×10^` / insert `π` (shift) / insert `e` (alpha) | Working |
| **Ans** | **DRG▶** | **PreAns** | Insert `Ans` / cycle angle mode (shift) / insert `PreAns` (alpha) | Working |
| **=** | — | — | Evaluate expression | Working |

## Modes

| Mode | Description | Status |
|------|-------------|--------|
| **COMP** | Basic computation (default) | Working |
| **CMPLX** | Complex arithmetic (+, −, ×, ÷, abs, conj) | Working |
| **STAT** | One-variable statistics (n, Σx, Σx², mean, sx, σx, min, max) | Working |
| **BASE-N** | Base conversion (DEC/HEX/BIN/OCT) with +, −, ×, ÷, AND, OR, XOR, NOT | Working |
| **EQN** | Equation solving (numeric, one-variable via `solve()`) | Working |
| **MATRIX** | Matrix calculations (2×2, 3×3, add, sub, mul, det, transpose, inv) | Working |
| **TABLE** | Function table (f(X) with start/end/step) | Working |
| **VECTOR** | Vector calculations (2D/3D: +, −, ×, dot, cross, mag, unit) | Working |
| **INEQ/VERIF/DIST** | Inequality / verification / distributions | Phase 2 |
| **DEG** | Degrees (angle mode) | Working |
| **RAD** | Radians (angle mode) | Working |
| **GRAD** | Gradians (angle mode) | Working |
| **HYP** | Hyperbolic trig toggle | Working |
| **ENG** | Engineering notation | Working |
| **S⇔D** | Decimal ↔ Fraction | Working |
| **INS** | Insert/Overwrite toggle | Working |

## Memory Variables

| Key | Function | Status |
|-----|----------|--------|
| **STO** | SHIFT+RCL, then press ALPHA+letter (A-F, X, Y, M) to store | Working |
| **RCL** | Inserts `Ans` (recall last result) | Working |
| **M+** | Adds current result to memory M | Working |
| **M−** | Subtracts current result from memory M | Working |
| **A–F, X, Y, M** | Variables via ALPHA+key; engine recalls stored values | Working |
| **Ans** | Last result | Working |
| **PreAns** | Previous result before Ans | Working |

## Special Functions

| Function | Syntax / Behavior | Status |
|----------|------------------|--------|
| x⁻¹ | Computes 1 / current-value | Working |
| x! | Insert `!` — factorial on evaluate | Working |
| nPr | `5 nPr 2` = 20 | Working |
| nCr | `5 nCr 2` = 10 | Working |
| GCD | `gcd(12,18)` = 6 | Working |
| LCM | `lcm(12,18)` = 36 | Working |
| d/dx | `d/dx(X²,2)` = numeric derivative at x=2 | Working |
| RanInt | `RanInt(1,10)` = random int between 1–10 | Working |
| Int | `Int(3.7)` = 3 (truncation) | Working |
| Intg | `Intg(3.7)` = 3 (floor) | Working |
| Pol | `Pol(x,y)` → r | Working |
| Rec | `Rec(r,θ)` → x | Working |
| ∫ (integral) | `∫(f(X), a, b)` — Simpson's rule | Working |
| Σ (summation) | `Σ(f(X), start, end)` | Working |
| Constants | SHIFT+7 (CONST) opens menu, press number to insert | Working |
| Conversions | SHIFT+8 (CONV) opens menu, press number to convert | Working |
| Ran# | Random number 0–1 | Working |
| Rnd | Round displayed value | Working |
| DRG▶ | Cycle DEG/RAD/GRAD | Working |
| ∛ | Cube root engine (insert via SHIFT+√) | Working |

### STAT Operations
| Function | Description | Status |
|----------|-------------|--------|
| **Activation** | SHIFT+1 (STAT) or MODE→2 | Working |
| **Data entry** | Type comma-separated values (e.g., `10,20,30,40`) | Working |
| **=** | Opens STAT result menu (1–8) | Working |
| **n** | Count of values | Working |
| **Σx** | Sum of values | Working |
| **Σx²** | Sum of squares | Working |
| **x̄** | Mean (average) | Working |
| **sx** | Sample standard deviation (σₙ₋₁) | Working |
| **σx** | Population standard deviation (σₙ) | Working |
| **min** | Minimum value | Working |
| **max** | Maximum value | Working |
| **DEL** | Remove last entered value | Working |
| **AC** | Exit STAT mode, clear data | Working |
| **NAV keys** | Scroll through result menu | Working |
| **Number keys (1–8)** | Jump to specific result | Working |
| **Negative numbers** | Supported (enter with `-` prefix) | Working |
| **Decimal numbers** | Supported | Working |
| Frequency input (`value:frequency`) | Deferred | Future |

### CMPLX Activation
- **Keyboard**: SHIFT+2 (input becomes `CMPLX` via ShiftMap) activates CMPLX mode.
- **MODE menu**: MODE → press 1 → selects CMPLX from the mode list.
- **LCD indicator**: "CMPLX" appears in the status line when active.
- **Imaginary unit**: ALPHA+√ or ALPHA+ENG inserts `i`. Keyboard `I` key also inserts `i` when in CMPLX mode.
- **Expression entry**: Type expressions like `1+2i`, `(3+4i)*(1+2i)`, `abs(3+4i)`, `conj(3+4i)`.
- **Exit**: AC or MODE (switches back to COMP mode).

### CMPLX Operations
| Function | Syntax / Behavior | Status |
|----------|------------------|--------|
| Addition | `1+2i + 3+4i` → `4+6i` | Working |
| Subtraction | `3+4i - 1+2i` → `2+2i` | Working |
| Multiplication | `(1+2i)*(3+4i)` → `-5+10i` | Working |
| Division | `(3+4i)/(1+2i)` → `2.2-0.4i` | Working |
| Absolute value | `abs(3+4i)` → `5` | Working |
| Complex conjugate | `conj(3+4i)` → `3-4i` | Working |
| Imaginary unit entry | ALPHA+√ (`i`) or keyboard `I` | Working |
| Implicit multiply | `2i` = `2*i`, `i2` = `i*2`, `)i` = `)*i` | Working |
| Parentheses | `(1+2i)*(3+4i)` | Working |
| Whitespace | Spaces between tokens are ignored | Working |
| LCD indicator | "CMPLX" shown in status line | Working |
| Result formatting | `i` not `1i`, `0` hidden, no `+-`, near-zero cleaned | Working |

### CMPLX Known Limitations
- Polar/rectangular conversion not implemented (deferred).
- Argument function not exposed in parser (only abs/conj).
- Mixed REAL × CMPLX not exposed (only CMPLX mode expressions).

### MATRIX Activation
- **Keyboard**: SHIFT+4 (input becomes `MATRIX` via ShiftMap) activates MATRIX mode.
- **MODE menu**: MODE → press 6 → selects MATRIX from the mode list.
- **LCD indicator**: "MATRIX" appears in the status line when active.
- **Matrix entry syntax**: `[[1,2],[3,4]]` for 2×2, `[[1,2,3],[4,5,6],[7,8,9]]` for 3×3.
- **Assignment**: `MatA[[1,2],[3,4]]` or `A[[1,2],[3,4]]` stores to MatA (type name then literal, no space).
- **Reference**: Use `MatA`, `MatB`, `MatC` or shorthands `A`, `B`, `C` in expressions.
- **Exit**: AC or MODE (switches back to COMP mode).

### MATRIX Operations
| Function | Syntax / Behavior | Status |
|----------|------------------|--------|
| Addition | `MatA + MatB` → `[[6,8],[10,12]]` | Working |
| Subtraction | `MatB - MatA` → `[[4,4],[4,4]]` | Working |
| Multiplication | `MatA × MatB` → `[[19,22],[43,50]]` | Working |
| Scalar multiplication | `2 × MatA` → `[[2,4],[6,8]]` | Working |
| Determinant | `det(MatA)` → `-2` | Working |
| Transpose | `transpose(MatA)` or `trn(MatA)` | Working |
| Inverse (2×2) | `inv(MatA)` → `[[-2,1],[1.5,-0.5]]` | Working |
| Matrix literal | `[[1,2],[3,4]]` evaluates and stores as MatAns | Working |
| Keyboard `[` / `]` | Unshifted bracket keys insert `[` / `]` | Working |

### MATRIX Known Limitations
- 4×4 and larger matrices not supported.
- Inverse for 3×3+ uses adjugate method (may lose precision for near-singular).
- Eigenvalues / eigenvectors not implemented.
- Complex matrices not supported.
- No dedicated UI buttons for MatA/MatB/MatC — use keyboard ALPHA entry or assignment syntax.

### VECTOR Activation
- **Keyboard**: SHIFT+5 (input becomes `VECTOR` via ShiftMap) activates VECTOR mode.
- **MODE menu**: MODE → press 8 → selects VECTOR from the mode list.
- **LCD indicator**: "VECTOR" appears in the status line when active.
- **Vector entry syntax**: `[1,2]` for 2D, `[1,2,3]` for 3D.
- **Assignment**: `VecA[1,2,3]` or `A[1,2,3]` stores to VecA (type name then literal, no space).
- **Reference**: Use `VecA`, `VecB`, `VecC` or shorthands `A`, `B`, `C` in expressions.
- **Exit**: AC or MODE (switches back to COMP mode).

### VECTOR Operations
| Function | Syntax / Behavior | Status |
|----------|------------------|--------|
| Addition | `VecA + VecB` → `[5,7,9]` | Working |
| Subtraction | `VecB - VecA` → `[3,3,3]` | Working |
| Scalar multiplication | `2 × VecA` → `[2,4,6]` | Working |
| Dot product | `dot(VecA, VecB)` → `32` | Working |
| Cross product (3D only) | `cross(VecA, VecB)` → `[-3,6,-3]` | Working |
| Magnitude | `mag(VecA)` or `norm(VecA)` → `3.741657387` | Working |
| Unit vector | `unit([3,0,0])` → `[1,0,0]` | Working |
| Vector literal | `[1,2,3]` evaluates and stores as VecAns | Working |
| Keyboard `[` / `]` | Unshifted bracket keys insert `[` / `]` | Working |

### VECTOR Known Limitations
- angle() function not implemented (deferred).
- Polar/rectangular conversion not implemented.
- No dedicated UI buttons for VecA/VecB/VecC — use keyboard entry or assignment syntax.
- Interactive 3D visualization not available.

### TABLE Activation
- **MODE menu**: MODE → press 7 → selects TABLE from the mode list.
- **LCD indicator**: "TABLE" appears in the status line when active.
- **Workflow**: 4-step guided input:
  1. Enter `f(X)` expression (e.g., `X^2`) → press `=` → prompted for Start
  2. Enter Start value → press `=` → prompted for End
  3. Enter End value → press `=` → prompted for Step
  4. Enter Step value → press `=` → table is generated
- **Result navigation**: Use ▲/▼ to scroll through result rows. `=` restarts. `AC` goes back one step.
- **Exit**: Press `AC` repeatedly or change mode.

### TABLE Operations
| Function | Example | Result |
|----------|---------|--------|
| Generate table | `f(X)=X^2`, Start=1, End=3, Step=1 | X=1→1, X=2→4, X=3→9 |
| Scrolling | ▲/▼ in result view | Moves through rows |
| Restart | `=` in result view | Returns to f(X) input |
| Go back | `AC` in result view | Returns to Step entry |
| Max rows | 100 limit enforced | "Too many rows (>100)" |

### TABLE Known Limitations
- Two-function table (f(X) and g(X)) not implemented (deferred).
- Graphical plotting not available.
- Table export not implemented.
- Row limit: 100 rows maximum (configurable in TableService).
- Functions must use variable `X` (uppercase or lowercase).

### SOLVE Activation
- **Keyboard**: SHIFT+CALC inserts `solve(` at the cursor position.
- **Syntax**: `solve(equation, initialGuess)` or `solve(equation)` (guess defaults to 0).
- **Equation format**: Use `=` to separate left and right sides: `X+2=5`, `2*X+3=7`.
- **Without `=`**: treated as `equation = 0`: `X^2-4` is same as `X^2-4=0`.
- **Result**: The solver finds the root nearest to the initial guess using Newton-Raphson with bisection fallback.
- **Exit**: Press AC to clear the expression after viewing the result.

### SOLVE Operations
| Example | Guess | Result |
|---------|-------|--------|
| `solve(X+2=5,0)` | 0 | 3 |
| `solve(2*X+3=7,0)` | 0 | 2 |
| `solve(X^2-4=0,1)` | 1 | 2 |
| `solve(X^2-4=0,-1)` | -1 | -2 |
| `solve(log(X)=2,10)` | 10 | ~100 |
| `solve(sin(X)=0.5,30)` (DEG) | 30 | ~30 |

### SOLVE Known Limitations
- Numeric solving only (no symbolic algebra).
- Systems of equations not supported.
- Polynomial exact solving (quadratic formula, etc.) deferred.
- Function must use variable `X` (uppercase).
- Implicit multiplication (e.g., `2X`) not supported — use `2*X`.
- Step size for derivative may cause issues with very small/large values.
- Limited to 100 Newton-Raphson iterations, with bisection fallback on divergence.

### BASE-N Operations
| Function | Syntax / Behavior | Status |
|----------|------------------|--------|
| BIN entry | In BASE-N mode with BIN: type `1010` → `=` → shows 1010 | Working |
| HEX entry | In BASE-N mode with HEX: type `A` → `=` → shows A | Working |
| OCT entry | In BASE-N mode with OCT: type `12` → `=` → shows 12 | Working |
| BIN + | `1010 + 1` → `=` → 1011 | Working |
| HEX + | `A + 1` → `=` → B | Working |
| OCT + | `7 + 1` → `=` → 10 | Working |
| AND | `A AND 5` → `=` → 0 | Working |
| OR | `A OR 5` → `=` → F | Working |
| XOR | `A XOR 5` → `=` → F | Working |
| NOT | `NOT 0` → `=` → -1 | Working |
| ÷ (integer) | Integer division (floor) | Working |
| Base switch | ALPHA+x²→DEC, ALPHA+×ʸ→HEX, ALPHA+log→BIN, ALPHA+ln→OCT | Working |
| Input validation | Invalid digits rejected with LCD message | Working |
| Decimal point | Rejected with "Base-N integer only" | Working |

### STAT Activation
- **Keyboard**: SHIFT+1 (input becomes `STAT` via ShiftMap) activates STAT mode.
- **MODE menu**: MODE → press 2 → selects STAT from the mode list.
- **LCD indicator**: "STAT" appears in the status line when active.
- **Data entry**: Type values separated by commas, press `=` to view result menu.
- **Result menu**: Navigate with ↑/↓ or press 1–8 to jump to a specific result.
- **Exit**: AC or MODE (switches back to COMP/other mode).

## Known Limitations

### Deferred
- **SOLVE** — equation solving (CALC shift)
- **ʸ√x** — y-th root (xʸ shift)
- **VERIFY, INEQ, DIST** — remaining mode shortcuts and calculations

### BASE-N Mode
- Fraction and decimal-point operations are not supported (integer only).
- Expression evaluation is left-to-right with no operator precedence.
- Negative results in BIN/OCT/HEX display unsigned absolute value with a leading `-` sign.
- Bitwise operations use `AND`, `OR`, `XOR`, `NOT` keywords (inserted via letter keys or keyboard).
- No dedicated UI buttons for bitwise ops — use keywords in expression.
- Mode switching between bases resets the expression.

### STAT Mode (Working — Basic)
- Regression analysis not implemented (deferred).
- Frequency input (`value:frequency`, e.g., `10:2,20:3`) not implemented (deferred).
- Distribution calculations not implemented (deferred).
- Data editing (insert/delete at specific index) not implemented — only append and clear-last.

## Phase 1 Test Checklist

### Basic Arithmetic
- [ ] `2 + 3 =` → 5
- [ ] `10 ÷ 2 =` → 5
- [ ] `10 ÷ 0 =` → "Cannot divide by zero"
- [ ] `(2 + 3) × 4 =` → 20
- [ ] `−5 + 2 =` → -3
- [ ] `2 + 3 × 4 =` → 14

### Main Keys (Phase 1 fix)
- [ ] Press **x⁻¹** after `4 =` → 0.25
- [ ] Press **□/□** (FRAC) — toggles fraction display (same as S⇔D)
- [ ] Press **log□** — inserts `log(`
- [ ] Press **SHIFT + ∫** — inserts `d/dx(`
- [ ] Type `X^2,2)` and `=` → 4 (derivative of x² at 2)
- [ ] Press **SHIFT + √** — inserts `∛(`
- [ ] Type `27)` and `=` → 3 (cube root)

### SHIFT Key Tests
- [ ] **SHIFT + sin** → inserts `asin(`
- [ ] **SHIFT + cos** → inserts `acos(`
- [ ] **SHIFT + tan** → inserts `atan(`
- [ ] **SHIFT + log** → inserts `×10^`
- [ ] **SHIFT + ln** → inserts `exp(`
- [ ] **SHIFT + x²** → inserts `^3`
- [ ] **SHIFT + ×** → inserts ` nPr `
- [ ] **SHIFT + ÷** → inserts ` nCr `
- [ ] **SHIFT + HYP** → inserts `abs(`
- [ ] **SHIFT + DMS** → inserts `!`
- [ ] **SHIFT + (** → inserts ` % `
- [ ] **SHIFT + )** → inserts `,`
- [ ] **SHIFT + 7** → opens CONST menu
- [ ] **SHIFT + 8** → opens CONV menu
- [ ] **SHIFT + ×10ˣ** → inserts π
- [ ] **SHIFT + Ans** → cycles angle mode (DEG→RAD→GRAD)
- [ ] **SHIFT + .** → random number Ran#
- [ ] **SHIFT + DEL** → toggles INS/OVR
- [ ] **SHIFT + RCL** → stores value (then press ALPHA+letter)
- [ ] SHIFT indicator turns off after one mapped action

### ALPHA Key Tests
- [ ] **ALPHA + (−)** → inserts `A`
- [ ] **ALPHA + DMS** → inserts `B`
- [ ] **ALPHA + HYP** → inserts `C`
- [ ] **ALPHA + sin** → inserts `D`
- [ ] **ALPHA + cos** → inserts `E`
- [ ] **ALPHA + tan** → inserts `F`
- [ ] **ALPHA + (** → inserts `X`
- [ ] **ALPHA + )** → inserts `Y`
- [ ] **ALPHA + M+** → inserts `M`
- [ ] **ALPHA + ×** → inserts ` gcd(`
- [ ] **ALPHA + ÷** → inserts ` lcm(`
- [ ] **ALPHA + +** → inserts `Int(`
- [ ] **ALPHA + −** → inserts `Intg(`
- [ ] **ALPHA + .** → inserts `RanInt(`
- [ ] **ALPHA + ×10ˣ** → inserts `e`
- [ ] **ALPHA + Ans** → inserts `PreAns`
- [ ] **ALPHA + √** → inserts `i`
- [ ] **ALPHA + ENG** → inserts `i`
- [ ] ALPHA indicator turns off after one action (unless locked)

### STO / RCL
- [ ] Type `5 =` → then **SHIFT+RCL**, then **ALPHA+A** → shows "A stored"
- [ ] In new expression, type `A` and `=` → 5
- [ ] **M+** adds to memory M
- [ ] **M−** subtracts from memory M
- [ ] **ALPHA+M** inserts M variable
- [ ] `Int(3.7)` → 3
- [ ] `Intg(3.7)` → 3

### LCD Indicators
- [ ] Press **SHIFT** — "S" indicator becomes fully opaque
- [ ] Press a mapped key — "S" indicator dims (0.35 opacity)
- [ ] Press **ALPHA** — "A" indicator becomes fully opaque
- [ ] Press a mapped key — "A" indicator dims
- [ ] **MODE** opens setup with opaque LCD background
- [ ] **ⓘ (ABOUT)** opens about with opaque LCD background

### Keyboard
- [ ] Main keyboard `=` triggers evaluate
- [ ] `Enter` triggers evaluate
- [ ] While ALPHA is active, pressing `A`/`B`/`C`/`D`/`E`/`F`/`X`/`Y`/`M` inserts the variable

### BASE-N Mode Tests
- [ ] Press **SHIFT+3 (BASE)** — LCD shows "BASE-N" mode indicator
- [ ] LCD shows "BASE" + current base label (DEC/BIN/OCT/HEX)
- [ ] **ALPHA+x²** — base switches to DEC
- [ ] **ALPHA+×ʸ** — base switches to HEX
- [ ] **ALPHA+log** — base switches to BIN
- [ ] **ALPHA+ln** — base switches to OCT
- [ ] Type `10` in DEC mode → `=` → 10
- [ ] Switch to HEX → type `A` → `=` → A
- [ ] Switch to BIN → type `1010` → `=` → 1010
- [ ] Switch to OCT → type `12` → `=` → 12
- [ ] In BIN: type `1010 + 1` → `=` → 1011
- [ ] In HEX: type `A + 1` → `=` → B
- [ ] In OCT: type `7 + 1` → `=` → 10
- [ ] In HEX: type `A AND 5` → `=` → 0
- [ ] In HEX: type `A OR 5` → `=` → F
- [ ] In HEX: type `A XOR 5` → `=` → F
- [ ] In any base: type `NOT 0` → `=` → -1
- [ ] In BIN: type `2` → "Invalid BIN digit"
- [ ] In OCT: type `8` → "Invalid OCT digit"
- [ ] In any base: type `.` → "Base-N integer only"
- [ ] Type `5 - 10` in DEC → -5
- [ ] Press **AC** — exits BASE-N mode, LCD indicator clears

### STAT Mode Tests
- [ ] Press **SHIFT+1 (STAT)** — LCD shows "STAT" mode indicator
- [ ] Type `10,20,30,40` → press `=` → result menu appears
- [ ] Menu shows: `1: n = 4`, `2: Σx = 100`, `3: Σx² = 3000`, `4: x̄ = 25`
- [ ] Press `↓` → scrolls through menu items
- [ ] Press `7` → shows `min = 10`
- [ ] Press `8` → shows `max = 40`
- [ ] Press **DEL** — removes last value
- [ ] Type `abc` → shows "Invalid: abc"
- [ ] Type empty → "No data"
- [ ] One value only → sample std dev shows "N/A"
- [ ] Press **AC** — exits STAT mode, LCD indicator clears
- [ ] Type negative numbers: `-5,0,5` → mean = 0
- [ ] Type decimals: `1.5,2.5,3.0` → Σx = 7

### VECTOR Mode Tests
- [ ] Press **SHIFT+5 (VECTOR)** — LCD shows "VECTOR" mode indicator
- [ ] Type `[1,2,3]` → `=` → `[1,2,3]`
- [ ] Type `[1,2]` + `=` → `[1,2]`
- [ ] `VecA[1,2,3]` → `=` → `VecA = [1,2,3]`
- [ ] `VecB[4,5,6]` → `=` → `VecB = [4,5,6]`
- [ ] `VecA + VecB` → `=` → `[5,7,9]`
- [ ] `VecB - VecA` → `=` → `[3,3,3]`
- [ ] `2 × VecA` → `=` → `[2,4,6]`
- [ ] `dot(VecA, VecB)` → `=` → `32`
- [ ] `cross(VecA, VecB)` → `=` → `[-3,6,-3]`
- [ ] `mag(VecA)` → `=` → `3.741657387`
- [ ] `norm(VecA)` → `=` → `3.741657387`
- [ ] `unit([3,0,0])` → `=` → `[1,0,0]`
- [ ] `[1,2] + [3,4]` → `=` → `[4,6]`
- [ ] `mag([3,4])` → `=` → `5`
- [ ] `unit([3,4])` → `=` → `[0.6,0.8]`
- [ ] `A[1,2]` → `=` → `VecA = [1,2]` (shorthand assignment)
- [ ] `dot(A, B)` → `=` → `14` (2D × 3D dot product)
- [ ] `[]` → `=` → "Math Error: Empty vector"
- [ ] `[1,2,3,4]` → `=` → "Math Error: Vector must have 2 or 3 components"
- [ ] `[abc]` → `=` → "Math Error: Invalid number: abc"
- [ ] `cross([1,2], [3,4])` → `=` → "Math Error: Cross product requires 3D vectors"
- [ ] `unit([0,0])` → `=` → "Math Error: Zero vector has no unit vector"
- [ ] `[1,2] + [1,2,3]` → `=` → "Math Error: Vector dimension mismatch for addition"
- [ ] Press **AC** — exits VECTOR mode, LCD indicator clears

### TABLE Mode Tests
- [ ] Open **MODE** → press **7** — LCD shows "TABLE" mode indicator, prompt "Enter f(X)"
- [ ] Type `X^2` → press `=` — prompts "Start?"
- [ ] Type `1` → press `=` — prompts "End?"
- [ ] Type `3` → press `=` — prompts "Step?"
- [ ] Type `1` → press `=` — shows `f(X) = X^2` header; press ▼ to see X=1→1, X=2→4, X=3→9
- [ ] Press **▲**/**▼** — scrolls through table rows
- [ ] Press **=** in result view — returns to "Enter f(X)" prompt
- [ ] Press **AC** in result view — returns to Step entry
- [ ] Press **AC** repeatedly — exits TABLE mode
- [ ] f(X)=`2X+1`, Start=0, End=2, Step=1 → 1, 3, 5
- [ ] f(X)=`X^3`, Start=1, End=3, Step=1 → 1, 8, 27
- [ ] f(X)=`sin(X)` (DEG mode), Start=0, End=90, Step=45 → 0, 0.7071, 1
- [ ] Empty function → "Enter f(X) first"
- [ ] Function without X → "Function must use X"
- [ ] Step=0 → "Step cannot be zero"
- [ ] Start=0, End=100, Step=0.1 → "Too many rows (>100)"
- [ ] f(X)=`log(X)`, Start=-1, End=2, Step=1 → X=-1 shows "Error"
- [ ] **AC** in phase 1/2/3 — goes back to previous phase
- [ ] **AC** in phase 0 — exits TABLE mode
- [ ] Press **AC** — TABLE indicator clears

### Error Cases
- [ ] No button silently does nothing (all show info or insert something)
- [ ] `10/0=` → "Cannot divide by zero"
- [ ] `log(-1)=` → "Math Error"
- [ ] `√(-1)=` → "Math Error" (in COMP mode)
- [ ] `∛(27)=` → 3 (cube root succeeds)
- [ ] Empty `=` → 0

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| `0`–`9` | Digits |
| `.` | Decimal point |
| `+` | Add |
| `-` | Subtract |
| `*` | Multiply (numpad) |
| `/` | Divide (numpad) |
| `=` | Evaluate (main keyboard `=` and numpad `Enter`) |
| `Enter` | Evaluate |
| `(`, `)` | Parentheses |
| `Backspace` | Delete at cursor (DEL) |
| `Escape` | Clear all (AC) |
| `↑` | History up / menu up |
| `↓` | History down / menu down |
| `←` | Cursor left |
| `→` | Cursor right |
| `A`–`F` | Insert hex digit (when BASE-N+HEX active) OR insert variable (when ALPHA active) |
| `X`, `Y`, `M` | Insert variable (when ALPHA active) |
