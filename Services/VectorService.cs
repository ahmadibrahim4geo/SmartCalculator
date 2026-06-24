using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SmartCalculator.Models;

namespace SmartCalculator.Services;

public static class VectorService
{
    private static readonly Dictionary<string, VectorValue?> _store = new()
    {
        ["VecA"] = null, ["VecB"] = null, ["VecC"] = null, ["VecAns"] = null,
        ["A"] = null, ["B"] = null, ["C"] = null
    };

    private static string ResolveName(string name)
    {
        if (name.Length == 1 && "ABC".Contains(name))
            return "Vec" + name;
        return name;
    }

    public static bool IsDefined(string name) =>
        _store.TryGetValue(ResolveName(name), out var v) && v.HasValue;

    public static VectorValue Get(string name) =>
        _store.TryGetValue(ResolveName(name), out var v) && v.HasValue ? v.Value : throw new FormatException($"{ResolveName(name)} is not defined");

    public static void Store(string name, VectorValue vector)
    {
        var key = ResolveName(name);
        if (!_store.ContainsKey(key))
            throw new ArgumentException($"Unknown vector name: {name}");
        _store[key] = vector;
        _store["VecAns"] = vector;
    }

    public static string EvaluateToString(string expr)
    {
        try
        {
            expr = expr.Trim();
            if (string.IsNullOrEmpty(expr)) return "Math Error: Empty expression";

            // Assignment: VecA[1,2,3] or A[1,2,3]
            foreach (var name in new[] { "VecA", "VecB", "VecC", "A", "B", "C" })
            {
                if (expr.StartsWith(name) && expr.Length > name.Length)
                {
                    char next = expr[name.Length];
                    if (next == '[')
                    {
                        string rest = expr[name.Length..];
                        var vector = ParseVector(rest);
                        Store(name, vector);
                        return $"{ResolveName(name)} = {vector.ToFormattedString()}";
                    }
                }
            }

            var result = Evaluate(expr);
            if (result is VectorValue v)
            {
                _store["VecAns"] = v;
                return v.ToFormattedString();
            }
            else if (result is double d)
            {
                return d.ToString("G10", CultureInfo.InvariantCulture);
            }
            return "Math Error";
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

        // Named vector reference
        foreach (var name in _store.Keys)
        {
            if (expr == name || expr == ResolveName(name))
            {
                if (!IsDefined(name)) throw new FormatException($"{ResolveName(name)} is not defined");
                return Get(name);
            }
        }

        // Functions: dot(a,b), cross(a,b), mag(a), norm(a), unit(a), angle(a,b)
        string[] funcs1 = { "mag", "norm" };
        string[] funcs2 = { "dot", "cross", "angle" };
        foreach (var func in funcs1)
        {
            if (expr.StartsWith(func + "(") && expr.EndsWith(")"))
            {
                var inner = expr[(func.Length + 1)..^1].Trim();
                var arg = Evaluate(inner);
                if (arg is VectorValue vf)
                {
                    if (func == "mag" || func == "norm")
                        return vf.Magnitude;
                }
                throw new FormatException($"{func}() requires a vector");
            }
        }
        foreach (var func in funcs2)
        {
            if (expr.StartsWith(func + "(") && expr.EndsWith(")"))
            {
                var inner = expr[(func.Length + 1)..^1].Trim();
                var args = SplitArgs(inner);
                if (args.Count != 2) throw new FormatException($"{func}() requires two arguments");
                var left = Evaluate(args[0]);
                var right = Evaluate(args[1]);
                if (left is VectorValue va && right is VectorValue vb)
                {
                    return func switch
                    {
                        "dot" => VectorValue.Dot(va, vb),
                        "cross" => ApplyCross(va, vb),
                        "angle" => ApplyAngle(va, vb),
                        _ => throw new FormatException($"Unknown function: {func}")
                    };
                }
                throw new FormatException($"{func}() requires two vectors");
            }
        }

        // unit function
        if (expr.StartsWith("unit(") && expr.EndsWith(")"))
        {
            var inner = expr[5..^1].Trim();
            var arg = Evaluate(inner);
            if (arg is VectorValue vu)
                return vu.Unit();
            throw new FormatException("unit() requires a vector");
        }

        // Binary operators (search before literal to handle [a,b] + [c,d])
        int depth = 0;
        int opPos = -1;
        char opChar = ' ';
        int prec = int.MaxValue;

        for (int i = expr.Length - 1; i >= 0; i--)
        {
            char c = expr[i];
            if (c == ')' || c == ']') depth++;
            else if (c == '(' || c == '[') depth--;
            else if (depth == 0 && (c == '+' || c == '-' || c == '*' || c == '×'))
            {
                int p = c switch { '+' or '-' => 1, '*' or '×' => 2, _ => 3 };
                if (p < prec)
                {
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

        // Vector literal
        if (expr.StartsWith('['))
            return ParseVector(expr);

        // Single number (scalar)
        if (double.TryParse(expr, NumberStyles.Float, CultureInfo.InvariantCulture, out double num))
            return num;

        throw new FormatException($"Cannot parse: {expr}");
    }

    private static List<string> SplitArgs(string text)
    {
        var args = new List<string>();
        int depth = 0;
        int start = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '(') depth++;
            else if (text[i] == ')') depth--;
            else if (text[i] == ',' && depth == 0)
            {
                args.Add(text[start..i].Trim());
                start = i + 1;
            }
        }
        args.Add(text[start..].Trim());
        return args;
    }

    // ============ Parsing ============

    public static VectorValue ParseVector(string text)
    {
        text = text.Trim();
        if (!text.StartsWith('[') || !text.EndsWith(']'))
            throw new FormatException("Vector must be enclosed in brackets");
        text = text[1..^1].Trim();
        if (string.IsNullOrEmpty(text)) throw new FormatException("Empty vector");
        var parts = text.Split(',')
            .Select(p => p.Trim())
            .Select(p =>
            {
                if (!double.TryParse(p, NumberStyles.Float, CultureInfo.InvariantCulture, out double n))
                    throw new FormatException($"Invalid number: {p}");
                return n;
            })
            .ToArray();
        if (parts.Length < 2 || parts.Length > 3)
            throw new FormatException("Vector must have 2 or 3 components");
        return new VectorValue(parts[0], parts[1], parts.Length > 2 ? parts[2] : 0, parts.Length);
    }

    // ============ Operations ============

    private static object ApplyAdd(object left, object right)
    {
        if (left is VectorValue lv && right is VectorValue rv)
        {
            if (lv.Dimensions != rv.Dimensions)
                throw new ArgumentException("Vector dimension mismatch for addition");
            return lv + rv;
        }
        throw new FormatException("Addition requires two vectors");
    }

    private static object ApplySubtract(object left, object right)
    {
        if (left is VectorValue lv && right is VectorValue rv)
        {
            if (lv.Dimensions != rv.Dimensions)
                throw new ArgumentException("Vector dimension mismatch for subtraction");
            return lv - rv;
        }
        throw new FormatException("Subtraction requires two vectors");
    }

    private static object ApplyMultiply(object left, object right)
    {
        if (left is double s && right is VectorValue rv)
            return s * rv;
        if (left is VectorValue lv && right is double k)
            return lv * k;
        throw new FormatException("Multiplication requires scalar×vector or vector×scalar");
    }

    private static object ApplyCross(VectorValue a, VectorValue b)
    {
        if (a.Dimensions != 3 || b.Dimensions != 3)
            throw new ArgumentException("Cross product requires 3D vectors");
        return VectorValue.Cross(a, b);
    }

    private static double ApplyAngle(VectorValue a, VectorValue b)
    {
        double dot = VectorValue.Dot(a, b);
        double magA = a.Magnitude;
        double magB = b.Magnitude;
        if (magA < 1e-12 || magB < 1e-12)
            throw new ArgumentException("Cannot compute angle with zero vector");
        double cosAngle = dot / (magA * magB);
        cosAngle = Math.Max(-1, Math.Min(1, cosAngle));
        return Math.Acos(cosAngle);
    }
}
