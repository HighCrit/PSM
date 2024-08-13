// <copyright file="Exists.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.Common.Operators;

/// <summary>
/// The existential quantifier.
/// </summary>
/// <typeparam name="T">Either ActionFormula or ModalFormula.</typeparam>
/// <param name="variableName">The name of the element variable.</param>
/// <param name="domain">The domain iterated over.</param>
/// <param name="formula">The sub-formula.</param>
public class Exists<T>(string variableName, Domain domain, T formula)
{
    private string VariableName { get; set; } = variableName;

    private Domain Domain { get; set; } = domain;

    private T Formula { get; set; } = formula;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"(exists {this.VariableName}:{this.Domain.Name} . {this.Formula})";
    }
}
