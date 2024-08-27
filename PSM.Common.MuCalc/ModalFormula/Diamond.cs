// <copyright file="Diamond.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Parsers.Labels.Labels;

namespace PSM.Common.MuCalc.ModalFormula;

using PSM.Common.MuCalc.RegularFormula;

/// <summary>
/// The diamond modality.
/// </summary>
/// <param name="innerFormula">The contained regular formula.</param>
/// <param name="formula">The subsequent modal formula.</param>
public class Diamond(IRegularFormula innerFormula, IModalFormula formula) : IModalFormula
{
    public IRegularFormula InnerFormula { get; } = innerFormula;

    public IModalFormula Formula { get; } = formula;

    public IModalFormula Flatten()
    {
        return new Diamond(this.InnerFormula.Flatten(), this.Formula.Flatten());
    }

    /// <inheritdoc />
    public string ToLatex()
    {
        return $"\\langle{this.InnerFormula.ToLatex()}\\rangle{this.Formula.ToLatex()}";
    }

    /// <inheritdoc />
    public string ToMCRL2()
    {
        return $"<{this.InnerFormula.ToMCRL2()}>{this.Formula.ToMCRL2()}";
    }
    
    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
    {
        return new Diamond(this.InnerFormula, this.Formula.ApplySubstitutions(substitutions));
    }
}
