namespace PSM.Parsers.Labels.Labels;

public interface IExpression
{
    public IEnumerable<Command> GetCommandsInSubTree();

    public IEnumerable<IExpression> GetVariablesInSubTree();
    
    public string ToMCRL2();
    
    public string ToString() => this.ToMCRL2()!;
}
