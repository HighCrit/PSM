// <copyright file="Complement.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ActionFormula.Operators;

/// <summary>
/// The complement operator for an action formula.
/// </summary>
/// <param name="formula">The formula.</param>
public class Complement(IActionFormula formula) : IActionFormula
{
    private IActionFormula Formula { get; } = formula;

    public string ToLatex()
    {
        return $"\\overline{{{this.Formula.ToLatex()}}}";
    }

    public string ToMCRL2()
    {
        return $"!{this.Formula.ToMCRL2()}";
    }
}
