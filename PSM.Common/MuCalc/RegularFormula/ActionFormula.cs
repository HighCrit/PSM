// <copyright file="ActionFormula.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.RegularFormula.Operators;

using PSM.Common.MuCalc.ActionFormula;

/// <summary>
/// Regular formula represeting an action formula.
/// </summary>
/// <param name="formula">The action formula.</param>
public class ActionFormula(IActionFormula formula) : IRegularFormula
{
    private IActionFormula Formula { get; } = formula;

    public override string ToString() => this.Formula.ToString();
}
