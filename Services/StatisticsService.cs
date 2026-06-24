using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SmartCalculator.Models;

namespace SmartCalculator.Services;

public class StatisticsService
{
    public StatisticalDataset Data { get; } = new();
    public bool IsActive { get; set; }
    public string ErrorMessage { get; private set; } = "";

    public void AddValue(double v) => Data.Values.Add(v);
    public void Clear() { Data.Values.Clear(); Data.PairedValues.Clear(); ErrorMessage = ""; }
    public int Count => Data.Count;
    public double Sum => Data.Sum;
    public double SumSquares => Data.SumSquares;
    public double Mean => Data.Mean;
    public double SampleStdDev => Data.Count > 1 ? Data.StdDevSample : double.NaN;
    public double PopulationStdDev => Data.Count > 0 ? Data.StdDevPopulation : double.NaN;
    public double Min => Data.Count > 0 ? Data.Min : double.NaN;
    public double Max => Data.Count > 0 ? Data.Max : double.NaN;

    public static readonly string[] ResultLabels =
    {
        "n", "Σx", "Σx²", "x̄", "sx", "σx", "min", "max"
    };

    public double GetResult(int index) => index switch
    {
        0 => Count,
        1 => Sum,
        2 => SumSquares,
        3 => Mean,
        4 => SampleStdDev,
        5 => PopulationStdDev,
        6 => Min,
        7 => Max,
        _ => double.NaN
    };

    public string[] GetDisplayItems()
    {
        if (Data.Count == 0) return new[] { "No data" };
        return new[]
        {
            $"n = {Count}",
            $"Σx = {Sum:G6}",
            $"Σx² = {SumSquares:G6}",
            $"x̄ = {Mean:G6}",
            $"sx = {(double.IsNaN(SampleStdDev) ? "N/A (n<2)" : SampleStdDev.ToString("G6"))}",
            $"σx = {PopulationStdDev:G6}",
            $"min = {Min:G6}",
            $"max = {Max:G6}",
        };
    }

    public string[] GetMenuItems()
    {
        if (Data.Count == 0) return new[] { "1: n (no data)" };
        return new[]
        {
            $"1: n = {Count}",
            $"2: Σx = {Sum:G6}",
            $"3: Σx² = {SumSquares:G6}",
            $"4: x̄ = {Mean:G6}",
            $"5: sx = {(double.IsNaN(SampleStdDev) ? "N/A" : SampleStdDev.ToString("G6"))}",
            $"6: σx = {PopulationStdDev:G6}",
            $"7: min = {Min:G6}",
            $"8: max = {Max:G6}",
        };
    }

    public bool TryParseEntry(string text)
    {
        ErrorMessage = "";
        if (string.IsNullOrWhiteSpace(text))
        {
            ErrorMessage = "Empty data";
            return false;
        }

        string[] parts = text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0)
        {
            ErrorMessage = "Empty data";
            return false;
        }

        foreach (string part in parts)
        {
            if (!double.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
            {
                ErrorMessage = $"Invalid: \"{part}\"";
                return false;
            }
        }

        foreach (string part in parts)
        {
            if (double.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                Data.Values.Add(val);
        }

        return true;
    }

    public void ClearLast()
    {
        if (Data.Values.Count > 0)
            Data.Values.RemoveAt(Data.Values.Count - 1);
    }
}
