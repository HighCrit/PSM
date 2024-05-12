// <copyright file="Diamond.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula;

using PSM.Common.MuCalc.RegularFormula;

/// <summary>
/// The diamond modality.
/// </summary>
/// <param name="regularFormula">The contained regular formula.</param>
/// <param name="formula">The subsequent modal formula.</param>
public class Diamond(RegularFormulaBase regularFormula, ModalFormulaBase formula) : ModalFormulaBase
{
    private ModalFormulaBase Formula { get; set; } = formula;

    private RegularFormulaBase RegularFormula { get; set; } = regularFormula;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"<{this.RegularFormula}>{this.Formula}";
    }
}
