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
        var listofpolys = points;

        if (paths is not null)
        {
            foreach (var path in paths)
            {
                if (path.Count < 3) throw new ArgumentException("In the \"paths\" argument, each path must contain three or more points.");
            }
        }

        Paths listofpaths;
        if (paths is null || paths.Count == 0)
        {
            // create a list of paths based on the points
            listofpaths = new Paths(1);
            listofpaths.Add(new Path(listofpolys.Count));
            for (int i = 0; i < listofpolys.Count; i++)
            {
                listofpaths[0].Add(i);
            }
        }
        else
        {
            listofpaths = paths;
        }


        var allpoints = listofpolys;

        var sides = new List<Geom2.Side>();
        foreach (var path in listofpaths)
        {
            var setofpoints = new List<Vec2>(path.Count);
            for (var i = 0; i < path.Count; i++)
            {
                setofpoints.Add(allpoints[path[i]]);
            }
            var geometry = new Geom2(setofpoints);
            sides.AddRange(geometry.ToSides());
        }
        return new Geom2(sides.ToArray());
    }
}
