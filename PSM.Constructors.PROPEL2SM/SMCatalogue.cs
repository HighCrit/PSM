// <copyright file="SMCatalogue.cs" company="HighCrit">
// Copyright (c) HighCrit. This file is released under GPLv3.
// See LICENSE for full license details.
// </copyright>

using System.Diagnostics;
using PSM.Common;
using PSM.Common.PROPEL;
using PSM.UML.SM;

namespace PSM.Constructors.PROPEL2MuCalc;

public static class SMCatalogue
{
    public static IStateMachine GetSM(Behaviour behaviour, Scope scope, Option option, IDictionary<Event, string> events)
    {
        return behaviour switch
        {
            Behaviour.Absence => GetAbsenceSM(scope, option, events),
            Behaviour.Existence => GetExistenceSM(scope, option, events),
            Behaviour.Response => GetResponseSM(scope, option, events),
            _ => throw new NotSupportedException($"The provided behaviour '{behaviour}' is currently not supported")
        };
    }

    private static IStateMachine GetAbsenceSM(Scope scope, Option option, IDictionary<Event, string> events)
    {
        return scope switch
        {
            Scope.Global => GetAbsenceGlobalSM(option, events),
            Scope.Between => GetAbsenceBetweenSM(option, events),
            _ => throw new NotSupportedException(
                $"The provided scope '{scope}' is currently not supported for the absence behaviour")
        };
    }

    private static IStateMachine GetExistenceSM(Scope scope, Option option, IDictionary<Event, string> events)
    {
        return scope switch
        {
            Scope.Between => GetExistenceBetweenSM(option, events),
            _ => throw new NotSupportedException(
                $"The provided scope '{scope}' is currently not supported for the existence behaviour")
        };
    }

    private static IStateMachine GetPrecedenceSM(Scope scope, Option option, IDictionary<Event, string> events)
    {
        return scope switch
        {
            Scope.Between => GetPrecedenceBetweenSM(option, events),
            _ => throw new NotSupportedException(
                $"The provided scope '{scope}' is currently not supported for the precedence behaviour")
        };
    }

    private static IStateMachine GetResponseSM(Scope scope, Option option, IDictionary<Event, string> events)
    {
        return scope switch
        {
            Scope.Global => GetResponseGlobalSM(option, events),
            Scope.Between => GetResponseBetweenSM(option, events),
            _ => throw new NotSupportedException(
                $"The provided scope '{scope}' is currently not supported for the absence behaviour")
        };
    }

    #region Absence
    private static IStateMachine GetAbsenceGlobalSM(Option option, IDictionary<Event, string> events)
    {
        // Global absence doesn't have options.
        Debug.Assert(option is Option.None);

        var sm = new StateMachine();
        var initial = new State(StateType.Initial);
        var one = new State(StateType.Normal, "Accepting", "1");
        var two = new State(StateType.Normal, text: "2");
        sm.AddStates(initial, one, two);

        initial.AddTransition(one, string.Empty);
        one.AddTransition(two, $"[{events[Event.A]}]");
        one.AddTransition(one, $"[NOT ({events[Event.A]})]");
        two.AddTransition(two, "[TRUE]");

        return sm;
    }

    private static IStateMachine GetAbsenceBetweenSM(Option option, IDictionary<Event, string> events)
    {
        var sm = new StateMachine();
        var initial = new State(StateType.Initial);
        var one = new State(StateType.Normal, "Accepting", "1");
        var two = new State(StateType.Normal, "Accepting", "2");
        var three = new State(StateType.Normal, option.HasFlag(Option.OptionalEnd) ? "Accepting" : string.Empty, "3");
        sm.AddStates(initial, one, two, three);

        initial.AddTransition(one, string.Empty);

        one.AddTransition(one, $"[NOT ({events[Event.Start]})]");
        one.AddTransition(two, $"[{events[Event.Start]}]");

        two.AddTransition(two, $"[NOT (({events[Event.A]}) OR ({events[Event.End]}))]");
        two.AddTransition(one, $"[{events[Event.End]}]");
        two.AddTransition(three, $"[{events[Event.A]}]");

        if (option.HasFlag(Option.FirstStart))
        {
            three.AddTransition(three, $"[NOT ({events[Event.End]})]");
        }
        else // Last Start
        {
            three.AddTransition(three, $"[NOT (({events[Event.Start]}) OR ({events[Event.End]}))]");
            three.AddTransition(two, $"[{events[Event.Start]}]");
        }

        return sm;
    }
    #endregion

