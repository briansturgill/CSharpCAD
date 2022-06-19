using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class RoundedRectangleTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRoundedRectangleDefaults()
    {
        var geometry = RoundedRectangle(center: (0, 0)); // CSCAD changed the default center.
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();

        Assert.AreEqual(obs.Length, 36);
    }

    [Test]
    public void TestRoundedRectangleOptions()
    {
        // test center
        var geometry = RoundedRectangle(center: (4, 5), segments: 16);
        Assert.DoesNotThrow(() => geometry.Validate());
        var obs = geometry.ToPoints();
        if (WriteTests) TestData.Make("RoundRectOptsExp1", obs);
        var exp = UnitTestData.RoundRectOptsExp1;
        Assert.AreEqual(obs.Length, 20);
        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        // test size
        geometry = RoundedRectangle(size: (10, 6), segments: 16, center: (0, 0));
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        if (WriteTests) TestData.Make("RoundRectOptsExp2", obs);
        exp = UnitTestData.RoundRectOptsExp2;
        Assert.AreEqual(obs.Length, 20);
        Assert.IsTrue(Helpers.CompareArrays(obs, exp));

        // test roundRadius
        geometry = RoundedRectangle(size: (10, 6), roundRadius: 2, segments: 16, center: (0, 0));
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        if (WriteTests) TestData.Make("RoundRectOptsExp3", obs);
        exp = UnitTestData.RoundRectOptsExp3;
        Assert.AreEqual(obs.Length, 20);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // test segments
        geometry = RoundedRectangle(size: (10, 6), roundRadius: 2, segments: 64, center: (0, 0));
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = geometry.ToPoints();
        Assert.AreEqual(obs.Length, 68);
    }

}
