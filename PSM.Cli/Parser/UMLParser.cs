// <copyright file="UMLParser.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.CLI.Parser;

using System.Xml;

/// <summary>
/// A parser to convert UML diagrams from XML to mcf-files.
/// </summary>
public class UMLParser
{
    /// <summary>
    /// Parses the provided UML state-machine diagram to a mu-calculus formula.
    /// </summary>
    /// <param name="filePath">The path to XML file containing the diagram.</param>
    public void Parse(string filePath)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(filePath);
    }
}
