// <copyright file="IModalFormula.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Dissections.Labels;

namespace PSM.Common.MuCalc.ModalFormula;

/// <summary>
/// The base class for modal formulas.
/// </summary>
public interface IModalFormula : IMuCalc
{
    public IModalFormula Flatten();

    /// <summary>
    /// Converts the mu-calculus formula to a string for pretty LaTeX markup.
    /// </summary>
    /// <param name="substitutions">The event substitutions used in the dissections</param>
    /// <returns>A latex mu-calculus formula.</returns>
    public string ToLatex(Dictionary<Event, IExpression> substitutions);

    /// <summary>
    /// Converts the mu-calculus formula to a string interpretable by the mCRL2 toolset.
    /// </summary>
    /// <param name="substitutions">The event substitutions used in the dissections</param>
    /// <returns>An mCRL2 mu-calculus formula.</returns>
    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions);

    public string ToString() => this.ToMCRL2(default);
}
