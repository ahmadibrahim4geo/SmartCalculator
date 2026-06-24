using System;
using System.Globalization;
using static System.Math;

namespace SmartCalculator.Models;

public struct VectorValue
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public int Dimensions { get; set; }

    public VectorValue(double x, double y, double z = 0, int dims = 2)
    {
        X = x; Y = y; Z = z; Dimensions = dims;
    }

    public double Magnitude => Sqrt(X * X + Y * Y + Z * Z);

    public static VectorValue operator +(VectorValue a, VectorValue b) =>
        new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, Max(a.Dimensions, b.Dimensions));

    public static VectorValue operator -(VectorValue a, VectorValue b) =>
        new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, Max(a.Dimensions, b.Dimensions));

    public static VectorValue operator *(double s, VectorValue v) =>
        new(s * v.X, s * v.Y, s * v.Z, v.Dimensions);

    public static VectorValue operator *(VectorValue v, double s) =>
        new(s * v.X, s * v.Y, s * v.Z, v.Dimensions);

    public static double Dot(VectorValue a, VectorValue b) =>
        a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static VectorValue Cross(VectorValue a, VectorValue b) =>
        new(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X, 3);

    public VectorValue Unit()
    {
        double m = Magnitude;
        if (m < 1e-12) throw new DivideByZeroException("Zero vector has no unit vector");
        return new(X / m, Y / m, Z / m, Dimensions);
    }

    private static double Clean(double v) => Abs(v) < 1e-12 ? 0 : v;

    public string ToFormattedString()
    {
        double x = Clean(X), y = Clean(Y), z = Clean(Z);
        if (Dimensions == 2)
            return $"[{FormatNum(x)},{FormatNum(y)}]";
        return $"[{FormatNum(x)},{FormatNum(y)},{FormatNum(z)}]";
    }

    private static string FormatNum(double v)
    {
        if (v == 0) return "0";
        string s = v.ToString("G10", CultureInfo.InvariantCulture);
        return s;
    }

    public override string ToString() => ToFormattedString();
}
