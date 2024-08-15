// <copyright file="PatternCatalogue.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using System.Diagnostics;
using PSM.Common;
using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Dissections;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.RegularFormula.Operators;
using PSM.Common.PROPEL;

namespace PSM.Constructors.PROPEL2MuCalc;

public static class PatternCatalogue
{
    public static IModalFormula GetPattern(Behaviour behaviour, Scope scope, Option option)
    {
        return behaviour switch
        {
            Behaviour.Absence => GetAbsencePattern(scope, option),
            _ => throw new NotSupportedException($"The provided behaviour '{behaviour}' is currently not supported")
        };
    }

    private static IModalFormula GetAbsencePattern(Scope scope, Option option)
    {
        return scope switch
        {
            Scope.Global => GetAbsenceGlobalPattern(option),
            Scope.Between => GetAbsenceBetweenPattern(option),
            _ => throw new NotSupportedException(
                $"The provided scope '{scope}' is currently not supported for the absence behaviour")
        };
    }

    #region Absence
    private static IModalFormula GetAbsenceGlobalPattern(Option option)
    {
        // Global absence doesn't have options.
        Debug.Assert(option is Option.None);
        // [true*. A] false
        return new Box(new Kleene(Bool.True), new Phi(PhiType.Pos, Event.A, Bool.False));
    }

    private static IModalFormula GetAbsenceBetweenPattern(Option option)
    {
        return option switch
        {
            // [true *.START. (not END) *.A. (not END) *.END] false
            Option.FirstStart | Option.Repeatability => new Box(
                new Kleene(Bool.True),
                new Phi(
                    PhiType.Pos,
                    Event.Start,
                    new Phi(
                        PhiType.Neg,
                        Event.End,
                        new Phi(PhiType.Pos, Event.A, new Phi(PhiType.Neg, Event.End, new Phi(PhiType.Pos, Event.End, Bool.False)))))),
            _ => throw new NotSupportedException(
                $"The provided option combination '{option}' is currently not supported for the absence behaviour with between scope")
        };
    }
    #endregion
}