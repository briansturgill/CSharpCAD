namespace CSharpCAD;


using Seg = Vec2;

public static partial class CSCAD
{
    internal struct Corner
    {
        internal Vec2 c;
        internal Seg[] s0;
        internal Seg[] s1;
        internal Corner(Vec2 c, Seg[] s0, Seg[] s1)
        {
            this.c = c;
            this.s0 = s0;
            this.s1 = s1;
        }
    };
    /*
     * Create a set of offset points from the given points using the given options (if any).
     * @param {Object} options - options for offset
     * @param {Float} [options.delta=1] - delta of offset (+ to exterior, - from interior)
     * @param {String} [options.corners="edge"] - type corner to create during of expansion; edge, chamfer, round
     * @param {Integer} [options.segments=16] - number of segments when creating round corners
     * @param {Integer} [options.closed=false] - is the last point connected back to the first point?
     * @param {Array} points - array of 2D points
     * @returns {Array} new set of offset points, plus points for each rounded corner
     */
    internal static List<Vec2> OffsetFromPoints(List<Vec2> points, double delta, string corners, int segments, bool closed)
    {
        if (Math.Abs(delta) < C.EPS) return points;

        var rotation = closed ? area(points) : 1.0; // + counter clockwise, - clockwise
        if (rotation == 0) rotation = 1.0;

        // use right hand normal?
        var orientation = ((rotation > 0) && (delta >= 0)) || ((rotation < 0) && (delta < 0));
        delta = Math.Abs(delta); // sign is no longer required

        Seg[]? previousSegment = null;
        var newPoints = new List<Vec2>();
        var newCorners = new List<Corner>(); ;
        var of = new Vec2();
        var n = points.Count;
        for (var i = 0; i < n; i++)
        {
            var j = (i + 1) % n;
            var p0 = points[i];
            var p1 = points[j];
            // calculate the unit normal
            of = orientation ? p0.Subtract(p1) : p1.Subtract(p0);
            of = of.Normal();
            of = of.Normalize();
            // calculate the offset vector
            of = of.Scale(delta);
            // calculate the new points (edge)
            var n0 = p0.Add(of);
            var n1 = p1.Add(of);

            var currentSegment = new Seg[] { n0, n1 };
            if (previousSegment is not null)
            {
                if (closed || (!closed && j != 0))
                {
                    // check for intersection of new line segments
                    var ip = intersect(previousSegment[0], previousSegment[1], currentSegment[0], currentSegment[1]);
                    if (ip is not null)
                    {
                        // adjust the previous points
                        newPoints.RemoveAt(newPoints.Count - 1); // Pop
                        // adjust current points
                        currentSegment[0] = (Vec2)ip;
                    }
                    else
                    {
                        newCorners.Add(new Corner(c: p0, s0: previousSegment, s1: currentSegment));
                    }
                }
            }
            previousSegment = new Seg[] { n0, n1 };

            if (j == 0 && !closed) continue;

            newPoints.Add(currentSegment[0]);
            newPoints.Add(currentSegment[1]);
        }
        // complete the closure if required
        if (closed && previousSegment != null)
        {
            // check for intersection of closing line segments
            var n0 = newPoints[0];
            var n1 = newPoints[1];
            var ip = intersect(previousSegment[0], previousSegment[1], n0, n1);
            if (ip is not null)
            {
                // adjust the previous points
                newPoints[0] = (Vec2)ip;
                newPoints.RemoveAt(newPoints.Count - 1); // Pop
            }
            else
            {
                var p0 = points[0];
                var cursegment = new Seg[] { n0, n1 };
                newCorners.Add(new Corner(c: p0, s0: previousSegment, s1: cursegment));
            }
        }

        // generate corners if necessary

        if (corners == "edge")
        {
            // map for fast point index lookup
            var pointIndex = new Dictionary<Vec2, int>(); // {point: index}
            for (var i = 0; i < newPoints.Count; i++)
            {
                pointIndex.Add(newPoints[i], i);
            }

            // create edge corners
            foreach (var corner in newCorners)
            {
                var ip = IntersectPointOfLines(corner.s0[0], corner.s0[1], corner.s1[0], corner.s1[1]);
                if (double.IsFinite(ip.x) && double.IsFinite(ip.y))
                {
                    var p0 = corner.s0[1];
                    var i = pointIndex[p0];
                    newPoints[i] = ip;
                    newPoints[(i + 1) % newPoints.Count] = new Vec2(double.NaN, double.NaN);
                }
                else
                {
                    // paralell segments, drop one
                    var p0 = corner.s1[0];
                    var i = pointIndex[p0];
                    newPoints[i] = new Vec2(double.NaN, double.NaN);
                }
            }
            newPoints = newPoints.Where((p) => !(double.IsNaN(p.x) && double.IsNaN(p.y))).ToList();
        }

        if (corners == "round")
        {
            // create rounded corners
            var cornersegments = segments / 4;
            foreach (var corner in newCorners)
            {
                // calculate angle of rotation
                rotation = corner.s1[0].Subtract(corner.c).AngleRadians();
                rotation -= corner.s0[1].Subtract(corner.c).AngleRadians();
                if (orientation && rotation < 0)
                {
                    rotation = rotation + Math.PI;
                    if (rotation < 0) rotation = rotation + Math.PI;
                }
                if ((!orientation) && rotation > 0)
                {
                    rotation = rotation - Math.PI;
                    if (rotation > 0) rotation = rotation - Math.PI;
                }

                if (rotation != 0.0)
                {
                    // generate the segments
                    cornersegments = (int)(segments * (Math.Abs(rotation) / (2 * Math.PI)));
                    var step = rotation / cornersegments;
                    var start = corner.s0[1].Subtract(corner.c).AngleRadians();
                    var cornerpoints = new List<Vec2>();
                    for (var i = 1; i < cornersegments; i++)
                    {
                        var radians = start + (step * i);
                        var point = Vec2.FromAngleRadians(radians);
                        point = point.Scale(delta);
                        point = point.Add(corner.c);
                        cornerpoints.Add(point);
                    }
                    if (cornerpoints.Count > 0)
                    {
                        var p0 = corner.s0[1];
                        var i = newPoints.FindIndex((point) => p0 == point);
                        i = (i + 1) % newPoints.Count;
                        newPoints.InsertRange(i, cornerpoints);
                    }
                }
                else
                {
                    // paralell segments, drop one
                    var p0 = corner.s1[0];
                    var i = newPoints.FindIndex((point) => p0 == point);
                    newPoints.RemoveAt(i);
                }
            }
        }
        return newPoints;
    }

