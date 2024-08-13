// <copyright file="Transtion.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.UML;

/// <summary>
/// A class representing a transition between <see cref="State"/>s.
/// </summary>
/// <param name="source">The starting point of the transition.</param>
/// <param name="target">The end point of the transition.</param>
/// <param name="label">The label of the transition.</param>
public class Transition(string source, string target, Label? label) : ICloneable
{
    public string Source { get; } = source;
    public string Target { get; } = target;
    public Label? Label { get; set; } = label;

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj is Transition t)
        {
            return this.Source == t.Source && this.Target == t.Target && this.Label == t.Label;
        }
        return false;
    }

    public object Clone()
    {
        return new Transition(this.Source, this.Target, this.Label);
    }

    public override int GetHashCode()
    {
        return (this.Source, this.Target, this.Label).GetHashCode();
    }
}
