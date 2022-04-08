namespace CSharpCAD;

public static partial class CSCAD
{

    private class Outpolygon
    {
        public Vec2 topleft;
        public Vec2 topright;
        public Vec2 bottomleft;
        public Vec2 bottomright;
        public (Vec2, Vec2) leftline;
        public (Vec2, Vec2) rightline;
        public bool leftlinecontinues;
        public bool rightlinecontinues;
        public class _outpolygon
        {
            public List<Vec2> leftpoints;
            public List<Vec2> rightpoints;
            public _outpolygon()
            {
                leftpoints = new List<Vec2>();
                rightpoints = new List<Vec2>();
            }
        };
        public _outpolygon? outpolygon;
        public Outpolygon()
        {
            topleft = new Vec2();
            topright = new Vec2();
            bottomleft = new Vec2();
            bottomright = new Vec2();
            leftline = (new Vec2(), new Vec2());
            rightline = (new Vec2(), new Vec2());
            leftlinecontinues = false;
            rightlinecontinues = false;
            outpolygon = null;
        }
    };
    private class ActivePolygon
    {
        public int polygonindex;
        public int leftvertexindex;
        public int rightvertexindex;
        public Vec2 topleft;
        public Vec2 topright;
        public Vec2 bottomleft;
        public Vec2 bottomright;
    };

