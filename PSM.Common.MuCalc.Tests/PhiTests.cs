using PSM.Common.MuCalc.Common;
using PSM.Common.MuCalc.Dissections;
using PSM.Common.MuCalc.Dissections.Labels;
using PSM.Common.MuCalc.Dissections.Labels.Operations;
using PSM.Common.MuCalc.ModalFormula;
using Action = PSM.Common.MuCalc.Actions.Action;

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
                "(((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [true]false) || [command(com,true)]false)"
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
                "((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [(command(com1,true) || command(com2,true))]false)"
            ],
            [
                PhiType.Pos,
                Bool.False,
                new And(
                    new Or(new Command("com"), new Variable("var2", "=", Domain.INT, 2)),
                    new Variable("var1", "=", Domain.INT, 1)), // (CmdChk(com) || var2 = 2) && var1 = 1
                "(((exists s_1:Int . (<var1(s_1)>true && s_1 = 1)) -> [command(com,true)]false) || (((exists s_1:Int . (<var2(s_1)>true && s_1 = 2)) && (exists s_1:Int . (<var1(s_1)>true && s_1 = 1))) -> [true]false))"
            ],
            /*
             * [(!A)* . B]false
             */
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Variable("var", "=", Domain.INT, 1), // var = 1
                "[true*]((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [B]false)"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Command("com"), // CmdChk(com)
                "[!command(com,true)*][B]false"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new Or(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) || var = 1
                "([true*]((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [B]false) || [!command(com,true)*][B]false)"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new And(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) && var = 1
                "[!command(com,true)*]((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [B]false)"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action(Event.B.ToString()))), Bool.False),
                new And(
                    new Or(new Command("com1"), new Command("com2")),
                    new Variable("var", "=", Domain.INT, 1)), // (CmdChk(com1) || CmdChk(com2)) && var = 1
                "[!(command(com1,true) || command(com2,true))*]((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [B]false)"
            ],
            /*
             * [(!A)* . B]false
             */
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Variable("var", "=", Domain.INT, 1), // var = 1
                "[true](!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> X)"
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
                "([true](!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> X) || [!command(com,true)]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new And(new Command("com"), new Variable("var", "=", Domain.INT, 1)), // CmdChk(com) || var = 1
                "[!command(com,true)](!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new And(
                    new Or(new Command("com1"), new Command("com2")),
                    new Variable("var", "=", Domain.INT, 1)), // (CmdChk(com1) || CmdChk(com2)) && var = 1
                "[!(command(com1,true) || command(com2,true))](!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> X)"
            ],
        ];
        #endregion
        
        [TestMethod]
        [DynamicData(nameof(PhiParseTestData))]
        public void ParseTest(PhiType type, IModalFormula sigma, IExpression expression, string expectedMCRL2)
        {
            var phi = new Phi(type, Event.A, sigma);

            var formula = phi.ParseExpression(expression);

            var substitutions = new Dictionary<Event, IExpression>
            {
                [Event.A] = expression
            };
            Assert.AreEqual(expectedMCRL2, formula.ToMCRL2(substitutions));
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
                "[!(command(A,true) || command(B,true))*][command(A,true)]false"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["A", "true"]))), Bool.False),
                new Command("A"),
                new Variable("var", "=", Domain.INT, 1),
                "([true*]((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(A,true)]false) || [!command(A,true)*][command(A,true)]false)"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["A", "true"]))), Bool.False),
                new Or(new Command("A"), new Command("B")),
                new Variable("var", "=", Domain.INT, 1),
                "([true*]((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(A,true)]false) || [!(command(A,true) || command(B,true))*][command(A,true)]false)"
            ],
            [
                PhiType.Neg,
                new Box(new RegularFormula.ActionFormula(new ActionFormula.ActionFormula(new Action("command", ["A", "true"]))), Bool.False),
                new Or(new Command("A"), new Command("B")),
                new And(new Command("C"), new Variable("var", "=", Domain.INT, 1)),
                "([!(command(A,true) || command(B,true))*][command(A,true)]false || [!command(C,true)*]((exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> [command(A,true)]false))"
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
                "([true](!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> X) || [!command(A,true)]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Or(new Command("A"), new Command("B")),
                new Variable("var", "=", Domain.INT, 1),
                "([true](!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> X) || [!(command(A,true) || command(B,true))]X)"
            ],
            [
                PhiType.Fix,
                new FixPoint("X"),
                new Or(new Command("A"), new Command("B")),
                new And(new Command("C"), new Variable("var", "=", Domain.INT, 1)),
                "([!(command(A,true) || command(B,true))]X || [!command(C,true)](!(exists s_1:Int . (<var(s_1)>true && s_1 = 1)) -> X))"
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

            var formula = phi.ToMuCalc(substitutions);
                
            Assert.AreEqual(expectedMCRL2, formula.ToMCRL2(substitutions));
        }
    }
}