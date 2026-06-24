using System.Collections.Generic;

namespace SmartCalculator.Models;

public enum KeyStatus
{
    Implemented,
    Partial,
    Deferred,
    VisualOnly
}

public class KeyDefinition
{
    public string CommandParameter { get; init; } = "";
    public string MainLabel { get; init; } = "";
    public string? ShiftLabel { get; init; }
    public string? AlphaLabel { get; init; }
    public string MainAction { get; init; } = "";
    public string? ShiftAction { get; init; }
    public string? AlphaAction { get; init; }
    public KeyStatus Status { get; init; } = KeyStatus.Implemented;
}

public static class KeyMaps
{
    public static readonly Dictionary<string, KeyDefinition> AllKeys = new()
    {
        ["0"] = new() { CommandParameter="0", MainLabel="0", ShiftLabel="Rnd", ShiftAction="Rnd", Status=KeyStatus.Implemented },
        ["1"] = new() { CommandParameter="1", MainLabel="1", ShiftLabel="STAT", ShiftAction="STAT", Status=KeyStatus.Deferred },
        ["2"] = new() { CommandParameter="2", MainLabel="2", ShiftLabel="CMPLX", ShiftAction="CMPLX", Status=KeyStatus.Deferred },
        ["3"] = new() { CommandParameter="3", MainLabel="3", ShiftLabel="BASE", ShiftAction="BASE", Status=KeyStatus.Deferred },
        ["4"] = new() { CommandParameter="4", MainLabel="4", ShiftLabel="MATRIX", ShiftAction="MATRIX", Status=KeyStatus.Deferred },
        ["5"] = new() { CommandParameter="5", MainLabel="5", ShiftLabel="VECTOR", ShiftAction="VECTOR", Status=KeyStatus.Deferred },
        ["6"] = new() { CommandParameter="6", MainLabel="6", ShiftLabel="VERIFY", ShiftAction="VERIFY", Status=KeyStatus.Deferred },
        ["7"] = new() { CommandParameter="7", MainLabel="7", ShiftLabel="CONST", ShiftAction="CONST", Status=KeyStatus.Implemented },
        ["8"] = new() { CommandParameter="8", MainLabel="8", ShiftLabel="CONV", ShiftAction="CONV", Status=KeyStatus.Implemented },
        ["9"] = new() { CommandParameter="9", MainLabel="9", ShiftLabel="CLR", ShiftAction="CLR", Status=KeyStatus.Implemented },
        ["."] = new() { CommandParameter=".", MainLabel=".", ShiftLabel="Ran#", ShiftAction="Ran#", AlphaLabel="RanInt", AlphaAction="RanInt", Status=KeyStatus.Implemented },
        ["+"] = new() { CommandParameter="+", MainLabel="+", ShiftLabel="Pol", ShiftAction="Pol", AlphaLabel="Int", AlphaAction="Int", Status=KeyStatus.Partial },
        ["−"] = new() { CommandParameter="−", MainLabel="−", ShiftLabel="Rec", ShiftAction="Rec", AlphaLabel="Intg", AlphaAction="Intg", Status=KeyStatus.Partial },
        ["×"] = new() { CommandParameter="×", MainLabel="×", ShiftLabel="nPr", ShiftAction="nPr", AlphaLabel="GCD", AlphaAction="GCD", Status=KeyStatus.Implemented },
        ["÷"] = new() { CommandParameter="÷", MainLabel="÷", ShiftLabel="nCr", ShiftAction="nCr", AlphaLabel="LCM", AlphaAction="LCM", Status=KeyStatus.Implemented },
        ["="] = new() { CommandParameter="=", MainLabel="=", Status=KeyStatus.Implemented },
        ["AC"] = new() { CommandParameter="AC", MainLabel="AC", ShiftLabel="OFF", ShiftAction="OFF", Status=KeyStatus.Implemented },
        ["DEL"] = new() { CommandParameter="DEL", MainLabel="DEL", ShiftLabel="INS", ShiftAction="INS", Status=KeyStatus.Implemented },
        ["ON"] = new() { CommandParameter="ON", MainLabel="ON", Status=KeyStatus.Implemented },
        ["SHIFT"] = new() { CommandParameter="SHIFT", MainLabel="SHIFT", Status=KeyStatus.Implemented },
        ["ALPHA"] = new() { CommandParameter="ALPHA", MainLabel="ALPHA", Status=KeyStatus.Implemented },
        ["MODE"] = new() { CommandParameter="MODE", MainLabel="MODE", Status=KeyStatus.Implemented },
        ["ABOUT"] = new() { CommandParameter="ABOUT", MainLabel="ⓘ", Status=KeyStatus.Implemented },
        ["CALC"] = new() { CommandParameter="CALC", MainLabel="CALC", ShiftLabel="SOLVE", ShiftAction="SOLVE", Status=KeyStatus.Partial },
        ["INTEGRAL"] = new() { CommandParameter="INTEGRAL", MainLabel="∫", ShiftLabel="d/dx", ShiftAction="DERIV", AlphaLabel="•", Status=KeyStatus.Partial },
        ["INVERSE"] = new() { CommandParameter="INVERSE", MainLabel="x⁻¹", ShiftLabel="x!", ShiftAction="fact", Status=KeyStatus.Implemented },
        ["LOGAB"] = new() { CommandParameter="LOGAB", MainLabel="log□", ShiftLabel="Σ", ShiftAction="Σ", Status=KeyStatus.Partial },
        ["FRAC"] = new() { CommandParameter="FRAC", MainLabel="□/□", ShiftLabel="a b/c", ShiftAction="SD", AlphaLabel="d/c", AlphaAction="d/c", Status=KeyStatus.Partial },
        ["√"] = new() { CommandParameter="√", MainLabel="√", ShiftLabel="∛", ShiftAction="∛", AlphaLabel="i", AlphaAction="i", Status=KeyStatus.Implemented },
        ["x²"] = new() { CommandParameter="x²", MainLabel="x²", ShiftLabel="x³", ShiftAction="x³", AlphaLabel="DEC", Status=KeyStatus.Deferred },
        ["xʸ"] = new() { CommandParameter="xʸ", MainLabel="xʸ", ShiftLabel="ʸ√x", AlphaLabel="HEX", Status=KeyStatus.Partial },
        ["log"] = new() { CommandParameter="log", MainLabel="log", ShiftLabel="10ˣ", ShiftAction="×10^", AlphaLabel="BIN", Status=KeyStatus.Deferred },
        ["ln"] = new() { CommandParameter="ln", MainLabel="ln", ShiftLabel="eˣ", ShiftAction="exp", AlphaLabel="OCT", Status=KeyStatus.Deferred },
        ["(-)"] = new() { CommandParameter="(-)", MainLabel="− (unary)", ShiftLabel="←", ShiftAction="LEFT", AlphaLabel="A", AlphaAction="A", Status=KeyStatus.Implemented },
        ["DMS"] = new() { CommandParameter="DMS", MainLabel="DMS", ShiftLabel="FACT", ShiftAction="fact", AlphaLabel="B", AlphaAction="B", Status=KeyStatus.Implemented },
        ["HYP"] = new() { CommandParameter="HYP", MainLabel="hyp", ShiftLabel="Abs", ShiftAction="abs", AlphaLabel="C", AlphaAction="C", Status=KeyStatus.Implemented },
        ["sin"] = new() { CommandParameter="sin", MainLabel="sin", ShiftLabel="sin⁻¹", ShiftAction="asin", AlphaLabel="D", AlphaAction="D", Status=KeyStatus.Implemented },
        ["cos"] = new() { CommandParameter="cos", MainLabel="cos", ShiftLabel="cos⁻¹", ShiftAction="acos", AlphaLabel="E", AlphaAction="E", Status=KeyStatus.Implemented },
        ["tan"] = new() { CommandParameter="tan", MainLabel="tan", ShiftLabel="tan⁻¹", ShiftAction="atan", AlphaLabel="F", AlphaAction="F", Status=KeyStatus.Implemented },
        ["RCL"] = new() { CommandParameter="RCL", MainLabel="RCL", ShiftLabel="STO", ShiftAction="STO", Status=KeyStatus.Partial },
        ["ENG"] = new() { CommandParameter="ENG", MainLabel="ENG", ShiftLabel="←", ShiftAction="LEFT", AlphaLabel="i", AlphaAction="i", Status=KeyStatus.Partial },
        ["("] = new() { CommandParameter="(", MainLabel="(", ShiftLabel="%", ShiftAction="%", AlphaLabel="X", AlphaAction="X", Status=KeyStatus.Implemented },
        [")"] = new() { CommandParameter=")", MainLabel=")", ShiftLabel=",", ShiftAction=",", AlphaLabel="Y", AlphaAction="Y", Status=KeyStatus.Implemented },
        ["SD"] = new() { CommandParameter="SD", MainLabel="S⇔D", ShiftLabel="a b/c", AlphaLabel="d/c", AlphaAction="d/c", Status=KeyStatus.Partial },
        ["M+"] = new() { CommandParameter="M+", MainLabel="M+", ShiftLabel="M−", ShiftAction="M-", AlphaLabel="M", AlphaAction="M", Status=KeyStatus.Implemented },
        ["×10^"] = new() { CommandParameter="×10^", MainLabel="×10ˣ", ShiftLabel="π", ShiftAction="π", AlphaLabel="e", AlphaAction="e", Status=KeyStatus.Implemented },
        ["Ans"] = new() { CommandParameter="Ans", MainLabel="Ans", ShiftLabel="DRG▶", ShiftAction="DRG▶", AlphaLabel="PreAns", AlphaAction="PreAns", Status=KeyStatus.Implemented },
    };

