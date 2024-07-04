using PSM.Common.UML;
using PSM.Translators.MuCalc.PROPEL;
using PSM.Translators.MuCalc.Rgx;

namespace PSM.Translators.MuCalc;

public class TemplatePatternGenerator
{
    private static readonly Guard GenericGuard = new("T");

    public IEnumerable<KeyValuePair<Option, RegexBase>> GenerateTemplatePattern(Scope scope, Behaviour behaviour)
    {
        return behaviour.Type switch
        {
            Behaviour.BType.Absence => GenerateAbsenceTemplatePattern(scope),
            Behaviour.BType.Existence => GenerateExistenceTemplatePattern(scope),
            _ => throw new ArgumentException(nameof(scope))
        };
    }

    public IEnumerable<KeyValuePair<Option, RegexBase>> GenerateAbsenceTemplatePattern(Scope scope)
    {
        return scope.Type switch
        {
            Scope.SType.Global => GenerateGlobalAbsencePatterns(),
            _ => throw new ArgumentException(nameof(scope))
        };
    }

    #region Absence
    public IEnumerable<KeyValuePair<Option, RegexBase>> GenerateGlobalAbsencePatterns()
    {
        var initial = new State("Initial", StateType.Initial);
        var sink = new State("Sink", StateType.Invalid);
        var interMediate = new State("Intermediate", StateType.Normal);
        var sm = new StateMachine(new Dictionary<string, State>
        {
            { initial.Name, initial },
            { sink.Name, sink },
            { interMediate.Name, interMediate },
        });

        initial.AddTransition(interMediate, null);
        interMediate.AddTransition(interMediate, new Guard("NOT A"));
        interMediate.AddTransition(sink, new Guard("A"));

        return [new(Option.None, sm.ToRegEx().Flatten())];
    }
    #endregion

    public IEnumerable<KeyValuePair<Option, RegexBase>> GenerateExistenceTemplatePattern(Scope scope)
    {
        return scope.Type switch
        {
            Scope.SType.Global => GenerateGlobalExistencePatterns(),
            _ => throw new ArgumentException(nameof(scope))
        };
    }

    #region Existence
    public IEnumerable<KeyValuePair<Option, RegexBase>> GenerateGlobalExistencePatterns()
    {
        var options = Behaviour.Existence.AvailableOptions.GetFlags().GetAllCombinations();

        foreach (var option in options)
        {
            var isBounded = option.HasFlag(Option.Bounded);

            var initial = new State("Initial", StateType.Initial);
            var final = new State("Final", isBounded ? StateType.Normal : StateType.Final);
            var interMediate = new State("Intermediate", StateType.Invalid);
            var sm = new StateMachine(new Dictionary<string, State>
                {
                    { initial.Name, initial },
                    { final.Name, final },
                    { interMediate.Name, interMediate },
                });

            initial.AddTransition(interMediate, null);
            interMediate.AddTransition(interMediate, GenericGuard);
            interMediate.AddTransition(final, GenericGuard);
            if (isBounded) final.AddTransition(final, GenericGuard);

            yield return new(option, sm.ToRegEx().Flatten());
        }
    }
    #endregion
}
