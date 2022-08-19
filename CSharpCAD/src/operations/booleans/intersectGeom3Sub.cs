namespace CSharpCAD;


public static partial class CSCAD
{
    /*
     * Return a new 3D geometry representing the space in both the first geometry and
     * the second geometry. None of the given geometries are modified.
     * @param {geom3} geometry1 - a geometry
     * @param {geom3} geometry2 - a geometry
     * @returns {geom3} new 3D geometry
     */
    private static Geom3 IntersectGeom3Sub(Geom3 geometry1, Geom3 geometry2)
    {
        if (!MayOverlap(geometry1, geometry2))
        {
            return new Geom3(); // empty geometry
        }

        var a = new Tree(geometry1.ToPolygons());
        var b = new Tree(geometry2.ToPolygons());

        a.Invert();
        b.ClipTo(a);
        b.Invert();
        a.ClipTo(b);
        b.ClipTo(a);
        a.AddPolygons(b.AllPolygons().ToArray());
        a.Invert();

        var polys = a.AllPolygons();
        var newpolys = Fix3DBooBoos("IntersectGeom3", polys);
        return new Geom3(newpolys);
    }
}