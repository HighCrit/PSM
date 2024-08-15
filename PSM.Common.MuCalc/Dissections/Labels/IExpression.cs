namespace PSM.Common.MuCalc.Dissections.Labels;

public interface IExpression
{
    public IEnumerable<Command> GetCommandsInSubTree();

    public IEnumerable<Variable> GetVariablesInSubTree();

    public bool ContainsCommands();

    public bool ContainsVariables();

    public bool ContainsAnd();

    public bool ContainsOr();

    public string ToLatex();

    public string ToMCRL2();
}
