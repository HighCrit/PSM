using PSM.Common.MuCalc.Dissections.Labels;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.RegularFormula;

namespace PSM.Common.MuCalc.Common;

public class Bool : IModalFormula, IRegularFormula
{
    public static Bool True = new("true");
    public static Bool False = new("false");

    private readonly string value;

    private Bool(string value)
    {
        this.value = value;
    }

    /// <inheritdoc />
    public string ToLatex(Dictionary<Event, IExpression> substitutions) => this.ToLatex();

    /// <inheritdoc />
    public string ToMCRL2(Dictionary<Event, IExpression> substitutions) => this.ToMCRL2();

    /// <inheritdoc />
    public string ToLatex()
    {
        return $"\\mathit{{{this.value}}}";
    }

    /// <inheritdoc />
    public string ToMCRL2()
    {
        return this.value;
    }
}