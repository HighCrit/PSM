// <copyright file="Implication.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Common;
using PSM.Parsers.Labels.Labels;

namespace PSM.Common.MuCalc.ModalFormula.Operators;

/// <summary>
/// The implication operator for modal formulas.
/// </summary>
/// <param name="left">The left modal formula.</param>
/// <param name="right">The right modal formula.</param>
public class Implication(IModalFormula left, IModalFormula right) : IModalFormula
{
    public IModalFormula Left { get; } = left;

    public IModalFormula Right { get; } = right;

    public IModalFormula Flatten()
    {
        var left = this.Left.Flatten();
        var right = this.Right.Flatten();

        if (left.Equals(Bool.True))
        {
            return right;
        }
        if (left.Equals(Bool.False) || right.Equals(Bool.True))
        {
            return Bool.True;
        }

        return new Implication(left, right);
    }

    public string ToLatex()
    {
        return $@"({this.Left.ToLatex()} \Rightarrow {this.Right.ToLatex()})";
    }

    public string ToMCRL2()
    {
        return $"({this.Left.ToMCRL2()} -> {this.Right.ToMCRL2()})";
    }
    
    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
    {
        return new Implication(this.Left.ApplySubstitutions(substitutions), this.Right.ApplySubstitutions(substitutions));
    }
}
