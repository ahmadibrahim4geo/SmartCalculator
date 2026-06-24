using System;
namespace SmartCalculator.Models;

public class CalculatorResult
{
    public double NumericValue { get; set; }
    public string DisplayText { get; set; } = "0";
    public bool IsError { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsFraction { get; set; }
    public FractionValue? Fraction { get; set; }
    public string? EngineeringNotation { get; set; }
}

