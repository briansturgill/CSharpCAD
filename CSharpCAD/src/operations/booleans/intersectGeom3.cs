namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Return a new 3D geometry representing space in both the first geometry and
     * in the subsequent geometries. None of the given geometries are modified.
     * @param {...geom3} geometries - list of 3D geometries
     * @returns {geom3} new 3D geometry
     */
    internal static Geom3 IntersectGeom3(params Geom3[] gobjs)
    {

        var newgobj = gobjs[0];
        for (var i = 1; i < gobjs.Length; i++)
        {
            newgobj = IntersectGeom3Sub(newgobj, gobjs[i]);
        }

        newgobj = Retessellate(newgobj);
        MakePointsStable("Geom3.Intersect", newgobj.ToPolygons());
        return newgobj;
    }
}