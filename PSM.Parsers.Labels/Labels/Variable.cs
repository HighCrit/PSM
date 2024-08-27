namespace PSM.Parsers.Labels.Labels;

public struct Variable(string name, string operand, string domain, object value) : IExpression
{
    public string Name { get; } = name;

    public string Operand { get; } = operand;

    public string Domain { get; } = domain;

    public object Value { get; } = value;

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
        return $"exists s_1 : {this.Domain} . <{this.Name}(s_1)> && s_1 {this.Operand} {this.Value}";
    }
}
