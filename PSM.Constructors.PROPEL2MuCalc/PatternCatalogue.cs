// <copyright file="PatternCatalogue.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using System.Diagnostics;
using PSM.Common;
using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Dissections;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.ModalFormula.Operators;
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
            Behaviour.Existence => GetExistencePattern(scope, option),
            Behaviour.Response => GetResponsePattern(scope, option),
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

    private static IModalFormula GetExistencePattern(Scope scope, Option option)
    {
        return scope switch
        {
            Scope.Global => GetExistenceGlobalPattern(option),
            Scope.Between => GetExistenceBetweenPattern(option),
            _ => throw new NotSupportedException(
                $"The provided scope '{scope}' is currently not supported for the existence behaviour")
        };
    }
    
    private static IModalFormula GetResponsePattern(Scope scope, Option option)
    {
        return scope switch
        {
            Scope.Global => GetResponseGlobalPattern(option),
            Scope.Between => GetResponseBetweenPattern(option),
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
            Option.FirstStart | Option.ScopeRepeatability => new Box(
                new Kleene(Bool.True),
                new Phi(
                    PhiType.Pos,
                    Event.Start,
                    new Phi(
                        PhiType.Neg,
                        Event.End,
                        new Phi(PhiType.Pos, Event.A, new Phi(PhiType.Neg, Event.End, new Phi(PhiType.Pos, Event.End, Bool.False)))))),
            // [true*. START . (not END )*. A ] false
            Option.FirstStart | Option.OptionalEnd | Option.ScopeRepeatability => new Box(
                new Kleene(Bool.True),
                new Phi(
                    PhiType.Pos,
                    Event.Start,
                    new Phi(PhiType.Neg, Event.End, new Phi(PhiType.Pos, Event.A, Bool.False)))),
            _ => throw new NotSupportedException(
                $"The provided option combination '{option}' is currently not supported for the absence behaviour with between scope")
        };
    }
    #endregion

    #region Existence
    private static IModalFormula GetExistenceGlobalPattern(Option option)
    {
        return option switch
        {
            // [true*] mu X. <true> true and [not A ] X
            Option.None =>
                new Box(new Kleene(Bool.True), new MuFixPoint("X", new Conjunction(new Diamond(Bool.True, Bool.True), new Phi(PhiType.Fix, Event.A, new FixPoint("X"))))),
            // [(not P )*. P . (not P )*. P ] false
            Option.Bounded => 
                new Phi(PhiType.Neg, Event.A, new Phi(PhiType.Pos, Event.A, new Phi(PhiType.Neg, Event.A, new Phi(PhiType.Pos, Event.A, Bool.False)))),
            _ => throw new NotSupportedException(
                $"The provided option combination '{option}' is currently not supported for the existence behaviour with between scope")
        };
    }

    private static IModalFormula GetExistenceBetweenPattern(Option option)
    {
        return option switch
        {
            // [true*. START . (not ( A or END ))*. END ] false
            Option.FirstStart | Option.ScopeRepeatability =>
                new Box(new Kleene(Bool.True), new Phi(PhiType.Pos, Event.Start, new Phi(PhiType.Neg, Event.A | Event.End, new Phi(PhiType.Pos, Event.End, Bool.False)))),
            _ => throw new NotSupportedException(
                $"The provided option combination '{option}' is currently not supported for the existence behaviour with between scope")
        };
    }
    #endregion
    
    #region Response
    private static IModalFormula GetResponseGlobalPattern(Option option)
    {
        return option switch
        {
            // [true*. A ] mu X. <true> true and [not B ] X
            Option.Nullity | Option.Precedency | Option.PreArity | Option.PostArity | Option.Repeatability =>
                new Box(new Kleene(Bool.True),
                    new Phi(PhiType.Pos, Event.A,
                        new MuFixPoint("X",
                            new Conjunction(new Diamond(Bool.True, Bool.True),
                                new Phi(PhiType.Fix, Event.B, new FixPoint("X")))))),
            _ => throw new NotSupportedException(
                $"The provided option combination '{option}' is currently not supported for the response behaviour with global scope")
        };
    }
    
    private static IModalFormula GetResponseBetweenPattern(Option option)
    {
        return option switch
        {
            // [true*. START . (not END )*. A. (not ( B or END ))*. END ] false
            Option.Nullity | Option.Precedency | Option.PreArity | Option.PostArity | Option.Repeatability |
                Option.ScopeRepeatability | Option.FirstStart =>
                new Box(new Kleene(Bool.True),
                    new Phi(PhiType.Pos, Event.Start,
                        new Phi(PhiType.Neg, Event.End,
                            new Phi(PhiType.Pos, Event.A,
                                new Phi(PhiType.Neg, Event.B | Event.End,
                                    new Phi(PhiType.Pos, Event.End, Bool.False)))))),
            // [true*. START . (not END )*. A ] mu X. <true> true and [ END ] false and [not B ] X
            Option.Nullity | Option.Precedency | Option.PreArity | Option.PostArity | Option.Repeatability |
                Option.ScopeRepeatability | Option.FirstStart | Option.OptionalEnd =>
                new Box(new Kleene(Bool.True),
                    new Phi(PhiType.Pos, Event.Start,
                        new Phi(PhiType.Neg, Event.End,
                            new Phi(PhiType.Pos, Event.A, 
                                new MuFixPoint("X", 
                                    new Conjunction(
                                        new Diamond(Bool.True, Bool.True),
                                        new Conjunction(
                                            new Phi(PhiType.Pos, Event.End, Bool.False),
                                            new Phi(PhiType.Fix, Event.B, new FixPoint("X"))))))))),
            _ => throw new NotSupportedException(
                $"The provided option combination '{option}' is currently not supported for the response behaviour with between scope")
        };
    }
    #endregion
}