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
     * <param name="center" default="(size.X/2, size.Y/2)">Center of rectangle.</param>
     * <example>
     * var g = Rectangle(size: (3,5));
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Rectangle(Vec2? size = null, Vec2? center = null)
    {
        return new Geom2(InternalRectangle(size, center));
    }

    internal static Vec2[] InternalRectangle(Vec2? size = null, Vec2? center = null)
    {
        var _size = size ?? new Vec2(2.0, 2.0);

        if (_size.X < 0 || _size.Y < 0) throw new ArgumentException("Size values must be greater than zero.");

        var points = new Vec2[] {
          new Vec2(0, 0),
          new Vec2(_size.X, 0),
          new Vec2(_size.X,_size.Y),
          new Vec2(0,_size.Y)
        };

        if (center is not null)
        {
            var _center = new Vec2(((Vec2)center).X - _size.X / 2, ((Vec2)center).Y - _size.Y / 2);
            for (var i = 0; i < points.Length; i++)
            {
                points[i] = _center.Add(points[i]);
            }
        }
        return points;
    }
}