    public static readonly Dictionary<string, string> ShiftMap = new()
    {
        ["sin"] = "asin", ["cos"] = "acos", ["tan"] = "atan",
        ["log"] = "×10^", ["ln"] = "exp",
        ["√"] = "∛", ["x²"] = "x³", ["×"] = "nPr", ["÷"] = "nCr",
        ["HYP"] = "abs",
        ["DMS"] = "fact",
        ["RCL"] = "STO",
        ["M+"] = "M-",
        ["("] = "%",
        [")"] = ",",
        ["AC"] = "OFF",
        ["7"] = "CONST", ["8"] = "CONV", ["9"] = "CLR",
        ["0"] = "Rnd",
        ["1"] = "STAT", ["2"] = "CMPLX", ["3"] = "BASE",
        ["4"] = "MATRIX", ["5"] = "VECTOR", ["6"] = "VERIFY",
        ["+"] = "Pol", ["−"] = "Rec",
        ["INVERSE"] = "fact",
        ["LOGAB"] = "Σ",
        ["INTEGRAL"] = "DERIV",
        ["FRAC"] = "SD",
        ["(-)"] = "LEFT",
        ["DEL"] = "INS",
        ["×10^"] = "π",
        ["Ans"] = "DRG▶",
        ["."] = "Ran#",
        ["CALC"] = "SOLVE",
    };

    public static readonly Dictionary<string, string> AlphaMap = new()
    {
        ["(-)"] = "A", ["DMS"] = "B", ["HYP"] = "C",
        ["sin"] = "D", ["cos"] = "E", ["tan"] = "F",
        ["("] = "X", [")"] = "Y", ["M+"] = "M",
        ["×"] = "GCD", ["÷"] = "LCM",
        ["+"] = "Int", ["−"] = "Intg",
        ["."] = "RanInt", ["×10^"] = "e",
        ["Ans"] = "PreAns",
        ["√"] = "i", ["ENG"] = "i",
        ["x²"] = "DEC", ["xʸ"] = "HEX",
        ["log"] = "BIN", ["ln"] = "OCT",
        ["SD"] = "d/c", ["FRAC"] = "d/c",
    };
}
