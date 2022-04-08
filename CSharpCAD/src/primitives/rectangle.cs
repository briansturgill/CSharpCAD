namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct an axis-aligned rectangle in two dimensional space with four sides at right angles.</summary>
     * <remarks>
     * @param {Array} [options.center=[0,0]] - center of rectangle
     * @param {Array} [options.size=[2,2]] - dimension of rectangle, width and length
     * @returns {geom2} new 2D geometry
     * </remarks>
     */
    public static Geom2 Rectangle(Opts opts)
    {
        var size = opts.GetVec2("size", (2.0, 2.0));
        var center = opts.GetVec2("center", (0.0, 0.0));

        if (size.x < 0 || size.y < 0) throw new ArgumentException("Size values must be greater than zero.");

        var point = new Vec2(size.x / 2, size.y / 2);
        var pswap = new Vec2(point.x, -point.y);

        var points = new List<Vec2> {
          center.Subtract(point),
          center.Add(pswap),
          center.Add(point),
          center.Subtract(pswap)
        };
        return new Geom2(points);
    }
}
