namespace CSharpCAD;

public static partial class CSCAD
{
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
    public static Geom2 Intersect(params Geom2[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        return IntersectGeom2(gobjs);
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
    public static Geom3 Intersect(params Geom3[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        return IntersectGeom3(gobjs);
    }
}