namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a Z axis-aligned elliptic cylinder in three dimensional space.</summary>
     * <remarks>The default center places the elliptic cylinder's base at (0,0,0).</remarks>
     * <param name="radius" default="(1,1)">Radius of the elliptic cylinder.</param>
     * <param name="height">Height of the elliptic cylinder.</param>
     * <param name="segments">Number of segments in the X, Y circular part of the elliptic cylinder.</param>
     * <param name="center" default="(0,0,height/2)">Center of elliptic cylinder</param>
     * <example>
     * var g = EllipticCylinder(height: 2, radius: 10);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 EllipticCylinder(Vec2? radius = null, double height = 2, int segments = 32, Vec3? center = null)
    {
        var _radius = radius ?? new Vec2(1, 1);
        Vec2? v2center = null;
        double? center_z = null;
        if (center is not null)
        {
            v2center = new Vec2(((Vec3)center).X, ((Vec3)center).Y);
            center_z = ((Vec3)center).Z;
        }

        return InternalExtrudeSimple(InternalEllipse(radius, segments, v2center), height: height, center_z: center_z, v2center);
    }

    /**
     * <summary>Construct a Z axis-aligned elliptic cylinder in three dimensional space.</summary>
     * <remarks>The default center places the elliptic cylinder's base at (0,0,0).</remarks>
     * <param name="radius" default="(1,1)">Radius of the elliptic cylinder.</param>
     * <param name="height">Height of the elliptic cylinder.</param>
     * <param name="segments">Number of segments in the X, Y elliptical part of the cylinder.</param>
     * <param name="startAngle">Begining of the rotation of the elliptical part (in degrees).</param>
     * <param name="endAngle">End of the rotation of the circular part (in degrees).</param>
     * <param name="center" default="(0,0,height/2)">Center of elliptic cylinder</param>
     * <example>
     * var g = EllipticCylinder(height: 2, radius: 10, startAngle: 90, endAngle: 135);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 SemiellipticCylinder(Vec2? radius = null, double height = 2, int segments = 32,
        double startAngle = 0, double endAngle = 90, Vec3? center = null)
    {
        Vec2? v2center = null;
        double? center_z = null;
        if (center is not null)
        {
            v2center = new Vec2(((Vec3)center).X, ((Vec3)center).Y);
            center_z = ((Vec3)center).Z;
        }

        return InternalExtrudeSimple(InternalSemiellipse(radius, segments, startAngle, endAngle, v2center),
            height: height, center_z: center_z, v2center);
    }
}
