// <copyright file="MuFixPoint.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula;

/// <summary>
/// The least fixed point.
/// </summary>
public class MuFixPoint : ModalFormulaBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MuFixPoint"/> class.
    /// </summary>
    /// <param name="id">The fixpoint's id.</param>
    /// <param name="formula">The sub-formulas.</param>
    /// <param name="parameters">The fixpoint's parameters.</param>
    public MuFixPoint(string id, ModalFormulaBase formula, IEnumerable<Parameter>? parameters = null)
    {
        this.Id = id;
        this.Formula = formula;
        this.Parameters = parameters;
    }

    private string Id { get; set; }

    private ModalFormulaBase Formula { get; set; }

    private IEnumerable<Parameter>? Parameters { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Parameters is null
                    ? $"mu {this.Id} . {this.Formula})"
                    : $"mu {this.Id} ({string.Join(',', this.Parameters)}) . {this.Formula})";
    }
}
