using System.Collections.Generic;
using SmartCalculator.Models;

namespace SmartCalculator.Services;

public class MemoryService
{
    private readonly Dictionary<string, double> _vars = new()
    {
        ["A"] = 0, ["B"] = 0, ["C"] = 0, ["D"] = 0, ["E"] = 0, ["F"] = 0,
        ["X"] = 0, ["Y"] = 0, ["M"] = 0
    };

    public double Ans { get; set; }
    public double PreAns { get; set; }

    public void Store(string name, double value)
    {
        var key = name.ToUpper();
        if (_vars.ContainsKey(key)) _vars[key] = value;
    }

    public double Recall(string name)
    {
        var key = name.ToUpper();
        return _vars.TryGetValue(key, out var v) ? v : 0;
    }

    public void AddToM(double value) => _vars["M"] += value;
    public void SubtractFromM(double value) => _vars["M"] -= value;
    public double M => _vars["M"];

    public void PushAns(double value) { PreAns = Ans; Ans = value; }

    public IEnumerable<string> VariableNames => _vars.Keys;
}
