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
        public static IEnumerable<object[]> PhiTestData { get; } =
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
                "(true -> [command(com,true)]false)"
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
                "[!command(com,true)*](true -> [B]false)"
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
                "[!command(com,true)](!true -> X)"
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
        ];

        [TestMethod]
        [DynamicData(nameof(PhiTestData))]
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
    }
}