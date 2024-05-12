namespace PSM.MuCalc.Actions.Operators;

public class Multi(Action left, Action right) {
    private Action Left { get; set; } = left;
    private Action Right { get; set; } = right;

    public override string ToString()
    {
        return $"{this.Left}|{this.Right}";
    }
}
