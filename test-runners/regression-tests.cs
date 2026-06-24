using SmartCalculator.Models;
using SmartCalculator.Services;

int passed = 0, failed = 0;
void T(string n, string a, string e) { if (a == e) { passed++; } else { failed++; Console.WriteLine($"  FAIL: {n} - expected '{e}', got '{a}'"); } }
void Td(string n, double a, double e) { if (double.IsNaN(a) && double.IsNaN(e)) { passed++; } else if (Math.Abs(a - e) < 1e-10) { passed++; } else { failed++; Console.WriteLine($"  FAIL: {n} - expected {e}, got {a}"); } }
void Tb(string n, bool a) { if (a) { passed++; } else { failed++; Console.WriteLine($"  FAIL: {n} - expected true"); } }
void Te(string n, string a, string p) { if (a.StartsWith(p)) { passed++; } else { failed++; Console.WriteLine($"  FAIL: {n} - expected start '{p}', got '{a}'"); } }

// ============ Phase 1: Engine ============
Console.WriteLine("\n=== Phase 1: Engine Arithmetic ===");
var engine = new CalculatorEngine { Memory = new MemoryService(), Angle = new AngleService() };
Td("2+3", engine.Evaluate("2+3"), 5);
Td("10-4", engine.Evaluate("10-4"), 6);
Td("3×5", engine.Evaluate("3*5"), 15); // engine uses * not ×
Td("15÷3", engine.Evaluate("15/3"), 5);
Td("2+3*4", engine.Evaluate("2+3*4"), 14);
Td("(2+3)*4", engine.Evaluate("(2+3)*4"), 20);
Td("2^10", engine.Evaluate("2^10"), 1024);
Td("0.5+0.25", engine.Evaluate("0.5+0.25"), 0.75);
Td("pi", engine.Evaluate("pi"), Math.PI);
Td("e", engine.Evaluate("e"), Math.E);
Td("√(9)", engine.Evaluate("√(9)"), 3);
Td("sin(90)", engine.Evaluate("sin(90)"), 1);
Td("cos(0)", engine.Evaluate("cos(0)"), 1);
Td("log(1000)", engine.Evaluate("log(1000)"), 3);
Td("ln(e)", engine.Evaluate("ln(e)"), 1);
Td("fact(5)", engine.Evaluate("fact(5)"), 120);
Td("abs(-5)", engine.Evaluate("abs(-5)"), 5);

Console.WriteLine("\n=== Phase 1: Error Handling ===");
try { engine.Evaluate("1/0"); failed++; Console.WriteLine("  FAIL: div by 0 should throw"); } catch { passed++; }
try { engine.Evaluate("√(-1)"); failed++; Console.WriteLine("  FAIL: √(-1) should throw"); } catch { passed++; }
try { engine.Evaluate("log(0)"); failed++; Console.WriteLine("  FAIL: log(0) should throw"); } catch { passed++; }

Console.WriteLine("\n=== Phase 1: Memory Service ===");
var mem = new MemoryService();
mem.Store("X", 42); Td("Store X=42", mem.Recall("X"), 42);
mem.Store("X", 100); Td("Overwrite X=100", mem.Recall("X"), 100);
mem.PushAns(5); mem.AddToM(5); Td("M+", mem.Recall("M"), 5);
mem.PushAns(3); mem.SubtractFromM(3); Td("M-", mem.Recall("M"), 2);

Console.WriteLine("\n=== Phase 1: Fraction Value ===");
T("0.75→3/4", FractionValue.FromDouble(0.75).ToString(), "3/4");
T("0.5→1/2", FractionValue.FromDouble(0.5).ToString(), "1/2");
Td("3/4→0.75", FractionValue.FromDouble(0.75).ToDouble(), 0.75);

