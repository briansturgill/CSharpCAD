namespace CSharpCAD;

public static partial class CSCAD
{
    /*
     * <summary>Extrude the given geometry in an upward linear direction.</summary>
     * <remarks>
     * By default the geometry is positioned with its base at z=0 (height/2).
     * It works only on 2D geometry objects with a single path. (No cutouts.)
     * </remarks>
     * <param name="gobj">The 2D geometry to extrude. Must have only one path.</param>
     * <param name="height">The height of the extrusion.</param>
     * <param name="center_z" default="height/2">The Z axis center of the extrusion.</param>
     * <group>3D Primitives</group>
     */
    internal static Geom3 ExtrudeSimple(Geom2 gobj, double height = 1, double? center_z = null)
    {
        if (!gobj.HasOnlyOneConvexPath) throw new ArgumentException("ExtrudeSimple only works with 2D geometry objects have one path (no cutouts).");
        var v2array = gobj.ToPoints();
        return InternalExtrudeSimple(v2array, height, center_z);
    }

    internal static Geom3 InternalExtrudeSimple(Vec2[] v_in, double height, double? center_z)
    {
        if (Equalish(height, 0.0)) throw new ArgumentException("Height cannot be zero.");
        var len = v_in.Length;
        var top = new Vec3[len];
        var bottom = new Vec3[len];
        var polys = new Poly3[len + 2];
        var polys_i = 2;

        var top_most_p = height;
        var bottom_most_p = 0.0;

        if (center_z is not null)
        {
            top_most_p = (height / 2) + (double)center_z;
            bottom_most_p = (-height / 2) + (double)center_z;
        }

        if (height < 0)
        {
            var tmp = top_most_p;
            top_most_p = bottom_most_p;
            bottom_most_p = tmp;
        }

        var v0 = v_in[0];
        var bottom_p = new Vec3(v0, bottom_most_p);
        var top_p = new Vec3(v0, top_most_p);

        for (var i = 0; i < len; i++)
        {
            var bottom_i = len - (i + 1);
            var next_v = v_in[(i + 1) % len];
            var next_bottom_p = new Vec3(next_v, bottom_most_p);
            var next_top_p = new Vec3(next_v, top_most_p);
            bottom[bottom_i] = bottom_p;
            top[i] = top_p;
            polys[polys_i++] = new Poly3(new Vec3[] { bottom_p, next_bottom_p, next_top_p, top_p });
            bottom_p = next_bottom_p;
            top_p = next_top_p;
        }
        polys[0] = new Poly3(bottom);
        polys[1] = new Poly3(top);

        return new Geom3(polys);
    }
}