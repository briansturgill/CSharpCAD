using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class RoundedCuboidTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRoundedCuboidDefaults()
    {
        var obs = RoundedCuboid(center: (0, 0, 0)); // CSCAD changed the default center.
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 614);
    }


    [Test]
    public void TestRoundedCuboidOptions()
    {
        // test segments
        var obs = RoundedCuboid(segments: 8, center: (0, 0, 0));
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        var exp = new List<List<Vec3>> { };

        Assert.AreEqual(pts.Count, 62);

        // test center
        obs = RoundedCuboid(center: (4, 5, 6), segments: 8);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> { };

        Assert.AreEqual(pts.Count, 62);

        // test size
        obs = RoundedCuboid(size: (8, 10, 12), segments: 8, center: (0, 0, 0));
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(4, 4.8, -5.8), new Vec3(3.9414213562373095, 4.941421356237309, -5.8),
            new Vec3(3.9414213562373095, 4.941421356237309, 5.8), new Vec3(4, 4.8, 5.8)},
          new List<Vec3> {new Vec3(3.9414213562373095, 4.941421356237309, -5.8), new Vec3(3.8, 5, -5.8),
            new Vec3(3.8, 5, 5.8), new Vec3(3.9414213562373095, 4.941421356237309, 5.8)},
          new List<Vec3> {new Vec3(3.8, 5, -5.8), new Vec3(-3.8, 5, -5.8),
            new Vec3(-3.8, 5, 5.8), new Vec3(3.8, 5, 5.8)},
          new List<Vec3> {new Vec3(-3.8, 5, -5.8), new Vec3(-3.9414213562373095, 4.941421356237309, -5.8),
            new Vec3(-3.9414213562373095, 4.941421356237309, 5.8), new Vec3(-3.8, 5, 5.8)},
          new List<Vec3> {new Vec3(-3.9414213562373095, 4.941421356237309, -5.8), new Vec3(-4, 4.8, -5.8),
            new Vec3(-4, 4.8, 5.8), new Vec3(-3.9414213562373095, 4.941421356237309, 5.8)},
          new List<Vec3> {new Vec3(-4, 4.8, -5.8), new Vec3(-4, -4.8, -5.8),
            new Vec3(-4, -4.8, 5.8), new Vec3(-4, 4.8, 5.8)},
          new List<Vec3> {new Vec3(-4, -4.8, -5.8), new Vec3(-3.9414213562373095, -4.941421356237309, -5.8),
            new Vec3(-3.9414213562373095, -4.941421356237309, 5.8), new Vec3(-4, -4.8, 5.8)},
          new List<Vec3> {new Vec3(-3.9414213562373095, -4.941421356237309, -5.8), new Vec3(-3.8, -5, -5.8),
            new Vec3(-3.8, -5, 5.8), new Vec3(-3.9414213562373095, -4.941421356237309, 5.8)},
          new List<Vec3> {new Vec3(-3.8, -5, -5.8), new Vec3(3.8, -5, -5.8),
            new Vec3(3.8, -5, 5.8), new Vec3(-3.8, -5, 5.8)},
          new List<Vec3> {new Vec3(3.8, -5, -5.8), new Vec3(3.9414213562373095, -4.941421356237309, -5.8),
            new Vec3(3.9414213562373095, -4.941421356237309, 5.8), new Vec3(3.8, -5, 5.8)},
          new List<Vec3> {new Vec3(3.9414213562373095, -4.941421356237309, -5.8), new Vec3(4, -4.8, -5.8),
            new Vec3(4, -4.8, 5.8), new Vec3(3.9414213562373095, -4.941421356237309, 5.8)},
          new List<Vec3> {new Vec3(4, -4.8, -5.8), new Vec3(4, 4.8, -5.8),
            new Vec3(4, 4.8, 5.8), new Vec3(4, -4.8, 5.8)},
          new List<Vec3> {new Vec3(4, 4.8, 5.8), new Vec3(3.9414213562373095, 4.941421356237309, 5.8), new Vec3(3.9414213562373095, 4.8, 5.94142135623731)},
          new List<Vec3> {new Vec3(3.9414213562373095, 4.8, 5.94142135623731), new Vec3(3.9414213562373095, 4.941421356237309, 5.8), new Vec3(3.8, 4.941421356237309, 5.94142135623731)},
          new List<Vec3> {new Vec3(3.9414213562373095, 4.941421356237309, 5.8), new Vec3(3.8, 5, 5.8), new Vec3(3.8, 4.941421356237309, 5.94142135623731)},
          new List<Vec3> {new Vec3(-3.8, 5, 5.8), new Vec3(-3.9414213562373095, 4.941421356237309, 5.8), new Vec3(-3.8, 4.941421356237309, 5.94142135623731)},
          new List<Vec3> {new Vec3(-3.8, 4.941421356237309, 5.94142135623731), new Vec3(-3.9414213562373095, 4.941421356237309, 5.8), new Vec3(-3.9414213562373095, 4.8, 5.94142135623731)},
          new List<Vec3> {new Vec3(-3.9414213562373095, 4.941421356237309, 5.8), new Vec3(-4, 4.8, 5.8), new Vec3(-3.9414213562373095, 4.8, 5.94142135623731)},
          new List<Vec3> {new Vec3(-4, -4.8, 5.8), new Vec3(-3.9414213562373095, -4.941421356237309, 5.8), new Vec3(-3.9414213562373095, -4.8, 5.94142135623731)},
          new List<Vec3> {new Vec3(-3.9414213562373095, -4.8, 5.94142135623731), new Vec3(-3.9414213562373095, -4.941421356237309, 5.8), new Vec3(-3.8, -4.941421356237309, 5.94142135623731)},
          new List<Vec3> {new Vec3(-3.9414213562373095, -4.941421356237309, 5.8), new Vec3(-3.8, -5, 5.8), new Vec3(-3.8, -4.941421356237309, 5.94142135623731)},
          new List<Vec3> {new Vec3(3.8, -5, 5.8), new Vec3(3.9414213562373095, -4.941421356237309, 5.8), new Vec3(3.8, -4.941421356237309, 5.94142135623731)},
          new List<Vec3> {new Vec3(3.8, -4.941421356237309, 5.94142135623731), new Vec3(3.9414213562373095, -4.941421356237309, 5.8), new Vec3(3.9414213562373095, -4.8, 5.94142135623731)},
          new List<Vec3> {new Vec3(3.9414213562373095, -4.941421356237309, 5.8), new Vec3(4, -4.8, 5.8), new Vec3(3.9414213562373095, -4.8, 5.94142135623731)},
          new List<Vec3> {new Vec3(3.8, 5, 5.8), new Vec3(-3.8, 5, 5.8),
            new Vec3(-3.8, 4.941421356237309, 5.94142135623731), new Vec3(3.8, 4.941421356237309, 5.94142135623731)},
          new List<Vec3> {new Vec3(-4, 4.8, 5.8), new Vec3(-4, -4.8, 5.8),
            new Vec3(-3.9414213562373095, -4.8, 5.94142135623731), new Vec3(-3.9414213562373095, 4.8, 5.94142135623731)},
          new List<Vec3> {new Vec3(-3.8, -5, 5.8), new Vec3(3.8, -5, 5.8),
            new Vec3(3.8, -4.941421356237309, 5.94142135623731), new Vec3(-3.8, -4.941421356237309, 5.94142135623731)},
          new List<Vec3> {new Vec3(4, -4.8, 5.8), new Vec3(4, 4.8, 5.8),
            new Vec3(3.9414213562373095, 4.8, 5.94142135623731), new Vec3(3.9414213562373095, -4.8, 5.94142135623731)},
          new List<Vec3> {new Vec3(4, -4.8, -5.8), new Vec3(3.9414213562373095, -4.941421356237309, -5.8), new Vec3(3.9414213562373095, -4.8, -5.94142135623731)},
          new List<Vec3> {new Vec3(3.9414213562373095, -4.8, -5.94142135623731), new Vec3(3.9414213562373095, -4.941421356237309, -5.8), new Vec3(3.8, -4.941421356237309, -5.94142135623731)},
          new List<Vec3> {new Vec3(3.9414213562373095, -4.941421356237309, -5.8), new Vec3(3.8, -5, -5.8), new Vec3(3.8, -4.941421356237309, -5.94142135623731)},
          new List<Vec3> {new Vec3(-3.8, -5, -5.8), new Vec3(-3.9414213562373095, -4.941421356237309, -5.8), new Vec3(-3.8, -4.941421356237309, -5.94142135623731)},
          new List<Vec3> {new Vec3(-3.8, -4.941421356237309, -5.94142135623731), new Vec3(-3.9414213562373095, -4.941421356237309, -5.8), new Vec3(-3.9414213562373095, -4.8, -5.94142135623731)},
          new List<Vec3> {new Vec3(-3.9414213562373095, -4.941421356237309, -5.8), new Vec3(-4, -4.8, -5.8), new Vec3(-3.9414213562373095, -4.8, -5.94142135623731)},
          new List<Vec3> {new Vec3(-4, 4.8, -5.8), new Vec3(-3.9414213562373095, 4.941421356237309, -5.8), new Vec3(-3.9414213562373095, 4.8, -5.94142135623731)},
          new List<Vec3> {new Vec3(-3.9414213562373095, 4.8, -5.94142135623731), new Vec3(-3.9414213562373095, 4.941421356237309, -5.8), new Vec3(-3.8, 4.941421356237309, -5.94142135623731)},
          new List<Vec3> {new Vec3(-3.9414213562373095, 4.941421356237309, -5.8), new Vec3(-3.8, 5, -5.8), new Vec3(-3.8, 4.941421356237309, -5.94142135623731)},
          new List<Vec3> {new Vec3(3.8, 5, -5.8), new Vec3(3.9414213562373095, 4.941421356237309, -5.8), new Vec3(3.8, 4.941421356237309, -5.94142135623731)},
          new List<Vec3> {new Vec3(3.8, 4.941421356237309, -5.94142135623731), new Vec3(3.9414213562373095, 4.941421356237309, -5.8), new Vec3(3.9414213562373095, 4.8, -5.94142135623731)},
          new List<Vec3> {new Vec3(3.9414213562373095, 4.941421356237309, -5.8), new Vec3(4, 4.8, -5.8), new Vec3(3.9414213562373095, 4.8, -5.94142135623731)},
          new List<Vec3> {new Vec3(3.8, -5, -5.8), new Vec3(-3.8, -5, -5.8),
            new Vec3(-3.8, -4.941421356237309, -5.94142135623731), new Vec3(3.8, -4.941421356237309, -5.94142135623731)},
          new List<Vec3> {new Vec3(-4, -4.8, -5.8), new Vec3(-4, 4.8, -5.8),
            new Vec3(-3.9414213562373095, 4.8, -5.94142135623731), new Vec3(-3.9414213562373095, -4.8, -5.94142135623731)},
          new List<Vec3> {new Vec3(-3.8, 5, -5.8), new Vec3(3.8, 5, -5.8),
            new Vec3(3.8, 4.941421356237309, -5.94142135623731), new Vec3(-3.8, 4.941421356237309, -5.94142135623731)},
          new List<Vec3> {new Vec3(4, 4.8, -5.8), new Vec3(4, -4.8, -5.8),
            new Vec3(3.9414213562373095, -4.8, -5.94142135623731), new Vec3(3.9414213562373095, 4.8, -5.94142135623731)},
          new List<Vec3> {new Vec3(3.9414213562373095, 4.8, 5.94142135623731), new Vec3(3.8, 4.941421356237309, 5.94142135623731), new Vec3(3.8, 4.8, 6)},
          new List<Vec3> {new Vec3(-3.8, 4.941421356237309, 5.94142135623731), new Vec3(-3.9414213562373095, 4.8, 5.94142135623731), new Vec3(-3.8, 4.8, 6)},
          new List<Vec3> {new Vec3(-3.9414213562373095, -4.8, 5.94142135623731), new Vec3(-3.8, -4.941421356237309, 5.94142135623731), new Vec3(-3.8, -4.8, 6)},
          new List<Vec3> {new Vec3(3.8, -4.941421356237309, 5.94142135623731), new Vec3(3.9414213562373095, -4.8, 5.94142135623731), new Vec3(3.8, -4.8, 6)},
          new List<Vec3> {new Vec3(3.8, 4.941421356237309, 5.94142135623731), new Vec3(-3.8, 4.941421356237309, 5.94142135623731),
            new Vec3(-3.8, 4.8, 6), new Vec3(3.8, 4.8, 6)},
          new List<Vec3> {new Vec3(-3.9414213562373095, 4.8, 5.94142135623731), new Vec3(-3.9414213562373095, -4.8, 5.94142135623731),
            new Vec3(-3.8, -4.8, 6), new Vec3(-3.8, 4.8, 6)},
          new List<Vec3> {new Vec3(-3.8, -4.941421356237309, 5.94142135623731), new Vec3(3.8, -4.941421356237309, 5.94142135623731),
            new Vec3(3.8, -4.8, 6), new Vec3(-3.8, -4.8, 6)},
          new List<Vec3> {new Vec3(3.9414213562373095, -4.8, 5.94142135623731), new Vec3(3.9414213562373095, 4.8, 5.94142135623731),
            new Vec3(3.8, 4.8, 6), new Vec3(3.8, -4.8, 6)},
          new List<Vec3> {new Vec3(3.9414213562373095, -4.8, -5.94142135623731), new Vec3(3.8, -4.941421356237309, -5.94142135623731), new Vec3(3.8, -4.8, -6)},
          new List<Vec3> {new Vec3(-3.8, -4.941421356237309, -5.94142135623731), new Vec3(-3.9414213562373095, -4.8, -5.94142135623731), new Vec3(-3.8, -4.8, -6)},
          new List<Vec3> {new Vec3(-3.9414213562373095, 4.8, -5.94142135623731), new Vec3(-3.8, 4.941421356237309, -5.94142135623731), new Vec3(-3.8, 4.8, -6)},
          new List<Vec3> {new Vec3(3.8, 4.941421356237309, -5.94142135623731), new Vec3(3.9414213562373095, 4.8, -5.94142135623731), new Vec3(3.8, 4.8, -6)},
          new List<Vec3> {new Vec3(3.8, -4.941421356237309, -5.94142135623731), new Vec3(-3.8, -4.941421356237309, -5.94142135623731),
            new Vec3(-3.8, -4.8, -6), new Vec3(3.8, -4.8, -6)},
          new List<Vec3> {new Vec3(-3.9414213562373095, -4.8, -5.94142135623731), new Vec3(-3.9414213562373095, 4.8, -5.94142135623731),
            new Vec3(-3.8, 4.8, -6), new Vec3(-3.8, -4.8, -6)},
          new List<Vec3> {new Vec3(-3.8, 4.941421356237309, -5.94142135623731), new Vec3(3.8, 4.941421356237309, -5.94142135623731),
            new Vec3(3.8, 4.8, -6), new Vec3(-3.8, 4.8, -6)},
          new List<Vec3> {new Vec3(3.9414213562373095, 4.8, -5.94142135623731), new Vec3(3.9414213562373095, -4.8, -5.94142135623731),
            new Vec3(3.8, -4.8, -6), new Vec3(3.8, 4.8, -6)},
          new List<Vec3> {new Vec3(3.8, 4.8, 6), new Vec3(-3.8, 4.8, 6),
            new Vec3(-3.8, -4.8, 6), new Vec3(3.8, -4.8, 6)},
          new List<Vec3> {new Vec3(3.8, -4.8, -6), new Vec3(-3.8, -4.8, -6),
            new Vec3(-3.8, 4.8, -6), new Vec3(3.8, 4.8, -6)}
        };
        Assert.AreEqual(pts.Count, 62);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test roundRadius
        obs = RoundedCuboid(size: (8, 10, 12), roundRadius: 2, segments: 8, center: (0, 0, 0));
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(4, 3, -4), new Vec3(3.414213562373095, 4.414213562373095, -4),
            new Vec3(3.414213562373095, 4.414213562373095, 4), new Vec3(4, 3, 4)},
          new List<Vec3> {new Vec3(3.414213562373095, 4.414213562373095, -4), new Vec3(2, 5, -4),
            new Vec3(2, 5, 4), new Vec3(3.414213562373095, 4.414213562373095, 4)},
          new List<Vec3> {new Vec3(2, 5, -4), new Vec3(-1.9999999999999998, 5, -4),
            new Vec3(-1.9999999999999998, 5, 4), new Vec3(2, 5, 4)},
          new List<Vec3> {new Vec3(-1.9999999999999998, 5, -4), new Vec3(-3.414213562373095, 4.414213562373095, -4),
            new Vec3(-3.414213562373095, 4.414213562373095, 4), new Vec3(-1.9999999999999998, 5, 4)},
          new List<Vec3> {new Vec3(-3.414213562373095, 4.414213562373095, -4), new Vec3(-4, 3.0000000000000004, -4),
            new Vec3(-4, 3.0000000000000004, 4), new Vec3(-3.414213562373095, 4.414213562373095, 4)},
          new List<Vec3> {new Vec3(-4, 3.0000000000000004, -4), new Vec3(-4, -2.9999999999999996, -4),
            new Vec3(-4, -2.9999999999999996, 4), new Vec3(-4, 3.0000000000000004, 4)},
          new List<Vec3> {new Vec3(-4, -2.9999999999999996, -4), new Vec3(-3.414213562373095, -4.414213562373095, -4),
            new Vec3(-3.414213562373095, -4.414213562373095, 4), new Vec3(-4, -2.9999999999999996, 4)},
          new List<Vec3>{new Vec3(-3.414213562373095, -4.414213562373095, -4), new Vec3(-2.0000000000000004, -5, -4),
            new Vec3(-2.0000000000000004, -5, 4), new Vec3(-3.414213562373095, -4.414213562373095, 4)},
          new List<Vec3> {new Vec3(-2.0000000000000004, -5, -4), new Vec3(1.9999999999999996, -5, -4),
            new Vec3(1.9999999999999996, -5, 4), new Vec3(-2.0000000000000004, -5, 4)},
          new List<Vec3> {new Vec3(1.9999999999999996, -5, -4), new Vec3(3.414213562373095, -4.414213562373095, -4),
            new Vec3(3.414213562373095, -4.414213562373095, 4), new Vec3(1.9999999999999996, -5, 4)},
          new List<Vec3> {new Vec3(3.414213562373095, -4.414213562373095, -4), new Vec3(4, -3.0000000000000004, -4),
            new Vec3(4, -3.0000000000000004, 4), new Vec3(3.414213562373095, -4.414213562373095, 4)},
          new List<Vec3> {new Vec3(4, -3.0000000000000004, -4), new Vec3(4, 3, -4),
            new Vec3(4, 3, 4), new Vec3(4, -3.0000000000000004, 4)},
          new List<Vec3> {new Vec3(4, 3, 4), new Vec3(3.414213562373095, 4.414213562373095, 4), new Vec3(3.414213562373095, 3, 5.414213562373095)},
          new List<Vec3> {new Vec3(3.414213562373095, 3, 5.414213562373095), new Vec3(3.414213562373095, 4.414213562373095, 4), new Vec3(2, 4.414213562373095, 5.414213562373095)},
          new List<Vec3> {new Vec3(3.414213562373095, 4.414213562373095, 4), new Vec3(2, 5, 4), new Vec3(2, 4.414213562373095, 5.414213562373095)},
          new List<Vec3> {new Vec3(-1.9999999999999998, 5, 4), new Vec3(-3.414213562373095, 4.414213562373095, 4), new Vec3(-2, 4.414213562373095, 5.414213562373095)},
          new List<Vec3> {new Vec3(-2, 4.414213562373095, 5.414213562373095), new Vec3(-3.414213562373095, 4.414213562373095, 4), new Vec3(-3.414213562373095, 3, 5.414213562373095)},
          new List<Vec3> {new Vec3(-3.414213562373095, 4.414213562373095, 4), new Vec3(-4, 3.0000000000000004, 4), new Vec3(-3.414213562373095, 3, 5.414213562373095)},
          new List<Vec3> {new Vec3(-4, -2.9999999999999996, 4), new Vec3(-3.414213562373095, -4.414213562373095, 4), new Vec3(-3.414213562373095, -3, 5.414213562373095)},
          new List<Vec3> {new Vec3(-3.414213562373095, -3, 5.414213562373095), new Vec3(-3.414213562373095, -4.414213562373095, 4), new Vec3(-2.0000000000000004, -4.414213562373095, 5.414213562373095)},
          new List<Vec3> {new Vec3(-3.414213562373095, -4.414213562373095, 4), new Vec3(-2.0000000000000004, -5, 4), new Vec3(-2.0000000000000004, -4.414213562373095, 5.414213562373095)},
          new List<Vec3> {new Vec3(1.9999999999999996, -5, 4), new Vec3(3.414213562373095, -4.414213562373095, 4), new Vec3(1.9999999999999998, -4.414213562373095, 5.414213562373095)},
          new List<Vec3> {new Vec3(1.9999999999999998, -4.414213562373095, 5.414213562373095), new Vec3(3.414213562373095, -4.414213562373095, 4), new Vec3(3.414213562373095, -3.0000000000000004, 5.414213562373095)},
          new List<Vec3> {new Vec3(3.414213562373095, -4.414213562373095, 4), new Vec3(4, -3.0000000000000004, 4), new Vec3(3.414213562373095, -3.0000000000000004, 5.414213562373095)},
          new List<Vec3> {new Vec3(2, 5, 4), new Vec3(-1.9999999999999998, 5, 4),
            new Vec3(-2, 4.414213562373095, 5.414213562373095), new Vec3(2, 4.414213562373095, 5.414213562373095)},
          new List<Vec3> {new Vec3(-4, 3.0000000000000004, 4), new Vec3(-4, -2.9999999999999996, 4),
            new Vec3(-3.414213562373095, -3, 5.414213562373095), new Vec3(-3.414213562373095, 3, 5.414213562373095)},
          new List<Vec3> {new Vec3(-2.0000000000000004, -5, 4), new Vec3(1.9999999999999996, -5, 4),
            new Vec3(1.9999999999999998, -4.414213562373095, 5.414213562373095), new Vec3(-2.0000000000000004, -4.414213562373095, 5.414213562373095)},
          new List<Vec3> {new Vec3(4, -3.0000000000000004, 4), new Vec3(4, 3, 4),
            new Vec3(3.414213562373095, 3, 5.414213562373095), new Vec3(3.414213562373095, -3.0000000000000004, 5.414213562373095)},
          new List<Vec3> {new Vec3(4, -3.0000000000000004, -4), new Vec3(3.414213562373095, -4.414213562373095, -4), new Vec3(3.414213562373095, -3.0000000000000004, -5.414213562373095)},
          new List<Vec3> {new Vec3(3.414213562373095, -3.0000000000000004, -5.414213562373095), new Vec3(3.414213562373095, -4.414213562373095, -4), new Vec3(1.9999999999999998, -4.414213562373095, -5.414213562373095)},
          new List<Vec3> {new Vec3(3.414213562373095, -4.414213562373095, -4), new Vec3(1.9999999999999996, -5, -4), new Vec3(1.9999999999999998, -4.414213562373095, -5.414213562373095)},
          new List<Vec3> {new Vec3(-2.0000000000000004, -5, -4), new Vec3(-3.414213562373095, -4.414213562373095, -4), new Vec3(-2.0000000000000004, -4.414213562373095, -5.414213562373095)},
          new List<Vec3> {new Vec3(-2.0000000000000004, -4.414213562373095, -5.414213562373095), new Vec3(-3.414213562373095, -4.414213562373095, -4), new Vec3(-3.414213562373095, -3, -5.414213562373095)},
          new List<Vec3> {new Vec3(-3.414213562373095, -4.414213562373095, -4), new Vec3(-4, -2.9999999999999996, -4), new Vec3(-3.414213562373095, -3, -5.414213562373095)},
          new List<Vec3> {new Vec3(-4, 3.0000000000000004, -4), new Vec3(-3.414213562373095, 4.414213562373095, -4), new Vec3(-3.414213562373095, 3, -5.414213562373095)},
          new List<Vec3> {new Vec3(-3.414213562373095, 3, -5.414213562373095), new Vec3(-3.414213562373095, 4.414213562373095, -4), new Vec3(-2, 4.414213562373095, -5.414213562373095)},
          new List<Vec3> {new Vec3(-3.414213562373095, 4.414213562373095, -4), new Vec3(-1.9999999999999998, 5, -4), new Vec3(-2, 4.414213562373095, -5.414213562373095)},
          new List<Vec3> {new Vec3(2, 5, -4), new Vec3(3.414213562373095, 4.414213562373095, -4), new Vec3(2, 4.414213562373095, -5.414213562373095)},
          new List<Vec3> {new Vec3(2, 4.414213562373095, -5.414213562373095), new Vec3(3.414213562373095, 4.414213562373095, -4), new Vec3(3.414213562373095, 3, -5.414213562373095)},
          new List<Vec3> {new Vec3(3.414213562373095, 4.414213562373095, -4), new Vec3(4, 3, -4), new Vec3(3.414213562373095, 3, -5.414213562373095)},
          new List<Vec3> {new Vec3(1.9999999999999996, -5, -4), new Vec3(-2.0000000000000004, -5, -4),
            new Vec3(-2.0000000000000004, -4.414213562373095, -5.414213562373095), new Vec3(1.9999999999999998, -4.414213562373095, -5.414213562373095)},
          new List<Vec3> {new Vec3(-4, -2.9999999999999996, -4), new Vec3(-4, 3.0000000000000004, -4),
            new Vec3(-3.414213562373095, 3, -5.414213562373095), new Vec3(-3.414213562373095, -3, -5.414213562373095)},
          new List<Vec3> {new Vec3(-1.9999999999999998, 5, -4), new Vec3(2, 5, -4),
            new Vec3(2, 4.414213562373095, -5.414213562373095), new Vec3(-2, 4.414213562373095, -5.414213562373095)},
          new List<Vec3> {new Vec3(4, 3, -4), new Vec3(4, -3.0000000000000004, -4),
            new Vec3(3.414213562373095, -3.0000000000000004, -5.414213562373095), new Vec3(3.414213562373095, 3, -5.414213562373095)},
          new List<Vec3> {new Vec3(3.414213562373095, 3, 5.414213562373095), new Vec3(2, 4.414213562373095, 5.414213562373095), new Vec3(2, 3, 6)},
          new List<Vec3> {new Vec3(-2, 4.414213562373095, 5.414213562373095), new Vec3(-3.414213562373095, 3, 5.414213562373095), new Vec3(-2, 3, 6)},
          new List<Vec3> {new Vec3(-3.414213562373095, -3, 5.414213562373095), new Vec3(-2.0000000000000004, -4.414213562373095, 5.414213562373095), new Vec3(-2, -3, 6)},
          new List<Vec3> {new Vec3(1.9999999999999998, -4.414213562373095, 5.414213562373095), new Vec3(3.414213562373095, -3.0000000000000004, 5.414213562373095), new Vec3(2, -3, 6)},
          new List<Vec3> {new Vec3(2, 4.414213562373095, 5.414213562373095), new Vec3(-2, 4.414213562373095, 5.414213562373095),
            new Vec3(-2, 3, 6), new Vec3(2, 3, 6)},
          new List<Vec3> {new Vec3(-3.414213562373095, 3, 5.414213562373095), new Vec3(-3.414213562373095, -3, 5.414213562373095),
            new Vec3(-2, -3, 6), new Vec3(-2, 3, 6)},
          new List<Vec3> {new Vec3(-2.0000000000000004, -4.414213562373095, 5.414213562373095), new Vec3(1.9999999999999998, -4.414213562373095, 5.414213562373095),
            new Vec3(2, -3, 6), new Vec3(-2, -3, 6)},
          new List<Vec3> {new Vec3(3.414213562373095, -3.0000000000000004, 5.414213562373095), new Vec3(3.414213562373095, 3, 5.414213562373095),
            new Vec3(2, 3, 6), new Vec3(2, -3, 6)},
          new List<Vec3> {new Vec3(3.414213562373095, -3.0000000000000004, -5.414213562373095), new Vec3(1.9999999999999998, -4.414213562373095, -5.414213562373095), new Vec3(2, -3, -6)},
          new List<Vec3> {new Vec3(-2.0000000000000004, -4.414213562373095, -5.414213562373095), new Vec3(-3.414213562373095, -3, -5.414213562373095), new Vec3(-2, -3, -6)},
          new List<Vec3> {new Vec3(-3.414213562373095, 3, -5.414213562373095), new Vec3(-2, 4.414213562373095, -5.414213562373095), new Vec3(-2, 3, -6)},
          new List<Vec3> {new Vec3(2, 4.414213562373095, -5.414213562373095), new Vec3(3.414213562373095, 3, -5.414213562373095), new Vec3(2, 3, -6)},
          new List<Vec3> {new Vec3(1.9999999999999998, -4.414213562373095, -5.414213562373095), new Vec3(-2.0000000000000004, -4.414213562373095, -5.414213562373095),
            new Vec3(-2, -3, -6), new Vec3(2, -3, -6)},
          new List<Vec3> {new Vec3(-3.414213562373095, -3, -5.414213562373095), new Vec3(-3.414213562373095, 3, -5.414213562373095),
            new Vec3(-2, 3, -6), new Vec3(-2, -3, -6)},
          new List<Vec3> {new Vec3(-2, 4.414213562373095, -5.414213562373095), new Vec3(2, 4.414213562373095, -5.414213562373095),
            new Vec3(2, 3, -6), new Vec3(-2, 3, -6)},
          new List<Vec3> {new Vec3(3.414213562373095, 3, -5.414213562373095), new Vec3(3.414213562373095, -3.0000000000000004, -5.414213562373095),
            new Vec3(2, -3, -6), new Vec3(2, 3, -6)},
          new List<Vec3> {new Vec3(2, 3, 6), new Vec3(-2, 3, 6), new Vec3(-2, -3, 6), new Vec3(2, -3, 6)},
          new List<Vec3> {new Vec3(2, -3, -6), new Vec3(-2, -3, -6), new Vec3(-2, 3, -6), new Vec3(2, 3, -6)}
        };
        Assert.AreEqual(pts.Count, 62);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
