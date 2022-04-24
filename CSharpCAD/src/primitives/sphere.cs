namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * Construct a sphere in three dimensional space where all points are at the same distance from the center.
     * @see [ellipsoid]{@link module:modeling/primitives.ellipsoid} for more options
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0,0]] - center of sphere
     * @param {Number} [options.radius=1] - radius of sphere
     * @param {Number} [options.segments=32] - number of segments to create per full rotation
     * @param {Array} [options.axes] -  an array with three vectors for the x, y and z base vectors
     * @returns {geom3} new 3D geometry
     * @alias module:modeling/primitives.sphere
     *
     * @example
     * var myshape = sphere({radius: 5})
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

        return Ellipsoid(center: new Vec3(_center.x,_center.y,_center.z),
            radius: new Vec3(radius,radius,radius), segments: segments,
            axes_x: new Vec3(_axes_x.x,_axes_x.y,_axes_x.z),
            axes_y: new Vec3(_axes_y.x,_axes_y.y,_axes_y.z),
            axes_z: new Vec3(_axes_z.x,_axes_z.y,_axes_z.z));
    }
}
