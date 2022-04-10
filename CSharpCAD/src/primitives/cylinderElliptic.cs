namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a Z axis-aligned elliptic cylinder in three dimensional space.</summary>
     * <remarks>
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0,0]] - center of cylinder
     * @param {Number} [options.height=2] - height of cylinder
     * @param {Array} [options.startRadius=[1,1]] - radius of rounded start, must be two dimensional array
     * @param {Number} [options.startAngle=0] - start angle of cylinder, in radians
     * @param {Array} [options.endRadius=[1,1]] - radius of rounded end, must be two dimensional array
     * @param {Number} [options.endAngle=(Math.PI * 2)] - end angle of cylinder, in radians
     * @param {Number} [options.segments=32] - number of segments to create per full rotation
     * </remarks>
     */
    public static Geom3 CylinderElliptic(double height = 2.0, Vec2? startRadius = null,
        Vec2? endRadius = null, double startAngle = 0.0, double endAngle = Math.PI * 2,
        int segments = 32, Vec3? center = null)
    {
        var _center = center ?? new Vec3(0, 0, 0);
        var _startRadius = startRadius ?? new Vec2(1.0, 1.0);
        var _endRadius = endRadius ?? new Vec2(1.0, 1.0);

        if (height <= 0.0) throw new ArgumentException("Option height must be greater then zero.");
        if (_startRadius.x < 0 || _startRadius.y <= 0) throw new ArgumentException("Option startRadius values must be positive.");
        if (_endRadius.x <= 0 || _endRadius.y <= 0) throw new ArgumentException("Option endRadius values must be positive.");
        if (_endRadius.x == 0 && _endRadius.y == 0) throw new ArgumentException("At least one of option endRadius values must be positive.");
        if (startAngle < 0) throw new ArgumentException("Option startAngle must be positive.");
        if (endAngle < 0) throw new ArgumentException("Option endAngle must be positive.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        startAngle = startAngle % (Math.PI * 2);
        endAngle = endAngle % (Math.PI * 2);

        var rotation = (Math.PI * 2);
        if (startAngle < endAngle)
        {
            rotation = endAngle - startAngle;
        }
        if (startAngle > endAngle)
        {
            rotation = endAngle + ((Math.PI * 2) - startAngle);
        }

        var minradius = Math.Min(Math.Min(_startRadius.x, _startRadius.y), Math.Min(_endRadius.x, _endRadius.y));
        var minangle = Math.Acos(((minradius * minradius) + (minradius * minradius) - (C.EPS * C.EPS)) /
                                  (2 * minradius * minradius));
        if (rotation < minangle) throw new ArgumentException("Options startAngle and endAngle do not define a significant rotation.");

        var slices = Floorish(segments * (rotation / (Math.PI * 2)));

        var start = new Vec3(0, 0, -(height / 2));
        var end = new Vec3(0, 0, height / 2);
        var ray = end.Subtract(start);

        var axisX = new Vec3(1, 0, 0);
        var axisY = new Vec3(0, 1, 0);

        var v1 = new Vec3();
        var v2 = new Vec3();
        var v3 = new Vec3();

        Vec3 point(double stack, double slice, Vec2 radius)
        {
            var angle = slice * rotation + startAngle;
            v1 = axisX.Scale(radius.x * cos(angle));
            v2 = axisY.Scale(radius.y * sin(angle));
            v1 = v1.Add(v2);

            v3 = ray.Scale(stack);
            v3 = v3.Add(start);
            return v1.Add(v3);
        }

        // adjust the points to center
        List<Vec3> fromPoints(params Vec3[] points)
        {
            var newpoints = new List<Vec3>(points.Length);
            foreach (var point in points)
            {
                newpoints.Add(point.Add(_center));
            }
            return newpoints;
        }

        var polygons = new List<List<Vec3>>();
        for (var i = 0; i < slices; i++)
        {
            var t0 = i / (double)slices;
            var t1 = (i + 1) / (double)slices;

            if (_endRadius.x == _startRadius.x && _endRadius.y == _startRadius.y)
            {
                polygons.Add(fromPoints(start, point(0, t1, _endRadius), point(0, t0, _endRadius)));
                polygons.Add(fromPoints(point(0, t1, _endRadius), point(1, t1, _endRadius), point(1, t0, _endRadius), point(0, t0, _endRadius)));
                polygons.Add(fromPoints(end, point(1, t0, _endRadius), point(1, t1, _endRadius)));
            }
            else
            {
                if (_startRadius.x > 0 && _startRadius.y > 0)
                {
                    polygons.Add(fromPoints(start, point(0, t1, _startRadius), point(0, t0, _startRadius)));
                }
                if (_startRadius.x > 0 || _startRadius.y > 0)
                {
                    polygons.Add(fromPoints(point(0, t0, _startRadius), point(0, t1, _startRadius), point(1, t0, _endRadius)));
                }
                if (_endRadius.x > 0 && _endRadius.y > 0)
                {
                    polygons.Add(fromPoints(end, point(1, t0, _endRadius), point(1, t1, _endRadius)));
                }
                if (_endRadius.x > 0 || _endRadius.y > 0)
                {
                    polygons.Add(fromPoints(point(1, t0, _endRadius), point(0, t1, _startRadius), point(1, t1, _endRadius)));
                }
            }
        }
        if (rotation < (Math.PI * 2))
        {
            polygons.Add(fromPoints(start, point(0, 0, _startRadius), end));
            polygons.Add(fromPoints(point(0, 0, _startRadius), point(1, 0, _endRadius), end));
            polygons.Add(fromPoints(start, end, point(0, 1, _startRadius)));
            polygons.Add(fromPoints(point(0, 1, _startRadius), end, point(1, 1, _endRadius)));
        }
        var result = new Geom3(polygons);
        return result;
    }
}
