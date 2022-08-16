using System.Text;
using CSharpCAD;
using static CSharpCAD.CSCAD;
using Path = CSharpCAD.CSCAD.Path;

using System.Diagnostics;

GlobalParams.CheckingEnabled = true;
GlobalParams.DebugOutput = true;

var loops = 100000000;
var watch = new Stopwatch();
loops++;
loops--;
var g = new Geom3();
var g2 = new Geom2();

var se = new SegmentedExtruder(Circle(20));
//se.AddSegment(Circle(10), 100);
se.AddZeroTopCap(50);
g = se.Finished();
g.Validate();
Save("/tmp/se.stl", g);

g=Cone(top: 2, bottom: 20, segments: 12);
Save("/tmp/cone.stl", g);
g=Cone(top: 0, bottom: 20, segments: 12);
Save("/tmp/cone2.stl", g);

var rr = RoundedRectangle((10, 10));

#if LATER
watch.Reset();
watch.Start();
var sum = 0.0;
for (var i = 0; i < loops; i++)
{
    var v = Cos(i);
    sum += v;
}
watch.Stop();
Console.WriteLine($"New RoundRect: {watch.ElapsedMilliseconds}");
watch.Reset();
watch.Start();
sum = 0.0;
for (var i = 0; i < loops; i++)
{
    var v = Math.Round(Cos(i), 15);
    sum += v;
}
watch.Stop();
Console.WriteLine($"Offset RoundRect: {watch.ElapsedMilliseconds}");
#endif

#if LATER
//g=ExtrudeRotate(Translate((20,20), Circle(10)), 32, 0, 270);
//g=ExtrudeRotate(Translate((20,20), Circle(10)), 32, 0, 360);
g=ExtrudeRotate(Semicircle(10, segments: 64, center:(0, 10), startAngle:270, endAngle: 90), 64, 0, 360);
View(g);
//g.Validate();
//g = ExtrudeRotate(Semiellipse((7, 5), center:(0, 5), startAngle: 270, endAngle:90), 32, 0, 360);
//g = Torus();
Save("/tmp/torus.stl", g);
try
{
    g.Validate();
}
catch (ValidationException e)
{
    Console.WriteLine($"Exception: {e.Message}");
}
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

Geom3 TapPost(double h, double size, double post_wall = 2, bool add_taper = false, double z_rot = 0)
{
    var inner_d = size * 0.8; //Possibly snug, but with PLA I prefer that
    var outer_d = post_wall * 2 + size;
    var a = ExtrudeLinear(height: h, gobj: Circle(outer_d / 2.0));
    var b = Translate((0, 0, -1), ExtrudeLinear(height: h + 2, gobj: Circle(inner_d / 2.0)));
    //a = Cylinder(radius: outer_d / 2.0, height: h);
    //b = Translate((0, 0, -1), Cylinder(radius: inner_d / 2.0, height: h + 2));
    //a = Triangulate(a);
    //b = Triangulate(b);
    var gobj = Subtract(a, b);
    //gobj = ExtrudeSimpleOutlines(Circle(outer_d / 2.0), Circle(inner_d / 2.0), height: h);
    //gobj = Triangulate(gobj);
    //gobj.Validate();
    if (add_taper)
    {
        var cout = Circle(outer_d / 2.0);
        var cylout = ExtrudeLinear(height: h * 2, gobj: cout);
        //cylout.Validate();
        var cin = Circle(inner_d / 2.0);
        var cylin = ExtrudeLinear(height: h * 2 + 2, gobj: cin);
        //cylin.Validate();
        //cylout = Cylinder(radius: outer_d / 2.0, height: h * 2);
        //cylin = Translate((0, 0, -1), Cylinder(radius: inner_d / 2.0, height: h * 2 + 2));
        var cb = Cuboid((outer_d, outer_d, h * 3 + 2), center: (0, 0, 0));
        Save("/tmp/cb.stl", cb);
        cb.Validate();
        var g3 = Subtract(cylout, Translate((0, 0, -1), cylin));
        //g3.Validate();
        g3 = Subtract(g3, Rotate((45, 0, z_rot), Translate((0, 0, h), cb)));
        //g3.Validate();
        gobj = Union(gobj, Translate((0, 0, -h * 2), g3));
    }
    return gobj;
}

