using PSM.MuCalc.Common;

namespace PSM.MuCalc.ModalFormula;

public class Parameter {
    public string Name { get; set;}
    private Domain Domain { get; set;}
    private object Value { get; set;}

    public Parameter(string name, Domain domain, object value){
        this.Name = name;
        this.Domain = domain;
        this.Value = value;
    }

    public override string ToString()
    {
        return $"{this.Name}:{this.Domain} = {this.Value}";
    }
}
