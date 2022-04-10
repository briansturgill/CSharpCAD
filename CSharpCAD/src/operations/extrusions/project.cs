namespace CSharpCAD;

public static partial class CSCAD
{

    private static Geom2 ProjectGeom3(Geom3 obj, Vec3 axis, Vec3 origin)
    {
        // create a plane from the options, and verify
        var projplane = new Plane(axis, origin);
        if (double.IsNaN(projplane.Normal.x) || double.IsNaN(projplane.Normal.y)
          || double.IsNaN(projplane.Normal.z) || double.IsNaN(projplane.W))
        {
            throw new ArgumentException("project: invalid axis or origin arguments.");
        }

        var epsilon = obj.MeasureEpsilon();
        var epsilonArea = (epsilon * epsilon * Math.Sqrt(3) / 4);


        if (epsilon == 0) return new Geom2();

        // project the polygons to the plane
        var polygons = obj.ToPolygons();
        var projpolys = new List<Poly3>(polygons.Length);
        for (var i = 0; i < polygons.Length; i++)
        {
            var plen = polygons[i].Vertices.Length;
            var newpoints = new Vec3[plen];
            for (var j = 0; j < plen; j++)
            {
                var vertex = polygons[i].Vertices[j];
                newpoints[j] = projplane.ProjectionOfPoint(vertex);
            }
            var newpoly = new Poly3(newpoints);
            // only keep projections that have a measurable area
            if (newpoly.Area() < epsilonArea) continue;
            // only keep projections that face the same direction as the plane
            var newplane = newpoly.Plane();
            if (!AboutEqualNormals(projplane.Normal, newplane.Normal)) continue;
            projpolys.Add(newpoly);
        }
        // union the projected polygons to eliminate overlaying polygons
        var projection = new Geom3(projpolys.ToArray());
        projection = UnionGeom3(projection, projection);
        // rotate the projection to lay on X/Y axes if necessary
        if (!AboutEqualNormals(projplane.Normal, new Vec3(0, 0, 1)))
        {
            var rotation = Mat4.FromVectorRotation(projplane.Normal, new Vec3(0, 0, 1));
            projection = projection.Transform(rotation);
        }

        // convert the projection (polygons) into a series of 2D geometry
        List<Vec2> toListVec2(List<Vec3> inList)
        {
            var outList = new List<Vec2>(inList.Count);
            foreach (var v in inList)
            {
                outList.Add(new Vec2(v.x, v.y)); // Tossing the z.
            }
            return outList;
        }
        var polys3D = projection.ToPolygons();
        var len = polys3D.Length;
        var projections2D = new List<Geometry>(len);
        for (var i = 0; i < len; i++) {
            projections2D.Add(new Geom2(toListVec2(polys3D[i].ToPoints())));
        }
        // union the 2D geometries to obtain the outline of the projection
        var projection2D = UnionGeom2(projections2D.ToArray());

        return projection2D;
    }

    /**
     * Project the given 3D geometry onto the given plane.
     * <param name="obj">The given 3D geometry object.</param>
     * <param name="axis">The axis of the plane. Default: [0,0,1] (the Z axis).</param>
     * <param name="origin">The origin of the plane. Default [0,0,0].</param>
     * <returns>The projected 2D geometry.</returns>
     */
    public static Geom2 Project(Geom3 obj, Vec3? axis = null, Vec3? origin = null)
    {
        Vec3 _axis = axis ?? new Vec3(0, 0, 1);
        Vec3 _origin = origin ?? new Vec3(0, 0, 0);

        return ProjectGeom3(obj, axis: _axis, origin: _origin);
    }
}