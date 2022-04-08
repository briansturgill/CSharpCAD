namespace CSharpCAD;


public static partial class CSCAD
{

    /**
     * <summary>Mirror the given object using the given options.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin">The origin of the plane. Default: new Vec3(0,0,0)</param>
     * <param name="normal">The normal vector of the plane. Default: new Vec3(0,0,1)</param>
     * <returns>The mirrored geometry.</returns>
     */
    public static Geometry Mirror(Geometry g, Vec3? origin = null, Vec3? normal = null)
    {
        var _origin = origin ?? new Vec3(0, 0, 0);
        var _normal = normal ?? new Vec3(0, 0, 1); // Z axis

        var planeOfMirror = new Plane(_normal, _origin);
        // verify the plane, i.e. check that the given normal was valid
        if (double.IsNaN(planeOfMirror.normal.x))
        {
            throw new ArgumentException("The given origin and normal do not define a proper plane");
        }

        var matrix = Mat4.MirrorByPlane(planeOfMirror);

        return g.Is2D ? ((Geom2)g).Transform(matrix) : ((Geom3)g).Transform(matrix);
    }

    /**
     * <summary>Mirror the given ogeometry about the X axis.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin">The origin of the plane. Default: new Vec3(0,0,0)</param>
     * <returns>The mirrored geometry.</returns>
     */
    public static Geometry MirrorX(Geometry g, Vec3? origin = null) => Mirror(g, origin: origin, normal: new Vec3(1, 0, 0));

    /**
     * <summary>Mirror the given geometry about the Y axis.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin">The origin of the plane. Default: new Vec3(0,0,0)</param>
     * <returns>The mirrored geometry.</returns>
     */
    public static Geometry MirrorY(Geometry g, Vec3? origin = null) => Mirror(g, origin: origin, normal: new Vec3(0, 1, 0));

    /**
     * <summary>Mirror the given geometry about the Z axis.</summary>
     * <param name="g">The object to mirror.</param>
     * <param name="origin">The origin of the plane. Default: new Vec3(0,0,0)</param>
     * <returns>The mirrored geometry.</returns>
     */
    public static Geometry MirrorZ(Geometry g, Vec3? origin = null) => Mirror(g, origin: origin, normal: new Vec3(0, 0, 1));
}