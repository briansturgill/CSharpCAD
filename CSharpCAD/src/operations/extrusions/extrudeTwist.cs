namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Extrude the given geometry in an upward linear direction using the given options.</summary>
     * <remarks>By default, the method of triangulation will be automatically selected based on convexity.</remarks>
     * <param name="gobj">The geometries to extrude.</param>
     * <param name="height">The height of the extrusion.</param>
     * <param name="twistAngle">The final rotation (DEGREES) about the origin of the shape (if any).</param>
     * <param name="twistSteps">The resolution of the twist about the axis (if any).</param>
     * <param name="initialZ">The initial place segment's Z coordinate.</param>
     * <param name="useEarcut" default="calculated">Set to "true" or "false" to force selection of Earcut or midpoint triangulation.</param>
     * <group>3D Primitives</group>
     */
    public static Geom3 ExtrudeTwist(Geom2 gobj, double height = 1, double twistAngle = 90, int twistSteps = 1,
        double initialZ = 0, bool? useEarcut = null)
    {
        if (LessThanOrEqualish(height, 0)) throw new ArgumentException("Argument height must be greater than zero.");
        if (LessThanOrEqualish(twistSteps, 0)) throw new ArgumentException("Argument twistSteps must be greater than zero.");
        twistAngle = DegToRad(twistAngle);
        var se = new SegmentedExtruder(gobj, initialZ: initialZ, useEarcut: useEarcut);
        var sliceHeight = height / twistSteps;
        var anglePerStep = twistAngle / twistSteps;
        var i = 0;
        for (i = 1; i < twistSteps; i++)
        {
            var mat = Mat4.FromZRotation(anglePerStep*i);
            var seg = gobj.Transform(mat);
            se.AddSegment(seg, sliceHeight);
        }
        se.AddSegment(gobj.Transform(Mat4.FromZRotation(twistAngle)), height - ((i - 1) * sliceHeight));
        return se.Finished();
    }
}