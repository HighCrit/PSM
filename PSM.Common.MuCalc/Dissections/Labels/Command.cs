namespace PSM.Common.MuCalc.Dissections.Labels;

public class Command(string name) : IExpression
{
    public string Name { get; } = name;

    public bool ContainsCommands() => true;

    public bool ContainsVariables() => false;

    public bool ContainsAnd() => false;

    public bool ContainsOr() => false;

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return [this];
    }

    public IEnumerable<Variable> GetVariablesInSubTree()
    {
        return [];
    }

    public string ToLatex()
    {
        return $"\\mathit{{CmdChk({this.Name})}}";
    }

    public string ToMCRL2()
    {
        return $"CmdChk({this.Name})";
    }
}
