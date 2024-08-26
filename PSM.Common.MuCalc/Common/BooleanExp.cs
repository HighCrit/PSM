using PSM.Common.MuCalc.Dissections.Labels;
using PSM.Common.MuCalc.ModalFormula;

namespace PSM.Common.MuCalc.Common;

public class BooleanExp(string value) : IModalFormula
{
    public string Value { get; } = value;

    public IModalFormula Flatten() => this;

    public virtual string ToLatex(Dictionary<Event, IExpression> substitutions)
    {
        // TODO: Make value an expression instead of string.
        return this.Value;
    }

    public virtual string ToMCRL2(Dictionary<Event, IExpression> substitutions)
    {
        return this.Value;
    }
}