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
public class Intersection(ActionFormulaBase left, ActionFormulaBase right)
{
    private ActionFormulaBase Left { get; set; } = left;

    private ActionFormulaBase Right { get; set; } = right;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({this.Left} && {this.Right})";
    }
}
