namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct a circle in 2D space, by default centered at (0, 0).</summary>
    /// <param name="radius">Radius of circle.</param>
    /// <param name="segments">Number of segments in a full rotation of circle.</param>
    /// <param name="startAngle">Begining of the rotation of the circle (in radians).</param>
    /// <param name="endAngle" default="Math.PI*2">End of the rotation of the circle (in radians).</param>
    /// <param name="center" default="(0,0)">Center of circle.</param>
    /// <group>2D Primitives</group>
    public static Geom2 Circle(double radius = 1, int segments = 32,
        double startAngle = 0, double endAngle = Math.PI * 2, Vec2? center = null)
    {
        if (radius <= 0.0) throw new ArgumentException("Radius value must be postive.");
        if (segments < 3) throw new ArgumentException("Segments must be at least 3.");
        var _center = center ?? new Vec2(0, 0);

        return Ellipse(radius: (radius, radius), segments: segments,
          center: _center, startAngle: startAngle, endAngle: endAngle);
    }
}
