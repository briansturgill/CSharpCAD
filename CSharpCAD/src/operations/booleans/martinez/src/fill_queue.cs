namespace CSharpCAD;


internal static partial class Geom2Booleans
{
    internal static int contourId = 0;


    internal static void processPolygon(List<Vec2> contourOrHole, bool isSubject, int depth, TinyQueue Q,
      ref (Vec2, Vec2) bbox, bool isExteriorRing)
    {
        var len = contourOrHole.Count-1;
        var min = contourOrHole[0];
        var max = min;
        for (var i = 0; i < len; i++)
        {
            var s1 = contourOrHole[i];
            var s2 = contourOrHole[i + 1];
            var e1 = new SweepEvent(s1, false, null, isSubject);
            var e2 = new SweepEvent(s2, false, e1, isSubject);
            e1.otherEvent = e2;

            if (Equalish(s1.X, s2.X) && Equalish(s1.Y, s2.Y))
            {
                continue; // skip collapsed edges, or it breaks
            }

            e1.outputContourId = e2.outputContourId = depth;
            if (!isExteriorRing)
            {
                e1.isExteriorRing = false;
                e2.isExteriorRing = false;
            }
            if (compareEvents(e1, e2) > 0)
            {
                e2.left = true;
            }
            else
            {
                e1.left = true;
            }

            min = min.Min(s1);
            max = max.Max(s1);

            // Pushing it so the queue is sorted from left to right,
            // with object on the left having the highest priority.
            Q.push(e1);
            Q.push(e2);
        }
        bbox = (min, max);
    }


    internal static TinyQueue fillQueue(List<List<List<Vec2>>> subject, List<List<List<Vec2>>> clipping,
      ref (Vec2, Vec2) sbbox, ref (Vec2, Vec2) cbbox, int operation)
    {
        var eventQueue = new TinyQueue();

        var ii = subject.Count;
        for (var i = 0; i < ii; i++)
        {
            var polygonSet = subject[i];
            var jj = polygonSet.Count;
            for (var j = 0; j < jj; j++)
            {
                var isExteriorRing = j == 0;
                if (isExteriorRing) contourId++;
                processPolygon(polygonSet[j], true, contourId, eventQueue, ref sbbox, isExteriorRing);
            }
        }

        ii = clipping.Count;
        for (var i = 0; i < ii; i++)
        {
            var polygonSet = clipping[i];
            var jj = polygonSet.Count;
            for (var j = 0; j < jj; j++)
            {
                var isExteriorRing = j == 0;
                if (operation == DIFFERENCE) isExteriorRing = false;
                if (isExteriorRing) contourId++;
                processPolygon(polygonSet[j], false, contourId, eventQueue, ref cbbox, isExteriorRing);
            }
        }

        return eventQueue;
    }
}