    /*
     * Retesselation for a set of COPLANAR polygons.
     * @param {poly3[]} sourcepolygons - list of polygons
     * @returns {poly3[]} new set of polygons
     */
    internal static List<Poly3> ReTessellateCoplanarPolygons(List<Poly3> sourcepolygons)
    {
        if (sourcepolygons.Count < 2) return sourcepolygons;

        var numpolygons = sourcepolygons.Count;
        var destpolygons = new List<Poly3>(numpolygons);
        var plane = sourcepolygons[0].Plane();
        var orthobasis = new OrthoNormalBasis(plane);
        var polygonvertices2d = new List<List<Vec2>>(); // array of array of Vector2D
        var polygontopvertexindexes = new List<int>(); // array of indexes of topmost vertex per polygon
        var topy2polygonindexes = new Dictionary<double, List<int>>();
        var ycoordinatetopolygonindexes = new Dictionary<double, Dictionary<int, bool>>();
        var ycoordinatebins = new Dictionary<double, double>();

        // convert all polygon vertices to 2D
        // Make a list of all encountered y coordinates
        // And build a map of all polygons that have a vertex at a certain y coordinate:
        var ycoordinateBinningFactor = 1.0 / C.EPS * 10;
        for (var polygonindex = 0; polygonindex < numpolygons; polygonindex++)
        {
            var poly3d = sourcepolygons[polygonindex];
            var vertices2d = new List<Vec2>();
            var numvertices = poly3d.Vertices.Length;
            var minindex = -1;
            if (numvertices > 0)
            {
                double miny = 0;
                double maxy = 0;
                for (var i = 0; i < numvertices; i++)
                {
                    var pos2d = orthobasis.To2D(poly3d.Vertices[i]);
                    // perform binning of y coordinates: If we have multiple vertices very
                    // close to each other, give them the same y coordinate:
                    var ycoordinatebin = Math.Floor(pos2d.y * ycoordinateBinningFactor);
                    double newy;
                    if (ycoordinatebins.ContainsKey(ycoordinatebin))
                    {
                        newy = ycoordinatebins[ycoordinatebin];
                    }
                    else if (ycoordinatebins.ContainsKey(ycoordinatebin + 1))
                    {
                        newy = ycoordinatebins[ycoordinatebin + 1];
                    }
                    else if (ycoordinatebins.ContainsKey(ycoordinatebin - 1))
                    {
                        newy = ycoordinatebins[ycoordinatebin - 1];
                    }
                    else
                    {
                        newy = pos2d.y;
                        ycoordinatebins[ycoordinatebin] = pos2d.y;
                    }
                    pos2d = new Vec2(pos2d.x, newy);
                    vertices2d.Add(pos2d);
                    var y = pos2d.y;
                    if ((i == 0) || (y < miny))
                    {
                        miny = y;
                        minindex = i;
                    }
                    if ((i == 0) || (y > maxy))
                    {
                        maxy = y;
                    }
                    if (!(ycoordinatetopolygonindexes.ContainsKey(y)))
                    {
                        ycoordinatetopolygonindexes[y] = new Dictionary<int, bool>();
                    }
                    ycoordinatetopolygonindexes[y][polygonindex] = true;
                }
                if (miny >= maxy)
                {
                    // degenerate polygon, all vertices have same y coordinate. Just ignore it from now:
                    vertices2d = new List<Vec2>();
                    numvertices = 0;
                    minindex = -1;
                }
                else
                {
                    if (!(topy2polygonindexes.ContainsKey(miny)))
                    {
                        topy2polygonindexes[miny] = new List<int>();
                    }
                    topy2polygonindexes[miny].Add(polygonindex);
                }
            } // if(numvertices > 0)
              // reverse the vertex order:
            vertices2d.Reverse();
            minindex = numvertices - minindex - 1;
            polygonvertices2d.Add(vertices2d);
            polygontopvertexindexes.Add(minindex);
        }
        var ycoordinates = new List<double>();
        foreach (var ycoordinate in ycoordinatetopolygonindexes.Keys) ycoordinates.Add(ycoordinate);
        ycoordinates.Sort(); // Was only doing by a-b, so default comparison should be fine.

        // Now we will iterate over all y coordinates, from lowest to highest y coordinate
        // activepolygons: source polygons that are 'active', i.e. intersect with our y coordinate
        //   Is sorted so the polygons are in left to right order
        // Each element in activepolygons has these properties:
        //        polygonindex: the index of the source polygon (i.e. an index into the sourcepolygons
        //                      and polygonvertices2d arrays)
        //        leftvertexindex: the index of the vertex at the left side of the polygon (lowest x)
        //                         that is at or just above the current y coordinate
        //        rightvertexindex: dito at right hand side of polygon
        //        topleft, bottomleft: coordinates of the left side of the polygon crossing the current y coordinate
        //        topright, bottomright: coordinates of the right hand side of the polygon crossing the current y coordinate
        var activepolygons = new List<ActivePolygon>();
        var prevoutpolygonrow = new List<Outpolygon>();
        for (var yindex = 0; yindex < ycoordinates.Count; yindex++)
        {
            var newoutpolygonrow = new List<Outpolygon>();

            // CBS C# translation note... apparently the were using a string to index, though I don't see actually being done.
            //var ycoordinateasstring = ycoordinates[yindex];
            //var ycoordinate = Number(ycoordinateasstring);
            var ycoordinate = ycoordinates[yindex];

            // update activepolygons for this y coordinate:
            // - Remove any polygons that end at this y coordinate
            // - update leftvertexindex and rightvertexindex (which point to the current vertex index
            //   at the the left and right side of the polygon
            // Iterate over all polygons that have a corner at this y coordinate:
            var polygonindexeswithcorner = ycoordinatetopolygonindexes[ycoordinate];
            for (var activepolygonindex = 0; activepolygonindex < activepolygons.Count; ++activepolygonindex)
            {
                var activepolygon = activepolygons[activepolygonindex];
                var polygonindex = activepolygon.polygonindex;
                if (polygonindexeswithcorner.ContainsKey(polygonindex) && polygonindexeswithcorner[polygonindex])
                {
                    // this active polygon has a corner at this y coordinate:
                    var vertices2d = polygonvertices2d[polygonindex];
                    var numvertices = vertices2d.Count;
                    var newleftvertexindex = activepolygon.leftvertexindex;
                    var newrightvertexindex = activepolygon.rightvertexindex;
                    // See if we need to increase leftvertexindex or decrease rightvertexindex:
                    while (true)
                    {
                        var nextleftvertexindex = newleftvertexindex + 1;
                        if (nextleftvertexindex >= numvertices) nextleftvertexindex = 0;
                        if (vertices2d[nextleftvertexindex].y != ycoordinate) break;
                        newleftvertexindex = nextleftvertexindex;
                    }
                    var nextrightvertexindex = newrightvertexindex - 1;
                    if (nextrightvertexindex < 0) nextrightvertexindex = numvertices - 1;
                    if (vertices2d[nextrightvertexindex].y == ycoordinate)
                    {
                        newrightvertexindex = nextrightvertexindex;
                    }
                    if ((newleftvertexindex != activepolygon.leftvertexindex) && (newleftvertexindex == newrightvertexindex))
                    {
                        // We have increased leftvertexindex or decreased rightvertexindex, and now they point to the same vertex
                        // This means that this is the bottom point of the polygon. We'll remove it:
                        activepolygons.RemoveAt(activepolygonindex);
                        --activepolygonindex;
                    }
                    else
                    {
                        activepolygon.leftvertexindex = newleftvertexindex;
                        activepolygon.rightvertexindex = newrightvertexindex;
                        activepolygon.topleft = vertices2d[newleftvertexindex];
                        activepolygon.topright = vertices2d[newrightvertexindex];
                        var _nextleftvertexindex = newleftvertexindex + 1;
                        if (_nextleftvertexindex >= numvertices) _nextleftvertexindex = 0;
                        activepolygon.bottomleft = vertices2d[_nextleftvertexindex];
                        var _nextrightvertexindex = newrightvertexindex - 1;
                        if (_nextrightvertexindex < 0) _nextrightvertexindex = numvertices - 1;
                        activepolygon.bottomright = vertices2d[_nextrightvertexindex];
                    }
                } // if polygon has corner here
            } // for activepolygonindex
            double nextycoordinate = double.NaN;
            if (yindex >= ycoordinates.Count - 1)
            {
                // last row, all polygons must be finished here:
                activepolygons = new List<ActivePolygon>();
                nextycoordinate = double.NaN;
            }
            else
            { // yindex < ycoordinates.length-1
                nextycoordinate = ycoordinates[yindex + 1];
                var middleycoordinate = 0.5 * (ycoordinate + nextycoordinate);
                // update activepolygons by adding any polygons that start here:
                var startingpolygonindexes = topy2polygonindexes.ContainsKey(ycoordinate) ? topy2polygonindexes[ycoordinate] : new List<int>(0);
                foreach (var polygonindex in startingpolygonindexes)
                {
                    //var polygonindex = startingpolygonindexes[polygonindexKey]; Javascript Map foreach works oddly.
                    var vertices2d = polygonvertices2d[polygonindex];
                    var numvertices = vertices2d.Count;
                    var topvertexindex = polygontopvertexindexes[polygonindex];
                    // the top of the polygon may be a horizontal line. In that case topvertexindex can point to any point on this line.
                    // Find the left and right topmost vertices which have the current y coordinate:
                    var topleftvertexindex = topvertexindex;
                    while (true)
                    {
                        var i = topleftvertexindex + 1;
                        if (i >= numvertices) i = 0;
                        if (vertices2d[i].y != ycoordinate) break;
                        if (i == topvertexindex) break; // should not happen, but just to prevent endless loops
                        topleftvertexindex = i;
                    }
                    var toprightvertexindex = topvertexindex;
                    while (true)
                    {
                        var i = toprightvertexindex - 1;
                        if (i < 0) i = numvertices - 1;
                        if (vertices2d[i].y != ycoordinate) break;
                        if (i == topleftvertexindex) break; // should not happen, but just to prevent endless loops
                        toprightvertexindex = i;
                    }
                    var nextleftvertexindex = topleftvertexindex + 1;
                    if (nextleftvertexindex >= numvertices) nextleftvertexindex = 0;
                    var nextrightvertexindex = toprightvertexindex - 1;
                    if (nextrightvertexindex < 0) nextrightvertexindex = numvertices - 1;
                    var newactivepolygon = new ActivePolygon();
                    newactivepolygon.polygonindex = polygonindex;
                    newactivepolygon.leftvertexindex = topleftvertexindex;
                    newactivepolygon.rightvertexindex = toprightvertexindex;
                    newactivepolygon.topleft = vertices2d[topleftvertexindex];
                    newactivepolygon.topright = vertices2d[toprightvertexindex];
                    newactivepolygon.bottomleft = vertices2d[nextleftvertexindex];
                    newactivepolygon.bottomright = vertices2d[nextrightvertexindex];
                    insertSorted(activepolygons, newactivepolygon, (ActivePolygon el1, ActivePolygon el2) =>
                    {
                        var x1 = InterpolateBetween2DPointsForY(el1.topleft, el1.bottomleft, middleycoordinate);
                        var x2 = InterpolateBetween2DPointsForY(el2.topleft, el2.bottomleft, middleycoordinate);
                        if (x1 > x2) return 1;
                        if (x1 < x2) return -1;
                        return 0;
                    });
                } // for(var polygonindex in startingpolygonindexes)
            } //  yindex < ycoordinates.length-1
              // if( (yindex == ycoordinates.length-1) || (nextycoordinate - ycoordinate > EPS) )
              // FIXME : what ???

            // Now activepolygons is up to date
            // Build the output polygons for the next row in newoutpolygonrow:
            foreach (var activepolygon in activepolygons)
            {
                var x = InterpolateBetween2DPointsForY(activepolygon.topleft, activepolygon.bottomleft, ycoordinate);
                var topleft = new Vec2(x, ycoordinate);
                x = InterpolateBetween2DPointsForY(activepolygon.topright, activepolygon.bottomright, ycoordinate);
                var topright = new Vec2(x, ycoordinate);
                x = InterpolateBetween2DPointsForY(activepolygon.topleft, activepolygon.bottomleft, nextycoordinate);
                var bottomleft = new Vec2(x, nextycoordinate);
                x = InterpolateBetween2DPointsForY(activepolygon.topright, activepolygon.bottomright, nextycoordinate);
                var bottomright = new Vec2(x, nextycoordinate);
                var outpolygon = new Outpolygon();
                outpolygon.topleft = topleft;
                outpolygon.topright = topright;
                outpolygon.bottomleft = bottomleft;
                outpolygon.bottomright = bottomright;
                outpolygon.leftline = (topleft, bottomleft);
                outpolygon.rightline = (bottomright, topright);
                if (newoutpolygonrow.Count > 0)
                {
                    var prevoutpolygon = newoutpolygonrow[newoutpolygonrow.Count - 1];
                    var d1 = outpolygon.topleft.Distance(prevoutpolygon.topright);
                    var d2 = outpolygon.bottomleft.Distance(prevoutpolygon.bottomright);
                    if ((d1 < C.EPS) && (d2 < C.EPS))
                    {
                        // we can join this polygon with the one to the left:
                        outpolygon.topleft = prevoutpolygon.topleft;
                        outpolygon.leftline = prevoutpolygon.leftline;
                        outpolygon.bottomleft = prevoutpolygon.bottomleft;
                        newoutpolygonrow.RemoveAt(newoutpolygonrow.Count - 1);
                    }
                }
                newoutpolygonrow.Add(outpolygon);
            } // for(activepolygon in activepolygons)
            if (yindex > 0)
            {
                // try to match the new polygons against the previous row:
                var prevcontinuedindexes = new Dictionary<int, bool>();
                var matchedindexes = new Dictionary<int, bool>();
                for (var i = 0; i < newoutpolygonrow.Count; i++)
                {
                    var thispolygon = newoutpolygonrow[i];
                    for (var ii = 0; ii < prevoutpolygonrow.Count; ii++)
                    {
                        if (!matchedindexes.ContainsKey(ii))
                        { // not already processed?
                          // We have a match if the sidelines are equal or if the top coordinates
                          // are on the sidelines of the previous polygon
                            var prevpolygon = prevoutpolygonrow[ii];
                            if (prevpolygon.bottomleft.Distance(thispolygon.topleft) < C.EPS)
                            {
                                if (prevpolygon.bottomright.Distance(thispolygon.topright) < C.EPS)
                                {
                                    // Yes, the top of this polygon matches the bottom of the previous:
                                    matchedindexes[ii] = true;
                                    // Now check if the joined polygon would remain convex:
                                    var v1 = line2_direction(thispolygon.leftline);
                                    var v2 = line2_direction(prevpolygon.leftline);
                                    var d1 = v1.x - v2.x;

                                    var v3 = line2_direction(thispolygon.rightline);
                                    var v4 = line2_direction(prevpolygon.rightline);
                                    var d2 = v3.x - v4.x;

                                    var leftlinecontinues = Math.Abs(d1) < C.EPS;
                                    var rightlinecontinues = Math.Abs(d2) < C.EPS;
                                    var leftlineisconvex = leftlinecontinues || (d1 >= 0);
                                    var rightlineisconvex = rightlinecontinues || (d2 >= 0);
                                    if (leftlineisconvex && rightlineisconvex)
                                    {
                                        // yes, both sides have convex corners:
                                        // This polygon will continue the previous polygon
                                        thispolygon.outpolygon = prevpolygon.outpolygon;
                                        thispolygon.leftlinecontinues = leftlinecontinues;
                                        thispolygon.rightlinecontinues = rightlinecontinues;
                                        prevcontinuedindexes[ii] = true;
                                    }
                                    break;
                                }
                            }
                        } // if(!prevcontinuedindexes[ii])
                    } // for ii
                } // for i
                for (var ii = 0; ii < prevoutpolygonrow.Count; ii++)
                {
                    if (!prevcontinuedindexes.ContainsKey(ii))
                    {
                        // polygon ends here
                        // Finish the polygon with the last point(s):
                        var prevpolygon = prevoutpolygonrow[ii];

#nullable disable // CBS C# translation note: I think I loath nullable checks sometimes!
                        prevpolygon.outpolygon.rightpoints.Add(prevpolygon.bottomright);
                        if (prevpolygon.bottomright.Distance(prevpolygon.bottomleft) > C.EPS)
                        {
                            // polygon ends with a horizontal line:
                            prevpolygon.outpolygon.leftpoints.Add(prevpolygon.bottomleft);
                        }
                        // reverse the left half so we get a counterclockwise circle:
                        prevpolygon.outpolygon.leftpoints.Reverse();
#nullable enable

                        var points2d = new List<Vec2>();
                        points2d.AddRange(prevpolygon.outpolygon.rightpoints);
                        points2d.AddRange(prevpolygon.outpolygon.leftpoints);
                        var vertices3d = new List<Vec3>();
                        foreach (var point2d in points2d)
                        {
                            vertices3d.Add(orthobasis.To3D(point2d));
                        }
                        var polygon = new Poly3(vertices3d, sourcepolygons[0].Color);

                        // if we let empty polygon out, next retesselate will crash
                        if (polygon.Vertices.Length > 0) destpolygons.Add(polygon);
                    }
                }
            } // if(yindex > 0)
            for (var i = 0; i < newoutpolygonrow.Count; i++)
            {
                var thispolygon = newoutpolygonrow[i];
                if (thispolygon.outpolygon is null)
                {
                    // polygon starts here:
                    thispolygon.outpolygon = new Outpolygon._outpolygon();

                    thispolygon.outpolygon.leftpoints.Add(thispolygon.topleft);
                    if (thispolygon.topleft.Distance(thispolygon.topright) > C.EPS)
                    {
                        // we have a horizontal line at the top:
                        thispolygon.outpolygon.rightpoints.Add(thispolygon.topright);
                    }
                }
                else
                {
                    // continuation of a previous row
                    if (!thispolygon.leftlinecontinues)
                    {
                        thispolygon.outpolygon.leftpoints.Add(thispolygon.topleft);
                    }
                    if (!thispolygon.rightlinecontinues)
                    {
                        thispolygon.outpolygon.rightpoints.Add(thispolygon.topright);
                    }
                }
            }
            prevoutpolygonrow = newoutpolygonrow;
        } // for yindex
        return destpolygons;
    }

