namespace CSharpCAD;

public static partial class Modifiers
{

    public static void TriangulatePolygon(double epsilon, Poly3 polygon, List<Poly3> triangles)
    {
        var nv = polygon.Vertices.Length;
        var vertices = polygon.Vertices;
        if (nv > 3)
        {
            if (nv > 4)
            {
                // split the polygon using a midpoint
                var midpoint = new Vec3(0, 0, 0);
                foreach (var vertex in vertices)
                {
                    midpoint = midpoint.Add(vertex);
                }
                midpoint = midpoint.Divide(new Vec3(nv, nv, nv)).Snap(epsilon);
                for (var i = 0; i < nv; i++)
                {
                    var poly = new Poly3(new Vec3[] { midpoint, vertices[i], vertices[(i + 1) % nv] }, polygon.Color);
                    triangles.Add(poly);
                }
                return;
            }
            // exactly 4 vertices, use simple triangulation
            var poly0 = new Poly3(new Vec3[] { vertices[0], vertices[1], vertices[2] }, polygon.Color);
            var poly1 = new Poly3(new Vec3[] { vertices[0], vertices[2], vertices[3] }, polygon.Color);
            triangles.Add(poly0);
            triangles.Add(poly1);
            return;
        }
        // exactly 3 vertices, so return the original
        triangles.Add(polygon);
    }

    /*
     * Convert the given polygons into a list of triangles (polygons with 3 vertices).
     * NOTE: this is possible because poly3 is CONVEX by definition
     */
    public static Poly3[] triangulatePolygons(double epsilon, Poly3[] polygons)
    {
        var triangles = new List<Poly3>(polygons.Length);
        foreach (var polygon in polygons)
        {
            TriangulatePolygon(epsilon, polygon, triangles);
        }
        return triangles.ToArray();
    }
}
