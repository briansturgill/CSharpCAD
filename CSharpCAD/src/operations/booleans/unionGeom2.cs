namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Return a new 2D geometry representing the total space in the given 2D geometries.
     * @param {...geom2} geometries - list of 2D geometries to union
     * @returns {geom2} new 2D geometry
     */
    private static Geom2 UnionGeom2(params Geom2[] gobjs)
    {
        var newgobjs = new Geom3[gobjs.Length];

        for (var i = 0; i < gobjs.Length; i++)
        {
            var gobj = gobjs[i];
            newgobjs[i] = To3DWalls(z0: -1, z1: 1, gobj);
        }

        var newgeom3 = UnionGeom3(newgobjs);
        var epsilon = newgeom3.MeasureEpsilon();

        return FromFakePolygons(epsilon, newgeom3.ToPolygons());
    }

}