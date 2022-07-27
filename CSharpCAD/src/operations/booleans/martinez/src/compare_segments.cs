namespace CSharpCAD;

internal static partial class Geom2Booleans
{

    internal static int compareSegments(SweepEvent? le1, SweepEvent? le2)
    {
        if (le1 is null || le2 is null) throw new ArgumentException("Should not be null");
        if (le1 == le2) return 0;

        // Segments are not collinear
        if (signedArea(le1.point, le1.otherEvent.point, le2.point) != 0 ||
          signedArea(le1.point, le1.otherEvent.point, le2.otherEvent.point) != 0)
        {

            // If they share their left endpoint use the right endpoint to sort
            if (equals(le1.point, le2.point)) return le1.isBelow(le2.otherEvent.point) ? -1 : 1;

            // Different left endpoint: use the left endpoint to sort
            if (le1.point.X == le2.point.X) return le1.point.Y < le2.point.Y ? -1 : 1;

            // has the line segment associated to e1 been inserted
            // into S after the line segment associated to e2 ?
            if (compareEvents(le1, le2) == 1) return le2.isAbove(le1.point) ? -1 : 1;

            // The line segment associated to e2 has been inserted
            // into S after the line segment associated to e1
            return le1.isBelow(le2.point) ? -1 : 1;
        }

        if (le1.isSubject == le2.isSubject)
        { // same polygon
            var p1 = le1.point;
            var p2 = le2.point;
            if (equals(le1.point, le2.point))
            {
                p1 = le1.otherEvent.point; p2 = le2.otherEvent.point;
                if (equals(p1, p2)) return 0;
                else return le1.outputContourId > le2.outputContourId ? 1 : -1;
            }
        }
        else
        { // Segments are collinear, but belong to separate polygons
            return le1.isSubject ? -1 : 1;
        }

        return compareEvents(le1, le2) == 1 ? 1 : -1;
    }
}
