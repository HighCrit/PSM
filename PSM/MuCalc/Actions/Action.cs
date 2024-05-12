namespace PSM.MuCalc.Actions;

public class Action(string name, IEnumerable<object>? values = null)
{
    public static readonly Action TAU = new("tau");
    public static readonly Action TRUE = new("true");
    public static readonly Action FALSE = new("false");

    private string Name { get; set; } = name;
    private IEnumerable<object>? Values { get; set; } = values;

    public override string ToString()
    {
        return this.Values is null 
                ? $"{this.Name}"
                : $"{this.Name}({string.Join(',', this.Values)})";
    }
}
