// <copyright file="Cli.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

namespace PSM.CLI;

using PSM.Parsers.Cordis;
using PSM.Translators.MuCalc;

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
        var parser = new CordisParser();
        var psms = parser.Parse(@"C:\Users\dandor\Desktop\WashingMachine_C.xml");

        var translator = new TranslateToMuCalc();
        foreach (var (name, sm) in psms)
        {
            var res = translator.Translate(sm);
        }
    }
}
