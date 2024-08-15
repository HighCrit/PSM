// <copyright file="Disjunction.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Dissections.Labels;

namespace PSM.Common.MuCalc.ModalFormula.Operators;

/// <summary>
/// The disjunction operator for modal formulas.
/// </summary>
/// <param name="left">The left modal formula.</param>
/// <param name="right">The right modal formula.</param>
public class Disjunction(IModalFormula left, IModalFormula right) : IModalFormula
{
    public IModalFormula Left { get; } = left;

    public IModalFormula Right { get; } = right;

    public string ToLatex(Dictionary<Event, IExpression> substitutions)
    {
        return $@"({this.Left.ToLatex(substitutions)} \lor {this.Right.ToLatex(substitutions)})";
    }

    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions)
    {
        return $"({this.Left.ToMCRL2(substitutions)} || {this.Right.ToMCRL2(substitutions)})";
    }
}
