namespace CSharpCAD;

public static partial class CSCAD
{
    internal static Geom2 OffsetGeom2(Geom2 gobj, double delta = 1, Corners corners = Corners.Edge, int segments = 16)
    {
        var outlines = gobj.ToOutlines();
        var allSides = new List<Geom2.Side>();
        // convert the geometry to outlines, and generate offsets from each
        foreach (var outline in outlines)
        {
            var level = 0;
            foreach (var polygon in outlines)
            {
                level += ArePointsInside(outline, polygon);
            }
            var outside = (level % 2) == 0;

            var newOutline = OffsetFromPoints(outline, delta: outside ? delta : -delta, corners: corners, segments: segments, closed: true);
            var newSides = new Geom2(newOutline).ToSides();
            allSides.AddRange(newSides);
        }

        // create a composite geometry from the new outlines
        return new Geom2(allSides.ToArray());
    }

    /*
     * Determine if the given points are inside the given polygon.
     *
     * @param {Array} points - a list of points, where each point is an array with X and Y values
     * @param {poly2} polygon - a 2D polygon
     * @return {Integer} 1 if all points are inside, 0 if some or none are inside
     * @alias module:modeling/geometries/poly2.arePointsInside
     */
    internal static int ArePointsInside(Vec2[] points, Vec2[] polygon)
    {
        if (points.Length== 0) return 0; // nothing to check

        var vertices = polygon;
        if (vertices.Length < 3) return 0; // nothing can be inside an empty polygon

/*
        if (AreaVec2(polygon) < 0)
        {
            polygon.Reverse(); // CCW is required
        }
*/
        
        var sum = 0;
        foreach (var point in points)
        {
            sum += IsPointInside(point, vertices);
        }
        return sum == points.Length ? 1 : 0;
    }

    /*
     * Determine if the given point is inside the polygon.
     *
     * @see http://erich.realtimerendering.com/ptinpoly/ (Crossings Test)
     * @param {Array} point - an array with X and Y values
     * @param {Array} polygon - a list of points, where each point is an array with X and Y values
     * @return {Integer} 1 if the point is inside, 0 if outside
     */
    internal static int IsPointInside(Vec2 point, Vec2[] polygon)
    {
        var numverts = polygon.Length;

        var tx = point.X;
        var ty = point.Y;

        var vtx0 = polygon[numverts - 1];
        var vtx1 = polygon[0];

        var yflag0 = (vtx0.Y > ty);

        var insideFlag = 0;

        var i = 0;
        for (var idx = 0; idx < numverts-1; idx++) // CBS C# tranlation note... Stupid loop structure... just go with it!
        {
            /*
             * check if Y endpoints straddle (are on opposite sides) of point's Y
             * if so, +X ray could intersect this edge.
             */
            var yflag1 = (vtx1.Y > ty);
            if (yflag0 != yflag1)
            {
                /*
                 * check if X endpoints are on same side of the point's X
                 * if so, it's easy to test if edge hits or misses.
                 */
                var xflag0 = (vtx0.X > tx);
                var xflag1 = (vtx1.X > tx);
                if (xflag0 && xflag1)
                {
                    /* if edge's X values are both right of the point, then the point must be inside */
                    insideFlag = insideFlag == 0 ? 1 : 0;
                }
                else
                {
                    /*
                     * if X endpoints straddle the point, then
                     * the compute intersection of polygon edge with +X ray
                     * if intersection >= point's X then the +X ray hits it.
                     */
                    if ((vtx1.X - (vtx1.Y - ty) * (vtx0.X - vtx1.X) / (vtx0.Y - vtx1.Y)) >= tx)
                    {
                        insideFlag = insideFlag == 0 ? 1 : 0;
                    }
                }
            }
            /* move to next pair of vertices, retaining info as possible */
            yflag0 = yflag1;
            vtx0 = vtx1;
            vtx1 = polygon[++i];
        }
        return insideFlag;
    }
    /*
     * > 0 : p2 is left of the line p0 -> p1
     * = 0 : p2 is on the line p0 -> p1
     * < 0 : p2 is right of the line p0 -> p1
     */
    internal static double IsLeft(Vec2 p0, Vec2 p1, Vec2 p2) => (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);
}