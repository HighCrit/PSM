namespace PSM.Parsers.Labels.Labels.Operations;

public class Neg(IExpression exp) : IExpression
{
    public IExpression Expression { get; } = exp;

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return [];
    }

    public IEnumerable<IExpression> GetVariablesInSubTree()
    {
        if (this.Expression is Variable)
        {
            return [this];
        }

        return [];
    }

    public string ToMCRL2()
    {
        return $"!{this.Expression.ToMCRL2()}";
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Neg neg && neg.Expression.Equals(this.Expression);
    }
}