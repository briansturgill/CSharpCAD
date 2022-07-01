namespace CSharpCAD;

public static partial class CSCAD
{

    // objects must be an array of 3D geomertries (with polygons)
    internal static void SerializeToSTLText(string file, Geom3 g)
    {
        // convert to triangles
        var object3d = Modifiers.generalize(g, snap: true, triangulate: true);

        var builder = new StringBuilder();
        builder.Append("solid CSCAD\n");
        ConvertToStl(builder, object3d);
        builder.Append("endsolid CSCAD\n");
        System.IO.File.WriteAllText(file, builder.ToString());
    }

    private static void ConvertToStl(StringBuilder builder, Geom3 obj)
    {
        ConvertToFacets(builder, obj);
    }

    internal static void ConvertToFacets(StringBuilder builder, Geom3 obj)
    {
        var polygons = obj.ToPolygons();
        foreach (var polygon in polygons)
        {
            ConvertToFacet(builder, polygon);
        }
    }

    private static string Vector3DtoStlString(Vec3 v) => $"{v.X} {v.Y} {v.Z}";

    private static string VertextoStlString(Vec3 vertex) => $"vertex {Vector3DtoStlString(vertex)}";

    private static void ConvertToFacet(StringBuilder builder, Poly3 polygon)
    {
        if (polygon.Vertices.Length >= 3)
        {
            // STL requires triangular polygons. If our polygon has more vertices, create multiple triangles:
            var firstVertexStl = VertextoStlString(polygon.Vertices[0]);
            for (var i = 0; i < polygon.Vertices.Length - 2; i++)
            {
                var facet = $@"facet normal {Vector3DtoStlString(polygon.Plane().Normal)}
outer loop
{firstVertexStl}
{VertextoStlString(polygon.Vertices[i + 1])}
{VertextoStlString(polygon.Vertices[i + 2])}
endloop
endfacet
";
                builder.Append(facet);
            }
        }
    }
}