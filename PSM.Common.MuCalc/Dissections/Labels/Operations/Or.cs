namespace PSM.Common.MuCalc.Dissections.Labels.Operations;

public class Or(IExpression left, IExpression right) : IExpression
{
    public IExpression Left { get; } = left;

    public IExpression Right { get; } = right;

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return this.Left.GetCommandsInSubTree().Concat(this.Right.GetCommandsInSubTree());
    }

    public IEnumerable<Variable> GetVariablesInSubTree()
    {
        return this.Left.GetVariablesInSubTree().Concat(this.Right.GetVariablesInSubTree());
    }

    public bool ContainsCommands()
    {
        return this.Left.ContainsCommands() || this.Right.ContainsCommands();
    }

    public bool ContainsVariables()
    {
        return this.Left.ContainsVariables() || this.Right.ContainsVariables();
    }

    public bool ContainsAnd() => this.Left.ContainsAnd() || this.Right.ContainsAnd();

    public bool ContainsOr() => true;

    public string ToLatex()
    {
        return $"({this.Left.ToLatex()} \\lor {this.Right.ToLatex()})";
    }

    public string ToMCRL2()
    {
        return $"({this.Left.ToMCRL2()} || {this.Right.ToMCRL2()})";
    }
}