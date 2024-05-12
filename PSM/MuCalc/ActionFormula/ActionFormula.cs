using Action = PSM.MuCalc.Actions.Action;

namespace PSM.MuCalc.ActionFormula;

public class ActionFormula(Action action) : ActionFormulaBase {
    private Action Action { get; set; } = action;

    public override string ToString()
    {
        return this.Action.ToString();
    }
}
