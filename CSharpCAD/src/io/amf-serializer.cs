namespace CSharpCAD;

using System.Xml;

/*
JSCAD Object to AMF (XML) Format Serialization

Notes:
1) geom2 conversion to:
     none
2) geom3 conversion to:
     mesh
3) path2 conversion to:
     none
*/

public static partial class CSCAD
{

    // Serialize the give objects (geometry) to AMF source data (XML).
    internal static void SerializeToAMF(string file, Geom3 g)
    {
        string unit = GlobalParams.Units;
        switch (unit)
        {
            case "millimeter":
            case "mm":
                unit = "millimeter";
                break;
            case "inch":
            case "in":
                unit = "inch";
                break;
            default:
                throw new ArgumentException($"Unexpected Global.Params failure on Units{GlobalParams.Units}");
        }

        // convert to triangles
        var object3d = Modifiers.generalize(g, snap: true, triangulate: true);
        // convert only 3D geometries

        var xml = new XmlTextWriter(file, new UTF8Encoding());

        // construct the contents of the XML
        xml.WriteStartDocument();
        xml.WriteStartElement("amf");
        xml.WriteAttributeString("unit", unit);
        xml.WriteAttributeString("version", "1.1");
        xml.WriteStartElement("metadata");
        xml.WriteAttributeString("type", "author");
        xml.WriteString("Created by CSCAD");
        xml.WriteEndElement(); // metadata

        TranslateObject(xml, object3d);

        xml.WriteEndElement(); // amf
        xml.WriteEndDocument();
        xml.Flush();
        xml.Close();
    }

    private static int id = 0;
    internal static void TranslateObject(XmlTextWriter xml, Geom3 obj)
    {
        var polygons = obj.ToPolygons();
        if (polygons.Length > 0)
        {
            ConvertToObject(xml, obj);
        }
    }

    internal static void ConvertToObject(XmlTextWriter xml, Geom3 obj)
    {
        xml.WriteStartElement("object");
        xml.WriteAttributeString("id", id.ToString());
        id++;
        ConvertToMesh(xml, obj);
        xml.WriteEndElement(); // object
    }

    internal static void ConvertToMesh(XmlTextWriter xml, Geom3 obj)
    {
        xml.WriteStartElement("mesh");
        ConvertToVertices(xml, obj);
        ConvertToVolumes(xml, obj);
        xml.WriteEndElement(); // mesh
    }

    /*
     * This section converts each 3D geometry to a list of vertex / coordinates
     */

    internal static void ConvertToVertices(XmlTextWriter xml, Geom3 obj)
    {
        xml.WriteStartElement("vertices");

        var polygons = obj.ToPolygons();

        foreach (var polygon in polygons)
        {
            var vertices = polygon.ToPoints();
            for (var i = 0; i < vertices.Count; i++)
            {
                ConvertToVertex(xml, vertices[i]);
            }
        }
        xml.WriteEndElement(); // vertices
    }

    internal static void ConvertToVertex(XmlTextWriter xml, Vec3 vertex)
    {
        xml.WriteStartElement("vertex");
        ConvertToCoordinates(xml, vertex);
        xml.WriteEndElement(); // vertex;
    }

    internal static void ConvertToCoordinates(XmlTextWriter xml, Vec3 vertex)
    {
        xml.WriteStartElement("coordinates");
        xml.WriteStartElement("x");
        xml.WriteString($"{vertex.X}");
        xml.WriteEndElement(); // x
        xml.WriteStartElement("y");
        xml.WriteString($"{vertex.Y}");
        xml.WriteEndElement(); // y
        xml.WriteStartElement("z");
        xml.WriteString($"{vertex.Z}");
        xml.WriteEndElement(); // z
        xml.WriteEndElement(); // coordinates
    }

    /*
     * This section converts each 3D geometry to a list of volumes consisting of indexes into the list of vertices
     */

    internal static void ConvertToVolumes(XmlTextWriter xml, Geom3 obj)
    {
        var polygons = obj.ToPolygons();

        xml.WriteStartElement("volume");
        OutputColor(xml, obj.Color); // will add color element if present.
        var vcount = 0;
        foreach (var polygon in polygons)
        {
            var len = polygon.Vertices.Length;
            if (len < 3)
            {
                continue;
            }

            ConvertToTriangles(xml, polygon, vcount);
            vcount += len;
        }
        xml.WriteEndElement(); // volume
    }

    internal static void OutputColor(XmlTextWriter xml, Color? _color)
    {
        if (_color == null) return;
        Color color = _color ?? new Color(0, 0, 0);
        xml.WriteStartElement("color");
        xml.WriteStartElement("r");
        xml.WriteString($"{color.r / 255.0}");
        xml.WriteEndElement(); // r
        xml.WriteStartElement("g");
        xml.WriteString($"{color.g / 255.0}");
        xml.WriteEndElement(); // g
        xml.WriteStartElement("b");
        xml.WriteString($"{color.b / 255.0}");
        xml.WriteEndElement(); // b
        xml.WriteStartElement("a");
        xml.WriteString($"{color.a / 255.0}");
        xml.WriteEndElement(); // a
        xml.WriteEndElement(); // color
    }

    internal static void ConvertToTriangles(XmlTextWriter xml, Poly3 polygon, int index)
    {
        // making sure they are all triangles (triangular polygons)
        for (var i = 0; i < polygon.Vertices.Length - 2; i++)
        {
            xml.WriteStartElement("triangle");
            OutputColor(xml, polygon.Color);
            xml.WriteStartElement("v1");
            xml.WriteString((index + i).ToString());
            xml.WriteEndElement(); // v1
            xml.WriteStartElement("v2");
            xml.WriteString((index + i + 1).ToString());
            xml.WriteEndElement(); // v2
            xml.WriteStartElement("v3");
            xml.WriteString((index + i + 2).ToString());
            xml.WriteEndElement(); // v3
            xml.WriteEndElement(); // triangle
        }
    }
}