/*
var a = Cylinder(radius: 4.5, height: 8, segments: 10);
a.Validate();
var b = Translate((0, 0, -1), Cylinder(radius: 2, height: 10, segments: 10));
b.Validate();
var g = Subtract(a, b);// TapPost(8, 5, add_taper: false);
*/
g = TapPost(8, 5, add_taper: true);
Save("/tmp/testtp.stl", g);
//g.Validate();
g = Union(Sphere(radius: 20), Translate((10, 10, 10), Sphere(radius: 20)));
g = Subtract(g, Cuboid((5, 5, 50), center: (0, 0, 0)));
g = Subtract(g, Rotate((45, 0, 0), Cuboid((5, 5, 50), center: (0, 0, 0))));
g = Intersect(g, Translate((10, 10, 10), Torus(outerRadius: 30, innerRadius: 15)));
g = Union(g, Cuboid(size: (10, 10, 10)), Translate((7, 7, 7), Cuboid((10, 10, 10))));
Save("/tmp/test.stl", g);
g = Union(Cube(size: 8, center: (0, 0, 0)), RotateZ(0, Cube(center: (0, 0, 4))));
Save("/tmp/badcube.stl", g);


//g2 = Subtract(Rectangle((8, 4)), Translate((2, 1), Rectangle((4, 2))));
//g2.Validate();
//g2 = Union(g2, Translate((7, 1), Rectangle((2, 2))), Translate((3, 1.5), Rectangle((1, 1))));
g2 = Translate((3, 1.5), Rectangle((1, 1)));
g2.Validate();
g2 = Subtract(g2, Translate((3.15, 1.65), Rectangle((0.20, 0.20))));
g2.Validate();
g2 = Subtract(g2, Translate((3.4, 2.0), Rectangle((0.30, 0.40))));
g2.Validate();
g = ExtrudeLinear(g2, 0.1);
//g.Validate();
Save("/tmp/test3.stl", g);
g2 = Colorize("red", g2);
Save("/tmp/test3.svg", g2);

g2 = Subtract(Rectangle((8, 4)), Translate((2, 1), Rectangle((4, 2))));
g2.Validate();
g2 = Union(g2, Translate((7, 1), Rectangle((2, 2))), Translate((3, 1.5), Rectangle((1, 1))));
g2.Validate();
g2 = Subtract(g2, Translate((3.15, 1.65), Rectangle((0.20, 0.20))));
g2.Validate();
g2 = Subtract(g2, Translate((3.6, 2.0), Rectangle((0.20, 0.40))));
g = ExtrudeLinear(g2, 0.1);
//g.Validate();
Save("/tmp/test2.stl", g);
g2.Validate();
g2 = Colorize("green", g2);
Save("/tmp/test2.svg", g2);

g2 = Subtract(Rectangle((10, 20)), Translate((1, 1), Rectangle((8, 18))));
View(g2, "Test of SVG2 subtract");
g2 = Union(g2, Translate((3, 3), Rectangle((5, 10))));
View(g2, "Test of SVG2 Union");
g2 = Subtract(g2, Translate((4, 4), Rectangle((3, 7))));
//g2 = Union(g2, Translate((3.5, 3.5), Rectangle((5, 10))));
View(g2, "Test of SVG2 Union again");
g2 = Subtract(g2, Translate((0.2, 0.2), Rectangle((0.5, 0.5))));
View(g2, "Test of SVG2 subtract again");
g2 = Subtract(Rectangle((10, 20)), Translate((1, 1), Rectangle((8, 18))));
View(g2, "Test of SVG2 subtract");
g2 = Union(g2, Translate((3, 3), Rectangle((5, 10))));
View(g2, "Test of SVG2 Union");
g2 = Subtract(g2, Translate((4, 4), Rectangle((3, 7))));
View(g2, "Test of SVG2 Union again");
g2 = Subtract(g2, Translate((0.2, 0.2), Rectangle((0.5, 0.5))));
Save("/tmp/test.svg", g2);
g2 = new Geom2();
g2 = Translate((0, 0), Rectangle((2, 10)));
g2 = Union(g2, Translate((0, 0), Rectangle((10, 2))));
g2 = Union(g2, Translate((0, 8), Rectangle((10, 2))));
g2 = Union(g2, Translate((8, 0), Rectangle((2, 10))));
g2 = Union(g2, Translate((3, 3), Rectangle((1, 4))));
g2 = Union(g2, Translate((3, 3), Rectangle((4, 1))));
g2 = Union(g2, Translate((3, 6), Rectangle((4, 1))));
g2 = Union(g2, Translate((6, 3), Rectangle((1, 4))));
View(g2, "Test of SVG2 Union");

