namespace CSharpCAD;

internal static partial class Geom2Booleans
{
    /*
     * @param  {SweepEvent} se
     * @param  {Array.<Number>} p
     * @param  {Queue} queue
     * @return {Queue}
     */
    internal static TinyQueue divideSegment(SweepEvent se, Vec2 p, TinyQueue queue)
    {
        var r = new SweepEvent(p, false, se, se.isSubject);
        var l = new SweepEvent(p, true, se.otherEvent, se.isSubject);

        if (equals(se.point, se.otherEvent.point))
        {
            Console.Error.WriteLine("What is that, a collapsed segment?", se);
        }

        r.outputContourId = l.outputContourId = se.outputContourId;

        // avoid a rounding error. The left event would be processed after the right event
        if (compareEvents(l, se.otherEvent) > 0)
        {
            se.otherEvent.left = true;
            l.left = false;
        }

        // avoid a rounding error. The left event would be processed after the right event
        // if (compareEvents(se, r) > 0) {}

        se.otherEvent.otherEvent = l;
        se.otherEvent = r;

        queue.push(l);
        queue.push(r);

        return queue;
    }
}