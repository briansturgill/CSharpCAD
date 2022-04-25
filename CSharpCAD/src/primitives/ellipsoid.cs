namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct an axis-aligned ellipsoid in three dimensional space.</summary>
     * <param> name="radius" default="(1,1,1)">Radius of ellipsoid, along X, Y and Z.</param>
     * <param> name="segments">Number of segments to create per full rotation.</param>
     * <param> name="axes_x" default=(1,0,0)">The X base vector.</param>
     * <param> name="axes_y" default=(0,-1,0)">The Y base vector.</param>
     * <param> name="axes_z" default=(0,0,1)">The Z base vector.</param>
     * <param> name="center" default="(0,0,0)">Center of ellipsoid.</param>
     * <example>
     * var g = Ellipsoid(radius: (10, 5, 20), segments: 50);
     * </example>
     * <group>3D Primitives</group>
*/
    public static Geom3 Ellipsoid(Vec3? radius = null, int segments = 32, Vec3? center = null,
        Vec3? axes_x = null, Vec3? axes_y = null, Vec3? axes_z = null)
    {
        var _center = center ?? new Vec3(0, 0, 0);
        var _radius = radius ?? new Vec3(1.0, 1.0, 1.0);
        var _axes_x = axes_x ?? new Vec3(1.0, 0.0, 0.0);
        var _axes_y = axes_y ?? new Vec3(0.0, -1.0, 0.0);
        var _axes_z = axes_z ?? new Vec3(0.0, 0.0, 1.0);

        if (_radius.X <= 0 || _radius.Y <= 0 || _radius.Z <= 0) throw new ArgumentException("Option radius values must be greater than zero.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        var xvector = _axes_x.Normalize().Scale(_radius.X);
        var yvector = _axes_y.Normalize().Scale(_radius.Y);
        var zvector = _axes_z.Normalize().Scale(_radius.Z);

        var qsegments = segments / 4;
        var prevcylinderpoint = new Vec3();
        var polygons = new List<List<Vec3>>();
        for (var slice1 = 0; slice1 <= segments; slice1++)
        {
            var angle = Math.PI * 2.0 * slice1 / segments;
            var cylinderpoint = xvector.Scale(Math.Cos(angle)).Add(yvector.Scale(Math.Sin(angle)));
            if (slice1 > 0)
            {
                double prevcospitch = 0.0;
                double prevsinpitch = 0.0;
                for (var slice2 = 0; slice2 <= qsegments; slice2++)
                {
                    var pitch = 0.5 * Math.PI * slice2 / (double)qsegments;
                    var cospitch = Math.Cos(pitch);
                    var sinpitch = Math.Sin(pitch);
                    if (slice2 > 0)
                    {
                        var points = new List<Vec3>();
                        var point = new Vec3();
                        point = prevcylinderpoint.Scale(prevcospitch).Subtract(zvector.Scale(prevsinpitch));
                        point = point.Add(_center);
                        points.Add(point);
                        point = cylinderpoint.Scale(prevcospitch).Subtract(zvector.Scale(prevsinpitch));
                        point = point.Add(_center);
                        points.Add(point);
                        if (slice2 < qsegments)
                        {
                            point = cylinderpoint.Scale(cospitch).Subtract(zvector.Scale(sinpitch));
                            point = point.Add(_center);
                            points.Add(point);
                        }
                        point = prevcylinderpoint.Scale(cospitch).Subtract(zvector.Scale(sinpitch));
                        point = point.Add(_center);
                        points.Add(point);

                        polygons.Add(points);

                        points = new List<Vec3>();
                        point = prevcylinderpoint.Scale(prevcospitch).Add(zvector.Scale(prevsinpitch));
                        point = _center.Add(point);
                        points.Add(point);
                        point = cylinderpoint.Scale(prevcospitch).Add(zvector.Scale(prevsinpitch));
                        point = _center.Add(point);
                        points.Add(point);
                        if (slice2 < qsegments)
                        {
                            point = cylinderpoint.Scale(cospitch).Add(zvector.Scale(sinpitch));
                            point = _center.Add(point);
                            points.Add(point);
                        }
                        point = prevcylinderpoint.Scale(cospitch).Add(zvector.Scale(sinpitch));
                        point = _center.Add(point);
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
