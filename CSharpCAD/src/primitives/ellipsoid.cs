namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * Construct an axis-aligned ellipsoid in three dimensional space.
     * <remarks>
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0,0]] - center of ellipsoid
     * @param {Array} [options.radius=[1,1,1]] - radius of ellipsoid, along X, Y and Z
     * @param {Number} [options.segments=32] - number of segements to create per full rotation
     * @param {Array} [options.axes] -  an array with three vectors for the x, y and z base vectors
     * @returns {geom3} new 3D geometry
     * </remarks>
*/
    public static Geom3 Ellipsoid(Opts opts)
    {
        var center = opts.GetVec3("center", (0.0, 0.0, 0.0));
        var radius = opts.GetVec3("radius", (1.0, 1.0, 1.0));
        var segments = opts.GetInt("segments", 32);
        var axes_x = opts.GetVec3("axes_x", (1.0, 0.0, 0.0));
        var axes_y = opts.GetVec3("axes_y", (0.0, -1.0, 0.0));
        var axes_z = opts.GetVec3("axes_z", (0.0, 0.0, 1.0));

        if (radius.x <= 0 || radius.y <= 0 || radius.z <= 0) throw new ArgumentException("Option radius values must be greater than zero.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        var xvector = axes_x.Normalize().Scale(radius.x);
        var yvector = axes_y.Normalize().Scale(radius.y);
        var zvector = axes_z.Normalize().Scale(radius.z);

        var qsegments = segments / 4.0;
        var prevcylinderpoint = new Vec3();
        var polygons = new List<List<Vec3>>();
        for (var slice1 = 0; slice1 <= segments; slice1++)
        {
            var angle = Math.PI * 2.0 * slice1 / segments;
            var cylinderpoint = xvector.Scale(cos(angle)).Add(yvector.Scale(sin(angle)));
            if (slice1 > 0)
            {
                double prevcospitch = 0.0;
                double prevsinpitch = 0.0;
                for (var slice2 = 0; slice2 <= qsegments; slice2++)
                {
                    var pitch = 0.5 * Math.PI * slice2 / (double)qsegments;
                    var cospitch = cos(pitch);
                    var sinpitch = sin(pitch);
                    if (slice2 > 0)
                    {
                        var points = new List<Vec3>();
                        var point = new Vec3();
                        point = prevcylinderpoint.Scale(prevcospitch).Subtract(zvector.Scale(prevsinpitch));
                        point = point.Add(center);
                        points.Add(point);
                        point = cylinderpoint.Scale(prevcospitch).Subtract(zvector.Scale(prevsinpitch));
                        point = point.Add(center);
                        points.Add(point);
                        if (slice2 < qsegments)
                        {
                            point = cylinderpoint.Scale(cospitch).Subtract(zvector.Scale(sinpitch));
                            point = point.Add(center);
                            points.Add(point);
                        }
                        point = prevcylinderpoint.Scale(cospitch).Subtract(zvector.Scale(sinpitch));
                        point = point.Add(center);
                        points.Add(point);

                        polygons.Add(points);

                        points = new List<Vec3>();
                        point = prevcylinderpoint.Scale(prevcospitch).Add(zvector.Scale(prevsinpitch));
                        point = center.Add(point);
                        points.Add(point);
                        point = cylinderpoint.Scale(prevcospitch).Add(zvector.Scale(prevsinpitch));
                        point = center.Add(point);
                        points.Add(point);
                        if (slice2 < qsegments)
                        {
                            point = cylinderpoint.Scale(cospitch).Add(zvector.Scale(sinpitch));
                            point = center.Add(point);
                            points.Add(point);
                        }
                        point = prevcylinderpoint.Scale(cospitch).Add(zvector.Scale(sinpitch));
                        point = center.Add(point);
                        points.Add(point);
                        points.Reverse();

                        polygons.Add(points);
                    }
                    prevcospitch = cospitch;
                    prevsinpitch = sinpitch;
                }
            }
            prevcylinderpoint = cylinderpoint;
        }
        return new Geom3(polygons);
    }
}
