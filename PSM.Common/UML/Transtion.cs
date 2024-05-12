// <copyright file="Transtion.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.UML;

/// <summary>
/// A class representing a transition between <see cref="State"/>s.
/// </summary>
/// <param name="Source">The starting point of the transition.</param>
/// <param name="Target">The end point of the transition.</param>
/// <param name="Guard">The guard of the transition.</param>
public record Transition(State Source, State Target, string Guard);
