using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Dissections;
using PSM.Common.MuCalc.Dissections.Labels;
using PSM.Common.MuCalc.Dissections.Labels.Operations;
using PSM.Common.MuCalc.ModalFormula;
using Action = PSM.Common.MuCalc.Actions.Action;

[assembly: TestDataSourceDiscovery(TestDataSourceDiscoveryOption.DuringExecution)]
namespace PSM.Common.MuCalc.Tests
{
    [TestClass]
    public class PhiTests
    {
        #region PhiParseTestData
        public static IEnumerable<object[]> PhiParseTestData { get; } =
        [
            /*
             * [A]false
             */
            [
                PhiType.Pos, 
                Bool.False,
                new Variable("var", "=", Domain.INT, 1), // var = 1
                "((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]false)"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new Command("com"), // CmdChk(com)
                "[command(com,true)]false"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new Or(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) || var = 1
                "(((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]false) && [command(com,true)]false)"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new And(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) && var = 1
                "((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(com,true)]false)"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new And(
                    new Or(new Command("com1"), new Command("com2")),
                    new Variable("var", "=", Domain.INT, 1)), // (CmdChk(com1) || CmdChk(com2)) && var = 1
                "(((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(com1,true)]false) && ((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(com2,true)]false))"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new And(
                    new Or(new Command("com"), new Variable("var2", "=", Domain.INT, 2)),
                    new Variable("var1", "=", Domain.INT, 1)), // (CmdChk(com) || var2 = 2) && var1 = 1
                "(((exists s_1:Int . (<var1(s_1)>true && s_1 = 1)) -> [command(com,true)]false) && " +
                "(((exists s_1:Int . (<var1(s_1)>true && s_1 = 1)) && (exists s_1:Int . (<var2(s_1)>true && s_1 = 2))) -> [true]false))"
            ],
            /*
             * [(!A)* . B]false
             */
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Variable("var", "=", Domain.INT, 1), // var = 1
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]P_1) && [B]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Command("com"), // CmdChk(com)
                "nu P_1 . ([!command(com,true)]P_1 && [B]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Or(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) || var = 1
                "(nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]P_1) && [B]false)) && nu P_1 . ([!command(com,true)]P_1 && [B]false)))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new And(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) && var = 1
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!command(com,true)]P_1) && [B]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new And(
                    new Or(new Command("com1"), new Command("com2")),
                    new Variable("var", "=", Domain.INT, 1)), // (CmdChk(com1) || CmdChk(com2)) && var = 1
                "(nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(com1,true) || command(com2,true))]P_1) && [B]false)) && " +
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(com2,true) || command(com1,true))]P_1) && [B]false)))"
            ],
            /*
             * [(!A)* . B]false
             */
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Variable("var", "=", Domain.INT, 1), // var = 1
                "(!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Command("com"), // CmdChk(com)
                "[!command(com,true)]X"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Or(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) || var = 1
                "((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]X) && [!command(com,true)]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new And(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) || var = 1
                "(!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!command(com,true)]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new And(
                    new Or(new Command("com1"), new Command("com2")),
                    new Variable("var", "=", Domain.INT, 1)), // (CmdChk(com1) || CmdChk(com2)) && var = 1
                "((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(com1,true) || command(com2,true))]X) && " +
                "(!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(com2,true) || command(com1,true))]X))"
            ],
        ];
        #endregion
        
        [TestMethod]
        [DynamicData(nameof(PhiParseTestData))]
        public void ParseTest(PhiType type, IModalFormula sigma, IExpression expression, string expectedMCRL2)
        {
            var phi = new Phi(type, Event.A, sigma);

            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = expression
            };

            var formula = phi.ToMuCalc(substitutions).Flatten().ToMCRL2(substitutions);

            Assert.AreEqual(expectedMCRL2, formula);
        }

        #region PhiDoubleExpParseTestData

        public static IEnumerable<object[]> PhiDoubleExpParseTestData { get; } =
        [
            /*
             * [!(A || B)*][A]false
             */
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["A", "true"]))), Bool.False),
                new Command("A"),
                new Command("B"),
                "nu P_1 . ([!(command(A,true) || command(B,true))]P_1 && [command(A,true)]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["B", "true"]))), Bool.False),
                new Command("A"),
                new Variable("var", "=", Domain.INT, 1),
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!command(A,true)]P_1) && [command(B,true)]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["C", "true"]))), Bool.False),
                new Or(new Command("A"), new Command("B")),
                new Variable("var", "=", Domain.INT, 1),
                "(nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(A,true) || command(B,true))]P_1) && [command(C,true)]false)) && " +
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(B,true) || command(A,true))]P_1) && [command(C,true)]false)))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["D", "true"]))), Bool.False),
                new Or(new Command("A"), new Command("B")),
                new And(new Command("C"), new Variable("var", "=", Domain.INT, 1)),
                "((nu P_1 . ([!command(A,true)]P_1 && [command(D,true)]false)) && nu P_1 . ([!command(B,true)]P_1 && [command(D,true)]false))) && " +
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!((command(C,true) && command(A,true)) && command(B,true))]P_1) && [command(D,true)]false)))"
            ],
            /*
             * [!(A || B)]X
             */
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Command("A"),
                new Command("B"),
                "[!(command(A,true) || command(B,true))]X"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Command("A"),
                new Variable("var", "=", Domain.INT, 1),
                "(!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!command(A,true)]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Or(new Command("A"), new Command("B")),
                new Variable("var", "=", Domain.INT, 1),
                "((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(A,true) || command(B,true))]X) && (!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(B,true) || command(A,true))]X))"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Or(new Command("A"), new Command("B")),
                new And(new Command("C"), new Variable("var", "=", Domain.INT, 1)),
                "(([!command(A,true)]X && [!command(B,true)]X) && (!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!((command(C,true) && command(A,true)) && command(B,true))]X))"
            ],
        ];
        #endregion
        
        [TestMethod]
        [DynamicData(nameof(PhiDoubleExpParseTestData))]
        public void DoubleExpressionParseTest(PhiType type, IModalFormula sigma, IExpression exp1, IExpression exp2, string expectedMCRL2)
        {
            var phi = new Phi(type, Event.A | Event.B, sigma);

            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = exp1,
                [Event.B] = exp2,
            };

            var formula = phi.ToMuCalc(substitutions).Flatten().ToMCRL2(substitutions);
                
            Assert.AreEqual(expectedMCRL2, formula);
        }

        public static IEnumerable<object[]> ParseCommandTestData { get; } =
        [
            [PhiType.Pos, "[command(a,true)]X", false],
            //[PhiType.Pos, "[!command(a,true)]false", true],
            [PhiType.Neg, "nu P_1 . ([!command(a,true)]P_1 && X))", false],
            //[PhiType.Neg, "[!!command(a,true)*]false", true],
            [PhiType.Fix, "[!command(a,true)]X", false],
            //[PhiType.Fix, "[!!command(a,true)]false", true]
        ];
        
        [TestMethod]
        [DynamicData(nameof(ParseCommandTestData))]
        public void ParseCommandTest(PhiType type, string expectedFormula, bool negated)
        {
            var phi = new Phi(type, Event.A, new FixPoint("X"));

            var cmd = new Command("a");
            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = cmd,
            };

            var formula = phi.ToMuCalc(substitutions).Flatten().ToMCRL2(null);

            Assert.AreEqual(expectedFormula, formula);
        }
        
        public static IEnumerable<object[]> ParseVariableTestData { get; } =
        [
            [PhiType.Pos, "((exists s_1:Int . (<v(s_1)>true && s_1 = 1)) -> [true]X)"],
            [PhiType.Neg, "nu P_1 . ((!(exists s_1:Int . (<v(s_1)>true && s_1 = 1)) -> [true]P_1) && X))"],
            [PhiType.Fix, "(!(exists s_1:Int . (<v(s_1)>true && s_1 = 1)) -> [true]X)"]
        ];
        
        [TestMethod]
        [DynamicData(nameof(ParseVariableTestData))]
        public void ParseVariableTest(PhiType type, string expectedFormula)
        {
            var phi = new Phi(type, Event.A, new FixPoint("X"));

            var variable = new Variable("v", "=", Domain.INT, 1);
            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = variable,
            };

            var formula = phi.ToMuCalc(substitutions).Flatten().ToMCRL2(null);

            Assert.AreEqual(expectedFormula, formula);
        }
        
        public static IEnumerable<object[]> ParseOrTestData { get; } =
        [
            [
                PhiType.Pos, 
                new Or(new Variable("v", "=", Domain.INT, 1), new Variable("w", "=", Domain.INT, 1)),
                "(((exists s_1:Int . (<v(s_1)>true && s_1 = 1)) || (exists s_1:Int . (<w(s_1)>true && s_1 = 1))) -> [true]false)"
            ],
            [
                PhiType.Pos, 
                new Or(new Command("a"), new Variable("w", "=", Domain.INT, 1)),
                "(((exists s_1:Int . (<w(s_1)>true && s_1 = 1)) -> [true]false) && [command(a,true)]false)"
            ],
            [
                PhiType.Pos, 
                new Or(new Command("a"), new Command("b")),
                "[(command(a,true) || command(b,true))]false"
            ],
        ];
        
        [TestMethod]
        [DynamicData(nameof(ParseOrTestData))]
        public void ParseOrTest(PhiType type, Or expression, string expectedFormula)
        {
            var phi = new Phi(type, Event.A, Bool.False);

            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = expression,
            };

            var formula = phi.ToMuCalc(substitutions).Flatten().ToMCRL2(null);

            Assert.AreEqual(expectedFormula, formula);
        }
        
        public static IEnumerable<object[]> ParseAndTestData { get; } =
        [
            [
                PhiType.Pos, 
                new And(new Variable("v", "=", Domain.INT, 1), new Variable("w", "=", Domain.INT, 1)),
                "(((exists s_1:Int . (<v(s_1)>true && s_1 = 1)) && (exists s_1:Int . (<w(s_1)>true && s_1 = 1))) -> [true]false)"
            ],
            [
                PhiType.Pos, 
                new And(new Command("a"), new Variable("w", "=", Domain.INT, 1)),
                "((exists s_1:Int . (<w(s_1)>true && s_1 = 1)) -> [command(a,true)]false)"
            ],
        ];
        
        [TestMethod]
        [DynamicData(nameof(ParseAndTestData))]
        public void ParseAndTest(PhiType type, And expression, string expectedFormula)
        {
            var phi = new Phi(type, Event.A, Bool.False);

            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = expression,
            };

            var formula = phi.ToMuCalc(substitutions).Flatten().ToMCRL2(null);
            
            Assert.AreEqual(expectedFormula, formula);
        }
    }
}