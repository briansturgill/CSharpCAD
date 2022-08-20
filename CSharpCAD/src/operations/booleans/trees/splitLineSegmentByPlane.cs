namespace CSharpCAD;

public static partial class CSCAD
{
    // I really hate making this a static, but the existing design makes it hard to do anything otherwise.
    internal static Dictionary<(Vec3, Vec3), List<Vec3>> SplitAddedPoints = new Dictionary<(Vec3, Vec3), List<Vec3>>();

    private static Vec3 SplitLineSegmentByPlane(Plane plane, Vec3 p1, Vec3 p2)
    {
        var edge = (p1, p2);
        var revedge = (p2, p1);
        var direction = p2.Subtract(p1);
        var lambda = (plane.W - plane.Normal.Dot(p1)) / plane.Normal.Dot(direction);
        if (double.IsNaN(lambda)) lambda = 0;
        if (lambda > 1) lambda = 1;
        if (lambda < 0) lambda = 0;

        direction = direction.Scale(lambda);
        var intersectionPoint = p1.Add(direction);

        if (intersectionPoint.IsNearlyEqual(p1)) intersectionPoint = p1;
        else if (intersectionPoint.IsNearlyEqual(p2)) intersectionPoint = p2;

        if (SplitAddedPoints.ContainsKey(revedge))
        {
            var pts = SplitAddedPoints[revedge];
            var len = pts.Count;
            for (var i = 0; i < len; i++)
            {
                var pt = pts[i];
                if (pt.IsNearlyEqual(intersectionPoint))
                {
                    intersectionPoint = pt;
                    break;
                }
            }
        }

        if (SplitAddedPoints.ContainsKey(edge))
        {
            var pts = SplitAddedPoints[edge];
            var len = pts.Count;
            int i;
            for (i = 0; i < len; i++)
            {
                var pt = pts[i];
                if (pt.IsNearlyEqual(intersectionPoint))
                {
                    intersectionPoint = pt;
                    break;
                }
            }
            if (i == len) pts.Add(intersectionPoint);
        }
        else
        {
            SplitAddedPoints[edge] = new List<Vec3> { intersectionPoint };
        }

        return intersectionPoint;
    }

    internal static Poly3[] Fix3DBooBoos(string tag, List<Poly3> _polys)
    {
        var polys = _polys.ToArray();

        MakePointsStable(tag, polys);

        polys = Retessellate(new Geom3(polys)).ToPolygons();

        //polys = Modifiers.InsertTjunctions(polys);

        //polys = triangulate(polys);

        SplitAddedPoints.Clear();

        return polys;
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