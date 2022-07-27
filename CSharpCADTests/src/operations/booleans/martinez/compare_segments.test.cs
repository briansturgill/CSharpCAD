namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class MartinezTestsCompareSegments
{
    //static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestNotCollinearSharedLeftRight()
    {
        //"shared left point - right point first"
        var tree = new SplayTree<SweepEvent, int>(compareSegments);
        var pt = new Vec2(0.0, 0.0);
        var se1 = new SweepEvent(pt, true, new SweepEvent(new Vec2(1, 1), false, null, false), false);
        var se2 = new SweepEvent(pt, true, new SweepEvent(new Vec2(2, 3), false, null, false), false);

        tree.insert(se1, 0);
        tree.insert(se2, 0);

        Assert.AreEqual(tree.maxNode(tree.root).key.otherEvent.point, new Vec2(2, 3));
        Assert.AreEqual(tree.minNode(tree.root).key.otherEvent.point, new Vec2(1, 1));
    }

    [Test]
    public void TestNotCollinearDifferentLeftRight()
    {
        //"different left point - right point y coord to sort"
        var tree = new SplayTree<SweepEvent, int>(compareSegments);
        var se1 = new SweepEvent(new Vec2(0, 1), true, new SweepEvent(new Vec2(1, 1), false, null, false), false);
        var se2 = new SweepEvent(new Vec2(0, 2), true, new SweepEvent(new Vec2(2, 3), false, null, false), false);

        tree.insert(se1, 0);
        tree.insert(se2, 0);

        Assert.AreEqual(tree.minNode(tree.root).key.otherEvent.point, new Vec2(1, 1));
        Assert.AreEqual(tree.maxNode(tree.root).key.otherEvent.point, new Vec2(2, 3));
    }

    [Test]
    public void TestNotCollinearEventsOrderInSweepLine()
    {
        //"events order in sweep line"
        var se1 = new SweepEvent(new Vec2(0, 1), true, new SweepEvent(new Vec2(2, 1), false, null, false), false);
        var se2 = new SweepEvent(new Vec2(-1, 0), true, new SweepEvent(new Vec2(2, 3), false, null, false), false);

        var se3 = new SweepEvent(new Vec2(0, 1), true, new SweepEvent(new Vec2(3, 4), false, null, false), false);
        var se4 = new SweepEvent(new Vec2(-1, 0), true, new SweepEvent(new Vec2(3, 1), false, null, false), false);

        Assert.AreEqual(compareEvents(se1, se2), 1);
        Assert.IsFalse(se2.isBelow(se1.point));
        Assert.IsTrue(se2.isAbove(se1.point));

        Assert.AreEqual(compareSegments(se1, se2), -1, "compare segments");
        Assert.AreEqual(compareSegments(se2, se1), 1, "compare segments inverted");

        Assert.AreEqual(compareEvents(se3, se4), 1);
        Assert.IsFalse(se4.isAbove(se3.point));
    }

    [Test]
    public void TestNotCollinearEventsOrderInSweepLineFirstPointBelow()
    {
        //"first point is below"
        var se2 = new SweepEvent(new Vec2(0, 1), true, new SweepEvent(new Vec2(2, 1), false, null, false), false);
        var se1 = new SweepEvent(new Vec2(-1, 0), true, new SweepEvent(new Vec2(2, 3), false, null, false), false);

        Assert.IsFalse(se1.isBelow(se2.point));
        Assert.AreEqual(compareSegments(se1, se2), 1, "compare segments");
    }

    [Test]
    public void TestCollinear()
    {
        var se1 = new SweepEvent(new Vec2(1, 1), true, new SweepEvent(new Vec2(5, 1), false, null, false), true);
        var se2 = new SweepEvent(new Vec2(2, 1), true, new SweepEvent(new Vec2(3, 1), false, null, false), false);

        Assert.AreNotEqual(se1.isSubject, se2.isSubject);
        Assert.AreEqual(compareSegments(se1, se2), -1);
    }

    [Test]
    public void TestCollinearSharedLeft()
    {
        //"collinear shared left point"
        var pt = new Vec2(0, 1);

        var se1 = new SweepEvent(pt, true, new SweepEvent(new Vec2(5, 1), false, null, false), false);
        var se2 = new SweepEvent(pt, true, new SweepEvent(new Vec2(3, 1), false, null, false), false);

        se1.outputContourId = 1;
        se2.outputContourId = 2;

        Assert.AreEqual(se1.isSubject, se2.isSubject);
        Assert.AreEqual(se1.point, se2.point);

        Assert.AreEqual(compareSegments(se1, se2), -1);

        se1.outputContourId = 2;
        se2.outputContourId = 1;

        Assert.AreEqual(compareSegments(se1, se2), +1);
    }


    [Test]
    public void TestCollinearDifferentLeft()
    {
        //"collinear same polygon different left points"
        {
            var se1 = new SweepEvent(new Vec2(1, 1), true, new SweepEvent(new Vec2(5, 1), false, null, false), true);
            var se2 = new SweepEvent(new Vec2(2, 1), true, new SweepEvent(new Vec2(3, 1), false, null, false), true);

            Assert.AreEqual(se1.isSubject, se2.isSubject);
            Assert.AreNotEqual(se1.point, se2.point);
            Assert.AreEqual(compareSegments(se1, se2), -1);
            Assert.AreEqual(compareSegments(se2, se1), 1);
        }
    }
}