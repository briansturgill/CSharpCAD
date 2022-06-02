using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class TestAlign
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSingleAllUnaligned()
    {
        var original = Cube(size: 4, center: (10, 10, 10));
        var aligned = (Geom3)Align(original, modes: AM.UUU);
        var bounds = aligned.BoundingBox();
        var expectedBounds = (new Vec3(8, 8, 8), new Vec3(12, 12, 12));
        Assert.DoesNotThrow(() => aligned.Validate());
        Assert.AreEqual(bounds, expectedBounds);
    }

    public void TestSingleDifferentModesEachAxis()
    {
        var original = Cube(size: 4, center: (10, 10, 10));
        var aligned = (Geom3)Align(original, modes: AM.CNX);
        var bounds = aligned.BoundingBox();
        var expectedBounds = (new Vec3(-2, 0, -4), new Vec3(2, 4, 0));
        Assert.DoesNotThrow(() => aligned.Validate());
        Assert.AreEqual(bounds, expectedBounds);
    }

    public void TestMultipleUnioned()
    {
        var original = Union(
          Cube(size: 4, center: (10, 10, 10)),
          Cube(size: 2, center: (4, 4, 4))
        );
        var aligned = (Geom3)Align(original, AM.CNX, relativeTo: (30, 30, 30));
        var bounds = aligned.BoundingBox();
        var expectedBounds = (new Vec3(28, 30, 26), new Vec3(32, 34, 30));
        Assert.DoesNotThrow(() => aligned.Validate());
        Assert.AreEqual(bounds, expectedBounds);
    }
}