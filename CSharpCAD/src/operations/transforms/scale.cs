namespace CSharpCAD;

public static partial class CSCAD
{

    /// <summary>Scale the given geometry object using the given factors.</summary>
    /// <remarks>The Z point is ignored for 2d geometry objects.</remarks>
    /// <group>Transformations</group>
    public static Geometry Scale(Vec3 factors, Geometry gobj)
    {
        if (gobj.Is2D && factors.z != 1)
        {
            factors = new Vec3(factors.x, factors.y, 1);
        }

        if (factors.x <= 0 || factors.y <= 0 || factors.z <= 0)
        {
            throw new ArgumentException("Argument factors must be positive.");
        }

        var matrix = Mat4.FromScaling(factors);

        if (gobj.Is3D)
        {
            return ((Geom3)gobj).Transform(matrix);
        }
        else
        {
            return ((Geom2)gobj).Transform(matrix);
        }
    }

    /// <summary>Scale the given objects about the X axis using the given factor.</summary>
    /// <group>Transformations</group>
    public static Geometry ScaleX(double factor, Geometry gobj) => Scale(new Vec3(factor, 1, 1), gobj);

    /// <summary>Scale the given objects about the Y axis using the given factor.</summary>
    /// <group>Transformations</group>
    public static Geometry ScaleY(double factor, Geometry gobj) => Scale(new Vec3(1, factor, 1), gobj);

    /// <summary>Scale the given objects about the Z axis using the given factor.</summary>
    /// <group>Transformations</group>
    public static Geometry ScaleZ(double factor, Geometry gobj) => Scale(new Vec3(1, 1, factor), gobj);

}