Console.WriteLine("\n=== Phase 1: Angle Service ===");
var angle = new AngleService();
angle.CurrentMode = AngleMode.DEG; Td("DEG ToRad(90)", angle.ToRadians(90), Math.PI / 2);
angle.CurrentMode = AngleMode.RAD; Td("RAD identity", angle.ToRadians(1.0), 1.0);
angle.CurrentMode = AngleMode.GRAD; Td("GRAD ToRad(100)", angle.ToRadians(100), Math.PI / 2);

// ============ BASE-N ============
Console.WriteLine("\n=== BASE-N ===");
var bn = new BaseNService();
bn.CurrentBase = BaseNService.NumBase.BIN; Tb("BIN IsValidChar 0", bn.IsValidChar('0'));
bn.CurrentBase = BaseNService.NumBase.BIN; Tb("BIN invalid 2", !bn.IsValidChar('2'));
bn.CurrentBase = BaseNService.NumBase.HEX; Tb("HEX IsValidChar F", bn.IsValidChar('F'));
bn.CurrentBase = BaseNService.NumBase.HEX; Tb("HEX invalid G", !bn.IsValidChar('G'));
bn.CurrentBase = BaseNService.NumBase.OCT; Tb("OCT IsValidChar 7", bn.IsValidChar('7'));
bn.CurrentBase = BaseNService.NumBase.OCT; Tb("OCT invalid 8", !bn.IsValidChar('8'));
bn.CurrentBase = BaseNService.NumBase.DEC; Tb("DEC IsValid 1010", bn.IsValidExpression("1010"));
bn.CurrentBase = BaseNService.NumBase.BIN; Tb("BIN IsValid 1010", bn.IsValidExpression("1010"));
bn.CurrentBase = BaseNService.NumBase.BIN; Tb("BIN invalid 123", !bn.IsValidExpression("123"));
bn.CurrentBase = BaseNService.NumBase.HEX; Tb("HEX IsValid A", bn.IsValidExpression("A"));
bn.CurrentBase = BaseNService.NumBase.HEX; T("HEX ToSigned -1", bn.ToSignedBase(-1), "-1");
bn.CurrentBase = BaseNService.NumBase.BIN; T("BIN ToSigned -1", bn.ToSignedBase(-1), "-1");
bn.CurrentBase = BaseNService.NumBase.OCT; T("OCT ToSigned -1", bn.ToSignedBase(-1), "-1");
bn.CurrentBase = BaseNService.NumBase.HEX; T("HEX strip", bn.StripInvalidChars("ABG"), "AB");
bn.CurrentBase = BaseNService.NumBase.BIN; T("BIN strip", bn.StripInvalidChars("12034"), "10");
bn.CurrentBase = BaseNService.NumBase.HEX; T("HEX base label", bn.BaseLabel, "HEX");
bn.CurrentBase = BaseNService.NumBase.BIN; T("BIN base label", bn.BaseLabel, "BIN");

// ============ STAT ============
Console.WriteLine("\n=== STAT ===");
var ss = new StatisticsService();
Tb("STAT count 0", ss.Count == 0);
Tb("STAT add 10", ss.TryParseEntry("10"));
Tb("STAT add 20,30", ss.TryParseEntry("20,30"));
Tb("STAT count 3", ss.Count == 3);
Td("STAT mean", ss.GetResult(3), 20);
Td("STAT n", ss.GetResult(0), 3);
Td("STAT sum", ss.GetResult(1), 60);
Td("STAT min", ss.GetResult(6), 10);
Td("STAT max", ss.GetResult(7), 30);
ss.ClearLast(); Td("STAT clear last count", ss.Count, 2);
T("STAT menu count", ss.GetMenuItems().Length.ToString(), "8");
ss.Clear(); Tb("STAT clear", ss.Count == 0);
Tb("STAT invalid entry", !ss.TryParseEntry("abc"));
Tb("STAT error msg", ss.ErrorMessage.Length > 0);