    private static double area(List<Vec2> points)
    {
        var area = ((double)0.0);
        var len = points.Count;
        for (var i = 0; i < len; i++)
        {
            var j = (i + 1) % len;
            area += points[i].x * points[j].y;
            area -= points[j].x * points[i].y;
        }
        return (area / 2.0);
    }

    /**
     * Calculate the intersect point of the two line segments (p1-p2 and p3-p4), end points included.
     * Note: If the line segments do NOT intersect then undefined is returned.
     * @see http://paulbourke.net/geometry/pointlineplane/
     * @param {vec2} p1 - first point of first line segment
     * @param {vec2} p2 - second point of first line segment
     * @param {vec2} p3 - first point of second line segment
     * @param {vec2} p4 - second point of second line segment
     * @returns {vec2} intersection point of the two line segments, or undefined
     * @alias module:modeling/maths/utils.intersect
     */
    private static Vec2? intersect(Vec2 p1, Vec2 p2, Vec2 p3, Vec2 p4)
    {
        // Check if none of the lines are of length 0
        if ((p1.x == p2.x && p1.y == p2.y) || (p3.x == p4.x && p3.y == p4.y))
        {
            return null;
        }

        var denominator = ((p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y));

        // Lines are parallel
        if (Math.Abs(denominator) < double.MinValue)
        {
            return null;
        }

        var ua = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
        var ub = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;

        // is the intersection along the segments
        if (ua < 0 || ua > 1 || ub < 0 || ub > 1)
        {
            return null;
        }

        // Return the x and y coordinates of the intersection
        var x = p1.x + ua * (p2.x - p1.x);
        var y = p1.y + ua * (p2.y - p1.y);

        return new Vec2(x, y);
    }

    private static Vec2 Solve2Linear(double a, double b, double c, double d, double u, double v)
    {
        var det = a * d - b * c;
        var invdet = 1.0 / det;
        var x = u * d - b * v;
        var y = -u * c + a * v;
        x *= invdet;
        y *= invdet;
        return new Vec2(x, y);
    }

    private static Vec2 IntersectPointOfLines(Vec2 line00, Vec2 line01, Vec2 line10, Vec2 line11)
    {
        var line0_vec = line01.Subtract(line00).Normal().Normalize();
        var line0_dist = line00.Dot(line0_vec);
        var line1_vec = line11.Subtract(line10).Normal().Normalize();
        var line1_dist = line10.Dot(line1_vec);
        return Solve2Linear(line0_vec.x, line0_vec.y, line1_vec.x, line1_vec.y, line0_dist, line1_dist);
    }
}