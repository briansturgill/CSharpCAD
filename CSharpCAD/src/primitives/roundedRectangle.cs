namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct an axis-aligned rectangle in 2D space with rounded corners.</summary>
     * <remarks>
     * The default center point is selected such that the bottom left
     * corner of the rectangle is (0,0). (The rectangle is entirely in the first quadrant.)
     * </remarks>
     * <param name="size" default="(2,2)">Dimension of rounded rectangle: width and length.</param>
     * <param name="roundRadius">Round radius of corners.</param>
     * <param name="segments">Number of segments to create per full rotation.</param>
     * <param name="center" default="(size.X/2,size.Y/2)">Center of rounded rectangle.</param>
     * <example>
     * var myshape = RoundedRectangle(size: (10, 20), roundRadius: 2);
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 RoundedRectangle(Vec2? size = null, double roundRadius = 0.2,
        int segments = 32, Vec2? center = null)
    {
        var _size = size ?? new Vec2(2, 2);
        var _center = center ?? new Vec2(_size.X / 2, _size.Y / 2);

        if (_size.X <= 0 || _size.Y <= 0) throw new ArgumentException("Option size values must be greater than zero.");
        if (roundRadius <= 0) throw new ArgumentException("Option roundRadius must be greater than zero.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        _size = new Vec2(_size.X / 2, _size.Y / 2); // convert to radius

        if (roundRadius > (_size.X - C.EPS) ||
            roundRadius > (_size.Y - C.EPS)) throw new ArgumentException("Option roundRadius must be smaller then the radius of all dimensions.");

        var cornersegments = segments / 4;

        // create sets of points that define the corners
        var corner0 = _center.Add(new Vec2(_size.X - roundRadius, _size.Y - roundRadius));
        var corner1 = _center.Add(new Vec2(roundRadius - _size.X, _size.Y - roundRadius));
        var corner2 = _center.Add(new Vec2(roundRadius - _size.X, roundRadius - _size.Y));
        var corner3 = _center.Add(new Vec2(_size.X - roundRadius, roundRadius - _size.Y));
        var corner0Points = new List<Vec2>();
        var corner1Points = new List<Vec2>();
        var corner2Points = new List<Vec2>();
        var corner3Points = new List<Vec2>();
        for (var i = 0; i <= cornersegments; i++)
        {
            var radians = Math.PI / 2 * i / cornersegments;
            var point = Vec2.FromAngleRadians(radians);
            point = point.Scale(roundRadius);
            corner0Points.Add(corner0.Add(point));
            point = point.Rotate(new Vec2(), Math.PI / 2);
            corner1Points.Add(corner1.Add(point));
            point = point.Rotate(new Vec2(), Math.PI / 2);
            corner2Points.Add(corner2.Add(point));
            point = point.Rotate(new Vec2(), Math.PI / 2);
            corner3Points.Add(corner3.Add(point));
        }

        var points = new List<Vec2>();
        points.AddRange(corner0Points);
        points.AddRange(corner1Points);
        points.AddRange(corner2Points);
        points.AddRange(corner3Points);
        return new Geom2(points);
    }
}
