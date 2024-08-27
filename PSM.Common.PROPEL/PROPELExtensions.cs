namespace PSM.Common.PROPEL;

public static class PROPELExtensions
{
    public static Option[] GetOptions(this (Scope scope, Behaviour behaviour) sb)
    {
        var res = Option.None;
        if (sb.scope is Scope.After or Scope.Between) res |= Option.FirstStart;
        if (sb.scope is Scope.Before or Scope.Between) res |= Option.OptionalEnd;
        if (sb.scope is Scope.Between) res |= Option.ScopeRepeatability;

        res |= sb.behaviour switch
        {
            Behaviour.Existence => Option.Bounded,
            Behaviour.Precedence => Option.Nullity | Option.PreArity | Option.Immediacy | Option.PostArity | Option.Finalisation | Option.Repeatability,
            Behaviour.Response => Option.Nullity | Option.Precedency | Option.PreArity | Option.Immediacy | Option.PostArity | Option.Finalisation | Option.Repeatability,
            _ => Option.None
        };

        return res.GetFlags().ToArray();
    }

    public static Event[] GetEvents(this (Scope scope, Behaviour behaviour) sb)
    {
        var res = Event.A;
        
        if (sb.behaviour is Behaviour.Precedence or Behaviour.Response) res |= Event.B;
        
        if (sb.scope is Scope.After or Scope.Between) res |= Event.Start;
        if (sb.scope is Scope.Before or Scope.Between) res |= Event.End;

        return res.GetFlags().ToArray();
    }
}