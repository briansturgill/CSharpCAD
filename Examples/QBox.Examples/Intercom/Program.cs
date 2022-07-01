namespace QBoxExample; 

using CSharpCAD;
using static CSharpCAD.CSCAD;
using QBox;
using static QBox.QBoxUtils;
using static QBox.QBoxComponents;

static class Program
{
    static int segments = 50;
    static int fillet = 7;
    static double w = 120; // Inner box width (X)
    static double d = w / 1.618; // Inner box depth (Y)
    static double h = 65; // Inner box height (Z)


    static bool useMarkers = false; // Use markers instead of printing side wall items.

    static double spkHoleSpacing = 41;

    static QBox qb = new QBox(w, d, h, segments: segments, fillet: fillet);

    static Geom3 Speaker()
    {
        return Union(
            Translate((0, 0, 0), TapPost(6, 3)),
            Translate((spkHoleSpacing, 0, 0), TapPost(6, 3)),
            Translate((0, spkHoleSpacing, 0), TapPost(6, 3)),
            Translate((spkHoleSpacing, spkHoleSpacing, 0), TapPost(6, 3))
        );
    }

    static Geom3 SpeakerHoles()
    {
        var ctr = spkHoleSpacing / 2;
        return Translate((ctr, ctr, 0.01),
            CircularSpeakerGridHoles(20, 3));
    }

    static Geom3 MakeBody()
    {
        return Subtract(
            Union(
                qb.SimpleBoxBody(false),
                !useMarkers ? qb.Left(d / 2, 2 * (h / 3), Bme280()) : new Geom3()
            ),
            qb.Right(d / 2, h - 15, HorizontalSlotHole(12, 7)), // Slot for micro-usb
            useMarkers ? qb.Left(d / 2, 2 * (h / 3), Bme280Marker()) : new Geom3(),
            qb.Left(d / 2, 2 * (h / 3), Bme280Hole())
        );
    }


    static Geom3 MakeBottom()
    {
        var spk_off = (d - spkHoleSpacing) / 2;
        return Subtract(
            Union(
                qb.SimpleBoxBottom(),
                qb.Bottom(spk_off + 8, spk_off, Speaker()),
                qb.Bottom(w - 28, d / 3, Max9814Mic(rot: 90))
            ),
            qb.Bottom(spk_off + 8, spk_off, SpeakerHoles()),
            qb.Bottom(w - 28, 2 * (d / 3), Hole(6)), // 12mm d pushbutton
            qb.Bottom(w - 28, d / 3, Max9814MicHole())
        );
    }

    static Geom3 MakeLid()
    {
        return qb.SimpleBoxLid();
    }

    static void Main(string[] args)
    {
        Save("/tmp/body.stl", MakeBody());
        Save("/tmp/lid.stl", MakeLid());
        Save("/tmp/bottom.stl", MakeBottom());
        var glue = Union(
            Translate((-10, -20, 0), Bme280()),
            qb.SimpleBoxPosts()
        );
        Save("/tmp/glue.stl", glue);
        Save("/tmp/test.stl", TapPost(10, 5, add_taper: true));
    }
}
