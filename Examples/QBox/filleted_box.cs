namespace QBox;

using static QBoxUtils;

public partial class QBox
{
    public Geom3 MakeFilletedBox()
    {
        return Subtract(
            _make_filleted_box(w, d, h, wall, fillet),
            _make_filleted_box_holes()
        );
    }

    private Geom3 _make_filleted_box_holes()
    {
        return Union(
            Left(d * 0.25, h - lid_hole_z_offset,
                Rotate((180, 0, 0), Translate((0, 0, wall),
                    TaperedBoltHole((lid_hole_size / 2.0), 8)))),
            Left(d * 0.75, h - lid_hole_z_offset,
                Rotate((180, 0, 0), Translate((0, 0, wall),
                    TaperedBoltHole((lid_hole_size / 2.0), 8)))),
            Right(d * 0.25, h - lid_hole_z_offset,
                Rotate((180, 0, 0), Translate((0, 0, wall),
                    TaperedBoltHole((lid_hole_size / 2.0), 8)))),
            Right(d * 0.75, h - lid_hole_z_offset,
                Rotate((180, 0, 0), Translate((0, 0, wall),
                    TaperedBoltHole((lid_hole_size / 2.0), 8))))
        );
    }

    private Geom3 _make_filleted_box(double w, double d, double h, double wall, double fillet)
    {
        var gobjs = new List<Geom3>();
        gobjs.Add(
            Subtract(
                Translate((-wall, -wall, 0),
                    ExtrudeLinear(height: h,
                            gobj: RoundedRectangle((w + 2 * wall, d + 2 * wall), fillet))),
                Translate((0, 0, -1),
                    ExtrudeLinear(height: h + 2,
                            gobj: RoundedRectangle((w, d), fillet - wall)))
            )
        );
        var tiny = 0.1;
        for (var iota = 0.1; iota <= 2.0 + tiny; iota += 0.1)
        {
            gobjs.Add(
                Translate((-wall + iota, -wall + iota, -iota),
                    ExtrudeLinear(height: tiny,
                        gobj: RoundedRectangle((w + 2 * (wall - iota), d + 2 * (wall - iota)), fillet - tiny)))
            );
        }
        return Union(gobjs.ToArray());
    }

    private Geom3 _make_filleted_lid_insert(double lid_h, double insert_h)
    {
        return Union(
            Subtract(
                Translate((0, 0, 0),
                    ExtrudeLinear(height: lid_h + insert_h,
                        gobj: RoundedRectangle((w, d), fillet - wall))),
                Translate((wall + lid_inset, wall + lid_inset, -1),
                    ExtrudeLinear(height: lid_h + insert_h + 2,
                        gobj: RoundedRectangle((w - 2 * wall - lid_inset, d - 2 * wall - lid_inset), fillet - 2 * wall))),
                Subtract(
                    Translate((0, 0, lid_h),
                        ExtrudeLinear(height: insert_h,
                            gobj: RoundedRectangle((w, d), fillet - wall))),
                    Translate((lid_inset, lid_inset, lid_h),
                        ExtrudeLinear(height: insert_h + 2,
                            gobj: RoundedRectangle((w - 2 * lid_inset, d - 2 * lid_inset), fillet - wall - lid_inset)))
                )
            ),
            Left(d * 0.25, lid_h + insert_h - lid_hole_z_offset, Translate((0, 0, wall), TapPost(8, lid_hole_size))),
            Left(d * 0.75, lid_h + insert_h - lid_hole_z_offset, Translate((0, 0, wall), TapPost(8, lid_hole_size))),
            Right(d * 0.25, lid_h + insert_h - lid_hole_z_offset, Translate((0, 0, wall), TapPost(8, lid_hole_size))),
            Right(d * 0.75, lid_h + insert_h - lid_hole_z_offset, Translate((0, 0, wall), TapPost(8, lid_hole_size)))
        );
    }

    public Geom3 FilletedShellLid()
    {
        return Subtract(
            Union(
                _make_filleted_box(w, d, lid_h, wall, fillet),
                _make_filleted_lid_insert(lid_h, lid_h)
            ),
            _make_filleted_lid_holes(lid_h, lid_h)
        );
    }

    public Geom3 FilletedFacadeLid(double oversize = 2, double lid_thickness = -1)
    {
        if (lid_thickness == -1) lid_thickness = this.wall;
        return Subtract(
            Union(
                Translate((-wall - oversize, -wall - oversize, -lid_thickness),
                    ExtrudeLinear(height: lid_thickness,
                        gobj: RoundedRectangle((w + 2 * wall + 2 * oversize, d + 2 * wall + 2 * oversize), fillet + oversize))),
                _make_filleted_lid_insert(0, lid_h)
            ),
            _make_filleted_lid_holes(0, lid_h)
        );
    }

    public Geom3 _make_filleted_lid_holes(double lid_h, double insert_h, double wall = 2)
    {
        return Union(
            Left(d * 0.25, lid_h + insert_h - lid_hole_z_offset, Translate((0, 0, wall), Hole(lid_hole_size / 2.0, 8, wall: wall))),
            Left(d * 0.75, lid_h + insert_h - lid_hole_z_offset, Translate((0, 0, wall), Hole(lid_hole_size / 2.0, 8, wall: wall))),
            Right(d * 0.25, lid_h + insert_h - lid_hole_z_offset, Translate((0, 0, wall), Hole(lid_hole_size / 2.0, 8, wall: wall))),
            Right(d * 0.75, lid_h + insert_h - lid_hole_z_offset, Translate((0, 0, wall), Hole(lid_hole_size / 2.0, 8, wall: wall)))
        );
    }
}