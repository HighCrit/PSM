namespace PSM.Common.MuCalc.Dissections.Labels;

public interface IExpression
{
    public IEnumerable<Command> GetCommandsInSubTree();

    public IEnumerable<IExpression> GetVariablesInSubTree();

    public string ToLatex();

    public string ToMCRL2();
}
