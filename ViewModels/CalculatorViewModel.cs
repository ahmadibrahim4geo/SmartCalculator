using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using SmartCalculator.Models;
using SmartCalculator.Services;

namespace SmartCalculator.ViewModels;

public class CalculatorViewModel : INotifyPropertyChanged
{
    private readonly CalculatorEngine _engine = new();
    private readonly MemoryService _memory = new();
    private readonly AngleService _angle = new();
    private readonly StatisticsService _stats = new();
    private readonly BaseNService _baseN = new();
    private readonly TableService _table = new();
    private string _expression = "";
    private string _resultText = "0";
    private SolidColorBrush _resultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
    private bool _justEvaluated;
    private bool _hasError;
    private bool _isShiftActive;
    private bool _isAlphaActive;
    private bool _isHypActive;
    private int _cursorIndex;
    private bool _insertMode = true;
    private bool _showSetup;
    private int _setupPage;
    private string _setupInfoText = "";
    private bool _setupInfoVisible;
    private CalculatorMode _currentMode = CalculatorMode.COMP;
    private bool _isBaseNMode;
    private bool _isStatsMode;
    private bool _isComplexMode;
    private bool _isMatrixMode;
    private bool _isVectorMode;
    private bool _isEqnMode;
    private bool _isTableMode;
    private bool _showConstants;
    private bool _showConversions;
    private bool _showHistory;
    private ResultDisplayMode _resultDisplayMode;
    private double _lastNumericResult;
    private long _fracNumerator;
    private long _fracDenominator;
    private string _fracInlineText = "";
    private int _historyIndex = -1;
    private readonly List<string> _history = new();
    private string _setupStatusText = "";
    private bool _showAbout;
    private bool _showStatsMenu;
    private int _statsMenuPage;
    private int _aboutPage;
    private int _tablePhase;
    private int _tablePage;
    private string _tableFunction = "";

    public CalculatorViewModel()
    {
        _engine.Memory = _memory;
        _engine.Angle = _angle;
        InputCommand = new RelayCommand(HandleInput);
    }

    // ============ Properties ============

    public string ExpressionText
    {
        get => _expression;
        set { _expression = value; OnPropertyChanged(); }
    }

    public string ResultText
    {
        get => _resultText;
        set { _resultText = value; OnPropertyChanged(); }
    }

    public SolidColorBrush ResultBrush
    {
        get => _resultBrush;
        set { _resultBrush = value; OnPropertyChanged(); }
    }

    public string AngleModeText => _angle.DisplayText;

    public bool IsShiftActive
    {
        get => _isShiftActive;
        set { _isShiftActive = value; OnPropertyChanged(); }
    }

    public bool IsAlphaActive
    {
        get => _isAlphaActive;
        set { _isAlphaActive = value; OnPropertyChanged(); }
    }

    public bool IsHypActive
    {
        get => _isHypActive;
        set { _isHypActive = value; OnPropertyChanged(); }
    }

