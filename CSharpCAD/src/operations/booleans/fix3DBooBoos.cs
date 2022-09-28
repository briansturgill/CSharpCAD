namespace CSharpCAD;

public static partial class CSCAD
{

    internal static Poly3[] Fix3DBooBoos(string tag, List<Poly3> _polys)
    {
        _polys = Retessellate(_polys);
        //var epsilon = new Geom3(_polys.ToArray()).MeasureEpsilon();
        //_polys = Modifiers.snapPolygons(epsilon, _polys.ToArray()).ToList();
        //_polys = Modifiers.InsertTjunctions(_polys.ToArray()).ToList();
        //_polys = Modifiers.TriangulatePolygons(epsilon, _polys.ToArray()).ToList();

        var ret = _polys.ToArray();

        MakePointsStable(tag, ret);

        return ret;
    }

    internal static Geom3 DoGeneralize(Geom3 geometry)
    {
        var epsilon = geometry.MeasureEpsilon();
        var polygons = geometry.ToPolygons();
        polygons = Modifiers.snapPolygons(epsilon, polygons);
        polygons = Modifiers.InsertTjunctions(polygons);
        polygons = Modifiers.TriangulatePolygons(epsilon, polygons);
        return new Geom3(polygons, Color: geometry.Color);
    }

    internal static void CorrectDroppedJunctions(List<List<Vec3>> polys)
    {
        // Find unmatched edges.
        var edges = new Dictionary<(Vec3, Vec3), int>(polys.Count);
        var polysLen = polys.Count;
        for (var i = 0; i < polysLen; i++)
        {
            var poly = polys[i];
            var plen = poly.Count;
            var prev = poly[plen - 1];
            foreach (var cur in poly)
            {
                var edge = (prev, cur);
                var revEdge = (cur, prev);
                if (edges.ContainsKey(revEdge))
                {
                    edges.Remove(revEdge);
                    continue; // We have matched.
                }
                edges[edge] = i;
                prev = cur;
            }
        }
        // Here edges contains those edges that are unmatched.

        // The BSP algorithm incorrectly removes some edges of inserted split (intersection) points, we need to replace them.
        // Basically a situation like this regularly occurs: one set of edges is A, B, C, D, E. But on the reverse side,
        // one only has edge E, A. We need to add D, C, B in the middle of that edge.
        // (The middle set of edges are not always 3, they can range from 1 to many. 1 or 2 being quite common.)
        // The points A, B, C, D, and E are all colinear.

        // See also the comments saved in the old file: insertTjunctions.cs
        // I believe insertTjunctions.cs mischaracterizes the issue.
        // I believe this is really a flaw in the BSP-based algorithm.
        // Basically the algorithm inserts the intersection point on both the edge and the reverse edge,
        // but further on incorrrectly removes one side of the insersection points. It just doesn't have enough information to
        // know to keep those points around.


        // Gather edges accessible by endpoint.
        var edgeStarts = new Dictionary<Vec3, List<Vec3>>(edges.Count);
        foreach (var (start, end) in edges.Keys)
        {
            if (!edgeStarts.ContainsKey(start))
            {
                edgeStarts[start] = new List<Vec3>();
            }
            edgeStarts[start].Add(end);
        }

        var edge_list = edges.Keys.ToList();

        // Sort edges such that the edge with greatest SquaredDistance is first.
        // This ensures that edges that have missing points appear before the edges that contain those points.
        edge_list.Sort(((Vec3, Vec3) a, (Vec3, Vec3) b) =>
        {
            var (a_start, a_end) = a;
            var (b_start, b_end) = b;
            var bd = b_start.SquaredDistance(b_end);
            var ad = a_start.SquaredDistance(a_end);
            return bd.CompareTo(ad);
        });

        var allFixed = true;

        foreach (var edge_to_fix in edge_list)
        {
            Console.WriteLine($"etf: {edge_to_fix}");
            if (!edges.ContainsKey(edge_to_fix)) continue; // Already handled
            allFixed &= fixEdge(edge_to_fix, polys, edges, edgeStarts);
        }

        if (!allFixed)
        {
            Console.WriteLine($"Allfixed: {allFixed}");
            // LATER need to warn 3D Object is probably not manifold.
        }
    }

