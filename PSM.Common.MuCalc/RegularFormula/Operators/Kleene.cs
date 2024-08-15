// <copyright file="Kleene.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.RegularFormula.Operators;

/// <summary>
/// The Kleene star operator for regular formulas.
/// </summary>
/// <param name="formula">The regular formula.</param>
public class Kleene(IRegularFormula formula) : IRegularFormula
{
    public string ToLatex()
    {
        return $"{formula.ToLatex()}^*";
    }

    public string ToMCRL2()
    {
        return $"{formula.ToMCRL2()}*";
    }
}
