namespace CSharpCAD;

internal static partial class Geom2Booleans
{

    internal static List<SweepEvent> subdivideSegments(TinyQueue evtQueue, List<List<List<Vec2>>> subject,
      List<List<List<Vec2>>> clipping, (Vec2, Vec2)sbbox, (Vec2, Vec2)cbbox, int operation)
    {
        var sweepLine = new SplayTree<SweepEvent, int?>(compareSegments);
        var sortedEvents = new List<SweepEvent>();

        var (smin, smax) = sbbox;
        var (cmin, cmax) = cbbox;
        var rightbound = Math.Min(smax.X, cmax.X);

        Node<SweepEvent, int?>? prev = null, next = null, begin = null;

        while (evtQueue.length != 0)
        {
            var evt = evtQueue.pop();
            sortedEvents.Add(evt);

            // optimization by bboxes for intersection and difference goes here
            if ((operation == INTERSECTION && evt.point.X > rightbound) ||
                (operation == DIFFERENCE && evt.point.X > smax.X))
            {
                break;
            }

            if (evt.left)
            {
                next = prev = sweepLine.insert(evt, null);
                begin = sweepLine.minNode(sweepLine.root);

                if (prev != begin) prev = sweepLine.prev(prev);
                else prev = null;

                next = sweepLine.next(next);

                var prevEvent = prev is not null ? prev.key : null;
                SweepEvent? prevprevEvent = null;
                computeFields(evt, prevEvent, operation);
                if (next is not null)
                {
                    if (possibleIntersection(evt, next.key, evtQueue) == 2)
                    {
                        computeFields(evt, prevEvent, operation);
                        computeFields(next.key, evt, operation);
                    }
                }

                if (prev is not null)
                {
                    if (possibleIntersection(prev.key, evt, evtQueue) == 2)
                    {
                        var prevprev = prev;
                        if (prevprev != begin) prevprev = sweepLine.prev(prevprev);
                        else prevprev = null;

                        prevprevEvent = prevprev is not null ? prevprev.key : null;
                        computeFields(prevEvent, prevprevEvent, operation);
                        computeFields(evt, prevEvent, operation);
                    }
                }
            }
            else
            {
                evt = evt.otherEvent;
                next = prev = sweepLine.find(evt);

                if (prev is not null && next is not null)
                {

                    if (prev != begin) prev = sweepLine.prev(prev);
                    else prev = null;

                    next = sweepLine.next(next);
                    sweepLine.remove(evt);

                    if (next is not null && prev is not null)
                    {
                        possibleIntersection(prev.key, next.key, evtQueue);
                    }
                }
            }
        }
        return sortedEvents;
    }
}