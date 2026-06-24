using System;
namespace SmartCalculator.Models;

public enum TokenType
{
    Number, Operator, Function, Constant, LeftParen, RightParen
}

public enum CalcOperator
{
    Add, Subtract, Multiply, Divide, Percent, Power
}

public enum CalcFunction
{
    Sin, Cos, Tan, Log, Ln, Sqrt, Negate, Asin, Acos, Atan, Exp,
    Sinh, Cosh, Tanh, Power10, Factorial, Abs, CubeRoot
}

public enum CalcConstant
{
    Pi, Ans, E, PreAns, A, B, C, D, E_const, F, X, Y, M
}

public class CalcToken
{
    public TokenType Type { get; set; }
    public double NumberValue { get; set; }
    public CalcOperator OperatorType { get; set; }
    public CalcFunction FunctionType { get; set; }
    public CalcConstant ConstantType { get; set; }
}

