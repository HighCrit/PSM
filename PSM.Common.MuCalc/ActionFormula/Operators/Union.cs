// <copyright file="Union.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ActionFormula.Operators;

using Action = PSM.Common.MuCalc.Actions.Action;

/// <summary>
/// The union operator for action formulas.
/// </summary>
/// <param name="left">The left hand formula.</param>
/// <param name="right">The right hand formula.</param>
public class Union(IActionFormula left, IActionFormula right) : IActionFormula
{
    public IActionFormula Left { get; } = left;

    public IActionFormula Right { get; } = right;
    public IActionFormula Flatten()
    {
        var left = this.Left.Flatten();
        var right = this.Right.Flatten();

        if (left.Equals(new ActionFormula(Action.True)) || right.Equals(new ActionFormula(Action.True)))
        {
            return new ActionFormula(Action.True);
        }
        if (left.Equals(new ActionFormula(Action.False)))
        {
            return right;
        }
        if (right.Equals(new ActionFormula(Action.False)))
        {
            return left;
        }

        return new Union(left, right);
    }

    public string ToLatex()
    {
        return $"{this.Left.ToLatex()} \\cup {this.Right.ToLatex()}";
    }

    public string ToMCRL2()
    {
        return $"({this.Left.ToMCRL2()} || {this.Right.ToMCRL2()})";
    }

    public override bool Equals(object? obj)
    {
        return obj is Union union && 
               ((union.Left.Equals(this.Left) && union.Right.Equals(this.Right)) || 
                (union.Right.Equals(this.Left) && union.Left.Equals(this.Right)));
    }
}
