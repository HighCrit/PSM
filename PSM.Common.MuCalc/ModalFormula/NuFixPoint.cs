// <copyright file="NuFixPoint.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.Dissections.Labels;

namespace PSM.Common.MuCalc.ModalFormula;

/// <summary>
/// The greatest fixed point.
/// </summary>
public class NuFixPoint : IModalFormula
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NuFixPoint"/> class.
    /// </summary>
    /// <param name="id">The fixpoint's id.</param>
    /// <param name="formula">The sub-formulas.</param>
    /// <param name="parameters">The fixpoint's parameters.</param>
    public NuFixPoint(string id, IModalFormula formula, IEnumerable<Parameter>? parameters = null)
    {
        this.Id = id;
        this.Formula = formula;
        this.Parameters = parameters?.ToList();
    }

    public string Id { get; }

    public IModalFormula Formula { get; }

    public IList<Parameter>? Parameters { get; }

    public IModalFormula Flatten()
    {
        return new NuFixPoint(this.Id, this.Formula.Flatten(), this.Parameters);
    }

    /// <inheritdoc />
    public string ToLatex(Dictionary<Event, IExpression> substitutions)
    {
        return this.Parameters is null
            ? $"\\nu {this.Id} . {this.Formula.ToLatex(substitutions)})"
            : $"\\nu {this.Id} ({string.Join(',', this.Parameters)}) . {this.Formula.ToLatex(substitutions)})";
    }

    /// <inheritdoc />
    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions)
    {
        return this.Parameters is null
            ? $"nu {this.Id} . {this.Formula.ToMCRL2(substitutions)})"
            : $"nu {this.Id} ({string.Join(',', this.Parameters)}) . {this.Formula.ToMCRL2(substitutions)})";
    }
}
