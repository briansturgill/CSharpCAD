namespace CSharpCAD;

public static partial class CSCAD
{
    private static List<List<Vec3>> createCorners(Vec3 center, Vec3 size, double radius, int segments, int slice, bool positive)
    {
        var pitch = (Math.PI / 2) * slice / segments;
        var cospitch = Rezero(Math.Cos(pitch));
        var sinpitch = Rezero(Math.Sin(pitch));

        var layersegments = segments - slice;
        var layerradius = radius * cospitch;
        var layeroffset = size.Z - (radius - (radius * sinpitch));
        if (!positive) layeroffset = (radius - (radius * sinpitch)) - size.Z;

        layerradius = layerradius > C.EPS ? layerradius : 0;

        var corner0 = center.Add(new Vec3(size.X - radius, size.Y - radius, layeroffset));
        var corner1 = center.Add(new Vec3(radius - size.X, size.Y - radius, layeroffset));
        var corner2 = center.Add(new Vec3(radius - size.X, radius - size.Y, layeroffset));
        var corner3 = center.Add(new Vec3(size.X - radius, radius - size.Y, layeroffset));
        var corner0Points = new List<Vec3>();
        var corner1Points = new List<Vec3>();
        var corner2Points = new List<Vec3>();
        var corner3Points = new List<Vec3>();
        for (var i = 0; i <= layersegments; i++)
        {
            var radians = layersegments > 0 ? Math.PI / 2 * i / layersegments : 0;
            var point2d = Vec2.FromAngleRadians(radians);
            point2d = point2d.Scale(layerradius);
            var point3d = new Vec3(point2d.X, point2d.Y, 0);
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
     * The default center point is selected such that the bottom left corner of
     * the cuboid is (0,0,0). (The cuboid is entirely in the first quadrant.)
     * <param name="size" default="(2,2,2)">Dimension of rounded cube: width, depth, height.</param>
     * <param name="roundRadius">Radius of rounded edges.</param>
     * <param name="segments">Number of segments to create per full rotation.</param>
     * <param name="center" default="(size.X/2,size.Y/2,size.Z/2)">Center of rounded cube.</param>
     * <example>
     * var mycube = RoundedCuboid(size: (10, 20, 10), roundRadius: 2, segments: 16);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 RoundedCuboid(Vec3? size = null, double roundRadius = 0.2,
        int segments = 32, Vec3? center = null)
    {
        var _size = size ?? new Vec3(2.0, 2.0, 2.0);
        var _center = center ?? new Vec3(_size.X/2, _size.Y/2, _size.Z/2);

        if (_size.X <= 0 || _size.Y <= 0 || _size.Z <= 0) throw new ArgumentException("Option size values must be greater than zero.");
        if (roundRadius <= 0) throw new ArgumentException("Option roundRadius must be greater than zero.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        _size = new Vec3(_size.X / 2, _size.Y / 2, _size.Z / 2); // convert to radius;

        if (roundRadius > (_size.X - C.EPS) ||
            roundRadius > (_size.Y - C.EPS) ||
            roundRadius > (_size.Z - C.EPS)) throw new ArgumentException("Option roundRadius must be smaller then the radius of all dimensions.");

        segments = segments / 4;

        List<List<Vec3>>? prevCornersPos = null;
        List<List<Vec3>>? prevCornersNeg = null;
        var polygons = new List<List<Vec3>>();
        for (var slice = 0; slice <= segments; slice++)
        {
            var cornersPos = createCorners(_center, _size, roundRadius, segments, slice, true);
            var cornersNeg = createCorners(_center, _size, roundRadius, segments, slice, false);

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
