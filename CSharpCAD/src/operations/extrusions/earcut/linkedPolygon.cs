namespace CSharpCAD;

public static partial class CSCAD
{
    internal static partial class Earcut
    {
#nullable disable
        /*
         * create a circular doubly linked list from polygon points in the specified winding order
         */
        public static Node LinkedPolygon(double[] data, int start, int end, int dim, bool clockwise)
        {
            Node last = null;

            if (clockwise == (SignedArea(data, start, end, dim) > 0))
            {
                for (var i = start; i < end; i += dim)
                {
                    last = InsertNode(i, data[i], data[i + 1], last);
                }
            }
            else
            {
                for (var i = end - dim; i >= start; i -= dim)
                {
                    last = InsertNode(i, data[i], data[i + 1], last);
                }
            }

            if (Equals(last, last.next))
            {
                RemoveNode(last);
                last = last.next;
            }

            return last;
        }

        /*
         * eliminate colinear or duplicate points
         */
        internal static Node FilterPoints(Node start, Node end = null)
        {
            if (start is null) return start;
            if (end is null) end = start;

            var p = start;
            bool again;
            do
            {
                again = false;

                if (!p.steiner && (Equals(p, p.next) || AreaOfT(p.prev, p, p.next) == 0))
                {
                    RemoveNode(p);
                    p = end = p.prev;
                    if (p == p.next) break;
                    again = true;
                }
                else
                {
                    p = p.next;
                }
            } while (again || p != end);


            return end;
        }

        /*
         * go through all polygon nodes and cure small local self-intersections
         */
        internal static Node CureLocalIntersections(Node start, List<int> triangles, int dim)
        {
            var p = start;
            do
            {
                if (p is null || p.next is null)
                {
                    return null;
                }
                var a = p.prev;
                var b = p.next.next;

                if (!Equals(a, b) && Intersects(a, p, p.next, b) && LocallyInside(a, b) && LocallyInside(b, a))
                {
                    triangles.Add(a.i / dim); // Probably not a float error... external indicies are /2.
                    triangles.Add(p.i / dim);
                    triangles.Add(b.i / dim);

                    // remove two nodes involved
                    RemoveNode(p);
                    RemoveNode(p.next);
                    p = start = b;
                }
                p = p.next;
            } while (p != start);

            return FilterPoints(p);
        }

        /*
         * check if a polygon diagonal intersects any polygon segments
         */
        internal static bool IntersectsPolygon(Node a, Node b)
        {
            var p = a;
            do
            {
                if (p.i != a.i && p.next.i != a.i && p.i != b.i && p.next.i != b.i &&
                    Intersects(p, p.next, a, b)) return true;
                p = p.next;
            } while (p != a);
            return false;
        }

        /*
         * check if a polygon diagonal is locally inside the polygon
         */
        internal static bool LocallyInside(Node a, Node b) => AreaOfT(a.prev, a, a.next) < 0
          ? AreaOfT(a, b, a.next) >= 0 && AreaOfT(a, a.prev, b) >= 0
          : AreaOfT(a, b, a.prev) < 0 || AreaOfT(a, a.next, b) < 0;

/*
 * check if the middle point of a polygon diagonal is inside the polygon
 */
internal static bool MiddleInside(Node a, Node b)
        {
            var p = a;
            var inside = false;
            var px = (a.x + b.x) / 2;
            var py = (a.y + b.y) / 2;
            do
            {
                if (((p.y > py) != (p.next.y > py)) && p.next.y != p.y &&
                    (px < (p.next.x - p.x) * (py - p.y) / (p.next.y - p.y) + p.x)) { inside = !inside; }
                p = p.next;
            } while (p != a);

            return inside;
        }

        /*
         * link two polygon vertices with a bridge; if the vertices belong to the same ring, it splits polygon into two
         * if one belongs to the outer ring and another to a hole, it merges it into a single ring
         */
        internal static Node SplitPolygon(Node a, Node b)
        {
            var a2 = new Node(a.i, a.x, a.y);
            var b2 = new Node(b.i, b.x, b.y);
            var an = a.next;
            var bp = b.prev;

            a.next = b;
            b.prev = a;

            a2.next = an;
            an.prev = a2;

            b2.next = a2;
            a2.prev = b2;

            bp.next = b2;
            b2.prev = bp;

            return b2;
        }

        /*
         * check if a diagonal between two polygon nodes is valid (lies in polygon interior)
         */
        internal static bool IsValidDiagonal(Node a, Node b) => a.next.i != b.i &&
            a.prev.i != b.i &&
            !IntersectsPolygon(a, b) && // doesn't intersect other edges
            (
              LocallyInside(a, b) && LocallyInside(b, a) && MiddleInside(a, b) && // locally visible
                (AreaOfT(a.prev, a, b.prev) != 0 || AreaOfT(a, b.prev, b) != 0) || // does not create opposite-facing sectors
                Equals(a, b) && AreaOfT(a.prev, a, a.next) > 0 && AreaOfT(b.prev, b, b.next) > 0
            );

        /*
         * check if two segments intersect
         */
        internal static bool Intersects(Node p1, Node q1, Node p2, Node q2)
        {
            var o1 = Math.Sign(AreaOfT(p1, q1, p2));
            var o2 = Math.Sign(AreaOfT(p1, q1, q2));
            var o3 = Math.Sign(AreaOfT(p2, q2, p1));
            var o4 = Math.Sign(AreaOfT(p2, q2, q1));

            if (o1 != o2 && o3 != o4) return true; // general case

            if (o1 == 0 && OnSegment(p1, p2, q1)) return true; // p1, q1 and p2 are colinear and p2 lies on p1q1
            if (o2 == 0 && OnSegment(p1, q2, q1)) return true; // p1, q1 and q2 are colinear and q2 lies on p1q1
            if (o3 == 0 && OnSegment(p2, p1, q2)) return true; // p2, q2 and p1 are colinear and p1 lies on p2q2
            if (o4 == 0 && OnSegment(p2, q1, q2)) return true; // p2, q2 and q1 are colinear and q1 lies on p2q2

            return false;
        }

        /*
         * for colinear points p, q, r, check if point q lies on segment pr
         */
        internal static bool OnSegment(Node p, Node q, Node r) => q.x <= Math.Max(p.x, r.x) &&
            q.x >= Math.Min(p.x, r.x) && q.y <= Math.Max(p.y, r.y) && q.y >= Math.Min(p.y, r.y);


        internal static double SignedArea(double[] data, int start, int end, int dim)
        {
            double sum = 0;
            for (int i = start, j = end - dim; i < end; i += dim)
            {
                sum += (data[j] - data[i]) * (data[i + 1] + data[j + 1]);
                j = i;
            }

            return sum;
        }

        /*
         * check if two points are equal
         */
        internal static bool Equals(Node p1, Node p2) {
          if (p1 is null || p2 is null) return false;
          return p1.x == p2.x && p1.y == p2.y;
        }
#nullable enable
    }
}