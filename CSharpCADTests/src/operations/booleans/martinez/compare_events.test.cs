namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class MartinezTestsCompareEvents
{
    //static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void QueueShouldProcessLestByXFirst()
    {
        //"queue should process lest(by x) sweep event first"
        var queue = new TinyQueue();
        var e1 = new SweepEvent((0.0, 0.0), false, null, false);
        var e2 = new SweepEvent((0.5, 0.5), false, null, false);

        queue.push(e1);
        queue.push(e2);

        Assert.AreEqual(e1, queue.pop());
        Assert.AreEqual(e2, queue.pop());
    }

    [Test]
    public void QueueShouldProcessLestByYFirst()
    {
        //"queue should process lest(by y) sweep event first"
        var queue = new TinyQueue();
        var e1 = new SweepEvent((0.0, 0.0), false, null, false);
        var e2 = new SweepEvent((0.0, 0.5), false, null, false);

        queue.push(e1);
        queue.push(e2);

        Assert.AreEqual(e1, queue.pop());
        Assert.AreEqual(e2, queue.pop());
    }


    [Test]
    public void QueueShouldProcessLestByLeftPropFirst()
    {
        //"queue should pop least(by left prop) sweep event first"
        var queue = new TinyQueue();
        var e1 = new SweepEvent((0.0, 0.0), true, null, false);
        var e2 = new SweepEvent((0.0, 0.0), false, null, false);

        queue.push(e1);
        queue.push(e2);

        Assert.AreEqual(e2, queue.pop());
        Assert.AreEqual(e1, queue.pop());
    }

    [Test]
    public void SweepEventCompXCoordinates()
    {
        //"sweep event comparison x coordinates"
        var e1 = new SweepEvent((0.0, 0.0), false, null, false);
        var e2 = new SweepEvent((0.5, 0.5), false, null, false);

        Assert.AreEqual(compareEvents(e1, e2), -1);
        Assert.AreEqual(compareEvents(e2, e1), 1);
    }

    [Test]
    public void SweepEventCompYCoordinates()
    {
        //"sweep event comparison y coordinates"
        var e1 = new SweepEvent((0.0, 0.0), false, null, false);
        var e2 = new SweepEvent((0.0, 0.5), false, null, false);

        Assert.AreEqual(compareEvents(e1, e2), -1);
        Assert.AreEqual(compareEvents(e2, e1), 1);
    }

    [Test]
    public void SweepEventCompNotLeftFirst()
    {
        //"sweep event comparison not left first", (t) => {
        var e1 = new SweepEvent((0.0, 0.0), true, null, false);
        var e2 = new SweepEvent((0.0, 0.0), false, null, false);

        Assert.AreEqual(compareEvents(e1, e2), 1);
        Assert.AreEqual(compareEvents(e2, e1), -1);
    }

    [Test]
    public void SweepEventCompSharedStartNotCollinear()
    {
        //"sweep event comparison shared start point not collinear edges"

        var e1 = new SweepEvent((0.0, 0.0), true, new SweepEvent((1, 1), false, null, false), false);
        var e2 = new SweepEvent((0.0, 0.0), true, new SweepEvent((2, 3), false, null, false), false);

        Assert.AreEqual(compareEvents(e1, e2), -1, "lower is processed first");
        Assert.AreEqual(compareEvents(e2, e1), 1, "higher is processed second");
    }

    [Test]
    public void SweepEventCompCollinearEdges()
    {
        //"sweep event comparison collinear edges"
        var e1 = new SweepEvent((0.0, 0.0), true, new SweepEvent((1, 1), false, null, false), true);
        var e2 = new SweepEvent((0.0, 0.0), true, new SweepEvent((2, 2), false, null, false), false);

        Assert.AreEqual(compareEvents(e1, e2), -1, "clipping is processed first");
        Assert.AreEqual(compareEvents(e2, e1), 1, "subject is processed second");
    }
}
