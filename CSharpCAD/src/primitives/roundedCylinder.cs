namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * Construct a Z axis-aligned solid cylinder in three dimensional space with rounded ends.
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0,0]] - center of cylinder
     * @param {Number} [options.height=2] - height of cylinder
     * @param {Number} [options.radius=1] - radius of cylinder
     * @param {Number} [options.roundRadius=0.2] - radius of rounded edges
     * @param {Number} [options.segments=32] - number of segments to create per full rotation
     * @returns {geom3} new 3D geometry
     * @alias module:modeling/primitives.roundedCylinder
     *
     * @example
     * var myshape = roundedCylinder({ height: 10, radius: 2, roundRadius: 0.5 })
     * <group>3D Primitives</group>
     */
    public static Geom3 RoundedCylinder(double height = 2, double radius = 1,
        double roundRadius = 0.2, int segments = 32, Vec3? center = null)
    {
        var _center = center ?? new Vec3(0, 0, 0);

        if (height <= 0) throw new ArgumentException("Option height must be greater then zero.");
        if (radius <= 0) throw new ArgumentException("Option radius must be greater then zero.");
        if (roundRadius <= 0) throw new ArgumentException("Option roundRadius must be greater then zero.");
        if (roundRadius > (radius - C.EPS)) throw new ArgumentException("Option roundRadius must be smaller then the radius.");
        if (segments < 4) throw new ArgumentException("segments must be four or more");

        var start = new Vec3(0, 0, -(height / 2));
        var end = new Vec3(0, 0, height / 2);
        var direction = end.Subtract(start);
        var length = direction.Length();

        if ((2 * roundRadius) > (length - C.EPS)) throw new ArgumentException("Option height must be larger than twice option roundRadius.");

        Vec3 defaultnormal;
        if (Math.Abs(direction.x) > Math.Abs(direction.y))
        {
            defaultnormal = new Vec3(0, 1, 0);
        }
        else
        {
            defaultnormal = new Vec3(1, 0, 0);
        }

        var zvector = direction.Normalize().Scale(roundRadius);
        var xvector = zvector.Cross(defaultnormal).Normalize().Scale(radius);
        var yvector = xvector.Cross(zvector).Normalize().Scale(radius);

        start = start.Add(zvector);
        end = end.Subtract(zvector);

        var qsegments = Floorish(0.25 * segments);

        List<Vec3> fromPoints(List<Vec3> points)
        {
            // adjust the points to center
            var newpoints = new List<Vec3>(points.Count);
            foreach (var p in points)
            {
                newpoints.Add(p.Add(_center));
            }
            return newpoints;
        }

        var polygons = new List<List<Vec3>>();
        Vec3 prevcylinderpoint = new Vec3();
        for (var slice1 = 0; slice1 <= segments; slice1++)
        {
            var angle = (Math.PI * 2.0 * slice1) / segments;
            var cylinderpoint = xvector.Scale(cos(angle)).Add(yvector.Scale(sin(angle)));
            if (slice1 > 0)
            {
                // cylinder wall
                var points = new List<Vec3>();
                points.Add(start.Add(cylinderpoint));
                points.Add(start.Add(prevcylinderpoint));
                points.Add(end.Add(prevcylinderpoint));
                points.Add(end.Add(cylinderpoint));
                polygons.Add(fromPoints(points));

                double prevcospitch = 0.0, prevsinpitch = 0.0;
                for (var slice2 = 0; slice2 <= qsegments; slice2++)
                {
                    var pitch = 0.5 * Math.PI * slice2 / (double)qsegments;
                    var cospitch = cos(pitch);
                    var sinpitch = sin(pitch);
                    if (slice2 > 0)
                    {
                        // cylinder rounding, start
                        points = new List<Vec3>();
                        Vec3 point;
                        point = start.Add(prevcylinderpoint.Scale(prevcospitch).Subtract(zvector.Scale(prevsinpitch)));
                        points.Add(point);
                        point = start.Add(cylinderpoint.Scale(prevcospitch).Subtract(zvector.Scale(prevsinpitch)));
                        points.Add(point);
                        if (slice2 < qsegments)
                        {
                            point = start.Add(cylinderpoint.Scale(cospitch).Subtract(zvector.Scale(sinpitch)));
                            points.Add(point);
                        }
                        point = start.Add(prevcylinderpoint.Scale(cospitch).Subtract(zvector.Scale(sinpitch)));
                        points.Add(point);

                        polygons.Add(fromPoints(points));

                        // cylinder rounding, end
                        points = new List<Vec3>();
                        point = prevcylinderpoint.Scale(prevcospitch).Add(zvector.Scale(prevsinpitch));
                        point = point.Add(end);
                        points.Add(point);
                        point = cylinderpoint.Scale(prevcospitch).Add(zvector.Scale(prevsinpitch));
                        point = point.Add(end);
                        points.Add(point);
                        if (slice2 < qsegments)
                        {
                            point = cylinderpoint.Scale(cospitch).Add(zvector.Scale(sinpitch));
                            point = point.Add(end);
                            points.Add(point);
                        }
                        point = prevcylinderpoint.Scale(cospitch).Add(zvector.Scale(sinpitch));
                        point = point.Add(end);
                        points.Add(point);
                        points.Reverse();

                        polygons.Add(fromPoints(points));
                    }
                    prevcospitch = cospitch;
                    prevsinpitch = sinpitch;
                }
            }
            prevcylinderpoint = cylinderpoint;
        }
        var result = new Geom3(polygons);
        return result;
    }
}
