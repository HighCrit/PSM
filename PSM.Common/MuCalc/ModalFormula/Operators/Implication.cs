// <copyright file="Implication.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula.Operators;

/// <summary>
/// The implication operator for modal formulas.
/// </summary>
/// <param name="left">The left modal formula.</param>
/// <param name="right">The right modal formula.</param>
public class Implication(IModalFormula left, IModalFormula right)
{
    private IModalFormula Left { get; set; } = left;

    private IModalFormula Right { get; set; } = right;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({this.Left} -> {this.Right})";
    }
}
