namespace CSharpCAD;

#nullable disable

internal static partial class Geom2Booleans
{
    private static List<List<List<Vec2>>> EMPTY = new List<List<List<Vec2>>>(0);

    internal static List<List<List<Vec2>>> trivialOperation(List<List<List<Vec2>>> subject, List<List<List<Vec2>>>clipping, int operation)
    {
        List<List<List<Vec2>>> result = null;
        if (subject.Count * clipping.Count == 0)
        {
            if (operation == INTERSECTION)
            {
                result = EMPTY;
            }
            else if (operation == DIFFERENCE)
            {
                result = subject;
            }
            else if (operation == UNION || operation == XOR)
            {
                result = (subject.Count == 0) ? clipping : subject;
            }
        }
        return result;
    }


    internal static List<List<List<Vec2>>> compareBBoxes(List<List<List<Vec2>>> subject, List<List<List<Vec2>>> clipping,
      (Vec2, Vec2) sbbox, (Vec2, Vec2)cbbox, int operation)
    {
        List<List<List<Vec2>>> result = null;
        var (smin, smax) = sbbox;
        var (cmin, cmax) = cbbox;
        if (smin.X > cmax.X ||
            cmin.X > smax.X ||
            smin.Y > cmax.Y ||
            cmin.Y > smax.Y)
        {
            if (operation == INTERSECTION)
            {
                result = EMPTY;
            }
            else if (operation == DIFFERENCE)
            {
                result = subject;
            }
            else if (operation == UNION ||
                       operation == XOR)
            {
                result = subject.Concat(clipping).ToList();
            }
        }
        return result;
    }


    internal static List<List<List<Vec2>>> boolean(List<List<List<Vec2>>> subject, List<List<List<Vec2>>> clipping, int operation)
    {
        var trivial = trivialOperation(subject, clipping, operation);
        if (trivial is not null)
        {
            return trivial == EMPTY ? null : trivial;
        }
        var sbbox = (new Vec2(Double.PositiveInfinity, Double.PositiveInfinity), new Vec2(Double.NegativeInfinity, Double.NegativeInfinity));
        var cbbox = (new Vec2(Double.PositiveInfinity, Double.PositiveInfinity), new Vec2(Double.NegativeInfinity, Double.NegativeInfinity));

        // console.time('fill queue');
        var eventQueue = fillQueue(subject, clipping, ref sbbox, ref cbbox, operation);
        //console.timeEnd('fill queue');

        trivial = compareBBoxes(subject, clipping, sbbox, cbbox, operation);
        if (trivial is not null)
        {
            return trivial == EMPTY ? null : trivial;
        }
        // console.time('subdivide edges');
        var sortedEvents = subdivideSegments(eventQueue, subject, clipping, sbbox, cbbox, operation);
        //console.timeEnd('subdivide edges');

        // console.time('connect vertices');
        var contours = connectEdges(sortedEvents);
        //console.timeEnd('connect vertices');

        // Convert contours to polygons
        var polygons = new List<List<List<Vec2>>>();
        for (var i = 0; i < contours.Count; i++)
        {
            var contour = contours[i];
            if (contour.isExterior())
            {
                // The exterior ring goes first
                var rings = new List<List<Vec2>>{ contour.points };
                // Followed by holes if any
                for (var j = 0; j < contour.holeIds.Count; j++)
                {
                    var holeId = contour.holeIds[j];
                    var c = contours[holeId].points;
                    c.Reverse();
                    rings.Add(c);
                }
                polygons.Add(rings);
            }
        }

        return polygons;
    }
}