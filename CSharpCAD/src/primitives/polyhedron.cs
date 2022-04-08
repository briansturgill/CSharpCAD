namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * Construct a polyhedron in three dimensional space from the given set of 3D points and faces.
     * The faces can define outward or inward facing polygons (orientation).
     * However, each face must define a counter clockwise rotation of points which follows the right hand rule.
     * @param {Object} options - options for construction
     * @param {Array} options.points - list of points in 3D space
     * @param {Array} options.faces - list of faces, where each face is a set of indexes into the points
     * @param {Array} [options.colors=undefined] - list of RGBA colors to apply to each face
     * @param {Array} [options.orientation="outward"] - orientation of faces
     * @returns {geom3} new 3D geometry
     * @alias module:modeling/primitives.polyhedron
     *
     * @example
     * var mypoints = [ [10, 10, 0], [10, -10, 0], [-10, -10, 0], [-10, 10, 0], [0, 0, 10] ]
     * var myfaces = [ [0, 1, 4], [1, 2, 4], [2, 3, 4], [3, 0, 4], [1, 0, 3], [2, 1, 3] ]
     * var myshape = polyhedron({points: mypoint, faces: myfaces, orientation: "inward"})
     */
    public static Geom3 Polyhedron(Opts opts)
    {
        var points = opts.GetListOfVec3("points", new List<Vec3>(0));
        var faces = opts.GetListOfListOfInt("faces", new List<List<int>>(0));
        var colors = opts.GetListOfColor("colors", new List<Color>(0));
        var orientation = opts.GetString("orientation", "outward");

        if (points.Count < 3)
        {
            throw new ArgumentException("Three or more points are required.");
        }
        if (faces.Count < 1)
        {
            throw new ArgumentException("One or more faces are required.");
        }
        if (colors.Count != 0)
        {
            if (colors.Count != faces.Count)
            {
                throw new ArgumentException("Lists for options faces and colors must have the same length.");
            }
        }

        // invert the faces if orientation is inwards, as all internals expect outwarding facing polygons
        if (orientation != "outward")
        {
            var newFaces = new List<List<int>>(faces.Count);
            foreach (var face in faces)
            {
                var newFace = face.ToList();
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
            polygons.Add(new Poly3(pts, (colors.Count != 0) ? colors[i] : null));
        }

        return new Geom3(polygons.ToArray());
    }
}
