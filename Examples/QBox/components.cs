namespace QBox;

using static QBoxUtils;

public static partial class QBoxComponents
{
    private const double tiny = 0.01; // Value to add hole depths to fully clear OpenSCAD preview.

    public static Geom3 DopplerMotionSensor()
    {
        return Union(
            Translate((0, 0, 0), TapPost(6, 1.4)),
            Translate((15, 0, 0), TapPost(6, 1.4)),
            Translate((0, 15, 0), TapPost(6, 1.4)),
            Translate((15, 15, 0), TapPost(6, 1.4))
        );
    }

    public static Geom3 RoundBuzzer(double buzzer_d, double hole_d)
    {
        return Subtract(
            ExtrudeLinear(height: 2, gobj: Circle((buzzer_d + 2) / 2.0)),
            Translate((0, 0, -tiny), ExtrudeLinear(height: 2 + 2 * tiny, gobj: Circle(buzzer_d / 2.0)))
        );
    }

    public static Geom3 RoundBuzzerHole(double hole_d, double wall = 2)
    {
        return Hole(hole_d / 2.0, wall: wall);
    }

    // Works best if r is evenly divisible by hole_w
    public static Geom3 CircularSpeakerGridHoles(double r, double h = -1, double hole_w = 2, double wall = 2)
    {
        if (h == -1) h = wall;
        var gobjs = new List<Geom3>();
        var steps = (r / hole_w) + 1;
        var start = -((r % hole_w) / 2);
        for (var i = 0; i < steps; i++)
        {
            var y = -r + start + (i * 2 * hole_w);
            gobjs.Add(Translate((0, y, 0), Cuboid((r * 2, hole_w, h + 2 * tiny), center: (0, 0, 0))));
        }
        return Translate((0, 0, -(h / 2) - tiny), Intersect(
            Union(gobjs.ToArray()),
            Cylinder(radius: r, height: h + 2 * tiny, center: (0,0,0))
        ));
    }

    public static Geom3 Wedge(double sz, double h)
    {
        /*
        return Polyhedron(points: new Points3{
             (0, 0, 0), (0, -sz, 0), (0, 0, sz),
             (h, 0, 0), (h, -sz, 0), (h, 0, sz)},
             faces: new Faces{
                 new Face { 0, 1, 2, 3 },
                 new Face { 5, 4, 3, 2 },
                 new Face { 0, 4, 5, 1 },
                 new Face { 0, 3, 4 },
                 new Face { 5, 2, 1 }
                 }
        );
        */
        return Polyhedron(points: new Points3{
             (0, 0, 0), (0, -sz, 0), (0, 0, sz),
             (h, 0, 0), (h, -sz, 0), (h, 0, sz)},
             faces: new Faces{
                 new Face { 5, 2, 1, 4 },
                 new Face { 3, 0, 2, 5 },
                 new Face { 3, 4, 1, 0 },
                 new Face { 0, 1, 2 },
                 new Face { 5, 4, 3 }
                 }
        );
    }

    public static Geom3 Bme280()
    {
        var wedges = Union(
            Translate((-8, 8, 0), Wedge(8, 2)),
            Translate((-8, 8, 3), Wedge(5, 2)),
            Translate((-8, -8 - 2, 0), Wedge(8, 2)),
            Translate((8 - 2, 8, 3), Wedge(5, 2)),
            Translate((8 - 2, -8 - 2, 0), Wedge(8, 2))
        );
        var bmeBox = Union(
            Translate((0, 0, 3 / 2.0), Subtract(
                Cuboid((12 + 4, 16 + 4, 3), center: (0, 0, 0)),
                Cuboid((12, 16, 3 + 2 * tiny), center: (0, 0, 0))
            )),
            Subtract(
                Translate((-8, -8 - 2, 3), Cuboid((16, 2, 5))),
                Translate((-3 / 2.0, -8 - 2 - tiny, 4), Cuboid((3 + 2 * tiny, 3, 2)))
            ),
            Subtract(
                Translate((-8, 8, 3), Cuboid((16, 2, 5))),
                Translate((-3 / 2.0, 8 - tiny, 4), Cuboid((3 + 2 * tiny, 3, 2)))
            )
        );
        var all = Union(bmeBox, wedges);
        return all;
    }

    public static Geom3 Bme280Marker()
    {
        var small = 0.2;
        return Translate((0, -(((16 + 4) / 2) + small), 0),
            Cuboid((12 + 4 + 2 * small, small * 3, small * 2), center: (0, 0, 0)));
    }

    public static Geom3 Bme280Hole(double wall = 2)
    {
        return Translate(((11.5 / 2) - 3.5, 0, -tiny), Hole(3 / 2, wall: wall));
    }

    public static Geom3 Max9814Mic(double rot = 0)
    {
        return Rotate((0, 0, rot),
            Union(
                Subtract(
                    Translate((-7.5 - 2, -7.5 - 2, 0), Cuboid((15.5 + 4, 26.5 + 4, 6))),
                    Translate((-7.5, -7.5, -tiny), Cuboid((15.5, 26.5, 6 + 2 * tiny)))
                ),
                Translate((-7.5 - 2, -7.5 - 2, 0), Cuboid((15.5 + 4, 26.5, 3))),
                Translate((8, 5, 4), Rotate((0, 0, 0), WireTieLoop())),
                Translate((-9.5, 5, 4), Rotate((0, 0, 0), WireTieLoop()))
            ));
    }

    public static Geom3 Max9814MicHole(double wall = 2)
    {
        return Translate((0, 0, 6 - tiny), Hole(5, wall + 6, wall: wall));
    }

    public static Geom3 WireTieLoop()
    {
        return Translate((1, 3.5, 3), Subtract(
            Cuboid((2, 7, 6), center: (0, 0, 0)),
            Cuboid((2 + 2 * tiny, 3, 2), center: (0, 0, 0))
        ));
    }

    public static Geom3 USBBreakout()
    {
        return Rotate((0, 90, 90),
            Union(
                Translate((-7, -4, 0), TapPost(6, 3)),
                Translate((-7, 4, 0), TapPost(6, 3))
            )
        );
    }

    public static Geom3 USBBreakoutHole(double wall = 2)
    {
        return Translate((0, 6 + 2 + 1.5, 0),
            HorizontalSlotHole(8, 3, wall: wall));
    }
}