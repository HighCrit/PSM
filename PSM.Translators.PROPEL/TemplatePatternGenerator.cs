using PSM.Common.UML;
using PSM.Translators.MuCalc.Rgx;
using System.Diagnostics;
using PSM.Common.PROPEL;
using PSM.Translators.PROPEL;

namespace PSM.Translators.MuCalc;

public class TemplatePatternGenerator
{
    private static readonly Guard GenericGuard = new("T");

    private List<(PropertyInfo Info, RegexBase? Rgx)> templatePatterns = [];
    private List<(string Signature, PropertyInfo Info)> templateSignatures = [];

    public IEnumerable<(PropertyInfo Info, RegexBase? Rgx)> TemplatePatterns
    {
        get
        {
            if (this.templatePatterns.Count == 0)
            {
                this.templatePatterns = this.GenerateAllPatterns().ToList();
            }
            return this.templatePatterns;
        }
    }

    public IEnumerable<(string Signature, PropertyInfo Info)> TemplateSignatures
    {
        get
        {
            if (this.templateSignatures.Count == 0)
            {
                this.templateSignatures = this.TemplatePatterns
                    .Where(t => t.Rgx is not null)
                    .Select(t => (t.Rgx!.ToString(true), t.Info))
                    .ToList();
                // Make sure that if there are duplicate signatures they are within the same behaviour and scope.
                Debug.Assert(
                    this.templateSignatures.All(
                        s1 => this.templateSignatures.All(
                            s2 => s1.Signature != s2.Signature || (s1.Info.Behaviour == s2.Info.Behaviour && s1.Info.Scope == s2.Info.Scope))));
            }
            return this.templateSignatures;
        }
    }

    public IEnumerable<(PropertyInfo Info, RegexBase? Rgx)> GenerateAllPatterns()
    {
        var result = new List<(PropertyInfo Info, RegexBase? Rgx)>();
        foreach (var behaviour in Enum.GetValues<Behaviour>()) foreach (var scope in Enum.GetValues<Scope>())
        {
            var options = PropertyInfo
                .GetAvailableOptionsFor(behaviour, scope)
                .GetFlags()
                .GetAllCombinations().ToList();

            foreach (var option in options)
            {
                var pattern = (behaviour, scope) switch
                {
                    (Behaviour.Absence, Scope.Global) => this.GenerateGlobalAbsencePattern(option),
                    (Behaviour.Existence, Scope.Global) => this.GenerateGlobalExistencePattern(option),
                    (Behaviour.Existence, Scope.After_Q) => this.GenerateAfterExistencePattern(option),
                    (Behaviour.Existence, Scope.Before_P) => this.GenerateBeforeExistencePattern(option),
                    (Behaviour.Existence, Scope.Between_Q_and_P) => this.GenerateBetweenExistencePattern(option),
                    _ => default,
                };

                result.Add((new(behaviour, scope, option), pattern));
            }
        }

        return result;
    }

    #region Absence
    public RegexBase GenerateGlobalAbsencePattern(Option option)
    {
        var initial = new State("Initial", StateType.Initial);
        var sink = new State("Sink", StateType.Invalid);
        var interMediate = new State("Intermediate", StateType.Normal);
        var sm = new StateMachine([initial, sink, interMediate]);

        initial.AddTransition(interMediate, null);
        interMediate.AddTransition(interMediate, GenericGuard);
        interMediate.AddTransition(sink, GenericGuard);

        return sm.ToRegEx().Flatten();
    }
    #endregion

    #region Existence
    public RegexBase GenerateGlobalExistencePattern(Option option)
    {
        var isBounded = option.HasFlag(Option.Bounded);

        var initial = new State("Initial", StateType.Initial);
        var final = new State("Final", isBounded ? StateType.Normal : StateType.Final);
        var interMediate = new State("Intermediate", StateType.Invalid);
        var sm = new StateMachine([initial, final, interMediate]);

        initial.AddTransition(interMediate, null);
        interMediate.AddTransition(interMediate, GenericGuard);
        interMediate.AddTransition(final, GenericGuard);
        if (isBounded) final.AddTransition(final, GenericGuard);

        return sm.ToRegEx().Flatten();
    }

