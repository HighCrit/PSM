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

    public string ToLatex(Dictionary<Event, IExpression> substitutions)
    {
        throw new NotImplementedException();
    }

    public string ToMCRL2(Dictionary<Event, IExpression>? substitutions)
    {
        if (this.Formula is IModalFormula modal)
        {
            return $"(exists {this.VariableName}:{this.Domain.Name} . {modal.ToMCRL2(substitutions)})";
        }

        if (this.Formula is IRegularFormula regular)
        {
            return $"(exists {this.VariableName}:{this.Domain.Name} . {regular.ToMCRL2()})";
        }

        throw new ArgumentException($"Invalid type {this.Formula!.GetType()}");
    }

    public string ToLatex()
    {
        throw new NotImplementedException();
    }

    public string ToMCRL2()
    {
        return $"(exists {this.VariableName}:{this.Domain.Name} . {this.Formula})";
    }
}
