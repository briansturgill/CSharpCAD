namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct an axis-aligned ellipse in two dimensional space.</summary>
     * <remarks>
     * @see https://en.wikipedia.org/wiki/Ellipse
     * </remarks>
     * <param name="center">Center of Ellipse. Default: [0,0].</param>
     * <param name="radius">Radius of ellipse, along X and Y axes. Default:[1,1]</param>
     * <param name="startAngle">Start angle of ellipse, in radians</param>
     * <param name="endAngle">End angle of ellipse, in radians.</param>
     * <param name="segments">Number of segments to create per full rotation.</param>
     * <example>
     * var g = CylinderElliptic(radius: (10, 5), segments: 50);
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Ellipse(Vec2? radius = null, int segments = 32,
        Vec2? center = null, double startAngle = 0, double endAngle = (Math.PI * 2))
    {
        var _radius = radius ?? new Vec2(1.0, 1.0);
        var _center = center ?? new Vec2(0.0, 0.0);

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

        var minradius = Math.Min(_radius.X, _radius.Y);
        var minangle = Math.Acos(((minradius * minradius) + (minradius * minradius) - (C.EPS * C.EPS)) /
                                  (2 * minradius * minradius));
        if (rotation < minangle) throw new ArgumentException("startAngle and endAngle do not define a significant rotation");

        segments = Floorish(segments * (rotation / (Math.PI * 2)));

        var centerv = new Vec2(_center.X, _center.Y);
        var step = rotation / (double)segments; // radians per segment

        var points = new List<Vec2>(segments);
        segments = (LessThanish(rotation, Math.PI * 2)) ? segments + 1 : segments;
        for (var i = 0; i < segments; i++)
        {
            var angle = (step * i) + startAngle;
            var point = new Vec2(_radius.X * Math.Cos(angle), _radius.Y * Math.Sin(angle));
            point = centerv.Add(point);
            points.Add(point);
        }
        if (LessThanish(rotation, Math.PI * 2)) points.Add(centerv);
        return new Geom2(points);
    }
}
