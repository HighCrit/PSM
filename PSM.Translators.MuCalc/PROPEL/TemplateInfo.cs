namespace PSM.Translators.MuCalc.PROPEL;

public record TemplateInfo(Behaviour Behaviour, Scope Scope, Option Option)
{
    public static Option GetAvailableOptionsFor(Behaviour behaviour, Scope scope)
    {
        return behaviour switch
        {
            Behaviour.Absence => Option.None,
            Behaviour.Existence => 
                Option.Bounded,
            Behaviour.Precedence => 
                Option.Nullity |
                Option.Pre_arity |
                Option.Immediacy |
                Option.Post_arity |
                Option.Finalisation |
                Option.Repeatability,
            Behaviour.Response => 
                Option.Nullity |
                Option.Precedency |
                Option.Pre_arity |
                Option.Immediacy |
                Option.Post_arity |
                Option.Finalisation |
                Option.Repeatability,
            _ => throw new ArgumentException($"Invalid behaviour: {behaviour}.")
        } | (scope is Scope.Between_Q_and_P ? Option.Scope_Repeatability : Option.None)
        | (scope is Scope.After_Q or Scope.Between_Q_and_P ? Option.Last_Start : Option.None)
        | (scope is Scope.Before_P or Scope.Between_Q_and_P ? Option.Missing_End : Option.None);
    }
}
