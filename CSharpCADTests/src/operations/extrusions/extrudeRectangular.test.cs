using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class ExtrudeRectangularTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExtrudeRectangularDefaults()
    {
        var geometry2 = Rectangle(new Opts { { "size", (5, 5) } });

        var obs = ExtrudeRectangular(geometry2);
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Count, 32);
    }

    [Test]
    public void TestExtrudeRectangularChamfer()
    {
        var geometry2 = Rectangle(new Opts { { "size", (5, 5) } });

        var obs = ExtrudeRectangular(geometry2, corners: Corners.Chamfer);
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Count, 48);
    }

    [Test]
    public void TestExtrudeRectangularSegsRound()
    {
        var geometry2 = Rectangle(new Opts { { "size", (5, 5) } });

        var obs = ExtrudeRectangular(geometry2, segments: 8, corners: Corners.Round);
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Count, 64);
    }

    [Test]
    public void TestExtrudeRectangularHoles()
    {
        var geometry2 = new Geom2(new Geom2.Side[] {
          new Geom2.Side(new Vec2(15.00000, 15.00000), new Vec2(-15.00000, 15.00000)),
          new Geom2.Side(new Vec2(-15.00000, 15.00000), new Vec2(-15.00000, -15.00000)),
          new Geom2.Side(new Vec2(-15.00000, -15.00000), new Vec2(15.00000, -15.00000)),
          new Geom2.Side(new Vec2(15.00000, -15.00000), new Vec2(15.00000, 15.00000)),
          new Geom2.Side(new Vec2(-5.00000, 5.00000), new Vec2(5.00000, 5.00000)),
          new Geom2.Side(new Vec2(5.00000, 5.00000), new Vec2(5.00000, -5.00000)),
          new Geom2.Side(new Vec2(5.00000, -5.00000), new Vec2(-5.00000, -5.00000)),
          new Geom2.Side(new Vec2(-5.00000, -5.00000), new Vec2(-5.00000, 5.00000))
        });

        var obs = ExtrudeRectangular(geometry2, size: 2, height: 15, segments: 16, corners: Corners.Round);
    var pts = obs.ToPoints();
    Assert.AreEqual(pts.Count, 192);
    }
}