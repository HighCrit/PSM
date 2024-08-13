namespace PSM.Common.PROPEL;

public record PropertyInfo(Behaviour Behaviour, Scope Scope, Option Option)
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
                Option.PreArity |
                Option.Immediacy |
                Option.PostArity |
                Option.Finalisation |
                Option.Repeatability,
            Behaviour.Response => 
                Option.Nullity |
                Option.Precedency |
                Option.PreArity |
                Option.Immediacy |
                Option.PostArity |
                Option.Finalisation |
                Option.Repeatability,
            _ => throw new ArgumentException($"Invalid behaviour: {behaviour}.")
        } | (scope is Scope.Between_Q_and_P ? Option.ScopeRepeatability : Option.None)
        | (scope is Scope.After_Q or Scope.Between_Q_and_P ? Option.FirstStart : Option.None)
        | (scope is Scope.Before_P or Scope.Between_Q_and_P ? Option.OptionalEnd : Option.None);
    }
}
