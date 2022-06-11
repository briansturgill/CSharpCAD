using CSharpCAD;
using static CSharpCAD.CSCAD;
using Path = CSharpCAD.CSCAD.Path;

using System.Diagnostics;

/*
var loops = 100;
var watch = new Stopwatch();
var g2 = new Geom2();
var g3 = new Geom3();

System.Console.WriteLine("Starting");
watch.Start();
for (var i = 0; i < loops; i++)
{
    g2 = Circle(radius: 10, segments: 50, startAngle: Math.PI, endAngle: Math.PI / 2);
}
watch.Stop();
g2.Validate();
System.Console.WriteLine($"Circle with 90 degree cut out via API time for loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g2api.svg", g2);
System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g2 = (Geom2)Subtract(Circle(radius: 10, segments: 50), Rotate((0, 0, 90), Cutter2D(radius: 10, angle: 90)));
}
watch.Stop();
g2.Validate();
System.Console.WriteLine($"Circle with 90 degree cut out via Cutter2D time for loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g2cut.svg", g2);

var g3 = new Geom3();
System.Console.WriteLine("Starting");
watch.Start();
for (var i = 0; i < loops; i++)
{
    g3 = CylinderElliptic(height: 20, startRadius: (10, 10), endRadius: (10, 10), segments: 50, startAngle: Math.PI, endAngle: Math.PI / 2);
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"CylinderElliptic with 90 degree cut out via API time for loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g3api.stl", g3);
System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g3 = (Geom3)Subtract(CylinderElliptic(height: 20, startRadius: (10, 10), endRadius: (10, 10), segments: 50), Rotate((0, 0, 90), Cutter3D(radius: 10, angle: 90)));
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"CylinderElliptic with 90 degree cut out via Cutter3D time for loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g3cut.stl", g3);

System.Console.WriteLine("Starting");
watch.Start();
for (var i = 0; i < loops; i++)
{
    g2 = Circle(radius: 10, segments: 50, startAngle: Math.PI, endAngle: Math.PI / 2);
    g3 = (Geom3)ExtrudeLinear(g2, height: 20);
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Extrude Circle with 90 degree cut out via API time for loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g3eapi.stl", g3);
System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g2 = (Geom2)Subtract(Circle(radius: 10, segments: 50), Rotate((0, 0, 90), Cutter2D(radius: 10, angle: 90)));
    g3 = (Geom3)Translate((0, 0, -10), ExtrudeLinear(g2, height: 20, repair: false));
    g3.ApplyTransforms();
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Extrude Circle with 90 degree cut out via Cutter2D time for loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g3ecut.stl", g3);

System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g2 = (Geom2)Circle(radius: 10, segments: 50);
    g3 = (Geom3)Subtract(Translate((0, 0, -10), ExtrudeLinear(g2, height: 20, repair: false)), Rotate((0, 0, 90), Cutter3D(radius: 10, angle: 90)));
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Extrude Circle with 90 degree cut out via Cutter3D time for loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g3e3Dcut.stl", g3);


loops = 1;
System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g3 = (Geom3)Subtract(Sphere(segments: 32), Translate((0, 0.5, 0.5), Sphere(segments: 32)));
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Sphere 32 loops: {loops}: {watch.ElapsedMilliseconds}ms");
System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g3 = (Geom3)Subtract(Sphere(segments: 32), Translate((0, 0.5, 0.5), Sphere(segments: 64)));
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Sphere 64 loops: {loops}: {watch.ElapsedMilliseconds}ms");
System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g3 = (Geom3)Subtract(Sphere(segments: 32), Translate((0, 0.5, 0.5), Sphere(segments: 128)));
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Sphere 128 loops: {loops}: {watch.ElapsedMilliseconds}ms");


System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g3 = (Geom3)Subtract(Sphere(radius: 10, segments: 32), Cutter3D(radius: 10, angle: 90));
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Subtract(Sphere(radius: 10, segments: 32), Cutter3D(radius: 10, angle: 90)); loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g3Sphere32.stl", g3);
System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g3 = (Geom3)Subtract(Sphere(radius: 10, segments: 64), Cutter3D(radius: 10, angle: 90));
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Subtract(Sphere(radius: 10, segments: 64), Cutter3D(radius: 10, angle: 90)); loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g3Sphere64.stl", g3);
System.Console.WriteLine("Starting");
watch.Reset();
watch.Start();
for (var i = 0; i < loops; i++)
{
    g3 = (Geom3)Subtract(Sphere(radius: 10, segments: 128), Cutter3D(radius: 10, angle: 90));
}
watch.Stop();
g3.Validate();
System.Console.WriteLine($"Subtract(Sphere(radius: 10, segments: 128), Cutter3D(radius: 10, angle: 90)); loops: {loops}: {watch.ElapsedMilliseconds}ms");
Save("/tmp/g3Sphere128.stl", g3);

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
Console.WriteLine($"Sphere(32) {vCount(Sphere(radius: 10, segments: 32))}");
Console.WriteLine($"Sphere(64) {vCount(Sphere(radius: 10, segments: 64))}");
Console.WriteLine($"Sphere(128) {vCount(Sphere(radius: 10, segments: 128))}");

loops = 1;
var maxp2 = Math.Pow(2, 17);
for (var i = 32; i <= maxp2; i *= 2)
{
    System.Console.WriteLine("Starting");
    watch.Reset();
    watch.Start();
    for (var j = 0; j < loops; j++)
    {
        g3 = (Geom3)Subtract(CylinderElliptic(startRadius: (10, 10), endRadius: (10, 10), segments: i), Cutter3D(radius: 10, angle: 90));
    }
    watch.Stop();
    g3.Validate();
    System.Console.WriteLine($"Subtract(CylinderElliptic(startRadius: (10, 10), endRadius: (10, 10), segments: {i}), Cutter3D(radius: 10, angle: 90)); loops: {loops}: {watch.ElapsedMilliseconds}ms");
    Save($"/tmp/g3CyEl{i}.stl", g3);
}

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


for (var i = 32; i <= maxp2; i *= 2)
{
    Console.WriteLine($"CylinderElliptic({i}) {vCount(CylinderElliptic(startRadius: (10, 10), endRadius: (10, 10), segments: i))}");
}
*/

Console.WriteLine("Hello, World!");