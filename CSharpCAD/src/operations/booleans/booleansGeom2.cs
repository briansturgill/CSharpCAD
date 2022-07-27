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
    public static Geom2 Union2(params Geom2[] gobjs)
    {
        if (gobjs.Length < 1)
        {
            throw new ArgumentException("At least 1 geometric object must be given as arguments to union.");
        }

        var poly = gobjs[0].ToMultiPolygon();

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = Geom2Booleans.boolean(poly, gobjs[i].ToMultiPolygon(), Geom2Booleans.UNION);

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
    public static Geom2 Intersect2(params Geom2[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        var poly = gobjs[0].ToMultiPolygon();

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = Geom2Booleans.boolean(poly, gobjs[i].ToMultiPolygon(), Geom2Booleans.INTERSECTION);

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
    public static Geom2 Subtract2(params Geom2[] gobjs)
    {
        if (gobjs.Length == 0) throw new ArgumentException("Must specify at least 1 geometry.");

        var poly = gobjs[0].ToMultiPolygon();

        for (var i = 1; i < gobjs.Length; i++)
        {
            poly = Geom2Booleans.boolean(poly, gobjs[i].ToMultiPolygon(), Geom2Booleans.DIFFERENCE);

        }
        return createGeom2(poly);
    }

    /*
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

    private static List<List<Vec2>> createOutlines(Polygon poly)
    {
        var holes = new List<List<Vec2>>(); // LATER
        var solids = new List<List<Vec2>>(); // LATER
        // poly comes presorted by boundingBox
        var pr = poly.Regions;
        var prLen = pr.Count;
        if (prLen < 1)
        {
            return pr;
        }
        pr.Reverse();
        pr[0].Reverse();
        var solid = true;
        for (var i = 1; i < prLen; i++)
        {
            var a = pr[i - 1];
            var b = pr[i];
            var (aMin, aMax) = bbox(a);
            var (bMin, bMax) = bbox(b);
            if (contains(aMin, aMax, bMin, bMax))
            {
                solid = !solid;
            }
            if (solid)
            {
                pr[i].Reverse();
            }
            if (solid) solids.Add(b); else holes.Add(b); // LATER
            Console.WriteLine($"{solid} {bMin}, {bMax}");
        }
        solids.Add(pr[0]); // LATER
        solids.AddRange(holes); // LATER
        //pr = solids; // LATER
        Console.WriteLine($"Count: {prLen}");
        foreach (var l in pr) { Console.Write($"{Winding(l)} "); foreach (var v in l) Console.Write($"{v}"); Console.WriteLine(""); }
        pr.Reverse();
        return pr;
    }
    */

    private static Geom2 createGeom2(List<List<List<Vec2>>> multipoly)
    {
        var sides = new List<Geom2.Side>();
        foreach (var polys in multipoly)
        {
            for (var i = 0; i < polys.Count; i++)
            {
                var p = polys[i];
                sides.AddRange(new Geom2(p).ToSides());
            }
        }
        return new Geom2(sides.ToArray());
    }
}