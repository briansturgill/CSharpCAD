namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Create a chain of hulled geometries from the given geometries.</summary>
     * <remarks>
     * Essentially hull A+B, B+C, C+D, etc., then union the results.
     * </remarks>
     * <example>
     * var newshape = HullChain(Rectangle(center: (-5,-5)), Circle(center: (0,0)), Rectangle(center: (5,5)));
     * </example>
     * <pre>
     * +-------+   +-------+     +-------+   +------+
     * |       |   |       |     |        \ /       |
     * |   A   |   |   C   |     |         |        |
     * |       |   |       |     |                  |
     * +-------+   +-------+     +                  +
     *                       =   \                 /
     *       +-------+            \               /
     *       |       |             \             /
     *       |   B   |              \           /
     *       |       |               \         /
     *       +-------+                +-------+
     * </pre>
     * <group>Transformations</group>
     */
    public static Geom2 HullChain(params Geom2[] gobjs)
    {
        if (gobjs.Length < 2) throw new ArgumentException("Wrong number of arguments.");

        var hulls = new List<Geom2>();
        for (var i = 1; i < gobjs.Length; i++)
        {
            hulls.Add(Hull(gobjs[i - 1], gobjs[i]));
        }
        return Union(hulls.ToArray());
    }

    /**
     * <summary>Create a chain of hulled geometries from the given geometries.</summary>
     * <remarks>
     * Essentially hull A+B, B+C, C+D, etc., then union the results.
     * </remarks>
     * <example>
     * var newshape = HullChain(Rectangle(center: (-5,-5)), Circle(center: (0,0)), Rectangle(center: (5,5)));
     * </example>
     * <pre>
     * +-------+   +-------+     +-------+   +------+
     * |       |   |       |     |        \ /       |
     * |   A   |   |   C   |     |         |        |
     * |       |   |       |     |                  |
     * +-------+   +-------+     +                  +
     *                       =   \                 /
     *       +-------+            \               /
     *       |       |             \             /
     *       |   B   |              \           /
     *       |       |               \         /
     *       +-------+                +-------+
     * </pre>
     * <group>Transformations</group>
     */
    public static Geom3 HullChain(params Geom3[] gobjs)
    {
        if (gobjs.Length < 2) throw new ArgumentException("Wrong number of arguments.");

        var hulls = new List<Geom3>();
        for (var i = 1; i < gobjs.Length; i++)
        {
            hulls.Add(Hull(gobjs[i - 1], gobjs[i]));
        }
        return Union(hulls.ToArray());
    }
}