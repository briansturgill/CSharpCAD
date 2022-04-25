namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Return a new geometry representing the total space in the given geometries.</summary>
     * <remarks>The given geometries should be of the same type, either geom2 or geom3.</remarks>
     *
     * <param name="gobjs">At least 2 geometry objects, all of the same type. (All 2D or 3D).</param>
     * <returns>The new geometry formed the union of all the geometry objects.</returns>
     * <pre>
     * +-------+            +-------+
     * |       |            |       |
     * |   A   |            |       |
     * |    +--+----+   =   |       +----+
     * +----+--+    |       +----+       |
     *      |   B   |            |       |
     *      |       |            |       |
     *      +-------+            +-------+
     * </pre>
     * <example>
     * var oAll = Union(o1, o2, o3, o4);
     * </example>
     * <group>Boolean Operations</group>
     */
    public static Geometry Union(params Geometry[] gobjs)
    {
        if (gobjs.Length < 1)
        {
            throw new ArgumentException("At least 1 geometric objects must be given as arguments to union.");
        }

        var firstWas3D = gobjs[0].Is3D;
        foreach (var g in gobjs)
        {
            if (firstWas3D != g.Is3D)
            {
                throw new ArgumentException("Only unions of geometry objects of the same type (2D or 3D) are supported.");
            }
        }

        if (firstWas3D)
        {
            return UnionGeom3(gobjs);
        }
        return UnionGeom2(gobjs);
    }
}