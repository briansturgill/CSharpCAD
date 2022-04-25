namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct an axis-aligned rectangle in 2D space with four sides at right angles.</summary>
     * <remarks>
     * The default center point is selected such that the bottom left
     * corner of the rectangle is (0,0). (The rectangle is entirely in the first quadrant.)
     * </remarks>
     * <param name="size" default="(2,2)">Dimension of rectangle, width and length.</param>
     * <param name="center" default="(0,0)">Center of rectangle.</param>
     * <example>
     * var g = Rectangle(size: (3,5);
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Rectangle(Vec2? size = null, Vec2? center = null)
    {
      var _size = size ?? new Vec2(2.0, 2.0);
      var _center = center ?? new Vec2(_size.X/2, _size.Y/2);

        if (_size.X < 0 || _size.Y < 0) throw new ArgumentException("Size values must be greater than zero.");

        var point = new Vec2(_size.X / 2, _size.Y / 2);
        var pswap = new Vec2(point.X, -point.Y);

        var points = new List<Vec2> {
          _center.Subtract(point),
          _center.Add(pswap),
          _center.Add(point),
          _center.Subtract(pswap)
        };
        return new Geom2(points);
    }
}
