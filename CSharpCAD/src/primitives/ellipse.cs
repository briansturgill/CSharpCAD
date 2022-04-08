namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct an axis-aligned ellipse in two dimensional space.</summary>
     * <remarks>
     * @see https://en.wikipedia.org/wiki/Ellipse
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0]] - center of ellipse
     * @param {Array} [options.radius=[1,1]] - radius of ellipse, along X and Y
     * @param {Number} [options.startAngle=0] - start angle of ellipse, in radians
     * @param {Number} [options.endAngle=(Math.PI * 2)] - end angle of ellipse, in radians
     * @param {Number} [options.segments=32] - number of segments to create per full rotation
     * </remarks>
     */
    public static Geom2 Ellipse(Opts opts)
    {
        var radius = opts.GetVec2("radius", (1.0, 1.0));
        int segments = opts.GetInt("segments", 32);
        var center = opts.GetVec2("center", (0.0, 0.0));
        double startAngle = opts.GetDouble("startAngle", 0);
        double endAngle = opts.GetDouble("endAngle", (Math.PI * 2));

        if (startAngle < 0) throw new ArgumentException("startAngle must be positive");
        if (endAngle < 0) throw new ArgumentException("endAngle must be positive");
        if (segments < 3) throw new ArgumentException("segments must be three or more");

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

        var minradius = Math.Min(radius.x, radius.y);
        var minangle = Math.Acos(((minradius * minradius) + (minradius * minradius) - (C.EPS * C.EPS)) /
                                  (2 * minradius * minradius));
        if (rotation < minangle) throw new ArgumentException("startAngle and endAngle do not define a significant rotation");

        segments = (int)Math.Floor(segments * (rotation / (Math.PI * 2)));

        var centerv = new Vec2(center.x, center.y);
        var step = rotation / segments; // radians per segment

        var points = new List<Vec2>(segments);
        segments = (rotation < Math.PI * 2) ? segments + 1 : segments;
        for (var i = 0; i < segments; i++)
        {
            var angle = (step * i) + startAngle;
            var point = new Vec2(radius.x * cos(angle), radius.y * sin(angle));
            point = centerv.Add(point);
            points.Add(point);
        }
        if (rotation < Math.PI * 2) points.Add(centerv);
        return new Geom2(points);
    }
}
