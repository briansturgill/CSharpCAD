using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class ExpandTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExpandGeom2()
    {
        var geometry = new Geom2(new Vec2[] { new Vec2(-8, -8), new Vec2(8, -8), new Vec2(8, 8), new Vec2(-8, 8) });

        var obs = (Geom2)Expand(geometry, delta: 2, corners: Corners.Round, segments: 8);
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
    }

    [Test]
    public void TestExpandComplexGeom2()
    {
        var geometry = new Geom2(new Geom2.Side[] {
          new Geom2.Side(new Vec2(-75.00000, 75.00000), new Vec2(-75.00000, -75.00000)),
          new Geom2.Side(new Vec2(-75.00000, -75.00000), new Vec2(75.00000, -75.00000)),
          new Geom2.Side(new Vec2(75.00000, -75.00000), new Vec2(75.00000, 75.00000)),
          new Geom2.Side(new Vec2(-40.00000, 75.00000), new Vec2(-75.00000, 75.00000)),
          new Geom2.Side(new Vec2(75.00000, 75.00000), new Vec2(40.00000, 75.00000)),
          new Geom2.Side(new Vec2(40.00000, 75.00000), new Vec2(40.00000, 0.00000)),
          new Geom2.Side(new Vec2(40.00000, 0.00000), new Vec2(-40.00000, 0.00000)),
          new Geom2.Side(new Vec2(-40.00000, 0.00000), new Vec2(-40.00000, 75.00000)),
          new Geom2.Side(new Vec2(15.00000, -10.00000), new Vec2(15.00000, -40.00000)),
          new Geom2.Side(new Vec2(-15.00000, -10.00000), new Vec2(15.00000, -10.00000)),
          new Geom2.Side(new Vec2(-15.00000, -40.00000), new Vec2(-15.00000, -10.00000)),
          new Geom2.Side(new Vec2(-8.00000, -40.00000), new Vec2(-15.00000, -40.00000)),
          new Geom2.Side(new Vec2(15.00000, -40.00000), new Vec2(8.00000, -40.00000)),
          new Geom2.Side(new Vec2(-8.00000, -25.00000), new Vec2(-8.00000, -40.00000)),
          new Geom2.Side(new Vec2(8.00000, -25.00000), new Vec2(-8.00000, -25.00000)),
          new Geom2.Side(new Vec2(8.00000, -40.00000), new Vec2(8.00000, -25.00000)),
          new Geom2.Side(new Vec2(-2.00000, -15.00000), new Vec2(-2.00000, -19.00000)),
          new Geom2.Side(new Vec2(-2.00000, -19.00000), new Vec2(2.00000, -19.00000)),
          new Geom2.Side(new Vec2(2.00000, -19.00000), new Vec2(2.00000, -15.00000)),
          new Geom2.Side(new Vec2(2.00000, -15.00000), new Vec2(-2.00000, -15.00000))
        });

        // expand +
        var obs = (Geom2)Expand(geometry, delta: 2, corners: Corners.Edge);
        var pts = obs.ToPoints();
        var exp = new Vec2[] {
          new Vec2(77, -77),
          new Vec2(77, 77),
          new Vec2(38, 77),
          new Vec2(38, 2),
          new Vec2(-38, 2),
          new Vec2(-37.99999999999999, 77),
          new Vec2(-77, 77),
          new Vec2(16.999999999999996, -42),
          new Vec2(6, -42),
          new Vec2(6, -27),
          new Vec2(-6, -27),
          new Vec2(-6.000000000000001, -42),
          new Vec2(-17, -42),
          new Vec2(-16.999999999999996, -8),
          new Vec2(17, -8.000000000000004),
          new Vec2(-4, -21),
          new Vec2(3.9999999999999996, -21),
          new Vec2(4, -13),
          new Vec2(-4, -13),
          new Vec2(-77, -77)
          };
        Assert.AreEqual(pts.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
    }
}