    #region Existence
    private static IStateMachine GetExistenceBetweenSM(Option option, IDictionary<Event, string> events)
    {
        var sm = new StateMachine();
        var initial = new State(StateType.Initial);
        var one = new State(StateType.Normal, "Accepting", "1");
        var two = new State(StateType.Normal, option.HasFlag(Option.OptionalEnd) ? string.Empty : "Accepting", "2");
        var three = new State(StateType.Normal, "Accepting", "3");
        sm.AddStates(initial, one, two, three);

        one.AddTransition(one, $"[NOT ({events[Event.Start]})]");
        one.AddTransition(two, $"[{events[Event.Start]}]");

        two.AddTransition(two, $"[NOT (({events[Event.A]}) OR ({events[Event.End]}))]");
        two.AddTransition(three, $"[{events[Event.A]}]");

        three.AddTransition(one, $"{events[Event.End]}");
        if (option.HasFlag(Option.FirstStart))
        {
            three.AddTransition(three, $"[NOT ({events[Event.End]})]");
        }
        else
        {
            three.AddTransition(three, $"[NOT (({events[Event.Start]}) OR ({events[Event.End]}))]");
            three.AddTransition(two, $"[{events[Event.Start]}]");
        }

        return sm;
    }
    #endregion

    #region Precedence
    private static IStateMachine GetPrecedenceBetweenSM(Option option, IDictionary<Event, string> events)
    {
        var sm = new StateMachine();
        var initial = new State(StateType.Initial);
        var one = new State(StateType.Normal, "Accepting", "1");
        var two = new State(StateType.Normal, !option.HasFlag(Option.Nullity) ? string.Empty : "Accepting", "2");
        var three = new State(StateType.Normal, "Accepting", "3");
        var four = new State(StateType.Normal, "Accepting", "4");
        sm.AddStates(initial, one, two, three, four);

        initial.AddTransition(one, string.Empty);

        // One
        one.AddTransition(one, $"[NOT ({events[Event.Start]})]");
        one.AddTransition(two, $"[{events[Event.Start]}]");

        // Two
        two.AddTransition(two, $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.End]}))]");
        if (option.HasFlag(Option.Nullity))
        {
            two.AddTransition(one, $"[{events[Event.End]}]");
        }
        two.AddTransition(three, $"[{events[Event.A]}]");

        // Three
        if (!option.HasFlag(Option.Immediacy))
        {
            three.AddTransition(one, $"[{events[Event.End]}]");
        }
        if (!option.HasFlag(Option.FirstStart) && !option.HasFlag(Option.Immediacy)) // Last start
        {
            three.AddTransition(two, $"[{events[Event.Start]}]");
        }
        three.AddTransition(four, $"[{events[Event.B]}]");

        string? threeSelfLoop = null;
        if (option.HasFlag(Option.Immediacy) && option.HasFlag(Option.PreArity))
        {
            threeSelfLoop = $"[{events[Event.A]}]";
        }
        else if (option.HasFlag(Option.Immediacy) && !option.HasFlag(Option.FirstStart))
        {
            threeSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.End]}))]";
        }
        else if (option.HasFlag(Option.Immediacy) && option.HasFlag(Option.FirstStart))
        {
            threeSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
        } else if (option.HasFlag(Option.FirstStart))
        {
            threeSelfLoop = $"[NOT (({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
        }
        else
        {
            threeSelfLoop = $"[NOT (({events[Event.B]}) OR ({events[Event.End]}))]";
        }
        three.AddTransition(three, threeSelfLoop!);

        // Four
        four.AddTransition(one, $"[{events[Event.End]}]");
        if (!option.HasFlag(Option.FirstStart) || option.HasFlag(Option.OptionalEnd))
        {
            four.AddTransition(two, $"[{events[Event.Start]}]");
        }
        if (option.HasFlag(Option.Repeatability) && !option.HasFlag(Option.Finalisation))
        {
            four.AddTransition(three, $"[{events[Event.A]}]");
        }

        string? fourSelfLoop = null;
        if (option.HasFlag(Option.Repeatability) && !option.HasFlag(Option.Finalisation))
        {
            if (option.HasFlag(Option.PostArity))
            {
                if (option.HasFlag(Option.OptionalEnd))
                {
                    fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                } 
                else
                {
                    fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.End]}))]";
                }
            } 
            else
            {
                if (option.HasFlag(Option.OptionalEnd))
                {
                    fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                }
                else
                {
                    fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.End]}))]";
                }
            }
        }
        else
        {
            if (option.HasFlag(Option.Finalisation))
            {
                if (option.HasFlag(Option.PostArity))
                {
                    if (option.HasFlag(Option.OptionalEnd))
                    {
                        fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                    } 
                    else
                    {
                        fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.End]}))]";
                    }
                }
                else
                {
                    if (option.HasFlag(Option.OptionalEnd))
                    {
                        fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                    }
                    else
                    {
                        fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.End]}))]";
                    }
                }
            }
            else
            {
                if (option.HasFlag(Option.PostArity))
                {
                    if (option.HasFlag(Option.OptionalEnd))
                    {
                        fourSelfLoop = $"[NOT ({events[Event.Start]}) OR ({events[Event.End]})]";
                    }
                    else
                    {
                        fourSelfLoop = $"[NOT ({events[Event.End]})]";
                    }
                }
                else
                {
                    if (option.HasFlag(Option.OptionalEnd))
                    {
                        fourSelfLoop = $"[NOT (({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                    }
                    else
                    {
                        fourSelfLoop = $"[NOT (({events[Event.B]}) OR ({events[Event.End]}))]";
                    }
                }
            }
        }
        four.AddTransition(four, fourSelfLoop!);

        return sm;
    }
    #endregion

    #region Response
    private static IStateMachine GetResponseGlobalSM(Option option, IDictionary<Event, string> events)
    {
        var sm = new StateMachine();
        var initial = new State(StateType.Initial);
        var one = new State(StateType.Normal, option.HasFlag(Option.Nullity) ? "Accepting" : string.Empty, "1");
        var two = new State(StateType.Normal, text: "2");
        var three = new State(StateType.Normal, "Accepting", "3");
        sm.AddStates(initial, one, two, three);

        initial.AddTransition(one, string.Empty);

        if (option.HasFlag(Option.Precedency))
        {
            one.AddTransition(one, $"[NOT ({events[Event.A]})]");
        }
        else
        {
            one.AddTransition(one, $"[NOT (({events[Event.A]}) OR ({events[Event.B]}))]");
        }
        one.AddTransition(two, $"[{events[Event.A]}]");

        two.AddTransition(three, $"[{events[Event.B]}]");
        if (option.HasFlag(Option.Immediacy) && option.HasFlag(Option.PreArity))
        {
            two.AddTransition(two, $"[{events[Event.A]}]");
        }
        else if (option.HasFlag(Option.Immediacy) && !option.HasFlag(Option.PreArity))
        {
            two.AddTransition(two, $"[NOT (({events[Event.A]}) OR ({events[Event.B]}))]");
        }
        else
        {
            two.AddTransition(two, $"[NOT ({events[Event.B]})]");
        }

        if (!option.HasFlag(Option.Finalisation) && option.HasFlag(Option.Repeatability))
        {
            three.AddTransition(two, $"[{events[Event.A]}]");
        }

        if (option.HasFlag(Option.Finalisation) || option.HasFlag(Option.Repeatability))
        {
            if (option.HasFlag(Option.PostArity))
            {
                three.AddTransition(three, $"[NOT ({events[Event.A]})]");
            } 
            else
            {
                three.AddTransition(three, $"[NOT (({events[Event.A]}) OR ({events[Event.B]}))]");
            }
        }
        else
        {
            if (option.HasFlag(Option.PostArity))
            {
                three.AddTransition(three, "[TRUE]");
            }
            else
            {
                three.AddTransition(three, $"[NOT ({events[Event.B]})]");
            }
        }

        return sm;
    }
    
    private static IStateMachine GetResponseBetweenSM(Option option, IDictionary<Event, string> events)
    {
        var sm = new StateMachine();
        var initial = new State(StateType.Initial);
        var one = new State(StateType.Normal, "Accepting", "1");
        var two = new State(StateType.Normal, !option.HasFlag(Option.Nullity) ? string.Empty : "Accepting", "2");
        var three = new State(StateType.Normal, option.HasFlag(Option.OptionalEnd) ? string.Empty : "Accepting", "3");
        var four = new State(StateType.Normal, "Accepting", "4");
        sm.AddStates(initial, one, two, three, four);

        initial.AddTransition(one, string.Empty);

        // One
        one.AddTransition(one, $"[NOT ({events[Event.Start]})]");
        one.AddTransition(two, $"[{events[Event.Start]}]");

        // Two
        if (option.HasFlag(Option.Precedency))
        {
            two.AddTransition(two, $"[NOT (({events[Event.A]}) OR ({events[Event.End]}))]");
        }
        else
        {
            two.AddTransition(two, $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.End]}))]");
        }
        if (option.HasFlag(Option.Nullity))
        {
            two.AddTransition(one, $"[{events[Event.End]}]");
        }
        two.AddTransition(three, $"[{events[Event.A]}]");

        // Three
        if (!option.HasFlag(Option.FirstStart) && !option.HasFlag(Option.Immediacy)) // Last start
        {
            three.AddTransition(two, $"[{events[Event.Start]}]");
        }
        three.AddTransition(four, $"[{events[Event.B]}]");

        string? threeSelfLoop = null;
        if (option.HasFlag(Option.Immediacy) && option.HasFlag(Option.PreArity))
        {
            threeSelfLoop = $"[{events[Event.A]}]";
        }
        else if (option.HasFlag(Option.Immediacy) && !option.HasFlag(Option.FirstStart))
        {
            threeSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.End]}))]";
        }
        else if (option.HasFlag(Option.Immediacy) && option.HasFlag(Option.FirstStart))
        {
            threeSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
        }
        else if (option.HasFlag(Option.FirstStart))
        {
            threeSelfLoop = $"[NOT (({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
        }
        else
        {
            threeSelfLoop = $"[NOT (({events[Event.B]}) OR ({events[Event.End]}))]";
        }
        three.AddTransition(three, threeSelfLoop!);

        // Four
        four.AddTransition(one, $"[{events[Event.End]}]");
        if (!option.HasFlag(Option.FirstStart) || option.HasFlag(Option.OptionalEnd))
        {
            four.AddTransition(two, $"[{events[Event.Start]}]");
        }
        if (option.HasFlag(Option.Repeatability) && !option.HasFlag(Option.Finalisation))
        {
            four.AddTransition(three, $"[{events[Event.A]}]");
        }

        string? fourSelfLoop = null;
        if (option.HasFlag(Option.Repeatability) && !option.HasFlag(Option.Finalisation))
        {
            if (option.HasFlag(Option.PostArity))
            {
                if (option.HasFlag(Option.OptionalEnd))
                {
                    fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                }
                else
                {
                    fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.End]}))]";
                }
            }
            else
            {
                if (option.HasFlag(Option.OptionalEnd))
                {
                    fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                }
                else
                {
                    fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.End]}))]";
                }
            }
        }
        else
        {
            if (option.HasFlag(Option.Finalisation))
            {
                if (option.HasFlag(Option.PostArity))
                {
                    if (option.HasFlag(Option.OptionalEnd))
                    {
                        fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                    }
                    else
                    {
                        fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.End]}))]";
                    }
                }
                else
                {
                    if (option.HasFlag(Option.OptionalEnd))
                    {
                        fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                    }
                    else
                    {
                        fourSelfLoop = $"[NOT (({events[Event.A]}) OR ({events[Event.B]}) OR ({events[Event.End]}))]";
                    }
                }
            }
            else
            {
                if (option.HasFlag(Option.PostArity))
                {
                    if (option.HasFlag(Option.OptionalEnd))
                    {
                        fourSelfLoop = $"[NOT ({events[Event.Start]}) OR ({events[Event.End]})]";
                    }
                    else
                    {
                        fourSelfLoop = $"[NOT ({events[Event.End]})]";
                    }
                }
                else
                {
                    if (option.HasFlag(Option.OptionalEnd))
                    {
                        fourSelfLoop = $"[NOT (({events[Event.B]}) OR ({events[Event.Start]}) OR ({events[Event.End]}))]";
                    }
                    else
                    {
                        fourSelfLoop = $"[NOT (({events[Event.B]}) OR ({events[Event.End]}))]";
                    }
                }
            }
        }
        four.AddTransition(four, fourSelfLoop!);

        return sm;
    }
    #endregion
}