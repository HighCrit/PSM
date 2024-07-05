using PSM.Common.UML;
using PSM.Translators.MuCalc.PROPEL;
using PSM.Translators.MuCalc.Rgx;

namespace PSM.Translators.MuCalc;

public class TemplatePatternGenerator
{
    private static readonly Guard GenericGuard = new("T");

    private List<(TemplateInfo Info, RegexBase? Rgx)> templatePatterns = [];
    private Dictionary<string, TemplateInfo> templateSignatures = [];

    public IEnumerable<(TemplateInfo Info, RegexBase? Rgx)> TemplatePatterns
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

    public IEnumerable<KeyValuePair<string, TemplateInfo>> TemplateSignatures
    {
        get
        {
            if (this.templateSignatures.Count == 0)
            {
                this.templateSignatures = this.TemplatePatterns
                    .Where(t => t.Rgx is not null)
                    .Select(t => KeyValuePair.Create(t.Rgx!.ToString(true), t.Info))
                    .ToDictionary();
            }
            return this.templateSignatures;
        }
    }

    public IEnumerable<(TemplateInfo Info, RegexBase? Rgx)> GenerateAllPatterns()
    {
        // TODO: Remove when all are supported
        List<Scope> scopes = [Scope.Global, Scope.After_Q /*, Scope.Before_P, Scope.After_Q_Until_P, Scope.Between_Q_and_P*/];
        List<Behaviour> behaviours = [Behaviour.Absence, Behaviour.Existence/*, Behaviour.Precedence, Behaviour.Response*/];

        var result = new List<(TemplateInfo Info, RegexBase? Rgx)>();
        foreach (var behaviour in behaviours) foreach (var scope in scopes)
            {
                var options = TemplateInfo
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
        var lastStart = option.HasFlag(Option.Last_Start);

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

    // TODO: Redo choices
    public RegexBase GenerateBetweenExistencePattern(Option option)
    {
        var isBounded = option.HasFlag(Option.Bounded);
        var scopeRepeatable = option.HasFlag(Option.Scope_Repeatability);

        var initial = new State("Initial", StateType.Initial);
        var state1 = new State("State1", StateType.Normal);
        var state2 = new State("State2", StateType.Invalid); // Choice
        var state3 = new State("State3", StateType.Normal);
        var state4 = new State("State4", StateType.Normal);
        var state5 = new State("State5", StateType.Invalid); // Choice
        var sm = new StateMachine([initial, state1, state2, state3, state4, state5]);

        initial.AddTransition(state1);
        state1.AddTransition(state1, new Guard("NOT START"));
        state1.AddTransition(state2, new Guard("START"));

        state2.AddTransition(state2, new Guard("NOT (A OR END)"));
        state2.AddTransition(state3, new Guard("A"));
        
        if (!isBounded && !scopeRepeatable) state3.AddTransition(state3, new Guard("NOT (A OR END)"));
        else if (!isBounded && scopeRepeatable) state3.AddTransition(state3, new Guard("NOT (A OR START OR END)"));
        else if (isBounded && !scopeRepeatable) state3.AddTransition(state3, new Guard("NOT END"));
        else if (isBounded && scopeRepeatable) state3.AddTransition(state3, new Guard("NOT (START OR END)"));
        state3.AddTransition(state4, new Guard("END"));
        if (!isBounded) state3.AddTransition(state5, new Guard("A"));

        if (scopeRepeatable) state4.AddTransition(state5, new Guard("NOT START"));
        else if (!scopeRepeatable) state4.Type = StateType.Final;
        if (scopeRepeatable) state4.AddTransition(state2, new Guard("START"));

        state5.AddTransition(state5, new Guard("NOT END"));
        state5.AddTransition(state5, new Guard("NOT (START OR END)"));
        if (scopeRepeatable) state5.AddTransition(state2, new Guard("START"));

        return sm.ToRegEx().Flatten();
    }
    #endregion
}
