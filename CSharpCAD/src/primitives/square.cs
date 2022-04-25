namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * Construct an axis-aligned square in two dimensional space with four equal sides at right angles.
     * <remarks>
     * The default center point is selected such that the bottom left
     * corner of the square is (0,0). (The square is entirely in the first quadrant.)
     * </remarks>
     * <param  name="size">Dimension of square.</param>
     * <param name="center" default="(0,0)">Center of square.</param>
     * <example>
     * var myshape = Square(size: 10);
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Square(double? size = null, Vec2? center = null)
    {
        var _size = size ?? 2;
        var _center = center ?? new Vec2(_size/2, _size/2);

        if (_size <= 0) throw new ArgumentException("Option size must be greater than zero.");

        return Rectangle(center: _center, size: new Vec2(_size, _size));
    }
}
