namespace CSharpCAD;
public static partial class CSCAD
{

    // see http://en.wikipedia.org/wiki/STL_%28file_format%29#Binary_STL

    internal static void SerializeToSTLBinary(string file, Geometry g)
    {

        // first check if the host is little-endian:
        if (!BitConverter.IsLittleEndian)
        {
            throw new Exception("Binary STL output is currently only supported on little-endian (Intel) processors.");
        }

        if (g.Is2D)
        {
            throw new ArgumentException("STL Serialization only works on 3D objects.");
        }

        var obj = (Geom3)Modifiers.generalize(g, snap: true, triangulate: true);

        uint numtriangles = 0;
        var numpolygons = 0;
        var polygons = obj.ToPolygons();
        foreach (var polygon in polygons)
        {
            var numvertices = polygon.Vertices.Length;
            var thisnumtriangles = (uint)((numvertices >= 3) ? numvertices - 2 : 0);
            numtriangles += thisnumtriangles;
            numpolygons += 1;
        }

        var headerarray = new byte[80]; // Uint8Array(80);
        for (var i = 0; i < 80; i++)
        {
            headerarray[i] = (byte)65;
        }

        var fs = new FileStream(file, FileMode.Create);
        var binW = new BinaryWriter(fs);

        binW.Write(headerarray);
        binW.Write(numtriangles);

        foreach (var polygon in polygons)
        {
            var vertices = polygon.Vertices;
            var numvertices = vertices.Length;
            var normal = polygon.Plane().Normal;
            for (var i = 0; i < numvertices - 2; i++)
            {
                binW.Write((float)normal.x);
                binW.Write((float)normal.y);
                binW.Write((float)normal.z);
                // STL requires triangular polygons. If our polygon has more vertices, create multiple triangles:
                for (var v = 0; v < 3; v++)
                {
                    var vv = v + ((v > 0) ? i : 0);
                    var vertex = vertices[vv];
                    binW.Write((float)vertex.x);
                    binW.Write((float)vertex.y);
                    binW.Write((float)vertex.z);
                }
                binW.Write((ushort)0); // A not used by us attribute value.
            }
        }
        binW.Close();
    }
}