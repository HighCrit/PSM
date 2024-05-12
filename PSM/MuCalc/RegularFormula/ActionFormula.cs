
using PSM.MuCalc.ActionFormula;

namespace PSM.MuCalc.RegularFormula.Operators;

public class ActionFormula(ActionFormulaBase formula) : RegularFormulaBase {
    private ActionFormulaBase Formula { get; set; } = formula;

    public override string ToString()
    {
        return this.Formula.ToString();
    }
}