// ============ CMPLX ============
Console.WriteLine("\n=== CMPLX ===");
T("1+2i", new ComplexValue(1, 2).ToFormattedString(), "1+2i");
T("3-4i", new ComplexValue(3, -4).ToFormattedString(), "3-4i");
T("i", new ComplexValue(0, 1).ToFormattedString(), "i");
T("-i", new ComplexValue(0, -1).ToFormattedString(), "-i");
T("5", new ComplexValue(5, 0).ToFormattedString(), "5");
T("0", new ComplexValue(0, 0).ToFormattedString(), "0");
T("5i", new ComplexValue(0, 5).ToFormattedString(), "5i");
T("-5i", new ComplexValue(0, -5).ToFormattedString(), "-5i");
T("-3+4i", new ComplexValue(-3, 4).ToFormattedString(), "-3+4i");
T("-3-4i", new ComplexValue(-3, -4).ToFormattedString(), "-3-4i");
T("Add", (new ComplexValue(1, 2) + new ComplexValue(3, 4)).ToFormattedString(), "4+6i");
T("Sub", (new ComplexValue(3, 4) - new ComplexValue(1, 2)).ToFormattedString(), "2+2i");
T("Mul", (new ComplexValue(1, 2) * new ComplexValue(3, 4)).ToFormattedString(), "-5+10i");
T("Div", (new ComplexValue(3, 4) / new ComplexValue(1, 2)).ToFormattedString(), "2.2-0.4i");
T("Conj", new ComplexValue(3, 4).Conjugate.ToFormattedString(), "3-4i");
Td("Mag", new ComplexValue(3, 4).Magnitude, 5);
Td("Arg", new ComplexValue(0, 1).Argument, Math.PI / 2);
T("1+2i", ComplexService.EvaluateToString("1+2i"), "1+2i");
T("3-4i", ComplexService.EvaluateToString("3-4i"), "3-4i");
T("5i", ComplexService.EvaluateToString("5i"), "5i");
T("i", ComplexService.EvaluateToString("i"), "i");
T("2i", ComplexService.EvaluateToString("2i"), "2i");
T("3", ComplexService.EvaluateToString("3"), "3");
T("(1+2i)+(3+4i)", ComplexService.EvaluateToString("(1+2i)+(3+4i)"), "4+6i");
T("(1+2i)*(3+4i)", ComplexService.EvaluateToString("(1+2i)*(3+4i)"), "-5+10i");
T("abs(3+4i)", ComplexService.EvaluateToString("abs(3+4i)"), "5");
T("conj(3+4i)", ComplexService.EvaluateToString("conj(3+4i)"), "3-4i");
T("i*i", ComplexService.EvaluateToString("i*i"), "-1");
Tb("NeedsComplex", ComplexService.NeedsComplexEvaluation("1+2i"));
Tb("Not complex", !ComplexService.NeedsComplexEvaluation("123"));
Te("Empty error", ComplexService.EvaluateToString(""), "Math Error");
T("DivByZero", ComplexService.EvaluateToString("1/0i"), "Cannot divide by zero");

