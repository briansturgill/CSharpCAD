namespace CSharpCAD;

public static partial class CSCAD
{
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
    public static Geom2 Subtract2(params Geom2[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        return SubtractGeom2(gobjs);
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
    public static Geom3 Subtract(params Geom3[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        return SubtractGeom3(gobjs);
    }
}