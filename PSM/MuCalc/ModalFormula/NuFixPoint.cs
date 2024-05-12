namespace PSM.MuCalc.ModalFormula;

public class NuFixPoint : ModalFormulaBase {
    private string Id { get; set; }
    private ModalFormulaBase Formula { get; set;}
    private IEnumerable<Parameter>? Parameters { get; set;}

    public NuFixPoint(string id, ModalFormulaBase formula, IEnumerable<Parameter>? parameters = null) {
        this.Id = id;
        this.Formula = formula;
        this.Parameters = parameters;
    }

    public override string ToString()
    {
        return this.Parameters is null 
                    ? $"nu {this.Id} . {this.Formula})"
                    : $"nu {this.Id} ({string.Join(',', this.Parameters)}) . {this.Formula})";
    }
}
