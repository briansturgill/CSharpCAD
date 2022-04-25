namespace CSharpCAD;

public static partial class CSCAD
{
    // @see http://www.jdawiseman.com/papers/easymath/surds_star_inner_radius.html
    private static double getRadiusRatio(double vertices, double density)
    {
        if (vertices > 0 && density > 1 && density < vertices / 2)
        {
            return Math.Cos(Math.PI * density / vertices) / Math.Cos(Math.PI * (density - 1) / vertices);
        }
        return 0;
    }

    private static List<Vec2> getPoints(double vertices, double radius, double startAngle, Vec2 center)
    {
        var a = (Math.PI * 2) / vertices;

        var points = new List<Vec2>();
        for (var i = 0; i < vertices; i++)
        {
            var point = Vec2.FromAngleRadians(a * i + startAngle);
            point = point.Scale(radius);
            point = center.Add(point);
            points.Add(point);
        }
        return points;
    }

    /**
     * <summary>Construct a star in two dimensional space.</summary>
     * <remarks>https://en.wikipedia.org/wiki/Star_polygon</remarks>
     * <param name="vertices">Number of vertices (P) on the star.</param>
     * <param name="density">Density (Q) of star.</param>
     * <param name="outerRadius">Outer radius of vertices.</param>
     * <param name="innerRadius">Inner radius of vertices, or zero to calculate.</param>
     * <param name="startAngle">Starting angle for first vertice, in radians.</param>
     * <param name="center" default="(0,0)">Center of star.</param>
     * <example>
     * var star1 = Star(vertices: 8, outerRadius: 10); // Star with 8/2 density.
     * var star2 = Star(vertices: 12, outerRadius: 40, innerRadius: 20); // Star with given radius.
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Star(double vertices = 5, double outerRadius = 1, double innerRadius = 0,
        double density = 2, double startAngle = 0, Vec2? center = null)
    {
        var _center = center ?? new Vec2(0, 0);

        if (vertices < 2) throw new ArgumentException("Option vertices must be two or more.");
        if (outerRadius < 0) throw new ArgumentException("Option outerRadius must be greater than zero.");
        if (innerRadius < 0) throw new ArgumentException("Option innerRadius must be greater than zero.");
        if (startAngle < 0) throw new ArgumentException("Option startAngle must be greater than zero.");

        // force integers
        vertices = Math.Floor(vertices);
        density = Math.Floor(density);

        startAngle = startAngle % (Math.PI * 2);

        if (innerRadius == 0)
        {
            if (density < 2) throw new ArgumentException("Option density must be two or more.");
            innerRadius = outerRadius * getRadiusRatio(vertices, density);
        }

        var centerv = _center;

        var outerPoints = getPoints(vertices, outerRadius, startAngle, centerv);
        var innerPoints = getPoints(vertices, innerRadius, startAngle + Math.PI / vertices, centerv);

        var allPoints = new List<Vec2>();
        for (var i = 0; i < vertices; i++)
        {
            allPoints.Add(outerPoints[i]);
            allPoints.Add(innerPoints[i]);
        }

        return new Geom2(allPoints);
    }
}
