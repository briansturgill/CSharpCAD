namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a Z axis-aligned cylinder in three dimensional space.</summary>
     * <remarks>The default center places the cylinder's base at (0,0,0).</remarks>
     * <param name="radius">Radius of the cylinder.</param>
     * <param name="height">Height of the cylinder.</param>
     * <param name="segments">Number of segments in the X, Y circular part of the cylinder.</param>
     * <param name="center" default="(0,0,height/2)">Center of cylinder</param>
     * <example>
     * var g = Cylinder(height: 2, radius: 10);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 Cylinder(double radius = 1, double height = 2, int segments = 32, Vec3? center = null)
    {
        if (radius <= 0) throw new ArgumentException("Option radius must be greater than zero.");
        Vec2? v2center = null;
        double? center_z = null;
        if (center is not null)
        {
            v2center = new Vec2(((Vec3)center).X, ((Vec3)center).Y);
            center_z = ((Vec3)center).Z;
        }

        return InternalExtrudeSimple(InternalCircle(radius, segments, v2center), height: height, center_z: center_z);
    }

    /**
     * <summary>Construct a Z axis-aligned cylinder in three dimensional space.</summary>
     * <remarks>The default center places the cylinder's base at (0,0,0).</remarks>
     * <param name="radius">Radius of the cylinder.</param>
     * <param name="height">Height of the cylinder.</param>
     * <param name="segments">Number of segments in the X, Y circular part of the cylinder.</param>
     * <param name="startAngle">Begining of the rotation of the circular part (in degrees).</param>
     * <param name="endAngle">End of the rotation of the circular part (in degrees).</param>
     * <param name="center" default="(0,0,height/2)">Center of cylinder</param>
     * <example>
     * var g = Semicylinder(height: 2, radius: 10, startAngle: 90, endAngle: 135);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 Semicylinder(double radius = 1, double height = 2, int segments = 32,
        double startAngle = 0, double endAngle = 90, Vec3? center = null)
    {
        if (radius <= 0) throw new ArgumentException("Option radius must be greater than zero.");
        Vec2? v2center = null;
        double? center_z = null;
        if (center is not null)
        {
            v2center = new Vec2(((Vec3)center).X, ((Vec3)center).Y);
            center_z = ((Vec3)center).Z;
        }

        return InternalExtrudeSimple(InternalSemicircle(radius, segments, startAngle, endAngle, v2center),
            height: height, center_z: center_z);
    }
}
