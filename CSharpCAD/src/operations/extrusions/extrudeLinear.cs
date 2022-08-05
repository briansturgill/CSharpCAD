namespace CSharpCAD;

public static partial class CSCAD
{
    /**
    * <summary>Extrude the given geometry in an upward linear direction using the given options.</summary>
    * <param name="gobj">The geometries to extrude.</param>
    * <param name="height">The height of the extrusion.</param>
    * <group>3D Primitives</group>
    */
    public static Geom3 ExtrudeLinear(Geom2 gobj, double height = 1)
    {
        if (gobj.IsEmpty) return new Geom3();
        if (Equalish(height, 0.0)) throw new ArgumentException("Height cannot be zero.");
        var outlines = gobj.ToOutlines();
        var polys = new List<Poly3>();

        // If we have only a simple path, we use midpoint triangulation, else, earcut.
        var useEarcut = !gobj.HasOnlyOneConvexPath;

        var midpoint = new Vec2();

        useEarcut = true; // LATER resolve
        if (!useEarcut)
        {
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
            midpoint = calcMidpoint(outlines[0]);
        }

        var top_most_p = height;
        var bottom_most_p = 0.0;

        if (height < 0)
        {
            var tmp = top_most_p;
            top_most_p = bottom_most_p;
            bottom_most_p = tmp;
        }

        var bottom_mp = new Vec3(midpoint, bottom_most_p);
        var top_mp = new Vec3(midpoint, top_most_p);

        foreach (var outline in outlines)
        {
            var v0 = outline[0];
            var bottom_p = new Vec3(v0, bottom_most_p);
            var top_p = new Vec3(v0, top_most_p);
            var olen = outline.Length;
            for (var i = 0; i < olen; i++)
            {
                var next_v = outline[(i + 1) % olen];
                var next_bottom_p = new Vec3(next_v, bottom_most_p);
                var next_top_p = new Vec3(next_v, top_most_p);
                polys.Add(new Poly3(new Vec3[] { bottom_p, next_bottom_p, next_top_p }));
                polys.Add(new Poly3(new Vec3[] { bottom_p, next_top_p, top_p }));
                if (!useEarcut)
                {
                    polys.Add(new Poly3(new Vec3[] { bottom_p, bottom_mp, next_bottom_p }));
                    polys.Add(new Poly3(new Vec3[] { top_p, next_top_p, top_mp }));
                }
                bottom_p = next_bottom_p;
                top_p = next_top_p;
            }
        }

        if (useEarcut)
        {
            var shapesAndHoles = gobj.ToEarcutNesting();
            // top and bottom polys will be added here.
            Earcut.DoEarcutCaps(shapesAndHoles, polys, bottom_most_p, top_most_p);
        }

        return new Geom3(polys.ToArray());
    }
}