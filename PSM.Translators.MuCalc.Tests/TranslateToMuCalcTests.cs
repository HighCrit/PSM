using PSM.Common.UML;

namespace PSM.Translators.MuCalc.Tests;

[TestClass]
public class TranslateToMuCalcTests
{
    private TranslateToMuCalc? translator = null;

    [TestInitialize]
    public void Initialize()
    {
        this.translator = new TranslateToMuCalc();
    }

    [TestMethod]
    public void TranslateTest()
    {
        var initial = new State("Initial", StateType.Initial);
        var A_has_not_happened = new State("A_has_not_happened", StateType.Invalid);
        var final = new State("Final", StateType.Final);
        var sm = new StateMachine([
            initial,
            A_has_not_happened,
            final,
            ]);

        initial.AddTransition(A_has_not_happened);
        A_has_not_happened.AddTransition(A_has_not_happened, new Guard("NOT A"));
        A_has_not_happened.AddTransition(final, new Guard("A"));

        //var res = this.translator!.Translate(sm);
    }
}
