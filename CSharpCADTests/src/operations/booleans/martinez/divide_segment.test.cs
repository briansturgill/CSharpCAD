namespace CSharpCADTests;

#nullable disable

using static Geom2Booleans;

[TestFixture]
public class MartinezTestsDivideSegments
{
    //static bool WriteTests = false;

    static List<List<List<Vec2>>> subject = new List<List<List<Vec2>>> {
      new List<List<Vec2>> {
        new List<Vec2> {
          new Vec2(16,282),
          new Vec2(298,359),
          new Vec2(153,203.5),
          new Vec2(16,282)
        }
      }
    };

    static List<List<List<Vec2>>> clipping = new List<List<List<Vec2>>> {
      new List<List<Vec2>> {
        new List<Vec2> {
          new Vec2(56,181),
          new Vec2(153,294.5),
          new Vec2(241.5,229.5),
          new Vec2(108.5,120),
          new Vec2(56,181)
        }
      }
    };

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestDivide2Segments()
    {
        var se1 = new SweepEvent(new Vec2(0, 0), true, new SweepEvent(new Vec2(5, 5), false, null, false), true);
        var se2 = new SweepEvent(new Vec2(0, 5), true, new SweepEvent(new Vec2(5, 0), false, null, false), false);
        var q = new TinyQueue();

        q.push(se1);
        q.push(se2);

        var iter = segmentIntersection(
          se1.point, se1.otherEvent.point,
          se2.point, se2.otherEvent.point
        );


        divideSegment(se1, iter[0], q);
        divideSegment(se2, iter[0], q);

        Assert.AreEqual(q.length, 6, "subdivided in 4 segments by intersection point");
    }

    [Test]
    public void TestPossibleInteractions()
    {
        var s = subject[0];
        var c = clipping[0];

        var q = new TinyQueue();

        var se1 = new SweepEvent(s[0][3], true, new SweepEvent(s[0][2], false, null, false), true);
        var se2 = new SweepEvent(c[0][0], true, new SweepEvent(c[0][1], false, null, false), false);

        Assert.AreEqual(possibleIntersection(se1, se2, q), 1);
        Assert.AreEqual(q.length, 4);

        var e = q.pop();
        Assert.AreEqual(e.point, new Vec2(100.79403384562251, 233.41363754101192));
        Assert.AreEqual(e.otherEvent.point, new Vec2(56, 181), "1");

        e = q.pop();
        Assert.AreEqual(e.point, new Vec2(100.79403384562251, 233.41363754101192));
        Assert.AreEqual(e.otherEvent.point, new Vec2(16, 282), "2");

        e = q.pop();
        Assert.AreEqual(e.point, new Vec2(100.79403384562251, 233.41363754101192));
        Assert.AreEqual(e.otherEvent.point, new Vec2(153, 203.5), "3");

        e = q.pop();
        Assert.AreEqual(e.point, new Vec2(100.79403384562251, 233.41363754101192));
        Assert.AreEqual(e.otherEvent.point, new Vec2(153, 294.5), "4");
    }
    internal class Interval
    {
        Vec2 l;
        Vec2 r;
        bool inOut;
        bool otherInOut;
        bool inResult;
        Interval prevInResult;
        internal Interval(Vec2 l, Vec2 r, bool inOut, bool otherInOut, bool inResult, Interval prevInResult)
        {
            this.l = l;
            this.r = r;
            this.inOut = inOut;
            this.otherInOut = otherInOut;
            this.inResult = inResult;
            this.prevInResult = prevInResult;
        }
    }

    [Test]
    public void TestPossibleInteractions2Polygons()
    {
        var s = subject;
        var c = clipping;

        var bbox = (new Vec2(Double.PositiveInfinity, Double.PositiveInfinity), new Vec2(Double.NegativeInfinity, Double.NegativeInfinity));
        var q = fillQueue(s, c, ref bbox, ref bbox, 0);
        var p0 = new Vec2(16, 282);
        var p1 = new Vec2(298, 359);
        var p2 = new Vec2(156, 203.5);

        var te = new SweepEvent(p0, true, null, true);
        var te2 = new SweepEvent(p1, false, te, false);
        te.otherEvent = te2;

        var te3 = new SweepEvent(p0, true, null, true);
        var te4 = new SweepEvent(p2, true, te3, false);
        te3.otherEvent = te4;

        var tr = new SplayTree<SweepEvent, int>(compareSegments);

        Assert.IsTrue(tr.insert(te, 0) is not null, "insert");
        Assert.IsTrue(tr.insert(te3, 0) is not null, "insert");

        Assert.AreEqual(tr.find(te).key, te);
        Assert.AreEqual(tr.find(te3).key, te3);

        Assert.AreEqual(compareSegments(te, te3), 1);
        Assert.AreEqual(compareSegments(te3, te), -1);

        var segments = subdivideSegments(q, s, c, bbox, bbox, 0);
        var leftSegments = new List<SweepEvent>();
        for (var i = 0; i < segments.Count; i++)
        {
            if (segments[i].left)
            {
                leftSegments.Add(segments[i]);
            }
        }

        Assert.AreEqual(leftSegments.Count, 11);

        var E = new Vec2(16, 282);
        var I = new Vec2(100.79403384562252, 233.41363754101192);
        var G = new Vec2(298, 359);
        var C = new Vec2(153, 294.5);
        var J = new Vec2(203.36313843035356, 257.5101243166895);
        var F = new Vec2(153, 203.5);
        var D = new Vec2(56, 181);
        var A = new Vec2(108.5, 120);
        var B = new Vec2(241.5, 229.5);


        var intervals = new Dictionary<string, Interval>();
        intervals["EI"] = new Interval(E, I, false, true, false, null);
        intervals["IF"] = new Interval(I, F, false, false, true, null);
        intervals["FJ"] = new Interval(F, J, false, false, true, null);
        intervals["JG"] = new Interval(J, G, false, true, false, null);
        intervals["EG"] = new Interval(E, G, true, true, false, null);
        intervals["DA"] = new Interval(D, A, false, true, false, null);
        intervals["AB"] = new Interval(A, B, false, true, false, null);
        intervals["JB"] = new Interval(J, B, true, true, false, null);
        intervals["CJ"] = new Interval(C, J, true, false, true, new Interval(F, J, false, false, false, null));
        intervals["IC"] = new Interval(I, C, true, false, true, new Interval(I, F, false, false, false, null));
        intervals["DI"] = new Interval(D, I, true, true, false, null);

        /*
        function checkContain(interval)
        {
            var data = intervals[interval];
            for (var x = 0; x < leftSegments.length; x++)
            {
                var seg = leftSegments[x];
                if (equals(seg.point, data.l) &&
                   equals(seg.otherEvent.point, data.r) &&
                   seg.inOut === data.inOut &&
                   seg.otherInOut === data.otherInOut &&
                   seg.inResult === data.inResult &&
                   ((seg.prevInResult === null && data.prevInResult === null) ||
                    (equals(seg.prevInResult.point, data.prevInResult.l) &&
                    equals(seg.prevInResult.otherEvent.point, data.prevInResult.r))))
                {
                    Assert.Pass(interval);
                    return;
                }
            }
            Assert.Fail(interval);
        }

        Object.keys(intervals).forEach((key) => checkContain(key));
    */
    }
}