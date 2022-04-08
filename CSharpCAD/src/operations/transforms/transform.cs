namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Transform the geometry object using the given matrix.</summary>
    public static Geometry Transform(Mat4 matrix, Geometry gobj)
    {
        return gobj.Is2D ? ((Geom2)gobj).Transform(matrix) : ((Geom3)gobj).Transform(matrix);
    }
}