using System;

namespace SmartCalculator.Models
{
    public class CalculatorException : Exception
    {
        public CalculatorException(string message) : base(message)
        {
        }
    }
}
