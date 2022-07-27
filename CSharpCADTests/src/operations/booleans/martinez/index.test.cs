namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class MartinezTestsGeneral
{
    //static bool WriteTests = false;

    List<List<List<Vec2>>> subject = new List<List<List<Vec2>>>
    {
      new List<List<Vec2>> {
        new List<Vec2>{
          new Vec2(20, -23.5),
          new Vec2(170, 74),
          new Vec2(226.5, -113.5),
          new Vec2(20, -23.5)
        }
      }
    };
    List<List<List<Vec2>>> clipping = new List<List<List<Vec2>>>
    {
      new List<List<Vec2>> {
        new List<Vec2>{
          new Vec2(54.5, -170.5),
          new Vec2(140.5, 33.5),
          new Vec2(239.5, -198),
          new Vec2(54.5, -170.5)
        }
      }
    };

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestFillEventQueue()
    {
        var s = subject;
        var c = clipping;

        var sbbox = (new Vec2(Double.PositiveInfinity, Double.PositiveInfinity), new Vec2(Double.NegativeInfinity, Double.NegativeInfinity));
        var cbbox = (new Vec2(Double.PositiveInfinity, Double.PositiveInfinity), new Vec2(Double.NegativeInfinity, Double.NegativeInfinity));
        var q = fillQueue(s, c, ref sbbox, ref cbbox, 0);
        SweepEvent currentPoint;

        Assert.AreEqual(sbbox, (new Vec2(20, -113.5), new Vec2(226.5, 74)), "subject bbox");
        Assert.AreEqual(cbbox, (new Vec2(54.5, -198), new Vec2(239.5, 33.5)), "clipping bbox");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(20, -23.5)); /* s[0][0] */
        Assert.IsTrue(currentPoint.left, "is left");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(226.5, -113.5), "other event"); /* s[0][2] */
        Assert.IsFalse(currentPoint.otherEvent.left, "other event is right");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(20, -23.5)); /* s[0][0] */
        Assert.IsTrue(currentPoint.left, "is left");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(170, 74), "other event"); /* s[0][1] */
        Assert.IsFalse(currentPoint.otherEvent.left, "other event is right");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(54.5, -170.5)); /* c[0][0] */
        Assert.IsTrue(currentPoint.left, "is left");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(239.5, -198), "other event"); /* c[0][2] */
        Assert.IsFalse(currentPoint.otherEvent.left, "other event is right");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(54.5, -170.5)); /* c[0][0] */
        Assert.IsTrue(currentPoint.left, "is left");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(140.5, 33.5), "other event"); /* c[0][1] */
        Assert.IsFalse(currentPoint.otherEvent.left, "other event is right");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(140.5, 33.5)); /* c[0][0] */
        Assert.IsFalse(currentPoint.left, "is right");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(54.5, -170.5), "other event"); /* c[0][1] */
        Assert.IsTrue(currentPoint.otherEvent.left, "other event is left");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(140.5, 33.5)); /* c[0][0] */
        Assert.IsTrue(currentPoint.left, "is left");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(239.5, -198), "other event"); /* c[0][1] */
        Assert.IsFalse(currentPoint.otherEvent.left, "other event is right");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(170, 74)); /* s[0][1] */
        Assert.IsFalse(currentPoint.left, "is right");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(20, -23.5), "other event"); /* s[0][0] */
        Assert.IsTrue(currentPoint.otherEvent.left, "other event is left");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(170, 74)); /* s[0][1] */
        Assert.IsTrue(currentPoint.left, "is left");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(226.5, -113.5), "other event"); /* s[0][3] */
        Assert.IsFalse(currentPoint.otherEvent.left, "other event is right");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(226.5, -113.5)); /* s[0][1] */
        Assert.IsFalse(currentPoint.left, "is right");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(20, -23.5), "other event"); /* s[0][0] */
        Assert.IsTrue(currentPoint.otherEvent.left, "other event is left");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(226.5, -113.5)); /* s[0][1] */
        Assert.IsFalse(currentPoint.left, "is right");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(170, 74), "other event"); /* s[0][0] */
        Assert.IsTrue(currentPoint.otherEvent.left, "other event is left");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(239.5, -198)); /* c[0][2] */
        Assert.IsFalse(currentPoint.left, "is right");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(54.5, -170.5), "other event"); /* c[0][0] */
        Assert.IsTrue(currentPoint.otherEvent.left, "other event is left");

        currentPoint = q.pop();
        Assert.AreEqual(currentPoint.point, new Vec2(239.5, -198)); /* c[0][2] */
        Assert.IsFalse(currentPoint.left, "is right");
        Assert.AreEqual(currentPoint.otherEvent.point, new Vec2(140.5, 33.5), "other event"); /* s[0][1] */
        Assert.IsTrue(currentPoint.otherEvent.left, "other event is left");
    }

}