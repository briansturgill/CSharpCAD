using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class CutterTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCutter2DDefaults()
    {
        var obs = Cutter2D();
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 3);
    }

    [Test]
    public void TestCutter2DOptions()
    {
        // test height
        var circ1 = Circle(radius: 20);
        var circ2 = Circle(radius: 20, center: (23, 23));
        var cut1 = Cutter2D(radius: 20, startAngle: 90, endAngle: 270);
        var cut2 = Cutter2D(radius: 20, startAngle: 90, endAngle: 270, center: (23, 23));
        Assert.DoesNotThrow(() => cut1.Validate());
        Assert.DoesNotThrow(() => cut2.Validate());

        var obs1 = Subtract(circ1, cut1);
        Assert.DoesNotThrow(() => obs1.Validate());
        var pts = obs1.ToPoints();
        if (WriteTests) TestData.Make("CutterOptsExp1", pts);
        var exp = UnitTestData.CutterOptsExp1;

        Assert.AreEqual(pts.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test center
        var obs2 = Subtract(circ2, cut2);
        Assert.DoesNotThrow(() => obs2.Validate());
        pts = obs2.ToPoints();
        if (WriteTests) TestData.Make("CutterOptsExp2", pts);
        exp = UnitTestData.CutterOptsExp2;

        Assert.AreEqual(pts.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        //Save("/tmp/a.svg", Union(obs1, Translate((50, 0, 0), obs2)));
    }

    [Test]
    public void TestCutter3DDefaults()
    {
        var obs = Cutter3D();
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 12);
    }

    [Test]
    public void TestCutter3DOptions()
    {
        // test height
        var sphere1 = Sphere(radius: 20);
        var sphere2 = Sphere(radius: 20, center: (23, 23, 23));
        var cut1 = Cutter3D(radius: 20, startAngle: 90, endAngle: 270);
        var cut2 = Cutter3D(radius: 20, startAngle: 90, endAngle: 270, center: (23, 23, 23));
        Assert.DoesNotThrow(() => cut1.Validate());
        Assert.DoesNotThrow(() => cut2.Validate());

        var obs1 = Subtract(sphere1, cut1);
        Assert.DoesNotThrow(() => obs1.Validate());
        var pts = obs1.ToPoints();
        if (WriteTests) TestData.Make("Cutter3DOptsExp1", pts);
        var exp = UnitTestData.Cutter3DOptsExp1;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test center
        var obs2 = Subtract(sphere2, cut2);
        Assert.DoesNotThrow(() => obs2.Validate());
        pts = obs2.ToPoints();
        if (WriteTests) TestData.Make("Cutter3DOptsExp2", pts);
        exp = UnitTestData.Cutter3DOptsExp2;

        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
