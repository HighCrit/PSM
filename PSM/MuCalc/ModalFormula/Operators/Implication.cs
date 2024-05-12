namespace PSM.MuCalc.ModalFormula.Operators;

public class Implication {

    private ModalFormulaBase Left { get; set; }
    private ModalFormulaBase Right { get; set; }

    public Implication(ModalFormulaBase left, ModalFormulaBase right){
        this.Left = left;
        this.Right = right;
    }

    public override string ToString() {
        return $"({this.Left} -> {this.Right})";
    }
}
