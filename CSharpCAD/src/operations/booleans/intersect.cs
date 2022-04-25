namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Return a new geometry representing space in both the first geometry and
     * all subsequent geometries.</summary>
     * <remarks>The given geometries should be of the same type, either geom2 or geom3.</remarks>
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
    public static Geometry Intersect(params Geometry[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        var firstWas3D = gobjs[0].Is3D;
        foreach (var g in gobjs)
        {
            if (firstWas3D != g.Is3D)
            {
                throw new ArgumentException("Only the intersect of geometry objects of the same type (2D or 3D) are supported.");
            }
        }

        if (firstWas3D)
        {
            return IntersectGeom3(gobjs);
        }
        return IntersectGeom2(gobjs);
    }

}