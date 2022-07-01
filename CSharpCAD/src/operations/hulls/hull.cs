namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Create a convex hull of the given geometries.</summary>
     * <pre>
     * +-------+           +-------+
     * |       |           |        \
     * |   A   |           |         \
     * |       |           |          \
     * +-------+           +           \
     *                  =   \           \
     *       +-------+       \           +
     *       |       |        \          |
     *       |   B   |         \         |
     *       |       |          \        |
     *       +-------+           +-------+
     * </pre>
     * <example>
     * var g = Hull(gobj1, gobj2, gobj3);
     * </example>
     * <group>Transformations</group>
     */
    public static Geom2 Hull(params Geom2[] geometries)
    {
        if (geometries.Length == 0) throw new ArgumentException("At least one geometry object must be given as an argument.");

        return HullGeom2(geometries);
    }

    /**
     * <summary>Create a convex hull of the given geometries.</summary>
     * <pre>
     * +-------+           +-------+
     * |       |           |        \
     * |   A   |           |         \
     * |       |           |          \
     * +-------+           +           \
     *                  =   \           \
     *       +-------+       \           +
     *       |       |        \          |
     *       |   B   |         \         |
     *       |       |          \        |
     *       +-------+           +-------+
     * </pre>
     * <example>
     * var g = Hull(gobj1, gobj2, gobj3);
     * </example>
     * <group>Transformations</group>
     */
    public static Geom3 Hull(params Geom3[] geometries)
    {
        if (geometries.Length == 0) throw new ArgumentException("At least one geometry object must be given as an argument.");

        return HullGeom3(geometries);
    }
}