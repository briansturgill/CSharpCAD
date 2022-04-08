namespace CSharpCAD;

public static partial class CSCAD
{
    private static List<List<Vec3>> createCorners(Vec3 center, Vec3 size, double radius, int segments, int slice, bool positive)
    {
        var pitch = (Math.PI / 2) * slice / segments;
        var cospitch = Math.Cos(pitch);
        var sinpitch = Math.Sin(pitch);

        var layersegments = segments - slice;
        var layerradius = radius * cospitch;
        var layeroffset = size.z - (radius - (radius * sinpitch));
        if (!positive) layeroffset = (radius - (radius * sinpitch)) - size.z;

        layerradius = layerradius > C.EPS ? layerradius : 0;

        var corner0 = center.Add(new Vec3(size.x - radius, size.y - radius, layeroffset));
        var corner1 = center.Add(new Vec3(radius - size.x, size.y - radius, layeroffset));
        var corner2 = center.Add(new Vec3(radius - size.x, radius - size.y, layeroffset));
        var corner3 = center.Add(new Vec3(size.x - radius, radius - size.y, layeroffset));
        var corner0Points = new List<Vec3>();
        var corner1Points = new List<Vec3>();
        var corner2Points = new List<Vec3>();
        var corner3Points = new List<Vec3>();
        for (var i = 0; i <= layersegments; i++)
        {
            var radians = layersegments > 0 ? Math.PI / 2 * i / layersegments : 0;
            var point2d = Vec2.FromAngleRadians(radians);
            point2d = point2d.Scale(layerradius);
            var point3d = new Vec3(point2d.x, point2d.y, 0);
            corner0Points.Add(corner0.Add(point3d));
            point3d = point3d.RotateZ(new Vec3(0, 0, 0), (Math.PI / 2));
            corner1Points.Add(corner1.Add(point3d));
            point3d = point3d.RotateZ(new Vec3(0, 0, 0), (Math.PI / 2));
            corner2Points.Add(corner2.Add(point3d));
            point3d = point3d.RotateZ(new Vec3(0, 0, 0), (Math.PI / 2));
            corner3Points.Add(corner3.Add(point3d));
        }
        if (!positive)
        {
            corner0Points.Reverse();
            corner1Points.Reverse();
            corner2Points.Reverse();
            corner3Points.Reverse();
            return new List<List<Vec3>> { corner3Points, corner2Points, corner1Points, corner0Points };
        }
        return new List<List<Vec3>> { corner0Points, corner1Points, corner2Points, corner3Points };
    }

    private static List<List<Vec3>> stitchCorners(List<List<Vec3>> previousCorners, List<List<Vec3>> currentCorners)
    {
        var polygons = new List<List<Vec3>>();
        for (var i = 0; i < previousCorners.Count; i++)
        {
            var previous = previousCorners[i];
            var current = currentCorners[i];
            for (var j = 0; j < (previous.Count - 1); j++)
            {
                polygons.Add(new List<Vec3> { previous[j], previous[j + 1], current[j] });

                if (j < (current.Count - 1))
                {
                    polygons.Add(new List<Vec3> { current[j], previous[j + 1], current[j + 1] });
                }
            }
        }
        return polygons;
    }

    private static List<List<Vec3>> stitchWalls(List<List<Vec3>> previousCorners, List<List<Vec3>> currentCorners)
    {
        var polygons = new List<List<Vec3>>();
        for (var i = 0; i < previousCorners.Count; i++)
        {
            var previous = previousCorners[i];
            var current = currentCorners[i];
            var p0 = previous[previous.Count - 1];
            var c0 = current[current.Count - 1];

            var j = (i + 1) % previousCorners.Count;
            previous = previousCorners[j];
            current = currentCorners[j];
            var p1 = previous[0];
            var c1 = current[0];

            polygons.Add(new List<Vec3> { p0, p1, c1, c0 });
        }
        return polygons;
    }

