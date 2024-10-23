namespace PSM.Parsers.Labels.Labels;

public struct Variable : IExpression
{
    public Variable(ModelInfo lhs, string operand, string domain, object rhs)
    {
        this.LHS = lhs;
        this.Operand = operand;
        this.Domain = domain;
        this.RHS = rhs;
    }
    
    public ModelInfo LHS { get; }

    public string Operand { get; }

    public string Domain { get; }

    public object RHS { get; }

    public bool Negated { get; set; } = false;

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return [];
    }

    public IEnumerable<IExpression> GetVariablesInSubTree()
    {
        return [this];
    }

    public string ToMCRL2()
    {
        var lhsName = $"state_M{this.LHS.MachineIndex}'{this.LHS.Name}";

        if (this.RHS is ModelInfo rhsInfo)
        {
            var rhsName = $"state_M{rhsInfo.MachineIndex}'{rhsInfo.Name}";

            return $"exists s_1,s_2 : {this.Domain} . <{lhsName}(s_1)> && <{rhsName}(s_2)> && (s_1 {this.Operand} s_2)";
        }

        return $"exists s_1,s_2 : {this.Domain} . <{lhsName}(s_1)> && (s_1 {this.Operand} {this.RHS})";
    }
}
