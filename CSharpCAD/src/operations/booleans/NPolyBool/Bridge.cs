using CSharpCAD.PolyBool;

namespace CSharpCAD;

public static partial class CSCAD
{
    private static Polygon PolyFromGeom2(PolyBool.PolyBool pb, Geom2 gobj)
    {
        var shapesAndHoles = gobj.ToShapesAndHoles();
        Polygon poly = new Polygon(new Region[0], false);
        if (shapesAndHoles.Count == 0)
        {
            return poly;
        }
        var empty = poly;

        foreach (var (shape, holes) in shapesAndHoles)
        {
            var len = holes.Length;
            var regions = new Region[len+1];
            regions[0] = new Region(shape.ToArray()); // Shape
            if (Winding(regions[0].Points) == "ccw") Array.Reverse(regions[0].Points);
            for (var i = 0; i < len; i++) // Holes
            {
                regions[i+1] = new Region(holes[i].ToArray());
            }
            var newpoly = new Polygon(regions, false);
            if (Object.ReferenceEquals(poly, empty))
            {
                poly = newpoly;
            }
            else
            {
                poly = pb.Union(poly, newpoly);
            }
        }
        return poly;
    }

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
        var pb = new PolyBool.PolyBool();
        if (gobjs.Length < 1)
        {
            throw new ArgumentException("At least 1 geometric object must be given as arguments to union.");
        }

        var poly = PolyFromGeom2(pb, gobjs[0]);

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = pb.Union(poly, PolyFromGeom2(pb, gobjs[i]));

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
        var pb = new PolyBool.PolyBool();
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        var poly = PolyFromGeom2(pb, gobjs[0]);

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = pb.Intersect(poly, PolyFromGeom2(pb, gobjs[i]));

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
        var pb = new PolyBool.PolyBool();
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        var poly = PolyFromGeom2(pb, gobjs[0]);

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = pb.Difference(poly, PolyFromGeom2(pb, gobjs[i]));

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
        var prLen = pr.Length;
        if (prLen < 1)
        {
            return new Geom2();
        }
        var nrt = new Geom2.NRTree();
        foreach (var apr in pr)
        {
            nrt.Insert(apr.Points);
        }
        nrt.CorrectWindings();
        return new Geom2(nrt);
    }
}
