namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct an axis-aligned solid cube in three dimensional space with six square faces.</summary>
    /// <remarks>
    /// You may have actually wanted to use a Cuboid.
    /// To be clear, this makes a "square" cube.
    /// The default center point is selected such that the bottom left
    /// corner of the cube is (0,0,0). (The cube is entirely in the first quadrant.)
    /// </remarks>
    /// <param name="size">The length of all three dimensions.</param>
    /// <param name="center" default="(size/2,size/2,size/2)">The center point of the cube.</param>
    /// <example>
    /// var g = Cube(size: 10)); // Makes a 10x10x10 Cuboid.
    /// </example>
    /// <group>3D Primitives</group>
    public static Geom3 Cube(double size = 2.0, Vec3? center = null)
    {
        return Cuboid(size: new Vec3(size, size, size), center: center);
    }
}
