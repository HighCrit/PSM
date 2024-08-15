// <copyright file="Negation.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Dissections.Labels;

namespace PSM.Common.MuCalc.ModalFormula.Operators;

/// <summary>
/// The negation operator for modal formulas.
/// </summary>
/// <param name="formula">The sub-formula.</param>
public class Negation(IModalFormula formula) : IModalFormula
{
    public IModalFormula Formula { get; } = formula;

    public string ToLatex(Dictionary<Event, IExpression> substitutions)
    {
        return $@"\neg{this.Formula.ToLatex(substitutions)}";
    }

    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions)
    {
        return $"!{this.Formula.ToMCRL2(substitutions)}";
    }
}
