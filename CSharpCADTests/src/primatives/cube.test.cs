using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class CubeTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCubeDefaults()
    {
        var obs = Cube(size: 2, center: (0,0, 0)); // CSCAD changed the default center.
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = ((Geom3)obs).ToPolygons();
        foreach (var p in pts)
        {
        }
        Assert.AreEqual(pts.Length, 6);
    }

    [Test]
    public void TestCubeOptions()
    {
        // test center
        var obs = Cube(size: 7, center: (6.5, 6.5, 6.5));
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = ((Geom3)obs).ToPolygons();
        var exp = new Poly3[] {
          new Poly3(new Vec3[] { new Vec3(3, 3, 3), new Vec3(3, 3, 10), new Vec3(3, 10, 10), new Vec3(3, 10, 3)}),
          new Poly3(new Vec3[] { new Vec3(10, 3, 3), new Vec3(10, 10, 3), new Vec3(10, 10, 10), new Vec3(10, 3, 10)}),
          new Poly3(new Vec3[] { new Vec3(3, 3, 3), new Vec3(10, 3, 3), new Vec3(10, 3, 10), new Vec3(3, 3, 10)}),
          new Poly3(new Vec3[] { new Vec3(3, 10, 3), new Vec3(3, 10, 10), new Vec3(10, 10, 10), new Vec3(10, 10, 3)}),
          new Poly3(new Vec3[] { new Vec3(3, 3, 3), new Vec3(3, 10, 3), new Vec3(10, 10, 3), new Vec3(10, 3, 3)}),
          new Poly3(new Vec3[] { new Vec3(3, 3, 10), new Vec3(10, 3, 10), new Vec3(10, 10, 10), new Vec3(3, 10, 10)})
        };

        Assert.AreEqual(pts.Length, 6);
        Assert.IsTrue(Helpers.CompareArrays(pts, exp));

        // test size
        obs = Cube(size: 7, center: (0,0, 0));
        Assert.DoesNotThrow(() => obs.Validate());
        pts = ((Geom3)obs).ToPolygons();
        exp = new Poly3[] {
          new Poly3(new Vec3[] { new Vec3(-3.5, -3.5, -3.5), new Vec3(-3.5, -3.5, 3.5), new Vec3(-3.5, 3.5, 3.5), new Vec3(-3.5, 3.5, -3.5)}),
          new Poly3(new Vec3[] { new Vec3(3.5, -3.5, -3.5), new Vec3(3.5, 3.5, -3.5), new Vec3(3.5, 3.5, 3.5), new Vec3(3.5, -3.5, 3.5)}),
          new Poly3(new Vec3[] { new Vec3(-3.5, -3.5, -3.5), new Vec3(3.5, -3.5, -3.5), new Vec3(3.5, -3.5, 3.5), new Vec3(-3.5, -3.5, 3.5)}),
          new Poly3(new Vec3[] { new Vec3(-3.5, 3.5, -3.5), new Vec3(-3.5, 3.5, 3.5), new Vec3(3.5, 3.5, 3.5), new Vec3(3.5, 3.5, -3.5)}),
          new Poly3(new Vec3[] { new Vec3(-3.5, -3.5, -3.5), new Vec3(-3.5, 3.5, -3.5), new Vec3(3.5, 3.5, -3.5), new Vec3(3.5, -3.5, -3.5)}),
          new Poly3(new Vec3[] { new Vec3(-3.5, -3.5, 3.5), new Vec3(3.5, -3.5, 3.5), new Vec3(3.5, 3.5, 3.5), new Vec3(-3.5, 3.5, 3.5)})
        };

        Assert.AreEqual(pts.Length, 6);
        Assert.IsTrue(Helpers.CompareArrays(pts, exp));
    }
}
