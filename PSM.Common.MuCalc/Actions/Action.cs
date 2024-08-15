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
    public static readonly Action Tau = new ("tau");

    /// <summary>
    /// The 'true' action, matching all actions.
    /// </summary>
    public static readonly Action True = new ("true");

    /// <summary>
    /// The 'false' action, matching no actions.
    /// </summary>
    public static readonly Action False = new ("false");

    private string Name { get; } = name;

    private IEnumerable<object>? Values { get; } = values;

    /// <summary>
    /// Converts the mu-calculus formula to a string for pretty LaTeX markup.
    /// </summary>
    /// <returns>A latex mu-calculus formula.</returns>
    public string ToLatex()
    {
        return this.Values is null
            ? $"\\mathit{{{this.Name}}}"
            : $"\\mathit{{{this.Name}}}({string.Join(',', this.Values)})";
    }

    /// <summary>
    /// Converts the mu-calculus formula to a string interpretable by the mCRL2 toolset.
    /// </summary>
    /// <returns>An mCRL2 mu-calculus formula.</returns>
    public string ToMCRL2()
    {
        return this.Values is null
            ? $"{this.Name}"
            : $"{this.Name}({string.Join(',', this.Values)})";
    }

    public override string ToString() => this.ToMCRL2();

    public override bool Equals(object? obj)
    {
        return obj is Action action && action.Name == this.Name;
    }
}
