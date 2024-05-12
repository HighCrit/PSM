// <copyright file="UMLParser.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.CLI.Parser;

using PSM.Common.MuCalc.ModalFormula;
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
    /// <returns>The resulting modal formula.</returns>
    public ModalFormulaBase? Parse(string filePath)
    {
        var doc = new XmlDocument();
        doc.Load(filePath);

        var diagrams = doc.DocumentElement!.SelectNodes("diagram")
            ?? throw new ArgumentException("No diagrams in file");

        foreach (XmlNode diagram in diagrams)
        {
            var elements = diagram.SelectNodes("//mxCell")
                ?? throw new ArgumentException("No elements in diagram");

            foreach (XmlNode element in elements)
            {
                if (!element.HasChildNodes)
                {
                    continue;
                }

                // Transition
                if (element.Attributes?["source"] is not null && element.Attributes["target"] is not null)
                {
                    // Parse transition
                } // Final State
                else if (element.Attributes?["style"]?.Value.Contains("shape=endState;") ?? false)
                {
                } // Initial State
                else if (element.Attributes?["style"]?.Value.Contains("ellipse;") ?? false)
                {
                } // Regular State
                else if (element.Attributes?["style"]?.Value.Contains("rounded=1;") ?? false)
                {
                }
            }
        }

        return default;
    }
}
