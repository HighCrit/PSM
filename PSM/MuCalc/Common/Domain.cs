namespace PSM.MuCalc.Common;

public readonly struct Domain(string Name)
{
    public static readonly Domain INT = new("Int");
    public static readonly Domain BOOL = new("Bool");
    public readonly string Name { get; } = Name;
}