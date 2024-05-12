// <copyright file="NuFixPoint.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula;

/// <summary>
/// The greatest fixed point.
/// </summary>
public class NuFixPoint : ModalFormulaBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NuFixPoint"/> class.
    /// </summary>
    /// <param name="id">The fixpoint's id.</param>
    /// <param name="formula">The sub-formulas.</param>
    /// <param name="parameters">The fixpoint's parameters.</param>
    public NuFixPoint(string id, ModalFormulaBase formula, IEnumerable<Parameter>? parameters = null)
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
                    ? $"nu {this.Id} . {this.Formula})"
                    : $"nu {this.Id} ({string.Join(',', this.Parameters)}) . {this.Formula})";
    }
}
