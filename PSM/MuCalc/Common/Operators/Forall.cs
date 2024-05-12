namespace PSM.MuCalc.Common.Operators;

public class Forall<T>(string variableName, Domain domain, T formula)
{
    private string VariableName { get; set; } = variableName;
    private Domain Domain { get; set; } = domain;
    private T Formula { get; set; } = formula;

    public override string ToString() {
        return $"(forall {this.VariableName}:{this.Domain.Name} . {this.Formula})";
    }
}
