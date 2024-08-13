using PSM.Common.PROPEL;

namespace PSM.Translators.PROPEL.Tests;

[TestClass]
public class EnumExtensionsTest
{
    [TestMethod]
    public void TestGetFlags()
    {
        var flag = Option.Precedency | Option.Bounded;
        var flags = flag.GetFlags().ToList();

        var expected = new List<Option> { Option.Bounded, Option.Precedency };

        CollectionAssert.AreEqual(expected, flags);
    }

    [TestMethod]
    public void TestGetFlagsWithNone()
    {
        var flag = Option.Precedency | Option.Bounded | Option.None;
        var flags = flag.GetFlags().ToList();

        var expected = new List<Option> { Option.Bounded, Option.Precedency };

        CollectionAssert.AreEqual(expected, flags);
    }

    [TestMethod]
    public void TestGetCombinations()
    {
        var flag = Option.Precedency | Option.Bounded;
        var flags = flag.GetFlags().ToList();
        var combinations = flags.GetAllCombinations().ToList();

        var expected = new List<Option>
        {
            Option.None,
            Option.Bounded,
            Option.Precedency,
            Option.Bounded | Option.Precedency,
        };

        CollectionAssert.AreEqual(expected, combinations);
    }

    [TestMethod]
    public void TestGetCombinations2()
    {
        var flag = PropertyInfo.GetAvailableOptionsFor(Behaviour.Existence, Scope.After_Q);
        var flags = flag.GetFlags().ToList();
        var combinations = flags.GetAllCombinations().ToList();

        var expected = new List<Option>
        {
            Option.None,
            Option.Bounded,
            Option.LastStart,
            Option.Bounded | Option.LastStart,
        };

        CollectionAssert.AreEqual(expected, combinations);
    }
}