    private static List<List<Vec3>> stitchSides(List<List<Vec3>> bottomCorners, List<List<Vec3>> topCorners)
    {
        // make a copy and reverse the bottom corners
        bottomCorners = new List<List<Vec3>> { bottomCorners[3], bottomCorners[2], bottomCorners[1], bottomCorners[0] };
        for (var i = 0; i < bottomCorners.Count; i++)
        {
            bottomCorners[i] = bottomCorners[i].ToList();
            bottomCorners[i].Reverse();
        }

        var bottomPoints = new List<Vec3>();
        foreach (var corner in bottomCorners)
        {
            foreach (var point in corner)
            {
                bottomPoints.Add(point);
            }
        }

        var topPoints = new List<Vec3>();
        foreach (var corner in topCorners)
        {
            foreach (var point in corner)
            {
                topPoints.Add(point);
            }
        }

        var polygons = new List<List<Vec3>>();
        for (var i = 0; i < topPoints.Count; i++)
        {
            var j = (i + 1) % topPoints.Count;
            polygons.Add(new List<Vec3> { bottomPoints[i], bottomPoints[j], topPoints[j], topPoints[i] });
        }
        return polygons;
    }

    /**
     * Construct an axis-aligned solid cuboid in three dimensional space with rounded corners.
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0,0]] - center of rounded cube
     * @param {Array} [options.size=[2,2,2]] - dimension of rounded cube; width, depth, height
     * @param {Number} [options.roundRadius=0.2] - radius of rounded edges
     * @param {Number} [options.segments=32] - number of segments to create per full rotation
     * @returns {geom3} new 3D geometry
     * @alias module:modeling/primitives.roundedCuboid
     *
     * @example
     * var mycube = roundedCuboid({size: [10, 20, 10], roundRadius: 2, segments: 16})
     */
    public static Geom3 RoundedCuboid(Opts opts)
    {
        var center = opts.GetVec3("center", (0.0, 0.0, 0.0));
        var size = opts.GetVec3("size", (2.0, 2.0, 2.0));
        var roundRadius = opts.GetDouble("roundRadius", 0.2);
        var segments = opts.GetInt("segments", 32);

        if (size.x <= 0 || size.y <= 0 || size.z <= 0) throw new ArgumentException("Option size values must be greater than zero.");
        if (roundRadius <= 0) throw new ArgumentException("Option roundRadius must be greater than zero.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        size = new Vec3(size.x / 2, size.y / 2, size.z / 2); // convert to radius;

        if (roundRadius > (size.x - C.EPS) ||
            roundRadius > (size.y - C.EPS) ||
            roundRadius > (size.z - C.EPS)) throw new ArgumentException("Option roundRadius must be smaller then the radius of all dimensions.");

        segments = segments / 4;

        List<List<Vec3>>? prevCornersPos = null;
        List<List<Vec3>>? prevCornersNeg = null;
        var polygons = new List<List<Vec3>>();
        for (var slice = 0; slice <= segments; slice++)
        {
            var cornersPos = createCorners(center, size, roundRadius, segments, slice, true);
            var cornersNeg = createCorners(center, size, roundRadius, segments, slice, false);

            if (slice == 0)
            {
                polygons.AddRange(stitchSides(cornersNeg, cornersPos));
            }

            if (prevCornersPos is not null)
            {
                polygons.AddRange(stitchCorners(prevCornersPos, cornersPos));
                polygons.AddRange(stitchWalls(prevCornersPos, cornersPos));
            }
            if (prevCornersNeg is not null)
            {
                polygons.AddRange(stitchCorners(prevCornersNeg, cornersNeg));
                polygons.AddRange(stitchWalls(prevCornersNeg, cornersNeg));
            }

            if (slice == segments)
            {
                // add the top
                var points = new List<Vec3>(cornersPos.Count);
                foreach (var corner in cornersPos)
                {
                    points.Add(corner[0]);
                }
                polygons.Add(points);
                // add the bottom
                points = new List<Vec3>(cornersNeg.Count);
                foreach (var corner in cornersNeg)
                {
                    points.Add(corner[0]);
                }
                polygons.Add(points);
            }

            prevCornersPos = cornersPos;
            prevCornersNeg = cornersNeg;
        }

        return new Geom3(polygons);
    }
}
