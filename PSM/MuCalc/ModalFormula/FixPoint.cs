namespace PSM.MuCalc.ModalFormula;

public class FixPoint(string id, IEnumerable<object>? values = null) : ModalFormulaBase {
    private string Id { get; } = id;
    private readonly IEnumerable<object>? Values = values;

    public override string ToString()
    {
        return this.Values is null
                    ? $"{this.Id}"
                    : $"{this.Id}({string.Join(',', this.Values)})";
    }
}
