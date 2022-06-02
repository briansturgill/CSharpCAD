namespace CSharpCAD;

public static partial class CSCAD
{
    /*
     * Original source from quickhull3d (https://github.com/mauriciopoppe/quickhull3d)
     * Copyright (c) 2015 Mauricio Poppe
     *
     * Adapted to JSCAD by Jeff Gay
     */

    internal static double PointLineDistanceSquared(Vec3 p, Vec3 a, Vec3 b)
    {
        // == parallelogram solution
        //
        //            s
        //      __a________b__
        //       /   |    /
        //      /   h|   /
        //     /_____|__/
        //    p
        //
        //  s = b - a
        //  area = s * h
        //  |ap x s| = s * h
        //  h = |ap x s| / s
        //
        var ab = b.Subtract(a);
        var ap = p.Subtract(a);
        var cr = ap.Cross(ab);
        var area = cr.SquaredLength();
        var s = ab.SquaredLength();
        if (s == 0)
        {
            throw new ArgumentException("a and b are the same point");
        }
        return area / s;
    }

    internal static double PointLineDistance(Vec3 point, Vec3 a, Vec3 b) => Math.Sqrt(PointLineDistanceSquared(point, a, b));
}