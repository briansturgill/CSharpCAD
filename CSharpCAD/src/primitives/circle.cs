namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct a circle in 2D space.</summary>
    /// <param name="radius">Radius of circle.</param>
    /// <param name="segments">Number of segments in a full circle.</param>
    /// <param name="center" default="(0,0)">Center of circle.</param>
    /// <example>
    /// var g = Circle(radius: 5, segments: 50));
    /// </example>
    /// <group>2D Primitives</group>
    public static Geom2 Circle(double radius = 1, int segments = 32, Vec2? center = null)
    {
        if (radius <= 0.0) throw new ArgumentException("Radius value must be postive.");
        if (segments < 3) throw new ArgumentException("Segments must be at least 3.");

        var step = (Math.PI * 2) / segments; // radians per segment

        var points = new Vec2[segments];

        for (var i = 0; i < segments; i++)
        {
            var angle = step * i;
            points[i] = new Vec2(radius * Math.Cos(angle), radius * Math.Sin(angle));
        }

        if (center is not null && !(((Vec2)center).X == 0 && ((Vec2)center).Y == 0))
        {
            for (var i = 0; i < segments; i++)
            {
                points[i] = ((Vec2)center).Add(points[i]);
            }
        }

        return new Geom2(points);
    }

    /// <summary>Construct a partial circle in 2D space.</summary>
    /// <param name="radius">Radius of circle.</param>
    /// <param name="segments">Number of segments in a full circle.</param>
    /// <param name="startAngle">Begining of the rotation of the circle (in degrees).</param>
    /// <param name="endAngle">End of the rotation of the circle (in degrees).</param>
    /// <param name="center" default="(0,0)">Center of circle.</param>
    /// <example>
    /// var g = SemiCircle(radius: 5, segments: 50, startAngle: 90, endAngle: 135);
    /// </example>
    /// <group>2D Primitives</group>
    public static Geom2 SemiCircle(double radius = 1, int segments = 32,
        double startAngle = 0, double endAngle = 90, Vec2? center = null)
    {
        if (radius <= 0.0) throw new ArgumentException("Radius value must be postive.");
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

        var minangle = Math.Acos(((2 * radius * radius) - (C.EPS * C.EPS)) / (2 * radius * radius));
        if (rotation < minangle) throw new ArgumentException("Argments startAngle and endAngle do not define a significant rotation.");

        var step = rotation / (double)(segments - 1); // radians per segment

        var points = new Vec2[segments + 1];
        int i;
        for (i = 0; i < segments; i++)
        {
            var angle = (step * i) + startAngle;
            points[i] = new Vec2(radius * Math.Cos(angle), radius * Math.Sin(angle));
        }

        if (center is not null && !(((Vec2)center).X == 0 && ((Vec2)center).Y == 0))
        {
            for (i = 0; i < segments; i++)
            {
                points[i] = ((Vec2)center).Add(points[i]);
            }
        }
        points[i] = center ?? new Vec2();
        return new Geom2(points);
    }
}
