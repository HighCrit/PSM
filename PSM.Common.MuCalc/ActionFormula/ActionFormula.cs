// <copyright file="ActionFormula.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ActionFormula;

using PSM.Common.MuCalc.Actions;

/// <summary>
/// Action formula representing a single action.
/// </summary>
/// <param name="action">The represented action.</param>
public class ActionFormula(Action action) : IActionFormula
{
    private Action Action { get; } = action;

    public IActionFormula Flatten() => this;

    public string ToLatex()
    {
        return this.Action.ToLatex();
    }

    public string ToMCRL2()
    {
        return this.Action.ToMCRL2();
    }

    public override bool Equals(object? obj)
    {
        return obj is ActionFormula af && this.Action.Equals(af.Action);
    }
}