// ============ MATRIX ============
Console.WriteLine("\n=== MATRIX ===");
T("[[1,2],[3,4]]", MatrixService.EvaluateToString("[[1,2],[3,4]]"), "[[1,2],[3,4]]");
T("3x3", MatrixService.EvaluateToString("[[1,2,3],[4,5,6],[7,8,9]]"), "[[1,2,3],[4,5,6],[7,8,9]]");
Te("invalid row", MatrixService.EvaluateToString("[[1,2],[3,4,5]]"), "Math Error");
Te("invalid val", MatrixService.EvaluateToString("[[abc]]"), "Math Error");
Te("empty", MatrixService.EvaluateToString(""), "Math Error");
var mA = MatrixService.ParseMatrix("[[1,2],[3,4]]");
var mB = MatrixService.ParseMatrix("[[5,6],[7,8]]");
var mC = MatrixService.ParseMatrix("[[1,2,3],[0,1,4],[5,6,0]]");
MatrixService.Store("MatA", mA); MatrixService.Store("MatB", mB); MatrixService.Store("MatC", mC);
T("MatA+MatB", MatrixService.EvaluateToString("MatA + MatB"), "[[6,8],[10,12]]");
T("MatB-MatA", MatrixService.EvaluateToString("MatB - MatA"), "[[4,4],[4,4]]");
T("MatA*MatB", MatrixService.EvaluateToString("MatA * MatB"), "[[19,22],[43,50]]");
T("det(MatA)", MatrixService.EvaluateToString("det(MatA)"), "-2");
T("transpose(MatA)", MatrixService.EvaluateToString("transpose(MatA)"), "[[1,3],[2,4]]");
T("trn(MatA)", MatrixService.EvaluateToString("trn(MatA)"), "[[1,3],[2,4]]");
T("inv(MatA)", MatrixService.EvaluateToString("inv(MatA)"), "[[-2,1],[1.5,-0.5]]");
T("det(MatC)", MatrixService.EvaluateToString("det(MatC)"), "1");
T("2*MatA", MatrixService.EvaluateToString("2 * MatA"), "[[2,4],[6,8]]");
Te("det non-square", MatrixService.EvaluateToString("det([[1,2]])"), "Math Error");
Te("inv singular", MatrixService.EvaluateToString("inv([[1,0],[0,0]])"), "Math Error");
Te("dim mismatch", MatrixService.EvaluateToString("MatA + [[1,2,3],[4,5,6]]"), "Math Error");
Te("invalid syntax", MatrixService.EvaluateToString("[[1,2"), "Math Error");
T("1x1", MatrixService.EvaluateToString("[[5]]"), "[[5]]");
T("det(1x1)", MatrixService.EvaluateToString("det([[5]])"), "5");

// ============ VECTOR ============
Console.WriteLine("\n=== VECTOR ===");
T("[1,2,3]", VectorService.EvaluateToString("[1,2,3]"), "[1,2,3]");
T("[4,5,6]", VectorService.EvaluateToString("[4,5,6]"), "[4,5,6]");
T("2D [1,2]", VectorService.EvaluateToString("[1,2]"), "[1,2]");
Te("invalid syntax", VectorService.EvaluateToString("[1,2,"), "Math Error");
Te("empty", VectorService.EvaluateToString("[]"), "Math Error");
Te("4D", VectorService.EvaluateToString("[1,2,3,4]"), "Math Error");
Te("non-numeric", VectorService.EvaluateToString("[abc]"), "Math Error");
Te("non-numeric 2", VectorService.EvaluateToString("[1,abc]"), "Math Error");
VectorService.Store("VecA", VectorService.ParseVector("[1,2,3]"));
VectorService.Store("VecB", VectorService.ParseVector("[4,5,6]"));
T("VecA+VecB", VectorService.EvaluateToString("VecA + VecB"), "[5,7,9]");
T("VecB-VecA", VectorService.EvaluateToString("VecB - VecA"), "[3,3,3]");
T("2*VecA", VectorService.EvaluateToString("2 * VecA"), "[2,4,6]");
T("VecA*2", VectorService.EvaluateToString("VecA * 2"), "[2,4,6]");
Td("dot(VecA,VecB)", double.Parse(VectorService.EvaluateToString("dot(VecA, VecB)"), System.Globalization.CultureInfo.InvariantCulture), 32);
T("cross(VecA,VecB)", VectorService.EvaluateToString("cross(VecA, VecB)"), "[-3,6,-3]");
{ double mag = double.Parse(VectorService.EvaluateToString("mag(VecA)"), System.Globalization.CultureInfo.InvariantCulture); if (Math.Abs(mag - 3.7416573867739413) < 1e-9) passed++; else { failed++; Console.WriteLine($"  FAIL: mag(VecA) - expected {3.7416573867739413}, got {mag}"); } }
{ double nrm = double.Parse(VectorService.EvaluateToString("norm(VecA)"), System.Globalization.CultureInfo.InvariantCulture); if (Math.Abs(nrm - 3.7416573867739413) < 1e-9) passed++; else { failed++; Console.WriteLine($"  FAIL: norm(VecA) - expected {3.7416573867739413}, got {nrm}"); } }
T("unit([3,0,0])", VectorService.EvaluateToString("unit([3,0,0])"), "[1,0,0]");
Td("mag([3,4])", double.Parse(VectorService.EvaluateToString("mag([3,4])"), System.Globalization.CultureInfo.InvariantCulture), 5);
T("unit([3,4])", VectorService.EvaluateToString("unit([3,4])"), "[0.6,0.8]");
T("[1,2]+[3,4]", VectorService.EvaluateToString("[1,2] + [3,4]"), "[4,6]");
T("VecA[10,20,30]", VectorService.EvaluateToString("VecA[10,20,30]"), "VecA = [10,20,30]");
T("A[1,2]", VectorService.EvaluateToString("A[1,2]"), "VecA = [1,2]");
Tb("VecA now 2D", VectorService.ParseVector(VectorService.EvaluateToString("VecA")).Dimensions == 2);
Td("dot 2D×3D", double.Parse(VectorService.EvaluateToString("dot(A, B)"), System.Globalization.CultureInfo.InvariantCulture), 14);
VectorService.Store("VecA", VectorService.ParseVector("[1,2,3]"));
VectorService.Store("VecB", VectorService.ParseVector("[4,5,6]"));
Te("cross 2D", VectorService.EvaluateToString("cross([1,2], [3,4])"), "Math Error");
Te("unit zero 2D", VectorService.EvaluateToString("unit([0,0])"), "Math Error");
Te("unit zero 3D", VectorService.EvaluateToString("unit([0,0,0])"), "Math Error");
Te("dim mismatch", VectorService.EvaluateToString("[1,2] + [1,2,3]"), "Math Error");
Te("undefined A", VectorService.EvaluateToString("novec"), "Math Error");

