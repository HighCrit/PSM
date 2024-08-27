// <copyright file="Cli.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using System.Formats.Asn1;
using PSM.Common;
using PSM.Common.PROPEL;
using PSM.Constructors.PROPEL2MuCalc;
using PSM.Parsers.Labels;
using PSM.Parsers.Labels.Labels;

namespace PSM.CLI;

/// <summary>
/// Class containing the entrypoint of program.
/// </summary>
public class Cli
{
    /// <summary>
    /// Entrypoint of the program.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        var behaviour = Inquire("What behaviour would you like to use?", Enum.GetValues<Behaviour>());
        var scope = Inquire("What scope would you like to use?", Enum.GetValues<Scope>());

        var availableOptions = (scope, behaviour).GetOptions();
        var selectedOptions = Option.None;
        foreach (var option in availableOptions)
        {
            selectedOptions |= Inquire($"Would you like to include {option}?", [Option.None, option]);
        }

        var pattern = PatternCatalogue.GetPattern(behaviour, scope, selectedOptions);

        var substitutions = new Dictionary<Event, IExpression>();
        foreach (var @event in (scope, behaviour).GetEvents())
        {
            substitutions.Add(@event, ReadExpr($"What label does {@event} represent?"));
        }
        var formula = pattern.ApplySubstitutions(substitutions).Flatten();

        var outputFormat = Inquire("What output format should be used?", Enum.GetValues<Output>());
        switch (outputFormat)
        {
            case Output.Latex:
                Console.WriteLine(formula.ToLatex());
                break;
            case Output.mCRL2:
                Console.WriteLine(formula.ToMCRL2());
                break;
            default:
                Console.WriteLine("Not a valid output format was given");
                break;
        }
    }

    private static T Inquire<T>(string message, T[] options)
    {
        Console.WriteLine($"{message}{Environment.NewLine}{string.Join(Environment.NewLine, options.Select((s, i) => $"({i}) {s}"))}");
        
        while (true)
        {
            var @in = Console.ReadLine();
            if (!int.TryParse(@in, out var i))
            {
                Console.WriteLine("Please enter a valid integer.");
            }
            else if (i < 0 || i >= options.Length)
            {
                Console.WriteLine($"Please enter a valid integer between 0 and {options.Length - 1}.");
            }
            else
            {
                return options[i];
            }
        }
    }

    private static IExpression ReadExpr(string message)
    {
        Console.WriteLine(message);
        var res = LabelParser.Parse(string.Empty + Console.ReadLine());
        while (res is null)
        {
            Console.WriteLine("Please enter a valid expression:");
            res = LabelParser.Parse(string.Empty + Console.ReadLine());
        }

        return res;
    }

    private enum Output
    {
        Latex,
        mCRL2
    }
}
