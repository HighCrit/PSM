// <copyright file="Summation.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.RegularFormula.Operators;

/// <summary>
/// The summation operator for regular formulas.
/// </summary>
/// <param name="left">The left regular formula.</param>
/// <param name="right">The right regular formula.</param>
public class Summation(RegularFormulaBase left, RegularFormulaBase right) : RegularFormulaBase
{
    private RegularFormulaBase Left { get; set; } = left;

    private RegularFormulaBase Right { get; set; } = right;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({this.Left} + {this.Right})";
    }
}
