namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Return a new 2D geometry representing the total space in the given 2D geometries.
     * @param {...geom2} geometries - list of 2D geometries to union
     * @returns {geom2} new 2D geometry
     */
    private static Geom2 UnionGeom2(params Geometry[] gobjs)
    {
        var newgobjs = new Geometry[gobjs.Length];

        for (var i = 0; i < gobjs.Length; i++)
        {
            var gobj = gobjs[i];
            newgobjs[i] = To3DWalls(z0: -1, z1: 1, (Geom2)gobj);
        }

        var newgeom3 = UnionGeom3(newgobjs);
        var epsilon = newgeom3.MeasureEpsilon();

        return FromFakePolygons(epsilon, newgeom3.ToPolygons());
    }

}