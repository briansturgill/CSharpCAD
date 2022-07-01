namespace CSharpCAD;

public static partial class CSCAD
{
    private class pt
    {
        internal readonly Vec2 point;
        internal readonly double angle;
        internal readonly double distSq;
        internal pt(Vec2 point, double angle, double distSq)
        {
            this.point = point;
            this.angle = angle;
            this.distSq = distSq;
        }
    };
    /*
     * Create a convex hull of the given geom2 geometries.
     * Uses https://en.wikipedia.org/wiki/Graham_scan
     */
    internal static Geom2 HullGeom2(params Geom2[] geometries)
    {
        // Extract unique points from the geometries.
        // To avoid a second pass, also determine the minimum point.
        var uniquePoints = new HashSet<Vec2>();
        var min = new Vec2(double.PositiveInfinity, double.PositiveInfinity);
        foreach (var g in geometries)
        {
            var sides = g.ToSides();
            foreach (var side in sides)
            {
                var point = side.v0;
                uniquePoints.Add(point);
                if (point.Y < min.Y || (point.Y == min.Y && point.X < min.X))
                {
                    min = point;
                }
            }
        }

        // Returned "angle" is really 1/tan (inverse of slope) made negative to increase with angle.
        // This function is strictly for sorting in this algorithm.
        double fakeAtan2(double y, double x)
        {
            // The "if" is a special case for when the minimum vector found in loop above is present.
            // We need to insure that it sorts as the minimum point. Otherwise this becomes NaN.
            if (y == 0 && x == 0) { return double.NegativeInfinity; }
            return -(x / y);
        }

        // Gather information for sorting by polar coordinates.
        var points = new List<pt>(uniquePoints.Count);
        foreach (var v in uniquePoints)
        {
            // Use of fakeAtan2 avoids use of Math.Atan2 which slows things down.
            // A simple timing test suggests it saves about 10% of total time.
            var angle = fakeAtan2((v.Y - min.Y), (v.X - min.X));
            //var angle = Math.Atan2((v.Y - min.Y), (v.X - min.X));
            var distSq = min.SquaredDistance(v);
            points.Add(new pt(v, angle, distSq));
        }
        points.Sort((pt pt1, pt pt2) => pt1.angle < pt2.angle ? -1 : pt1.angle > pt2.angle ? 1 :
            pt1.distSq < pt2.distSq ? -1 : pt1.distSq > pt2.distSq ? 1 : 0);

        // ccw returns:  < 0 clockwise, 0 colinear, > 0 counter-clockwise.
        double ccw(Vec2 v1, Vec2 v2, Vec2 v3) => (v2.X - v1.X) * (v3.Y - v1.Y) - (v2.Y - v1.Y) * (v3.X - v1.X);
        var stack = new List<Vec2>(); // Start with empty stack
        foreach (var point in points)
        {
            var cnt = stack.Count;
            while (cnt > 1 && ccw(stack[cnt - 2], stack[cnt - 1], point.point) <= C.EPSILON)
            {
                stack.RemoveAt(stack.Count - 1); // Pop - gets rid of colinear and interior (clockwise) points.
                cnt = stack.Count;
            }
            stack.Add(point.point); // Push
        }

        if (stack.Count < 3) return new Geom2();

        return new Geom2(stack);
    }
}