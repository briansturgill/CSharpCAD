namespace CSharpCAD;

internal static partial class CSharpCADInternals
{
    /*
     * <summary>Expand and extrude the given geometry (geom2).
     * @see expand for additional options
     * @param {Object} options - options for extrusion, if any
     * @param {Number} [options.size=1] - size of the rectangle
     * @param {Number} [options.height=1] - height of the extrusion
     * @param {geom2} geometry - the geometry to extrude
     * @return {geom3} the extruded geometry
     */
    internal static Geom3 ExtrudeRectangularGeom2(Geom2 gobj, double size = 1, double height = 1,
        double twistAngle = 0, int twistSteps = 12, Corners corners = Corners.Edge, int segments = 16, bool repair = true)
    {
        // convert the geometry to outlines
        var outlines = gobj.ToOutlines();
        if (outlines.Count == 0) throw new ArgumentException("The given geometry object cannot be empty");

        // expand the outlines
        var newParts = new List<Geom2>();
        foreach (var outline in outlines)
        {
            if (AreaVec2(outline) < 0) outline.Reverse();
            var points = outline;
            MakePointsLikePath2(points);
            var external = OffsetFromPoints(points, delta: size, corners: corners, segments: segments, closed: true);
            var @internal = OffsetFromPoints(points, delta: -size, corners: corners, segments: segments, closed: true);
            if (AreaVec2(external) < 0)
            {
                external.Reverse();
            }
            else
            {
                @internal.Reverse();
            }
            MakePointsLikePath2(external);
            MakePointsLikePath2(@internal);
            var externalSides = new Geom2(external).ToSides();
            var internalSides = new Geom2(@internal).ToSides();
            var bothSides = new Geom2.Side[externalSides.Length+internalSides.Length];
            Array.Copy(externalSides, bothSides, externalSides.Length);
            Array.Copy(internalSides, 0, bothSides, externalSides.Length, internalSides.Length);
            newParts.Add(new Geom2(bothSides));
        }

        var allSides = new List<Geom2.Side>();
        foreach(var part in newParts) {
          allSides.AddRange(part.ToSides());
        }
        var newGobj = new Geom2(allSides.ToArray());
        //var newGobj = Expand(gobj, delta: size, corners: corners, segments: segments);

        return ExtrudeLinearGeom2(newGobj, new Vec3(0, 0, height), twistAngle, twistSteps, repair);
    }

    internal static void MakePointsLikePath2(List<Vec2> points)
    {
        if (points.Count > 1)
        {
            var p0 = points[0];
            var pn = points[points.Count - 1];
            while (p0.Distance(pn) < C.EPS * C.EPS)
            {
                points.RemoveAt(points.Count - 1);
                if (points.Count == 1) break;
                pn = points[points.Count - 1];
            }
        }
    }
}