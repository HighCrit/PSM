namespace PSM.Common.MuCalc.Dissections.Labels.Operations;

public class Or : IExpression
{
    public IList<IExpression> Expressions { get; }

    public Or(params IExpression[] expressions) : this(expressions.ToList())
    {
    }
    
    public Or(IEnumerable<IExpression> expressions)
    {
        this.Expressions = expressions.ToList();
        
        if (this.Expressions.Count() < 2)
            throw new ArgumentException("There must be at least 2 expressions.");
    }

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return this.Expressions.SelectMany(e => e.GetCommandsInSubTree());
    }

    public IEnumerable<IExpression> GetVariablesInSubTree()
    {
        return this.Expressions.SelectMany(e => e.GetVariablesInSubTree());
    }

    public string ToLatex()
    {
        return $"({string.Join(@"\lor", this.Expressions.Select(e => e.ToLatex()))})";
    }

    public string ToMCRL2()
    {
        return $"({string.Join("||", this.Expressions.Select(e => e.ToMCRL2()))})";
    }
}