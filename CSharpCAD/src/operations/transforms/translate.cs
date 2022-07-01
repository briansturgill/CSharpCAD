namespace CSharpCAD;


public static partial class CSCAD
{
    /**
     * <summary>Translate (move) the given geometry.</summary>
     * <param name="offset">The vector of offsets to applied to the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     * <group>Transformations</group>
     */
    public static Geom3 Translate(Vec3 offset, Geom3 g)
    {
        var matrix = Mat4.FromTranslation(offset);
        return g.Transform(matrix);
    }

    /**
     * <summary>Translate the given objects along the X axis.</summary>
     * <param name="offset">X offset of which to translate the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     * <group>Transformations</group>
     */
    public static Geom3 TranslateX(double offset, Geom3 g) => Translate(new Vec3(offset, 0, 0), g);

    /**
     * <summary>Translate the given objects along the Y axis.</summary>
     * <param name="offset">Y offset of which to translate the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     * <group>Transformations</group>
     */
    public static Geom3 TranslateY(double offset, Geom3 g) => Translate(new Vec3(0, offset, 0), g);

    /**
     * <summary>Translate the given objects along the Z axis.</summary>
     * <param name="offset">Z offset of which to translate the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     * <group>Transformations</group>
     */
    public static Geom3 TranslateZ(double offset, Geom3 g) => Translate(new Vec3(0, 0, offset), g);

    /**
     * <summary>Translate (move) the given geometry.</summary>
     * <param name="offset">The vector of offsets to applied to the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     * <group>Transformations</group>
     */
    public static Geom2 Translate(Vec2 offset, Geom2 g)
    {
        var matrix = Mat4.FromTranslation(new Vec3(offset, 0));
        return g.Transform(matrix);
    }

    /**
     * <summary>Translate the given objects along the X axis.</summary>
     * <param name="offset">X offset of which to translate the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     * <group>Transformations</group>
     */
    public static Geom2 TranslateX(double offset, Geom2 g) => Translate(new Vec2(offset, 0), g);

    /**
     * <summary>Translate the given objects along the Y axis.</summary>
     * <param name="offset">Y offset of which to translate the object.</param>
     * <param name="g">The geometry object to translate.</param>
     * <returns>The translated geometry object.</returns>
     * <group>Transformations</group>
     */
    public static Geom2 TranslateY(double offset, Geom2 g) => Translate(new Vec2(0, offset), g);
}
