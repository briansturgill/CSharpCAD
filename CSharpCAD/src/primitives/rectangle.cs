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
     * <group>2D Primitives</group>
     */
    public static Geom2 Rectangle(Vec2? size = null, Vec2? center = null)
    {
      var _size = size ?? new Vec2(2.0, 2.0);
      var _center = center ?? new Vec2(_size.x/2, _size.y/2);

        if (_size.x < 0 || _size.y < 0) throw new ArgumentException("Size values must be greater than zero.");

        var point = new Vec2(_size.x / 2, _size.y / 2);
        var pswap = new Vec2(point.x, -point.y);

        var points = new List<Vec2> {
          _center.Subtract(point),
          _center.Add(pswap),
          _center.Add(point),
          _center.Subtract(pswap)
        };
        return new Geom2(points);
    }
}
