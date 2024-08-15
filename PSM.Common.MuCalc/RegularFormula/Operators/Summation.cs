// <copyright file="Summation.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.RegularFormula.Operators;

/// <summary>
/// The summation operator for regular formulas.
/// </summary>
/// <param name="left">The left regular formula.</param>
/// <param name="right">The right regular formula.</param>
public class Summation(IRegularFormula left, IRegularFormula right) : IRegularFormula
{
    public string ToLatex()
    {
        return $"{left.ToLatex()} + {right.ToLatex()}";
    }

    public string ToMCRL2()
    {
        return $"{left.ToMCRL2()} + {right.ToMCRL2()}";
    }
}
