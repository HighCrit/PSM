// <copyright file="IRegularFormula.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.RegularFormula;

/// <summary>
/// The base class for regular formulas.
/// </summary>
public interface IRegularFormula : IMuCalc
{
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

    public string ToString => this.ToMCRL2();
}