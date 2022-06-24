using System.Text;
using CSharpCAD;
using static CSharpCAD.CSCAD;
using Path = CSharpCAD.CSCAD.Path;

using System.Diagnostics;

var loops = 100;
var watch = new Stopwatch();
loops++;
loops--;

/*
static int vCount(Geom3 g)
{
    var llv = g.ToPoints();
    var sum = 0;
    foreach (var lv in llv)
    {
        sum += lv.Count;
    }
    return sum;
}
*/

var g2 = new Geom2();
Geometry TapPost(double h, double size, double post_wall = 2, bool add_taper = false, double z_rot = 0)
{
    var inner_d = size * 0.8; //Possibly snug, but with PLA I prefer that
    var outer_d = post_wall * 2 + size;
    var a = ExtrudeLinear(height: h, gobj: Circle(outer_d / 2.0));
    var b = Translate((0, 0, -1), ExtrudeLinear(height: h + 2, gobj: Circle(inner_d / 2.0)));
    a = Cylinder(radius: outer_d / 2.0, height: h);
    b = Translate((0, 0, -1), Cylinder(radius: inner_d / 2.0, height: h + 2));
    var gobj = Subtract(a, b);
    gobj = ExtrudeSimpleOutlines(Circle(outer_d / 2.0), Circle(inner_d / 2.0), height: h);
    if (add_taper)
    {
        gobj = Union(gobj,
            Translate((0, 0, -h * 2), Subtract(
                ExtrudeLinear(height: h * 2, gobj: Circle(outer_d / 2.0)),
                Translate((0, 0, -1), ExtrudeLinear(height: h * 2 + 2, gobj: Circle(inner_d / 2.0))),
                Rotate((45, 0, z_rot), Translate((0, 0, h),
                    Cuboid((outer_d, outer_d, h * 3 + 2), center: (0, 0, 0))))
            ))
        );
    }
    return gobj;
}

var g = TapPost(8, 5, add_taper: true);
Save("/tmp/test.stl", g);
g.Validate();

/*
// Succeeds: var obs = Union(Cube(size: 8, center: (0, 0, 0)), Cube(size: 2, center: (0, 0, 4)));
var obs = Union(Cube(size: 8, center: (0, 0, 0)), Cube(center: (0, 0, 4)));
obs = InsertTjunctions((Geom3)obs);
Save("/tmp/test.stl", obs, binary: false);
obs.Validate();
var llv = ((Geom3)obs).ToPoints();
foreach (var lv in llv)
{
    StringBuilder sb = new StringBuilder();
    foreach (var v in lv)
    {
        sb.Append($"{v},");
    }
        Console.WriteLine($"{sb.ToString()}");
}
*/