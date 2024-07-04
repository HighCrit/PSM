namespace PSM.Translators.MuCalc.PROPEL;

public record Scope
{
    public static readonly Scope Global = new(SType.Global, false);
    public static readonly Scope After_Q = new(SType.After_Q, false);
    public static readonly Scope Before_P = new(SType.Before_P, false);
    public static readonly Scope After_Q_Until_P = new(SType.After_Q_Until_P, true);
    public static readonly Scope Between_Q_and_P = new(SType.Between_Q_and_P, true);

    private Scope(SType type, bool repeatable)
    {
        this.Type = type;
        this.Repeatable = repeatable;
    }

    public SType Type { get; }

    public bool Repeatable { get; }

    public enum SType
    {
        Global,
        After_Q,
        Before_P,
        After_Q_Until_P,
        Between_Q_and_P,
    }
}
