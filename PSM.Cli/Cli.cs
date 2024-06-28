// <copyright file="Cli.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using PSM.CLI.Parser;

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
        var parser = new CordisParser();
        var psms = parser.Parse(@"C:\Users\dortm\Desktop\01. Model\WashingMachine_C.xml");
    }
}
