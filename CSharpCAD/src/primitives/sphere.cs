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
     */
    public static Geom3 Sphere(Opts opts)
    {
        var center = opts.GetVec3("center", (0, 0, 0));
        var radius = opts.GetDouble("radius", 1);
        var segments = opts.GetInt("segments", 32);
        var axes_x = opts.GetVec3("axes_x", (1, 0, 0));
        var axes_y = opts.GetVec3("axes_y", (0, -1, 0));
        var axes_z = opts.GetVec3("axes_z", (0, 0, 1));

        if (radius <= 0) throw new ArgumentException("Option radius must be greater than zero.");

        return Ellipsoid(new Opts{{"center", (center.x,center.y,center.z)}, {"radius", (radius,radius,radius)}, {"segments", segments},
          {"axes_x", (axes_x.x,axes_x.y,axes_x.z)}, {"axes_y", (axes_y.x,axes_y.y,axes_y.z)}, {"axes_z", (axes_z.x,axes_z.y,axes_z.z)}});
    }
}
