namespace CSharpCAD;


public static partial class CSCAD
{
    /**
     * @ingroup Transforms
     * <summary>Translate (move) the given geometry.</summary>
     * <param name="offset">The vector of offsets to applied to the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     */
    public static Geometry Translate(Vec3 offset, Geometry g)
    {
        var matrix = Mat4.FromTranslation(offset);
        return g.Is2D ? ((Geom2)g).Transform(matrix) : ((Geom3)g).Transform(matrix);
    }

    /**
     * <summary>Translate the given objects along the X axis.</summary>
     * <param name="offset">X offset of which to translate the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     */
    public static Geometry TranslateX(double offset, Geometry g) => Translate(new Vec3(offset, 0, 0), g);

    /**
     * <summary>Translate the given objects along the Y axis.</summary>
     * <param name="offset">Y offset of which to translate the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     */
    public static Geometry TranslateY(double offset, Geometry g) => Translate(new Vec3(0, offset, 0), g);

    /**
     * <summary>Translate the given objects along the Z axis.</summary>
     * <param name="offset">Z offset of which to translate the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     */
    public static Geometry TranslateZ(double offset, Geometry g) => Translate(new Vec3(0, 0, offset), g);

}
