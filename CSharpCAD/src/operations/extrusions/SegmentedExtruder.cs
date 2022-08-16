namespace CSharpCAD;

/**
    <summary>Class returned by extrudeSegmented.</summary>
    <remarks>
        It's methods allow the building of a custom extrusion.
        Generally this is for advanced users.
        Other extrusions, such as extrudeTwist, use this to provide easier to use extruders.
        Take a look at their source code if you want to understand how it works.

        SegmentExtruder always builds from bottom to top.
    </remarks>
*/
public class SegmentedExtruder
{
    double initialZ;
    double currentZ;

    List<Poly3> polys = new List<Poly3>();

    private bool? useEarcut;
    private bool alreadyHasBottomCap = false;
    private bool alreadyHasTopCap = false;
    private Geom2 currentGeom2;

    /**
        <summary>Construct a segment extruder.</summary>
        <remarks>
        SegmentExtruder always builds from bottom to top.

        Triangulation note:

        If you know your all your shapes will always be convex, then setting useEarcut to true will
        speed things up (by bypassing convexity check and using midpoint instead of Earcut).

        If you know there is a problem with using midpoint, you can force the use of Earcut, saving the time of the
        algorithmic check of convexity.
        </remarks>
        <param name="initialGobj">The shape of the bottom of the extrusion.</param>
        <param name="initialZ">The Z value for the bottom of the extrusion.</param>
        <param name="useEarcut">By default triangulation algorithm will be decided by a convexity check.</param>
     */
    public SegmentedExtruder(Geom2 initialGobj, double initialZ = 0.0, bool? useEarcut = null)
    {
        this.initialZ = initialZ;
        this.currentZ = initialZ;
        this.useEarcut = useEarcut;
        this.currentGeom2 = initialGobj;
    }

    // Can only be used where Earcut is not needed.
    internal SegmentedExtruder(Vec2[] initialPath, double initialZ = 0.0)
    {
        this.initialZ = initialZ;
        this.currentZ = initialZ;
        this.useEarcut = false;
        this.currentGeom2 = new Geom2(initialPath);
    }

    private void addSegment(Geom2 seg, double height)
    {
        var topOutlines = seg.ToOutlines();
        var bottomOutlines = currentGeom2.ToOutlines();
        var olen = topOutlines.Length;
        for (var idx = 0; idx < olen; idx++)
        {
            var top = topOutlines[idx];
            var bottom = bottomOutlines[idx];
            var v0_top = top[0];
            var v0_bottom = bottom[0];
            var bottom_p = new Vec3(v0_bottom, currentZ);
            var top_p = new Vec3(v0_top, currentZ + height);
            var len = top.Length;
            for (var i = 0; i < len; i++)
            {
                var next_top_v = top[(i + 1) % len];
                var next_bottom_v = bottom[(i + 1) % len];
                var next_bottom_p = new Vec3(next_bottom_v, currentZ);
                var next_top_p = new Vec3(next_top_v, currentZ + height);
                polys.Add(new Poly3(new Vec3[] { bottom_p, next_bottom_p, next_top_p }));
                polys.Add(new Poly3(new Vec3[] { bottom_p, next_top_p, top_p }));
                bottom_p = next_bottom_p;
                top_p = next_top_p;
            }
        }
        currentGeom2 = seg;
        currentZ += height;
    }

    private void addBottomCap()
    {
        if (alreadyHasBottomCap) throw new ValidationException("Attempt to add another bottom cap.");
        alreadyHasBottomCap = true;
        var polyList = CSharpCADInternals.TriangulateGeom2(currentGeom2, useEarcut: this.useEarcut);
        foreach (var p in polyList)
        {
            var v0 = p[0];
            var v1 = p[1];
            var v2 = p[2];
            // bottom -- needs to be reversed
            polys.Add(new Poly3(new Vec3[] { new Vec3(v2, initialZ), new Vec3(v1, initialZ), new Vec3(v0, initialZ) }));
        }
    }

    private void addTopCap()
    {
        if (alreadyHasTopCap) throw new ValidationException("Attempt to add another top cap.");
        alreadyHasTopCap = true;
        var polyList = CSharpCADInternals.TriangulateGeom2(currentGeom2, useEarcut: this.useEarcut);
        foreach (var p in polyList)
        {
            var v0 = p[0];
            var v1 = p[1];
            var v2 = p[2];
            // top
            polys.Add(new Poly3(new Vec3[] { new Vec3(v0, currentZ), new Vec3(v1, currentZ), new Vec3(v2, currentZ) }));
        }
    }

