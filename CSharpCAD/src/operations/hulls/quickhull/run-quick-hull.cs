namespace CSharpCAD;

/*
 * Original source from quickhull3d (https://github.com/mauriciopoppe/quickhull3d)
 * Copyright (c) 2015 Mauricio Poppe
 *
 * Adapted to JSCAD by Jeff Gay
 */

public static partial class CSCAD
{
    internal static Poly3[] RunQuickHull(HashSet<Vec3> hs_points, bool skipTriangulation)
    {
        var points = hs_points.ToArray();
        var instance = new QuickHull(points);
        instance.Build();
        var faces = instance.CollectFaces(skipTriangulation);
        var polys = new Poly3[faces.Count];
        var pi = 0;
        foreach (var face in faces)
        {
            polys[pi++] = new Poly3(face.Select((idx) => points[idx]).ToArray());
        }
        return polys;
    }
}