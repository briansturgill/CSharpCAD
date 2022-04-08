namespace CSharpCADTests;

[TestFixture]
public class MirroringTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestMirroringGeom2()
    {
        var geometry = new Geom2(new List<Vec2> { new Vec2(-5, -5), new Vec2(0, 5), new Vec2(10, -5) });

        // mirror about X
        Geom2 mirrored = (Geom2)Mirror(geometry, normal: new Vec3(1, 0, 0));
        Assert.DoesNotThrow(() => mirrored.Validate());
        var obs = mirrored.ToPoints();
        var exp = new Vec2[] { new Vec2(5, -5), new Vec2(0, 5), new Vec2(-10, -5) };
        Assert.AreEqual(obs, exp);

        mirrored = (Geom2)MirrorX(geometry);
        Assert.DoesNotThrow(() => mirrored.Validate());
        obs = mirrored.ToPoints();
        Assert.AreEqual(obs, exp);

        // mirror about Y
        mirrored = (Geom2)Mirror(geometry, normal: new Vec3(0, 1, 0));
        Assert.DoesNotThrow(() => mirrored.Validate());
        obs = mirrored.ToPoints();
        exp = new Vec2[] { new Vec2(-5, 5), new Vec2(0, -5), new Vec2(10, 5) };
        Assert.AreEqual(obs, exp);

        mirrored = (Geom2)MirrorY(geometry);
        Assert.DoesNotThrow(() => mirrored.Validate());
        obs = mirrored.ToPoints();
        Assert.AreEqual(obs, exp);
    }


    [Test]
    public void TestMirroringGeom3()
    {
        var points = new List<List<Vec3>>{
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(-2, -7, 18), new Vec3(-2, 13, 18), new Vec3(-2, 13, -12)},
          new List<Vec3>{new Vec3(8, -7, -12), new Vec3(8, 13, -12), new Vec3(8, 13, 18), new Vec3(8, -7, 18)},
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(8, -7, -12), new Vec3(8, -7, 18), new Vec3(-2, -7, 18)},
          new List<Vec3>{new Vec3(-2, 13, -12), new Vec3(-2, 13, 18), new Vec3(8, 13, 18), new Vec3(8, 13, -12)},
          new List<Vec3>{new Vec3(-2, -7, -12), new Vec3(-2, 13, -12), new Vec3(8, 13, -12), new Vec3(8, -7, -12)},
          new List<Vec3>{new Vec3(-2, -7, 18), new Vec3(8, -7, 18), new Vec3(8, 13, 18), new Vec3(-2, 13, 18)}
        };
        var geometry = new Geom3(points);

        // mirror about X
        var mirrored = (Geom3)Mirror(geometry, normal: new Vec3(1, 0, 0));
        Assert.DoesNotThrow(() => mirrored.Validate());
        var obs = mirrored.ToPoints();
        var exp = new List<List<Vec3>>{
          new List<Vec3>{new Vec3(2, 13, -12), new Vec3(2, 13, 18), new Vec3(2, -7, 18), new Vec3(2, -7, -12)},
          new List<Vec3>{new Vec3(-8, -7, 18), new Vec3(-8, 13, 18), new Vec3(-8, 13, -12), new Vec3(-8, -7, -12)},
          new List<Vec3>{new Vec3(2, -7, 18), new Vec3(-8, -7, 18), new Vec3(-8, -7, -12), new Vec3(2, -7, -12)},
          new List<Vec3>{new Vec3(-8, 13, -12), new Vec3(-8, 13, 18), new Vec3(2, 13, 18), new Vec3(2, 13, -12)},
          new List<Vec3>{new Vec3(-8, -7, -12), new Vec3(-8, 13, -12), new Vec3(2, 13, -12), new Vec3(2, -7, -12)},
          new List<Vec3>{new Vec3(2, 13, 18), new Vec3(-8, 13, 18), new Vec3(-8, -7, 18), new Vec3(2, -7, 18)}
        };
        Assert.AreEqual(obs, exp);

        mirrored = (Geom3)MirrorX(geometry);
        Assert.DoesNotThrow(() => mirrored.Validate());
        obs = mirrored.ToPoints();
        Assert.AreEqual(obs, exp);

        // mirror about Y
        mirrored = (Geom3)Mirror(geometry, normal: new Vec3(0, 1, 0));
        Assert.DoesNotThrow(() => mirrored.Validate());
        obs = mirrored.ToPoints();
        exp = new List<List<Vec3>>{
          new List<Vec3> { new Vec3(-2, -13, -12), new Vec3(-2, -13, 18), new Vec3(-2, 7, 18), new Vec3(-2, 7, -12) },
          new List<Vec3> { new Vec3(8, 7, 18), new Vec3(8, -13, 18), new Vec3(8, -13, -12), new Vec3(8, 7, -12)},
          new List<Vec3> { new Vec3(-2, 7, 18), new Vec3(8, 7, 18), new Vec3(8, 7, -12), new Vec3(-2, 7, -12)},
          new List<Vec3> { new Vec3(8, -13, -12), new Vec3(8, -13, 18), new Vec3(-2, -13, 18), new Vec3(-2, -13, -12)},
          new List<Vec3> { new Vec3(8, 7, -12), new Vec3(8, -13, -12), new Vec3(-2, -13, -12), new Vec3(-2, 7, -12)},
          new List<Vec3> { new Vec3(-2, -13, 18), new Vec3(8, -13, 18), new Vec3(8, 7, 18), new Vec3(-2, 7, 18)}
        };
        Assert.AreEqual(obs, exp);

        mirrored = (Geom3)MirrorY(geometry);
        Assert.DoesNotThrow(() => mirrored.Validate());
        obs = mirrored.ToPoints();
        Assert.AreEqual(obs, exp);

        // mirror about Z
        mirrored = (Geom3)Mirror(geometry, normal: new Vec3(0, 0, 1));
        Assert.DoesNotThrow(() => mirrored.Validate());
        obs = mirrored.ToPoints();
        exp = new List<List<Vec3>>{
          new List<Vec3> { new Vec3(-2, 13, 12), new Vec3(-2, 13, -18), new Vec3(-2, -7, -18), new Vec3(-2, -7, 12)},
          new List<Vec3> { new Vec3(8, -7, -18), new Vec3(8, 13, -18), new Vec3(8, 13, 12), new Vec3(8, -7, 12)},
          new List<Vec3> { new Vec3(-2, -7, -18), new Vec3(8, -7, -18), new Vec3(8, -7, 12), new Vec3(-2, -7, 12) },
          new List<Vec3> { new Vec3(8, 13, 12), new Vec3(8, 13, -18), new Vec3(-2, 13, -18), new Vec3(-2, 13, 12) },
          new List<Vec3> { new Vec3(8, -7, 12), new Vec3(8, 13, 12), new Vec3(-2, 13, 12), new Vec3(-2, -7, 12) },
          new List<Vec3> { new Vec3(-2, 13, -18), new Vec3(8, 13, -18), new Vec3(8, -7, -18), new Vec3(-2, -7, -18) }
        };
        Assert.AreEqual(obs, exp);

        mirrored = (Geom3)MirrorZ(geometry);
        Assert.DoesNotThrow(() => mirrored.Validate());
        obs = mirrored.ToPoints();
        Assert.AreEqual(obs, exp);
    }
}