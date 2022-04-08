namespace CSharpCAD;

public static partial class Modifiers
{
    public static Geom2 snap(Geom2 geometry)
    {
        var epsilon = geometry.MeasureEpsilon();
        var sides = geometry.ToSides();
        var newsides = new List<Geom2.Side>(sides.Length);

        for (var i = 0; i < sides.Length; i++)
        {
            var side_0 = sides[i].v0;
            var side_1 = sides[i].v1;
            side_0 = side_0.Snap(epsilon);
            side_1 = side_1.Snap(epsilon);
            if (side_0 != side_1)
            {
                newsides.Add(new Geom2.Side(side_0, side_1));
            }
        }
        // snap can produce sides with zero (0) length, remove those
        return new Geom2(newsides.ToArray(), geometry.Transforms, geometry.Color);
    }

    public static Geom3 snap(Geom3 geometry)
    {
        var epsilon = geometry.MeasureEpsilon();
        var polygons = geometry.ToPolygons();
        var newpolygons = snapPolygons(epsilon, polygons);
        return new Geom3(newpolygons, geometry.Transforms, geometry.Color);
    }

    public static bool isValidPoly3(double epsilon, Poly3 polygon)
    {
        var area = Math.Abs(polygon.Area());
        return (double.IsFinite(area) && area > epsilon);
    }

    /*
     * Snap the given list of polygons to the epsilon.
     */
    public static Poly3[] snapPolygons(double epsilon, Poly3[] polygons)
    {
        var epsilonArea = (epsilon * epsilon * Math.Sqrt(3) / 4);
        var newpolygons = new List<Poly3>(polygons.Length);
        foreach (var polygon in polygons)
        {
            var p = polygon.Vertices;
            var len = p.Length;
            var snapvertices = new Vec3[len];
            for (int i = 0; i < len; i++)
            {
                snapvertices[i] = p[i].Snap(epsilon);
            }
            // only retain unique vertices
            var newvertices = new List<Vec3>(len);
            for (var i = 0; i < snapvertices.Length; i++)
            {
                var j = (i + 1) % snapvertices.Length;
                if (snapvertices[i] != snapvertices[j]) newvertices.Add(snapvertices[i]);
            }
            var newpolygon = new Poly3(newvertices.ToArray(), polygon.Color);
            // snap can produce polygons with zero (0) area, remove those
            if (isValidPoly3(epsilonArea, newpolygon))
            {
                newpolygons.Add(newpolygon);
            }
        }
        return newpolygons.ToArray();
    }

}