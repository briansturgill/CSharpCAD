namespace CSharpCAD;

public static partial class CSCAD
{
    /*
     * Return a new 3D geometry representing the space in the given geometries.
     * @param {geom3} geometry1 - geometry to union
     * @param {geom3} geometry2 - geometry to union
     * @returns {goem3} new 3D geometry
     */
    private static Geom3 UnionGeom3Sub(Geom3 geometry1, Geom3 geometry2)
    {
        if (!MayOverlap(geometry1, geometry2))
        {
            return UnionForNonIntersecting(geometry1, geometry2);
        }

        var a = new Tree(geometry1.ToPolygons());
        var b = new Tree(geometry2.ToPolygons());

        a.ClipTo(b, false);
        // b.clipTo(a, true); // ERROR: doesn't work
        b.ClipTo(a);
        b.Invert();
        b.ClipTo(a);
        b.Invert();

        List<Poly3> polys = a.AllPolygons();
        polys.AddRange(b.AllPolygons());
        var newpolys = Fix3DBooBoos("UnionGeom3", polys);
        return new Geom3(newpolys, new Mat4(), geometry1.Color, isRetesselated: true);
    }

    // Like union, but when we know that the two solids are not intersecting
    // Do not use if you are not completely sure that the solids do not intersect!
    private static Geom3 UnionForNonIntersecting(Geom3 geometry1, Geom3 geometry2)
    {
        var g1polys = geometry1.ToPolygons();
        var g2polys = geometry2.ToPolygons();
        var newpolygons = new Poly3[g1polys.Length + g2polys.Length];
        Array.Copy(g1polys, newpolygons, g1polys.Length);
        Array.Copy(g2polys, 0, newpolygons, g1polys.Length, g2polys.Length);
        return new Geom3(newpolygons, new Mat4(), geometry1.Color);
    }

}