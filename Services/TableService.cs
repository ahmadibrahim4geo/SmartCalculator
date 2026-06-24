using System;
using System.Collections.Generic;
using System.Globalization;

namespace SmartCalculator.Services;

public class TableService
{
    public bool IsActive { get; set; }
    public string FunctionText { get; set; } = "";
    public double Start { get; set; }
    public double End { get; set; }
    public double Step { get; set; }
    public List<(double X, double Y)> Rows { get; } = new();
    public string ErrorMessage { get; private set; } = "";

    public bool Generate(Func<double, double> f)
    {
        Rows.Clear();
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(FunctionText))
        { ErrorMessage = "No function defined"; return false; }

        if (Math.Abs(Step) < 1e-12)
        { ErrorMessage = "Step cannot be zero"; return false; }

        if (Step > 0 && End < Start - 1e-12)
        { ErrorMessage = "End must be ≥ Start"; return false; }

        if (Step < 0 && End > Start + 1e-12)
        { ErrorMessage = "End must be ≤ Start"; return false; }

        double maxRows = Math.Abs((End - Start) / Step) + 1;
        if (maxRows > 101)
        { ErrorMessage = "Too many rows (>100). Increase step."; return false; }

        int count = 0;
        for (double x = Start; (Step > 0 ? x <= End + Step * 0.5 : x >= End + Step * 0.5); x += Step)
        {
            try
            {
                double y = f(x);
                Rows.Add((x, y));
            }
            catch
            {
                Rows.Add((x, double.NaN));
            }
            count++;
            if (count > 100) break;
        }
        return true;
    }

    public string[] GetDisplayItems()
    {
        var items = new List<string> { $"f(X) = {FunctionText}" };
        items.Add("  X          f(X)");
        foreach (var row in Rows)
        {
            string yStr = double.IsNaN(row.Y) ? "Error" : FormatVal(row.Y);
            items.Add($" {FormatVal(row.X)}   {yStr}");
        }
        return items.ToArray();
    }

    public int DisplayCount => Rows.Count + 2;

    private static string FormatVal(double v) => v.ToString("G6", CultureInfo.InvariantCulture);
}
