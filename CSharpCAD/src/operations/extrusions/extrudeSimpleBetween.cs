namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Extrude the from the bottom to the top the given geometries.</summary>
     * <remarks>
     * By default the geometry is positioned with its base at z=0.
     * It works only on 2D geometry objects with a single path. (No cutouts.)
     * Both geometries MUST have the same number of points.
     * </remarks>
     * <param name="top">Top 2D geometry to extrude. Must have only one path.</param>
     * <param name="bottom">Bottom 2D geometry to extrude. Must have only one path.</param>
     * <param name="height">The height of the extrusion.</param>
     * <param name="center_z" default="height/2">The Z axis center of the extrusion.</param>
     * <group>3D Primitives</group>
     */
    public static Geom3 ExtrudeSimpleBetween(Geometry top, Geometry bottom, double height = 1,
        double? center_z = null)
    {
        if (!top.Is2D) throw new ArgumentException("Top geometry must be 2D.");
        if (!bottom.Is2D) throw new ArgumentException("Bottom geometry must be 2D.");
        var _top = (Geom2)top;
        if (!_top.HasOnlyOnePath) throw new ArgumentException("Top geometry must have one path (no cutouts).");
        var _bottom = (Geom2)bottom;
        if (!_bottom.HasOnlyOnePath) throw new ArgumentException("Bottom geometry must have one path (no cutouts).");

        var v2arrayTop = _top.ToPoints();
        var v2arrayBottom = _bottom.ToPoints();
        return InternalExtrudeSimpleBetween(v2arrayTop, v2arrayBottom, height, center_z);
    }

    internal static Geom3 InternalExtrudeSimpleBetween(Vec2[] v_in_top, Vec2[] v_in_bottom,
        double height, double? center_z)
    {
        if (Equalish(height, 0.0)) throw new ArgumentException("Height cannot be zero.");
        if (v_in_bottom.Length != v_in_top.Length) throw new ArgumentException("Top and bottom must have the same number points.");
        var len = v_in_top.Length; // Length of both.
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

        var v0_top = v_in_top[0];
        var v0_bottom = v_in_bottom[0];
        var bottom_p = new Vec3(v0_bottom, bottom_most_p);
        var top_p = new Vec3(v0_top, top_most_p);

        for (var i = 0; i < len; i++)
        {
            var bottom_i = len - (i + 1);
            var next_v_top = v_in_top[(i + 1) % len];
            var next_v_bottom = v_in_bottom[(i + 1) % len];
            var next_bottom_p = new Vec3(next_v_bottom, bottom_most_p);
            var next_top_p = new Vec3(next_v_top, top_most_p);
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