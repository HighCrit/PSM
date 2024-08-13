// <copyright file="FixPoint.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula;

/// <summary>
/// A fixpoint.
/// </summary>
/// <param name="id">The fixpoint's id.</param>
/// <param name="values">The value to set.</param>
public class FixPoint(string id, IEnumerable<object>? values = null) : IModalFormula
{
    private readonly IEnumerable<object>? values = values;

    /// <summary>
    /// Gets the id of the fixpoint.
    /// </summary>
    public string Id { get; } = id;

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.values is null
                    ? $"{this.Id}"
                    : $"{this.Id}({string.Join(',', this.values)})";
    }
}
