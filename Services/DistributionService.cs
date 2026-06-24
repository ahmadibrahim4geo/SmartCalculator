using static System.Math;

namespace SmartCalculator.Services;

public class DistributionService
{
    public static double NormalPdf(double x, double mean = 0, double stddev = 1)
    {
        double z = (x - mean) / stddev;
        return Exp(-0.5 * z * z) / (stddev * Sqrt(2 * PI));
    }

    public static double NormalCdf(double x, double mean = 0, double stddev = 1)
    {
        double z = (x - mean) / stddev;
        return 0.5 * (1 + Erf(z / Sqrt(2)));
    }

    private static double Erf(double x)
    {
        // Approximation
        double a1 = 0.254829592, a2 = -0.284496736, a3 = 1.421413741;
        double a4 = -1.453152027, a5 = 1.061405429, p = 0.3275911;
        double sign = x < 0 ? -1 : 1;
        x = Abs(x);
        double t = 1.0 / (1.0 + p * x);
        double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Exp(-x * x);
        return sign * y;
    }

    public static double InvNormalCdf(double p, double mean = 0, double stddev = 1)
    {
        if (p <= 0 || p >= 1) return 0;
        // Newton-Raphson
        double x = 0;
        for (int i = 0; i < 100; i++)
        {
            double f = NormalCdf(x, mean, stddev) - p;
            double df = NormalPdf(x, mean, stddev);
            if (Abs(df) < 1e-15) break;
            x -= f / df;
            if (Abs(f) < 1e-12) break;
        }
        return x;
    }
}
