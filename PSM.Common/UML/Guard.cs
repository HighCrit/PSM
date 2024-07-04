// <copyright file="Guard.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.Common.UML;

public class Guard(string content) : Label
{
    public string Content { get; } = content;

    public override string ToString()
    {
        return this.Content;
    }
}
