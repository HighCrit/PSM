// <copyright file="MuFixPoint.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Parsers.Labels.Labels;

namespace PSM.Common.MuCalc.ModalFormula;

/// <summary>
/// The least fixed point.
/// </summary>
public class MuFixPoint : IModalFormula
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MuFixPoint"/> class.
    /// </summary>
    /// <param name="id">The fixpoint's id.</param>
    /// <param name="formula">The sub-formulas.</param>
    /// <param name="parameters">The fixpoint's parameters.</param>
    public MuFixPoint(string id, IModalFormula formula, IEnumerable<Parameter>? parameters = null)
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
        return new MuFixPoint(this.Id, this.Formula.Flatten(), this.Parameters);
    }

    public string ToLatex()
    {
        return this.Parameters is null
            ? $"\\mu {this.Id} . {this.Formula.ToLatex()}"
            : $"\\mu {this.Id} ({string.Join(',', this.Parameters)}) . {this.Formula.ToLatex()}";
    }

    public string ToMCRL2()
    {
        return this.Parameters is null
            ? $"mu {this.Id} . {this.Formula.ToMCRL2()}"
            : $"mu {this.Id} ({string.Join(',', this.Parameters)}) . {this.Formula.ToMCRL2()}";
    }
    
    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
    {
        return new MuFixPoint(this.Id, this.Formula.ApplySubstitutions(substitutions), this.Parameters);
    }
}
