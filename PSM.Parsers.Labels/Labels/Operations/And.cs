using Microsoft.VisualBasic;

namespace PSM.Parsers.Labels.Labels.Operations;

public class And : IExpression
{
    public IList<IExpression> Expressions { get; }

    public And(params IExpression[] expressions) : this(expressions.ToList())
    {
    }
    
    public And(IEnumerable<IExpression> expressions)
    {
        this.Expressions = expressions.ToList();
        
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

    public string ToMCRL2()
    {
        if (this.Expressions.Count == 1)
        {
            return this.Expressions[0].ToMCRL2();
        }
        
        return $"({string.Join("&&", this.Expressions.Select(e => e.ToMCRL2()))})";
    }

    public override bool Equals(object? obj)
    {
        return (obj is And and && and.Expressions.SequenceEqual(this.Expressions)) || 
               (this.Expressions.Count == 1 && this.Expressions.First().Equals(obj));
    }
}