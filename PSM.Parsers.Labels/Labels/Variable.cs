namespace PSM.Parsers.Labels.Labels;

public struct Variable : IExpression
{
    public Variable(string name, string operand, string domain, object value)
    {
        this.Name = name;
        this.Operand = operand;
        this.Domain = domain;
        this.Value = value;
    }
    
    public string Name { get; }

    public string Operand { get; }

    public string Domain { get; }

    public object Value { get; }

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
        return $"exists s_1,s_2 : {this.Domain} . <{this.Name}(s_1)> && (s_1 {this.Operand} s_2) && (s_2 == {this.Value})";
    }
}
