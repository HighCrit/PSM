using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.MuCalc.RegularFormula;

namespace PSM.Common.MuCalc.Common;

public class BooleanExp(string value) : IModalFormula, IRegularFormula
{
    public static BooleanExp True = new("true");
    public static BooleanExp False = new("false");

    private readonly string value = value;

    public override string ToString()
    {
        return this.value;
    }
}