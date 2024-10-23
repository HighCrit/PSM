using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Dissections;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Parsers.Labels.Labels;
using PSM.Parsers.Labels.Labels.Operations;
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
                new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString().ToString(), 1), // var = 1
                "((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]false)"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new Command(0, "com"), // CmdChk(com)
                "[command(com,true)]false"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new Or(new Command(0, "com"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // CmdChk(com) || var = 1
                "(((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]false) && [command(com,true)]false)"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new And(new Command(0, "com"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // CmdChk(com) && var = 1
                "((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(com,true)]false)"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new And(
                    new Or(new Command(0, "com1"), new Command(0, "com2")),
                    new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // (CmdChk(com1) || CmdChk(com2)) && var = 1
                "(((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(com1,true)]false) && ((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(com2,true)]false))"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new And(
                    new Or(new Command(0, "com"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var2"), "=", Domain.INT.ToString(), 2)),
                    new Variable(ModelInfoFor(ModelInfoType.Variable, "var1"), "=", Domain.INT.ToString(), 1)), // (CmdChk(com) || var2 = 2) && var1 = 1
                "(((exists s_1:Int . (<var1(s_1)>true && s_1 = 1)) -> [command(com,true)]false) && " +
                "(((exists s_1:Int . (<var1(s_1)>true && s_1 = 1)) && (exists s_1:Int . (<var2(s_1)>true && s_1 = 2))) -> [true]false))"
            ],
            /*
             * [(!A)* . B]false
             */
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1), // var = 1
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]P_1) && [B]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Command(0, "com"), // CmdChk(com)
                "nu P_1 . ([!command(com,true)]P_1 && [B]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Or(new Command(0, "com"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // CmdChk(com) || var = 1
                "(nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]P_1) && [B]false)) && nu P_1 . ([!command(com,true)]P_1 && [B]false)))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new And(new Command(0, "com"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // CmdChk(com) && var = 1
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!command(com,true)]P_1) && [B]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new And(
                    new Or(new Command(0, "com1"), new Command(0, "com2")),
                    new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // (CmdChk(com1) || CmdChk(com2)) && var = 1
                "(nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(com1,true) || command(com2,true))]P_1) && [B]false)) && " +
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(com2,true) || command(com1,true))]P_1) && [B]false)))"
            ],
            /*
             * [(!A)* . B]false
             */
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1), // var = 1
                "(!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Command(0, "com"), // CmdChk(com)
                "[!command(com,true)]X"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Or(new Command(0, "com"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // CmdChk(com) || var = 1
                "((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]X) && [!command(com,true)]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new And(new Command(0, "com"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // CmdChk(com) || var = 1
                "(!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!command(com,true)]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new And(
                    new Or(new Command(0, "com1"), new Command(0, "com2")),
                    new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)), // (CmdChk(com1) || CmdChk(com2)) && var = 1
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

            var formula = phi.ApplySubstitutions(substitutions).Flatten().ToMCRL2();

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
                new Command(0, "A"),
                new Command(0, "B"),
                "nu P_1 . ([!(command(A,true) || command(B,true))]P_1 && [command(A,true)]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["B", "true"]))), Bool.False),
                new Command(0, "A"),
                new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1),
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!command(A,true)]P_1) && [command(B,true)]false))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["C", "true"]))), Bool.False),
                new Or(new Command(0, "A"), new Command(0, "B")),
                new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1),
                "(nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(A,true) || command(B,true))]P_1) && [command(C,true)]false)) && " +
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(B,true) || command(A,true))]P_1) && [command(C,true)]false)))"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["D", "true"]))), Bool.False),
                new Or(new Command(0, "A"), new Command(0, "B")),
                new And(new Command(0, "C"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)),
                "((nu P_1 . ([!command(A,true)]P_1 && [command(D,true)]false)) && nu P_1 . ([!command(B,true)]P_1 && [command(D,true)]false))) && " +
                "nu P_1 . ((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!((command(C,true) && command(A,true)) && command(B,true))]P_1) && [command(D,true)]false)))"
            ],
            /*
             * [!(A || B)]X
             */
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Command(0, "A"),
                new Command(0, "B"),
                "[!(command(A,true) || command(B,true))]X"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Command(0, "A"),
                new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1),
                "(!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!command(A,true)]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Or(new Command(0, "A"), new Command(0, "B")),
                new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1),
                "((!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(A,true) || command(B,true))]X) && (!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [!(command(B,true) || command(A,true))]X))"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Or(new Command(0, "A"), new Command(0, "B")),
                new And(new Command(0, "C"), new Variable(ModelInfoFor(ModelInfoType.Variable, "var"), "=", Domain.INT.ToString(), 1)),
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

            var formula = phi.ApplySubstitutions(substitutions).Flatten().ToMCRL2();
                
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

            var cmd = new Command(0, "a");
            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = cmd,
            };

            var formula = phi.ApplySubstitutions(substitutions).Flatten().ToMCRL2();

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

            var variable = new Variable(ModelInfoFor(ModelInfoType.Variable, "v"), "=", Domain.INT.ToString(), 1);
            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = variable,
            };

            var formula = phi.ApplySubstitutions(substitutions).Flatten().ToMCRL2();

            Assert.AreEqual(expectedFormula, formula);
        }
        
        public static IEnumerable<object[]> ParseOrTestData { get; } =
        [
            [
                PhiType.Pos, 
                new Or(new Variable(ModelInfoFor(ModelInfoType.Variable, "v"), "=", Domain.INT.ToString(), 1), new Variable(ModelInfoFor(ModelInfoType.Variable, "w"), "=", Domain.INT.ToString(), 1)),
                "(((exists s_1:Int . (<v(s_1)>true && s_1 = 1)) || (exists s_1:Int . (<w(s_1)>true && s_1 = 1))) -> [true]false)"
            ],
            [
                PhiType.Pos, 
                new Or(new Command(0, "a"), new Variable(ModelInfoFor(ModelInfoType.Variable, "w"), "=", Domain.INT.ToString(), 1)),
                "(((exists s_1:Int . (<w(s_1)>true && s_1 = 1)) -> [true]false) && [command(a,true)]false)"
            ],
            [
                PhiType.Pos, 
                new Or(new Command(0, "a"), new Command(0, "b")),
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

            var formula = phi.ApplySubstitutions(substitutions).Flatten().ToMCRL2();

            Assert.AreEqual(expectedFormula, formula);
        }
        
        public static IEnumerable<object[]> ParseAndTestData { get; } =
        [
            [
                PhiType.Pos, 
                new And(new Variable(ModelInfoFor(ModelInfoType.Variable, "v"), "=", Domain.INT.ToString(), 1), new Variable(ModelInfoFor(ModelInfoType.Variable, "w"), "=", Domain.INT.ToString(), 1)),
                "(((exists s_1:Int . (<v(s_1)>true && s_1 = 1)) && (exists s_1:Int . (<w(s_1)>true && s_1 = 1))) -> [true]false)"
            ],
            [
                PhiType.Pos, 
                new And(new Command(0, "a"), new Variable(ModelInfoFor(ModelInfoType.Variable, "w"), "=", Domain.INT.ToString(), 1)),
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

            var formula = phi.ApplySubstitutions(substitutions).Flatten().ToMCRL2();
            
            Assert.AreEqual(expectedFormula, formula);
        }

        private static ModelInfo ModelInfoFor(ModelInfoType type, string name)
        {
            return new ModelInfo(type, 0, name);
        }
    }
}