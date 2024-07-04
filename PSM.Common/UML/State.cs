// <copyright file="State.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.UML;

/// <summary>
/// A class representing a state of a state-machine diagram.
/// </summary>
/// <param name="name">The name of the state.</param>
/// <param name="type">The type of the state.</param>
public class State(string name, StateType type = StateType.Normal) : ICloneable
{
    /// <summary>
    /// Gets the name of the state.
    /// </summary>
    public string Name { get; private set; } = name;

    /// <summary>
    /// Gets the type of the state.
    /// </summary>
    public StateType Type { get; set; } = type;

    /// <summary>
    /// Gets the list of transitions originating from this state.
    /// </summary>
    public List<Transition> Transitions { get; private set; } = [];

    /// <summary>
    /// Adds a transition to this state with the given target and guard.
    /// </summary>
    /// <param name="target">The target state of the transition.</param>
    /// <param name="guard">The transition's guard.</param>
    /// <returns>The current state.</returns>
    public State AddTransition(string target, Label? label)
    {
        this.Transitions.Add(new Transition(this.Name, target, label));
        return this;
    }

    /// <inheritdoc cref="AddTransition(string, Label?)"/>
    public State AddTransition(State target, Label? label)
    {
        this.Transitions.Add(new Transition(this.Name, target.Name, label));
        return this;
    }

    /// <summary>
    /// Removes a transition of this state.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <returns>The current state.</returns>
    public State RemoveTransition(Transition transition)
    {
        this.Transitions.Remove(transition);
        return this;
    }

    /// <summary>
    /// Removes a transition of this state.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <returns>The current state.</returns>
    public State RemoveTransitionByTarget(State target)
    {
        this.Transitions.RemoveAll(t => t.Target.Equals(target));
        return this;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;

        if (obj is State s)
        {
            return s.Name.Equals(this.Name);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (this.Name, this.Type, this.Transitions).GetHashCode();
    }

    public object Clone()
    {
        var @new = new State(this.Name, this.Type)
        {
            Transitions = this.Transitions.Select(t => t.Clone()).Cast<Transition>().ToList()
        };

        return @new;
    }
}
