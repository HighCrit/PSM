// <copyright file="FixPoint.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Dissections.Labels;

namespace PSM.Common.MuCalc.ModalFormula;

/// <summary>
/// A fixpoint.
/// </summary>
/// <param name="id">The fixpoint's id.</param>
/// <param name="values">The value to set.</param>
public class FixPoint(string id, IEnumerable<object>? values = null) : IModalFormula
{
    /// <summary>
    /// Gets the id of the fixpoint.
    /// </summary>
    public string Id { get; } = id;

    public IList<object>? Values { get; } = values?.ToList();

    public IModalFormula Flatten()
    {
        return this;
    }

    /// <inheritdoc />
    public string ToLatex()
    {
        return this.Values is null
            ? $"{this.Id}"
            : $"{this.Id}({string.Join(',', this.Values)})";
    }

    /// <inheritdoc />
    public string ToMCRL2()
    {
        return this.Values is null
            ? $"{this.Id}"
            : $"{this.Id}({string.Join(',', this.Values)})";
    }

    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
        => this;
}
