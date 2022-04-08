namespace CSharpCAD;

public static partial class CSCAD
{
    /**
     * <summary>Construct a polygon in two dimensional space from a list of points, or a list of points and paths.</summary>
     * NOTE: The ordering of points is VERY IMPORTANT.
     * @param {Array} options.points - points of the polygon : list of 2D points
     * @param {Array} [options.paths] - paths of the polygon : list of list of point indexes
     * @returns {geom2} new 2D geometry
     *
     * @example
     * var points = new Points2 {
     *   // roof
     *   [10,11], [0,11], [5,20],
     *   // wall
     *   [0,0], [10,0], [10,10], [0,10]
     *  };
     *  var paths = new Paths {
     *    new Path { 0, 1, 2},
     *    new Path { 3, 4, 5, 6}
     *  };
     *
     * var poly = Polygon(new Opts {{ "points", points}, {"paths", paths }});
     */
    public static Geom2 Polygon(Opts opts)
    {
        var points = opts.GetListOfVec2("points", new List<Vec2>(0));
        var paths = opts.GetListOfListOfInt("paths", new List<List<int>>(0));

        var listofpolys = points;

        foreach (var path in paths)
        {
            if (path.Count < 3) throw new ArgumentException("In option paths, each path must contain three or more points.");
        }

        var listofpaths = paths;
        if (paths.Count == 0)
        {
            // create a list of paths based on the points
            listofpaths = new List<List<int>>(1);
            listofpaths.Add(new List<int>(listofpolys.Count));
            for (int i = 0; i < listofpolys.Count; i++)
            {
                listofpaths[0].Add(i);
            }
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
