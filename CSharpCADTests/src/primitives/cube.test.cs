using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class CubeTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCubeDefaults()
    {
        var obs = Cube(size: 2, center: (0,0, 0)); // CSCAD changed the default center.
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPolygons();
        foreach (var p in pts)
        {
        }
        Assert.AreEqual(pts.Length, 6);
    }

    [Test]
    public void TestCubeOptions()
    {
        // test center
        var obs = Cube(size: 7, center: (6.5, 6.5, 6.5));
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        if (WriteTests) TestData.Make("CubeOptionsExp1", pts);
        var exp = UnitTestData.CubeOptionsExp1;

        Assert.AreEqual(pts.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test size
        obs = Cube(size: 7, center: (0,0, 0));
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        if (WriteTests) TestData.Make("CubeOptionsExp2", pts);
        exp = UnitTestData.CubeOptionsExp2;

        Assert.AreEqual(pts.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
