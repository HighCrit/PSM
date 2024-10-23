using PSM.Common.MuCalc.ModalFormula;
using PSM.Parsers.Labels.Labels;

namespace PSM.Common.MuCalc.Common;

public class BooleanExp(string value) : IModalFormula
{
    public string Value { get; } = value;

    public IModalFormula Flatten() => this;

    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
        => this;

    public virtual string ToLatex()
    {
        // TODO: Make value an expression instead of string.
        return this.Value;
    }

    public virtual string ToMCRL2()
    {
        return $"val({this.Value})";
    }
}