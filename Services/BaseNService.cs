using System;
using System.Linq;
using System.Text;

namespace SmartCalculator.Services;

public class BaseNService
{
    public enum NumBase { DEC = 10, HEX = 16, BIN = 2, OCT = 8 }

    public NumBase CurrentBase { get; set; } = NumBase.DEC;

    // Expression display in BASE-N: raw user input before evaluation
    public string DisplayExpression { get; set; } = "";

    public string ToBase(long value)
    {
        if (value == 0 && CurrentBase == NumBase.HEX) return "0";
        if (value == 0) return "0";
        string result = CurrentBase switch
        {
            NumBase.HEX => value.ToString("X"),
            NumBase.BIN => Convert.ToString(value, 2),
            NumBase.OCT => Convert.ToString(value, 8),
            _ => value.ToString()
        };
        return result;
    }

    public string ToSignedBase(long value)
    {
        if (value >= 0) return ToBase(value);
        // Show negative sign + absolute value in current base
        return "-" + ToBase(Math.Abs(value));
    }

    public long FromString(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;
        text = text.Trim();
        bool neg = text.StartsWith('-');
        if (neg) text = text[1..];
        try
        {
            long val = CurrentBase switch
            {
                NumBase.HEX => Convert.ToInt64(text, 16),
                NumBase.BIN => Convert.ToInt64(text, 2),
                NumBase.OCT => Convert.ToInt64(text, 8),
                _ => long.TryParse(text, out var r) ? r : 0
            };
            return neg ? -val : val;
        }
        catch { return 0; }
    }

    public bool IsValidChar(char c)
    {
        return CurrentBase switch
        {
            NumBase.BIN => c is '0' or '1',
            NumBase.OCT => c is >= '0' and <= '7',
            NumBase.DEC => c is >= '0' and <= '9',
            NumBase.HEX => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'),
            _ => false
        };
    }

    public bool IsValidExpression(string expr)
    {
        if (string.IsNullOrEmpty(expr)) return true;
        // Split by spaces and check each token
        string[] tokens = expr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string token in tokens)
        {
            // Check for bitwise operator keywords
            if (token.Equals("AND", StringComparison.OrdinalIgnoreCase) ||
                token.Equals("OR", StringComparison.OrdinalIgnoreCase) ||
                token.Equals("XOR", StringComparison.OrdinalIgnoreCase) ||
                token.Equals("NOT", StringComparison.OrdinalIgnoreCase))
                continue;
            // Check each character
            foreach (char c in token)
            {
                if ("+-*/^%()".Contains(c)) continue;
                if (!IsValidChar(c)) return false;
            }
        }
        return true;
    }

    public string StripInvalidChars(string expr)
    {
        var sb = new StringBuilder();
        foreach (char c in expr)
        {
            if ("+-*/^%() ".Contains(c)) { sb.Append(c); continue; }
            if (IsValidChar(c)) sb.Append(c);
        }
        return sb.ToString();
    }

    public void CycleBase() =>
        CurrentBase = (NumBase)(((int)CurrentBase switch
        {
            10 => 16, 16 => 2, 2 => 8, 8 => 10, _ => 10
        }));

    public string BaseLabel => CurrentBase switch
    {
        NumBase.DEC => "DEC",
        NumBase.HEX => "HEX",
        NumBase.BIN => "BIN",
        NumBase.OCT => "OCT",
        _ => "DEC"
    };

    public bool IsBaseActive => CurrentBase != NumBase.DEC;

    // Bitwise operations (operate on long integers)
    public static long And(long a, long b) => a & b;
    public static long Or(long a, long b) => a | b;
    public static long Xor(long a, long b) => a ^ b;
    public static long Not(long a) => ~a;
}
