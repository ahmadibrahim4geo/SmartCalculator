using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCalculator.Models;

public class StatisticalDataset
{
    public List<double> Values { get; set; } = new();
    public List<(double X, double Y)> PairedValues { get; set; } = new();
    public bool IsPaired { get; set; }

    public int Count => Values.Count;
    public double Sum => Values.Sum();
    public double SumSquares => Values.Sum(v => v * v);
    public double Mean => Count > 0 ? Sum / Count : 0;
    public double VarianceSample => Count > 1 ? (SumSquares - Sum * Sum / Count) / (Count - 1) : 0;
    public double VariancePopulation => Count > 0 ? (SumSquares - Sum * Sum / Count) / Count : 0;
    public double StdDevSample => Math.Sqrt(VarianceSample);
    public double StdDevPopulation => Math.Sqrt(VariancePopulation);
    public double Min => Values.Count > 0 ? Values.Min() : 0;
    public double Max => Values.Count > 0 ? Values.Max() : 0;
}

