using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class MayOverlapTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestMayOverlapReliability()
    {
        var geometry1 = Center(Cuboid(size: (4, 4, 4), center: (0, 0, 0)), relativeTo: (0, 0, 0));
        var geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (0, 0, 0));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));

        // overlap at each corner
        geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (3, 3, 3));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (-3, 3, 3));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (-3, -3, 3));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (3, -3, 3));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (-3, -3, -3));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (3, -3, -3));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (3, 3, -3));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (2, 2, 2), center: (0,0,0)), relativeTo: (-3, 3, -3));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));

        // from issue #137, precision errors cause determination to fail
        // see the value of EPS
        var issue1 = Center(Cuboid(size: (44, 26, 5), center: (0,0,0)), relativeTo: (0, 0, -1));
        var issue2 = Center(Cuboid(size: (44, 26, 1.8), center: (0,0,0)), relativeTo: (5, 0, -4.400001));
        Assert.IsTrue(MayOverlap(issue1, issue2));

        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (0, 0, 4 + 0.000001));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (0, 0, -4 - 0.000001));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (0, 4 + 0.000001, 0));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (0, -4 - 0.000001, 0));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (4 + 0.000001, 0, 0));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (-4 - 0.000001, 0, 0));
        Assert.IsTrue(MayOverlap(geometry1, geometry2));

        // NO overlap tests

        Assert.IsFalse(MayOverlap(geometry1, new Geom3()));
        Assert.IsFalse(MayOverlap(new Geom3(), geometry1));

        var eps = C.EPS + (C.EPS * 0.001);
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (0, 0, 4 + eps));
        Assert.IsFalse(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (0, 0, -4 - eps));
        Assert.IsFalse(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (0, 4 + eps, 0));
        Assert.IsFalse(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (0, -4 - eps, 0));
        Assert.IsFalse(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (4 + eps, 0, 0));
        Assert.IsFalse(MayOverlap(geometry1, geometry2));
        geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0,0,0)), relativeTo: (-4 - eps, 0, 0));
        Assert.IsFalse(MayOverlap(geometry1, geometry2));
    }
}

