namespace PSM.MuCalc.RegularFormula.Operators;

public class Summation(RegularFormulaBase left, RegularFormulaBase right) : RegularFormulaBase {
    private RegularFormulaBase Left { get; set; } = left;
    private RegularFormulaBase Right { get; set; } = right;

    public override string ToString()
    {
        return $"({this.Left} + {this.Right})";
    }
}
