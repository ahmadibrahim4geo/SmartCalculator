using System;
using System.Linq;

namespace SmartCalculator.Services;

public static class UnitConversionService
{
    public static readonly (string Name, Func<double, double> ToBase, Func<double, double> FromBase)[] Conversions;

    static UnitConversionService()
    {
        Conversions = new (string Name, Func<double, double> ToBase, Func<double, double> FromBase)[]
        {
            ("m↔km",  v => v * 1000, v => v / 1000),
            ("m↔ft",  v => v * 3.28084, v => v / 3.28084),
            ("kg↔lb", v => v * 2.20462, v => v / 2.20462),
            ("°C↔°F", v => v * 9.0 / 5.0 + 32, v => (v - 32) * 5.0 / 9.0),
            ("°C↔K",  v => v + 273.15, v => v - 273.15),
            ("L↔gal", v => v * 0.264172, v => v / 0.264172),
            ("m/s↔km/h", v => v * 3.6, v => v / 3.6),
            ("Pa↔psi", v => v / 6894.76, v => v * 6894.76),
        };
    }

    public static string[] DisplayItems =>
        Conversions.Select((c, i) => $"{i + 1}:{c.Name}").ToArray();

    public static (double Result, string Label) Convert(int index, double value, bool toBase)
    {
        var c = Conversions[index];
        double r = toBase ? c.ToBase(value) : c.FromBase(value);
        return (r, c.Name);
    }
}
