namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * Construct an axis-aligned square in two dimensional space with four equal sides at right angles.
     * @see [rectangle]{@link module:modeling/primitives.rectangle} for more options
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0]] - center of square
     * @param {Number} [options.size=2] - dimension of square
     * @returns {geom2} new 2D geometry
     * @alias module:modeling/primitives.square
     *
     * @example
     * var myshape = square({size: 10})
     */
    public static Geom2 Square(double? size = null, Vec2? center = null)
    {
        var _size = size ?? 2;
        var _center = center ?? new Vec2(_size/2, _size/2);

        if (_size <= 0) throw new ArgumentException("Option size must be greater than zero.");

        return Rectangle(center: _center, size: new Vec2(_size, _size));
    }
}
