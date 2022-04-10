using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class CuboidTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCuboidDefaults()
    {
        var obs = Cuboid(center: (0,0,0)); // CSCAD changed default center.
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(-1.0000000, -1.0000000, -1.0000000),
            new Vec3(-1.0000000, -1.0000000, 1.0000000),
            new Vec3(-1.0000000, 1.0000000, 1.0000000),
            new Vec3(-1.0000000, 1.0000000, -1.0000000)},
          new List<Vec3> {new Vec3(1.0000000, -1.0000000, -1.0000000),
            new Vec3(1.0000000, 1.0000000, -1.0000000),
            new Vec3(1.0000000, 1.0000000, 1.0000000),
            new Vec3(1.0000000, -1.0000000, 1.0000000)},
          new List<Vec3> {new Vec3(-1.0000000, -1.0000000, -1.0000000),
            new Vec3(1.0000000, -1.0000000, -1.0000000),
            new Vec3(1.0000000, -1.0000000, 1.0000000),
            new Vec3(-1.0000000, -1.0000000, 1.0000000)},
          new List<Vec3> {new Vec3(-1.0000000, 1.0000000, -1.0000000),
            new Vec3(-1.0000000, 1.0000000, 1.0000000),
            new Vec3(1.0000000, 1.0000000, 1.0000000),
            new Vec3(1.0000000, 1.0000000, -1.0000000)},
          new List<Vec3> {new Vec3(-1.0000000, -1.0000000, -1.0000000),
            new Vec3(-1.0000000, 1.0000000, -1.0000000),
            new Vec3(1.0000000, 1.0000000, -1.0000000),
            new Vec3(1.0000000, -1.0000000, -1.0000000)},
          new List<Vec3> {new Vec3(-1.0000000, -1.0000000, 1.0000000),
            new Vec3(1.0000000, -1.0000000, 1.0000000),
            new Vec3(1.0000000, 1.0000000, 1.0000000),
            new Vec3(-1.0000000, 1.0000000, 1.0000000)}
        };
        Assert.AreEqual(pts.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestCuboidOptions()
    {
        // test center
        var obs = Cuboid(size: (6, 6, 6), center: (3, 5, 7));
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(0, 2, 4), new Vec3(0, 2, 10), new Vec3(0, 8, 10), new Vec3(0, 8, 4)},
          new List<Vec3> {new Vec3(6, 2, 4), new Vec3(6, 8, 4), new Vec3(6, 8, 10), new Vec3(6, 2, 10)},
          new List<Vec3> {new Vec3(0, 2, 4), new Vec3(6, 2, 4), new Vec3(6, 2, 10), new Vec3(0, 2, 10)},
          new List<Vec3> {new Vec3(0, 8, 4), new Vec3(0, 8, 10), new Vec3(6, 8, 10), new Vec3(6, 8, 4)},
          new List<Vec3> {new Vec3(0, 2, 4), new Vec3(0, 8, 4), new Vec3(6, 8, 4), new Vec3(6, 2, 4)},
          new List<Vec3> {new Vec3(0, 2, 10), new Vec3(6, 2, 10), new Vec3(6, 8, 10), new Vec3(0, 8, 10)}
        };

        Assert.AreEqual(pts.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test size
        obs = Cuboid(size: (4.5, 1.5, 7), center: (0,0,0));
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(-2.25, -0.75, -3.5), new Vec3(-2.25, -0.75, 3.5), new Vec3(-2.25, 0.75, 3.5), new Vec3(-2.25, 0.75, -3.5)},
          new List<Vec3> {new Vec3(2.25, -0.75, -3.5), new Vec3(2.25, 0.75, -3.5), new Vec3(2.25, 0.75, 3.5), new Vec3(2.25, -0.75, 3.5)},
          new List<Vec3> {new Vec3(-2.25, -0.75, -3.5), new Vec3(2.25, -0.75, -3.5), new Vec3(2.25, -0.75, 3.5), new Vec3(-2.25, -0.75, 3.5)},
          new List<Vec3> {new Vec3(-2.25, 0.75, -3.5), new Vec3(-2.25, 0.75, 3.5), new Vec3(2.25, 0.75, 3.5), new Vec3(2.25, 0.75, -3.5)},
          new List<Vec3> {new Vec3(-2.25, -0.75, -3.5), new Vec3(-2.25, 0.75, -3.5), new Vec3(2.25, 0.75, -3.5), new Vec3(2.25, -0.75, -3.5)},
          new List<Vec3> {new Vec3(-2.25, -0.75, 3.5), new Vec3(2.25, -0.75, 3.5), new Vec3(2.25, 0.75, 3.5), new Vec3(-2.25, 0.75, 3.5)}
        };

        Assert.AreEqual(pts.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
