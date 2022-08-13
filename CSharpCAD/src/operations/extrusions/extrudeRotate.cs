namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Rotate extrude the given geometry using the given options.</summary>
     * <param name="gobj">The 2D geometry to extrude.</param>
     * <param name="angle">Angle of the extrusion (DEGREES in total extrusion).</param>
     * <param name="startAngle">Start angle of the extrusion (DEGREES).</param>
     * <param name="segments">Number of segments in a full (360 degree) extrusion.</param>
     * <param name="makeZeroCap" default="false">Set to true if you don't want the inner hole.</param>
     * <remarks>
     * All X points in gobj must be positive (it must be located in the first and fourth quadrants).
     * The best way to make things work at X==0 is make sure that the first and last points of the outer path of gobj both equal 0.
     * We set makeZeroCap automatically if the first and last point have X==0.
     * </remarks>
     * <returns>The extruded 3D geometry</returns>
     * <group>3D Primitives</group>
     */
    public static Geom3 ExtrudeRotate(Geom2 gobj, int segments = 32, double startAngle = 0, double angle = 360, bool? makeZeroCap = null)
    {
        if (segments < 5) throw new ArgumentException("Segments must be greater than or equal to 5.");
        if (GreaterThanish(angle, 360)) throw new ArgumentException("Argument angle must be less than or equal to 360.");
        if (LessThanOrEqualish(angle, 0)) throw new ArgumentException("Argument angle must be greater than 0.");
        if (GreaterThanOrEqualish(startAngle, 360)) throw new ArgumentException("Argument startAngle must be less than 360.");
        if (LessThanish(startAngle, 0)) throw new ArgumentException("Argument startAngle must be greater than or equal to 0.");
        if (gobj.IsEmpty) throw new ArgumentException("Argument gobj cannot be empty.");

        var (min, max) = gobj.BoundingBox();
        if (LessThanish(min.X, 0)) throw new ArgumentException("Negative X points are present in argument gobj.");

        if ((makeZeroCap is null || (bool)makeZeroCap) && Equalish(min.X, 0))
        {
            var nrtree = new Geom2.NRTree();
            var outlines = gobj.ToOutlines();
            if (Equalish(outlines[0][0].X, 0) && Equalish(outlines[0][outlines[0].Length - 1].X, 0))
            {
                //outlines[0][outlines[0].Length - 1] = new Vec2(0.001, outlines[0][outlines[0].Length - 1].Y);
                //outlines[0][0] = new Vec2(0.001, outlines[0][0].Y);
                makeZeroCap = true;
            }
            foreach (var outline in outlines)
            {
                var olen = outline.Length;
                var newo = new Vec2[olen];
                for (var i = 0; i < olen; i++)
                {
                    var pt = outline[i];
                    var x = pt.X;
                    if (Equalish(x, 0)) x = 0.001;
                    var y = pt.Y;
                    if (Equalish(y, 0)) y = 0;
                    newo[i] = new Vec2(x, y);
                }
                nrtree.Insert(newo);
            }
            gobj = new Geom2(nrtree);
        }

        if (makeZeroCap is null) makeZeroCap = false;

        var closed = Equalish(angle, 360);

        startAngle = DegToRad(startAngle);
        angle = DegToRad(angle);

        var polys = new List<Poly3>();

        var slices = Math.Round(segments * (angle / (Math.PI * 2))) + 1;

        var first_slice = gobj;

        var rotationPerSlice = angle / (slices - 1);

        var prev_slice3d = make3dSlice(first_slice, startAngle);
        var cur_slice = first_slice; // Need to initialize it to something
        for (var i = 1; i < slices; i++) // Note starts on 1 because first_slice is prev.
        {
            var rot = 0.0;
            if (i == slices - 1) // Making sure we end precisely
            {
                if (closed)
                {
                    rot = startAngle;
                }
                else
                {
                    rot = startAngle + angle;
                }
            }
            else
            {
                rot = startAngle + rotationPerSlice * i;
            }
            var cur_slice3d = make3dSlice(first_slice, rot);
            connect3dSlices(prev_slice3d, cur_slice3d, polys, (bool)makeZeroCap);
            prev_slice3d = cur_slice3d;
        }

        if (!closed)
        {
            // top and bottom polys will be added here.
            var lastCapPolyList = CSharpCADInternals.TriangulateGeom2(first_slice);
            var firstCapPolyList = new List<Vec2[]>(lastCapPolyList.Count);
            var len = lastCapPolyList.Count;
            for (var i = 0; i < len; i++)
            {
                firstCapPolyList.Add(lastCapPolyList[i].ToArray());
                Array.Reverse(firstCapPolyList[i]);
            }

            createCapFromPolys(lastCapPolyList, startAngle, polys);
            createCapFromPolys(firstCapPolyList, angle + startAngle, polys);

            return new Geom3(polys.ToArray());
        }

        var apolys = polys.ToArray();
        return new Geom3(apolys);
    }

    private static void createCapFromPolys(List<Vec2[]> lv2, double rot, List<Poly3> polys)
    {
        var mat = Mat4.FromZRotation(rot).Multiply(rotateX90);
        foreach (var poly in lv2)
        {
            int i = 0;
            var av3 = new Vec3[3];
            foreach (var pt in poly)
            {
                var pt3d = new Vec3(pt.X, pt.Y, 0);
                pt3d = pt3d.Transform(mat);
                av3[i++] = pt3d;
            }
            polys.Add(new Poly3(av3));
        }
    }

    private static void connect3dSlices(List<List<Vec3>> prev, List<List<Vec3>> cur, List<Poly3> polys, bool makeZeroCap)
    {
        var olen = cur.Count;
        for (var i = 0; i < olen; i++)
        {
            var pline = prev[i];
            var cline = cur[i];
            var ppt = pline[0];
            var cpt = cline[0];
            var len = pline.Count;
            for (var j = 0; j < len; j++)
            {
                var idx = (j + 1) % len;
                var next_cpt = cline[idx];
                var next_ppt = pline[idx];
                if (makeZeroCap && idx == 0)
                {
                    Console.WriteLine($"{ppt}, {cpt}, {next_ppt}, {next_cpt}");
                    polys.Add(new Poly3(new Vec3[] { ppt, cpt, new Vec3(0, 0, ppt.Z) }));
                    polys.Add(new Poly3(new Vec3[] { next_cpt, next_ppt, new Vec3(0, 0, next_cpt.Z) }));
                    Console.WriteLine(polys[polys.Count - 1]);
                    Console.WriteLine(polys[polys.Count - 2]);
                    break; // Supresses polys below so that X=0 triangle is not made.
                }
                polys.Add(new Poly3(new Vec3[] { next_cpt, next_ppt, ppt, }));
                polys.Add(new Poly3(new Vec3[] { cpt, next_cpt, ppt }));
                ppt = next_ppt;
                cpt = next_cpt;
            }
        }
    }

    private static Mat4 rotateX90 = Mat4.FromXRotation(Math.PI / 2);
    private static List<List<Vec3>> make3dSlice(Geom2 slice2d, double rot)
    {
        var outlines = slice2d.ToOutlines();
        var slice3d = new List<List<Vec3>>(outlines.Length);
        var mat = Mat4.FromZRotation(rot).Multiply(rotateX90);
        foreach (var outline in outlines)
        {
            var l3d = new List<Vec3>(outline.Length);
            foreach (var pt in outline)
            {
                var pt3d = new Vec3(pt.X, pt.Y, 0);
                pt3d = pt3d.Transform(mat);
                l3d.Add(pt3d);
            }
            slice3d.Add(l3d);
        }
        return slice3d;
    }
}
