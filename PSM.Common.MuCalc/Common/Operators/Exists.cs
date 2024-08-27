// <copyright file="Exists.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.Common.MuCalc.ActionFormula;
using PSM.Common.MuCalc.Dissections.Labels;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.RegularFormula;

namespace PSM.Common.MuCalc.Common.Operators;

/// <summary>
/// The existential quantifier.
/// </summary>
/// <typeparam name="T">Either ActionFormula or ModalFormula.</typeparam>
/// <param name="variableName">The name of the element variable.</param>
/// <param name="domain">The domain iterated over.</param>
/// <param name="formula">The sub-formula.</param>
public class Exists<T>(string variableName, Domain domain, T formula) : IModalFormula, IActionFormula
{
    private string VariableName { get; } = variableName;

    private Domain Domain { get; } = domain;

    private T Formula { get; } = formula;

    public IModalFormula Flatten() => new Exists<IModalFormula>(
        this.VariableName,
        this.Domain,
        (this.Formula as IModalFormula)!.Flatten());

    IActionFormula IActionFormula.Flatten() => new Exists<IActionFormula>(
        this.VariableName,
        this.Domain,
        (this.Formula as IActionFormula)!.Flatten());

    public string ToLatex()
    {
        var f = "";
        if (this.Formula is IModalFormula mf)
        {
            f = mf.ToLatex();
        }
        if (this.Formula is IActionFormula af)
        {
            f = af.ToLatex();
        }
        
        return $@"(\exists_{{{this.VariableName}:{this.Domain.Name}}} {f})";
    }

    public string ToMCRL2()
    {
        var f = "";
        if (this.Formula is IModalFormula mf)
        {
            f = mf.ToMCRL2();
        }
        if (this.Formula is IActionFormula af)
        {
            f = af.ToMCRL2();
        }
        
        return $"(exists {this.VariableName}:{this.Domain.Name} . {f})";
    }

    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
    {
        if (this.Formula is IModalFormula modalFormula)
        {
            return new Exists<IModalFormula>(this.VariableName, this.Domain, modalFormula.ApplySubstitutions(substitutions));
        }
        return new Exists<IActionFormula>(this.VariableName, this.Domain, (this.Formula as IActionFormula)!);
    }
}
