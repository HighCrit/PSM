// <copyright file="Box.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula;

using PSM.Common.MuCalc.Dissections.Labels;
using PSM.Common.MuCalc.RegularFormula;

/// <summary>
/// The box modality.
/// </summary>
/// <param name="innerFormula">The contained regular formula.</param>
/// <param name="formula">The subsequent modal formula.</param>
public class Box(IRegularFormula innerFormula, IModalFormula formula) : IModalFormula
{
    public IRegularFormula InnerFormula { get; } = innerFormula;

    public IModalFormula Formula { get; } = formula;

    /// <inheritdoc />
    public string ToLatex(Dictionary<Event, IExpression> substitutions)
    {
        return $"[{this.InnerFormula.ToLatex()}]{this.Formula.ToLatex(substitutions)}";
    }

    /// <inheritdoc />
    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions)
    {
        return $"[{this.InnerFormula.ToMCRL2()}]{this.Formula.ToMCRL2(substitutions)}";
    }
}
