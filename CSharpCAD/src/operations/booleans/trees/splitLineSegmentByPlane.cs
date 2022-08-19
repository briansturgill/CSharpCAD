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

        //polys = Modifiers.insertTjunctions(polys);

        polys = Modifiers.TriangulatePolygons(C.EPS, polys);

        SplitAddedPoints.Clear();

        return polys;
    }

    static string pl(List<Vec3> lv)
    {
        string ret = "";
        foreach (var v in lv) ret += v + " ";
        return ret;
    }
}