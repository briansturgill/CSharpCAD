namespace CSharpCAD;


/*
JSCAD Object to SVG Format Serialization

Notes:
1) geom2 conversion to:
     SVG GROUP containing a continous SVG PATH that contains the outlines of the geometry
2) path2 conversion to:
     SVG GROUP containing a SVG PATH for each path
*/


public static partial class CSCAD
{

    internal static void SerializeToSVG(string file, Geometry g)
    {
        var svg = SerializeToSVG(g);
        System.IO.File.WriteAllText(file, svg);
    }

    /**
     * <summary>Serializer of JSCAD geometries to SVG source (XML).</summary>
     * <remarks>
     * The serialization of the following geometries are possible.
     * - serialization of 2D geometry (geom2) to SVG path (a continous path containing the outlines of the geometry)
     * - serialization of 2D geometry (path2) to SVG path
     * Colors are added to SVG shapes when found on the geometry.
     * Special attributes (id and class) are added to SVG shapes when found on the geometry.
     * </remarks>
     */
    internal static string SerializeToSVG(Geometry g)
    {
        string unit = GlobalParams.Units;

        // convert only 2D geometries
        if (!g.Is2D)
        {
            throw new ArgumentException("Only 2D geometries can be serialized to SVG");
        }

        var geom2 = (Geom2)g;

        // Get the lower and upper bounds of ALL convertable objects
        var (min, max) = geom2.BoundingBox();


        var width = Math.Round(max.x - min.x, 5);
        var height = Math.Round(max.y - min.y, 5);

        var svg = new StringBuilder();
        svg.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        svg.Append("<!-- Created by CSCAD SVG Serializer -->\n");
        svg.Append("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1 Tiny//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny.dtd\">\n");
        svg.Append($"<svg width=\"{width}{unit}\" height=\"{height}{unit}\" viewBox=\"0 0 {width} {height}\" fill=\"none\" fill-rule=\"evenodd\" stroke-width=\"0.1px\" version=\"1.1\" baseProfile=\"tiny\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">\n");

        ConvertGeom2(svg, geom2, min, max);

        svg.Append("</svg>");
        return svg.ToString();
    }


    private static Vec2 Reflect(double x, double y, double px, double py)
    {
        var ox = x - px;
        var oy = y - py;
        if (x == px && y == px) return new Vec2(x, y);
        if (x == px) return new Vec2(x, py - (oy));
        if (y == py) return new Vec2(px - (-ox), y);
        return new Vec2(px - (-ox), py - (oy));
    }

    private static void ConvertGeom2(StringBuilder svg, Geom2 g, Vec2 min, Vec2 max)
    {
        var offset = new Vec2(
            0 - min.x, // offset to X=0
            0 - max.y // offset to Y=0
        );

        var outlines = g.ToOutlines();

        var color = ConvertColor(g.Color);

        foreach (var path in outlines)
        {
            ConvertToContinuousPath(svg, path, offset, color);
        }
    }

    private static void ConvertToContinuousPath(StringBuilder svg, List<Vec2> path, Vec2 offset, string color)
    {
        svg.Append($"<g><path fill=\"{color}\" d=\"");
        ConvertPath(svg, path, offset, true);
        svg.Append("\"/></g>\n");
    }

    private static void ConvertPath(StringBuilder svg, List<Vec2> points, Vec2 offset, bool isClosed)
    {
        var numpointsClosed = points.Count + (isClosed ? 1 : 0);
        for (var pointindex = 0; pointindex < numpointsClosed; pointindex++)
        {
            var pointindexwrapped = pointindex;
            if (pointindexwrapped >= points.Count) pointindexwrapped -= points.Count;
            var point = points[pointindexwrapped];
            var offpoint = new Vec2(point.x + offset.x, point.y + offset.y);
            var svgpoint = Reflect(offpoint.x, offpoint.y, 0, 0);
            var x = Math.Round(svgpoint.x, 5);
            var y = Math.Round(svgpoint.y, 5);
            if (pointindex > 0)
            {
                svg.Append($"L{x} {y}");
            }
            else
            {
                svg.Append($"M{x} {y}");
            }
        }
    }

    private static string ConvertColor(Color? color)
    {
        Color c = color ?? new Color(0, 0, 0); // Works around a C# compiler bug with nullable structs.
        return $"rgba({c.r},{c.g},{c.b},{c.a / 255.0})";
    }

}
