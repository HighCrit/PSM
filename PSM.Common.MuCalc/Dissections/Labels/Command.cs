namespace PSM.Common.MuCalc.Dissections.Labels;

public class Command(string name) : IExpression
{
    public string Name { get; } = name;

    public bool Negated { get; init; } = false;

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return [this];
    }

    public IEnumerable<IExpression> GetVariablesInSubTree()
    {
        return [];
    }

    public string ToLatex()
    {
        var res = $@"\mathit{{CmdChk({this.Name})}}";
        return this.Negated ? $@"\overline{{{res}}}" : res;
    }

    public string ToMCRL2()
    {
        return $"{(this.Negated ? "!" : string.Empty)}CmdChk({this.Name})";
    }
}
