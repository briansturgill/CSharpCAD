using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class TestCenter
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCenterGeom2()
    {
        var geometry = new Geom2(new List<Vec2> { new Vec2(0, 0), new Vec2(10, 0), new Vec2(0, 10) });

        // center about Y
        var centered = Center(geometry, axisX: false, axisY: true);
        Assert.DoesNotThrow(() => centered.Validate());
        var pts = new List<Vec2>();
        pts.AddRange(centered.ToPoints());
        var exp = new List<Vec2> { new Vec2(0, -5), new Vec2(10, -5), new Vec2(0, 5) };
        Assert.True(Helpers.CompareListsNEVec2(pts, exp));

        centered = CenterY(geometry);
        Assert.DoesNotThrow(() => centered.Validate());
        pts = new List<Vec2>();
        pts.AddRange(centered.ToPoints());
        Assert.True(Helpers.CompareListsNEVec2(pts, exp));
    }

    [Test]
    public void TestGeom3()
    {
        var points = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(-2, -7, 18), new Vec3(-2, 13, 18), new Vec3(-2, 13, -12)},
          new List<Vec3>{new Vec3(8, -7, -12), new Vec3(8, 13, -12), new Vec3(8, 13, 18), new Vec3(8, -7, 18)},
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(8, -7, -12), new Vec3(8, -7, 18), new Vec3(-2, -7, 18)},
          new List<Vec3>{new Vec3(-2, 13, -12), new Vec3(-2, 13, 18), new Vec3(8, 13, 18), new Vec3(8, 13, -12)},
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(-2, 13, -12), new Vec3(8, 13, -12), new Vec3(8, -7, -12)},
          new List<Vec3>{new Vec3(-2, -7, 18), new Vec3(8, -7, 18), new Vec3(8, 13, 18), new Vec3(-2, 13, 18)}
        };
        var geometry = new Geom3(points);

        // center about X
        var centered = Center(geometry, axisX: true, axisY: false, axisZ: false);
        Assert.DoesNotThrow(() => centered.Validate());
        var pts = centered.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-5, -7, -12), new Vec3(-5, -7, 18), new Vec3(-5, 13, 18), new Vec3(-5, 13, -12)},
          new List<Vec3>{new Vec3(5, -7, -12), new Vec3(5, 13, -12), new Vec3(5, 13, 18), new Vec3(5, -7, 18)},
          new List<Vec3>{new Vec3(-5, -7, -12), new Vec3(5, -7, -12), new Vec3(5, -7, 18), new Vec3(-5, -7, 18)},
          new List<Vec3>{new Vec3(-5, 13, -12), new Vec3(-5, 13, 18), new Vec3(5, 13, 18), new Vec3(5, 13, -12)},
          new List<Vec3>{new Vec3(-5, -7, -12), new Vec3(-5, 13, -12), new Vec3(5, 13, -12), new Vec3(5, -7, -12)},
          new List<Vec3>{new Vec3(-5, -7, 18), new Vec3(5, -7, 18), new Vec3(5, 13, 18), new Vec3(-5, 13, 18)}
        };
        Assert.True(Helpers.CompareListOfListsNEVec3(pts, exp));

        centered = CenterX(geometry);
        Assert.DoesNotThrow(() => centered.Validate());
        pts = centered.ToPoints();
        Assert.True(Helpers.CompareListOfListsNEVec3(pts, exp));

        // center about Y
        centered = Center(geometry, axisX: false, axisY: true, axisZ: false);
        Assert.DoesNotThrow(() => centered.Validate());
        pts = centered.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, -10, -12), new Vec3(-2, -10, 18), new Vec3(-2, 10, 18), new Vec3(-2, 10, -12)},
          new List<Vec3>{new Vec3(8, -10, -12), new Vec3(8, 10, -12), new Vec3(8, 10, 18), new Vec3(8, -10, 18)},
          new List<Vec3>{new Vec3(-2, -10, -12), new Vec3(8, -10, -12), new Vec3(8, -10, 18), new Vec3(-2, -10, 18)},
          new List<Vec3>{new Vec3(-2, 10, -12), new Vec3(-2, 10, 18), new Vec3(8, 10, 18), new Vec3(8, 10, -12)},
          new List<Vec3>{new Vec3(-2, -10, -12), new Vec3(-2, 10, -12), new Vec3(8, 10, -12), new Vec3(8, -10, -12)},
          new List<Vec3>{new Vec3(-2, -10, 18), new Vec3(8, -10, 18), new Vec3(8, 10, 18), new Vec3(-2, 10, 18)}
        };
        Assert.True(Helpers.CompareListOfListsNEVec3(pts, exp));

        centered = CenterY(geometry);
        Assert.DoesNotThrow(() => centered.Validate());
        pts = centered.ToPoints();
        Assert.True(Helpers.CompareListOfListsNEVec3(pts, exp));

        // center about Z
        centered = Center(geometry, axisX: false, axisY: false, axisZ: true);
        Assert.DoesNotThrow(() => centered.Validate());
        pts = centered.ToPoints();
        exp = new List<List<Vec3>> {
            new List<Vec3>{new Vec3(-2, -7, -15), new Vec3(-2, -7, 15), new Vec3(-2, 13, 15), new Vec3(-2, 13, -15)},
            new List<Vec3>{new Vec3(8, -7, -15), new Vec3(8, 13, -15), new Vec3(8, 13, 15), new Vec3(8, -7, 15)},
            new List<Vec3>{new Vec3(-2, -7, -15), new Vec3(8, -7, -15), new Vec3(8, -7, 15), new Vec3(-2, -7, 15)},
            new List<Vec3>{new Vec3(-2, 13, -15), new Vec3(-2, 13, 15), new Vec3(8, 13, 15), new Vec3(8, 13, -15)},
            new List<Vec3>{new Vec3(-2, -7, -15), new Vec3(-2, 13, -15), new Vec3(8, 13, -15), new Vec3(8, -7, -15)},
            new List<Vec3>{new Vec3(-2, -7, 15), new Vec3(8, -7, 15), new Vec3(8, 13, 15), new Vec3(-2, 13, 15)}
        };
        Assert.True(Helpers.CompareListOfListsNEVec3(pts, exp));

        centered = CenterZ(geometry);
        Assert.DoesNotThrow(() => centered.Validate());
        pts = centered.ToPoints();
        Assert.True(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestMisc()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(-5, -5), new Vec2(0, 5), new Vec2(10, -5) });

        var centered2 = Center(geometry2, axisX: true, axisY: true, relativeTo: new Vec2(10, 15));
        Assert.DoesNotThrow(() => centered2.Validate());

        var pts2 = new List<Vec2>();
        pts2.AddRange(centered2.ToPoints());
        var exp2 = new List<Vec2> { new Vec2(2.5, 10), new Vec2(7.5, 20), new Vec2(17.5, 10) };
        Assert.True(Helpers.CompareListsNEVec2(pts2, exp2));
    }
}