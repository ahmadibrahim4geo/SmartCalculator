using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SmartCalculator.Models;

namespace SmartCalculator.Services;

public static class MatrixService
{
    private static readonly Dictionary<string, MatrixValue> _store = new()
    {
        ["MatA"] = null!, ["MatB"] = null!, ["MatC"] = null!, ["MatAns"] = null!,
        ["A"] = null!, ["B"] = null!, ["C"] = null!
    };

    private static string ResolveName(string name)
    {
        if (name.Length == 1 && "ABC".Contains(name))
            return "Mat" + name;
        return name;
    }

    public static bool IsDefined(string name) =>
        _store.ContainsKey(ResolveName(name)) && _store[ResolveName(name)] != null;

    public static MatrixValue Get(string name) =>
        _store.TryGetValue(ResolveName(name), out var m) && m != null ? m : null!;

    public static void Store(string name, MatrixValue matrix)
    {
        var key = ResolveName(name);
        if (!_store.ContainsKey(key))
            throw new ArgumentException($"Unknown matrix name: {name}");
        _store[key] = matrix;
        _store["MatAns"] = matrix;
    }

    public static string EvaluateToString(string expr)
    {
        try
        {
            // Assignment: MatA[[1,2],[3,4]] or A[[1,2],[3,4]] (no space, no operator after name)
            foreach (var name in new[] { "MatA", "MatB", "MatC", "A", "B", "C" })
            {
                if (expr.StartsWith(name) && expr.Length > name.Length)
                {
                    char next = expr[name.Length];
                    if (next == '[')
                    {
                        string rest = expr[name.Length..];
                        var matrix = ParseMatrix(rest);
                        Store(name, matrix);
                        return $"{ResolveName(name)} = {matrix}";
                    }
                }
            }

            // Expression evaluation
            var result = Evaluate(expr);
            if (result is MatrixValue m)
            {
                _store["MatAns"] = m;
                return m.ToString();
            }
            else if (result is double d)
            {
                return d.ToString("G10", CultureInfo.InvariantCulture);
            }
            return "Error";
        }
        catch (Exception ex) when (ex is FormatException or ArgumentException or DivideByZeroException)
        {
            return $"Math Error: {ex.Message}";
        }
    }

    private static object Evaluate(string expr)
    {
        expr = expr.Trim();
        if (string.IsNullOrEmpty(expr)) throw new FormatException("Empty expression");

        // Try matrix literal
        if (expr.StartsWith('['))
            return ParseMatrix(expr);

        // Named matrix reference (including shorthands A, B, C)
        foreach (var name in _store.Keys)
        {
            if (expr == name || expr == ResolveName(name))
            {
                if (!IsDefined(name)) throw new FormatException($"{ResolveName(name)} is not defined");
                return Get(name);
            }
        }

        // Function calls: det(expr), transpose(expr), trn(expr), inv(expr)
        foreach (var func in new[] { "det", "transpose", "trn", "inv" })
        {
            if (expr.StartsWith(func + "(") && expr.EndsWith(")"))
            {
                var inner = expr[(func.Length + 1)..^1];
                var arg = Evaluate(inner);
                return func switch
                {
                    "det" => ApplyDeterminant(arg),
                    "transpose" or "trn" => ApplyTranspose(arg),
                    "inv" => ApplyInverse(arg),
                    _ => throw new FormatException($"Unknown function: {func}")
                };
            }
        }

        // Binary operators: find lowest-precedence operator outside parens
        int depth = 0;
        int opPos = -1;
        char opChar = ' ';
        int prec = int.MaxValue;

        for (int i = expr.Length - 1; i >= 0; i--)
        {
            char c = expr[i];
            if (c == ')') depth++;
            else if (c == '(') depth--;
            else if (depth == 0 && (c == '+' || c == '-' || c == '*' || c == '×'))
            {
                int p = c switch { '+' or '-' => 1, '*' or '×' => 2, _ => 3 };
                if (p < prec)
                {
                    // Skip unary minus at start or after operator
                    if (c == '-' && (i == 0 || "+-*/×".Contains(expr[i - 1])))
                        continue;
                    prec = p; opPos = i; opChar = c;
                }
            }
        }

