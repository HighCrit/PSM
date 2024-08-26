namespace PSM.Common.MuCalc.Dissections.Labels.Operations;

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

    public string ToLatex()
    {
        return $@"\neg{this.Expression.ToLatex()}";
    }

    public string ToMCRL2()
    {
        return $"!{this.Expression.ToMCRL2()}";
    }
}