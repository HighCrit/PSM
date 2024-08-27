using PSM.Common.MuCalc.Common;

namespace PSM.Common.MuCalc.Dissections.Labels;

public struct Variable(string name, string operand, Domain domain, object value) : IExpression
{
    public string Name { get; } = name;

    public string Operand { get; } = operand;

    public Domain Domain { get; } = domain;

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

    public string ToLatex()
    {
        return $@"\exists_{{s_1: {this.Domain}}} . \langle{this.Name}(s_1)\rangle \land s_1 {this.Operand} {this.Value}";
    }

    public string ToMCRL2()
    {
        return $"exists s_1 : {this.Domain} . <{this.Name}(s_1)> && s_1 {this.Operand} {this.Value}";
    }
}
