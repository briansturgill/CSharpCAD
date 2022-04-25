namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a polyhedron in 3D space from the given set of 3D points and faces.</summary>
     * <remarks>
     * The faces can define outward or inward facing polygons (orientation).
     * However, each face must define a counter clockwise rotation of points which follows the right hand rule.
     * </remarks>
     * <param name="points">List of points in 3D space.</param>
     * <param name="faces">List of faces, where each face is a set of indexes into the points.</param>
     * <param name="colors=undefined">List of RGBA colors to apply to each face.</param>
     * <param name="orientationOutward">Orientation of faces is outward?</param>
     * <example>
     * var mypoints = new Points3 { (10, 10, 0), (10, -10, 0), (-10, -10, 0), (-10, 10, 0), (0, 0, 10) };
     * var myfaces = new Faces { new Face {0, 1, 4}, new Face { 1, 2, 4 },
     *     new Face {2, 3, 4}, new Face{3, 0, 4}, new Face {1, 0, 3}, new Face{2, 1, 3} };
     * var myshape = Polyhedron(points: mypoints, faces: myfaces, orientationOutward: false);
     * </example>
     * <group>3D Primitives</group>
     */
    public static Geom3 Polyhedron(Points3 points, Faces faces,
        List<Color>? colors = null, bool orientationOutward = true)
    {
        if (points.Count < 3)
        {
            throw new ArgumentException("Three or more points are required.");
        }
        if (faces.Count < 1)
        {
            throw new ArgumentException("One or more faces are required.");
        }
        if (colors is not null && colors.Count != 0)
        {
            if (colors.Count != faces.Count)
            {
                throw new ArgumentException("Lists for options faces and colors must have the same length.");
            }
        }

        // invert the faces if orientation is inwards, as all internals expect outwarding facing polygons
        if (!orientationOutward)
        {
            var newFaces = new Faces(faces.Count);
            foreach (var face in faces)
            {
                var newFace = new Face();
                newFace.AddRange(face);
                newFace.Reverse();
                newFaces.Add(newFace);
            }
            faces = newFaces;
        }

        var polygons = new List<Poly3>(faces.Count);
        for (var i = 0; i < faces.Count; i++)
        {
            var pts = new List<Vec3>(faces[i].Count);
            for (var j = 0; j < faces[i].Count; j++)
            {
                pts.Add(points[faces[i][j]]);
            }
            polygons.Add(new Poly3(pts, (colors is not null && colors.Count != 0) ? colors[i] : (Color?)null));
        }

        return new Geom3(polygons.ToArray());
    }
}
