using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class GeodesicSphereTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestGeodesicSphereDefaults()
    {
        var obs = GeodesicSphere();
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Count, 20);
    }

    [Test]
    public void TestGeodesicSphereOptions()
    {
        // test radius
        var obs = GeodesicSphere(radius: 5);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(4.253254557317035, 0, 2.628654726407001), new Vec3(2.628654726407001, -4.253254557317035, 0), new Vec3(4.253254557317035, 0, -2.628654726407001)},
          new List<Vec3> {new Vec3(4.253254557317035, 0, -2.628654726407001), new Vec3(2.628654726407001, 4.253254557317035, 0), new Vec3(4.253254557317035, 0, 2.628654726407001)},
          new List<Vec3> {new Vec3(4.253254557317035, 0, -2.628654726407001), new Vec3(0, -2.628654726407001, -4.253254557317035), new Vec3(0, 2.628654726407001, -4.253254557317035)},
          new List<Vec3> {new Vec3(4.253254557317035, 0, -2.628654726407001), new Vec3(0, 2.628654726407001, -4.253254557317035), new Vec3(2.628654726407001, 4.253254557317035, 0)},
          new List<Vec3> {new Vec3(4.253254557317035, 0, -2.628654726407001), new Vec3(2.628654726407001, -4.253254557317035, 0), new Vec3(0, -2.628654726407001, -4.253254557317035)},
          new List<Vec3> {new Vec3(0, -2.628654726407001, 4.253254557317035), new Vec3(4.253254557317035, 0, 2.628654726407001), new Vec3(0, 2.628654726407001, 4.253254557317035)},
          new List<Vec3> {new Vec3(2.628654726407001, -4.253254557317035, 0), new Vec3(4.253254557317035, 0, 2.628654726407001), new Vec3(0, -2.628654726407001, 4.253254557317035)},
          new List<Vec3> {new Vec3(4.253254557317035, 0, 2.628654726407001), new Vec3(2.628654726407001, 4.253254557317035, 0), new Vec3(0, 2.628654726407001, 4.253254557317035)},
          new List<Vec3> {new Vec3(-4.253254557317035, 0, -2.628654726407001), new Vec3(-2.628654726407001, -4.253254557317035, 0), new Vec3(-4.253254557317035, 0, 2.628654726407001)},
          new List<Vec3> {new Vec3(-4.253254557317035, 0, 2.628654726407001), new Vec3(-2.628654726407001, 4.253254557317035, 0), new Vec3(-4.253254557317035, 0, -2.628654726407001)},
          new List<Vec3> {new Vec3(0, -2.628654726407001, 4.253254557317035), new Vec3(0, 2.628654726407001, 4.253254557317035), new Vec3(-4.253254557317035, 0, 2.628654726407001)},
          new List<Vec3> {new Vec3(-4.253254557317035, 0, 2.628654726407001), new Vec3(-2.628654726407001, -4.253254557317035, 0), new Vec3(0, -2.628654726407001, 4.253254557317035)},
          new List<Vec3> {new Vec3(0, 2.628654726407001, 4.253254557317035), new Vec3(-2.628654726407001, 4.253254557317035, 0), new Vec3(-4.253254557317035, 0, 2.628654726407001)},
          new List<Vec3> {new Vec3(0, 2.628654726407001, -4.253254557317035), new Vec3(0, -2.628654726407001, -4.253254557317035), new Vec3(-4.253254557317035, 0, -2.628654726407001)},
          new List<Vec3> {new Vec3(-4.253254557317035, 0, -2.628654726407001), new Vec3(-2.628654726407001, 4.253254557317035, 0), new Vec3(0, 2.628654726407001, -4.253254557317035)},
          new List<Vec3> {new Vec3(-4.253254557317035, 0, -2.628654726407001), new Vec3(0, -2.628654726407001, -4.253254557317035), new Vec3(-2.628654726407001, -4.253254557317035, 0)},
          new List<Vec3> {new Vec3(0, -2.628654726407001, 4.253254557317035), new Vec3(-2.628654726407001, -4.253254557317035, 0), new Vec3(2.628654726407001, -4.253254557317035, 0)},
          new List<Vec3> {new Vec3(0, 2.628654726407001, 4.253254557317035), new Vec3(2.628654726407001, 4.253254557317035, 0), new Vec3(-2.628654726407001, 4.253254557317035, 0)},
          new List<Vec3> {new Vec3(0, 2.628654726407001, -4.253254557317035), new Vec3(-2.628654726407001, 4.253254557317035, 0), new Vec3(2.628654726407001, 4.253254557317035, 0)},
          new List<Vec3> {new Vec3(0, -2.628654726407001, -4.253254557317035), new Vec3(2.628654726407001, -4.253254557317035, 0), new Vec3(-2.628654726407001, -4.253254557317035, 0)}
        };

        Assert.AreEqual(pts.Count, 20);
        Assert.AreEqual(exp.Count, 20);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // test frequency
        obs = GeodesicSphere(radius: 5, frequency: 18);
        // LATER JSCAD Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 180);
    }
}