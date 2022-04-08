namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a Z axis-aligned elliptic cylinder in three dimensional space.</summary>
     * <remarks>
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0,0]] - center of cylinder
     * @param {Number} [options.height=2] - height of cylinder
     * @param {Array} [options.startRadius=[1,1]] - radius of rounded start, must be two dimensional array
     * @param {Number} [options.startAngle=0] - start angle of cylinder, in radians
     * @param {Array} [options.endRadius=[1,1]] - radius of rounded end, must be two dimensional array
     * @param {Number} [options.endAngle=(Math.PI * 2)] - end angle of cylinder, in radians
     * @param {Number} [options.segments=32] - number of segments to create per full rotation
     * </remarks>
     */
    public static Geom3 CylinderElliptic(Opts opts)
    {
        var center = opts.GetVec3("center", (0.0, 0.0, 0.0));
        var height = opts.GetDouble("height", 2.0);
        var startRadius = opts.GetVec2("startRadius", (1.0, 1.0));
        var startAngle = opts.GetDouble("startAngle", 0.0);
        var endRadius = opts.GetVec2("endRadius", (1.0, 1.0));
        var endAngle = opts.GetDouble("endAngle", Math.PI * 2);
        var segments = opts.GetInt("segments", 32);

        if (height <= 0.0) throw new ArgumentException("Option height must be greater then zero.");
        if (startRadius.x < 0 || startRadius.y <= 0) throw new ArgumentException("Option startRadius values must be positive.");
        if (endRadius.x <= 0 || endRadius.y <= 0) throw new ArgumentException("Option endRadius values must be positive.");
        if (endRadius.x == 0 && endRadius.y == 0) throw new ArgumentException("At least one of option endRadius values must be positive.");
        if (startAngle < 0) throw new ArgumentException("Option startAngle must be positive.");
        if (endAngle < 0) throw new ArgumentException("Option endAngle must be positive.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        startAngle = startAngle % (Math.PI * 2);
        endAngle = endAngle % (Math.PI * 2);

        var rotation = (Math.PI * 2);
        if (startAngle < endAngle)
        {
            rotation = endAngle - startAngle;
        }
        if (startAngle > endAngle)
        {
            rotation = endAngle + ((Math.PI * 2) - startAngle);
        }

        var minradius = Math.Min(Math.Min(startRadius.x, startRadius.y), Math.Min(endRadius.x, endRadius.y));
        var minangle = Math.Acos(((minradius * minradius) + (minradius * minradius) - (C.EPS * C.EPS)) /
                                  (2 * minradius * minradius));
        if (rotation < minangle) throw new ArgumentException("Options startAngle and endAngle do not define a significant rotation.");

        var slices = Floorish(segments * (rotation / (Math.PI * 2)));

        var start = new Vec3(0, 0, -(height / 2));
        var end = new Vec3(0, 0, height / 2);
        var ray = end.Subtract(start);

        var axisX = new Vec3(1, 0, 0);
        var axisY = new Vec3(0, 1, 0);

        var v1 = new Vec3();
        var v2 = new Vec3();
        var v3 = new Vec3();

        Vec3 point(double stack, double slice, Vec2 radius)
        {
            var angle = slice * rotation + startAngle;
            v1 = axisX.Scale(radius.x * cos(angle));
            v2 = axisY.Scale(radius.y * sin(angle));
            v1 = v1.Add(v2);

            v3 = ray.Scale(stack);
            v3 = v3.Add(start);
            return v1.Add(v3);
        }

        // adjust the points to center
        List<Vec3> fromPoints(params Vec3[] points)
        {
            var newpoints = new List<Vec3>(points.Length);
            foreach (var point in points)
            {
                newpoints.Add(point.Add(center));
            }
            return newpoints;
        }

        var polygons = new List<List<Vec3>>();
        for (var i = 0; i < slices; i++)
        {
            var t0 = i / (double)slices;
            var t1 = (i + 1) / (double)slices;

            if (endRadius.x == startRadius.x && endRadius.y == startRadius.y)
            {
                polygons.Add(fromPoints(start, point(0, t1, endRadius), point(0, t0, endRadius)));
                polygons.Add(fromPoints(point(0, t1, endRadius), point(1, t1, endRadius), point(1, t0, endRadius), point(0, t0, endRadius)));
                polygons.Add(fromPoints(end, point(1, t0, endRadius), point(1, t1, endRadius)));
            }
            else
            {
                if (startRadius.x > 0 && startRadius.y > 0)
                {
                    polygons.Add(fromPoints(start, point(0, t1, startRadius), point(0, t0, startRadius)));
                }
                if (startRadius.x > 0 || startRadius.y > 0)
                {
                    polygons.Add(fromPoints(point(0, t0, startRadius), point(0, t1, startRadius), point(1, t0, endRadius)));
                }
                if (endRadius.x > 0 && endRadius.y > 0)
                {
                    polygons.Add(fromPoints(end, point(1, t0, endRadius), point(1, t1, endRadius)));
                }
                if (endRadius.x > 0 || endRadius.y > 0)
                {
                    polygons.Add(fromPoints(point(1, t0, endRadius), point(0, t1, startRadius), point(1, t1, endRadius)));
                }
            }
        }
        if (rotation < (Math.PI * 2))
        {
            polygons.Add(fromPoints(start, point(0, 0, startRadius), end));
            polygons.Add(fromPoints(point(0, 0, startRadius), point(1, 0, endRadius), end));
            polygons.Add(fromPoints(start, end, point(0, 1, startRadius)));
            polygons.Add(fromPoints(point(0, 1, startRadius), end, point(1, 1, endRadius)));
        }
        var result = new Geom3(polygons);
        return result;
    }
}
