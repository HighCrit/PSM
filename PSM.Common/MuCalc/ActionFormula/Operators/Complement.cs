// <copyright file="Complement.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ActionFormula.Operators;

/// <summary>
/// The complement operator for an action formula.
/// </summary>
/// <param name="formula">The formula.</param>
public class Complement(ActionFormulaBase formula)
{
    private ActionFormulaBase Formula { get; set; } = formula;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"!{this.Formula}";
    }
}
