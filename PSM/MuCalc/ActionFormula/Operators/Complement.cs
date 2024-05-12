namespace PSM.MuCalc.ActionFormula.Operators;

public class Complement(ActionFormulaBase formula) {
    private ActionFormulaBase Formula { get; set; } = formula;

    public override string ToString()
    {
        return $"!{this.Formula}";
    }
}
