// <copyright file="Concatenation.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.RegularFormula.Operators;

/// <summary>
/// The concatenation operator for regular formulas.
/// </summary>
/// <param name="left">The left regular formula.</param>
/// <param name="right">The right regular formula.</param>
public class Concatenation(IRegularFormula left, IRegularFormula right) : IRegularFormula
{
    private IRegularFormula Left { get; } = left;

    private IRegularFormula Right { get; } = right;

    public IRegularFormula Flatten()
    {
        var left = this.Left.Flatten();
        var right = this.Right.Flatten();

        return new Concatenation(left, right);
    }

    public string ToLatex()
    {
        return $"{this.Left.ToLatex()} \\cdot {this.Right.ToLatex()}";
    }

    public string ToMCRL2()
    {
        return $"{this.Left.ToMCRL2()} . {this.Right.ToMCRL2()}";
    }
}
