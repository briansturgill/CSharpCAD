using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class TorusTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestTorusDefaults()
    {
        var obs = Torus();
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Count, 2048); // 32 * 32 * 2 (polys/segment) = 2048

        var (min, max) = obs.BoundingBox();
        Assert.IsTrue(min == new Vec3(-5, -5, -1) && max == new Vec3(5, 5, 1));
    }

    [Test]
    public void TestTorusSimpleOptions()
    {
        var obs = Torus(innerRadius: 0.5, innerSegments: 5, outerRadius: 5, outerSegments: 8);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Count, 80); // 4 * 8 * 2 (polys/segment) = 64
    }

    [Test]
    public void TestTorusComplexOptions()
    {
        var obs = Torus(innerRadius: 1, outerRadius: 5, innerSegments: 32, outerSegments: 72, startAngle: 90, outerRotation: 90);
        // LATER Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Count, 1216);
        var (min, max) = obs.BoundingBox();
        Assert.IsTrue(min.IsNearlyEqual(new Vec3(-6, 0, -1)));
        Assert.IsTrue(max.IsNearlyEqual(new Vec3(0, 6, 1)));
    }
}
