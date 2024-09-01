using PSM.Parsers.Labels.Labels;
using PSM.Parsers.Labels.Labels.Operations;

namespace PSM.Parsers.Labels.Tests;

[TestClass]
public class LabelParserTests
{
    [TestMethod]
    public void LabelParserTest()
    {
        var expectedExpr =
            new Or(
                new And(
                    new Variable("V1", "=", "Bool", "true"),
                    new Variable("V2", "=", "Bool", "true"),
                    new Variable("V3", "=", "Bool", "false")), 
                new Command("Test"));
        
        var labelString = "(V1 = true && V2 = true && V3 = false) || CmdChk(Test)";
        var actualExpr = LabelParser.Parse(labelString);
        
        Assert.AreEqual(expectedExpr, actualExpr);
    }
    
    [TestMethod]
    public void CommandParseTest()
    {
        var expectedExpr = new Command("Test");
        
        var labelString = "CmdChk(Test)";
        var actualExpr = LabelParser.Parse(labelString);
        
        Assert.AreEqual(expectedExpr, actualExpr);
    }
}