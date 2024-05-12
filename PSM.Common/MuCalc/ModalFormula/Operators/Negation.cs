// <copyright file="Negation.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula.Operators;

/// <summary>
/// The negation operator for modal formulas.
/// </summary>
/// <param name="formula">The sub-formula.</param>
public class Negation(ModalFormulaBase formula)
{
    private ModalFormulaBase Formula { get; set; } = formula;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"!{this.Formula}";
    }
}
