namespace PSM.MuCalc.ModalFormula.Operators;

public class Conjunction {

    private ModalFormulaBase Left { get; set; }
    private ModalFormulaBase Right { get; set; }

    public Conjunction(ModalFormulaBase left, ModalFormulaBase right){
        this.Left = left;
        this.Right = right;
    }

    public override string ToString() {
        return $"({this.Left} && {this.Right})";
    }
}
