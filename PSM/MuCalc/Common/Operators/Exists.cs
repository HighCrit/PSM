namespace PSM.MuCalc.Common.Operators;

public class Exists<T> {

    private string VariableName {get; set; }
    private Domain Domain { get; set;}
    private T Formula { get; set; }

    public Exists(string variableName, Domain domain, T formula){
        this.VariableName = variableName;
        this.Domain = domain;
        this.Formula = formula;
    }

    public override string ToString() {
        return $"(exists {this.VariableName}:{this.Domain.Name} . {this.Formula})";
    }
}
