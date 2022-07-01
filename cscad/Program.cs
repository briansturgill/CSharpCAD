﻿using System.Text;
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
        cylout.Validate();
        var cin = Circle(inner_d / 2.0);
        var cylin = ExtrudeLinear(height: h * 2 + 2, gobj: cin);
        cylin.Validate();
        var cb = Cuboid((outer_d, outer_d, h * 3 + 2), center: (0, 0, 0));
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
var g = TapPost(8, 5, add_taper: true);
Save("/tmp/test.stl", g);
//g.Validate();

/*
var llv = g.ToPoints();
prev = new Vec3(Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity);
foreach (var lv in llv)
{
    lv.Sort();
    foreach (var cur in lv)
    {
        if (prev == cur)
        {
            Console.WriteLine($"PTS EXACTLY Equal{prev}");
        }
        if (prev.IsNearlyEqual(cur))
        {
            Console.WriteLine($"PTS Nearly Equal: {prev}, {cur}");
        }
        prev = cur;
    }
}
*/

// Succeeds: var obs = Union(Cube(size: 8, center: (0, 0, 0)), Cube(size: 2, center: (0, 0, 4)));
//var obs = Union(Cube(size: 8, center: (0, 0, 0)), Cube(center: (0, 0, 4)));
//Save("/tmp/test.stl", obs, binary: false);
/*
obs.Validate();
var llv = obs.ToPoints();
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