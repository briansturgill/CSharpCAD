namespace CSharpCAD;


public static partial class CSCAD
{


    // Compare two normals (unit vectors) for equality.
    private static bool AboutEqualNormals(Vec3 a, Vec3 b) => (Math.Abs(a.X - b.X) <= C.NEPS &&
      Math.Abs(a.Y - b.Y) <= C.NEPS && Math.Abs(a.Z - b.Z) <= C.NEPS);

    private static bool Coplanar(Plane plane1, Plane plane2)
    {
        // expect the same distance from the origin, within tolerance
        if (Math.Abs(plane1.W - plane2.W) < 0.00000015)
        {
            return AboutEqualNormals(plane1.Normal, plane2.Normal);
        }
        return false;
    }

    /*
      After boolean operations all coplanar polygon fragments are joined by a retesselating
      operation. geom3.reTesselate(geom).
      Retesselation is done through a linear sweep over the polygon surface.
      The sweep line passes over the y coordinates of all vertices in the polygon.
      Polygons are split at each sweep line, and the fragments are joined horizontally and vertically into larger polygons
      (making sure that we will end up with convex polygons).
    */
    internal static Geom3 Retessellate(Geom3 geometry)
    {
        if (geometry.IsRetesselated)
        {
            return geometry;
        }

        var polygons = geometry.ToPolygons();
        var polygonsPerPlane = new List<(Plane, List<Poly3>)>();  // elements: [plane, [poly3...]]
        foreach (var polygon in polygons)
        {
            var polygon_plane = polygon.Plane();
            var mapping = false;
            foreach (var (plane, polys) in polygonsPerPlane)
                if (Coplanar(plane, polygon_plane))
                {
                    mapping = true;
                    polys.Add(polygon);
                    break;
                }
            if (!mapping)
            {
                polygonsPerPlane.Add((polygon_plane, new List<Poly3> { polygon }));
            }
        }
        var destpolygons = new List<Poly3>();
        foreach (var (plane, sourcepolygons) in polygonsPerPlane)
        {
            var retesselayedpolygons = ReTessellateCoplanarPolygons(sourcepolygons);
            destpolygons.AddRange(retesselayedpolygons);
        }

        var result = new Geom3(destpolygons.ToArray(), new Mat4(), geometry.Color, true);

        return result;
    }
}