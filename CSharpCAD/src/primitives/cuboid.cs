namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct an axis-aligned solid cuboid in three dimensional space.</summary>
    /// <group>3D Primitives</group>
    public static Geom3 Cuboid(Vec3? size = null, Vec3? center = null)
    {
        var _size = size ?? new Vec3(2.0, 2.0, 2.0);
        var _center = center ?? new Vec3(_size.x/2.0, _size.y/2.0, _size.z/2.0);

        if (_size.x <= 0 || _size.y <= 0 || _size.z <= 0)
        {
            throw new ArgumentException("All values in \"size\" must be greater than zero");
        }

        // adjust a basic shape to size
        var shape = new (int[], int[])[] {
          (new int[] {0, 4, 6, 2}, new int[] {-1, 0, 0}),
          (new int[] {1, 3, 7, 5}, new int[] {+1, 0, 0}),
          (new int[] {0, 1, 5, 4}, new int[] {0, -1, 0}),
          (new int[] {2, 6, 7, 3}, new int[] {0, +1, 0}),
          (new int[] {0, 2, 3, 1}, new int[] {0, 0, -1}),
          (new int[] {4, 5, 7, 6}, new int[] {0, 0, +1})
        };

        int cjsi(int expr) => expr != 0 ? 1 : 0; // cjsi == Crazy Javascript idiom, the meaning of !! in front of an expr

        var polygons = new List<List<Vec3>>(6);
        foreach (var (info, _) in shape)
        {
            var points = new List<Vec3>(4);
            foreach (var i in info)
            {
                points.Add(new Vec3(
                  _center.x + (_size.x / 2) * (2 * cjsi(i & 1) - 1),
                  _center.y + (_size.y / 2) * (2 * cjsi(i & 2) - 1),
                  _center.z + (_size.z / 2) * (2 * cjsi(i & 4) - 1)
                ));
            }
            polygons.Add(points);
        }
        return new Geom3(polygons);
    }
}