namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Extrude the given geometry in an upward linear direction.</summary>
     * <remarks>
     * By default the geometry is positioned with its base at z=0 (height/2).
     * It works only on 2D geometry objects with a single path. (No cutouts.)
     * Both geometries MUST have the same number of points.
     * </remarks>
     * <param name="gOuter">The exterior 2D geometry outline to extrude. Must have only one path.</param>
     * <param name="gInner">The interior 2D geometry outline to extrude. Must have only one path.</param>
     * <param name="height">The height of the extrusion.</param>
     * <param name="bottom">If zero, then not bottom, otherwise, the height of the bottom.</param>
     * <param name="center_z" default="height/2">The Z axis center of the extrusion.</param>
     * <group>3D Primitives</group>
     */
    public static Geom3 ExtrudeSimpleOutlines(Geometry gOuter, Geometry gInner, double height = 1, double bottom = 0, double? center_z = null)
    {
        if (height < 0.0) throw new ArgumentException("Height must be positive.");
        if (!gOuter.Is2D) throw new ArgumentException("gOuter must be a 2d geometry object.");
        if (!gInner.Is2D) throw new ArgumentException("gInner must be a 2d geometry object.");
        var _gOuter = (Geom2)gOuter;
        var _gInner = (Geom2)gInner;
        if (!_gOuter.HasOnlyOnePath) throw new ArgumentException("gOuter must be a 2D geometry object that has one path (no cutouts).");
        if (!_gInner.HasOnlyOnePath) throw new ArgumentException("gInner must be a 2D geometry object that has one path (no cutouts).");
        var v2arrayOuter = _gOuter.ToPoints();
        var v2arrayInner = _gInner.ToPoints();
        return new Geom3(InternalExtrudeSimpleOutlines(v2arrayOuter, v2arrayInner, height, bottom, center_z).ToArray());
    }

    // InternalExtrudeSimple has one mechanism not exposed in ExtrudeSimpleOutlines.
    // If bottom is negative, then the returned List<Poly3> is missing the bottom polygon, to allow for things like rounded bottoms by internal APIs.
    internal static List<Poly3> InternalExtrudeSimpleOutlines(Vec2[] v_outer, Vec2[] v_inner, double height, double bottom = 0, double? center_z = null)
    {
        if (LessThanOrEqualish(height, 0.0)) throw new ArgumentException("Height must be positive.");
        if (v_outer.Length != v_inner.Length) throw new ArgumentException("Inner and Outer objects must have the same number of points.");
        var len = v_outer.Length; // Length of inner and outer
        var polys = new List<Poly3>(len * 6); // Intential overestimate to avoid allocations.

        var top_most_p = height;
        var bottom_most_p = 0.0;

        if (center_z is not null)
        {
            top_most_p = (height / 2) + (double)center_z;
            bottom_most_p = (-height / 2) + (double)center_z;
        }

        var inner_v0 = v_inner[0];
        var outer_v0 = v_outer[0];
        var bottom_inner_p = new Vec3(inner_v0, bottom_most_p);
        var bottom_outer_p = new Vec3(outer_v0, bottom_most_p);
        var top_inner_p = new Vec3(inner_v0, top_most_p);
        var top_outer_p = new Vec3(outer_v0, top_most_p);

        int i;
        for (i = 0; i < len; i++)
        {
            var next_inner_v = v_inner[(i + 1) % len];
            var next_outer_v = v_outer[(i + 1) % len];
            var next_bottom_inner_p = new Vec3(next_inner_v, bottom_most_p);
            var next_bottom_outer_p = new Vec3(next_outer_v, bottom_most_p);
            var next_top_inner_p = new Vec3(next_inner_v, top_most_p);
            var next_top_outer_p = new Vec3(next_outer_v, top_most_p);
            polys.Add(new Poly3(new Vec3[] { top_outer_p, next_top_outer_p, next_top_inner_p, top_inner_p })); // Top
            if (bottom == 0.0)
            {
                polys.Add(new Poly3(new Vec3[] { next_bottom_inner_p, next_bottom_outer_p, bottom_outer_p, bottom_inner_p })); // Bottom
            }
            //polys[polys_i++] = new Poly3(new Vec3[] { bottom_outer_p, top_outer_p, top_inner_p, bottom_inner_p }); // Left
            polys.Add(new Poly3(new Vec3[] { top_inner_p, next_top_inner_p, next_bottom_inner_p, bottom_inner_p })); // Front
            //polys[polys_i++] = new Poly3(new Vec3[] { next_top_inner_p, next_top_outer_p, next_bottom_outer_p, next_bottom_inner_p }); // Right
            polys.Add(new Poly3(new Vec3[] { next_bottom_outer_p, next_top_outer_p, top_outer_p, bottom_outer_p })); // Back
            bottom_inner_p = next_bottom_inner_p;
            bottom_outer_p = next_bottom_outer_p;
            top_inner_p = next_top_inner_p;
            top_outer_p = next_top_outer_p;
        }

        if (bottom != 0.0)
        {
            var top_of_bottom = 0;
            var bottom_of_bottom = 0 - bottom;
            var bot_topInnerPoints = new Vec3[len];
            var bot_botInnerPoints = new Vec3[len];
            var bot_botOuterPoints = new Vec3[len];
            var pt_inner = v_inner[0];
            var pt_outer = v_outer[0];
            for (i = 0; i < len; i++)
            {
                var next_pt_inner = v_inner[(i + 1) % len];
                var next_pt_outer = v_outer[(i + 1) % len];
                polys.Add(new Poly3(new Vec3[] { new Vec3(pt_outer, bottom_of_bottom), new Vec3(next_pt_outer, bottom_of_bottom),
                    new Vec3(next_pt_outer, top_of_bottom), new Vec3(pt_outer, top_of_bottom) }));
                //polys.Add(new Poly3(new Vec3[] { new Vec3(pt_inner, top_of_bottom), new Vec3(next_pt_inner, top_of_bottom),
                    //new Vec3(next_pt_inner, bottom_of_bottom), new Vec3(pt_inner, bottom_of_bottom) }.Reverse().ToArray()));
                bot_botInnerPoints[len - (i + 1)] = new Vec3(pt_inner, bottom_of_bottom);
                bot_botOuterPoints[len - (i + 1)] = new Vec3(pt_outer, bottom_of_bottom);
                bot_topInnerPoints[i] = new Vec3(pt_inner, top_of_bottom);
                pt_inner = next_pt_inner;
                pt_outer = next_pt_outer;
            }
            polys.Add(new Poly3(bot_topInnerPoints));
            //polys.Add(new Poly3(bot_botInnerPoints));
            polys.Add(new Poly3(bot_botOuterPoints));
        }

        return polys;
    }
}