/*
Console.WriteLine("---");

foreach (var o in g2.ToOutlines())
{

    Console.Write($"{Winding(o)} ");
    foreach (var v2 in o)
    {
        Console.Write($"{v2}, ");
    }
    Console.WriteLine("");
}
*/
//g.Validate();

g2 = Star(5);
g2.Validate();
View(g2, "Star(5)");
g = ExtrudeLinear(g2, 10);
//g.Validate();
View(g, "Star(5)@10");

#endif

g2 = Subtract(
    RoundedRectangle((40, 40), roundRadius: 10, center: (0, 0), segments:50),
    RoundedRectangle((36, 36), roundRadius: 8, center: (0, 0), segments:50));
View(g2, "Twisty base");
g = ExtrudeTwist(g2, 60, twistAngle: 90, twistSteps: 60);
Save("/tmp/twisty2.stl", g);
g.Validate();

#if LATER
g2 = Offset(Square(20, center: (0, 0)), 10, Corners.Round, segments: 50);
var g2a = Offset(Square(20, center: (0, 0)), 8, Corners.Round, segments: 50);
g2 = Subtract(g2, g2a);
View(g2, "Offset Square");
g = ExtrudeTwist(g2, 60, twistAngle: 90, twistSteps: 60);
Save("/tmp/twisty2.stl", g);

g2 = Subtract(
    RoundedRectangle((40, 40), roundRadius: 10, center: (0, 0), segments:50),
    RoundedRectangle((36, 36), roundRadius: 8, center: (0, 0), segments:50));
g2 = Subtract(
    Rectangle((40, 40)),
    Translate((2, 2), Rectangle((36, 36))));
Save("/tmp/test.svg", g2);
g2 = Intersect(
    Rectangle((40, 20)),
    Rectangle((20, 46)));
View(g2, "Intersect2");
g2 = Union(
    Rectangle((40, 20)),
    Rectangle((20, 46)));
View(g2, "Union2");

Save("/tmp/semicyl.stl", Semicylinder(10, 25, 16, 115, 270));

View(Semicylinder(10, 25, 16, 115, 270), "Simple semicylinder");

Save("/tmp/cyl.stl", Cylinder(10, 25));

View(Cylinder(10, 25), "Simple Cylinder");

g = ExtrudeLinear(Subtract(Circle(10, 8), Circle(5, 8)), 20);
Save("/tmp/test.stl", g);

Console.WriteLine("here");
var outer_rect = Rectangle((2, 2), center: (0, 0));
var inner_rect1 = Rectangle((0.20, 0.20), center: (0, 0));
var inner_rect2 = Translate((0.30, 0.50), Rectangle((0.30, 0.40), center: (0, 0)));
Console.WriteLine("here1");
g2 = Subtract(outer_rect, inner_rect2, inner_rect1);
g2.Validate();
Console.WriteLine("here2");
g = ExtrudeLinear(g2, 0.1);
//g.Validate();
Save("/tmp/test3.stl", g);
Console.WriteLine(g2);

//Save("/tmp/test4.stl", Subtract(Cuboid((2, 2, 0.1), center: (0, 0, 0)), Cuboid((0.2, 0.2, 0.1), center:(0,0,0)), Translate((0.3, 0.5, 0), Cuboid((0.3, 0.4, 0.1), center: (0, 0, 0)))));

g2 = Subtract(Circle(10, 8), Translate((-2, -2), Circle(1, 8)), Translate((2,2), Circle(1, 8)));
g2.Validate();
Save("/tmp/cyl.svg", g2);
g = ExtrudeLinear(g2, 20);
View(g);
Save("/tmp/cyl.stl", g);
#endif

WaitForViewerTransfers();