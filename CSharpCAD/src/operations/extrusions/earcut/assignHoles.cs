namespace CSharpCAD;

public static partial class CSCAD
{

    internal static partial class Earcut
    {
        /*
         * Constructs a polygon hierarchy of solids and holes.
         * The hierarchy is represented as a forest of trees. All trees shall be depth at most 2.
         * If a solid exists inside the hole of another solid, it will be split out as its own root.
         *
         * @param {geom2} geometry
         * @returns {Array} an array of polygons with associated holes
         * @alias module:modeling/geometries/geom2.toTree
         *
         * @example
         * var geometry = subtract(rectangle({size: [5, 5]}), rectangle({size: [3, 3]}))
         * console.log(assignHoles(geometry))
         * [{
         *   "solid": [[-2.5,-2.5],[2.5,-2.5],[2.5,2.5],[-2.5,2.5]],
         *   "holes": [[[-1.5,1.5],[1.5,1.5],[1.5,-1.5],[-1.5,-1.5]]]
         * }]
         */
        private static double Area(List<Vec2> points)
        {
            double area = 0;
            for (var i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % points.Count;
                area += points[i].x * points[j].y;
                area -= points[j].x * points[i].y;
            }
            return (area / 2.0);
        }

        /*
         * Determine if the given point is inside the polygon.
         *
         * @see http://erich.realtimerendering.com/ptinpoly/ (Crossings Test)
         * @param {Array} point - an array with X and Y values
         * @param {Array} polygon - a list of points, where each point is an array with X and Y values
         * @return {Integer} 1 if the point is inside, 0 if outside
         */
        internal static bool IsPointInside(Vec2 point, List<Vec2> polygon)
        {
            var numverts = polygon.Count;

            var tx = point.x;
            var ty = point.y;

            var vtx0 = polygon[numverts - 1];
            var vtx1 = polygon[0];

            var yflag0 = (vtx0.y > ty);

            var insideFlag = false;

            for(var i=0; i<numverts; i++)
            {
                vtx1 = polygon[i];
                /*
                 * check if Y endpoints straddle (are on opposite sides) of point's Y
                 * if so, +X ray could intersect this edge.
                 */
                var yflag1 = (vtx1.y > ty);
                if (yflag0 != yflag1)
                {
                    /*
                     * check if X endpoints are on same side of the point's X
                     * if so, it's easy to test if edge hits or misses.
                     */
                    var xflag0 = (vtx0.x > tx);
                    var xflag1 = (vtx1.x > tx);
                    if (xflag0 && xflag1)
                    {
                        /* if edge's X values are both right of the point, then the point must be inside */
                        insideFlag = !insideFlag;
                    }
                    else
                    {
                        /*
                         * if X endpoints straddle the point, then
                         * the compute intersection of polygon edge with +X ray
                         * if intersection >= point's X then the +X ray hits it.
                         */
                        if ((vtx1.x - (vtx1.y - ty) * (vtx0.x - vtx1.x) / (vtx0.y - vtx1.y)) >= tx)
                        {
                            insideFlag = !insideFlag;
                        }
                    }
                }
                /* move to next pair of vertices, retaining info as possible */
                yflag0 = yflag1;
                vtx0 = vtx1;
            }
            return insideFlag;
        }

        /*
         * > 0 : p2 is left of the line p0 -> p1
         * = 0 : p2 is on the line p0 -> p1
         * < 0 : p2 is right of the line p0 -> p1
         */
        private static double IsLeft(Vec2 p0, Vec2 p1, Vec2 p2) => (p1.x - p0.x) * (p2.y - p0.y) - (p2.x - p0.x) * (p1.y - p0.y);

        internal static List<(List<Vec2>, List<List<Vec2>>)> AssignHoles(Geom2 geometry)
        {
            var outlines = geometry.ToOutlines();
            var solids = new List<int>(); // solid indices
            var holes = new List<int>(); // hole indices
            for (var i = 0; i < outlines.Count; i++)
            {
                var outline = outlines[i];
                var a = Area(outline);
                if (a < 0)
                {
                    holes.Add(i);
                }
                else if (a > 0)
                {
                    solids.Add(i);
                }
            }

            // for each hole, determine what solids it is inside of
            var children = new Dictionary<int, List<int>>(); // child holes of solid[i]
            var parents = new Dictionary<int, List<int>>(); // parent solids of hole[i]
            for (var i = 0; i < solids.Count; i++)
            {
                var s = solids[i];
                var solid = outlines[s];
                children[i] = new List<int>();
                for (var j = 0; j < holes.Count; j++)
                {
                    var h = holes[j];
                    var hole = outlines[h];
                    // check if a point of hole j is inside solid i
                    if (IsPointInside(hole[0], solid))
                    {
                        children[i].Add(h);
                        if (!parents.ContainsKey(j)) parents[j] = new List<int>();
                        parents[j].Add(i);
                    }
                }
            }

            // check if holes have multiple parents and choose one with fewest children
            for (var j = 0; j < holes.Count; j++)
            {
                var h = holes[j];
                // ensure at least one parent exists
                if (parents.ContainsKey(j) && parents[j].Count > 1)
                {
                    // the solid directly containing this hole
                    var directParent = MinIndex(parents[j], (p) => children[p].Count);
                    for (var i = 0; i < parents[j].Count; i++)
                    {
                        var p = parents[j][i];
                        if (i != directParent)
                        {
                            // Remove hole from skip level parents
                            children[p] = children[p].Where((c) => c != h).ToList();
                        }
                    }
                }
            }

            // map indices back to points
            var ret = new List<(List<Vec2>, List<List<Vec2>>)>();
            foreach (var (i, h_i_list) in children)
            {
                var hret = h_i_list.Select((h) => outlines[h]).ToList();
                var sret = outlines[solids[i]];
                ret.Add((sret, hret));
            }
            return ret;
        }

        /*
         * Find the item in the list with smallest score(item).
         * If the list is empty, return undefined.
         */
        private static int MinIndex(List<int> list, Func<int, int> score)
        {
            int bestIndex = -1;
            int best = -1;

            for (var index = 0; index < list.Count; index++)
            {
                var item = list[index];
                var value = score(item);

                if (best == -1 || value < best)
                {
                    bestIndex = index;
                    best = value;
                }
            }
            return bestIndex;
        }
    }
}