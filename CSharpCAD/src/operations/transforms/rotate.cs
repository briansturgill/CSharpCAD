namespace CSharpCAD;

public static partial class CSCAD
{

    /**
     * <summary>Rotate the given objects using the given options.</summary>
     * <param name="angles">The angle (RADIANS) of rotations about X, Y, and Z axis.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geometry Rotate(Vec3 angles, Geometry g)
    {
        var yaw = DegToRad(angles.Z);
        var pitch = DegToRad(angles.Y);
        var roll = DegToRad(angles.X);

        var matrix = Mat4.FromTaitBryanRotation(yaw, pitch, roll);

        return g.Is2D ? ((Geom2)g).Transform(matrix) : ((Geom3)g).Transform(matrix);
    }

    /**
     * <summary>Rotate the given objects about the X axis.</summary>
     * <param name="angle">The angle (RADIANS) of rotations about X.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geometry RotateX(double angle, Geometry g) => Rotate(new Vec3(angle, 0, 0), g);

    /**
     * <summary>Rotate the given objects about the Y axis.</summary>
     * <param name="angle">The angle (RADIANS) of rotations about Y.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geometry RotateY(double angle, Geometry g) => Rotate(new Vec3(0, angle, 0), g);

    /**
     * <summary>Rotate the given objects about the Z axis.</summary>
     * <param name="angle">The angle (RADIANS) of rotations about Z.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geometry RotateZ(double angle, Geometry g) => Rotate(new Vec3(0, 0, angle), g);
}