    /// <summary>Make current extrusion end in a single point.</summary>
    public void AddZeroTopCap(double height)
    {
        if (alreadyHasTopCap) throw new ValidationException("Attempt to add another top cap.");
        if (!this.alreadyHasBottomCap)
        {
            addBottomCap();
        }
        alreadyHasTopCap = true;
        var points = currentGeom2.ToSinglePath();
        Vec2 calcMidpoint(Vec2[] v_in)
        {
            var len = v_in.Length;
            var mp = new Vec2();
            for (var i = 0; i < len; i++)
            {
                mp = mp.Add(v_in[i]);
            }
            return mp.Divide(new Vec2(len, len));
        }
        var mp = calcMidpoint(points);
        var len = points.Length;
        for (var i = 0; i < len; i++)
        {
            var v0 = points[i];
            var v1 = points[(i + 1) % len];
            // top
            polys.Add(new Poly3(new Vec3[] { new Vec3(v0, currentZ), new Vec3(v1, currentZ), new Vec3(mp, currentZ + height) }));
        }
    }

    /**
        <summary>Add a segment, a solid shape, to the extrusion.</summary>
        <remarks>
            Each added segment is stacked on top of the preceeding one.

            If you call this without making a bottom a flat bottom is created and the segment is attached to it.
        </remarks>
        <params name="shape">The shape of the solid object that is extruded.</params>
        <params name="height">The height of the solid object.</params>
    */
    public void AddSegment(Geom2 shape, double height)
    {
        if (height <= 0) throw new ArgumentException("Height must be a positive number.");
        if (shape.IsEmpty) throw new ArgumentException("Empty shapes cannot be extruded.");
        if (this.alreadyHasTopCap) throw new ArgumentException("Cannot add segments to extrusions that already have top caps.");
        if (!this.alreadyHasBottomCap)
        {
            addBottomCap();
        }
        addSegment(shape, height);
    }

    internal void AddCappedWall(Vec2[] outerShape, Vec2[] innerShape, double height, Vec2[]? insetShape, double insetHeight)
    {
        // Need to handle bottom cap
        // Must have top cap.
    }

    /**
        <summary>Add a "wall": a "box" with the central portion removed.</summary>
        <remarks>
            This extrusion must be the last segment.

            The shapes used must have only one path each.

            The "wall" is the portion between the outer and inner shape.

            The number of points in the outer shape must be the same as in the inner shape.

            This call will cap the central portion as a "floor".
            This call will make a top cap for the "wall".

            If you call this without making a bottom, then there will be no floor
            and the bottom of the wall is capped in a similar fashion to the top.
            Essentially you'll end up with a box-shaped pipe.
        </remarks>
        <params name="outerShape">The outer shape of the wall.</params>
        <params name="innerShape">The inner shape of the wall.</params>
        <params name="height">The height of the wall.</params>
        <params name="insetShape">The inner, inner shape of the wall of a fitted lid.</params>
        <params name="insetHeight">The extra (beyond wall height) height of the inset.</params>
    */
    public void AddCappedWall(Geom2 outerShape, Geom2 innerShape, double height, Geom2? insetShape = null, double insetHeight = 10)
    {
        if (height <= 0) throw new ArgumentException("Height must be a positive number.");
        if (!outerShape.HasOnlyOnePath()) throw new ArgumentException("Outer shape must have only one path.");
        if (!innerShape.HasOnlyOnePath()) throw new ArgumentException("Inner shape must have only one path.");
        if (insetShape is not null && !insetShape.HasOnlyOnePath()) throw new ArgumentException("Inset shape must have only one path.");
        this.AddCappedWall(outerShape.ToSinglePath(), innerShape.ToSinglePath(), height,
            insetShape is not null ? insetShape.ToSinglePath() : null, insetHeight);
    }

    /**
        <summary>Call when finished adding segments to retrieve the 3D geometry object extruded.</summary>
    */
    public Geom3 Finished()
    {
        if (!alreadyHasBottomCap) return new Geom3();
        if (!alreadyHasTopCap) addTopCap();
        return new Geom3(polys.ToArray());
    }
}
