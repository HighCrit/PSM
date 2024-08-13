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
public class Union(IActionFormula left, IActionFormula right)
{
    private IActionFormula Left { get; set; } = left;

    private IActionFormula Right { get; set; } = right;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({this.Left} || {this.Right})";
    }
}
