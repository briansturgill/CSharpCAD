namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Return a new 3D geometry representing space in this geometry but not in the given geometries.
     * Neither this geometry nor the given geometries are modified.
     * @param {...geom3} geometries - list of geometries
     * @returns {geom3} new 3D geometry
     */
    private static Geom3 SubtractGeom3(params Geom3[] gobjs)
    {
        var newgobj = gobjs[0];
        for (var i = 1; i < gobjs.Length; i++)
        {
            newgobj = SubtractGeom3Sub(newgobj, gobjs[i]);
        }
        newgobj = Retessellate(newgobj);
        MakePointsStable("SubtractGeom3", newgobj.ToPolygons());
        return newgobj;
    }
}