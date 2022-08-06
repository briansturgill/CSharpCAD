namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Rotate extrude the given geometry using the given options.</summary>
     * <param name="gobj">The 2D geometry to extrude.</param>
     * <param name="angle">Angle of the extrusion (DEGREES).</param>
     * <param name="startAngle">Start angle of the extrusion (DEGREES).</param>
     * <param name="segments">Number of segments of the extrusion.</param>
     * <returns>The extruded 3D geometry</returns>
     * <group>3D Primitives</group>
     */
    public static Geom3 ExtrudeRotate(Geom2 gobj, int segments = 12, double startAngle = 0, double angle = 360)
    {
        // @param {String} [options.overflow="cap"] - what to do with points outside of bounds (+ / - x) :
        // defaults to capping those points to 0 (only supported behaviour for now)
        // CBS C# translation notd: We are not bothering with this as an option at all.
        // var overflow = opts.GetString("overflow", "cap");

        if (segments < 3) throw new ArgumentException("Segments must be greater than 3.");
        var geometry = gobj;

        startAngle = DegToRad(startAngle);
        angle = DegToRad(angle);
        var overflow = "cap";

        startAngle = Math.Abs(startAngle) > (Math.PI * 2) ? startAngle % (Math.PI * 2) : startAngle;
        angle = Math.Abs(angle) > (Math.PI * 2) ? angle % (Math.PI * 2) : angle;

        var endAngle = startAngle + angle;
        endAngle = Math.Abs(endAngle) > (Math.PI * 2) ? endAngle % (Math.PI * 2) : endAngle;

        if (endAngle < startAngle)
        {
            var x = startAngle;
            startAngle = endAngle;
            endAngle = x;
        }
        var totalRotation = endAngle - startAngle;
        if (LessThanOrEqualish(totalRotation, 0.0)) totalRotation = (Math.PI * 2);

        if (Math.Abs(totalRotation) < (Math.PI * 2))
        {
            // adjust the segments to achieve the total rotation requested
            var anglePerSegment = (Math.PI * 2) / (double)segments;
            segments = Floorish(Math.Abs(totalRotation) / anglePerSegment);
            if (Math.Abs(totalRotation) > (segments * anglePerSegment)) segments++;
        }

        // convert geometry to an array of sides, easier to deal with
        var shapeSides = geometry.ToSides();
        if (shapeSides.Length == 0) throw new ArgumentException("The given geometry cannot be empty.");

        // determine if the rotate extrude can be computed in the first place
        // ie all the points have to be either x > 0 or x < 0

        // generic solution to always have a valid solid, even if points go beyond x/ -x
        // 1. split points up between all those on the "left" side of the axis (x<0) & those on the "righ" (x>0)
        // 2. for each set of points do the extrusion operation IN OPOSITE DIRECTIONS
        // 3. union the two resulting solids

        // 1. alt : OR : just cap of points at the axis ?

        var pointsWithNegativeX = new List<Geom2.Side>();
        var pointsWithPositiveX = new List<Geom2.Side>();
        for (var i = 0; i < shapeSides.Length; i++)
        {
            var s = shapeSides[i];
            if (s.v0.X < 0)
            {
                pointsWithNegativeX.Add(s);
            }
            else
            {
                pointsWithPositiveX.Add(s);
            }
        }
        var arePointsWithNegAndPosX = pointsWithNegativeX.Count > 0 && pointsWithPositiveX.Count > 0;

        // FIXME actually there are cases where setting X=0 will change the basic shape
        // - Alternative #1 : don"t allow shapes with both negative and positive X values
        // - Alternative #2 : remove one half of the shape (costly)
        if (arePointsWithNegAndPosX && overflow == "cap")
        {
            if (pointsWithNegativeX.Count > pointsWithPositiveX.Count)
            {
                for (var i = 0; i < shapeSides.Length; i++)
                {
                    var side = shapeSides[i];
                    var point0 = side.v0;
                    var point1 = side.v1;
                    point0 = new Vec2(Math.Min(point0.X, 0), point0.Y);
                    point1 = new Vec2(Math.Min(point1.X, 0), point1.Y);
                    shapeSides[i] = new Geom2.Side(point0, point1);
                }
                // recreate the geometry from the (-) capped points
                geometry = new Geom2(shapeSides).Reverse();
                geometry = MirrorX(geometry);
            }
            else if (pointsWithPositiveX.Count >= pointsWithNegativeX.Count)
            {
                for (var i = 0; i < shapeSides.Length; i++)
                {
                    var side = shapeSides[i];
                    var point0 = side.v0;
                    var point1 = side.v1;
                    point0 = new Vec2(Math.Max(point0.X, 0), point0.Y);
                    point1 = new Vec2(Math.Max(point1.X, 0), point1.Y);
                    shapeSides[i] = new Geom2.Side(point0, point1);
                }
                // recreate the geometry from the (+) capped points
                geometry = new Geom2(shapeSides);
            }
        }

        var rotationPerSlice = totalRotation / (double)segments;
        var isCapped = Math.Abs(totalRotation) < (Math.PI * 2);
        var baseSlice = new Slice(geometry.ToOutlines());
        baseSlice = baseSlice.Reverse();

        Slice createSlice(double progress, int index, object baseSlice)
        {
            var Zrotation = rotationPerSlice * index + startAngle;
            // fix rounding error when rotating 2 * PI radians
            if (totalRotation == Math.PI * 2 && index == segments)
            {
                Zrotation = startAngle;
            }

            var matrix = Mat4.FromZRotation(Zrotation).Multiply(Mat4.FromXRotation(Math.PI / 2));

            return ((Slice)baseSlice).Transform(matrix);
        }

        return ExtrudeFromSlices(baseSlice, createSlice, numberOfSlices: segments + 1,
            capStart: isCapped, capEnd: isCapped, close: !isCapped);
    }

}