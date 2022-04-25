namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a Z axis-aligned cylinder in three dimensional space.</summary>
     * <param name="height">Height of the cylinder</param>
     * <param name="radius">Radius of the cylinder (at both start and end).</param>
     * <param name="segments">Number of segments to create per full rotation.</param>
     * <param name="center" default="(0,0,0)">Center of cylinder</param>
     * <example>
     * var g = Cylinder(height: 2, radius: 10);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 Cylinder(double radius = 1, double height = 2, int segments = 32, Vec3? center = null)
    {
        var _center = center ?? new Vec3(0, 0, 0);

        if (radius <= 0) throw new ArgumentException("Option radius must be greater than zero.");

        return CylinderElliptic(height: height, startRadius: new Vec2(radius, radius),
          endRadius: new Vec2(radius, radius), center: _center, segments: segments);
    }
}
