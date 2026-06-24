using System;
using System.Text;

namespace SmartCalculator.Models;

public class MatrixValue
{
    public int Rows { get; }
    public int Cols { get; }
    public double[,] Data { get; }

    public MatrixValue(int rows, int cols)
    {
        if (rows < 1 || cols < 1) throw new ArgumentException("Dimensions must be at least 1");
        Rows = rows; Cols = cols;
        Data = new double[rows, cols];
    }

    public MatrixValue(double[,] data)
    {
        Rows = data.GetLength(0);
        Cols = data.GetLength(1);
        Data = (double[,])data.Clone();
    }

    public double this[int r, int c]
    {
        get => Data[r, c];
        set => Data[r, c] = value;
    }

    public MatrixValue Clone() => new(Data);

    public bool IsSquare => Rows == Cols;

    public override string ToString()
    {
        var sb = new StringBuilder("[");
        for (int r = 0; r < Rows; r++)
        {
            if (r > 0) sb.Append(",");
            sb.Append("[");
            for (int c = 0; c < Cols; c++)
            {
                if (c > 0) sb.Append(",");
                sb.Append(Data[r, c].ToString("G10", System.Globalization.CultureInfo.InvariantCulture));
            }
            sb.Append("]");
        }
        sb.Append("]");
        return sb.ToString();
    }

    public string ToMultiLineString()
    {
        var sb = new StringBuilder();
        for (int r = 0; r < Rows; r++)
        {
            if (r > 0) sb.Append(", ");
            sb.Append("[");
            for (int c = 0; c < Cols; c++)
            {
                if (c > 0) sb.Append(",");
                sb.Append(Data[r, c].ToString("G10", System.Globalization.CultureInfo.InvariantCulture));
            }
            sb.Append("]");
        }
        return sb.ToString();
    }
}
