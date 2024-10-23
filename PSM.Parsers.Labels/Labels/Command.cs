namespace PSM.Parsers.Labels.Labels;

public class Command(int machinePartIndex, string name) : IExpression
{
    public string Name { get; } = name;

    public int MachinePartIndex { get; } = machinePartIndex;

    public IEnumerable<Command> GetCommandsInSubTree()
    {
        return [this];
    }

    public IEnumerable<IExpression> GetVariablesInSubTree()
    {
        return [];
    }
    
    public string ToMCRL2()
    {
        return $"cmd_chk({this.MachinePartIndex},{this.Name})";
    }

    public override bool Equals(object? obj)
    {
        return obj is not null && ((obj is Command cmd && cmd.Name == this.Name && cmd.MachinePartIndex == this.MachinePartIndex) || obj.Equals(this));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Name, this.MachinePartIndex);
    }
}