// ============ TABLE ============
Console.WriteLine("\n=== TABLE ===");
var tbl = new TableService();
var tblEngine = new CalculatorEngine { Memory = new MemoryService(), Angle = new AngleService() };
tbl.FunctionText = "X^2"; tbl.Start = 1; tbl.End = 3; tbl.Step = 1;
Tb("TABLE gen X^2", tbl.Generate(x => tblEngine.Evaluate("X^2".Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture)))));
Td("TABLE X=1", tbl.Rows[0].Y, 1);
Td("TABLE X=2", tbl.Rows[1].Y, 4);
Td("TABLE X=3", tbl.Rows[2].Y, 9);

tbl.FunctionText = "2*X+1"; tbl.Start = 0; tbl.End = 2; tbl.Step = 1;
Tb("TABLE gen 2X+1", tbl.Generate(x => tblEngine.Evaluate("2*X+1".Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture)))));
Td("TABLE 2X+1 X=0", tbl.Rows[0].Y, 1);
Td("TABLE 2X+1 X=1", tbl.Rows[1].Y, 3);
Td("TABLE 2X+1 X=2", tbl.Rows[2].Y, 5);

tbl.FunctionText = "X^3"; tbl.Start = 1; tbl.End = 3; tbl.Step = 1;
Tb("TABLE gen X^3", tbl.Generate(x => tblEngine.Evaluate("X^3".Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture)))));
Td("TABLE X^3 X=1", tbl.Rows[0].Y, 1);
Td("TABLE X^3 X=2", tbl.Rows[1].Y, 8);
Td("TABLE X^3 X=3", tbl.Rows[2].Y, 27);

tbl.FunctionText = "sin(X)"; tbl.Start = 0; tbl.End = 90; tbl.Step = 45;
tblEngine.Angle!.CurrentMode = AngleMode.DEG;
Tb("TABLE gen sin(X) DEG", tbl.Generate(x => tblEngine.Evaluate("sin(X)".Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture)))));
Td("TABLE sin(0)", tbl.Rows[0].Y, 0);
{ double sv = tbl.Rows[1].Y; if (Math.Abs(sv - 0.7071067811865476) < 1e-10) passed++; else { failed++; Console.WriteLine($"  FAIL: TABLE sin(45) - expected {0.7071067811865476}, got {sv}"); } }
Td("TABLE sin(90)", tbl.Rows[2].Y, 1);

