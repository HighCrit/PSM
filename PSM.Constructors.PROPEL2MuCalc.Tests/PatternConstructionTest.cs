using PSM.Common;
using PSM.Common.MuCalc.Common;
using PSM.Common.PROPEL;
using PSM.Parsers.Labels.Labels;
using PSM.Parsers.Labels.Labels.Operations;

namespace PSM.Constructors.PROPEL2MuCalc.Tests;

[TestClass]
public class PatternConstructionTest
{
    /**
     * Requirement of the cylinder that captures consistency between
     * different outputs.
     * Invariantly, when either oInEndPosition or oInZeroPosition is true,
     * also oEnabled is true.
     */
    [TestMethod]
    public void TestCylinderPropertyGeneration()
    {
        const string expectedMcrl2 =
            "[true*](" +
                "((<state_M2'oInEndPosition(true)>true || <state_M2'oInZeroPosition(true)>true) && !<state_M2'oEnabled(true)>true) -> [true]false)";
        
        // Absence: (oInEndPosition = true OR oInZeroPosition = true) AND
        var pattern = PatternCatalogue.GetPattern(Behaviour.Absence, Scope.Global, Option.None);

        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.A] = new And(
                new Or(new Variable("state_M2'oInEndPosition", "=", Domain.BOOL.ToString(), "true"),
                    new Variable("state_M2'oInZeroPosition", "=", Domain.BOOL.ToString(), "true")),
                new Neg(new Variable("state_M2'oEnabled", "=", Domain.BOOL.ToString(), "true")))
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToMCRL2();
        
