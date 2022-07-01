namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Transform the geometry object using the given matrix.</summary>
    /// <group>Transformations</group>
    public static Geom2 Transform(Mat4 matrix, Geom2 gobj)
    {
        return gobj.Transform(matrix);
    }

    /// <summary>Transform the geometry object using the given matrix.</summary>
    /// <group>Transformations</group>
    public static Geom3 Transform(Mat4 matrix, Geom3 gobj)
    {
        return gobj.Transform(matrix);
    }
}