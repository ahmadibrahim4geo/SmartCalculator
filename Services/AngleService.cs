using System;
using SmartCalculator.Models;

namespace SmartCalculator.Services;

public class AngleService
{
    public AngleMode CurrentMode { get; set; } = AngleMode.DEG;

    public void Cycle() =>
        CurrentMode = (AngleMode)(((int)CurrentMode + 1) % 3);

    public string DisplayText => CurrentMode switch
    {
        AngleMode.DEG => "DEG",
        AngleMode.RAD => "RAD",
        AngleMode.GRAD => "GRAD",
        _ => "DEG"
    };

    public double ToRadians(double angle) => CurrentMode switch
    {
        AngleMode.RAD => angle,
        AngleMode.GRAD => angle * Math.PI / 200.0,
        _ => angle * Math.PI / 180.0
    };

    public double FromRadians(double radians) => CurrentMode switch
    {
        AngleMode.RAD => radians,
        AngleMode.GRAD => radians * 200.0 / Math.PI,
        _ => radians * 180.0 / Math.PI
    };
}
