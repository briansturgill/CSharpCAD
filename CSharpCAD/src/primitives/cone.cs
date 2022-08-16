namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a Z axis-aligned cone in three dimensional space.</summary>
     * <remarks>The default center places the cone's base at (0,0,0).</remarks>
     * <param name="top">Radius of the top of the cone.</param>
     * <param name="bottom">Radius of the bottom of the cone.</param>
     * <param name="height">Height of the cylinder.</param>
     * <param name="segments">Number of segments in the X, Y circular part of the cylinder.</param>
     * <param name="center" default="(0,0,height/2)">Center of cylinder</param>
     * <example>
     * var g = Cylinder(height: 2, radius: 10);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 Cone(double top = 2, double bottom = 10, double height = 20,
        int segments = 32, Vec3? center = null)
    {
        if (top < 0) throw new ArgumentException("Option top radius must be positive.");
        if (LessThanOrEqualish(bottom, 0)) throw new ArgumentException("Option bottom radius must greater than 0.");
        Vec2? v2center = null;
        double? center_z = null;
        if (center is not null)
        {
            v2center = new Vec2(((Vec3)center).X, ((Vec3)center).Y);
            center_z = ((Vec3)center).Z - (height/2.0);
        }

        var iZ = center_z ?? 0.0;
        var se = new SegmentedExtruder(Circle(bottom, segments: segments, v2center), initialZ: iZ);
        if (Equalish(top, 0)) // It's natural to want a cone to stop at zero, but Circle doesn't like that.
        {
            se.AddZeroTopCap(height);
            return se.Finished();
        }

        se.AddSegment(Circle(top, segments, v2center), height);
        return se.Finished();
    }
}
