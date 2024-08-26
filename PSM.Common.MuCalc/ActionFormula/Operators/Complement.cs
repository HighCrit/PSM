// <copyright file="Complement.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using Action = PSM.Common.MuCalc.Actions.Action;

namespace PSM.Common.MuCalc.ActionFormula.Operators;

/// <summary>
/// The complement operator for an action formula.
/// </summary>
/// <param name="formula">The formula.</param>
public class Complement(IActionFormula formula) : IActionFormula
{
    private IActionFormula Formula { get; } = formula;

    public IActionFormula Flatten()
    {
        var formula = this.Formula.Flatten();

        if (formula.Equals(new ActionFormula(Action.True)))
        {
            return new ActionFormula(Action.False);
        }
        if (formula.Equals(new ActionFormula(Action.False)))
        {
            return new ActionFormula(Action.True);
        }

        return new Complement(formula);
    }

    public string ToLatex()
    {
        return $"\\overline{{{this.Formula.ToLatex()}}}";
    }

    public string ToMCRL2()
    {
        return $"!{this.Formula.ToMCRL2()}";
    }
}
