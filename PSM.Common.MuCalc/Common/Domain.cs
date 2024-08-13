// <copyright file="Domain.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.MuCalc.Common;

/// <summary>
/// A class representing a data domain.
/// </summary>
/// <param name="Name">The name of the domain.</param>
public record Domain(string Name)
{
    /// <summary>
    /// The built-in integer domain.
    /// </summary>
    public static readonly Domain INT = new ("Int");

    /// <summary>
    /// The built-in boolean domain.
    /// </summary>
    public static readonly Domain BOOL = new ("Bool");
}
