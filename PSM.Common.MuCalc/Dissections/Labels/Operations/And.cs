namespace PSM.Common.MuCalc.Dissections.Labels.Operations;

public class And : IExpression
{
    public IList<IExpression> Expressions { get; }

    public And(params IExpression[] expressions) : this(expressions.ToList())
    {
    }
    
    public And(IEnumerable<IExpression> expressions)
    {
        this.Expressions = expressions.ToList();
        
        if (this.Expressions .Count() < 2)
            throw new ArgumentException("There must be at least 2 expressions.");
        if (this.Expressions.Count(e => e.GetCommandsInSubTree().Any()) > 1)
            Console.WriteLine("Could not infer meaning from conjunction over two positive occurence of CmdChk.");
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
        return $"({string.Join(@"\land", this.Expressions.Select(e => e.ToLatex()))})";
    }

    public string ToMCRL2()
    {
        return $"({string.Join("&&", this.Expressions.Select(e => e.ToMCRL2()))})";
    }
}