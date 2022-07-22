namespace CSharpCAD;

public static partial class CSCAD
{

    /// <summary>Layer callback for ExtrudeSimpleOutlines.</summary>
    public delegate Geom2? ESO_NextLayer(double offset);
    internal delegate Vec2[]? I_ESO_NextLayer(double offset);

    /*
     * <summary>Extrude the given geometry in an upward linear direction.</summary>
     * <remarks>
     * By default the geometry is positioned with its base at z=0 (height/2).
     * It works only on 2D geometry objects with a single path. (No cutouts.)
     * Both geometries MUST have the same number of points.
     * See the source code for RectangularBox/CylindricalBox for a better understanding of nextLayer.
     * </remarks>
     * <param name="gOuter">The exterior 2D geometry outline to extrude. Must have only one path.</param>
     * <param name="gInner">The interior 2D geometry outline to extrude. Must have only one path.</param>
     * <param name="height">The height of the extrusion.</param>
     * <param name="bottom">If zero, then no bottom, otherwise, the height of the bottom, or of each layer if nextLayer is specified.</param>
     * <param name="nextLayer">Function Geom2? nextLayer(double offset) which you provide to give each layer of the bottom..</param>
     * <param name="center_z" default="height/2">The Z axis center of the extrusion.</param>
     * <example>
     *     beveledCylinderBox = ExtrudeSimpleOutlines(Circle(10, 50), Circle(8, 50), height: 10, bottom: 0.2,
     *          nextLayer: (double offset) => (offset >= 2 ? null:Circle(10-offset, 50)));
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 ExtrudeSimpleOutlines(Geom2 gOuter, Geom2 gInner, double height = 1, double bottom = 0, ESO_NextLayer? nextLayer = null, double? center_z = null)
    {
        if (nextLayer is not null && LessThanOrEqualish(bottom, 0.0)) throw new ArgumentException("If you provide a layer function, height must be greater than zero.");
        if (LessThanOrEqualish(height, 0.0)) throw new ArgumentException("Height must be greater than zero.");
        if (bottom < 0.0) throw new ArgumentException("Argument bottom must be positive.");
        if (!gOuter.HasOnlyOnePath) throw new ArgumentException("gOuter must be a 2D geometry object that has one path (no cutouts).");
        if (!gInner.HasOnlyOnePath) throw new ArgumentException("gInner must be a 2D geometry object that has one path (no cutouts).");
        var v2arrayOuter = gOuter.ToPoints();
        var v2arrayInner = gInner.ToPoints();

        Vec2[]? getVec2(double offset)
        {
            if (nextLayer is null) return null;
            var l = nextLayer(offset);
            if (l is null) return null;
            return l.ToPoints();
        }
        I_ESO_NextLayer? nl = nextLayer is null ? null : getVec2;

        return new Geom3(InternalExtrudeSimpleOutlines(v2arrayOuter, v2arrayInner, height, bottom, nl, center_z).ToArray());
    }

    internal static List<Poly3> InternalExtrudeSimpleOutlines(Vec2[] v_outer, Vec2[] v_inner, double height, double bottom = 0, I_ESO_NextLayer? nextLayer = null, double? center_z = null)
    {
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
        var bot_topInnerPoints = new Vec3[len];

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
            polys.Add(new Poly3(new Vec3[] { top_inner_p, next_top_inner_p, next_bottom_inner_p, bottom_inner_p })); // Front
            polys.Add(new Poly3(new Vec3[] { next_bottom_outer_p, next_top_outer_p, top_outer_p, bottom_outer_p })); // Back
            bot_topInnerPoints[i] = bottom_inner_p;
            bottom_inner_p = next_bottom_inner_p;
            bottom_outer_p = next_bottom_outer_p;
            top_inner_p = next_top_inner_p;
            top_outer_p = next_top_outer_p;
        }

        if (bottom != 0.0) polys.Add(new Poly3(bot_topInnerPoints));

        if (bottom > 0.0 && nextLayer is null)
        {
            var top_of_bottom = 0;
            var bottom_of_bottom = 0 - bottom;
            var bot_botOuterPoints = new Vec3[len];
            var pt_outer = v_outer[0];
            for (i = 0; i < len; i++)
            {
                var next_pt_outer = v_outer[(i + 1) % len];
                polys.Add(new Poly3(new Vec3[] { new Vec3(pt_outer, bottom_of_bottom), new Vec3(next_pt_outer, bottom_of_bottom),
                    new Vec3(next_pt_outer, top_of_bottom), new Vec3(pt_outer, top_of_bottom) }));
                bot_botOuterPoints[len - (i + 1)] = new Vec3(pt_outer, bottom_of_bottom);
                pt_outer = next_pt_outer;
            }
            polys.Add(new Poly3(bot_botOuterPoints));
        }

        if (nextLayer is not null)
        {
            var offset = bottom;
            var topLayerPoints = v_outer;
            var top_of_bottom = 0.0;
            var bottom_of_bottom = 0.0 - offset;
            var lastLayerSides = new Vec3[len];
            while (true)
            {
                var bottomLayerPoints = nextLayer(offset);
                if (bottomLayerPoints is null) break;
                var pt_top = topLayerPoints[0];
                var pt_bottom = bottomLayerPoints[0];
                for (i = 0; i < len; i++)
                {
                    var next_pt_top= topLayerPoints[(i + 1) % len];
                    var next_pt_bottom = bottomLayerPoints[(i + 1) % len];
                    polys.Add(new Poly3(new Vec3[] { new Vec3(pt_bottom, bottom_of_bottom), new Vec3(next_pt_bottom, bottom_of_bottom),
                    new Vec3(next_pt_top, top_of_bottom), new Vec3(pt_top, top_of_bottom) }));
                    lastLayerSides[len - (i + 1)] = new Vec3(pt_bottom, bottom_of_bottom); // We gather every time because we don't know which is last.
                    pt_top = next_pt_top;
                    pt_bottom = next_pt_bottom;
                }
                offset += bottom;
                top_of_bottom -= bottom;
                bottom_of_bottom -= bottom;
                topLayerPoints = bottomLayerPoints;
            }
            polys.Add(new Poly3(lastLayerSides)); // Only need this on the last layer.
        }

        return polys;
    }
}