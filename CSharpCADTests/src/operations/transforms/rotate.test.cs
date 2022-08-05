namespace CSharpCADTests;

[TestFixture]
public class RotateTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRotateGeom2()
    {
        var geometry = new Geom2(new List<Vec2> { new Vec2(0, 0), new Vec2(1, 0), new Vec2(0, 1) });

        // rotate about Z
        var rotated = Rotate(new Vec3(0, 0, -90), geometry);
        Assert.DoesNotThrow(() => rotated.Validate());
        var obs = rotated.ToPoints();
        var exp = new Vec2[] { new Vec2(0, 0), new Vec2(0, -1), new Vec2(1, 0) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        rotated = RotateZ(-90, geometry);
        Assert.DoesNotThrow(() => rotated.Validate());
        obs = rotated.ToPoints();
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }

    [Test]
    public void TestRotateGeom3()
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

        // rotate about X
        var rotated = Rotate(new Vec3(90, 0, 0), geometry);
        Assert.DoesNotThrow(() => rotated.Validate());
        var obs = rotated.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-2, 12, -7.000000000000001), new Vec3(-2, -18, -6.999999999999999),
            new Vec3(-2, -18, 13.000000000000002), new Vec3(-2, 12, 13)},
          new List<Vec3>{new Vec3(8, 12, -7.000000000000001), new Vec3(8, 12, 13),
            new Vec3(8, -18, 13.000000000000002), new Vec3(8, -18, -6.999999999999999)},
          new List<Vec3>{new Vec3(-2, 12, -7.000000000000001), new Vec3(8, 12, -7.000000000000001),
            new Vec3(8, -18, -6.999999999999999), new Vec3(-2, -18, -6.999999999999999)},
          new List<Vec3>{new Vec3(-2, 12, 13), new Vec3(-2, -18, 13.000000000000002),
            new Vec3(8, -18, 13.000000000000002), new Vec3(8, 12, 13)},
          new List<Vec3>{new Vec3(-2, 12, -7.000000000000001), new Vec3(-2, 12, 13),
            new Vec3(8, 12, 13), new Vec3(8, 12, -7.000000000000001)},
          new List<Vec3>{new Vec3(-2, -18, -6.999999999999999), new Vec3(8, -18, -6.999999999999999),
            new Vec3(8, -18, 13.000000000000002), new Vec3(-2, -18, 13.000000000000002)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        rotated = RotateX(90, geometry);
        Assert.DoesNotThrow(() => rotated.Validate());
        obs = rotated.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // rotate about Y
        rotated = Rotate(new Vec3(0, -90, 0), geometry);
        Assert.DoesNotThrow(() => rotated.Validate());
        obs = rotated.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(12, -7, -2.000000000000001), new Vec3(-18, -7, -1.999999999999999),
            new Vec3(-18, 13, -1.999999999999999), new Vec3(12, 13, -2.000000000000001)},
          new List<Vec3>{new Vec3(12, -7, 7.999999999999999), new Vec3(12, 13, 7.999999999999999),
            new Vec3(-18, 13, 8.000000000000002), new Vec3(-18, -7, 8.000000000000002)},
          new List<Vec3>{new Vec3(12, -7, -2.000000000000001), new Vec3(12, -7, 7.999999999999999),
            new Vec3(-18, -7, 8.000000000000002), new Vec3(-18, -7, -1.999999999999999)},
          new List<Vec3>{new Vec3(12, 13, -2.000000000000001), new Vec3(-18, 13, -1.999999999999999),
            new Vec3(-18, 13, 8.000000000000002), new Vec3(12, 13, 7.999999999999999)},
          new List<Vec3>{new Vec3(12, -7, -2.000000000000001), new Vec3(12, 13, -2.000000000000001),
            new Vec3(12, 13, 7.999999999999999), new Vec3(12, -7, 7.999999999999999)},
          new List<Vec3>{new Vec3(-18, -7, -1.999999999999999), new Vec3(-18, -7, 8.000000000000002),
            new Vec3(-18, 13, 8.000000000000002), new Vec3(-18, 13, -1.999999999999999)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        rotated = RotateY(-90, geometry);
        Assert.DoesNotThrow(() => rotated.Validate());
        obs = rotated.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // rotate about Z
        rotated = Rotate(new Vec3(0, 0, 180), geometry);
        Assert.DoesNotThrow(() => rotated.Validate());
        obs = rotated.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(2.000000000000001, 7, -12), new Vec3(2.000000000000001, 7, 18),
            new Vec3(1.9999999999999984, -13, 18), new Vec3(1.9999999999999984, -13, -12)},
          new List<Vec3>{new Vec3(-7.999999999999999, 7.000000000000001, -12), new Vec3(-8.000000000000002, -12.999999999999998, -12),
            new Vec3(-8.000000000000002, -12.999999999999998, 18), new Vec3(-7.999999999999999, 7.000000000000001, 18)},
          new List<Vec3>{new Vec3(2.000000000000001, 7, -12), new Vec3(-7.999999999999999, 7.000000000000001, -12),
            new Vec3(-7.999999999999999, 7.000000000000001, 18), new Vec3(2.000000000000001, 7, 18)},
          new List<Vec3>{new Vec3(1.9999999999999984, -13, -12), new Vec3(1.9999999999999984, -13, 18),
            new Vec3(-8.000000000000002, -12.999999999999998, 18), new Vec3(-8.000000000000002, -12.999999999999998, -12)},
          new List<Vec3>{new Vec3(2.000000000000001, 7, -12), new Vec3(1.9999999999999984, -13, -12),
            new Vec3(-8.000000000000002, -12.999999999999998, -12), new Vec3(-7.999999999999999, 7.000000000000001, -12)},
          new List<Vec3>{new Vec3(2.000000000000001, 7, 18), new Vec3(-7.999999999999999, 7.000000000000001, 18),
            new Vec3(-8.000000000000002, -12.999999999999998, 18), new Vec3(1.9999999999999984, -13, 18)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        rotated = RotateZ(180, geometry);
        Assert.DoesNotThrow(() => rotated.Validate());
        obs = rotated.ToPoints();
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));
    }

    [Test]
    public void TestRotateMisc()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, -5), new Vec2(0, 5), new Vec2(-5, -5) });

        var rotated = Rotate(new Vec3(0, 0, 90), geometry2);
        Assert.DoesNotThrow(() => rotated.Validate());

        var obs2 = rotated.ToPoints();
        var exp2 = new Vec2[] { new Vec2(5.000000000000001, 10), new Vec2(-5, 3.061616997868383e-16), new Vec2(5, -5) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs2, exp2));
    }

}