    delegate int CompareFunc(ActivePolygon el1, ActivePolygon el2);
    private static void insertSorted(List<ActivePolygon> array, ActivePolygon element, CompareFunc comparefunc)
    {
        var leftbound = 0;
        var rightbound = array.Count;
        while (rightbound > leftbound)
        {
            var testindex = (leftbound + rightbound) / 2; ;
            var testelement = array[testindex];
            var compareresult = comparefunc(element, testelement);
            if (compareresult > 0)
            { // element > testelement
                leftbound = testindex + 1;
            }
            else
            {
                rightbound = testindex;
            }
        }
        array.Insert(leftbound, element);
    }


    /**
     * Get the X coordinate of a point with a certain Y coordinate, interpolated between two points.
     * Interpolation is robust even if the points have the same Y coordinate
     * @param {vec2} point1
     * @param {vec2} point2
     * @param {Number} y
     * @return {Array} X and Y of interpolated point
     * @alias module:modeling/maths/utils.interpolateBetween2DPointsForY
     */
    private static double InterpolateBetween2DPointsForY(Vec2 point1, Vec2 point2, double y)
    {
        var f1 = y - point1.y;
        var f2 = point2.y - point1.y;
        if (f2 < 0)
        {
            f1 = -f1;
            f2 = -f2;
        }
        double t;
        if (f1 <= 0)
        {
            t = 0.0;
        }
        else if (f1 >= f2)
        {
            t = 1.0;
        }
        else if (f2 < 1e-10)
        { // FIXME Should this be EPS?
            t = 0.5;
        }
        else
        {
            t = f1 / f2;
        }
        return point1.x + t * (point2.x - point1.x);
    }

    private static Vec2 line2_direction((Vec2, Vec2) line)
    {
        // Create a line2 from two points
        var (point1, point2) = line;
        var vector = point2.Subtract(point1); // directional vector
        vector = vector.Normal().Normalize();
        var distance = point1.Dot(vector);
        // Now do direction.
        var dvector = vector.Normal();
        dvector = dvector.Negate();
        return dvector;
    }

}