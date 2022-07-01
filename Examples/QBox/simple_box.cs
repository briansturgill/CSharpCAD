namespace QBox;

using static QBoxUtils;

public partial class QBox
{

    public Geom3 SimpleBox()
    {
        return Union(
            _make_simple_box_bottom(),
            SimpleBoxBody()
        );
    }

    private Geom3 _make_simple_box_bottom()
    {
        return Translate((-wall, -wall, -wall),
            ExtrudeLinear(height: wall,
                gobj: RoundedRectangle((w + 2 * wall, d + 2 * wall), fillet)));
    }

    public Geom3 SimpleBoxBottom()
    {
        return Union(
            _make_simple_box_bottom(),
            Subtract(
                Translate((lid_inset, lid_inset, 0),
                    ExtrudeLinear(height: 2 * wall + 2,
                        gobj: RoundedRectangle((w - 2 * lid_inset, d - 2 * lid_inset), fillet - wall - lid_inset))),
                Translate((wall + lid_inset, wall + lid_inset, -1),
                    ExtrudeLinear(height: 2 * wall + 4,
                        gobj: RoundedRectangle((w - 2 * wall - 2 * lid_inset, d - 2 * wall - 2 * lid_inset), fillet - 2 * wall - lid_inset)))
            )
        );
    }

    public Geom3 SimpleBoxBody(bool add_posts = true)
    {
        var gobj = Subtract(
            Translate((-wall, -wall, 0),
                ExtrudeLinear(height: h,
                    gobj: RoundedRectangle((w + 2 * wall, d + 2 * wall), fillet))),
            Translate((0, 0, -1),
                ExtrudeLinear(height: h + 2,
                    gobj: RoundedRectangle((w, d), fillet - wall)))
        );
        if (add_posts)
        {
            var post_ctr = fillet - 2;
            // 2 is default post wall size, but we use achor_wall below to anchor solidly
            var anchor_wall = 4;
            gobj = Union(gobj,
                Translate((post_ctr, post_ctr, h - 8),
                    TapPost(8, lid_hole_size, anchor_wall, add_taper: true, z_rot: 180 - 45)),
                Translate((post_ctr, d - post_ctr, h - 8),
                    TapPost(8, lid_hole_size, anchor_wall, add_taper: true, z_rot: 45)),
                Translate((w - post_ctr, post_ctr, h - 8),
                    TapPost(8, lid_hole_size, anchor_wall, add_taper: true, z_rot: 180 + 45)),
                Translate((w - post_ctr, d - post_ctr, h - 8),
                    TapPost(8, lid_hole_size, anchor_wall, add_taper: true, z_rot: -45))
            );
        }
        return gobj;
    }

    public Geom3 SimpleBoxPosts(double h = 8, double sz = -1, bool heat_insert = false)
    {
        if (sz == -1) sz = lid_hole_size;
        var p_wall = fillet - 2 - (sz / 2);
        var p_r = fillet - 2;
        var p_d = p_r * 2;
        var sep_thickness = 0.5;
        var sep_len = p_d;
        var posts = new List<Geom3>();
        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 2; j ++)
            {
                var x = p_d * i * 2;
                var y = p_d * j * 2;
                if (!heat_insert)
                {
                    posts.Add(Translate((x, y, 0), TapPost(h, sz, p_wall)));
                }
                else
                {
                    posts.Add(Translate((x, y, 0), HeatInsertPost(h, sz, p_wall)));
                }
                posts.Add(Translate((x + p_r - 2, y - 1, 0), Cuboid((sep_len + 4, 2, sep_thickness))));
                posts.Add(Translate((x - 1, y + p_r - 2, 0), Cuboid((2, sep_len + 4, sep_thickness))));
            }
        }
        return Union(posts.ToArray());
    }

    public Geom3 SimpleBoxLid()
    {
        var post_ctr = fillet - 2;
        return Subtract(
            Translate((0, 0, wall), _make_simple_box_bottom()),
            Translate((post_ctr, post_ctr, 0),
                Rotate((180, 0, 0),
                    TaperedBoltHole((lid_hole_size / 2.0), 8))),
            Translate((post_ctr, d - post_ctr, 0),
                Rotate((180, 0, 0),
                    TaperedBoltHole((lid_hole_size / 2.0), 8))),
            Translate((w - post_ctr, post_ctr, 0),
                Rotate((180, 0, 0),
                    TaperedBoltHole((lid_hole_size / 2.0), 8))),
            Translate((w - post_ctr, d - post_ctr, 0),
                Rotate((180, 0, 0),
                    TaperedBoltHole((lid_hole_size / 2.0), 8)))
        );
    }
}