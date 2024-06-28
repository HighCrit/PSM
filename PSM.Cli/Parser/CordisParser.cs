// <copyright file="UMLParser.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.CLI.Parser;

using Cordis2Cordis.XML;
using CordisSchema;
using PSM.Common.UML;

/// <summary>
/// A parser to convert UML diagrams from XML to mcf-files.
/// </summary>
public class CordisParser
{
    private const string PropertyPackageName = "Properties";

    /// <summary>
    /// Parses the provided UML state-machine diagram to a mu-calculus formula.
    /// </summary>
    /// <param name="filePath">The path to XML file containing the diagram.</param>
    /// <returns>The resulting modal formula.</returns>
    public IDictionary<string, Common.UML.StateMachine> Parse(string filePath)
    {
        CordisModel cordisModel = XMLParser.Load(filePath);

        var psms = new Dictionary<string, Common.UML.StateMachine>();

        var propertiesPackage = cordisModel.Packages.First(p => p.Name == PropertyPackageName)
            ?? throw new ArgumentException($"Expected cordis model to have a package named: '{PropertyPackageName}', found none.");

        if (propertiesPackage.MachineParts is null)
        {
            return psms;
        }

        foreach (var sm in propertiesPackage.MachineParts.SelectMany(mp => mp.StateMachines).Where(sm => sm.ClassName != "CmdExecution"))
        {
            var commonSm = new Common.UML.StateMachine();

            foreach (var s in sm.States.Items.OfType<CordisSchema.State>())
            {
                Common.UML.State commonState = s.Name switch
                {
                    "Initial" => commonSm.FindOrCreate(s.Name, StateType.Initial),
                    "Final" => commonSm.FindOrCreate(s.Name, StateType.Final),
                    _ => commonSm.FindOrCreate(s.Name)
                };

                foreach (var t in s.Transitions)
                {
                    var target = commonSm.FindOrCreate(t.Target);
                    commonState.AddTransition(target, t.Action);
                }
            }

            psms.Add(sm.Name, commonSm);
        }

        return psms;
    }
}
