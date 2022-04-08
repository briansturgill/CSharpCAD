namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Return a new 2D geometry representing space in the first geometry but
     * not in the subsequent geometries. None of the given geometries are modified.
     * @param {...geom2} geometries - list of geometries
     * @returns {geom2} new 2D geometry
     */
    internal static Geom2 SubtractGeom2(params Geometry[] gobjs)
    {
        var newgobjs = new Geometry[gobjs.Length];

        for (var i = 0; i < gobjs.Length; i++)
        {
            var gobj = gobjs[i];
            newgobjs[i] = To3DWalls(z0: -1, z1: 1, (Geom2)gobj);
        }

        var newgeom3 = SubtractGeom3(newgobjs);
        var epsilon = newgeom3.MeasureEpsilon();

        return FromFakePolygons(epsilon, newgeom3.ToPolygons());
    }
}