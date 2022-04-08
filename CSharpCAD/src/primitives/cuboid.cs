namespace CSharpCAD;

public static partial class CSCAD
{
    /// <summary>Construct an axis-aligned solid cuboid in three dimensional space.</summary>
    /// opts -- size: Vec3, center: Vec3
    public static Geom3 Cuboid(Opts opts)
    {
        var size = opts.GetVec3("size", (2.0, 2.0, 2.0));
        var center = opts.GetVec3("center", (0.0, 0.0, 0.0));

        if (size.x <= 0 || size.y <= 0 || size.z <= 0)
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
                  center.x + (size.x / 2) * (2 * cjsi(i & 1) - 1),
                  center.y + (size.y / 2) * (2 * cjsi(i & 2) - 1),
                  center.z + (size.z / 2) * (2 * cjsi(i & 4) - 1)
                ));
            }
            polygons.Add(points);
        }
        return new Geom3(polygons);
    }
}
