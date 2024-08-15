// <copyright file="Epsilon.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.RegularFormula;

public class Epsilon : IRegularFormula
{
    public static Epsilon E { get; } = new();

    private Epsilon()
    {
    }

    public string ToLatex()
    {
        return "ε";
    }

    public string ToMCRL2()
    {
        return "";
    }
}