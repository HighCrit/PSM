namespace PSM.Translators.MuCalc.PROPEL;

public record Behaviour
{
    public static readonly Behaviour Absence = new(BType.Absence, Option.None);
    public static readonly Behaviour Existence = new(BType.Existence, Option.Bounded);
    public static readonly Behaviour Precedence =
        new(BType.Precedence,
            Option.Nullity |
            Option.Pre_arity |
            Option.Immediacy |
            Option.Post_arity |
            Option.Finalisation |
            Option.Repeatability);
    public static readonly Behaviour Response = 
        new(BType.Response,
            Option.Nullity |
            Option.Precedency |
            Option.Pre_arity |
            Option.Immediacy |
            Option.Post_arity |
            Option.Finalisation |
            Option.Repeatability);

    private Behaviour(BType type, Option availableOptions)
    {
        this.Type = type;
        this.AvailableOptions = availableOptions;
    }

    public BType Type { get; }

    public Option AvailableOptions { get; }

    public enum BType
    {
        Absence,
        Existence,
        Precedence,
        Response,
    }
}
