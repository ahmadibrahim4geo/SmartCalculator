using System;
using System.Linq;
namespace SmartCalculator.Services;

public static class FormattingService
{
    public enum DisplayMode { Normal, Scientific, Engineering, Fixed }
    public static DisplayMode Mode { get; set; } = DisplayMode.Normal;
    public static int DecimalPlaces { get; set; } = 9;

    public static string Format(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            return "Math Error";

        if (Mode == DisplayMode.Engineering)
            return ToEngineering(value);
        if (Mode == DisplayMode.Scientific)
            return value.ToString($"E{DecimalPlaces}");
        if (Mode == DisplayMode.Fixed)
            return value.ToString($"F{DecimalPlaces}");

        if (value == Math.Floor(value) && Math.Abs(value) < 1e15)
            return value.ToString("0.############################",
                System.Globalization.CultureInfo.InvariantCulture);

        string s = value.ToString("G10", System.Globalization.CultureInfo.InvariantCulture);
        return s;
    }

    public static string ToEngineering(double value)
    {
        if (value == 0) return "0";
        int exp = 0;
        double mag = Math.Abs(value);
        while (mag >= 1000) { mag /= 1000; exp += 3; }
        while (mag < 1 && mag != 0) { mag *= 1000; exp -= 3; }
        string prefix = exp switch
        {
            3 => "k", 6 => "M", 9 => "G", 12 => "T",
            -3 => "m", -6 => "μ", -9 => "n", -12 => "p",
            _ => ""
        };
        return $"{value / Math.Pow(1000, exp / 3):G4}{prefix}";
    }

    public static string FormatWithSeparators(double value)
    {
        return value.ToString("N" + DecimalPlaces,
            System.Globalization.CultureInfo.InvariantCulture);
    }
}

