using PSM.Common.UML;
using PSM.Translators.MuCalc;
using PSM.Translators.MuCalc.Rgx;

namespace PSM.Translators.PROPEL.Tests;

[TestClass]
public class SMToREConverterTests
{
    [TestMethod]
    public void TestAbsence()
    {
        var initial = new State("Initial", StateType.Initial);
        var sink = new State("Sink", StateType.Invalid);
        var interMediate = new State("Intermediate", StateType.Normal);
        var sm = new StateMachine([
            initial, sink, interMediate,
            ]);

        initial.AddTransition(interMediate, null);
        interMediate.AddTransition(interMediate, new Guard("NOT A"));
        interMediate.AddTransition(sink, new Guard("A"));

        var rgx = sm.ToRegEx().Flatten();

        // (NOT A)*
        var expectedRgx = new MuCalc.Rgx.Kleene(new Token("NOT A"));
        Assert.AreEqual(expectedRgx, rgx);
    }

    [TestMethod]
    public void TestExistence()
    {
        var initial = new State("Initial", StateType.Initial);
        var final = new State("Final", StateType.Final);
        var interMediate = new State("Intermediate", StateType.Invalid);
        var sm = new StateMachine([initial, final, interMediate]);

        initial.AddTransition(interMediate, null);
        interMediate.AddTransition(interMediate, new Guard("NOT A"));
        interMediate.AddTransition(final, new Guard("A"));

        var rgx = sm.ToRegEx().Flatten();

        // (NOT A)* A
        var expectedRgx = new MuCalc.Rgx.Concatenation(new MuCalc.Rgx.Kleene(new Token("NOT A")), new Token("A"));
        Assert.AreEqual(expectedRgx, rgx);
    }

    [TestMethod]
    public void TestBoundedExistence()
    {
        var initial = new State("Initial", StateType.Initial);
        var final = new State("Final", StateType.Normal);
        var interMediate = new State("Intermediate", StateType.Invalid);
        var sm = new StateMachine([initial, final, interMediate]);

        initial.AddTransition(interMediate, null);
        interMediate.AddTransition(interMediate, new Guard("NOT A"));
        interMediate.AddTransition(final, new Guard("A"));
        final.AddTransition(final, new Guard("NOT A"));

        var rgx = sm.ToRegEx().Flatten();

        // (NOT A)* A (NOT A)*
        var expectedRgx = new MuCalc.Rgx.Concatenation(
            new MuCalc.Rgx.Concatenation(
                new MuCalc.Rgx.Kleene(
                    new Token("NOT A")),
                new Token("A")), 
            new MuCalc.Rgx.Kleene(new Token("NOT A")));
        Assert.AreEqual(expectedRgx, rgx);
    }

    [TestMethod]
    public void TestResponseBetweenQP()
    {
        var initial = new State("Initial", StateType.Initial);
        var state1 = new State("State1", StateType.Normal);
        var state2 = new State("State2", StateType.Normal); // Choice Normal/Invalid
        var state3 = new State("State3", StateType.Normal);
        var state4 = new State("State4", StateType.Normal);
        var final = new State("Final", StateType.Final); // Choice to include

        var sm = new StateMachine([
            initial, state1, state2, state3, state4, final,
            ]);

        initial.AddTransition(state1, null);

        state1.AddTransition(state1, new Guard("NOT Q"));
        state1.AddTransition(state2, new Guard("Q"));

        state2.AddTransition(state2, new Guard("NOT (A OR P)")); // Choice V
        // Choice: state2.AddTransition(state2, new Guard("NOT (A OR B OR P)"));
        state2.AddTransition(state1, new Guard("P")); // Choice to include.
        state2.AddTransition(state3, new Guard("A"));

        state3.AddTransition(state3, new Guard("NOT (B OR P)")); // Choice V
        // Choice: state3.AddTransition(state3, new Guard("A OR Q"));
        // Choice: state3.AddTransition(state3, new Guard("NOT (A OR B OR P)"));
        state3.AddTransition(state4, new Guard("B"));

        state4.AddTransition(state4, new Guard("NOT (B OR P)")); // Choice V
        // Choice: state4.AddTransition(state4, new Guard("NOT (A OR P)"));
        // Choice: state4.AddTransition(state4, new Guard("NOT (A OR B OR P)"));
        state4.AddTransition(state4, new Guard("NOT P"));
        state4.AddTransition(state1, new Guard("P")); // Choice to include.
        state4.AddTransition(state3, new Guard("A")); // Choice to include.
        state4.AddTransition(final, new Guard("P")); // Choice to include.

        var rgx = sm.ToRegEx().Flatten();

        // (NOT A)* A (NOT A)*
        var expectedRgx = new MuCalc.Rgx.Concatenation(
            new MuCalc.Rgx.Concatenation(
                new MuCalc.Rgx.Kleene(
                    new Token("NOT A")),
                new Token("A")),
            new MuCalc.Rgx.Kleene(new Token("NOT A")));
        Assert.AreEqual(expectedRgx, rgx);
    }
}