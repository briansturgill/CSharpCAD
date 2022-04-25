namespace CSharpCAD;

public static partial class CSCAD
{

    internal static Geom3 ExtrudeLinearGeom2(Geom2 obj, Vec3 offset, double twistAngle = 0, int twistSteps = 12, bool repair = true)
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
        if (offsetv.Z < 0) baseSlice = baseSlice.Reverse();

        Slice? createTwist(double progress, int index, Slice baseSlice)
        {
            var Zrotation = index / (double)twistSteps * twistAngle;
            var Zoffset = offsetv.Scale(index / (double)twistSteps);
            var matrix = Mat4.FromZRotation(Zrotation).Multiply(Mat4.FromTranslation(Zoffset));

            return baseSlice.Transform(matrix);
        }

        return ExtrudeFromSlices(baseSlice, createTwist, numberOfSlices: twistSteps + 1,
           capStart: true, capEnd: true, repair: true);
    }
}