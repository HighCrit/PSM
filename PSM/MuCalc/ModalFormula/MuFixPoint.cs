namespace PSM.MuCalc.ModalFormula;

public class MuFixPoint : ModalFormulaBase {
    private string Id { get; set; }
    private ModalFormulaBase Formula { get; set;}
    private IEnumerable<Parameter>? Parameters { get; set;}

    public MuFixPoint(string id, ModalFormulaBase formula, IEnumerable<Parameter>? parameters = null) {
        this.Id = id;
        this.Formula = formula;
        this.Parameters = parameters;
    }

    public override string ToString()
    {
        return this.Parameters is null 
                    ? $"mu {this.Id} . {this.Formula})"
                    : $"mu {this.Id} ({string.Join(',', this.Parameters)}) . {this.Formula})";
    }
}
