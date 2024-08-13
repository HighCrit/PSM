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
public class Diamond(IRegularFormula regularFormula, IModalFormula formula) : IModalFormula
{
    private IModalFormula Formula { get; set; } = formula;

    private IRegularFormula RegularFormula { get; set; } = regularFormula;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"<{this.RegularFormula}>{this.Formula}";
    }
}
