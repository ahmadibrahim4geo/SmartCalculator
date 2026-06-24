# SmartCalculator v1.1.0 — Development Roadmap

## Planned Features (from deferred list)

### Scientific Functions
- [ ] **ʸ√x** (y-th root via SHIFT+xʸ) — implement engine support and UI wiring

### Modes
- [ ] **VERIFY mode** (SHIFT+6) — inequality/equality verification
- [ ] **INEQ mode** (MODE page 2) — inequality solving
- [ ] **DIST mode** (MODE page 2) — distribution calculations (normal, binomial, Poisson)

### STAT Mode Improvements
- [ ] **Advanced regression** — linear, logarithmic, exponential, power, quadratic
- [ ] **Frequency input** — support `value:frequency` syntax
- [ ] **Data editing** — insert/delete at specific indices

### Vector Mode Improvements
- [ ] **angle()** — angle between two vectors
- [ ] **Polar/rectangular conversion**

### Matrix Mode Improvements
- [ ] **4×4+ matrix support** (where practical)
- [ ] **Complex matrix support**

### SOLVE Improvements
- [ ] **Symbolic algebra** (basic) — quadratic formula, linear equations
- [ ] **Systems of equations** (2×2, 3×3)

### UI/UX
- [ ] **Theme selector** — swap between available theme XAML files at runtime
- [ ] **History browser** — improved history navigation with persistent storage
- [ ] **Font size adjustments** — user-configurable LCD/button font sizes

### Packaging
- [ ] **Installer improvements** — optional auto-update check
- [ ] **Portable mode detection** — auto-configure for USB/portable scenarios

### Quality
- [ ] **User feedback** — collect and triage issues from initial release
- [ ] **Additional test coverage** — edge cases, keyboard shortcuts, localization

## Branch Strategy

```powershell
git checkout -b develop/v1.1.0
```

Feature branches merge into `develop/v1.1.0`.
Release branch from `develop/v1.1.0` when ready.

## Timeline (target)

- Development: Q3 2026
- Beta: Q4 2026
- Release: Q4 2026

*Timeline is tentative and depends on community feedback and contributor availability.*
