// <copyright file="Action.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.Actions;

/// <summary>
/// An action.
/// </summary>
/// <param name="name">The action's name.</param>
/// <param name="values">The action's values.</param>
public class Action(string name, IEnumerable<object>? values = null)
{
    /// <summary>
    /// The tau action.
    /// </summary>
    public static readonly Action TAU = new ("tau");

    /// <summary>
    /// The `true' action, matching all actions.
    /// </summary>
    public static readonly Action TRUE = new ("true");

    /// <summary>
    /// The `false' action, matching no actions.
    /// </summary>
    public static readonly Action FALSE = new ("false");

    private string Name { get; set; } = name;

    private IEnumerable<object>? Values { get; set; } = values;

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Values is null
                ? $"{this.Name}"
                : $"{this.Name}({string.Join(',', this.Values)})";
    }
}
