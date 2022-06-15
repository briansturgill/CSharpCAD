namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct an axis-aligned ellipse in two dimensional space.</summary>
     * <remarks>
     * @see https://en.wikipedia.org/wiki/Ellipse
     * </remarks>
     * <param name="radius" default="(1,1)">Radius of ellipse, along X and Y axes.</param>
     * <param name="segments">Number of segments created in the full ellipse.</param>
     * <param name="center" default="(0,0)">Center of ellipse.</param>
     * <example>
     * var g = Ellipse(radius: (10, 5), segments: 50);
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Ellipse(Vec2? radius = null, int segments = 32, Vec2? center = null)
    {
        return new Geom2(InternalEllipse(radius, segments, center));
    }

    internal static Vec2[] InternalEllipse(Vec2? radius, int segments, Vec2? center)
    {
        var _radius = radius ?? new Vec2(1.0, 1.0);
        if (_radius.X <= 0.0 || _radius.Y <= 0.0) throw new ArgumentException("Radius values must be postive.");
        if (segments < 3) throw new ArgumentException("Segments must be at least 3.");

        var step = (Math.PI * 2) / segments; // radians per segment

        var points = new Vec2[segments];

        for (var i = 0; i < segments; i++)
        {
            var angle = step * i;
            points[i] = new Vec2(_radius.X * Math.Cos(angle), _radius.Y * Math.Sin(angle));
        }

        if (center is not null && !(((Vec2)center).X == 0 && ((Vec2)center).Y == 0))
        {
            for (var i = 0; i < segments; i++)
            {
                points[i] = ((Vec2)center).Add(points[i]);
            }
        }

        return points;
    }

    /**
     * <summary>Construct an axis-aligned ellipse in two dimensional space.</summary>
     * <remarks>
     * @see https://en.wikipedia.org/wiki/Ellipse
     * </remarks>
     * <param name="radius" default="(1,1)">Radius of ellipse, along X and Y axes.</param>
     * <param name="segments">Number of segments created in a full ellipse.</param>
     * <param name="startAngle">Start angle of semiellipse, in degrees</param>
     * <param name="endAngle">End angle of semiellipse, in degrees.</param>
     * <param name="center" default="(0,0)">Center of semiellipse.</param>
     * <example>
     * var g = SemiEllipse(radius: (10, 5), segments: 50, startAngle: 90, endAngle: 135);
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Semiellipse(Vec2? radius = null, int segments = 32,
        double startAngle = 0, double endAngle = 90, Vec2? center = null)
    {
        return new Geom2(InternalSemiellipse(radius, segments, startAngle, endAngle, center));
    }

    internal static Vec2[] InternalSemiellipse(Vec2? radius, int segments,
        double startAngle, double endAngle, Vec2? center)
    {
        var _radius = radius ?? new Vec2(1.0, 1.0);
        if (_radius.X <= 0.0 || _radius.Y <= 0.0) throw new ArgumentException("Radius value must be postive.");
        if (segments < 3) throw new ArgumentException("Segments must be at least 3.");
        if (startAngle < 0) throw new ArgumentException("Argument startAngle must be positive degrees.");
        if (GreaterThanOrEqualish(startAngle, 360)) throw new ArgumentException("Argument startAngle must be less than 360 degrees.");
        if (endAngle < 0) throw new ArgumentException("Argument endAngle must be positive.");
        if (endAngle < startAngle) endAngle += 360;
        var rotation = endAngle - startAngle;
        if (GreaterThanOrEqualish(rotation, 360)) throw new ArgumentException("Total rotation must be less than 360 degrees.");

        segments = Floorish(segments * (rotation / 360.0)) + 1; // +1 is to match JSCAD behavior.
        if (segments < 3) segments = 3;
        // Starting here, everything is in radians.
        rotation = DegToRad(rotation);
        startAngle = DegToRad(startAngle);

        var minradius = Math.Min(_radius.X, _radius.Y);
        var minangle = Math.Acos(((2 * minradius * minradius) - (C.EPS * C.EPS)) / (2 * minradius * minradius));
        if (rotation < minangle) throw new ArgumentException("Argments startAngle and endAngle do not define a significant rotation.");

        var step = rotation / (double)(segments - 1); // radians per segment

        var points = new Vec2[segments + 1];
        int i;
        for (i = 0; i < segments; i++)
        {
            var angle = (step * i) + startAngle;
            points[i] = new Vec2(_radius.X * Math.Cos(angle), _radius.Y * Math.Sin(angle));
        }

        if (center is not null && !(((Vec2)center).X == 0 && ((Vec2)center).Y == 0))
        {
            for (i = 0; i < segments; i++)
            {
                points[i] = ((Vec2)center).Add(points[i]);
            }
        }
        points[i] = center ?? new Vec2();
        return points;
    }
}
