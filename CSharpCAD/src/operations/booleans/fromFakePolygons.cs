namespace CSharpCAD;

public static partial class CSCAD
{

    private static Geom2.Side? FromFakePolygon(double epsilon, Poly3 polygon)
    {
        // this can happen based on union, seems to be residuals -
        // return null and handle in caller
        if (polygon.Vertices.Length < 4)
        {
            return null;
        }
        var vertices = polygon.Vertices;
        var len = vertices.Length;
        var vert1Indices = new List<int>(len);
        var points3D = new List<Vec3>(len);
        for (var i = 0; i < len; i++)
        {
            var vertex = vertices[i];
            if (vertex.z > 0) {
              points3D.Add(vertex);
              vert1Indices.Add(i);
            }
        }

        if (points3D.Count != 2)
        {
            throw new Exception("Assertion failed: fromFakePolygon: not enough points found"); // TBD remove later
        }

        var points2D = new List<Vec2>(len);

        foreach (var v3 in points3D)
        {
            var x = Math.Round(v3.x / epsilon) * epsilon + 0; // no more -0
            var y = Math.Round(v3.y / epsilon) * epsilon + 0; // no more -0

            points2D.Add(new Vec2(x, y));
        }


        if (points2D[0] == points2D[1]) { return null; }


        var d = vert1Indices[1] - vert1Indices[0];
        if (d == 1 || d == 3)
        {
            if (d == 1)
            {
                points2D.Reverse();
            }
        }
        else
        {
            throw new Exception("Assertion failed: fromFakePolygon: unknown index ordering");
        }
        return new Geom2.Side(points2D[0], points2D[1]);
    }

    /*
     * Convert the given polygons to a list of sides.
     * The polygons must have only z coordinates +1 and -1, as constructed by to3DWalls().
     */
    private static Geom2 FromFakePolygons(double epsilon, Poly3[] polygons)
    {
        var sides = new List<Geom2.Side>();
        foreach (var polygon in polygons)
        {
            Geom2.Side? side = FromFakePolygon(epsilon, polygon);
            if (side is not null)
            {
                sides.Add(side);
            }
        }
        return new Geom2(sides.ToArray(), new Mat4());
    }

}