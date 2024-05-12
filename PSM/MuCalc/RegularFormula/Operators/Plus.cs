namespace PSM.MuCalc.RegularFormula.Operators;

public class Plus(RegularFormulaBase formula) : RegularFormulaBase {
    private RegularFormulaBase Formula { get; set; } = formula;

    public override string ToString()
    {
        return $"{this.Formula}+";
    }
}
