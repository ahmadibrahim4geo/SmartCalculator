using System;

namespace SmartCalculator.Services;

public static class ScientificFunctions
{
    public static double Factorial(double n)
    {
        int k = (int)n;
        if (k < 0 || n != k) throw new ArgumentException("Factorial requires non-negative integer");
        double r = 1;
        for (int i = 2; i <= k; i++) r *= i;
        return r;
    }

    public static long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);
    public static long Lcm(long a, long b) => a == 0 || b == 0 ? 0 : Math.Abs(a * b) / Gcd(a, b);

    public static double Npr(double n, double r)
    {
        int nn = (int)n, rr = (int)r;
        if (nn < 0 || rr < 0 || rr > nn) throw new ArgumentException("Invalid nPr");
        double result = 1;
        for (int i = nn; i > nn - rr; i--) result *= i;
        return result;
    }

    public static double Ncr(double n, double r)
    {
        int nn = (int)n, rr = (int)r;
        if (nn < 0 || rr < 0 || rr > nn) throw new ArgumentException("Invalid nCr");
        rr = Math.Min(rr, nn - rr);
        double result = 1;
        for (int i = 1; i <= rr; i++)
            result = result * (nn - rr + i) / i;
        return result;
    }

    private static readonly Random _rng = new();

    public static double Ran() => _rng.NextDouble();
    public static int RanInt(int a, int b) => _rng.Next(a, b + 1);
    public static double Rnd(double v, int decimals) => Math.Round(v, decimals);
}
