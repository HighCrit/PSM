namespace PSM.Common.MuCalc.Dissections.Labels;

public readonly struct Command(string name) : IExpression
{
    public string Name { get; } = name;

    public bool Negated { get; init; } = false;

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return [this];
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
