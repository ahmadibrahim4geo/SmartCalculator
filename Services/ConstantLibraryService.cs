using System;
using System.Linq;
namespace SmartCalculator.Services;

public static class ConstantLibraryService
{
    public static readonly (string Name, double Value, string Symbol)[] Constants = new[]
    {
        ("π (Pi)", Math.PI, "π"),
        ("e (Euler)", Math.E, "e"),
        ("Speed of light (c)", 299792458, "c"),
        ("Gravitational accel (g)", 9.80665, "g"),
        ("Avogadro const (NA)", 6.02214076e23, "NA"),
        ("Gas constant (R)", 8.314462618, "R"),
        ("Planck const (h)", 6.62607015e-34, "h"),
        ("Electron charge (e)", 1.602176634e-19, "e⁻"),
        ("Electron mass (mₑ)", 9.1093837e-31, "mₑ"),
        ("Proton mass (mₚ)", 1.6726219e-27, "mₚ"),
    };

    public static (string Name, double Value, string Symbol) GetConstant(int index) =>
        Constants[index % Constants.Length];

    public static string[] DisplayItems =>
        Constants.Select((c, i) => $"{i + 1}:{c.Symbol} {c.Name}").ToArray();
}

