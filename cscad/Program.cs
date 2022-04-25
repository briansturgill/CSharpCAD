using CSharpCAD;
using static CSharpCAD.CSCAD;
using Path = CSharpCAD.CSCAD.Path;

using System.Diagnostics;
// See https://aka.ms/new-console-template for more information

//Console.WriteLine($"Is Vector hardware enabled? {Vector.IsHardwareAccelerated}");


/*
Circle(new Opts {{"radius", 5}});
var circle = Circle(new Opts {{"radius", 5}});

var l = new List<object>();
var r = 10;
var space = 3;
var move_y = r*2+space;
var move_x = r*2+space;
var end_x = move_x * 10;
var cur_x = 0;
var cur_y = 0;
foreach (var nc in GetColorNamesByRainbow()) {
    var (n, c) = nc;
    var o = Colorize(c, Circle(new Opts{{"radius", r}}));
    var m = Mat4.FromTranslation(new Vec3(cur_x, -cur_y, 0));
    o = ((Geom2)o).Transform(m);
    l.Add(o);
    cur_x += move_x;
    if (cur_x > end_x) {
        cur_y += move_y;
        cur_x = 0;
    }
}

var l1 =new Vec3(1,1,1);
var l2 =new Vec3(6, 6, 6);
var pt =new Vec3(5,5,5);
var b = Modifiers.closestPoint((l1, l2), pt);
//save("/tmp/t.svg", l.ToArray());

Mirror(circle);

Save("/tmp/t.stl", Torus(innerRadius:5, outerRadius:8));
//Save("/tmp/t.stl", Colorize((0,255,0), Cuboid(new Opts{{"size", (10, 20, 5)}})));

*/
var watch = new Stopwatch();
var circ1 = Circle(radius: 5, segments: 100);
var circ2 = Circle(radius: 5, segments: 100);
var circ3 = Circle(radius: 5, segments: 100);
var circ4 = Circle(radius: 5, segments: 100);
var circs = Union(
  Translate(new Vec3(2.5, 2.5, 0), circ1),
  Translate(new Vec3(2.5, 20 + 2.5, 0), circ2),
  Translate(new Vec3(30 + 2.5, 2.5, 0), circ3),
  Translate(new Vec3(30 + 2.5, 20 + 2.5, 0), circ4)
);
var loops = 10000;
Geom2 h = new Geom2();
System.Console.WriteLine("Starting");
watch.Start();
for (var i = 0; i < loops; i++)
{
    h = Hull(circs);
}
watch.Stop();
System.Console.WriteLine($"Hull time for {loops}: {watch.ElapsedMilliseconds}ms");
System.Console.WriteLine($"Hull has {h.ToPoints().Length} points.");
Save("/tmp/t.svg", h);

var c = Cuboid(size: (10, 20, 5));
var o = Colorize((128, 128, 0), circ4);
Save("/tmp/t.svg", o);

var g = CylinderElliptic(height: 30, startRadius: (5, 3), endRadius: (3, 5));
g = Ellipsoid(radius: (10, 5, 20), segments: 50);
Save("/tmp/t.stl", g);

var points = new Points2 {
        // roof
        (10,11), (0,11), (5,20),
        // wall
        (0,0), (10,0), (10,10), (0,10)
       };
var paths = new Paths {
         new Path { 0, 1, 2},
         new Path { 3, 4, 5, 6}
       };

var poly = Polygon(points: points, paths: paths);
     poly.Validate(); // Will throw an exception with explanatory message if polygon is bad
Save("/tmp/t.svg", poly);
var mypoints = new Points3 { (10, 10, 0), (10, -10, 0), (-10, -10, 0), (-10, 10, 0), (0, 0, 10) };
var myfaces = new Faces { new Face {0, 1, 4}, new Face { 1, 2, 4 },
    new Face {2, 3, 4}, new Face{3, 0, 4}, new Face {1, 0, 3}, new Face{2, 1, 3} };
var myshape = Polyhedron(points: mypoints, faces: myfaces, orientationOutward: false);

Save("/tmp/t.stl", myshape);