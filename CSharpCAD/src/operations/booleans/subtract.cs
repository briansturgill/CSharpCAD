namespace CSharpCAD;

public static partial class CSCAD
{

    /**
     * <summary>Return a new geometry representing space in the first geometry but
     * not in all subsequent geometries.</summary>
     * <remarks>The given geometries should be of the same type, either geom2 or geom3.</remarks>
     *<example>
     * +-------+            +-------+
     * |       |            |       |
     * |   A   |            |       |
     * |    +--+----+   =   |    +--+
     * +----+--+    |       +----+
     *      |   B   |
     *      |       |
     *      +-------+
     * </example>
     */
    public static Geometry Subtract(params Geometry[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        var firstWas3D = gobjs[0].Is3D;
        foreach (var g in gobjs)
        {
            if (firstWas3D != g.Is3D)
            {
                throw new ArgumentException("Only the subtract of geometry objects of the same type (2D or 3D) are supported.");
            }
        }

        if (firstWas3D)
        {
            return SubtractGeom3(gobjs);
        }
        return SubtractGeom2(gobjs);
    }
}