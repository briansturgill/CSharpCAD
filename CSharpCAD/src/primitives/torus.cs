namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a torus by revolving a small circle (inner) about the circumference of a large (outer) circle.</summary>
     * <param name="innerRadius">Radius of small (inner) circle.</param>
     * <param name="outerRadius">Radius of large (outer) circle.</param>
     * <param name="innerSegments">Number of segments to create per rotation.</param>
     * <param name="outerSegments">Number of segments to create per rotation.</param>
     * <param name="innerRotation">Rotation of small (inner) circle (DEGREES).</param>
     * <param name="outerRotation">Rotation (outer) of the torus (DEGREES).</param>
     * <param name="startAngle">Start angle of the torus (DEGREES).</param>
     * <example>
     * var myshape = Torus(innerRadius: 10, outerRadius: 100);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 Torus(double innerRadius = 1, int innerSegments = 32, double outerRadius = 4, int outerSegments = 32,
        double innerRotation = 0, double startAngle = 0, double outerRotation = 360)
    {
        if (innerRadius <= 0) throw new ArgumentException("Option innerRadius must be greater than zero");
        if (innerSegments < 3) throw new ArgumentException("Option innerSegments must be three or more");
        if (outerRadius <= 0) throw new ArgumentException("Option outerRadius must be greater than zero");
        if (outerSegments < 3) throw new ArgumentException("Option outerSegments must be three or more");
        if (startAngle < 0) throw new ArgumentException("Option startAngle must be positive");
        if (outerRotation <= 0) throw new ArgumentException("Option outerRotation must be greater than zero");
        if (innerRadius >= outerRadius) throw new ArgumentException("Inner circle is too large to rotate about the outer circle.");

        // We do not translate to radians, that will be handled within Rotate/ExtrudeRotate.

        var innerCircle = Circle(radius: innerRadius, segments: innerSegments);

        if (innerRotation != 0)
        {
            // LATER innerRotation was the z argument, but that isn't used on a Geom2.
            innerCircle = Rotate(new Vec3(0, 0, innerRotation), innerCircle);
        }

        innerCircle = Translate(new Vec2(outerRadius, 0), innerCircle);

        return ExtrudeRotate(innerCircle, startAngle: startAngle, angle: outerRotation, segments: outerSegments);
    }
}
