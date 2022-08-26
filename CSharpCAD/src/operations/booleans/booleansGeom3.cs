namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Return a new geometry representing the total space in the given geometries.</summary>
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
    public static Geom3 Union2(params Geom3[] gobjs)
    {
        if (gobjs.Length < 1)
        {
            throw new ArgumentException("At least 1 geometric object must be given as arguments to union.");
        }

        return UnionGeom3(gobjs);
    }

    /**
     * <summary>Return a new geometry representing space in both the first geometry and
     * all subsequent geometries.</summary>
     *<pre>
     * +-------+
     * |       |
     * |   A   |
     * |    +--+----+   =   +--+
     * +----+--+    |       +--+
     *      |   B   |
     *      |       |
     *      +-------+
     * </pre>
     * <example>
     * var oCommon = Intersect(o1, o2);
     * </example>
     * <group>Boolean Operations</group>
     */
    public static Geom3 Intersect2(params Geom3[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        return IntersectGeom3(gobjs);
    }

    /**
     * <summary>Return a new geometry representing space in the first geometry but
     * not in all subsequent geometries.</summary>
     *<pre>
     * +-------+            +-------+
     * |       |            |       |
     * |   A   |            |       |
     * |    +--+----+   =   |    +--+
     * +----+--+    |       +----+
     *      |   B   |
     *      |       |
     *      +-------+
     * </pre>
     * <example>
     * var oPartMissing = Subtract(o1, o2, o3);
     * </example>
     * <group>Boolean Operations</group>
     */
    public static Geom3 Subtract2(params Geom3[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        return SubtractGeom3(gobjs);
    }
}
