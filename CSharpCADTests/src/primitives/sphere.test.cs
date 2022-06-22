using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class SphereTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSphereDefaults()
    {
        var obs = Sphere();
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 512);
    }

    [Test]
    public void TestSphereOptions()
    {
        // test radius
        var obs = Sphere(radius: 5, segments: 12);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        var exp = new List<List<Vec3>> { };
        Assert.AreEqual(pts.Count, 72);
        // Assert.IsTrue(comparePolygonsAsPoints(pts, exp))

        // test segments
        obs = Sphere(segments: 8);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(1, 0, 0), new Vec3(0.7071067811865476, -0.7071067811865475, 0),
            new Vec3(0.5000000000000001, -0.5, -0.7071067811865475), new Vec3(0.7071067811865476, 0, -0.7071067811865475)},
          new List<Vec3> {new Vec3(0.7071067811865476, 0, 0.7071067811865475), new Vec3(0.5000000000000001, -0.5, 0.7071067811865475),
            new Vec3(0.7071067811865476, -0.7071067811865475, 0), new Vec3(1, 0, 0)},
          new List<Vec3> {new Vec3(0.7071067811865476, 0, -0.7071067811865475), new Vec3(0.5000000000000001, -0.5, -0.7071067811865475), new Vec3(6.123233995736766e-17, 0, -1)},
          new List<Vec3> {new Vec3(6.123233995736766e-17, 0, 1), new Vec3(0.5000000000000001, -0.5, 0.7071067811865475), new Vec3(0.7071067811865476, 0, 0.7071067811865475)},
          new List<Vec3> {new Vec3(0.7071067811865476, -0.7071067811865475, 0), new Vec3(6.123233995736766e-17, -1, 0),
            new Vec3(4.329780281177467e-17, -0.7071067811865476, -0.7071067811865475), new Vec3(0.5000000000000001, -0.5, -0.7071067811865475)},
          new List<Vec3> {new Vec3(0.5000000000000001, -0.5, 0.7071067811865475), new Vec3(4.329780281177467e-17, -0.7071067811865476, 0.7071067811865475),
            new Vec3(6.123233995736766e-17, -1, 0), new Vec3(0.7071067811865476, -0.7071067811865475, 0)},
          new List<Vec3> {new Vec3(0.5000000000000001, -0.5, -0.7071067811865475), new Vec3(4.329780281177467e-17, -0.7071067811865476, -0.7071067811865475),
            new Vec3(4.329780281177467e-17, -4.329780281177466e-17, -1)},
          new List<Vec3> {new Vec3(4.329780281177467e-17, -4.329780281177466e-17, 1),
            new Vec3(4.329780281177467e-17, -0.7071067811865476, 0.7071067811865475),
            new Vec3(0.5000000000000001, -0.5, 0.7071067811865475)},
          new List<Vec3> {new Vec3(6.123233995736766e-17, -1, 0),
            new Vec3(-0.7071067811865475, -0.7071067811865476, 0),
            new Vec3(-0.5, -0.5000000000000001, -0.7071067811865475),
            new Vec3(4.329780281177467e-17, -0.7071067811865476, -0.7071067811865475)},
          new List<Vec3> {new Vec3(4.329780281177467e-17, -0.7071067811865476, 0.7071067811865475),
            new Vec3(-0.5, -0.5000000000000001, 0.7071067811865475),
            new Vec3(-0.7071067811865475, -0.7071067811865476, 0),
            new Vec3(6.123233995736766e-17, -1, 0)},
          new List<Vec3> {new Vec3(4.329780281177467e-17, -0.7071067811865476, -0.7071067811865475),
            new Vec3(-0.5, -0.5000000000000001, -0.7071067811865475),
            new Vec3(3.749399456654644e-33, -6.123233995736766e-17, -1)},
          new List<Vec3> {new Vec3(3.749399456654644e-33, -6.123233995736766e-17, 1),
            new Vec3(-0.5, -0.5000000000000001, 0.7071067811865475),
            new Vec3(4.329780281177467e-17, -0.7071067811865476, 0.7071067811865475)},
          new List<Vec3> {new Vec3(-0.7071067811865475, -0.7071067811865476, 0),
            new Vec3(-1, -1.2246467991473532e-16, 0),
            new Vec3(-0.7071067811865476, -8.659560562354934e-17, -0.7071067811865475),
            new Vec3(-0.5, -0.5000000000000001, -0.7071067811865475)},
          new List<Vec3> {new Vec3(-0.5, -0.5000000000000001, 0.7071067811865475),
            new Vec3(-0.7071067811865476, -8.659560562354934e-17, 0.7071067811865475),
            new Vec3(-1, -1.2246467991473532e-16, 0),
            new Vec3(-0.7071067811865475, -0.7071067811865476, 0)},
          new List<Vec3> {new Vec3(-0.5, -0.5000000000000001, -0.7071067811865475),
            new Vec3(-0.7071067811865476, -8.659560562354934e-17, -0.7071067811865475),
            new Vec3(-4.329780281177466e-17, -4.329780281177467e-17, -1)},
          new List<Vec3> {new Vec3(-4.329780281177466e-17, -4.329780281177467e-17, 1),
            new Vec3(-0.7071067811865476, -8.659560562354934e-17, 0.7071067811865475),
            new Vec3(-0.5, -0.5000000000000001, 0.7071067811865475)},
          new List<Vec3> {new Vec3(-1, -1.2246467991473532e-16, 0),
            new Vec3(-0.7071067811865477, 0.7071067811865475, 0),
            new Vec3(-0.5000000000000001, 0.5, -0.7071067811865475),
            new Vec3(-0.7071067811865476, -8.659560562354934e-17, -0.7071067811865475)},
          new List<Vec3> {new Vec3(-0.7071067811865476, -8.659560562354934e-17, 0.7071067811865475),
            new Vec3(-0.5000000000000001, 0.5, 0.7071067811865475),
            new Vec3(-0.7071067811865477, 0.7071067811865475, 0),
            new Vec3(-1, -1.2246467991473532e-16, 0)},
          new List<Vec3> {new Vec3(-0.7071067811865476, -8.659560562354934e-17, -0.7071067811865475),
            new Vec3(-0.5000000000000001, 0.5, -0.7071067811865475),
            new Vec3(-6.123233995736766e-17, -7.498798913309288e-33, -1)},
          new List<Vec3> {new Vec3(-6.123233995736766e-17, -7.498798913309288e-33, 1),
            new Vec3(-0.5000000000000001, 0.5, 0.7071067811865475),
            new Vec3(-0.7071067811865476, -8.659560562354934e-17, 0.7071067811865475)},
          new List<Vec3> {new Vec3(-0.7071067811865477, 0.7071067811865475, 0),
            new Vec3(-1.8369701987210297e-16, 1, 0),
            new Vec3(-1.29893408435324e-16, 0.7071067811865476, -0.7071067811865475),
            new Vec3(-0.5000000000000001, 0.5, -0.7071067811865475)},
          new List<Vec3> {new Vec3(-0.5000000000000001, 0.5, 0.7071067811865475),
            new Vec3(-1.29893408435324e-16, 0.7071067811865476, 0.7071067811865475),
            new Vec3(-1.8369701987210297e-16, 1, 0),
            new Vec3(-0.7071067811865477, 0.7071067811865475, 0)},
          new List<Vec3> {new Vec3(-0.5000000000000001, 0.5, -0.7071067811865475),
            new Vec3(-1.29893408435324e-16, 0.7071067811865476, -0.7071067811865475),
            new Vec3(-4.3297802811774677e-17, 4.329780281177466e-17, -1)},
          new List<Vec3> {new Vec3(-4.3297802811774677e-17, 4.329780281177466e-17, 1),
            new Vec3(-1.29893408435324e-16, 0.7071067811865476, 0.7071067811865475),
            new Vec3(-0.5000000000000001, 0.5, 0.7071067811865475)},
          new List<Vec3> {new Vec3(-1.8369701987210297e-16, 1, 0),
            new Vec3(0.7071067811865474, 0.7071067811865477, 0),
            new Vec3(0.4999999999999999, 0.5000000000000001, -0.7071067811865475),
            new Vec3(-1.29893408435324e-16, 0.7071067811865476, -0.7071067811865475)},
          new List<Vec3> {new Vec3(-1.29893408435324e-16, 0.7071067811865476, 0.7071067811865475),
            new Vec3(0.4999999999999999, 0.5000000000000001, 0.7071067811865475),
            new Vec3(0.7071067811865474, 0.7071067811865477, 0),
            new Vec3(-1.8369701987210297e-16, 1, 0)},
          new List<Vec3> {new Vec3(-1.29893408435324e-16, 0.7071067811865476, -0.7071067811865475),
            new Vec3(0.4999999999999999, 0.5000000000000001, -0.7071067811865475),
            new Vec3(-1.1248198369963932e-32, 6.123233995736766e-17, -1)},
          new List<Vec3> {new Vec3(-1.1248198369963932e-32, 6.123233995736766e-17, 1),
            new Vec3(0.4999999999999999, 0.5000000000000001, 0.7071067811865475),
            new Vec3(-1.29893408435324e-16, 0.7071067811865476, 0.7071067811865475)},
          new List<Vec3> {new Vec3(0.7071067811865474, 0.7071067811865477, 0),
            new Vec3(1, 2.4492935982947064e-16, 0),
            new Vec3(0.7071067811865476, 1.7319121124709868e-16, -0.7071067811865475),
            new Vec3(0.4999999999999999, 0.5000000000000001, -0.7071067811865475)},
          new List<Vec3> {new Vec3(0.4999999999999999, 0.5000000000000001, 0.7071067811865475),
            new Vec3(0.7071067811865476, 1.7319121124709868e-16, 0.7071067811865475),
            new Vec3(1, 2.4492935982947064e-16, 0),
            new Vec3(0.7071067811865474, 0.7071067811865477, 0)},
          new List<Vec3> {new Vec3(0.4999999999999999, 0.5000000000000001, -0.7071067811865475),
            new Vec3(0.7071067811865476, 1.7319121124709868e-16, -0.7071067811865475),
            new Vec3(4.329780281177465e-17, 4.3297802811774677e-17, -1)},
          new List<Vec3> {new Vec3(4.329780281177465e-17, 4.3297802811774677e-17, 1),
            new Vec3(0.7071067811865476, 1.7319121124709868e-16, 0.7071067811865475),
            new Vec3(0.4999999999999999, 0.5000000000000001, 0.7071067811865475)}
        };
        Assert.AreEqual(pts.Count, 32);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test center
        obs = Sphere(center: (-3, 5, 7), segments: 8);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(-2, 5, 7), new Vec3(-2.2928932188134525, 4.292893218813452, 7),
            new Vec3(-2.5, 4.5, 6.292893218813452), new Vec3(-2.2928932188134525, 5, 6.292893218813452)},
          new List<Vec3> {new Vec3(-2.2928932188134525, 5, 7.707106781186548), new Vec3(-2.5, 4.5, 7.707106781186548),
            new Vec3(-2.2928932188134525, 4.292893218813452, 7), new Vec3(-2, 5, 7)},
          new List<Vec3> {new Vec3(-2.2928932188134525, 5, 6.292893218813452), new Vec3(-2.5, 4.5, 6.292893218813452), new Vec3(-3, 5, 6)},
          new List<Vec3> {new Vec3(-3, 5, 8), new Vec3(-2.5, 4.5, 7.707106781186548), new Vec3(-2.2928932188134525, 5, 7.707106781186548)},
          new List<Vec3> {new Vec3(-2.2928932188134525, 4.292893218813452, 7), new Vec3(-3, 4, 7),
            new Vec3(-3, 4.292893218813452, 6.292893218813452), new Vec3(-2.5, 4.5, 6.292893218813452)},
          new List<Vec3> {new Vec3(-2.5, 4.5, 7.707106781186548), new Vec3(-3, 4.292893218813452, 7.707106781186548),
            new Vec3(-3, 4, 7), new Vec3(-2.2928932188134525, 4.292893218813452, 7)},
          new List<Vec3> {new Vec3(-2.5, 4.5, 6.292893218813452), new Vec3(-3, 4.292893218813452, 6.292893218813452), new Vec3(-3, 5, 6)},
          new List<Vec3> {new Vec3(-3, 5, 8), new Vec3(-3, 4.292893218813452, 7.707106781186548), new Vec3(-2.5, 4.5, 7.707106781186548)},
          new List<Vec3> {new Vec3(-3, 4, 7), new Vec3(-3.7071067811865475, 4.292893218813452, 7),
            new Vec3(-3.5, 4.5, 6.292893218813452), new Vec3(-3, 4.292893218813452, 6.292893218813452)},
          new List<Vec3> {new Vec3(-3, 4.292893218813452, 7.707106781186548), new Vec3(-3.5, 4.5, 7.707106781186548),
            new Vec3(-3.7071067811865475, 4.292893218813452, 7), new Vec3(-3, 4, 7)},
          new List<Vec3> {new Vec3(-3, 4.292893218813452, 6.292893218813452), new Vec3(-3.5, 4.5, 6.292893218813452), new Vec3(-3, 5, 6)},
          new List<Vec3> {new Vec3(-3, 5, 8), new Vec3(-3.5, 4.5, 7.707106781186548), new Vec3(-3, 4.292893218813452, 7.707106781186548)},
          new List<Vec3> {new Vec3(-3.7071067811865475, 4.292893218813452, 7), new Vec3(-4, 5, 7),
            new Vec3(-3.7071067811865475, 5, 6.292893218813452), new Vec3(-3.5, 4.5, 6.292893218813452)},
          new List<Vec3> {new Vec3(-3.5, 4.5, 7.707106781186548), new Vec3(-3.7071067811865475, 5, 7.707106781186548),
            new Vec3(-4, 5, 7), new Vec3(-3.7071067811865475, 4.292893218813452, 7)},
          new List<Vec3> {new Vec3(-3.5, 4.5, 6.292893218813452), new Vec3(-3.7071067811865475, 5, 6.292893218813452), new Vec3(-3, 5, 6)},
          new List<Vec3> {new Vec3(-3, 5, 8), new Vec3(-3.7071067811865475, 5, 7.707106781186548), new Vec3(-3.5, 4.5, 7.707106781186548)},
          new List<Vec3> {new Vec3(-4, 5, 7), new Vec3(-3.707106781186548, 5.707106781186548, 7),
            new Vec3(-3.5, 5.5, 6.292893218813452), new Vec3(-3.7071067811865475, 5, 6.292893218813452)},
          new List<Vec3> {new Vec3(-3.7071067811865475, 5, 7.707106781186548), new Vec3(-3.5, 5.5, 7.707106781186548),
            new Vec3(-3.707106781186548, 5.707106781186548, 7), new Vec3(-4, 5, 7)},
          new List<Vec3> {new Vec3(-3.7071067811865475, 5, 6.292893218813452), new Vec3(-3.5, 5.5, 6.292893218813452), new Vec3(-3, 5, 6)},
          new List<Vec3> {new Vec3(-3, 5, 8), new Vec3(-3.5, 5.5, 7.707106781186548), new Vec3(-3.7071067811865475, 5, 7.707106781186548)},
          new List<Vec3> {new Vec3(-3.707106781186548, 5.707106781186548, 7), new Vec3(-3, 6, 7),
            new Vec3(-3, 5.707106781186548, 6.292893218813452), new Vec3(-3.5, 5.5, 6.292893218813452)},
          new List<Vec3> {new Vec3(-3.5, 5.5, 7.707106781186548), new Vec3(-3, 5.707106781186548, 7.707106781186548),
            new Vec3(-3, 6, 7), new Vec3(-3.707106781186548, 5.707106781186548, 7)},
          new List<Vec3> {new Vec3(-3.5, 5.5, 6.292893218813452), new Vec3(-3, 5.707106781186548, 6.292893218813452), new Vec3(-3, 5, 6)},
          new List<Vec3> {new Vec3(-3, 5, 8), new Vec3(-3, 5.707106781186548, 7.707106781186548), new Vec3(-3.5, 5.5, 7.707106781186548)},
          new List<Vec3> {new Vec3(-3, 6, 7), new Vec3(-2.2928932188134525, 5.707106781186548, 7),
            new Vec3(-2.5, 5.5, 6.292893218813452), new Vec3(-3, 5.707106781186548, 6.292893218813452)},
          new List<Vec3> {new Vec3(-3, 5.707106781186548, 7.707106781186548), new Vec3(-2.5, 5.5, 7.707106781186548),
            new Vec3(-2.2928932188134525, 5.707106781186548, 7), new Vec3(-3, 6, 7)},
          new List<Vec3> {new Vec3(-3, 5.707106781186548, 6.292893218813452), new Vec3(-2.5, 5.5, 6.292893218813452), new Vec3(-3, 5, 6)},
          new List<Vec3> {new Vec3(-3, 5, 8), new Vec3(-2.5, 5.5, 7.707106781186548), new Vec3(-3, 5.707106781186548, 7.707106781186548)},
          new List<Vec3> {new Vec3(-2.2928932188134525, 5.707106781186548, 7), new Vec3(-2, 5, 7),
            new Vec3(-2.2928932188134525, 5, 6.292893218813452), new Vec3(-2.5, 5.5, 6.292893218813452)},
          new List<Vec3> {new Vec3(-2.5, 5.5, 7.707106781186548), new Vec3(-2.2928932188134525, 5, 7.707106781186548),
            new Vec3(-2, 5, 7), new Vec3(-2.2928932188134525, 5.707106781186548, 7)},
          new List<Vec3> {new Vec3(-2.5, 5.5, 6.292893218813452), new Vec3(-2.2928932188134525, 5, 6.292893218813452), new Vec3(-3, 5, 6)},
          new List<Vec3> {new Vec3(-3, 5, 8), new Vec3(-2.2928932188134525, 5, 7.707106781186548), new Vec3(-2.5, 5.5, 7.707106781186548)}
        };
        Assert.AreEqual(pts.Count, 32);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}