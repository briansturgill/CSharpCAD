namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * Construct an axis-aligned rectangle in two dimensional space with rounded corners.
     * @param {Object} [options] - options for construction
     * @param {Array} [options.center=[0,0]] - center of rounded rectangle
     * @param {Array} [options.size=[2,2]] - dimension of rounded rectangle; width and length
     * @param {Number} [options.roundRadius=0.2] - round radius of corners
     * @param {Number} [options.segments=32] - number of segments to create per full rotation
     * @returns {geom2} new 2D geometry
     * @alias module:modeling/primitives.roundedRectangle
     *
     * @example
     * var myshape = roundedRectangle({size: [10, 20], roundRadius: 2})
     * <group>3D Primitives</group>
     */
    public static Geom2 RoundedRectangle(Vec2? size = null, double roundRadius = 0.2,
        int segments = 32, Vec2? center = null)
    {
        var _size = size ?? new Vec2(2, 2);
        var _center = center ?? new Vec2(_size.x / 2, _size.y / 2);

        if (_size.x <= 0 || _size.y <= 0) throw new ArgumentException("Option size values must be greater than zero.");
        if (roundRadius <= 0) throw new ArgumentException("Option roundRadius must be greater than zero.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        _size = new Vec2(_size.x / 2, _size.y / 2); // convert to radius

        if (roundRadius > (_size.x - C.EPS) ||
            roundRadius > (_size.y - C.EPS)) throw new ArgumentException("Option roundRadius must be smaller then the radius of all dimensions.");

        var cornersegments = segments / 4;

        // create sets of points that define the corners
        var corner0 = _center.Add(new Vec2(_size.x - roundRadius, _size.y - roundRadius));
        var corner1 = _center.Add(new Vec2(roundRadius - _size.x, _size.y - roundRadius));
        var corner2 = _center.Add(new Vec2(roundRadius - _size.x, roundRadius - _size.y));
        var corner3 = _center.Add(new Vec2(_size.x - roundRadius, roundRadius - _size.y));
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
