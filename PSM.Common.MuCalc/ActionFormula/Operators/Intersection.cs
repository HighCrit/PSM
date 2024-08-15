// <copyright file="Intersection.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ActionFormula.Operators;

/// <summary>
/// The intersection operator between action formulas.
/// </summary>
/// <param name="left">The left hand side formula.</param>
/// <param name="right">The right hand side formula.</param>
public class Intersection(IActionFormula left, IActionFormula right) : IActionFormula
{
    private IActionFormula Left { get; } = left;

    private IActionFormula Right { get; } = right;

    public string ToLatex()
    {
        return $"{this.Left.ToLatex()} \\cap {this.Right.ToLatex()}";
    }

    public string ToMCRL2()
    {
        return $"({this.Left.ToMCRL2()} && {this.Right.ToMCRL2()})";
    }
}
