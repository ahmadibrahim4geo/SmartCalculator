using System;
using System.Globalization;
using static System.Math;

namespace SmartCalculator.Models;

public struct ComplexValue
{
    public double Real { get; set; }
    public double Imag { get; set; }

    public ComplexValue(double r, double i) { Real = r; Imag = i; }

    public double Magnitude => Sqrt(Real * Real + Imag * Imag);
    public double Argument => Atan2(Imag, Real);

    public static ComplexValue operator +(ComplexValue a, ComplexValue b) => new(a.Real + b.Real, a.Imag + b.Imag);
    public static ComplexValue operator -(ComplexValue a, ComplexValue b) => new(a.Real - b.Real, a.Imag - b.Imag);
    public static ComplexValue operator *(ComplexValue a, ComplexValue b) =>
        new(a.Real * b.Real - a.Imag * b.Imag, a.Real * b.Imag + a.Imag * b.Real);
    public static ComplexValue operator /(ComplexValue a, ComplexValue b)
    {
        double d = b.Real * b.Real + b.Imag * b.Imag;
        if (d == 0) throw new DivideByZeroException("Division by zero");
        return new((a.Real * b.Real + a.Imag * b.Imag) / d, (a.Imag * b.Real - a.Real * b.Imag) / d);
    }
    public static ComplexValue operator -(ComplexValue a) => new(-a.Real, -a.Imag);
    public static ComplexValue operator +(ComplexValue a) => a;

    public ComplexValue Conjugate => new(Real, -Imag);

    private static double Clean(double v) => Abs(v) < 1e-12 ? 0 : v;

    public string ToFormattedString()
    {
        double r = Clean(Real);
        double i = Clean(Imag);

        bool showReal = Abs(r) >= 1e-12;
        bool showImag = Abs(i) >= 1e-12;

        if (!showReal && !showImag) return "0";
        if (!showImag) return FormatReal(r);
        if (!showReal) return FormatImag(i);

        return $"{FormatReal(r)}{FormatImagSigned(i)}";
    }

    private static string FormatReal(double v)
    {
        if (v == 0) return "0";
        string s = v.ToString("G10", CultureInfo.InvariantCulture);
        return s;
    }

    private static string FormatImag(double v)
    {
        if (v == 0) return "0";
        if (Abs(v - 1.0) < 1e-12) return "i";
        if (Abs(v + 1.0) < 1e-12) return "-i";
        return $"{v.ToString("G10", CultureInfo.InvariantCulture)}i";
    }

    private static string FormatImagSigned(double v)
    {
        if (v == 0) return "";
        if (Abs(v - 1.0) < 1e-12) return "+i";
        if (Abs(v + 1.0) < 1e-12) return "-i";
        return v > 0
            ? $"+{v.ToString("G10", CultureInfo.InvariantCulture)}i"
            : $"{v.ToString("G10", CultureInfo.InvariantCulture)}i";
    }

    public override string ToString() => ToFormattedString();
}
