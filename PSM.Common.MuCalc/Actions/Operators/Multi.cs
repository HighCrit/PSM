// <copyright file="Multi.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.Actions.Operators;

/// <summary>
/// The multi-action operator for actions.
/// </summary>
/// <param name="left">The left action.</param>
/// <param name="right">The right action.</param>
public class Multi(Action left, Action right)
{
    private Action Left { get; set; } = left;

    private Action Right { get; set; } = right;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{this.Left}|{this.Right}";
    }
}
