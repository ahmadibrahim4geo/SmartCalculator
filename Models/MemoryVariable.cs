using System;
namespace SmartCalculator.Models;

public class MemoryVariable
{
    public string Name { get; set; } = "";
    public double Value { get; set; }
    public bool HasValue { get; set; }
}

