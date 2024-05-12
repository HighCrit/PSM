// <copyright file="Conjunction.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula.Operators;

/// <summary>
/// The conjunction operator for modal formulas.
/// </summary>
/// <param name="left">The left modal formula.</param>
/// <param name="right">The right modal formula.</param>
public class Conjunction(ModalFormulaBase left, ModalFormulaBase right)
{
    private ModalFormulaBase Left { get; set; } = left;

    private ModalFormulaBase Right { get; set; } = right;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({this.Left} && {this.Right})";
    }
}
