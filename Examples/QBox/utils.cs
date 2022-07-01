namespace QBox;

public static partial class QBoxUtils
{
    public static Geom3 TapPost(double h, double size, double post_wall = 2, bool add_taper = false, double z_rot = 0)
    {
        var inner_d = size * 0.8; //Possibly snug, but with PLA I prefer that
        var outer_d = post_wall * 2 + size;
        Geom3 gobj = ExtrudeSimpleOutlines(Circle(outer_d / 2.0), Circle(inner_d / 2.0), height: h);
        if (add_taper)
        {
            gobj = Union(gobj,
                Translate((0, 0, -h * 2), Subtract(
                    ExtrudeSimpleOutlines(Circle(outer_d / 2.0), Circle(inner_d / 2.0), height: h * 2),
                    Rotate((45, 0, z_rot), Translate((0, 0, h),
                        Cuboid((outer_d, outer_d, h * 3 + 2), center: (0, 0, 0))))
                ))
            );
        }
        return gobj;
    }

    public static Geom3 HeatInsertPost(double h, double size, double post_wall = 2)
    {
        var inner_d = size;
        var outer_d = post_wall * 2 + size;
        var inset_r = (size + 2 + 0.15) / 2.0;
        var chamfer_h = Sin(8) * inset_r;
        return Subtract(
                ExtrudeLinear(height: h, gobj: Circle(outer_d / 2.0)),
                Translate((0, 0, -1), ExtrudeLinear(height: h + 2, gobj: Circle(inner_d / 2.0))),
                Translate((0, 0, -1), Cone(height: h + 2,
                    bottom: inset_r, top: inset_r - Cos(8) * inset_r))
                );
    }

    public static Geom3 HorizontalSlotHole(double sw, double sh, double wall = 2)
    {
        return Union(
            Translate((-(sw - sh) / 2.0, 0, 0), Hole(sh / 2.0, wall)),
            CenteredRectHole(sw - sh, sh, wall),
            Translate(((sw - sh) / 2.0, 0, 0), Hole(sh / 2.0, wall))
        );
    }

    public static Geom3 TaperedBoltHole(double r, double h)
    {
        var bolt_top_r = r * 2;
        var chamfer_h = Sin(45) * bolt_top_r;
        var h_chamfered = chamfer_h > h ? h : chamfer_h;
        var h_remaining = chamfer_h > h ? 0 : h - chamfer_h;
        return Rotate((180, 0, 0),
            Translate((0, 0, -1),
                Union(
                    Cylinder(height: 1, radius: bolt_top_r),
                    Translate((0, 0, 1),
                        Cone(height: h_chamfered, bottom: bolt_top_r, top: r)),
                    Translate((0, 0, h_chamfered),
                        Cylinder(height: h_remaining + 2, radius: r * 0.80))
                )
            )
        );
    }

    public static Geom3 Hole(double r, double h = -1, double wall = 2)
    {
        if (h == -1) h = wall;
        return Rotate((180, 0, 0), Translate((0, 0, -1), ExtrudeLinear(height: h + 2, gobj: Circle(r))));
    }

    public static Geom3 CenteredRectHole(double w, double d, double h = -1, double wall = 2)
    {
        if (h == -1) h = wall;
        return Rotate((180, 0, 0), Translate((0, 0, -1), ExtrudeLinear(height: h + 2, gobj: Rectangle((w, d), center: (0, 0)))));
    }
}