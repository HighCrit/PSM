// <copyright file="Union.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ActionFormula.Operators;

/// <summary>
/// The union operator for action formulas.
/// </summary>
/// <param name="left">The left hand formula.</param>
/// <param name="right">The right hand formula.</param>
public class Union(IActionFormula left, IActionFormula right) : IActionFormula
{
    public IActionFormula Left { get; } = left;

    public IActionFormula Right { get; } = right;

    public string ToLatex()
    {
        return $"{this.Left.ToLatex()} \\cup {this.Right.ToLatex()}";
    }

    public string ToMCRL2()
    {
        return $"({this.Left.ToMCRL2()} || {this.Right.ToMCRL2()})";
    }
}
