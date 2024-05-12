namespace PSM.MuCalc.ActionFormula.Operators;

public class Union(ActionFormulaBase left, ActionFormulaBase right) {
    private ActionFormulaBase Left { get; set; } = left;
    private ActionFormulaBase Right { get; set;} = right;

    public override string ToString()
    {
        return $"({this.Left} || {this.Right})";
    }
}
