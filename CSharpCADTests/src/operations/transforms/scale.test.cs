using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class TestScale
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestScaleGeom2()
    {
        var geometry = new Geom2(new List<Vec2> { new Vec2(-1, 0), new Vec2(1, 0), new Vec2(0, 1) });

        // scale X
        var scaled = Scale(new Vec2(3, 1), geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        var obs = scaled.ToPoints();
        var exp = new Vec2[] { new Vec2(-3, 0), new Vec2(3, 0), new Vec2(0, 1) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        scaled = (Geom2)ScaleX(3, geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        obs = scaled.ToPoints();
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // scale Y
        scaled = Scale(new Vec2(1, 3), geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        obs = scaled.ToPoints();
        exp = new Vec2[] { new Vec2(-1, 0), new Vec2(1, 0), new Vec2(0, 3) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        scaled = (Geom2)ScaleY(3, geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        obs = scaled.ToPoints();
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }


    [Test]
    public void TestScaleGeom3()
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

        // scale X
        var scaled = (Geom3)Scale(new Vec3(3, 1, 1), geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        var obs = scaled.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-6, -7, -12), new Vec3(-6, -7, 18), new Vec3(-6, 13, 18), new Vec3(-6, 13, -12)},
          new List<Vec3>{new Vec3(24, -7, -12), new Vec3(24, 13, -12), new Vec3(24, 13, 18), new Vec3(24, -7, 18)},
          new List<Vec3>{new Vec3(-6, -7, -12), new Vec3(24, -7, -12), new Vec3(24, -7, 18), new Vec3(-6, -7, 18)},
          new List<Vec3>{new Vec3(-6, 13, -12), new Vec3(-6, 13, 18), new Vec3(24, 13, 18), new Vec3(24, 13, -12)},
          new List<Vec3>{new Vec3(-6, -7, -12), new Vec3(-6, 13, -12), new Vec3(24, 13, -12), new Vec3(24, -7, -12)},
          new List<Vec3>{new Vec3(-6, -7, 18), new Vec3(24, -7, 18), new Vec3(24, 13, 18), new Vec3(-6, 13, 18)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        scaled = (Geom3)ScaleX(3, geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        obs = scaled.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // scale Y
        scaled = (Geom3)Scale(new Vec3(1, 0.5, 1), geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        obs = scaled.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, -3.5, -12), new Vec3(-2, -3.5, 18), new Vec3(-2, 6.5, 18), new Vec3(-2, 6.5, -12)},
          new List<Vec3>{new Vec3(8, -3.5, -12), new Vec3(8, 6.5, -12), new Vec3(8, 6.5, 18), new Vec3(8, -3.5, 18)},
          new List<Vec3>{new Vec3(-2, -3.5, -12), new Vec3(8, -3.5, -12), new Vec3(8, -3.5, 18), new Vec3(-2, -3.5, 18)},
          new List<Vec3>{new Vec3(-2, 6.5, -12), new Vec3(-2, 6.5, 18), new Vec3(8, 6.5, 18), new Vec3(8, 6.5, -12)},
          new List<Vec3>{new Vec3(-2, -3.5, -12), new Vec3(-2, 6.5, -12), new Vec3(8, 6.5, -12), new Vec3(8, -3.5, -12)},
          new List<Vec3>{new Vec3(-2, -3.5, 18), new Vec3(8, -3.5, 18), new Vec3(8, 6.5, 18), new Vec3(-2, 6.5, 18)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        scaled = (Geom3)ScaleY(0.5, geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        obs = scaled.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // scale Z
        scaled = (Geom3)Scale(new Vec3(1, 1, 5), geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        obs = scaled.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, -7, -60), new Vec3(-2, -7, 90), new Vec3(-2, 13, 90), new Vec3(-2, 13, -60)},
          new List<Vec3>{new Vec3(8, -7, -60), new Vec3(8, 13, -60), new Vec3(8, 13, 90), new Vec3(8, -7, 90)},
          new List<Vec3>{new Vec3(-2, -7, -60), new Vec3(8, -7, -60), new Vec3(8, -7, 90), new Vec3(-2, -7, 90)},
          new List<Vec3>{new Vec3(-2, 13, -60), new Vec3(-2, 13, 90), new Vec3(8, 13, 90), new Vec3(8, 13, -60)},
          new List<Vec3>{new Vec3(-2, -7, -60), new Vec3(-2, 13, -60), new Vec3(8, 13, -60), new Vec3(8, -7, -60)},
          new List<Vec3>{new Vec3(-2, -7, 90), new Vec3(8, -7, 90), new Vec3(8, 13, 90), new Vec3(-2, 13, 90)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        scaled = (Geom3)ScaleZ(5, geometry);
        Assert.DoesNotThrow(() => scaled.Validate());
        obs = scaled.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));
    }

    [Test]
    public void TestScaleMisc()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(-5, -5), new Vec2(0, 5), new Vec2(10, -5) });

        var scaled = Scale(new Vec2(3, 1), geometry2);
        Assert.DoesNotThrow(() => scaled.Validate());

        var obs2 = scaled.ToPoints();
        var exp2 = new Vec2[] { new Vec2(-15, -5), new Vec2(0, 5), new Vec2(30, -5) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs2, exp2));
    }
}