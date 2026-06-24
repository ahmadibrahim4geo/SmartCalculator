using System;
using System.Collections.Generic;
using System.Linq;
using SmartCalculator.Models;

namespace SmartCalculator.Services;

public class CalculatorEngine
{
    public MemoryService? Memory { get; set; }
    public AngleService? Angle { get; set; }

    public double Evaluate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new CalculatorException("Syntax Error");

        var tokens = Tokenize(expression);
        if (tokens.Count == 0) throw new CalculatorException("Syntax Error");

        var rpn = ShuntingYard(tokens);
        return EvaluateRPN(rpn);
    }

    private List<CalcToken> Tokenize(string expr)
    {
        expr = expr.Replace("×", "*").Replace("÷", "/").Replace("−", "-");
        var tokens = new List<CalcToken>();
        int i = 0;
        bool expectUnary = true;

        while (i < expr.Length)
        {
            char c = expr[i];

            if (char.IsWhiteSpace(c)) { i++; continue; }

            if (char.IsDigit(c) || (c == '.' && i + 1 < expr.Length && char.IsDigit(expr[i + 1])))
            {
                int start = i; bool hasDot = false;
                while (i < expr.Length && (char.IsDigit(expr[i]) || (expr[i] == '.' && !hasDot)))
                { if (expr[i] == '.') hasDot = true; i++; }
                string numStr = expr[start..i];
                if (!double.TryParse(numStr,
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double num))
                    throw new CalculatorException("Syntax Error");
                tokens.Add(new CalcToken { Type = TokenType.Number, NumberValue = num });
                expectUnary = false; continue;
            }

            if (c == 'π') { tokens.Add(new CalcToken { Type = TokenType.Constant, ConstantType = CalcConstant.Pi }); i++; expectUnary = false; continue; }

            if (c == 'e' && (i + 1 >= expr.Length || !char.IsLetterOrDigit(expr[i + 1])))
            { tokens.Add(new CalcToken { Type = TokenType.Constant, ConstantType = CalcConstant.E }); i++; expectUnary = false; continue; }

            // Constants and variables
            string[] constNames = { "Ans", "PreAns", "pi" };
            foreach (var cn in constNames)
            {
                if (i + cn.Length <= expr.Length && expr.Substring(i, cn.Length) == cn &&
                    (i + cn.Length >= expr.Length || !char.IsLetterOrDigit(expr[i + cn.Length])))
                {
                    var ct = cn switch { "Ans" => CalcConstant.Ans, "PreAns" => CalcConstant.PreAns, _ => CalcConstant.Pi };
                    tokens.Add(new CalcToken { Type = TokenType.Constant, ConstantType = ct });
                    i += cn.Length; expectUnary = false; goto nextChar;
                }
            }

            // Single-letter variables A-F, X, Y, M
            if (char.IsLetter(c) && i + 1 < expr.Length && !char.IsLetterOrDigit(expr[i + 1]) ||
                char.IsLetter(c) && i + 1 >= expr.Length)
            {
                CalcConstant? ct = char.ToUpper(c) switch
                {
                    'A' => CalcConstant.A, 'B' => CalcConstant.B, 'C' => CalcConstant.C,
                    'D' => CalcConstant.D, 'E' => CalcConstant.E_const, 'F' => CalcConstant.F,
                    'X' => CalcConstant.X, 'Y' => CalcConstant.Y, 'M' => CalcConstant.M,
                    _ => null
                };
                if (ct.HasValue)
                {
                    tokens.Add(new CalcToken { Type = TokenType.Constant, ConstantType = ct.Value });
                    i++; expectUnary = false; continue;
                }
            }

            if (c == '∛')
            {
                tokens.Add(new CalcToken { Type = TokenType.Function, FunctionType = CalcFunction.CubeRoot }); i++; expectUnary = true; continue;
            }

            if (c == '√')
            {
                if (i + 1 < expr.Length && expr[i + 1] == '√') { i += 2; tokens.Add(new CalcToken { Type = TokenType.Function, FunctionType = CalcFunction.CubeRoot }); expectUnary = true; continue; }
                tokens.Add(new CalcToken { Type = TokenType.Function, FunctionType = CalcFunction.Sqrt }); i++; expectUnary = true; continue;
            }

            string[] funcNames = { "asin", "acos", "atan", "sinh", "cosh", "tanh", "exp", "sin", "cos", "tan", "log", "ln", "abs", "fact" };
            bool found = false;
            foreach (var fname in funcNames)
            {
                if (i + fname.Length <= expr.Length &&
                    expr.Substring(i, fname.Length) == fname &&
                    (i + fname.Length >= expr.Length || !char.IsLetter(expr[i + fname.Length])))
                {
                    CalcFunction func = fname switch
                    {
                        "asin" => CalcFunction.Asin, "acos" => CalcFunction.Acos, "atan" => CalcFunction.Atan,
                        "sinh" => CalcFunction.Sinh, "cosh" => CalcFunction.Cosh, "tanh" => CalcFunction.Tanh,
                        "exp" => CalcFunction.Exp, "sin" => CalcFunction.Sin, "cos" => CalcFunction.Cos,
                        "tan" => CalcFunction.Tan, "log" => CalcFunction.Log, "ln" => CalcFunction.Ln,
                        "abs" => CalcFunction.Abs, "fact" => CalcFunction.Factorial,
                        _ => CalcFunction.Sin
                    };
                    tokens.Add(new CalcToken { Type = TokenType.Function, FunctionType = func });
                    i += fname.Length; expectUnary = true; found = true; break;
                }
            }
            if (found) continue;

            // 10^ as power-of-10 function
            if (i + 2 <= expr.Length && expr.Substring(i, 2) == "10" &&
                (i + 2 >= expr.Length || expr[i + 2] != '^'))
            {
                tokens.Add(new CalcToken { Type = TokenType.Number, NumberValue = 10 });
                i += 2; expectUnary = false; continue;
            }
            if (i + 1 < expr.Length && expr[i] == '1' && expr[i + 1] == '0')
            {
                tokens.Add(new CalcToken { Type = TokenType.Number, NumberValue = 10 });
                i += 2; expectUnary = false; continue;
            }

            if (c == '!')
            {
                tokens.Add(new CalcToken { Type = TokenType.Function, FunctionType = CalcFunction.Factorial });
                i++; expectUnary = true; continue;
            }

            if (c == '(') { tokens.Add(new CalcToken { Type = TokenType.LeftParen }); i++; expectUnary = true; continue; }
            if (c == ')') { tokens.Add(new CalcToken { Type = TokenType.RightParen }); i++; expectUnary = false; continue; }

            if ("+-*/%^".Contains(c))
            {
                if (c == '-' && expectUnary)
                { tokens.Add(new CalcToken { Type = TokenType.Function, FunctionType = CalcFunction.Negate }); i++; continue; }
                if (c == '+' && expectUnary) { i++; continue; }
                CalcOperator op = c switch
                { '+' => CalcOperator.Add, '-' => CalcOperator.Subtract, '*' => CalcOperator.Multiply,
                  '/' => CalcOperator.Divide, '%' => CalcOperator.Percent, '^' => CalcOperator.Power, _ => CalcOperator.Add };
                tokens.Add(new CalcToken { Type = TokenType.Operator, OperatorType = op }); i++; expectUnary = true; continue;
            }

            i++;
        nextChar:;
        }
        return tokens;
    }

    private List<CalcToken> ShuntingYard(List<CalcToken> tokens)
    {
        var output = new List<CalcToken>();
        var opStack = new Stack<CalcToken>();

        int Prec(CalcOperator op) => op switch
        { CalcOperator.Power => 4, CalcOperator.Multiply => 3, CalcOperator.Divide => 3,
          CalcOperator.Percent => 3, CalcOperator.Add => 2, CalcOperator.Subtract => 2, _ => 0 };

        bool LeftAssoc(CalcOperator op) => op != CalcOperator.Power;

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                case TokenType.Constant: output.Add(token); break;

                case TokenType.Function: opStack.Push(token); break;

                case TokenType.Operator:
                    while (opStack.Count > 0 &&
                           (opStack.Peek().Type == TokenType.Function ||
                            (opStack.Peek().Type == TokenType.Operator &&
                             (Prec(opStack.Peek().OperatorType) > Prec(token.OperatorType) ||
                              (Prec(opStack.Peek().OperatorType) == Prec(token.OperatorType) &&
                               LeftAssoc(token.OperatorType))))))
                        output.Add(opStack.Pop());
                    opStack.Push(token); break;

                case TokenType.LeftParen: opStack.Push(token); break;

                case TokenType.RightParen:
                    while (opStack.Count > 0 && opStack.Peek().Type != TokenType.LeftParen)
                        output.Add(opStack.Pop());
                    if (opStack.Count == 0) throw new CalculatorException("Syntax Error");
                    opStack.Pop();
                    if (opStack.Count > 0 && opStack.Peek().Type == TokenType.Function)
                        output.Add(opStack.Pop());
                    break;
            }
        }

        while (opStack.Count > 0)
        {
            var op = opStack.Pop();
            if (op.Type == TokenType.LeftParen) throw new CalculatorException("Unclosed parentheses");
            output.Add(op);
        }
        return output;
    }

    private double GetConstantValue(CalcConstant c) => c switch
    {
        CalcConstant.Pi => Math.PI,
        CalcConstant.E => Math.E,
        CalcConstant.Ans => Memory?.Ans ?? 0,
        CalcConstant.PreAns => Memory?.PreAns ?? 0,
        CalcConstant.A => Memory?.Recall("A") ?? 0,
        CalcConstant.B => Memory?.Recall("B") ?? 0,
        CalcConstant.C => Memory?.Recall("C") ?? 0,
        CalcConstant.D => Memory?.Recall("D") ?? 0,
        CalcConstant.E_const => Memory?.Recall("E") ?? 0,
        CalcConstant.F => Memory?.Recall("F") ?? 0,
        CalcConstant.X => Memory?.Recall("X") ?? 0,
        CalcConstant.Y => Memory?.Recall("Y") ?? 0,
        CalcConstant.M => Memory?.Recall("M") ?? 0,
        _ => 0
    };

    private double EvaluateRPN(List<CalcToken> rpn)
    {
        var stack = new Stack<double>();

        foreach (var token in rpn)
        {
            switch (token.Type)
            {
                case TokenType.Number: stack.Push(token.NumberValue); break;

                case TokenType.Constant: stack.Push(GetConstantValue(token.ConstantType)); break;

                case TokenType.Function:
                    if (stack.Count == 0) throw new CalculatorException("Syntax Error");
                    double arg = stack.Pop();

                    if (token.FunctionType == CalcFunction.Factorial)
                    {
                        int n = (int)arg;
                        if (n < 0 || arg != n) throw new CalculatorException("Math Error");
                        double r = 1;
                        for (int i = 2; i <= n; i++) r *= i;
                        stack.Push(r);
                        break;
                    }

                    double result = token.FunctionType switch
                    {
                        CalcFunction.Negate => -arg,
                        CalcFunction.Sin => Math.Sin(ToRad(arg)),
                        CalcFunction.Cos => Math.Cos(ToRad(arg)),
                        CalcFunction.Tan => Math.Tan(ToRad(arg)),
                        CalcFunction.Asin => FromRad(Math.Asin(arg)),
                        CalcFunction.Acos => FromRad(Math.Acos(arg)),
                        CalcFunction.Atan => FromRad(Math.Atan(arg)),
                        CalcFunction.Sinh => Math.Sinh(arg),
                        CalcFunction.Cosh => Math.Cosh(arg),
                        CalcFunction.Tanh => Math.Tanh(arg),
                        CalcFunction.Log => arg <= 0 ? throw new CalculatorException("Math Error") : Math.Log10(arg),
                        CalcFunction.Ln => arg <= 0 ? throw new CalculatorException("Math Error") : Math.Log(arg),
                        CalcFunction.Exp => Math.Exp(arg),
                        CalcFunction.Power10 => Math.Pow(10, arg),
                        CalcFunction.Abs => Math.Abs(arg),
                        CalcFunction.Sqrt => arg < 0 ? throw new CalculatorException("Math Error") : Math.Sqrt(arg),
                        CalcFunction.CubeRoot => Math.Pow(arg, 1.0 / 3.0),
                        _ => throw new CalculatorException("Math Error")
                    };

                    if (double.IsNaN(result) || double.IsInfinity(result))
                        throw new CalculatorException("Math Error");

                    stack.Push(result);
                    break;

                case TokenType.Operator:
                    if (stack.Count < 2) throw new CalculatorException("Syntax Error");
                    double right = stack.Pop();
                    double left = stack.Pop();
                    double opResult = token.OperatorType switch
                    {
                        CalcOperator.Add => left + right,
                        CalcOperator.Subtract => left - right,
                        CalcOperator.Multiply => left * right,
                        CalcOperator.Divide => right == 0 ? throw new CalculatorException("Cannot divide by zero") : left / right,
                        CalcOperator.Percent => left * right / 100.0,
                        CalcOperator.Power => Math.Pow(left, right),
                        _ => throw new CalculatorException("Math Error")
                    };
                    if (double.IsNaN(opResult) || double.IsInfinity(opResult))
                        throw new CalculatorException("Math Error");
                    stack.Push(opResult);
                    break;

                default: throw new CalculatorException("Syntax Error");
            }
        }

        if (stack.Count == 0 || stack.Count > 1) throw new CalculatorException("Syntax Error");
        return stack.Pop();
    }

    private double ToRad(double angle) => Angle?.ToRadians(angle) ?? angle * Math.PI / 180.0;
    private double FromRad(double radians) => Angle?.FromRadians(radians) ?? radians * 180.0 / Math.PI;
}
