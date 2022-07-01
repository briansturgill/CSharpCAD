namespace CSharpCAD;


public static partial class CSCAD
{
    /**
     * <summary>Mirror the given object using the given options.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin" default="(0,0,0)">The origin of the plane.</param>
     * <param name="normal" default="(0,0,1)">The normal vector of the plane.</param>
     * <returns>The mirrored geometry.</returns>
     * <group>Transformations</group>
     */
    public static Geom2 Mirror(Geom2 g, Vec3? origin = null, Vec3? normal = null)
    {
        var _origin = origin ?? new Vec3(0, 0, 0);
        var _normal = normal ?? new Vec3(0, 0, 1); // Z axis

        var planeOfMirror = new Plane(_normal, _origin);
        // verify the plane, i.e. check that the given normal was valid
        if (double.IsNaN(planeOfMirror.Normal.X))
        {
            throw new ArgumentException("The given origin and normal do not define a proper plane");
        }

        var matrix = Mat4.MirrorByPlane(planeOfMirror);

        return g.Transform(matrix);
    }

    /**
     * <summary>Mirror the given object using the given options.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin" default="(0,0,0)">The origin of the plane.</param>
     * <param name="normal" default="(0,0,1)">The normal vector of the plane.</param>
     * <returns>The mirrored geometry.</returns>
     * <group>Transformations</group>
     */
    public static Geom3 Mirror(Geom3 g, Vec3? origin = null, Vec3? normal = null)
    {
        var _origin = origin ?? new Vec3(0, 0, 0);
        var _normal = normal ?? new Vec3(0, 0, 1); // Z axis

        var planeOfMirror = new Plane(_normal, _origin);
        // verify the plane, i.e. check that the given normal was valid
        if (double.IsNaN(planeOfMirror.Normal.X))
        {
            throw new ArgumentException("The given origin and normal do not define a proper plane");
        }

        var matrix = Mat4.MirrorByPlane(planeOfMirror);

        return g.Transform(matrix);
    }

    /**
     * <summary>Mirror the given ogeometry about the X axis.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin" default="(0,0,0)">The origin of the plane.</param>
     * <returns>The mirrored geometry.</returns>
     * <group>Transformations</group>
     */
    public static Geom2 MirrorX(Geom2 g, Vec3? origin = null) => Mirror(g, origin: origin, normal: new Vec3(1, 0, 0));

    /**
     * <summary>Mirror the given geometry about the Y axis.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin" default="(0,0,0)">The origin of the plane.</param>
     * <returns>The mirrored geometry.</returns>
     * <group>Transformations</group>
     */
    public static Geom2 MirrorY(Geom2 g, Vec3? origin = null) => Mirror(g, origin: origin, normal: new Vec3(0, 1, 0));

    /**
     * <summary>Mirror the given ogeometry about the X axis.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin" default="(0,0,0)">The origin of the plane.</param>
     * <returns>The mirrored geometry.</returns>
     * <group>Transformations</group>
     */
    public static Geom3 MirrorX(Geom3 g, Vec3? origin = null) => Mirror(g, origin: origin, normal: new Vec3(1, 0, 0));

    /**
     * <summary>Mirror the given geometry about the Y axis.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin" default="(0,0,0)">The origin of the plane.</param>
     * <returns>The mirrored geometry.</returns>
     * <group>Transformations</group>
     */
    public static Geom3 MirrorY(Geom3 g, Vec3? origin = null) => Mirror(g, origin: origin, normal: new Vec3(0, 1, 0));

    /**
     * <summary>Mirror the given geometry about the Z axis.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin" default="(0,0,0)">The origin of the plane.</param>
     * <returns>The mirrored geometry.</returns>
     * <group>Transformations</group>
     */
    public static Geom3 MirrorZ(Geom3 g, Vec3? origin = null) => Mirror(g, origin: origin, normal: new Vec3(0, 0, 1));
}