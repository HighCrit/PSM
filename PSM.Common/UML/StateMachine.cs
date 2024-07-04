// <copyright file="StateMachine.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.UML;

/// <summary>
/// The class representing the state-machine from the diagram.
/// </summary>
public class StateMachine(IDictionary<string, State>? states = null) : ICloneable
{
    public IDictionary<string, State> States { get; private set; } = states ?? new Dictionary<string, State>();

    /// <summary>
    /// Finds or creates a state with the given name.
    /// </summary>
    /// <param name="name">Name of state.</param>
    /// <returns>A state with the given name.</returns>
    public State FindOrCreate(string name)
    {
        if (this.States.TryGetValue(name, out State? state))
        {
            return state;
        }

        var newState = new State(name);
        this.States.Add(name, newState);
        return newState;
    }

    public void Remove(string name)
    {
        if (this.States.Remove(name) && this.States.Select(kvp => kvp.Value).Any(s => s.Transitions.Any(t => t.Target == name)))
        {
            throw new InvalidOperationException($"State with name '{name}' cannot be removed, is still in use.");
        }
    }

    public object Clone()
    {
        return new StateMachine(this.States.Select(kvp => (kvp.Key, (State)kvp.Value.Clone())).ToDictionary());
    }
}
