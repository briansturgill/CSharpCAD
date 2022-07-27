namespace CSharpCAD;

#nullable disable

internal static partial class Geom2Booleans
{
    internal static List<SweepEvent> orderEvents(List<SweepEvent> sortedEvents)
    {
        var len = sortedEvents.Count;
        var resultEvents = new List<SweepEvent>(len);
        for (var i = 0; i < len; i++)
        {
            var evt = sortedEvents[i];
            if ((evt.left && evt.inResult()) ||
              (!evt.left && evt.otherEvent.inResult()))
            {
                resultEvents.Add(evt);
            }
        }
        // Due to overlapping edges the resultEvents array can be not wholly sorted
        var sorted = false;
        while (!sorted)
        {
            sorted = true;
            len = resultEvents.Count;
            for (var i = 0; i < len; i++)
            {
                if ((i + 1) < len &&
                  compareEvents(resultEvents[i], resultEvents[i + 1]) == 1)
                {
                    var tmp = resultEvents[i];
                    resultEvents[i] = resultEvents[i + 1];
                    resultEvents[i + 1] = tmp;
                    sorted = false;
                }
            }
        }

        len = resultEvents.Count;
        for (var i = 0; i < len; i++)
        {
            var evt = resultEvents[i];
            evt.otherPos = i;
        }

        // imagine, the right event is found in the beginning of the queue,
        // when his left counterpart is not marked yet
        len = resultEvents.Count;
        for (var i = 0; i < len; i++)
        {
            var evt = resultEvents[i];
            if (!evt.left)
            {
                var tmp = evt.otherPos;
                evt.otherPos = evt.otherEvent.otherPos;
                evt.otherEvent.otherPos = tmp;
            }
        }

        return resultEvents;
    }

    internal static int nextPos(int pos, List<SweepEvent> resultEvents, bool[] processed, int origPos)
    {
        var newPos = pos + 1;
        var p = resultEvents[pos].point;
        Vec2 p1 = new Vec2();
        var length = resultEvents.Count;

        if (newPos < length)
            p1 = resultEvents[newPos].point;

        while (newPos < length && equals(p1, p))
        {
            if (!processed[newPos])
            {
                return newPos;
            }
            else
            {
                newPos++;
            }
            if (newPos < length)
            {
                p1 = resultEvents[newPos].point;
            }
        }

        newPos = pos - 1;

        while (processed[newPos] && newPos > origPos)
        {
            newPos--;
        }

        return newPos;
    }


    internal static Contour initializeContourFromContext(SweepEvent evt, List<Contour> contours, int contourId)
    {
        var contour = new Contour();
        if (evt.prevInResult is not null)
        {
            var prevInResult = evt.prevInResult;
            // Note that it is valid to query the "previous in result" for its output contour id,
            // because we must have already processed it (i.e., assigned an output contour id)
            // in an earlier iteration, otherwise it wouldn't be possible that it is "previous in
            // result".
            var lowerContourId = prevInResult.outputContourId;
            var lowerResultTransition = prevInResult.resultTransition;
            if (lowerResultTransition > 0)
            {
                // We are inside. Now we have to check if the thing below us is another hole or
                // an exterior contour.
                var lowerContour = contours[lowerContourId];
                if (lowerContour.holeOf != -1)
                {
                    // The lower contour is a hole => Connect the new contour as a hole to its parent,
                    // and use same depth.
                    var parentContourId = lowerContour.holeOf;
                    contours[parentContourId].holeIds.Add(contourId);
                    contour.holeOf = parentContourId;
                    contour.depth = contours[lowerContourId].depth;
                }
                else
                {
                    // The lower contour is an exterior contour => Connect the new contour as a hole,
                    // and increment depth.
                    contours[lowerContourId].holeIds.Add(contourId);
                    contour.holeOf = lowerContourId;
                    contour.depth = contours[lowerContourId].depth + 1;
                }
            }
            else
            {
                // We are outside => this contour is an exterior contour of same depth.
                contour.holeOf = -1;
                contour.depth = contours[lowerContourId].depth;
            }
        }
        else
        {
            // There is no lower/previous contour => this contour is an exterior contour of depth 0.
            contour.holeOf = -1;
            contour.depth = 0;
        }
        return contour;
    }

    internal static List<Contour> connectEdges(List<SweepEvent> sortedEvents)
    {
        var resultEvents = orderEvents(sortedEvents);

        var len = resultEvents.Count;
        var processed = new bool[len]; // "false"-filled array
        var contours = new List<Contour>();

        for (var i = 0; i < len; i++)
        {

            if (processed[i])
            {
                continue;
            }

            var contourId = contours.Count;
            var contour = initializeContourFromContext(resultEvents[i], contours, contourId);

            // Helper function that combines marking an event as processed with assigning its output contour ID
            void markAsProcessed(int pos)
            {
                processed[pos] = true;
                if (pos < resultEvents.Count && resultEvents[pos] is not null)
                {
                    resultEvents[pos].outputContourId = contourId;
                }
            };

            var pos = i;
            var origPos = i;

            var initial = resultEvents[i].point;
            contour.points.Add(initial);

            /* eslint no-constant-condition: "off" */
            while (true)
            {
                markAsProcessed(pos);

                pos = resultEvents[pos].otherPos;

                markAsProcessed(pos);
                contour.points.Add(resultEvents[pos].point);

                pos = nextPos(pos, resultEvents, processed, origPos);

                if (pos == origPos || pos >= resultEvents.Count || resultEvents[pos] is null)
                {
                    break;
                }
            }

            contours.Add(contour);
        }

        return contours;
    }
}
