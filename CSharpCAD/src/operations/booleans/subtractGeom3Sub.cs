namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Return a new 3D geometry representing the space in the first geometry but not
     * in the second geometry. None of the given geometries are modified.
     * @param {geom3} geometry1 - a geometry
     * @param {geom3} geometry2 - a geometry
     * @returns {geom3} new 3D geometry
     */
    private static Geom3 SubtractGeom3Sub(Geom3 geometry1, Geom3 geometry2)
    {
        if (!MayOverlap(geometry1, geometry2))
        {
            return geometry1.Clone();
        }

        var a = new Tree(geometry1.ToPolygons());
        var b = new Tree(geometry2.ToPolygons());

        a.Invert();
        a.ClipTo(b);
        b.ClipTo(a, true);
        a.AddPolygons(b.AllPolygons().ToArray());
        a.Invert();

        var newpolygons = a.AllPolygons();
        return new Geom3(newpolygons.ToArray());
    }
}