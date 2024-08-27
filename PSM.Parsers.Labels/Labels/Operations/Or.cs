namespace PSM.Parsers.Labels.Labels.Operations;

public class Or : IExpression
{
    public IList<IExpression> Expressions { get; }

    public Or(params IExpression[] expressions) : this(expressions.ToList())
    {
    }
    
    public Or(IEnumerable<IExpression> expressions)
    {
        this.Expressions = expressions.ToList();
    }

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return this.Expressions.SelectMany(e => e.GetCommandsInSubTree());
    }

    public IEnumerable<IExpression> GetVariablesInSubTree()
    {
        return this.Expressions.SelectMany(e => e.GetVariablesInSubTree());
    }

    public string ToMCRL2()
    {
        if (this.Expressions.Count == 1)
        {
            return this.Expressions[0].ToMCRL2();
        }
        
        return $"({string.Join("||", this.Expressions.Select(e => e.ToMCRL2()))})";
    }
    
    public override bool Equals(object? obj)
    {
        return (obj is Or or && or.Expressions.SequenceEqual(this.Expressions)) || 
               (this.Expressions.Count == 1 && this.Expressions.First().Equals(obj));
    }
}