# SmartCalculator Help Manual & Guide

Welcome to **SmartCalculator** (v3.0.0), a premium scientific calculator built with WPF and .NET 8.

---

## Features Summary

1. **130 Unique Visual Themes**: Click the Theme button `🎨` to cycle, or hold it to open the scrollable theme list.
2. **20 Independent Ergonomic Skins**: Click the Skin button `🔲` to cycle, hold it for the list, or use `MODE → ▶ → 5:SKIN`. Skins change button profiles and navigation while secondary labels remain crisp and readable.
3. **Complete Compact Mode**: Switch to the space-saving mini-calculator using `Ctrl+M`, the header toggle `⤡`/`⤢`, or `MODE → ▶ → 6:COMPACT`.
4. **Persistent Preferences**: Theme, skin, compact mode, and always-on-top state are automatically saved to `%LocalAppData%\SmartCalculator\settings.json`.
5. **Balanced Header with Developer Signature**: Features developer Ahmad Ibrahim's authentic Arabic calligraphy logo ("أحمد إبراهيم") prominently at the center, flanked by branding and persistent developer subtitle "Scientific Display • by: Ahmad Ibrahim" on the left, and window controls (Pin, Compact, Close) and quick tools (Theme, Skin, About, Help) on the right.
6. **Persistent Developer Subtitle**: "Scientific Display • by: Ahmad Ibrahim" remains visible in both Full mode and Compact mode.
7. **Tactile Keyboard Sync**: The on-screen navigation buttons visually respond to mouse and keyboard input.
8. **Scientific Error Recovery**: Dedicated Syntax ERROR screen with direct left/right cursor return.
9. **Nth Root**: Press `SHIFT`, then `xʸ`, and enter `root(value,index)` (for example, `root(32,5)`).

---

## Operating Modes

Press the `MODE` button to view the modes selection list, then press the corresponding number key to activate:

### Page 1 (Modes)
| Number | Mode | Description |
| :---: | :--- | :--- |
| **1** | **COMP** | General arithmetic and basic scientific functions. |
| **2** | **CMPLX** | Complex numbers arithmetic using the imaginary coordinate `i`. |
| **3** | **STAT** | Statistical calculations (single-variable summary). |
| **4** | **BASE-N** | Base-N conversion and logical operators (Hex, Dec, Oct, Bin). |
| **5** | **EQN** | Numerical equation solvers. |
| **6** | **MATRIX** | Matrix operations (algebraic manipulations, transposition, determinants). |
| **7** | **TABLE** | Generation of tables of values for expressions $f(x)$. |
| **8** | **VECTOR** | Vector mathematics (dot product, cross product, magnitude). |

### Page 2 (Setup & Preferences - press `▶`)
| Number | Action | Description |
| :---: | :--- | :--- |
| **4** | **THEME** | Opens the visual theme selector (130 themes). |
| **5** | **SKIN** | Opens the button skin selector (20 skins). |
| **6** | **COMPACT** | Toggles between Full Scientific and Compact mode. |

---

## Keyboard Shortcuts & Binding

| Calculator Button | Keyboard Key | Action / Function |
| :--- | :--- | :--- |
| `0` - `9` | `0` - `9` or Numpad | Inputs numbers. |
| `+`, `-`, `×`, `÷` | `+`, `-`, `*`, `/` | Inputs math operators. |
| `=` | `Enter` or `=` | Calculates expression result. |
| `DEL` | `Backspace` | Deletes character at cursor. |
| `AC` | `Escape` | Clears all calculation input. |
| `UP` / `DOWN` | `Up` / `Down` Arrow | Navigate lists, scroll calculation history. |
| `LEFT` / `RIGHT` | `Left` / `Right` Arrow | Move blinking cursor. |
| `NAV` | `Enter` (inside menu list) | Select/confirm highlighted item. |
| Compact Mode | `Ctrl+M` | Toggle Compact / Full mode. |
| Help Manual | `F1` | Open user guide. |

---

## Troubleshooting Errors

- **Syntax ERROR**: Formula syntax is incorrect. Press any arrow key (`Left` / `Right` / `Up` / `Down`) to dismiss the error and place the cursor exactly at the location of the typo.
- **Math ERROR**: Mathematical division by zero or domain error. Press `AC` to clear the screen and begin a new formula.

---

*SmartCalculator v3.0.0 &copy; 2026 Ahmad Ibrahim. All Rights Reserved.*