        if (opPos >= 0)
        {
            string leftStr = expr[..opPos].Trim();
            string rightStr = expr[(opPos + 1)..].Trim();
            if (string.IsNullOrEmpty(leftStr) || string.IsNullOrEmpty(rightStr))
                throw new FormatException("Incomplete expression");

            var left = Evaluate(leftStr);
            var right = Evaluate(rightStr);

            return opChar switch
            {
                '+' => ApplyAdd(left, right),
                '-' => ApplySubtract(left, right),
                '*' or '×' => ApplyMultiply(left, right),
                _ => throw new FormatException($"Unknown operator: {opChar}")
            };
        }

        // Single number
        if (double.TryParse(expr, NumberStyles.Float, CultureInfo.InvariantCulture, out double num))
            return num;

        throw new FormatException($"Cannot parse: {expr}");
    }

    // ============ Parsing ============

    public static MatrixValue ParseMatrix(string text)
    {
        text = text.Trim();
        if (!text.StartsWith('[') || !text.EndsWith(']'))
            throw new FormatException("Matrix must be enclosed in brackets");

        text = text[1..^1].Trim(); // remove outer [ ]

        // Split rows: find top-level comma-separated [row1],[row2],...
        var rows = new List<string>();
        int depth = 0;
        int start = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '[') depth++;
            else if (text[i] == ']') depth--;
            else if (text[i] == ',' && depth == 0)
            {
                rows.Add(text[start..i].Trim());
                start = i + 1;
            }
        }
        rows.Add(text[start..].Trim());

        if (rows.Count == 0) throw new FormatException("Empty matrix");

        var data = new List<double[]>();
        foreach (var rowText in rows)
        {
            var trimmed = rowText.Trim();
            if (!trimmed.StartsWith('[') || !trimmed.EndsWith(']'))
                throw new FormatException("Invalid row format");
            var inner = trimmed[1..^1].Trim();
            if (string.IsNullOrEmpty(inner))
                throw new FormatException("Empty row");

            var values = inner.Split(',')
                .Select(v => v.Trim())
                .Select(v =>
                {
                    if (!double.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out double n))
                        throw new FormatException($"Invalid number: {v}");
                    return n;
                })
                .ToArray();
            data.Add(values);
        }

        int cols = data[0].Length;
        if (data.Any(r => r.Length != cols))
            throw new FormatException("Inconsistent row lengths");

        var result = new MatrixValue(data.Count, cols);
        for (int r = 0; r < data.Count; r++)
            for (int c = 0; c < cols; c++)
                result[r, c] = data[r][c];

        return result;
    }

    // ============ Operations ============

    public static MatrixValue Add(MatrixValue a, MatrixValue b)
    {
        if (a.Rows != b.Rows || a.Cols != b.Cols)
            throw new ArgumentException("Matrix dimension mismatch for addition");
        var result = new MatrixValue(a.Rows, a.Cols);
        for (int r = 0; r < a.Rows; r++)
            for (int c = 0; c < a.Cols; c++)
                result[r, c] = a[r, c] + b[r, c];
        return result;
    }

    public static MatrixValue Subtract(MatrixValue a, MatrixValue b)
    {
        if (a.Rows != b.Rows || a.Cols != b.Cols)
            throw new ArgumentException("Matrix dimension mismatch for subtraction");
        var result = new MatrixValue(a.Rows, a.Cols);
        for (int r = 0; r < a.Rows; r++)
            for (int c = 0; c < a.Cols; c++)
                result[r, c] = a[r, c] - b[r, c];
        return result;
    }

    public static MatrixValue Multiply(MatrixValue a, MatrixValue b)
    {
        if (a.Cols != b.Rows)
            throw new ArgumentException("Matrix dimension mismatch for multiplication");
        var result = new MatrixValue(a.Rows, b.Cols);
        for (int r = 0; r < a.Rows; r++)
            for (int c = 0; c < b.Cols; c++)
            {
                double sum = 0;
                for (int k = 0; k < a.Cols; k++)
                    sum += a[r, k] * b[k, c];
                result[r, c] = sum;
            }
        return result;
    }

    public static MatrixValue ScalarMultiply(double scalar, MatrixValue m)
    {
        var result = new MatrixValue(m.Rows, m.Cols);
        for (int r = 0; r < m.Rows; r++)
            for (int c = 0; c < m.Cols; c++)
                result[r, c] = scalar * m[r, c];
        return result;
    }

    public static double Determinant(MatrixValue m)
    {
        if (!m.IsSquare) throw new ArgumentException("Determinant requires a square matrix");
        return DeterminantInternal(m.Data, m.Rows);
    }

    private static double DeterminantInternal(double[,] m, int n)
    {
        if (n == 1) return m[0, 0];
        if (n == 2) return m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0];

        double det = 0;
        for (int c = 0; c < n; c++)
        {
            var sub = GetMinor(m, 0, c, n);
            det += (c % 2 == 0 ? 1 : -1) * m[0, c] * DeterminantInternal(sub, n - 1);
        }
        return det;
    }

    private static double[,] GetMinor(double[,] m, int row, int col, int n)
    {
        var result = new double[n - 1, n - 1];
        for (int r = 0, ri = 0; r < n; r++)
        {
            if (r == row) continue;
            for (int c = 0, ci = 0; c < n; c++)
            {
                if (c == col) continue;
                result[ri, ci] = m[r, c];
                ci++;
            }
            ri++;
        }
        return result;
    }

    public static MatrixValue Transpose(MatrixValue m)
    {
        var result = new MatrixValue(m.Cols, m.Rows);
        for (int r = 0; r < m.Rows; r++)
            for (int c = 0; c < m.Cols; c++)
                result[c, r] = m[r, c];
        return result;
    }

    public static MatrixValue Inverse(MatrixValue m)
    {
        if (!m.IsSquare) throw new ArgumentException("Inverse requires a square matrix");
        double det = Determinant(m);
        if (Math.Abs(det) < 1e-12) throw new DivideByZeroException("Matrix is singular, cannot invert");

        int n = m.Rows;
        var result = new MatrixValue(n, n);

        if (n == 2)
        {
            result[0, 0] = m[1, 1] / det;
            result[0, 1] = -m[0, 1] / det;
            result[1, 0] = -m[1, 0] / det;
            result[1, 1] = m[0, 0] / det;
            return result;
        }

        // For n > 2, use adjugate matrix
        for (int r = 0; r < n; r++)
            for (int c = 0; c < n; c++)
            {
                var minor = GetMinor(m.Data, r, c, n);
                double cofactor = ((r + c) % 2 == 0 ? 1 : -1) * DeterminantInternal(minor, n - 1);
                result[c, r] = cofactor / det; // transpose = adjugate
            }

        return result;
    }

    // ============ Apply helpers for Evaluate ============

    private static object ApplyAdd(object left, object right)
    {
        if (left is MatrixValue lm && right is MatrixValue rm) return Add(lm, rm);
        throw new FormatException("Addition requires two matrices");
    }

    private static object ApplySubtract(object left, object right)
    {
        if (left is MatrixValue lm && right is MatrixValue rm) return Subtract(lm, rm);
        throw new FormatException("Subtraction requires two matrices");
    }

    private static object ApplyMultiply(object left, object right)
    {
        if (left is double s && right is MatrixValue rm) return ScalarMultiply(s, rm);
        if (left is MatrixValue lm && right is double k) return ScalarMultiply(k, lm);
        if (left is MatrixValue lm2 && right is MatrixValue rm2) return Multiply(lm2, rm2);
        throw new FormatException("Multiplication requires matrix×matrix or scalar×matrix");
    }

    private static double ApplyDeterminant(object arg)
    {
        if (arg is MatrixValue m) return Determinant(m);
        throw new FormatException("det() requires a matrix");
    }

    private static MatrixValue ApplyTranspose(object arg)
    {
        if (arg is MatrixValue m) return Transpose(m);
        throw new FormatException("transpose() requires a matrix");
    }

    private static MatrixValue ApplyInverse(object arg)
    {
        if (arg is MatrixValue m) return Inverse(m);
        throw new FormatException("inv() requires a matrix");
    }
}
