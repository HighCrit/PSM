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
                    new Variable(this.ModelInfoFor(ModelInfoType.Variable, "V1"), "=", "Bool", "true"),
                    new Variable(this.ModelInfoFor(ModelInfoType.Variable, "V2"), "=", "Bool", "true"),
                    new Variable(this.ModelInfoFor(ModelInfoType.Variable, "V3"), "=", "Bool", "false")), 
                new Command(0, "Test"));
        
        var labelString = "(V1 = true && V2 = true && V3 = false) || CmdChk(0,Test)";
        var actualExpr = LabelParser.Parse([], labelString);
        
        Assert.AreEqual(expectedExpr, actualExpr);
    }
    
    [TestMethod]
    public void CommandParseTest()
    {
        var expectedExpr = new Command(0, "Test");
        
        var labelString = "CmdChk(0,Test)";
        var actualExpr = LabelParser.Parse([], labelString);
        
        Assert.AreEqual(expectedExpr, actualExpr);
    }

    private ModelInfo ModelInfoFor(ModelInfoType type, string name)
    {
        return new ModelInfo(type, 0, name);
    }
}