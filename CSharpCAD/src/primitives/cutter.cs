namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct a "cutter", a negative geometry object, suitable to cut a "pie slice" out of a circlular 2d object of radius r.</summary>
    /// <remarks>
    /// This object would normally be used as a second argument to "Subtract".
    /// To use with an elliptical object, specify radius as the largest radius of the elliptical.
    /// </remarks>
    /// <param name="radius">Radius that matches the circle you want to cut.</param>
    /// <param name="angle" default="90">Angle formed by the cut (in degrees).</param>
    /// <param name="center" default="(0,0)">Center that coincides with the center of the circular target.</param>
    /// <example>
    /// // Cuts the fourth quadrant of the circle away.
    /// var g = Subtract(Circle(radius: 5), Rotate((0, 0, 90), Cutter2D(radius: 6, angle: 90)));
    /// </example>
    /// <group>2D Primitives</group>
    public static Geom2 Cutter2D(double radius = 1, double angle = 90, Vec2? center = null)
    {
        if (radius <= 0.0) throw new ArgumentException("Radius value must be postive.");
        if (LessThanish(angle, 1)) throw new ArgumentException("Angle must be greater than one.");
        if (GreaterThanish(angle, 360)) throw new ArgumentException("Angle must be less than a full circle.");
        var _center = center ?? new Vec2(0, 0);

        radius = radius * 2 + 1; // Using +1 to make sure we are bigger than the diameter.

        var points = new List<Vec2>(6); // a start point, up to 3 90 degree points, and possibly an end point and a center point
        void addPoint(double x, double y) => points.Add(_center.Add(new Vec2(x, y)));

        if (!Equalish(angle, 180.0)) points.Add(_center); // Can't use addPoint, also, if 180ish then colinear with start and end.

        points.Add(new Vec2(radius * Cos(0), radius * Sin(0)));
        for (var pointAngle = 90.0 ; LessThanish(pointAngle, angle); pointAngle += 90.0)
        {
            addPoint(radius * Cos(pointAngle), radius * Sin(pointAngle));
        }
        addPoint(radius * Cos(angle), radius * Sin(angle));

        var cutter2D = new Geom2(points);
        return cutter2D;
    }

    /// <summary>Construct a "cutter", a negative geometry object, suitable to cut a "pie slice" out of a circlular 3D object of radius r.</summary>
    /// <remarks>
    /// This object would normally be used as a second argument to "Subtract".
    /// To use with an elliptical object, specify radius as the largest radius of the elliptical.
    /// </remarks>
    /// <param name="radius">Radius that matches the circular 3D object you want to cut.</param>
    /// <param name="height" default="radius*2+2">Height that matches the cylinder or other circular 3D object you want to cut.</param>
    /// <param name="angle" default="90">Angle formed by the cut (in degrees).</param>
    /// <param name="center" default="(0,0,0)">Center that coincides with the center of the circular target.</param>
    /// <example>
    /// var g = Subtract(Cylinder(height: 10, radius: 5), Cutter3D(radius: 5, height: 10, angle: 90));
    /// </example>
    /// <group>3D Primitives</group>
    public static Geom3 Cutter3D(double radius = 1, double? height = null, double angle = 90, Vec3? center = null)
    {
        if (radius <= 0.0) throw new ArgumentException("Radius value must be postive.");
        if (LessThanish(angle, 1)) throw new ArgumentException("Angle must be greater than one.");
        if (GreaterThanish(angle, 360)) throw new ArgumentException("Angle must be less than a full circle.");
        var _center = center ?? new Vec3(0, 0, 0);
        var _height = height ?? radius * 2;

        _height = _height + 2; // Make sure we cut everything.

        var cutter2D = Cutter2D(radius, angle);
        var cutter3D = (Geom3)Translate(new Vec3(_center.X, _center.Y, _center.Z - (_height / 2.0)), ExtrudeLinear(cutter2D, height: _height));
        return cutter3D;
    }
}
