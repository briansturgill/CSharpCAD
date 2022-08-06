namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a polygon in two dimensional space from a list of points, or a list of points and paths.</summary>
     * <remarks>
     * NOTE: The ordering of points is VERY IMPORTANT.
     * Polygon points must be in counter-clockwise order, convex shape, all points coplanar.
     * Use the ".Validate()" method on the resulting method to check if the data is good.
     * If "paths" is omitted, all points will be used in a single polygon in the order given.
     * </remarks>
     * <param name="points">Points of the polygon : list of 2D points.</param>
     * <param name="paths">Paths of the polygon : list of list of point indexes.</param>
     * <example>
     * var points = new Points2 {
     *   // roof
     *   (10,11), (0,11), (5,20),
     *   // wall
     *   (0,0), (10,0), (10,10), (0,10)
     *  };
     *  var paths = new Paths {
     *    new Path { 0, 1, 2},
     *    new Path { 3, 4, 5, 6}
     *  };
     *
     * var poly = Polygon(points: points, paths: paths);
     * poly.Validate(); // Will throw an exception with explanatory message if polygon is bad.
     * </example>
     * <group>2D Primitives</group>
     */
    public static Geom2 Polygon(Points2 points, Paths? paths = null)
    {
        var nrtree = new Geom2.NRTree();

        if (paths is not null)
        {
            foreach (var path in paths)
            {
                if (path.Count < 3) throw new ArgumentException("In the \"paths\" argument, each path must contain three or more points.");
            }
            foreach (var path in paths)
            {
                var npath = new List<Vec2>(path.Count);
                for (int i = 0; i < path.Count; i++)
                {
                    npath.Add(points[path[i]]);
                }
                nrtree.Insert(npath.ToArray());
            }
        }
        else
        {
            nrtree.Insert(points.ToArray());
        }
        // LATER we need to check windings and have good error messages.
        return new Geom2(nrtree);
    }
}