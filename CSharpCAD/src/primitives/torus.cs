namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a torus by revolving a small circle (inner) about the circumference of a large (outer) circle.</summary>
     * @param {Object} [options] - options for construction
     * @param {Number} [options.innerRadius=1] - radius of small (inner) circle
     * @param {Number} [options.outerRadius=4] - radius of large (outer) circle
     * @param {Integer} [options.innerSegments=32] - number of segments to create per rotation
     * @param {Integer} [options.outerSegments=32] - number of segments to create per rotation
     * @param {Integer} [options.innerRotation=0] - rotation of small (inner) circle in radians
     * @param {Number} [options.outerRotation=(PI * 2)] - rotation (outer) of the torus (RADIANS)
     * @param {Number} [options.startAngle=0] - start angle of the torus (RADIANS)
     * @returns {geom3} new 3D geometry
     * @alias module:modeling/primitives.torus
     *
     * @example
     * var myshape = torus({ innerRadius: 10, outerRadius: 100 })
     */
    public static Geom3 Torus(double innerRadius = 1, int innerSegments = 32, double outerRadius = 4, int outerSegments = 32,
        double innerRotation = 0, double startAngle = 0, double outerRotation = Math.PI * 2)
    {
        if (innerRadius <= 0) throw new ArgumentException("Option innerRadius must be greater than zero");
        if (innerSegments < 3) throw new ArgumentException("Option innerSegments must be three or more");
        if (outerRadius <= 0) throw new ArgumentException("Option outerRadius must be greater than zero");
        if (outerSegments < 3) throw new ArgumentException("Option outerSegments must be three or more");
        if (startAngle < 0) throw new ArgumentException("Option startAngle must be positive");
        if (outerRotation <= 0) throw new ArgumentException("Option outerRotation must be greater than zero");
        if (innerRadius >= outerRadius) throw new ArgumentException("Inner circle is too large to rotate about the outer circle.");

        var innerCircle = Circle(new Opts { { "radius", innerRadius }, { "segments", innerSegments } });

        if (innerRotation != 0)
        {
            innerCircle = (Geom2)Rotate(new Vec3(0, 0, innerRotation), innerCircle);
        }

        innerCircle = (Geom2)Translate(new Vec3(outerRadius, 0, 0), innerCircle);

        return (Geom3)ExtrudeRotate(innerCircle, startAngle: startAngle, angle: outerRotation, segments: outerSegments);
    }
}
