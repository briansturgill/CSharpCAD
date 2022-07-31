using CSharpCAD.PolyboolDotNet;

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
    public static Geom2 Union(params Geom2[] gobjs)
    {
        if (gobjs.Length < 1)
        {
            throw new ArgumentException("At least 1 geometric object must be given as arguments to union.");
        }

        var poly = new Polygon(gobjs[0].ToOutlinesLLV());

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = SegmentSelector.Union(poly, new Polygon(gobjs[i].ToOutlinesLLV()));

        }
        return createGeom2(poly);
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
    public static Geom2 Intersect(params Geom2[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        var poly = new Polygon(gobjs[0].ToOutlinesLLV());

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = SegmentSelector.Intersect(poly, new Polygon(gobjs[i].ToOutlinesLLV()));

        }
        return createGeom2(poly);
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
    public static Geom2 Subtract(params Geom2[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        var poly = new Polygon(gobjs[0].ToOutlinesLLV());

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = SegmentSelector.Difference(poly, new Polygon(gobjs[i].ToOutlinesLLV()));

        }
        return createGeom2(poly);
    }

    private static (Vec2, Vec2) bbox(List<Vec2> poly)
    {
        var min = poly[0];
        var max = poly[0];
        for (int i = 1; i < poly.Count; i++)
        {
            min = poly[i].Min(min);
            max = poly[i].Max(max);
        }
        return (min, max);
    }

    private static bool contains(Vec2 aMin, Vec2 aMax, Vec2 bMin, Vec2 bMax)
    {
        return aMin.X < bMin.X && aMin.Y < bMin.Y && aMax.X > bMax.X && aMax.Y > bMax.Y;
    }

    private static Geom2 createGeom2(Polygon poly)
    {
        var pr = poly.Regions;
        var prLen = pr.Count;
        if (prLen < 1)
        {
            return new Geom2();
        }
        var nrt = new Geom2.NRTree();
        foreach (var apr in pr)
        {
            nrt.Insert(apr.ToArray());
        }
        nrt.ReverseShapes();
        return new Geom2(nrt);
    }
}