    // calcDiff == 0.0 means the three points are precisely colinear.
    private static double calcDiff(Vec3 start, Vec3 mid, Vec3 end)
    {
        return Math.Abs(start.SquaredDistance(end) - (start.SquaredDistance(mid) + mid.SquaredDistance(end)));
    }

    private static bool fixEdge((Vec3, Vec3) edge_to_fix, List<List<Vec3>> polys, Dictionary<(Vec3, Vec3), int> edges,
        Dictionary<Vec3, List<Vec3>> edgeStarts)
    {
        var (start, end) = edge_to_fix;
        var mid = new Vec3(Double.PositiveInfinity, 0, 0);
        var segEnd = end;

        while (start != mid)
        {
            // The small segments are reversed from the edge with missing junction(s).
            if (edgeStarts.ContainsKey(segEnd))
            {
                var eList = edgeStarts[segEnd];
                var leastDiff = Double.MaxValue;
                var leastDiffIdx = -1;

                var len = eList.Count;
                for (var i = 0; i < len; i++)
                {
                    var diff = calcDiff(start, eList[i], segEnd);
                    if (diff < leastDiff)
                    {
                        leastDiff = diff;
                        leastDiffIdx = i;
                    }
                }

                if (leastDiff >= C.EPS_SQUARED)
                {
                    return false;
                }

                mid = eList[leastDiffIdx];
                eList.RemoveAt(leastDiffIdx);

                if (eList.Count == 0)
                {
                    edgeStarts.Remove(segEnd);
                }

                if (mid != start)
                {
                    var pidx = edges[edge_to_fix];
                    var idx = polys[pidx].IndexOf(end);
                    polys[pidx].Insert(idx, mid);
                    Console.WriteLine($"poly {polys[pidx]}");
                    edges.Remove((segEnd, mid));
                }

                segEnd = mid;
            }
            else
            {
                return false;
            }
        }
        // start == mid
        edges.Remove((start, end));

        return true;
    }

    private static bool triangleIsTooSmall(Vec3[] pts)
    {
        bool isTooSmall(Vec3 p1, Vec3 p2)
        {
            return Math.Abs(p1.X - p2.X) <= C.EPS &&
                Math.Abs(p1.Y - p2.Y) <= C.EPS &&
                Math.Abs(p1.Z - p2.Z) <= C.EPS;
        }
        var pt0 = pts[0];
        var pt1 = pts[1];
        var pt2 = pts[2];

        return isTooSmall(pt0, pt1) || isTooSmall(pt1, pt2) || isTooSmall(pt2, pt0);
    }

    private static Poly3[] triangulate(Poly3[] polys)
    {
        var triangles = new List<Poly3>();
        foreach (var poly in polys)
        {
            var pts = poly.Vertices;
            var ptsLen = pts.Length;
            if (ptsLen < 3)
            {
                throw new Exception("Should not happen");
            }
            if (triangleIsTooSmall(pts))
            {
                if (GlobalParams.DebugOutput)
                {
                    Console.WriteLine("WARNING: Zero area triangle discarded (poly)");
                }
                continue;
            }
            if (ptsLen == 3)
            {
                triangles.Add(poly);
            }
            else
            {
                var p0 = pts[0];
                for (var i = 1; i < ptsLen - 1; i++)
                {
                    var newpoly = new Poly3(new Vec3[] { p0, pts[i], pts[i + 1] });
                    if (triangleIsTooSmall(newpoly.Vertices))
                    {
                        if (GlobalParams.DebugOutput)
                        {
                            Console.WriteLine("WARNING: Zero area triangle discarded (newpoly)");
                        }
                        continue;
                    }
                    triangles.Add(newpoly);
                }
            }
        }
        return triangles.ToArray();
    }
}