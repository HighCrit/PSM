using PSM.MuCalc.RegularFormula;

namespace PSM.MuCalc.ModalFormula;

public class Box : ModalFormulaBase {
    private ModalFormulaBase Formula {get; set;}
    private RegularFormulaBase RegularFormula {get; set;}

    public Box(RegularFormulaBase regularFormula, ModalFormulaBase formula) {
        this.RegularFormula = regularFormula;
        this.Formula = formula;
    }

    public override string ToString()
    {
        return $"[{this.RegularFormula}]{this.Formula}";
    }
}
