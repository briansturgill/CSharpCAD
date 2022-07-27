namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class MartinezTestsSegementIntersect
{
    //static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSegementIntersect()
    {
        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(1, 0), new Vec2(2, 2)), null, "null if no intersections");
        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(1, 0), new Vec2(10, 2)), null, "null if no intersections");
        Assert.AreEqual(segmentIntersection(new Vec2(2, 2), new Vec2(3, 3), new Vec2(0, 6), new Vec2(2, 4)), null, "null if no intersections");

        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(1, 0), new Vec2(0, 1)), new List<Vec2> { new Vec2(0.5, 0.5) }, "1 intersection");
        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(0, 1), new Vec2(0, 0)), new List<Vec2> { new Vec2(0, 0) }, "shared point 1");
        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(0, 1), new Vec2(1, 1)), new List<Vec2> { new Vec2(1, 1) }, "shared point 2");

        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(0.5, 0.5), new Vec2(1, 0)), new List<Vec2> { new Vec2(0.5, 0.5) }, "T-crossing");

        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(10, 10), new Vec2(1, 1), new Vec2(5, 5)), new List<Vec2> { new Vec2(1, 1), new Vec2(5, 5) }, "full overlap");
        Assert.AreEqual(segmentIntersection(new Vec2(1, 1), new Vec2(10, 10), new Vec2(1, 1), new Vec2(5, 5)), new List<Vec2> { new Vec2(1, 1), new Vec2(5, 5) }, "shared point + overlap");
        Assert.AreEqual(segmentIntersection(new Vec2(3, 3), new Vec2(10, 10), new Vec2(0, 0), new Vec2(5, 5)), new List<Vec2> { new Vec2(3, 3), new Vec2(5, 5) }, "mutual overlap");
        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(0, 0), new Vec2(1, 1)), new List<Vec2> { new Vec2(0, 0), new Vec2(1, 1) }, "full overlap");
        Assert.AreEqual(segmentIntersection(new Vec2(1, 1), new Vec2(0, 0), new Vec2(0, 0), new Vec2(1, 1)), new List<Vec2> { new Vec2(1, 1), new Vec2(0, 0) }, "full overlap, orientation");

        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(1, 1), new Vec2(2, 2)), new List<Vec2> { new Vec2(1, 1) }, "collinear, shared point");
        Assert.AreEqual(segmentIntersection(new Vec2(1, 1), new Vec2(0, 0), new Vec2(1, 1), new Vec2(2, 2)), new List<Vec2> { new Vec2(1, 1) }, "collinear, shared other point");
        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(2, 2), new Vec2(4, 4)), null, "collinear, no overlap");
        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(0, -1), new Vec2(1, 0)), null, "parallel");
        Assert.AreEqual(segmentIntersection(new Vec2(1, 1), new Vec2(0, 0), new Vec2(0, -1), new Vec2(1, 0)), null, "parallel, orientation");
        Assert.AreEqual(segmentIntersection(new Vec2(0, -1), new Vec2(1, 0), new Vec2(0, 0), new Vec2(1, 1)), null, "parallel, position");

        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(0, 1), new Vec2(0, 0), true), null, "shared point 1, skip touches");
        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(0, 1), new Vec2(1, 1), true), null, "shared point 2, skip touches");

        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(1, 1), new Vec2(2, 2), true), null, "collinear, shared point, skip touches");
        Assert.AreEqual(segmentIntersection(new Vec2(1, 1), new Vec2(0, 0), new Vec2(1, 1), new Vec2(2, 2), true), null, "collinear, shared other point, skip touches");

        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(0, 0), new Vec2(1, 1), true), null, "full overlap, skip touches");
        Assert.AreEqual(segmentIntersection(new Vec2(1, 1), new Vec2(0, 0), new Vec2(0, 0), new Vec2(1, 1), true), null, "full overlap, orientation, skip touches");

        Assert.AreEqual(segmentIntersection(new Vec2(0, 0), new Vec2(1, 1), new Vec2(1, 0), new Vec2(0, 1), true), new List<Vec2> { new Vec2(0.5, 0.5) }, "1 intersection, skip touches");
    }
}