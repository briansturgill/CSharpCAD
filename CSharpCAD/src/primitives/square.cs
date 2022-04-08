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
    public static Geom2 Square(Opts opts)
    {
        var center = opts.GetVec2("center", (0, 0));
        var size = opts.GetDouble("size", 2);

        if (size <= 0) throw new ArgumentException("Optionsize must be greater than zero.");

        return Rectangle(new Opts { { "center", (center.x, center.y) }, { "size", (size, size) } });
    }
}
