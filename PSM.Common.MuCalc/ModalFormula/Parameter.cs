// <copyright file="Parameter.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.ModalFormula;

using PSM.Common.MuCalc.Common;

/// <summary>
/// A fixpoint parameter.
/// </summary>
/// <param name="name">The name of the parameter.</param>
/// <param name="domain">The domain of the parameter.</param>
/// <param name="value">The value of the parameter.</param>
public class Parameter(string name, Domain domain, object value)
{
    /// <summary>
    /// Gets the parameter's name.
    /// </summary>
    public string Name { get; } = name;

    public Domain Domain { get; } = domain;

    public object Value { get; } = value;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{this.Name}:{this.Domain} = {this.Value}";
    }
}
