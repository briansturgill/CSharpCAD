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
    public static Geom3 Cylinder(double radius = 1, double height = 2, int segments = 32, Vec3? center = null)
    {
        var _center = center ?? new Vec3(0, 0, 0);

        if (radius <= 0) throw new ArgumentException("Option radius must be greater than zero.");

        return CylinderElliptic(height: height, startRadius: new Vec2(radius, radius),
          endRadius: new Vec2(radius, radius), center: _center, segments: segments);
    }
}
