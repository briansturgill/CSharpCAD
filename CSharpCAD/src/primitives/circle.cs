namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct a circle in two dimensional space where all points are at the same distance from the center.</summary>
    public static Geom2 Circle(Opts opts)
    {
        var radius = opts.GetDouble("radius", 1.0);
        int segments = opts.GetInt("segments", 32);
        var center = opts.GetVec2("center", (0.0, 0.0));
        double startAngle = opts.GetDouble("startAngle", 0);
        double endAngle = opts.GetDouble("endAngle", (Math.PI * 2));

        if (radius <= 0) throw new ArgumentException("Radius must have a postive value.");
        if (segments < 3) throw new ArgumentException("Segments must be at least 3.");

        return Ellipse(new Opts{{"radius", (radius, radius)}, {"segments", segments},
          {"center", (center.x, center.y)}, {"startAngle", startAngle}, {"endAngle", endAngle}});
    }
}
