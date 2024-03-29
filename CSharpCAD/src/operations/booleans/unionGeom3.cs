namespace CSharpCAD;

public static partial class CSCAD
{
    /*
     * Return a new 3D geometry representing the space in the given 3D geometries.
     * @param {...objects} geometries - list of geometries to union
     * @returns {geom3} new 3D geometry
     */
    private static Geom3 UnionGeom3(params Geom3[] gobjs)
    {
        // combine geometries in a way that forms a balanced binary tree pattern
        var geometries = new List<Geom3>(2 * gobjs.Length);
        foreach (var gobj in gobjs)
        {
            geometries.Add(gobj);
        }
        int i;
        for (i = 1; i < geometries.Count; i += 2)
        {
            geometries.Add(UnionGeom3Sub(geometries[i - 1], geometries[i]));
        }
        var newgeometry = geometries[i - 1];
        return newgeometry;
    }
}