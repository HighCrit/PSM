namespace PSM.Common.MuCalc.Dissections.Labels.Operations;

public class And : IExpression
{
    public IExpression Left { get; }

    public IExpression Right { get; }

    public And(IExpression left, IExpression right)
    {
        if (left.ContainsCommands() && right.ContainsCommands())
            throw new ArgumentException("Cannot extract meaning from AND with commands on both sides.");

        this.Left = left;
        this.Right = right;
    }

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

    public bool ContainsAnd() => true;

    public bool ContainsOr() => this.Left.ContainsOr() || this.Right.ContainsOr();

    public string ToLatex()
    {
        return $"({this.Left.ToLatex()} \\land {this.Right.ToLatex()})";
    }

    public string ToMCRL2()
    {
        return $"({this.Left.ToMCRL2()} && {this.Right.ToMCRL2()})";
    }
}