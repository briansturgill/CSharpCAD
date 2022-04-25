namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a sphere in 3D space where all points are at the same distance from the center.</summary>
     * <param> name="radius">Radius of sphere.</param>
     * <param> name="segments">Number of segments to create per full rotation.</param>
     * <param> name="axes_x" default=(1,0,0)">The X base vector.</param>
     * <param> name="axes_y" default=(0,-1,0)">The Y base vector.</param>
     * <param> name="axes_z" default=(0,0,1)">The Z base vector.</param>
     * <param> name="center" default="(0,0,0)">Center of sphere.</param>
     * <example>
     * var myshape = Sphere(radius: 5);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 Sphere(double radius = 1, Vec3? center = null, int segments = 32,
        Vec3? axes_x = null, Vec3? axes_y = null, Vec3? axes_z = null)
    {
        var _center = center ?? new Vec3(0, 0, 0);
        var _axes_x = axes_x ?? new Vec3(1, 0, 0);
        var _axes_y = axes_y ?? new Vec3(0, -1, 0);
        var _axes_z = axes_z ?? new Vec3(0, 0, 1);

        if (radius <= 0) throw new ArgumentException("Option radius must be greater than zero.");

        return Ellipsoid(center: new Vec3(_center.X,_center.Y,_center.Z),
            radius: new Vec3(radius,radius,radius), segments: segments,
            axes_x: new Vec3(_axes_x.X,_axes_x.Y,_axes_x.Z),
            axes_y: new Vec3(_axes_y.X,_axes_y.Y,_axes_y.Z),
            axes_z: new Vec3(_axes_z.X,_axes_z.Y,_axes_z.Z));
    }
}
