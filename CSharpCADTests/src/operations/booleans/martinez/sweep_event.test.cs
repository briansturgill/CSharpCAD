namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class MartinezTestsSweepEvent
{
    //static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSweepEventisBelow()
    {
        var s1 = new SweepEvent((0, 0), true, new SweepEvent((1, 1), false, null, false), false);
        var s2 = new SweepEvent((0, 1), false, new SweepEvent((0, 0), false, null, false), false);

        Assert.IsTrue(s1.isBelow((0, 1)));
        Assert.IsTrue(s1.isBelow((1, 2)));
        Assert.IsFalse(s1.isBelow((0, 0)));
        Assert.IsFalse(s1.isBelow((5, -1)));

        Assert.IsFalse(s2.isBelow((0, 1)));
        Assert.IsFalse(s2.isBelow((1, 2)));
        Assert.IsFalse(s2.isBelow((0, 0)));
        Assert.IsFalse(s2.isBelow((5, -1)));
    }

    [Test]
    public void TestSweepEventisAbove()
    {
        var s1 = new SweepEvent((0, 0), true, new SweepEvent((1, 1), false, null, false), false);
        var s2 = new SweepEvent((0, 1), false, new SweepEvent((0, 0), false, null, false), false);

        Assert.IsFalse(s1.isAbove((0, 1)));
        Assert.IsFalse(s1.isAbove((1, 2)));
        Assert.IsTrue(s1.isAbove((0, 0)));
        Assert.IsTrue(s1.isAbove((5, -1)));

        Assert.IsTrue(s2.isAbove((0, 1)));
        Assert.IsTrue(s2.isAbove((1, 2)));
        Assert.IsTrue(s2.isAbove((0, 0)));
        Assert.IsTrue(s2.isAbove((5, -1)));
    }

    [Test]
    public void TestSweepEventisVertical()
    {
        Assert.IsTrue(new SweepEvent((0, 0), true, new SweepEvent((0, 1), false, null, false), false).isVertical());
        Assert.IsFalse(new SweepEvent((0, 0), true, new SweepEvent((0.0001, 1), false, null, false), false).isVertical());
    }
}