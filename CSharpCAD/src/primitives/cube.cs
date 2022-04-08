namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct an axis-aligned solid cube in three dimensional space with six square faces.</summary>
    public static Geom3 Cube(Opts opts)
    {
        var size = opts.GetDouble("size", 2.0);
        var center = opts.GetVec3("center", (0.0, 0.0, 0.0));
        return Cuboid(new Opts { { "size", (size, size, size) }, { "center", (center.x, center.y, center.z) } });
    }
}
