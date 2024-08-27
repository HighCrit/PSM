using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.RegularFormula;
using PSM.Parsers.Labels.Labels;

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

    public IModalFormula Flatten() => this;

    IRegularFormula IRegularFormula.Flatten() => this;

    /// <inheritdoc cref="IModalFormula.ToMCRL2" />
    public string ToLatex()
    {
        return $"\\mathit{{{this.value}}}";
    }

    /// <inheritdoc cref="IModalFormula.ToMCRL2" />
    public string ToMCRL2()
    {
        return this.value;
    }

    public IModalFormula ApplySubstitutions(Dictionary<Event, IExpression> substitutions)
        => this;
}