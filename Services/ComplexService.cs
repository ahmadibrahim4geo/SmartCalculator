using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SmartCalculator.Models;

namespace SmartCalculator.Services;

public static class ComplexService
{
    public static bool NeedsComplexEvaluation(string expr)
    {
        if (string.IsNullOrWhiteSpace(expr)) return false;
        return expr.Contains('i');
    }

    public static string EvaluateToString(string expr)
    {
        try
        {
            var result = Evaluate(expr);
            return result.ToFormattedString();
        }
        catch (DivideByZeroException)
        {
            return "Cannot divide by zero";
        }
        catch (Exception ex)
        {
            return $"Math Error: {ex.Message}";
        }
    }

    public static ComplexValue Evaluate(string expr)
    {
        if (string.IsNullOrWhiteSpace(expr)) throw new ArgumentException("Empty expression");
        var tokens = Tokenize(expr);
        if (tokens.Count == 0) throw new ArgumentException("Empty expression");
        var rpn = ShuntingYard(tokens);
        return EvaluateRPN(rpn);
    }

    private enum TokenType { Number, Imag, Operator, LeftParen, RightParen, Function }
    private enum FuncType { Abs, Conj, Negate }

    private struct Token
    {
        public TokenType Type;
        public double NumberValue;
        public char OperatorChar;
        public FuncType FunctionType;
    }

    private static List<Token> Tokenize(string expr)
    {
        var tokens = new List<Token>();
        int i = 0;
        bool expectUnary = true;

        while (i < expr.Length)
        {
            char c = expr[i];

            if (char.IsWhiteSpace(c)) { i++; continue; }

            // Number
            if (char.IsDigit(c) || (c == '.' && i + 1 < expr.Length && char.IsDigit(expr[i + 1])))
            {
                int start = i; bool hasDot = false;
                while (i < expr.Length && (char.IsDigit(expr[i]) || (expr[i] == '.' && !hasDot)))
                { if (expr[i] == '.') hasDot = true; i++; }
                string numStr = expr[start..i];
                if (!double.TryParse(numStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double num))
                    throw new FormatException($"Invalid number: {numStr}");
                tokens.Add(new Token { Type = TokenType.Number, NumberValue = num });
                expectUnary = false;
                if (i < expr.Length && expr[i] == 'i')
                    tokens.Add(new Token { Type = TokenType.Operator, OperatorChar = '*' });
                continue;
            }

            // Imaginary unit
            if (c == 'i')
            {
                tokens.Add(new Token { Type = TokenType.Imag });
                i++; expectUnary = false;
                if (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.' || expr[i] == '('))
                    tokens.Add(new Token { Type = TokenType.Operator, OperatorChar = '*' });
                continue;
            }

            // Function names
            if (char.IsLetter(c))
            {
                int start = i;
                while (i < expr.Length && char.IsLetter(expr[i])) i++;
                string name = expr[start..i];
                switch (name.ToLowerInvariant())
                {
                    case "abs": tokens.Add(new Token { Type = TokenType.Function, FunctionType = FuncType.Abs }); expectUnary = true; break;
                    case "conj": tokens.Add(new Token { Type = TokenType.Function, FunctionType = FuncType.Conj }); expectUnary = true; break;
                    default: throw new FormatException($"Unknown function: {name}");
                }
                continue;
            }

            // Operators and parentheses
            switch (c)
            {
                case '(':
                    tokens.Add(new Token { Type = TokenType.LeftParen }); i++; expectUnary = true; continue;
                case ')':
                    tokens.Add(new Token { Type = TokenType.RightParen }); i++; expectUnary = false;
                    if (i < expr.Length && (expr[i] == '(' || expr[i] == 'i' || expr[i] == '.' || char.IsDigit(expr[i])))
                        tokens.Add(new Token { Type = TokenType.Operator, OperatorChar = '*' });
                    continue;
                case '+':
                    if (expectUnary) { i++; continue; } // unary plus: skip
                    tokens.Add(new Token { Type = TokenType.Operator, OperatorChar = '+' }); i++; expectUnary = true; continue;
                case '-':
                    if (expectUnary)
                    {
                        tokens.Add(new Token { Type = TokenType.Function, FunctionType = FuncType.Negate }); i++; continue;
                    }
                    tokens.Add(new Token { Type = TokenType.Operator, OperatorChar = '-' }); i++; expectUnary = true; continue;
                case '*':
                case '×':
                    tokens.Add(new Token { Type = TokenType.Operator, OperatorChar = '*' }); i++; expectUnary = true; continue;
                case '/':
                case '÷':
                    tokens.Add(new Token { Type = TokenType.Operator, OperatorChar = '/' }); i++; expectUnary = true; continue;
                default:
                    i++; continue;
            }
        }
        return tokens;
    }

    private static int Precedence(char op) => op switch { '+' => 2, '-' => 2, '*' => 3, '/' => 3, _ => 0 };

    private static List<Token> ShuntingYard(List<Token> tokens)
    {
        var output = new List<Token>();
        var opStack = new Stack<Token>();

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                case TokenType.Imag:
                    output.Add(token);
                    break;
                case TokenType.Function:
                    opStack.Push(token);
                    break;
                case TokenType.Operator:
                    while (opStack.Count > 0 &&
                           (opStack.Peek().Type == TokenType.Function ||
                            (opStack.Peek().Type == TokenType.Operator &&
                             Precedence(opStack.Peek().OperatorChar) >= Precedence(token.OperatorChar))))
                        output.Add(opStack.Pop());
                    opStack.Push(token);
                    break;
                case TokenType.LeftParen:
                    opStack.Push(token);
                    break;
                case TokenType.RightParen:
                    while (opStack.Count > 0 && opStack.Peek().Type != TokenType.LeftParen)
                        output.Add(opStack.Pop());
                    if (opStack.Count == 0) throw new FormatException("Mismatched parentheses");
                    opStack.Pop(); // pop '('
                    if (opStack.Count > 0 && opStack.Peek().Type == TokenType.Function)
                        output.Add(opStack.Pop());
                    break;
            }
        }

        while (opStack.Count > 0)
        {
            var op = opStack.Pop();
            if (op.Type == TokenType.LeftParen) throw new FormatException("Mismatched parentheses");
            output.Add(op);
        }

        return output;
    }

    private static ComplexValue EvaluateRPN(List<Token> rpn)
    {
        var stack = new Stack<ComplexValue>();

        foreach (var token in rpn)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                    stack.Push(new ComplexValue(token.NumberValue, 0));
                    break;
                case TokenType.Imag:
                    stack.Push(new ComplexValue(0, 1));
                    break;
                case TokenType.Function:
                    if (stack.Count == 0) throw new FormatException("Syntax Error");
                    var arg = stack.Pop();
                    stack.Push(token.FunctionType switch
                    {
                        FuncType.Negate => -arg,
                        FuncType.Abs => new ComplexValue(arg.Magnitude, 0),
                        FuncType.Conj => arg.Conjugate,
                        _ => throw new FormatException("Unknown function")
                    });
                    break;
                case TokenType.Operator:
                    if (stack.Count < 2) throw new FormatException("Syntax Error");
                    var right = stack.Pop();
                    var left = stack.Pop();
                    stack.Push(token.OperatorChar switch
                    {
                        '+' => left + right,
                        '-' => left - right,
                        '*' => left * right,
                        '/' => left / right,
                        _ => throw new FormatException("Unknown operator")
                    });
                    break;
            }
        }

        if (stack.Count != 1) throw new FormatException("Syntax Error");
        return stack.Pop();
    }
}