// Validation
tbl.FunctionText = ""; tbl.Start = 1; tbl.End = 3; tbl.Step = 1;
Tb("TABLE empty func", !tbl.Generate(x => x));

tbl.FunctionText = "X"; tbl.Start = 1; tbl.End = 3; tbl.Step = 0;
Tb("TABLE step zero", !tbl.Generate(x => x));
Tb("TABLE step zero msg", tbl.ErrorMessage.Contains("zero"));

tbl.FunctionText = "X"; tbl.Start = 1; tbl.End = 100; tbl.Step = 0.1;
Tb("TABLE too many rows", !tbl.Generate(x => x));
Tb("TABLE too many msg", tbl.ErrorMessage.Contains("100"));

// Math error in row (log(-1))
tbl.FunctionText = "log(X)"; tbl.Start = -1; tbl.End = 2; tbl.Step = 1;
tbl.Generate(x => tblEngine.Evaluate("log(X)".Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture))));
Tb("TABLE log(-1) handled", double.IsNaN(tbl.Rows[0].Y));

// Display items
tbl.FunctionText = "X^2"; tbl.Start = 1; tbl.End = 3; tbl.Step = 1;
tbl.Generate(x => tblEngine.Evaluate("X^2".Replace("X", x.ToString(System.Globalization.CultureInfo.InvariantCulture))));
var items = tbl.GetDisplayItems();
Tb("TABLE display header", items[0].StartsWith("f(X)"));
Tb("TABLE display cols", items[1].Contains("X"));
Tb("TABLE display count", items.Length >= 5);

// ============ SOLVE ============
Console.WriteLine("\n=== SOLVE ===");
var solveEngine = new CalculatorEngine { Memory = new MemoryService(), Angle = new AngleService() };
Func<double, string> fmt = v => v.ToString("0.##########", System.Globalization.CultureInfo.InvariantCulture);

// Direct solver tests
Td("solve X+2=5,0", EquationSolverService.Solve(x => { return solveEngine.Evaluate(fmt(x) + "+2") - 5; }, 0), 3);
Td("solve 2X+3=7,0", EquationSolverService.Solve(x => { return solveEngine.Evaluate("2*" + fmt(x) + "+3") - 7; }, 0), 2);
Td("solve X^2-4=0,1", EquationSolverService.Solve(x => { return solveEngine.Evaluate(fmt(x) + "^2-4"); }, 1), 2);
Td("solve X^2-4=0,-1", EquationSolverService.Solve(x => { return solveEngine.Evaluate(fmt(x) + "^2-4"); }, -1), -2);
{ double sv = EquationSolverService.Solve(x => { return solveEngine.Evaluate("log(" + fmt(x) + ")") - 2; }, 10); if (Math.Abs(sv - 100) < 0.5) passed++; else { failed++; Console.WriteLine($"  FAIL: solve log(X)=2,10 - expected ~100, got {sv}"); } }
solveEngine.Angle!.CurrentMode = AngleMode.DEG;
{ double sv2 = EquationSolverService.Solve(x => { return solveEngine.Evaluate("sin(" + fmt(x) + ")") - 0.5; }, 30); if (Math.Abs(sv2 - 30) < 0.5) passed++; else { failed++; Console.WriteLine($"  FAIL: solve sin(X)=0.5,30 - expected ~30, got {sv2}"); } }
solveEngine.Angle!.CurrentMode = AngleMode.DEG;

// Error cases — div by zero during iteration should throw
try { EquationSolverService.Solve(x => { double z = 0; return 1 / z; }, 0); failed++; Console.WriteLine("  FAIL: div by zero should throw"); }
catch { passed++; }

Console.WriteLine($"\n========================================");
Console.WriteLine($"RESULTS: {passed} passed, {failed} failed");
Console.WriteLine($"========================================");
if (failed > 0) Environment.Exit(1);
