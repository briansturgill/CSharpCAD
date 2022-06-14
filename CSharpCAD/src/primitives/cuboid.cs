namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct an axis-aligned solid cuboid in three dimensional space.</summary>
    /// <remarks>
    /// The default center point is selected such that the bottom left
    /// corner of the cuboid is (0,0,0). (The cuboid is entirely in the first quadrant.)
    /// </remarks>
    /// <param name="size" default="(2,2,2)">A vector of the length of each dimension.</param>
    /// <param name="center" default="(size.X/2,size.Y/2,size.Z/2)">The center point of the cube.</param>
    /// <example>
    /// var g = Cuboid(size: (10,20,10));
    /// </example>
    /// <group>3D Primitives</group>
    public static Geom3 Cuboid(Vec3? size = null, Vec3? center = null)
    {
        var _size = size ?? new Vec3(2.0, 2.0, 2.0);
        Vec2? v2center = null;
        double? center_z = null;
        if (center is not null)
        {
            v2center = new Vec2(((Vec3)center).X, ((Vec3)center).Y);
            center_z = ((Vec3)center).Z;
        }

        if (_size.X <= 0 || _size.Y <= 0 || _size.Z <= 0)
        {
            throw new ArgumentException("All values in \"size\" must be greater than zero");
        }

        return InternalExtrudeSimple(InternalRectangle(new Vec2(_size.X, _size.Y), v2center), _size.Z, center_z);
    }
}