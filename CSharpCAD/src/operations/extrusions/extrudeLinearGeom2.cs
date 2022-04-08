namespace CSharpCAD;

public static partial class CSCAD
{

    /*
     * Extrude the given geometry using the given options.
     *
     * @param {Object} [options] - options for extrude
     * @param {Array} [options.offset] - the direction of the extrusion as a 3D vector
     * @param {Number} [options.twistAngle] - the final rotation (RADIANS) about the origin
     * @param {Integer} [options.twistSteps] - the number of steps created to produce the twist (if any)
     * @param {geom2} geometry - the geometry to extrude
     * @returns {geom3} the extruded 3D geometry
    */
    public static Geom3 ExtrudeLinearGeom2(Geom2 obj, Vec3 offset, double twistAngle = 0, int twistSteps = 12, bool repair = true)
    {
        if (twistSteps < 1) throw new ArgumentException("Argument twistSteps must be 1 or more.");

        if (twistAngle == 0)
        {
            twistSteps = 1;
        }

        // convert to vector in order to perform transforms
        var offsetv = offset;

        var baseSides = obj.ToSides();
        if (baseSides.Length == 0) throw new ArgumentException("The given geometry object cannot be empty.");

        var baseSlice = new Slice(baseSides);
        if (offsetv.z < 0) baseSlice = baseSlice.Reverse();

        Slice? createTwist(double progress, int index, Slice baseSlice)
        {
            var Zrotation = index / (double)twistSteps * twistAngle;
            var Zoffset = offsetv.Scale(index / (double)twistSteps);
            var matrix = Mat4.FromZRotation(Zrotation).Multiply(Mat4.FromTranslation(Zoffset));

            return baseSlice.Transform(matrix);
        }

        var options = new Opts{
          {"numberOfSlices", twistSteps + 1},
          {"capStart", true},
          {"capEnd", true},
          {"repair", true}
        };
        return ExtrudeFromSlices(options, baseSlice, createTwist);
    }
}