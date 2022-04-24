namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct an axis-aligned solid cube in three dimensional space with six square faces.</summary>
    /// <group>3D Primitives</group>
    public static Geom3 Cube(double size = 2.0, Vec3? center = null)
    {
        var _center = center ?? new Vec3(size / 2, size / 2, size / 2);
        return Cuboid(size: (size, size, size), center: _center);
    }
}
