// <copyright file="StateMachine.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.UML;

/// <summary>
/// The class representing the state-machine from the diagram.
/// </summary>
public class StateMachine
{
    private Dictionary<string, State> States { get; } = [];

    /// <summary>
    /// Finds or creates a state with the given name.
    /// </summary>
    /// <param name="name">Name of state.</param>
    /// <returns>A state with the given name.</returns>
    public State FindOrCreate(string name, StateType type = StateType.Normal)
    {
        if (this.States.TryGetValue(name, out State? state))
        {
            return state;
        }

        var newState = new State(name);
        this.States.Add(name, newState);
        return newState;
    }
}
