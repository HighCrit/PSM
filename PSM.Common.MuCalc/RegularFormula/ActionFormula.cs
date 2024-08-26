// <copyright file="ActionFormula.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.ActionFormula;

namespace PSM.Common.MuCalc.RegularFormula;

/// <summary>
/// Regular formula representing an action formula.
/// </summary>
/// <param name="formula">The action formula.</param>
public class ActionFormula(IActionFormula formula) : IRegularFormula
{
    private IActionFormula Formula { get; } = formula;

    public IRegularFormula Flatten() => new ActionFormula(this.Formula.Flatten());

    public string ToLatex() => this.Formula.ToLatex();

    public string ToMCRL2() => this.Formula.ToMCRL2();

    public override bool Equals(object? obj)
    {
        return obj is ActionFormula af && this.Formula.Equals(af.Formula);
    }
}