    public RegexBase GenerateAfterExistencePattern(Option option)
    {
        var isBounded = option.HasFlag(Option.Bounded);
        var lastStart = option.HasFlag(Option.LastStart);

        var initial = new State("Initial", StateType.Initial);
        var state1 = new State("State 1", StateType.Normal);
        var state2 = new State("State 2", StateType.Invalid);
        var state3 = new State("State 3", StateType.Normal);
        var state4 = new State("State 4", StateType.Invalid);
        var sm = new StateMachine([initial, state1, state2, state3, state4]);

        initial.AddTransition(state1, null);
        state1.AddTransition(state1, new Guard("NOT START"));
        state1.AddTransition(state2, new Guard("START"));

        state2.AddTransition(state2, new Guard("NOT A"));
        state2.AddTransition(state3, new Guard("A"));

        if (isBounded && !lastStart) state3.AddTransition(state3, new Guard("NOT A"));
        else if (isBounded && lastStart) state3.AddTransition(state3, new Guard("NOT (A OR START)"));
        else if (!isBounded && lastStart) state3.AddTransition(state3, new Guard("NOT START"));
        else if (!isBounded && !lastStart) state3.AddTransition(state3, new Guard("true"));
        if (lastStart) state3.AddTransition(state2, new Guard("START"));
        if (isBounded) state3.AddTransition(state4, new Guard("A"));

        if (lastStart) state4.AddTransition(state4, new Guard("NOT START"));
        if (!lastStart) state4.AddTransition(state4, new Guard("true"));
        if (lastStart) state4.AddTransition(state2, new Guard("START"));

        return sm.ToRegEx().Flatten();
    }

    public RegexBase GenerateBeforeExistencePattern(Option option)
    {
        var isBounded = option.HasFlag(Option.Bounded);
        var missingEnd = option.HasFlag(Option.MissingEnd);

        var initial = new State("Initial", StateType.Initial);
        // If the end may be missing, then the initial state is not an accepting state since A has to occur.
        var state1 = new State("State 1", missingEnd ? StateType.Invalid : StateType.Normal);
        var state2 = new State("State 2", StateType.Normal);
        var state3 = new State("State 3", StateType.Final);
        // If the end may be missing and A has occured twice even though it is bounded, it is invalid.
        var state4 = new State("State 4", missingEnd ? StateType.Invalid : StateType.Normal);
        var sm = new StateMachine([initial, state1, state2, state3, state4]);

        initial.AddTransition(state1);
        state1.AddTransition(state1, new Guard("NOT (A OR END)"));
        state1.AddTransition(state2, new Guard("A"));

        if (isBounded) state2.AddTransition(state2, new Guard("NOT (A OR END)"));
        else if (!isBounded) state2.AddTransition(state2, new Guard("NOT END")); 
        state2.AddTransition(state3, new Guard("END"));
        if (isBounded) state2.AddTransition(state4, new Guard("A"));

        state4.AddTransition(state4, new Guard("END"));

        return sm.ToRegEx().Flatten();
    }

    public RegexBase GenerateBetweenExistencePattern(Option option)
    {
        var isBounded = option.HasFlag(Option.Bounded);
        var lastStart = option.HasFlag(Option.LastStart);
        var missingEnd = option.HasFlag(Option.MissingEnd);
        var scopeRepeatable = option.HasFlag(Option.ScopeRepeatability);

        var initial = new State("Initial", StateType.Initial);
        var state1 = new State("State1", StateType.Normal);
        // If end may be missing this is not a valid state, since A has to occur.
        var state2 = new State("State2", missingEnd ? StateType.Invalid : StateType.Normal);
        var state3 = new State("State3", StateType.Normal);
        var state4 = new State("State4", StateType.Normal);
        // If end may be missing and A has occured twice and is bounded, this is not a valid state.
        var state5 = new State("State5", missingEnd ? StateType.Invalid : StateType.Normal);
        var sm = new StateMachine([initial, state1, state2, state3, state4, state5]);

        initial.AddTransition(state1);
        state1.AddTransition(state1, new Guard("NOT START"));
        state1.AddTransition(state2, new Guard("START"));

        state2.AddTransition(state2, new Guard("NOT (A OR END)"));
        state2.AddTransition(state3, new Guard("A"));

        if (isBounded && lastStart) state3.AddTransition(state3, new Guard("NOT (A OR END)"));
        else if (isBounded && !lastStart) state3.AddTransition(state3, new Guard("NOT (A OR START OR END)"));
        else if (!isBounded && !lastStart) state3.AddTransition(state3, new Guard("NOT END"));
        else if (!isBounded && lastStart) state3.AddTransition(state3, new Guard("NOT (START OR END)"));
        if (lastStart) state3.AddTransition(state2, new Guard("START"));
        state3.AddTransition(state4, new Guard("END"));
        if (isBounded) state3.AddTransition(state5, new Guard("A"));

        if (scopeRepeatable) state4.AddTransition(state4, new Guard("NOT START"));
        else if (!scopeRepeatable) state4.Type = StateType.Final;
        if (scopeRepeatable) state4.AddTransition(state2, new Guard("START"));

        if (!lastStart) state5.AddTransition(state5, new Guard("NOT END"));
        else if (lastStart) state5.AddTransition(state5, new Guard("NOT (START OR END)"));
        if (lastStart) state5.AddTransition(state2, new Guard("START"));

        return sm.ToRegEx().Flatten();
    }
    #endregion
}
