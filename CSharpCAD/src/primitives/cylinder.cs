namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a Z axis-aligned cylinder in three dimensional space.</summary>
     * @see [cylinderElliptic]{@link module:modeling/primitives.cylinderElliptic} for more options
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0,0]] - center of cylinder
     * @param {Number} [options.height=2] - height of cylinder
     * @param {Number} [options.radius=1] - radius of cylinder (at both start and end)
     * @param {Number} [options.segments=32] - number of segments to create per full rotation
     * @returns {geom3} new geometry
     * @alias module:modeling/primitives.cylinder
     *
     * @example
     * var myshape = cylinder({height: 2, radius: 10})
     */
    public static Geom3 Cylinder(Opts opts)
    {
        var center = opts.GetVec3("center", (0, 0, 0));
        var height = opts.GetDouble("height", 2);
        var radius = opts.GetDouble("radius", 1);
        var segments = opts.GetInt("segments", 32);

        if (radius <= 0) throw new ArgumentException("Option radius must be greater than zero.");

        var newoptions = new Opts {
          {"center", (center.x, center.y, center.z)},
          {"height", height},
          {"startRadius", (radius, radius)},
          {"endRadius", (radius, radius)},
          {"segments", segments}
        };

        return CylinderElliptic(newoptions);
    }
}