        Assert.AreEqual(expectedMcrl2, formula);
    }

    [TestMethod]
    public void Requirement1Generation()
    {
        // Absence: CmdChk(Stamp)
        // Between: CmdChk(EmergencyStop) and CmdChk(Enable)
        const string expectedMcrl2 = "[true*](" +
                                        "<CmdChk(EmergencyStop)>true -> [true]nu P_1 . ((!<CmdChk(Enable)>true -> [true]P_1) && (<CmdChk(Stamp)>true -> [true]false)))";
        
        var pattern = PatternCatalogue.GetPattern(Behaviour.Absence, Scope.Between, Option.FirstStart | Option.OptionalEnd | Option.ScopeRepeatability);
        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.A] = new Command("Stamp"),
            [Event.Start] = new Command("EmergencyStop"),
            [Event.End] = new Command("Enable"),
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToLatex();
        Assert.AreEqual(expectedMcrl2, formula);
    }
    
    [TestMethod]
    public void Requirement2Generation()
    {
        // Absence: CmdChk(Stamp) AND oSkipProduct
        const string expectedMcrl2 = "[true*]((<CmdChk(Stamp)>true && <state_M2'oSkipProduct(true)>true) -> [true]false)";
        
        var pattern = PatternCatalogue.GetPattern(Behaviour.Absence, Scope.Global, Option.None);
        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.A] = new And(new Command("Stamp"), new Variable("state_M2'oSkipProduct", "=", Domain.BOOL.ToString(), "true")),
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToLatex();
        Assert.AreEqual(expectedMcrl2, formula);
    }
    
    [TestMethod]
    public void Requirement3Generation()
    {
        // Response: if iCurrentTemperature < GoalTemp then oHeaterOn
        const string expectedMcrl2 = "[true*](" +
                                        "(<state_M2'oCarrierDetected(true)>true && !<state_M2'SkipProduct(true)>true) -> " +
                                            "[true]nu P_1 . ((!<state_M2'StopperDown(true)>true -> [true]P_1) && (" +
                                                "<state_M2'iCurrentTemperature(s_2)>true -> " +
                                                    "[true]nu P_1 . ((!(<state_M2'oHeaterOn(true)>true || <state_M2'StopperDown(true)>true) -> [true]P_1) && (<state_M2'StopperDown(true)>true -> [true]false)))))";
        
        var pattern = PatternCatalogue.GetPattern(Behaviour.Response, Scope.Between, Option.Nullity | Option.Precedency | Option.PreArity | Option.PostArity | Option.Repeatability | Option.FirstStart | Option.ScopeRepeatability);
        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.A] = new Variable("state_M2'iCurrentTemperature", ">=", Domain.INT.ToString(), "s_2"),
            [Event.B] = new Variable("state_M2'oHeaterOn", "=", Domain.BOOL.ToString(), "true"),
            [Event.Start] = new And(new Variable("state_M2'oCarrierDetected", "=", Domain.BOOL.ToString(), "true"),
                new Neg(new Variable("state_M2'SkipProduct", "=", Domain.BOOL.ToString(), "true"))),
            [Event.End] = new Variable("state_M2'StopperDown", "=", Domain.BOOL.ToString(), "true")
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToLatex();
        Assert.AreEqual(expectedMcrl2, formula);
    }
    
    [TestMethod]
    public void Requirement4Generation()
    {
        // Absence: SkipProduct && oHeaterOn
        const string expectedMcrl2 = "[true*](" +
                                        "(<state_M2'oCarrierDetected(true)>true && <state_M2'SkipProduct(true)>true) -> " +
                                            "[true]nu P_1 . ((!<state_M2'StopperDown(true)>true -> [true]P_1) && (<oHeaterOn(true)>true -> [true]false)))";
        
        var pattern = PatternCatalogue.GetPattern(Behaviour.Absence, Scope.Between, Option.FirstStart | Option.OptionalEnd | Option.ScopeRepeatability);
        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.A] = new Variable("oHeaterOn", "=", Domain.BOOL.ToString(), "true"),
            [Event.Start] = new And(new Variable("state_M2'oCarrierDetected", "=", Domain.BOOL.ToString(), "true"),
                new Variable("state_M2'SkipProduct", "=", Domain.BOOL.ToString(), "true")),
            [Event.End] = new Variable("state_M2'StopperDown", "=", Domain.BOOL.ToString(), "true")
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToLatex();
        Assert.AreEqual(expectedMcrl2, formula);
    }
    [TestMethod]
    public void Requirement5Generation()
    {
        // Absence: SkipProduct && oHeaterOn
        const string expectedMcrl2 = "[true*](" +
                                        "(<state_M2'oCarrierDetected(true)>true && !<state_M2'SkipProduct(true)>true) -> " +
                                            "[true]nu P_1 . ((!<state_M2'iCurrentTemperature(s_2)>true -> [true]P_1) && (<state_M2'StopperDown(true)>true -> [true]false)))";
        
        var pattern = PatternCatalogue.GetPattern(Behaviour.Absence, Scope.Between, Option.FirstStart | Option.OptionalEnd | Option.ScopeRepeatability);
        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.End] = new Variable("state_M2'iCurrentTemperature", ">=", Domain.INT.ToString(), "s_2"),
            [Event.Start] = new And(new Variable("state_M2'oCarrierDetected", "=", Domain.BOOL.ToString(), "true"),
                new Neg(new Variable("state_M2'SkipProduct", "=", Domain.BOOL.ToString(), "true"))),
            [Event.A] = new Variable("state_M2'StopperDown", "=", Domain.BOOL.ToString(), "true")
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToLatex();
        Assert.AreEqual(expectedMcrl2, formula);
    }
    
    [TestMethod]
    public void Requirement6Generation()
    {
        // Absence: StopperDown && InOperation
        const string expectedMcrl2 = "[true*]((<StopperDown(true)>true && <State.InOperation(true)>true) -> [true]false)";
        
        var pattern = PatternCatalogue.GetPattern(Behaviour.Absence, Scope.Global, Option.None);
        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.A] = new And(new Variable("StopperDown", "=", Domain.BOOL.ToString(), "true"),
                new Variable("State.InOperation", "=", Domain.BOOL.ToString(), "true")),
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToLatex();
        Assert.AreEqual(expectedMcrl2, formula);
    }
    
    [TestMethod]
    public void Requirement7Generation()
    {
        // Absence: StopperDown && InOperation
        const string expectedMcrl2 = "[true*](" +
                                        "(<State.InOperation(true)>true && !<oCarrierDetected(true)>true) -> " +
                                            "[true]mu X . (<true>true && (<State.InOperation(true)>true -> [true]X)))";
        
        var pattern = PatternCatalogue.GetPattern(Behaviour.Response, Scope.Global, Option.Nullity | Option.Precedency | Option.PreArity | Option.PostArity | Option.Repeatability);
        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.A] = new And(new Variable("State.InOperation", "=", Domain.BOOL.ToString(), "true"),
                new Neg(new Variable("oCarrierDetected", "=", Domain.BOOL.ToString(), "true"))),
            [Event.B] = new Neg(new Variable("State.InOperation", "=", Domain.BOOL.ToString(), "true")),
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToLatex();
        Assert.AreEqual(expectedMcrl2, formula);
    }
    
    [TestMethod]
    public void Requirement8Generation()
    {
        // Absence: StopperDown && InOperation
        const string expectedMcrl2 = "[true*](" +
                                        "<CmdChk(Enable)>true -> " +
                                            "[true]nu P_1 . ((!(<state_M2'oEmergencyValve(true)>true || <CmdChk(Stamp)>true) -> [true]P_1) && (<CmdChk(Stamp)>true -> [true]false)))";
        
        var pattern = PatternCatalogue.GetPattern(Behaviour.Existence, Scope.Between, Option.FirstStart | Option.ScopeRepeatability);
        var substitutions = new Dictionary<Event, IExpression>
        {
            [Event.A] = new Variable("state_M2'oEmergencyValve", "=", Domain.INT.ToString(), "true"),
            [Event.Start] = new Command("Enable"),
            [Event.End] = new Command("Stamp")
        };

        var formula = pattern.ApplySubstitutions(substitutions).Flatten().ToLatex();
        Assert.AreEqual(expectedMcrl2, formula);
    }
}