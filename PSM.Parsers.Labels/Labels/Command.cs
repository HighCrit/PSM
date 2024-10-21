namespace PSM.Parsers.Labels.Labels;

public class Command(string name) : IExpression
{
    public string Name { get; } = name;

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return [this];
    }

    public IEnumerable<IExpression> GetVariablesInSubTree()
    {
        return [];
    }
    
    public string ToMCRL2()
    {
        return $"cmd_chk({this.Name})";
    }

    public override bool Equals(object? obj)
    {
        return obj is not null && ((obj is Command cmd && cmd.Name == this.Name) || obj.Equals(this));
    }
}
