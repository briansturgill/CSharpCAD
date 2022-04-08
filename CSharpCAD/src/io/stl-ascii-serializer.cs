namespace CSharpCAD;

public static partial class CSCAD
{

    // objects must be an array of 3D geomertries (with polygons)
    public static void SerializeToSTLText(string file, Geometry g)
    {
        // convert only 3D geometries
        if (g.Is2D)
        {
            throw new ArgumentException("STL Serialization only works on 3D objects.");
        }
        // convert to triangles
        var object3d = (Geom3)Modifiers.generalize(new Opts { { "snap", true }, { "triangulate", true } }, g);

        var builder = new StringBuilder();
        builder.Append("solid CSCAD\n");
        convertToStl(builder, object3d);
        builder.Append("endsolid CSCAD\n");
        System.IO.File.WriteAllText(file, builder.ToString());
    }

    private static void convertToStl(StringBuilder builder, Geom3 obj)
    {
        convertToFacets(builder, (Geom3)obj);
    }

    public static void convertToFacets(StringBuilder builder, Geom3 obj)
    {
        var polygons = obj.ToPolygons();
        foreach (var polygon in polygons)
        {
            convertToFacet(builder, polygon);
        }
    }

    private static string vector3DtoStlString(Vec3 v) => $"{v.x} {v.y} {v.z}";

    private static string vertextoStlString(Vec3 vertex) => $"vertex {vector3DtoStlString(vertex)}";

    private static void convertToFacet(StringBuilder builder, Poly3 polygon)
    {
        if (polygon.Vertices.Length >= 3)
        {
            // STL requires triangular polygons. If our polygon has more vertices, create multiple triangles:
            var firstVertexStl = vertextoStlString(polygon.Vertices[0]);
            for (var i = 0; i < polygon.Vertices.Length - 2; i++)
            {
                var facet = $@"facet normal {vector3DtoStlString(polygon.Plane().normal)}
outer loop
{firstVertexStl}
{vertextoStlString(polygon.Vertices[i + 1])}
{vertextoStlString(polygon.Vertices[i + 2])}
endloop
endfacet
";
                builder.Append(facet);
            }
        }
    }
}