namespace CSharpCAD;

public static partial class CSCAD
{
    /*
     * Create a convex hull of the given geometries (geom3).
     * @param {...geometries} geometries - list of geom3 geometries
     * @returns {geom3} new geometry
     */
    internal static Geom3 HullGeom3(params Geometry[] gobjs)
    {
        if (gobjs.Length == 1) return (Geom3)gobjs[0];

        // extract the unique vertices from the gobjs
        var uniqueVertices = new HashSet<Vec3>();
        foreach (var gobj in gobjs)
        {
            var polys = ((Geom3)gobj).ToPolygons();
            foreach (var p in polys)
            {
                foreach (var v in p.Vertices)
                {
                    uniqueVertices.Add(v);
                }
            }
        }

        var polygons = RunQuickHull(uniqueVertices, skipTriangulation: true);

        return new Geom3(polygons);
    }
}