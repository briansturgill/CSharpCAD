namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Scale the given geometry object using the given factors.</summary>
    /// <group>Transformations</group>
    public static Geom2 Scale(Vec2 factors, Geom2 gobj)
    {
        var _factors = new Vec3(factors.X, factors.Y, 1);

        if (factors.X <= 0 || factors.Y <= 0)
        {
            throw new ArgumentException("Argument factors must be positive.");
        }

        var matrix = Mat4.FromScaling(_factors);

        return gobj.Transform(matrix);
    }

    /// <summary>Scale the given geometry object using the given factors.</summary>
    /// <group>Transformations</group>
    public static Geom3 Scale(Vec3 factors, Geom3 gobj)
    {
        if (factors.X <= 0 || factors.Y <= 0 || factors.Z <= 0)
        {
            throw new ArgumentException("Argument factors must be positive.");
        }

        var matrix = Mat4.FromScaling(factors);

        return gobj.Transform(matrix);
    }

    /// <summary>Scale the given objects about the X axis using the given factor.</summary>
    /// <group>Transformations</group>
    public static Geom2 ScaleX(double factor, Geom2 gobj) => Scale(new Vec2(factor, 1), gobj);

    /// <summary>Scale the given objects about the Y axis using the given factor.</summary>
    /// <group>Transformations</group>
    public static Geom2 ScaleY(double factor, Geom2 gobj) => Scale(new Vec2(1, factor), gobj);

    /// <summary>Scale the given objects about the X axis using the given factor.</summary>
    /// <group>Transformations</group>
    public static Geom3 ScaleX(double factor, Geom3 gobj) => Scale(new Vec3(factor, 1, 1), gobj);

    /// <summary>Scale the given objects about the Y axis using the given factor.</summary>
    /// <group>Transformations</group>
    public static Geom3 ScaleY(double factor, Geom3 gobj) => Scale(new Vec3(1, factor, 1), gobj);

    /// <summary>Scale the given objects about the Z axis using the given factor.</summary>
    /// <group>Transformations</group>
    public static Geom3 ScaleZ(double factor, Geom3 gobj) => Scale(new Vec3(1, 1, factor), gobj);

}