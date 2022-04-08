namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Create a convex hull of the given geometries.</summary>
     * <remarks>Currently, the given geometries must be of type Geom2.</remarks>
     * <example>
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
     * </example>
     */
    public static Geom2 Hull(params Geometry[] geometries)
    {
        if (geometries.Length == 0) throw new ArgumentException("At leas one geometry object must be given as an argument.");

        var firstIs3d = geometries[0].Is3D;
        foreach (var g in geometries)
        {
            if (g.Is3D != firstIs3d)
            {
                throw new ArgumentException("All geometries given as arguments must be of the same type (2D or 3D).");
            }
        }
        if (firstIs3d)
        {
            throw new ArgumentException("Sorry, currently only 2D objects are supported.");
        }

        return HullGeom2(geometries);
    }
}