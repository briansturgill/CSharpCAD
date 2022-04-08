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
     */
    public static Geom2 RoundedRectangle(Opts opts)
    {
        var center = opts.GetVec2("center", (0, 0));
        var size = opts.GetVec2("size", (2, 2));
        var roundRadius = opts.GetDouble("roundRadius", 0.2);
        var segments = opts.GetInt("segments", 32);

        if (size.x <= 0 || size.y <= 0) throw new ArgumentException("Option size values must be greater than zero.");
        if (roundRadius <= 0) throw new ArgumentException("Option roundRadius must be greater than zero.");
        if (segments < 4) throw new ArgumentException("Option segments must be four or more.");

        size = new Vec2(size.x / 2, size.y / 2); // convert to radius

        if (roundRadius > (size.x - C.EPS) ||
            roundRadius > (size.y - C.EPS)) throw new ArgumentException("Option roundRadius must be smaller then the radius of all dimensions.");

        var cornersegments = segments / 4;

        // create sets of points that define the corners
        var corner0 = center.Add(new Vec2(size.x - roundRadius, size.y - roundRadius));
        var corner1 = center.Add(new Vec2(roundRadius - size.x, size.y - roundRadius));
        var corner2 = center.Add(new Vec2(roundRadius - size.x, roundRadius - size.y));
        var corner3 = center.Add(new Vec2(size.x - roundRadius, roundRadius - size.y));
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
