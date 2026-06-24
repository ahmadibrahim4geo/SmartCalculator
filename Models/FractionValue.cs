using System;
using static System.Math;

namespace SmartCalculator.Models;

public class FractionValue
{
    public long Numerator { get; init; }
    public long Denominator { get; init; }
    public long WholePart { get; init; }
    public bool IsNegative { get; init; }

    public double ToDouble() => (IsNegative ? -1 : 1) * (WholePart + (double)Numerator / Denominator);

    /// <summary>
    /// Converts a double to a fraction using a continued fraction algorithm.
    /// Based on the classic algorithm: iterate the reciprocal of the fractional part.
    /// </summary>
    public static FractionValue FromDouble(double value, long maxDenom = 100000)
    {
        if (double.IsNaN(value) || double.IsInfinity(value) || Abs(value) > 1e15)
            return new FractionValue { Numerator = 0, Denominator = 1, WholePart = 0 };

        bool negative = value < 0;
        double absVal = Abs(value);

        long whole = (long)Floor(absVal);
        double frac = absVal - whole;

        if (frac < 1e-12)
            return new FractionValue { Numerator = 0, Denominator = 1, WholePart = whole, IsNegative = negative };

        // Continued fraction: compute convergents p/q
        long p0 = 0, q0 = 1; // previous convergent: 0/1
        long p1 = 1, q1 = 0; // convergent starts: 1/0 (infinity)

        long a = (long)Floor(frac);
        double remainder = frac;

        for (int i = 0; i < 100; i++)
        {
            // Compute next convergent
            long p2 = a * p1 + p0;
            long q2 = a * q1 + q0;

            if (q2 > maxDenom)
                break;

            p0 = p1; q0 = q1;
            p1 = p2; q1 = q2;

            double diff = remainder - a;
            if (Abs(diff) < 1e-12)
                break;

            remainder = 1.0 / diff;
            a = (long)Floor(remainder);
        }

        // p1/q1 is the best rational approximation within maxDenom
        long num = p1;
        long den = q1;

        // Reduce by GCD
        long g = Gcd(num, den);
        num /= g;
        den /= g;

        return new FractionValue
        {
            Numerator = num,
            Denominator = den,
            WholePart = whole,
            IsNegative = negative
        };
    }

    private static long Gcd(long a, long b)
    {
        a = Abs(a);
        b = Abs(b);
        while (b != 0)
        {
            long t = b;
            b = a % b;
            a = t;
        }
        return a;
    }

    public override string ToString()
    {
        string sign = IsNegative ? "-" : "";
        if (Numerator == 0)
            return $"{sign}{WholePart}";
        if (WholePart == 0)
            return $"{sign}{Numerator}/{Denominator}";
        return $"{sign}{WholePart} {Numerator}/{Denominator}";
    }

    /// <summary>
    /// Returns true if this fraction represents an exact representation of the original value.
    /// </summary>
    public bool IsExact(double originalValue)
    {
        double reconstructed = (IsNegative ? -1 : 1) * (WholePart + (double)Numerator / Denominator);
        return Abs(reconstructed - originalValue) < 1e-10;
    }
}
