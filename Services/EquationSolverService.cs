using System;

namespace SmartCalculator.Services;

public static class EquationSolverService
{
    public static double Solve(Func<double, double> f, double guess = 0, double tol = 1e-10, int maxIter = 100)
    {
        double x = guess;
        for (int i = 0; i < maxIter; i++)
        {
            double fx = f(x);
            if (Math.Abs(fx) < tol) return x;

            double h = Math.Max(1e-8, Math.Abs(x) * 1e-8);
            double df = (f(x + h) - f(x - h)) / (2 * h);
            if (Math.Abs(df) < 1e-15)
            {
                double rb = SolveBisection(f, guess - 100, guess + 100, tol, maxIter);
                if (!double.IsNaN(rb)) return rb;
                throw new InvalidOperationException("No convergence. Try a different guess.");
            }
            x = x - fx / df;
            if (double.IsNaN(x) || double.IsInfinity(x))
            {
                double rb = SolveBisection(f, guess - 100, guess + 100, tol, maxIter);
                if (!double.IsNaN(rb)) return rb;
                throw new InvalidOperationException("No convergence. Try a different guess.");
            }
        }
        if (Math.Abs(f(x)) >= tol)
            throw new InvalidOperationException("No convergence. Try a different guess.");
        return x;
    }

    private static double SolveBisection(Func<double, double> f, double a, double b, double tol, int maxIter)
    {
        double fa = f(a), fb = f(b);
        if (fa * fb > 0) return double.NaN; // no sign change
        for (int i = 0; i < maxIter; i++)
        {
            double m = (a + b) / 2;
            double fm = f(m);
            if (Math.Abs(fm) < tol || (b - a) / 2 < tol) return m;
            if (fa * fm < 0) { b = m; fb = fm; }
            else { a = m; fa = fm; }
        }
        return double.NaN;
    }

    // Quadratic: ax² + bx + c = 0 → returns roots or NaN
    public static (double? Root1, double? Root2) SolveQuadratic(double a, double b, double c)
    {
        double d = b * b - 4 * a * c;
        if (d < 0) return (null, null);
        double sqrtD = Math.Sqrt(d);
        return ((-b - sqrtD) / (2 * a), (-b + sqrtD) / (2 * a));
    }

    public static double DefiniteIntegral(Func<double, double> f, double a, double b, int n = 1000)
    {
        if (n % 2 != 0) n++;
        double h = (b - a) / n;
        double sum = f(a) + f(b);
        for (int i = 1; i < n; i++)
            sum += (i % 2 == 0 ? 2 : 4) * f(a + i * h);
        return sum * h / 3;
    }

    public static double Derivative(Func<double, double> f, double x, double h = 1e-8) =>
        (f(x + h) - f(x - h)) / (2 * h);

    public static double Summation(Func<double, double> f, double start, double end)
    {
        double sum = 0;
        int s = (int)Math.Round(start), e = (int)Math.Round(end);
        for (int i = s; i <= e; i++) sum += f(i);
        return sum;
    }
}
