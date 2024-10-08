// <copyright file="Disjunction.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Common;
using PSM.Parsers.Labels.Labels;

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

    public IModalFormula Flatten()
    {
        var left = this.Left.Flatten();
        var right = this.Right.Flatten();

        if (left.Equals(Bool.True) || right.Equals(Bool.True))
        {
            return Bool.True;
        }
        if (left.Equals(Bool.False))
        {
            return right;
        }
        if (right.Equals(Bool.False))
        {
            return left;
        }
        if (left.Equals(right))
        {
            return left;
        }

        return new Disjunction(left, right);
    }

    public string ToLatex()
    {
        return $@"({this.Left.ToLatex()} \lor {this.Right.ToLatex()})";
    }

    public string ToMCRL2()
    {
        return $"({this.Left.ToMCRL2()} || {this.Right.ToMCRL2()})";
    }
    
    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
    {
        return new Disjunction(this.Left.ApplySubstitutions(substitutions), this.Right.ApplySubstitutions(substitutions));
    }
}
