namespace CSharpCAD;


internal static partial class CSharpCADInternals
{

    // If useEarcut is null, the fuction caluculates the better choice.
    // inverted determines if polygons are reversed.
    internal static List<Vec2[]> TriangulateGeom2(Geom2 gobj, bool? useEarcut = null, bool inverted = false)
    {
        if (useEarcut is null)
        {
            useEarcut = !gobj.HasOnlyOneConvexPath();
        }
        var polys = new List<Vec2[]>();

        if (!(bool)useEarcut)
        {
            var outlines = gobj.ToOutlines();
            Vec2 calcMidpoint(Vec2[] v_in)
            {
                var len = v_in.Length;
                var mp = new Vec2();
                for (var i = 0; i < len; i++)
                {
                    mp = mp.Add(v_in[i]);
                }
                return mp.Divide(new Vec2(len, len));
            }
            var midpoint = calcMidpoint(outlines[0]);

            foreach (var outline in outlines)
            {
                var len = outline.Length;
                for (var i = 0; i < len; i++)
                {
                    var p = outline[i];
                    var next_p = outline[(i + 1) % len];
                    if (inverted)
                    {
                        polys.Add(new Vec2[] { p, midpoint, next_p });
                    }
                    else
                    {
                        polys.Add(new Vec2[] { p, next_p, midpoint });
                    }
                }
            }
        }
        else
        {
            var list = Earcut.DoEarcutList(gobj.ToEarcutNesting());
            if (!inverted) return list;
            foreach (var p in list)
            {
                polys.Add(new Vec2[] { p[2], p[1], p[0] });
            }
        }
        return polys;
    }
}