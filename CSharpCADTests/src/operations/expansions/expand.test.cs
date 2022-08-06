using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class ExpandTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExpandGeom2()
    {
        var geometry = new Geom2(new Vec2[] { new Vec2(-8, -8), new Vec2(8, -8), new Vec2(8, 8), new Vec2(-8, 8) });
        Assert.DoesNotThrow(() => (geometry).Validate());

        var obs = Expand(geometry, delta: 2, corners: Corners.Round, segments: 8);
        var pts = obs.ToPoints();
        var exp = new Vec2[] {
          new Vec2(-9.414213562373096, -9.414213562373096),
          new Vec2(-8, -10),
          new Vec2(8, -10),
          new Vec2(9.414213562373096, -9.414213562373096),
          new Vec2(10, -8),
          new Vec2(10, 8),
          new Vec2(9.414213562373096, 9.414213562373096),
          new Vec2(8, 10),
          new Vec2(-8, 10),
          new Vec2(-9.414213562373096, 9.414213562373096),
          new Vec2(-10, 8),
          new Vec2(-10, -8)
          };
        Assert.AreEqual(pts.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());
    }

    [Test]
    public void TestExpandComplexGeom2()
    {
        var nrtree = new Geom2.NRTree();
        nrtree.Insert(new Vec2[] {
          new Vec2(-75.00000, -75.00000),
          new Vec2(75.00000, -75.00000),
          new Vec2(75.00000, 75.00000),
          new Vec2(-75.00000, 75.00000),
          new Vec2(40.00000, 75.00000),
          new Vec2(40.00000, 0.00000),
          new Vec2(-40.00000, 0.00000),
          new Vec2(-40.00000, 75.00000),
        });
        nrtree.Insert(new Vec2[] {
          new Vec2(15.00000, -40.00000),
          new Vec2(15.00000, -10.00000),
          new Vec2(-15.00000, -10.00000),
          new Vec2(-15.00000, -40.00000),
          new Vec2(8.00000, -40.00000),
          new Vec2(-8.00000, -40.00000),
          new Vec2(-8.00000, -25.00000),
          new Vec2(8.00000, -25.00000),
        });
        nrtree.Insert(new Vec2[] {
          new Vec2(-2.00000, -19.00000),
          new Vec2(2.00000, -19.00000),
          new Vec2(2.00000, -15.00000),
          new Vec2(-2.00000, -15.00000)
        });
        nrtree.CorrectWindings();
        var geometry = new Geom2(nrtree);
        Assert.DoesNotThrow(() => (geometry).Validate());

        // expand +
        var obs = Expand(geometry, delta: 2, corners: Corners.Edge);
        var pts = obs.ToPoints();
        if(WriteTests) TestData.Make("ExpandComplexExp1", pts);
        var exp = UnitTestData.ExpandComplexExp1;
        Assert.AreEqual(pts.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());
    }
}
