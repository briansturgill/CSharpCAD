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
     * <param name="center" default="(size/2, size/2)">Center of square.</param>
     * <example>
     * var myshape = Square(size: 10);
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Square(double size = 2, Vec2? center = null)
    {
        var _center = center ?? new Vec2(size/2, size/2);

        if (size <= 0) throw new ArgumentException("Option size must be greater than zero.");

        return Rectangle(center: _center, size: new Vec2(size, size));
    }
}
