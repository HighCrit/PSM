// <copyright file="State.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.UML;

/// <summary>
/// A class representing a state of a state-machine diagram.
/// </summary>
/// <param name="name">The name of the state.</param>
public class State(string name)
{
    /// <summary>
    /// Gets the name of the state.
    /// </summary>
    public string Name { get; private set; } = name;

    /// <summary>
    /// Gets the list of transitions originating from this state.
    /// </summary>
    public List<Transition> Transitions { get; private set; } = [];

    /// <summary>
    /// Adds a transition to this state with the given target and guard.
    /// </summary>
    /// <param name="target">The target state of the transition.</param>
    /// <param name="guard">The transition's guard.</param>
    /// <returns>The source state.</returns>
    public State AddTransition(State target, string guard)
    {
        this.Transitions.Add(new Transition(this, target, guard));
        return this;
    }
}
