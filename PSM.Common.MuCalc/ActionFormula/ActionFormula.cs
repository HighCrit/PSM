// <copyright file="ActionFormula.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ActionFormula;

using PSM.Common.MuCalc.Actions;

/// <summary>
/// Action formual representing a single action.
/// </summary>
/// <param name="action">The represented action.</param>
public class ActionFormula(Action action) : IActionFormula
{
    private Action Action { get; } = action;

    /// <inheritdoc/>
    public override string ToString() => this.Action.ToString();
}