    public bool ShowSetup
    {
        get => _showSetup;
        set { _showSetup = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentSetupPage)); OnPropertyChanged(nameof(SetupDisplayItems)); }
    }

    public bool SetupInfoVisible
    {
        get => _setupInfoVisible;
        set { _setupInfoVisible = value; OnPropertyChanged(); }
    }

    public string SetupInfoText
    {
        get => _setupInfoText;
        set { _setupInfoText = value; OnPropertyChanged(); }
    }

    public string SetupStatusText
    {
        get => _setupStatusText;
        set { _setupStatusText = value; OnPropertyChanged(); }
    }

    public bool ShowConstants
    {
        get => _showConstants;
        set { _showConstants = value; OnPropertyChanged(); OnPropertyChanged(nameof(ConstantDisplayItems)); }
    }

    public bool ShowConversions
    {
        get => _showConversions;
        set { _showConversions = value; OnPropertyChanged(); OnPropertyChanged(nameof(ConversionDisplayItems)); }
    }

    public bool ShowHistory
    {
        get => _showHistory;
        set { _showHistory = value; OnPropertyChanged(); }
    }

    public bool ShowDecimal => _resultDisplayMode == ResultDisplayMode.Decimal;
    public bool ShowInlineFraction => _resultDisplayMode == ResultDisplayMode.InlineFraction;
    public bool ShowStackedFraction => _resultDisplayMode == ResultDisplayMode.StackedFraction;
    public string FracInlineText { get => _fracInlineText; set { _fracInlineText = value; OnPropertyChanged(); } }
    public long FracNumerator { get => _fracNumerator; set { _fracNumerator = value; OnPropertyChanged(); } }
    public long FracDenominator { get => _fracDenominator; set { _fracDenominator = value; OnPropertyChanged(); } }
    public string FracSignDisplay { get; set; } = "";
    public ResultDisplayMode ResultMode { get => _resultDisplayMode; set { _resultDisplayMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(ShowDecimal)); OnPropertyChanged(nameof(ShowInlineFraction)); OnPropertyChanged(nameof(ShowStackedFraction)); } }

    public string ModeLabel => _currentMode switch
    {
        CalculatorMode.CMPLX => "CMPLX",
        CalculatorMode.STAT => "STAT",
        CalculatorMode.BASE_N => "BASE",
        CalculatorMode.EQN => "EQN",
        CalculatorMode.MATRIX => "MATRIX",
        CalculatorMode.TABLE => "TABLE",
        CalculatorMode.VECTOR => "VECTOR",
        CalculatorMode.VERIFY => "VERIFY",
        _ => ""
    };

    public string BaseLabel => _baseN.BaseLabel;
    public bool IsBaseActive => _baseN.IsBaseActive;
    public bool IsBaseNActive => _isBaseNMode;
    public bool IsStatsActive => _isStatsMode;
    public bool IsCmplxActive => _isComplexMode;
    public bool IsMatrixActive => _isMatrixMode;
    public bool IsVectorActive => _isVectorMode;
    public bool IsEqnActive => _isEqnMode;
    public bool IsTableActive => _isTableMode;

    public string[] ConstantDisplayItems => ConstantLibraryService.DisplayItems;
    public string[] ConversionDisplayItems => UnitConversionService.DisplayItems;

    public static readonly string[][] SetupPages = new[]
    {
        new[] { "COMP", "CMPLX", "STAT", "BASE-N", "EQN", "MATRIX", "TABLE", "VECTOR" },
        new[] { "INEQ", "VERIF", "DIST" }
    };

    public static readonly string[][] AboutPages = new[]
    {
        new[] { "About 1/3", "SmartCalculator", "Scientific Calculator", "Version: 1.0.0" },
        new[] { "About 2/3", "Developer: Ahmad Ibrahim", "License: MIT License", "Free & Open Source" },
        new[] { "About 3/3", "Framework: .NET 8 WPF", "Copyright: \u00a9 2026 Ahmad Ibrahim", "All rights reserved." }
    };

    public bool ShowAbout
    {
        get => _showAbout;
        set { _showAbout = value; OnPropertyChanged(); OnPropertyChanged(nameof(AboutDisplayItems)); OnPropertyChanged(nameof(AboutPageText)); }
    }

    public string AboutPageText => $"About {_aboutPage + 1}/{AboutPages.Length}";

    public string[] AboutDisplayItems
    {
        get
        {
            var items = AboutPages[_aboutPage];
            // Return all items except the first (page title) for body; title shown separately
            return items[1..];
        }
    }

    public string[] CurrentSetupPage => SetupPages[_setupPage];

    public string[] SetupDisplayItems
    {
        get
        {
            var items = SetupPages[_setupPage];
            var result = new string[items.Length];
            for (int i = 0; i < items.Length; i++)
                result[i] = $"{i + 1}:{items[i]}";
            return result;
        }
    }

    public int CursorIndex
    {
        get => _cursorIndex;
        set { _cursorIndex = Math.Clamp(value, 0, _expression.Length); OnPropertyChanged(); }
    }

    // ============ Input Handler ============

    public ICommand InputCommand { get; }

    private void HandleInput(object? param)
    {
        string input = param as string ?? "";

        // Handle sub-menus first
        if (_showConstants) { HandleConstantsInput(input); return; }
        if (_showConversions) { HandleConversionsInput(input); return; }
        if (_showHistory) { HandleHistoryInput(input); return; }
        if (_isStatsMode) { HandleStatsInput(input); return; }
        if (_isTableMode)
        {
            if (_tablePhase == 0 && input is not ("=" or "CALC" or "AC" or "ON" or "SHIFT" or "ALPHA"))
            {
                // Phase 0: let normal expression entry handle most keys
            }
            else
            {
                HandleTableInput(input);
                return;
            }
        }
        if (_showSetup) { HandleSetupInput(input); return; }
        if (_showAbout) { HandleAboutInput(input); return; }

        // SHIFT/ALPHA toggle (apply before mode dispatch so mapping works in all modes)
        if (input == "SHIFT") { IsShiftActive = !_isShiftActive; IsAlphaActive = false; return; }
        if (input == "ALPHA") { IsAlphaActive = !_isAlphaActive; IsShiftActive = false; return; }

        // SHIFT mapping (turn off SHIFT after one mapped action)
        if (_isShiftActive)
        {
            string? shifted = KeyMaps.ShiftMap.GetValueOrDefault(input);
            if (shifted != null) { IsShiftActive = false; input = shifted; }
        }
        // ALPHA mapping (turn off ALPHA after every key press)
        if (_isAlphaActive)
        {
            string? alphaMapped = KeyMaps.AlphaMap.GetValueOrDefault(input);
            if (alphaMapped != null) { input = alphaMapped; }
            IsAlphaActive = false;
        }

        // Mode-specific dispatch (after SHIFT/ALPHA mapping)
        if (_isBaseNMode) { HandleBaseNInput(input); return; }

        if (_hasError)
        {
            if (input is "AC" or "ON") { ClearAll(); return; }
            if (input is "DEL") { DeleteAtCursor(); return; }
            ClearAll();
        }

        if (_justEvaluated && input is not ("=" or "AC" or "DEL" or "SHIFT" or "ALPHA" or "MODE" or "STO"
            or "INVERSE" or "INTEGRAL" or "DMS" or "SD" or "FRAC" or "LOGAB"
            or "CALC" or "SOLVE" or "RCL" or "M+" or "M-" or "HYP" or "DRG▶" or "ENG"
            or "ABOUT" or "CONST" or "CONV" or "OFF" or "CLR" or "INS"))
        {
            _justEvaluated = false;
            if (input is "+" or "-" or "−" or "×" or "÷" or "xʸ")
            {
                ExpressionText = FormatNumber(_memory.Ans);
                ResultText = "0";
            }
            else if (input is "x²") { ExpressionText = FormatNumber(_memory.Ans) + "^2"; ResultText = "0"; return; }
            else { ExpressionText = ""; ResultText = "0"; }
        }

        switch (input)
        {
            case "0": case "1": case "2": case "3": case "4":
            case "5": case "6": case "7": case "8": case "9":
            case ".": InsertAtCursor(input); break;
            case "+": InsertAtCursor(" + "); break;
            case "−": case "-": InsertAtCursor(" - "); break;
            case "×": InsertAtCursor(" × "); break;
            case "÷": InsertAtCursor(" ÷ "); break;
            case "xʸ": InsertAtCursor(" ^ "); break;
            case "x²": InsertAtCursor("^2"); break;
            case "x³": InsertAtCursor("^3"); break;
            case "^": InsertAtCursor("^"); break;
            case "sin": InsertAtCursor(_isHypActive ? "sinh(" : "sin("); ClearHyp(); break;
            case "cos": InsertAtCursor(_isHypActive ? "cosh(" : "cos("); ClearHyp(); break;
            case "tan": InsertAtCursor(_isHypActive ? "tanh(" : "tan("); ClearHyp(); break;
            case "asin": InsertAtCursor("asin("); break;
            case "acos": InsertAtCursor("acos("); break;
            case "atan": InsertAtCursor("atan("); break;
            case "log": InsertAtCursor("log("); break;
            case "ln": InsertAtCursor("ln("); break;
            case "√": InsertAtCursor("√("); break;
            case "SQRT": InsertAtCursor("√("); break;
            case "(-)": InsertAtCursor("-"); break;
            case "(": InsertAtCursor("("); break;
            case ")": InsertAtCursor(")"); break;
            case "π": InsertAtCursor("π"); break;
            case "e": InsertAtCursor("e"); break;
            case "Ans": InsertAtCursor("Ans"); break;
            case "PreAns": InsertAtCursor("PreAns"); break;
            case "×10^": InsertAtCursor("×10^"); break;
            case "exp": InsertAtCursor("exp("); break;
            case "abs": InsertAtCursor("abs("); break;
            case "fact": InsertAtCursor("!"); break;
            case "SOLVE": InsertAtCursor("solve("); break;

            case "=": case "CALC": Calculate(); break;

            case "AC": case "ON":
                if (_showSetup) { ShowSetup = false; return; }
                if (_showAbout) { ShowAbout = false; if (input == "ON") { ClearAll(); return; } return; }
                ClearAll(); break;
            case "DEL": DeleteAtCursor(); break;

            case "HYP": IsHypActive = !_isHypActive; ShowInfo(_isHypActive ? "HYP" : ""); break;

            case "MODE": ToggleModeMenu(); break;
            case "ABOUT": ToggleAbout(); break;

            case "STO": HandleStore(); break;
            case "RCL": InsertAtCursor("Ans"); break;
            case "M+": _memory.AddToM(EvaluateCurrent()); ShowInfo("M+"); break;
            case "M-": _memory.SubtractFromM(EvaluateCurrent()); ShowInfo("M-"); break;

            case "ENG": FormattingService.Mode = FormattingService.DisplayMode.Engineering; ShowInfo("ENG"); break;
            case "SD": ToggleFractionDisplay(); break;
            case "DMS": HandleDMS(); break;

            case "nPr": InsertAtCursor(" nPr "); break;
            case "nCr": InsertAtCursor(" nCr "); break;
            case ",": InsertAtCursor(","); break;
            case "%": InsertAtCursor(" % "); break;
            case "∛": InsertAtCursor("∛("); break;
            case "GCD": InsertAtCursor(" gcd("); break;
            case "LCM": InsertAtCursor(" lcm("); break;
            case "Ran#": ResultText = ScientificFunctions.Ran().ToString("G6"); _memory.PushAns(double.Parse(ResultText)); break;
            case "Rnd": ResultText = FormatNumber(Math.Round(EvaluateCurrent(), FormattingService.DecimalPlaces)); break;

            case "Pol": InsertAtCursor("Pol("); break;
            case "Rec": InsertAtCursor("Rec("); break;
            case "DRG▶": _angle.Cycle(); OnPropertyChanged(nameof(AngleModeText)); ShowInfo(_angle.DisplayText); break;

            case "INTEGRAL": InsertAtCursor("∫("); break;
            case "Σ": InsertAtCursor("Σ("); break;

            /* === Phase 1: newly wired keys === */
            case "INVERSE": HandleInverse(); break;
            case "FRAC": ToggleFractionDisplay(); break;
            case "LOGAB": InsertAtCursor("log("); break;
            case "DERIV": InsertAtCursor("d/dx("); break;

            /* === ALPHA variable letters (insert or store to pending STO) === */
            case "A": HandleVariableInput("A"); break;
            case "B": HandleVariableInput("B"); break;
            case "C": HandleVariableInput("C"); break;
            case "D": HandleVariableInput("D"); break;
            case "E": HandleVariableInput("E"); break;
            case "F": HandleVariableInput("F"); break;
            case "X": HandleVariableInput("X"); break;
            case "Y": HandleVariableInput("Y"); break;
            case "M": HandleVariableInput("M"); break;

            /* === ALPHA helper functions (insert template) === */
            case "RanInt": InsertAtCursor("RanInt("); break;
            case "Int": InsertAtCursor("Int("); break;
            case "Intg": InsertAtCursor("Intg("); break;
            case "i": InsertAtCursor("i"); break;
            case "[": InsertAtCursor("["); break;
            case "]": InsertAtCursor("]"); break;
            case "d/c": ToggleFractionDisplay(); break;

            /* === BASE-N mode selectors (swap base while in BASE-N mode) === */
            case "DEC": if (_isBaseNMode) { _baseN.CurrentBase = BaseNService.NumBase.DEC; ShowInfo(_baseN.BaseLabel); OnPropertyChanged(nameof(BaseLabel)); break; } ShowNotAvailable(); break;
            case "HEX": if (_isBaseNMode) { _baseN.CurrentBase = BaseNService.NumBase.HEX; ShowInfo(_baseN.BaseLabel); OnPropertyChanged(nameof(BaseLabel)); break; } ShowNotAvailable(); break;
            case "BIN": if (_isBaseNMode) { _baseN.CurrentBase = BaseNService.NumBase.BIN; ShowInfo(_baseN.BaseLabel); OnPropertyChanged(nameof(BaseLabel)); break; } ShowNotAvailable(); break;
            case "OCT": if (_isBaseNMode) { _baseN.CurrentBase = BaseNService.NumBase.OCT; ShowInfo(_baseN.BaseLabel); OnPropertyChanged(nameof(BaseLabel)); break; } ShowNotAvailable(); break;

            /* === Mode shortcut keys === */
            case "STAT": ActivateMode(CalculatorMode.STAT); ShowInfo("STAT"); break;
            case "CMPLX": ActivateMode(CalculatorMode.CMPLX); SetupStatusText = "CMPLX"; ShowInfo("CMPLX"); break;
            case "MATRIX": ActivateMode(CalculatorMode.MATRIX); SetupStatusText = "MATRIX"; ShowInfo("MATRIX"); break;
            case "VECTOR": ActivateMode(CalculatorMode.VECTOR); SetupStatusText = "VECTOR"; ShowInfo("VECTOR"); break;
            case "VERIFY": ShowInfo("VERIFY (Phase 2)"); break;
            case "BASE": ActivateMode(CalculatorMode.BASE_N); ShowInfo("BASE-N"); break;

            case "CONST": ShowConstants = true; break;
            case "CONV": ShowConversions = true; break;

            case "UP": if (_showAbout) NavigateAbout(-1); else NavigateHistory(-1); break;
            case "DOWN": if (_showAbout) NavigateAbout(1); else NavigateHistory(1); break;
            case "LEFT": CursorIndex--; break;
            case "RIGHT": CursorIndex++; break;
            case "INS": _insertMode = !_insertMode; ShowInfo(_insertMode ? "INS" : "OVR"); break;

            case "CLR": ClearAll(); break;
            case "OFF": ClearAll(); break;

            default:
                ShowNotAvailable();
                break;
        }
    }

    // ============ Core Operations ============

    private void InsertAtCursor(string text)
    {
        if (_cursorIndex >= _expression.Length)
            ExpressionText += text;
        else
            ExpressionText = _expression.Insert(_cursorIndex, text);
        CursorIndex += text.Length;
        _justEvaluated = false;
    }

    private void DeleteAtCursor()
    {
        if (_expression.Length == 0) return;
        if (_cursorIndex <= 0) return;
        int len = 1;
        if (_cursorIndex >= 3 && _expression[_cursorIndex - 1] == ' ' && _expression[_cursorIndex - 3] == ' ') len = 3;
        else if (_cursorIndex >= 1 && _expression[_cursorIndex - 1] == '(') len = 1;
        ExpressionText = _expression.Remove(_cursorIndex - len, len);
        CursorIndex -= len;
        _justEvaluated = false;
    }

    private void Calculate()
    {
        string expr = _expression.Trim().Replace("×10^", "*10^");
        if (string.IsNullOrEmpty(expr)) { ResultText = "0"; ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18)); return; }

        if (_isComplexMode && ComplexService.NeedsComplexEvaluation(expr))
        {
            string result = ComplexService.EvaluateToString(expr);
            ResultText = result;
            ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
            _history.Add($"{expr} = {result}");
            _historyIndex = _history.Count;
            _justEvaluated = true;
            _hasError = false;
            return;
        }

        if (_isMatrixMode)
        {
            string result = SmartCalculator.Services.MatrixService.EvaluateToString(expr);
            ResultText = result;
            ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
            _history.Add($"{expr} = {result}");
            _historyIndex = _history.Count;
            _justEvaluated = true;
            _hasError = false;
            return;
        }

        if (_isVectorMode)
        {
            string result = VectorService.EvaluateToString(expr);
            ResultText = result;
            ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
            _history.Add($"{expr} = {result}");
            _historyIndex = _history.Count;
            _justEvaluated = true;
            _hasError = false;
            return;
        }

        // Handle nPr, nCr
        if (expr.Contains(" nPr "))
        {
            var parts = expr.Split(" nPr ");
            if (parts.Length == 2 && double.TryParse(parts[0], out var n) && double.TryParse(parts[1], out var r))
            { _memory.PushAns(ScientificFunctions.Npr(n, r)); ShowResult(_memory.Ans); return; }
        }
        if (expr.Contains(" nCr "))
        {
            var parts = expr.Split(" nCr ");
            if (parts.Length == 2 && double.TryParse(parts[0], out var n) && double.TryParse(parts[1], out var r))
            { _memory.PushAns(ScientificFunctions.Ncr(n, r)); ShowResult(_memory.Ans); return; }
        }

        // Handle gcd/lcm
        if (expr.StartsWith("gcd(") && expr.EndsWith(")"))
        {
            var inner = expr[4..^1].Split(',');
            if (inner.Length == 2 && long.TryParse(inner[0], out var a) && long.TryParse(inner[1], out var b))
            { _memory.PushAns(ScientificFunctions.Gcd(a, b)); ShowResult(_memory.Ans); return; }
        }
        if (expr.StartsWith("lcm(") && expr.EndsWith(")"))
        {
            var inner = expr[4..^1].Split(',');
            if (inner.Length == 2 && long.TryParse(inner[0], out var a) && long.TryParse(inner[1], out var b))
            { _memory.PushAns(ScientificFunctions.Lcm(a, b)); ShowResult(_memory.Ans); return; }
        }

        // Handle Pol/Rec
        if (expr.StartsWith("Pol(") && expr.EndsWith(")"))
        {
            var inner = expr[4..^1].Split(',');
            if (inner.Length == 2 && double.TryParse(inner[0], out var x) && double.TryParse(inner[1], out var y))
            { double r = Math.Sqrt(x * x + y * y); double θ = _angle.ToRadians(Math.Atan2(y, x)); _memory.PushAns(r); ShowResult(r); return; }
        }
        if (expr.StartsWith("Rec(") && expr.EndsWith(")"))
        {
            var inner = expr[4..^1].Split(',');
            if (inner.Length == 2 && double.TryParse(inner[0], out var r) && double.TryParse(inner[1], out var θ))
            { double x = r * Math.Cos(_angle.ToRadians(θ)); _memory.PushAns(x); ShowResult(x); return; }
        }

        // Handle ∫( integration
        if (expr.StartsWith("∫(") && expr.EndsWith(")"))
        {
            HandleIntegration(expr);
            return;
        }

        // Handle Σ summation
        if (expr.StartsWith("Σ(") && expr.EndsWith(")"))
        {
            HandleSummation(expr);
            return;
        }

        // Handle d/dx( derivative
        if (expr.StartsWith("d/dx(") && expr.EndsWith(")"))
        {
            HandleDerivative(expr);
            return;
        }

        // Handle RanInt( random integer
        if (expr.StartsWith("RanInt(") && expr.EndsWith(")"))
        {
            HandleRanInt(expr);
            return;
        }

        // Handle Int( truncation
        if (expr.StartsWith("Int(") && expr.EndsWith(")"))
        {
            HandleIntFunction(expr);
            return;
        }

        // Handle Intg( floor
        if (expr.StartsWith("Intg(") && expr.EndsWith(")"))
        {
            HandleIntgFunction(expr);
            return;
        }

        // solve(equation, guess) — one-variable numeric solving
        if (expr.StartsWith("solve(") && expr.EndsWith(")"))
        {
            HandleSolveFunction(expr);
            return;
        }

        try
        {
            double result = _engine.Evaluate(expr);
            _memory.PushAns(result);
            ShowResult(result);
            _history.Add($"{expr} = {_resultText}");
            _historyIndex = _history.Count;
            _justEvaluated = true;
            _hasError = false;
        }
        catch (CalculatorException ex)
        {
            ResultText = ex.Message;
            ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D));
            _hasError = true; _justEvaluated = false;
        }
        catch (Exception)
        {
            ResultText = "Math Error";
            ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D));
            _hasError = true; _justEvaluated = false;
        }
    }

    private void ShowResult(double value)
    {
        _lastNumericResult = value;
        ResultText = FormatNumber(value);
        ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
        ResultMode = ResultDisplayMode.Decimal;
    }

    private void HandleIntegration(string expr)
    {
        // ∫(f(X), a, b)
        var inner = expr[2..^1].Split(',');
        if (inner.Length >= 3)
        {
            try
            {
                string funcExpr = inner[0].Trim();
                double a = _engine.Evaluate(inner[1].Trim());
                double b = _engine.Evaluate(inner[2].Trim());
                double result = EquationSolverService.DefiniteIntegral(x =>
                {
                    _memory.Store("X", x);
                    return _engine.Evaluate(funcExpr.Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                }, a, b);
                _memory.PushAns(result);
                ShowResult(result);
            }
            catch { ResultText = "Math Error"; ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D)); _hasError = true; }
        }
    }

    private void HandleSummation(string expr)
    {
        // Σ(f(X), start, end)
        var inner = expr[2..^1].Split(',');
        if (inner.Length >= 3)
        {
            try
            {
                string funcExpr = inner[0].Trim();
                double start = _engine.Evaluate(inner[1].Trim());
                double end = _engine.Evaluate(inner[2].Trim());
                double result = EquationSolverService.Summation(x =>
                    _engine.Evaluate(funcExpr.Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture))),
                    start, end);
                _memory.PushAns(result);
                ShowResult(result);
            }
            catch { ResultText = "Math Error"; ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D)); _hasError = true; }
        }
    }

    private bool _pendingStore;
    private double _pendingStoreValue;

    private void HandleStore()
    {
        _pendingStoreValue = EvaluateCurrent();
        _pendingStore = true;
        IsAlphaActive = true;
        ShowInfo("STO _");
    }

    private void HandleVariableInput(string varName)
    {
        if (_pendingStore)
        {
            _memory.Store(varName, _pendingStoreValue);
            _pendingStore = false;
            ShowInfo($"{varName} stored");
        }
        else
        {
            InsertAtCursor(varName);
        }
    }

    private double EvaluateCurrent()
    {
        if (string.IsNullOrEmpty(_expression)) return _memory.Ans;
        try { return _engine.Evaluate(_expression); }
        catch { return _memory.Ans; }
    }

    private void ClearAll()
    {
        ExpressionText = "";
        ResultText = "0";
        ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
        _justEvaluated = false; _hasError = false;
        IsShiftActive = false; IsAlphaActive = false; IsHypActive = false;
        _pendingStore = false;
        _cursorIndex = 0;
        if (_isBaseNMode) OnPropertyChanged(nameof(IsBaseNActive));
        if (_isStatsMode) OnPropertyChanged(nameof(IsStatsActive));
        if (_isComplexMode) OnPropertyChanged(nameof(IsCmplxActive));
        if (_isMatrixMode) OnPropertyChanged(nameof(IsMatrixActive));
        if (_isEqnMode) OnPropertyChanged(nameof(IsEqnActive));
        if (_isTableMode) OnPropertyChanged(nameof(IsTableActive));
        _isStatsMode = false; _isComplexMode = false; _isBaseNMode = false;
        _isEqnMode = false; _isMatrixMode = false; _isVectorMode = false; _isTableMode = false;
        _tablePhase = 0; _tablePage = 0; _tableFunction = ""; _table.Rows.Clear();
        _showConstants = false; _showConversions = false; _showHistory = false;
        ResultMode = ResultDisplayMode.Decimal;
        _currentMode = CalculatorMode.COMP;
        SetupStatusText = "";
        if (_baseN.IsBaseActive)
        {
            _baseN.CurrentBase = BaseNService.NumBase.DEC;
            OnPropertyChanged(nameof(BaseLabel));
        }
    }

    private void ClearHyp()
    {
        if (_isHypActive) { _isHypActive = false; OnPropertyChanged(nameof(IsHypActive)); }
    }

    // ============ SHIFT / ALPHA Maps ============
    // ShiftMap and AlphaMap are defined in Models/KeyDefinition.cs (KeyMaps class).
    // Keys not present in a map fall through to their default switch behavior.

    // ============ Menus ============

    private void ToggleModeMenu()
    {
        ShowSetup = !_showSetup;
        if (_showSetup) { _setupPage = 0; OnPropertyChanged(nameof(CurrentSetupPage)); OnPropertyChanged(nameof(SetupDisplayItems)); SetupInfoVisible = false; }
    }

    private void HandleSetupInput(string input)
    {
        if (_setupPage == 0 && int.TryParse(input, out int n) && n >= 1 && n <= 8)
        {
            string[] modes = { "COMP", "CMPLX", "STAT", "BASE-N", "EQN", "MATRIX", "TABLE", "VECTOR" };
            string mode = modes[n - 1];
            _currentMode = (CalculatorMode)(n - 1);
            ShowSetup = false;
            SetupInfoText = $"Mode: {mode}";
            SetupInfoVisible = true;
            SetupStatusText = mode;
            ActivateMode(_currentMode);
            return;
        }
        if (_setupPage == 1 && int.TryParse(input, out int m) && m >= 1 && m <= 3)
        {
            if (m == 1) SetupInfoText = "Mode: INEQ";
            else if (m == 2) SetupInfoText = "Mode: VERIF";
            else SetupInfoText = "Mode: DIST";
            SetupInfoVisible = true; ShowSetup = false;
            return;
        }
        if (input == "RIGHT" && _setupPage == 0) { _setupPage = 1; OnPropertyChanged(nameof(CurrentSetupPage)); OnPropertyChanged(nameof(SetupDisplayItems)); }
        if (input == "LEFT" && _setupPage == 1) { _setupPage = 0; OnPropertyChanged(nameof(CurrentSetupPage)); OnPropertyChanged(nameof(SetupDisplayItems)); }
        if (input is "AC" or "ON" or "MODE") { ShowSetup = false; }
    }

    private void ActivateMode(CalculatorMode mode)
    {
        bool wasStats = _isStatsMode;
        _isStatsMode = mode == CalculatorMode.STAT;
        _isComplexMode = mode == CalculatorMode.CMPLX;
        bool wasBaseN = _isBaseNMode;
        _isBaseNMode = mode == CalculatorMode.BASE_N;
        _isEqnMode = mode == CalculatorMode.EQN;
        _isMatrixMode = mode == CalculatorMode.MATRIX;
        _isVectorMode = mode == CalculatorMode.VECTOR;
        _isTableMode = mode == CalculatorMode.TABLE;
        if (wasBaseN != _isBaseNMode) OnPropertyChanged(nameof(IsBaseNActive));
        if (wasStats != _isStatsMode) OnPropertyChanged(nameof(IsStatsActive));
        OnPropertyChanged(nameof(IsCmplxActive));
        OnPropertyChanged(nameof(IsEqnActive));
        OnPropertyChanged(nameof(IsMatrixActive));
        OnPropertyChanged(nameof(IsVectorActive));
        OnPropertyChanged(nameof(IsTableActive));
        _tablePhase = mode == CalculatorMode.TABLE ? 0 : 0;
        _tablePage = 0;
        _tableFunction = "";
        if (mode == CalculatorMode.TABLE) { ExpressionText = ""; ResultText = "Enter f(X)"; }
        if (mode == CalculatorMode.COMP) ClearAll();
    }

    // ============ About ============

    private void ToggleAbout()
    {
        _showAbout = !_showAbout;
        if (_showAbout) { _aboutPage = 0; ShowAbout = true; }
        else ShowAbout = false;
    }

    private void NavigateAbout(int dir)
    {
        if (!_showAbout) return;
        int next = _aboutPage + dir;
        if (next < 0 || next >= AboutPages.Length) return;
        _aboutPage = next;
        OnPropertyChanged(nameof(AboutPageText));
        OnPropertyChanged(nameof(AboutDisplayItems));
    }

    private void HandleAboutInput(string input)
    {
        if (input is "AC" or "ON") { ShowAbout = false; if (input == "ON") ClearAll(); return; }
        if (input is "UP") NavigateAbout(-1);
        if (input is "DOWN") NavigateAbout(1);
        if (input is "LEFT") NavigateAbout(-1);
        if (input is "RIGHT") NavigateAbout(1);
        if (input is "ABOUT") { ShowAbout = false; }
    }

    // ============ Sub-Menu Handlers ============

    private void HandleConstantsInput(string input)
    {
        if (int.TryParse(input, out int n) && n >= 1 && n <= ConstantLibraryService.Constants.Length)
        {
            var c = ConstantLibraryService.GetConstant(n - 1);
            InsertAtCursor(c.Symbol);
            ShowConstants = false;
        }
        if (input is "AC" or "ON") ShowConstants = false;
    }

    private void HandleConversionsInput(string input)
    {
        if (int.TryParse(input, out int n) && n >= 1 && n <= UnitConversionService.Conversions.Length)
        {
            double val = EvaluateCurrent();
            var (result, label) = UnitConversionService.Convert(n - 1, val, true);
            ResultText = $"{val:G4} → {result:G4} ({label})";
            ResultBrush = new(Color.FromRgb(0x64, 0x70, 0x62));
            ShowConversions = false;
        }
        if (input is "AC" or "ON") ShowConversions = false;
    }

    private void HandleHistoryInput(string input)
    {
        if (input is "UP") { _historyIndex = Math.Max(0, _historyIndex - 1); LoadHistory(); }
        if (input is "DOWN") { _historyIndex = Math.Min(_history.Count, _historyIndex + 1); LoadHistory(); }
        if (input is "AC" or "ON") ShowHistory = false;
        if (input is "=") ShowHistory = false;
    }

    private void LoadHistory()
    {
        if (_historyIndex >= 0 && _historyIndex < _history.Count)
        {
            var parts = _history[_historyIndex].Split(" = ");
            ExpressionText = parts[0];
            if (parts.Length > 1) ResultText = parts[1];
        }
    }

    private void NavigateHistory(int dir)
    {
        if (_history.Count == 0) return;
        _historyIndex = Math.Clamp(_historyIndex + dir, -1, _history.Count - 1);
        if (_historyIndex >= 0) LoadHistory();
        else ExpressionText = "";
    }

    // ============ Mode-Specific Handlers ============

    private void HandleStatsInput(string input)
    {
        if (input is "AC" or "ON")
        {
            if (_showStatsMenu) { _showStatsMenu = false; return; }
            _isStatsMode = false;
            if (_isStatsMode != false) OnPropertyChanged(nameof(IsStatsActive));
            ClearAll(); return;
        }
        if (input is "=" or "CALC")
        {
            ShowStatsResults();
            return;
        }
        if (input is "DEL")
        {
            if (_stats.Data.Values.Count > 0)
            {
                _stats.ClearLast();
                ShowInfo($"n={_stats.Count}");
            }
            return;
        }
        if (input is "MODE")
        {
            _isStatsMode = false;
            if (_isStatsMode != false) OnPropertyChanged(nameof(IsStatsActive));
            ClearAll();
            ToggleModeMenu();
            return;
        }

        // If in stats menu view, handle navigation and selection
        if (_showStatsMenu)
        {
            if (input is "UP") { _statsMenuPage = Math.Max(0, _statsMenuPage - 1); ShowStatsMenuPage(); return; }
            if (input is "DOWN") { _statsMenuPage = Math.Min(7, _statsMenuPage + 1); ShowStatsMenuPage(); return; }
            if (input is "AC" or "ON") { _showStatsMenu = false; return; }
            if (input is "=") { _showStatsMenu = false; ResultText = FormatStatsValue(_statsMenuPage); ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18)); return; }
            if (int.TryParse(input, out int n) && n >= 1 && n <= 8)
            {
                _statsMenuPage = n - 1;
                _showStatsMenu = false;
                ResultText = FormatStatsValue(_statsMenuPage);
                ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
                return;
            }
            return;
        }

        // Data entry mode
        if (input is ",")
        {
            ExpressionText += ",";
            _cursorIndex = _expression.Length;
            return;
        }
        if (input is "-" or "−")
        {
            ExpressionText += "-";
            _cursorIndex = _expression.Length;
            return;
        }
        if (input == ".")
        {
            ExpressionText += ".";
            _cursorIndex = _expression.Length;
            return;
        }
        if (input is "0" or "1" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9")
        {
            ExpressionText += input;
            _cursorIndex = _expression.Length;
            return;
        }
    }

    private void ShowStatsResultValue(double val)
    {
        ResultText = FormatNumber(val);
        ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
    }

    private string FormatStatsValue(int index)
    {
        double val = _stats.GetResult(index);
        if (double.IsNaN(val)) return $"{StatisticsService.ResultLabels[index]}: N/A";
        return $"{StatisticsService.ResultLabels[index]} = {FormatNumber(val)}";
    }

    private void ShowStatsMenuPage()
    {
        var items = _stats.GetMenuItems();
        if (_statsMenuPage >= 0 && _statsMenuPage < items.Length)
        {
            ResultText = items[_statsMenuPage];
            ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
        }
    }

    private void ShowStatsResults()
    {
        if (_stats.Data.Values.Count == 0)
        {
            ShowInfo("No data");
            return;
        }
        _showStatsMenu = true;
        _statsMenuPage = 0;
        ShowStatsMenuPage();
    }

    private void HandleBaseNInput(string input)
    {
        if (input is "AC" or "ON") { _isBaseNMode = false; ClearAll(); return; }
        if (input is "DEC" or "HEX" or "BIN" or "OCT")
        {
            _baseN.CurrentBase = input switch { "DEC" => BaseNService.NumBase.DEC, "HEX" => BaseNService.NumBase.HEX,
                "BIN" => BaseNService.NumBase.BIN, _ => BaseNService.NumBase.OCT };
            OnPropertyChanged(nameof(BaseLabel));
            ShowInfo(_baseN.BaseLabel); return;
        }
        if (input is "=" or "CALC")
        {
            EvaluateBaseN();
            return;
        }
        if (input == ".")
        {
            ShowInfo("Base-N integer only");
            return;
        }
        // Validate digit against current base
        if (input.Length == 1 && !_baseN.IsValidChar(input[0]))
        {
            ShowInfo($"Invalid {_baseN.BaseLabel} digit");
            return;
        }
        if (input is "+" or "−" or "-" or "×" or "÷" or "*" or "/")
        {
            InsertAtCursor($" {input} ");
            return;
        }
        // Bitwise operation keywords (insert with spacing)
        if (input is "AND" or "OR" or "XOR" or "NOT")
        {
            InsertAtCursor($" {input} ");
            return;
        }
        if (input is "BASE") { return; } // Already in BASE-N mode
        if (input is "HYP")
        {
            IsHypActive = !_isHypActive; ShowInfo(_isHypActive ? "HYP" : "");
            return;
        }
        InsertAtCursor(input);
    }

    private void EvaluateBaseN()
    {
        string expr = _expression.Trim();
        if (string.IsNullOrEmpty(expr)) { ResultText = "0"; ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18)); return; }

        // Validate all chars are valid for current base
        if (!_baseN.IsValidExpression(expr))
        {
            ResultText = $"Invalid {_baseN.BaseLabel} digit";
            ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D));
            _hasError = true;
            return;
        }

        try
        {
            long result = EvaluateBaseNExpression(expr);
            ResultText = _baseN.ToSignedBase(result);
            ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
            _hasError = false;
            _justEvaluated = true;
            _memory.PushAns(result);
            _history.Add($"{expr} = {_resultText}");
            _historyIndex = _history.Count;
        }
        catch (Exception ex)
        {
            ResultText = ex.Message;
            ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D));
            _hasError = true;
            _justEvaluated = false;
        }
    }

    private long EvaluateBaseNExpression(string expr)
    {
        // Tokenize: split by spaces to get tokens (operators are space-delimited)
        string[] parts = expr.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0) return 0;

        // Handle NOT prefix: "NOT <value>"
        if (parts[0].Equals("NOT", StringComparison.OrdinalIgnoreCase))
        {
            if (parts.Length < 2) throw new CalculatorException("Syntax Error");
            long val = _baseN.FromString(parts[1]);
            return BaseNService.Not(val);
        }

        // Evaluate left-to-right for simplicity (no precedence)
        long result = _baseN.FromString(parts[0]);
        for (int i = 1; i < parts.Length; i += 2)
        {
            if (i >= parts.Length) throw new CalculatorException("Syntax Error");
            string op = parts[i].ToUpperInvariant();
            if (i + 1 >= parts.Length) throw new CalculatorException("Syntax Error");
            long right = _baseN.FromString(parts[i + 1]);

            switch (op)
            {
                case "+": result += right; break;
                case "-": case "−": result -= right; break;
                case "×": case "*": result *= right; break;
                case "÷": case "/":
                    if (right == 0) throw new CalculatorException("Cannot divide by zero");
                    result /= right; // integer division
                    break;
                case "AND": result = BaseNService.And(result, right); break;
                case "OR": result = BaseNService.Or(result, right); break;
                case "XOR": result = BaseNService.Xor(result, right); break;
                default: throw new CalculatorException($"Unknown op: {op}");
            }
        }
        return result;
    }

    private void HandleTableInput(string input)
    {
        if (input is "SHIFT" or "ALPHA") return;

        // AC: go back one phase or exit
        if (input is "AC" or "ON")
        {
            if (_tablePhase == 4) { _tablePhase = 0; _tablePage = 0; _table.Rows.Clear(); ExpressionText = ""; ResultText = "Enter f(X)"; return; }
            if (_tablePhase >= 1) { _tablePhase--; ExpressionText = ""; ResultText = PhasePrompt(_tablePhase); return; }
            _isTableMode = false; ClearAll(); return;
        }

        // = key: advance phase or generate
        if (input is "=" or "CALC")
        {
            if (_tablePhase == 0)
            {
                _tableFunction = _expression.Trim();
                if (string.IsNullOrWhiteSpace(_tableFunction)) { ShowInfo("Enter f(X) first"); return; }
                if (!_tableFunction.Contains("X") && !_tableFunction.Contains("x"))
                { ShowInfo("Function must use X"); return; }
                _tablePhase = 1; ExpressionText = ""; ResultText = "Start?"; return;
            }
            if (_tablePhase == 1)
            {
                if (!double.TryParse(_expression, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double sv))
                { ShowInfo("Invalid start"); return; }
                _table.Start = sv; _tablePhase = 2; ExpressionText = ""; ResultText = "End?"; return;
            }
            if (_tablePhase == 2)
            {
                if (!double.TryParse(_expression, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double ev))
                { ShowInfo("Invalid end"); return; }
                _table.End = ev; _tablePhase = 3; ExpressionText = ""; ResultText = "Step?"; return;
            }
            if (_tablePhase == 3)
            {
                if (!double.TryParse(_expression, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double sv2))
                { ShowInfo("Invalid step"); return; }
                if (Math.Abs(sv2) < 1e-12) { ShowInfo("Step cannot be zero"); return; }
                _table.Step = sv2; _table.FunctionText = _tableFunction;
                bool ok = _table.Generate(x => _engine.Evaluate(
                    _tableFunction.Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture))));
                if (!ok) { ShowInfo(_table.ErrorMessage); return; }
                _tablePhase = 4; _tablePage = 0;
                ExpressionText = $"f(X)={_tableFunction}";
                ShowTablePage(); return;
            }
            if (_tablePhase == 4)
            {
                _tablePhase = 0; _tablePage = 0; _table.Rows.Clear(); ExpressionText = ""; ResultText = "Enter f(X)"; return;
            }
            return;
        }

        // In result view (phase 4): navigation
        if (_tablePhase == 4)
        {
            if (input is "UP") { _tablePage = Math.Max(0, _tablePage - 1); ShowTablePage(); return; }
            if (input is "DOWN") { _tablePage = Math.Min(_table.DisplayCount - 1, _tablePage + 1); ShowTablePage(); return; }
            if (input is "DEL") { _tablePhase = 3; ExpressionText = FormatNumber(_table.Step); ResultText = "Step?"; return; }
            return;
        }

        // Phases 1-3: number entry (like STAT data entry)
        if (input is "-" or "−") { ExpressionText += "-"; _cursorIndex = _expression.Length; return; }
        if (input == ".") { ExpressionText += "."; _cursorIndex = _expression.Length; return; }
        if (input is "0" or "1" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9")
        { ExpressionText += input; _cursorIndex = _expression.Length; return; }
        if (input is "DEL")
        {
            if (_expression.Length > 0) { ExpressionText = _expression[..^1]; _cursorIndex = _expression.Length; }
            return;
        }
    }

    private string PhasePrompt(int phase) => phase switch
    {
        0 => "Enter f(X)", 1 => "Start?", 2 => "End?", 3 => "Step?", _ => ""
    };

    private void ShowTablePage()
    {
        var items = _table.GetDisplayItems();
        if (_tablePage >= 0 && _tablePage < items.Length)
        {
            ResultText = items[_tablePage];
            ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
        }
    }

    // ============ Fraction Toggle ============

    private void ToggleFractionDisplay()
    {
        // Use the last numeric result — do NOT re-evaluate the expression
        double val = _lastNumericResult;

        // If no result yet, do nothing
        if (val == 0 && ResultText is "0" or "")
        {
            ShowInfo("No result");
            return;
        }

        // Cycle: Decimal -> InlineFraction -> StackedFraction -> Decimal
        ResultMode = _resultDisplayMode switch
        {
            ResultDisplayMode.Decimal => ResultDisplayMode.InlineFraction,
            ResultDisplayMode.InlineFraction => ResultDisplayMode.StackedFraction,
            _ => ResultDisplayMode.Decimal
        };

        if (_resultDisplayMode == ResultDisplayMode.Decimal)
        {
            ResultText = FormatNumber(val);
            ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
            return;
        }

        try
        {
            var frac = FractionValue.FromDouble(val);

            // If result is a whole number (no fraction), stay in decimal mode
            if (frac.Numerator == 0)
            {
                ResultMode = ResultDisplayMode.Decimal;
                ResultText = FormatNumber(val);
                ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
                return;
            }

            FracNumerator = frac.Numerator;
            FracDenominator = frac.Denominator;
            FracSignDisplay = frac.IsNegative ? "-" : "";

            if (_resultDisplayMode == ResultDisplayMode.InlineFraction)
            {
                FracInlineText = frac.ToString();
                ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
                ResultText = "";
            }
            else // StackedFraction
            {
                ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
                ResultText = "";
            }
        }
        catch
        {
            ResultMode = ResultDisplayMode.Decimal;
            ResultText = FormatNumber(val);
            ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
        }
    }

    // ============ DMS Conversion ============

    private void HandleDMS()
    {
        double val = EvaluateCurrent();
        if (val == 0 && _expression.Length > 0)
        {
            try { val = _engine.Evaluate(_expression); }
            catch { }
        }
        if (val == 0) { ShowInfo("DMS"); return; }
        int deg = (int)Math.Floor(val);
        double remain = (val - deg) * 60;
        int min = (int)Math.Floor(remain);
        double sec = (remain - min) * 60;
        ResultText = $"{deg}°{min}′{sec:F1}″";
        ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18));
    }

    // ============ Info Display ============

    private void ShowInfo(string msg)
    {
        if (_resultDisplayMode != ResultDisplayMode.Decimal)
            ResultMode = ResultDisplayMode.Decimal;
        if (string.IsNullOrEmpty(msg))
        { ResultText = "0"; ResultBrush = new(Color.FromRgb(0x18, 0x20, 0x18)); }
        else { ResultText = msg; ResultBrush = new(Color.FromRgb(0x2D, 0x4B, 0x2D)); }
    }

    // ============ Phase 1 Helper Methods ============

    private void HandleInverse()
    {
        // x⁻¹: compute reciprocal of current expression
        double val = EvaluateCurrent();
        if (val == 0) { ShowInfo("Cannot divide by zero"); return; }
        double result = 1.0 / val;
        _memory.PushAns(result);
        ShowResult(result);
    }

    private void HandleDerivative(string expr)
    {
        // d/dx(f(X), a) — numeric central difference derivative
        var inner = expr[5..^1].Split(',');
        if (inner.Length >= 2)
        {
            try
            {
                string funcExpr = inner[0].Trim();
                double a = _engine.Evaluate(inner[1].Trim());
                double h = 1e-6;
                double f1 = _engine.Evaluate(funcExpr.Replace("X", (a + h).ToString(System.Globalization.CultureInfo.InvariantCulture)));
                double f2 = _engine.Evaluate(funcExpr.Replace("X", (a - h).ToString(System.Globalization.CultureInfo.InvariantCulture)));
                double result = (f1 - f2) / (2 * h);
                _memory.PushAns(result);
                ShowResult(result);
            }
            catch { ResultText = "Math Error"; ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D)); _hasError = true; }
        }
    }

    private void HandleRanInt(string expr)
    {
        // RanInt(a,b) — random integer between a and b inclusive
        var inner = expr[7..^1].Split(',');
        if (inner.Length == 2 && int.TryParse(inner[0], out int a) && int.TryParse(inner[1], out int b))
        {
            _memory.PushAns(ScientificFunctions.RanInt(a, b));
            ShowResult(_memory.Ans);
        }
        else { ShowInfo("RanInt(a,b)"); }
    }

    private void HandleIntFunction(string expr)
    {
        // Int(x) — truncate toward zero
        var inner = expr[4..^1];
        try
        {
            double val = _engine.Evaluate(inner.Trim());
            double result = Math.Truncate(val);
            _memory.PushAns(result);
            ShowResult(result);
        }
        catch { ResultText = "Math Error"; ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D)); _hasError = true; }
    }

    private void HandleIntgFunction(string expr)
    {
        // Intg(x) — greatest integer ≤ x (floor)
        var inner = expr[5..^1];
        try
        {
            double val = _engine.Evaluate(inner.Trim());
            double result = Math.Floor(val);
            _memory.PushAns(result);
            ShowResult(result);
        }
        catch { ResultText = "Math Error"; ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D)); _hasError = true; }
    }

    private void HandleSolveFunction(string expr)
    {
        // solve(equation, guess) or solve(equation)
        try
        {
            string inner = expr[6..^1].Trim();
            string equation, guessStr;
            int commaPos = -1;
            int depth = 0;
            for (int i = 0; i < inner.Length; i++)
            {
                if (inner[i] == '(') depth++;
                else if (inner[i] == ')') depth--;
                else if (inner[i] == ',' && depth == 0) { commaPos = i; break; }
            }

            double guess = 0;
            if (commaPos >= 0)
            {
                equation = inner[..commaPos].Trim();
                guessStr = inner[(commaPos + 1)..].Trim();
                if (!double.TryParse(guessStr, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out guess))
                { ResultText = "Invalid guess"; ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D)); _hasError = true; return; }
            }
            else
            {
                equation = inner;
            }

            if (!equation.Contains("X") && !equation.Contains("x"))
            { ResultText = "Equation must use X"; ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D)); _hasError = true; return; }

            // Split on = to get LHS and RHS
            int eqPos = equation.IndexOf('=');
            string leftExpr, rightExpr;
            if (eqPos >= 0)
            {
                leftExpr = equation[..eqPos].Trim();
                rightExpr = equation[(eqPos + 1)..].Trim();
            }
            else
            {
                leftExpr = equation;
                rightExpr = "0";
            }

            double result = EquationSolverService.Solve(x =>
            {
                string xStr = x.ToString(System.Globalization.CultureInfo.InvariantCulture);
                double l = _engine.Evaluate(leftExpr.Replace("X", xStr).Replace("x", xStr));
                double r = _engine.Evaluate(rightExpr.Replace("X", xStr).Replace("x", xStr));
                return l - r;
            }, guess);

            _memory.PushAns(result);
            ShowResult(result);
            _history.Add($"{expr} = {_resultText}");
            _historyIndex = _history.Count;
            _justEvaluated = true;
            _hasError = false;
        }
        catch (InvalidOperationException ex)
        {
            ResultText = ex.Message;
            ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D));
            _hasError = true;
        }
        catch (Exception)
        {
            ResultText = "Solve Error";
            ResultBrush = new(Color.FromRgb(0x7B, 0x2D, 0x2D));
            _hasError = true;
        }
    }

    private void ShowNotAvailable()
    {
        ShowInfo("N/A");
    }

    // ============ Formatting ============

    private static string FormatNumber(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value)) return "Math Error";
        return FormattingService.Format(value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
