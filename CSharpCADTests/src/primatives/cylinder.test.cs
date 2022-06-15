using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class CylinderTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCylinderDefaults()
    {
        var obs = Cylinder();
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 34);
    }


    [Test]
    public void TestCylinderOptions()
    {
        var obs = Cylinder(height: 10, radius: 4, segments: 5);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        if(WriteTests) TestData.Make("CylinderOptsExp1", pts);
        var exp = UnitTestData.CylinderOptsExp1;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test center
        obs = Cylinder(center: (-5, -5, -5), segments: 5);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        if(WriteTests) TestData.Make("CylinderOptsExp2", pts);
        exp = UnitTestData.CylinderOptsExp2;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
