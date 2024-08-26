// <copyright file="Negation.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Dissections.Labels;

namespace PSM.Common.MuCalc.ModalFormula.Operators;

/// <summary>
/// The negation operator for modal formulas.
/// </summary>
/// <param name="formula">The sub-formula.</param>
public class Negation(IModalFormula formula) : IModalFormula
{
    public IModalFormula Formula { get; } = formula;

    public IModalFormula Flatten()
    {
        var formula = this.Formula.Flatten();

        if (formula is Negation negation)
        {
            return negation.Formula.Flatten();
        }
        if (formula.Equals(Bool.True))
        {
            return Bool.False;
        }
        if (formula.Equals(Bool.False))
        {
            return Bool.True;
        }

        return new Negation(formula);
    }

    public string ToLatex(Dictionary<Event, IExpression> substitutions)
    {
        return $@"\neg{this.Formula.ToLatex(substitutions)}";
    }

    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions)
    {
        return $"!{this.Formula.ToMCRL2(substitutions)}";
    }
}
