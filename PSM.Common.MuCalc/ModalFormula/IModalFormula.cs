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
    /// <returns>A latex mu-calculus formula.</returns>
    public string ToLatex();

    /// <summary>
    /// Converts the mu-calculus formula to a string interpretable by the mCRL2 toolset.
    /// </summary>
    /// <returns>An mCRL2 mu-calculus formula.</returns>
    public string ToMCRL2();

    /// <summary>
    /// Applies the substitutions give the substitution dictionary.
    /// </summary>
    /// <param name="substitutions">The substitution dictionary.</param>
    /// <returns>The substituted formula.</returns>
    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions);
    
    public string ToString() => this.ToMCRL2();
}
