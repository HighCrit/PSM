namespace PSM.MuCalc.ModalFormula.Operators;

public class Negation {

    private ModalFormulaBase Formula { get; set; }

    public Negation(ModalFormulaBase formula){
        this.Formula = formula;
    }

    public override string ToString() {
        return $"!{this.Formula}";
    }
}
