using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class CuboidTests
{
    static bool WriteTests = false;
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCuboidDefaults()
    {
        var obs = Cuboid(center: (0,0,0)); // CSCAD changed default center.
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        if (WriteTests) TestData.Make("CuboidDefExp1", pts);
        var exp = UnitTestData.CuboidDefExp1;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestCuboidOptions()
    {
        // test center
        var obs = Cuboid(size: (6, 6, 6), center: (3, 5, 7));
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        if(WriteTests) TestData.Make("CuboidOptsExp1", pts);
        var exp = UnitTestData.CuboidOptsExp1;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test size
        obs = Cuboid(size: (4.5, 1.5, 7), center: (0,0,0));
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        if(WriteTests) TestData.Make("CuboidOptsExp2", pts);
        exp = UnitTestData.CuboidOptsExp2;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
