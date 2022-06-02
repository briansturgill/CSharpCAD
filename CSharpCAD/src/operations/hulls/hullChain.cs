namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Create a chain of hulled geometries from the given geometries.</summary>
     * <remarks>
     * Essentially hull A+B, B+C, C+D, etc., then union the results.
     * The given geometries should be of the same type, either geom2 or geom3.
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
     */
    public static Geometry HullChain(params Geometry[] gobjs)
    {
        if (gobjs.Length < 2) throw new ArgumentException("Wrong number of arguments.");

        var hulls = new List<Geometry>();
        for (var i = 1; i < gobjs.Length; i++)
        {
            hulls.Add(Hull(gobjs[i - 1], gobjs[i]));
        }
        return Union(hulls.ToArray());
    }
}