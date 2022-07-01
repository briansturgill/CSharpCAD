namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Extrude the given geometry in an upward linear direction using the given options.</summary>
     * <param name="gobj">The geometries to extrude.</param>
     * <param name="height">The height of the extrusion.</param>
     * <param name="twistAngle">The final rotation (DEGREES) about the origin of the shape (if any).</param>
     * <param name="twistSteps">The resolution of the twist about the axis (if any).</param>
     * <param name="repair">Repair the slice to make it conformant.</param>
     * <group>3D Primitives</group>
     */
    public static Geom3 ExtrudeLinear(Geom2 gobj, double height = 1, double twistAngle = 0, int twistSteps = 1, bool repair = true)
    {
        if (twistAngle == 0 && gobj.HasOnlyOnePath) return ExtrudeSimple(gobj, height);
        twistAngle = DegToRad(twistAngle);
        return ExtrudeLinearGeom2(gobj, new Vec3(0, 0, height), twistAngle, twistSteps, repair);
    }
}