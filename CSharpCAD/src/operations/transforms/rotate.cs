namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Rotate the given objects using the given options.</summary>
     * <param name="angles">The angle (DEGREES) of rotations about X, and Y axis.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geom2 Rotate(Vec3 angles, Geom2 g)
    {
        var yaw = DegToRad(angles.Z);
        var pitch = DegToRad(angles.Y);
        var roll = DegToRad(angles.X);

        var matrix = Mat4.FromTaitBryanRotation(yaw, pitch, roll);

        return g.Transform(matrix);
    }

    /**
     * <summary>Rotate the given objects using the given options.</summary>
     * <param name="angles">The angle (DEGREES) of rotations about X, Y, and Z axis.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geom3 Rotate(Vec3 angles, Geom3 g)
    {
        var yaw = DegToRad(angles.Z);
        var pitch = DegToRad(angles.Y);
        var roll = DegToRad(angles.X);

        var matrix = Mat4.FromTaitBryanRotation(yaw, pitch, roll);

        return g.Transform(matrix);
    }

    /**
     * <summary>Rotate the given objects about the Z axis.</summary>
     * <param name="angle">The angle (DEGREES) of rotations about Z.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geom2 RotateZ(double angle, Geom2 g) => Rotate(new Vec3(0, 0, angle), g);

    /**
     * <summary>Rotate the given objects about the X axis.</summary>
     * <param name="angle">The angle (DEGREES) of rotations about X.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geom3 RotateX(double angle, Geom3 g) => Rotate(new Vec3(angle, 0, 0), g);

    /**
     * <summary>Rotate the given objects about the Y axis.</summary>
     * <param name="angle">The angle (DEGREES) of rotations about Y.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geom3 RotateY(double angle, Geom3 g) => Rotate(new Vec3(0, angle, 0), g);

    /**
     * <summary>Rotate the given objects about the Z axis.</summary>
     * <param name="angle">The angle (DEGREES) of rotations about Z.</param>
     * <param name="g">The geometry object to rotate.</param>
     * <group>Transformations</group>
     */
    public static Geom3 RotateZ(double angle, Geom3 g) => Rotate(new Vec3(0, 0, angle), g);
}