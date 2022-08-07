namespace CSharpCAD;

public static partial class CSCAD
{
    /**
    * <summary>Extrude the given geometry in an upward linear direction using the given options.</summary>
    * <param name="gobj">The geometries to extrude.</param>
    * <param name="height">The height of the extrusion.</param>
    * <param name="useEarcut" default="calculated">The height of the extrusion.</param>
    * <remarks>
    * There are two possible triangulators, midpoint is faster but only works on convex geometries, earcut is more flexible.
    * By default a check is made to see if a geometry is convex and if so, midpoint is used.
    * It is possible on that on rather strange geometries that this check can be wrong.
    * You can force it to use Earcut by setting useEarcut to true.
    * Conversely, if you know your Geom2 is convex, you can save the time it takes to check convexity by setting useEarcut to false.
    * </remarks>
    * <group>3D Primitives</group>
    */
    public static Geom3 ExtrudeLinear(Geom2 gobj, double height = 1, bool? useEarcut = null)
    {
        if (gobj.IsEmpty) return new Geom3();
        if (Equalish(height, 0.0)) throw new ArgumentException("Height cannot be zero.");
        var outlines = gobj.ToOutlines();
        var polys = new List<Poly3>();

        // If we have only a simple path, we use midpoint triangulation, else, earcut.

        var top_most_p = height;
        var bottom_most_p = 0.0;

        if (height < 0)
        {
            var tmp = top_most_p;
            top_most_p = bottom_most_p;
            bottom_most_p = tmp;
        }

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
                bottom_p = next_bottom_p;
                top_p = next_top_p;
            }
        }

        // top and bottom polys will be added here.
        var bm_p = bottom_most_p;
        var tm_p = top_most_p;
        var polyList = CSharpCADInternals.TriangulateGeom2(gobj, useEarcut: useEarcut);
        foreach (var p in polyList)
        {
            var v0 = p[0];
            var v1 = p[1];
            var v2 = p[2];
            // bottom -- needs to be reversed
            polys.Add(new Poly3(new Vec3[] { new Vec3(v2, bm_p), new Vec3(v1, bm_p), new Vec3(v0, bm_p) }));
            // top
            polys.Add(new Poly3(new Vec3[] { new Vec3(v0, tm_p), new Vec3(v1, tm_p), new Vec3(v2, tm_p) }));
        }

        return new Geom3(polys.ToArray());
    }
}