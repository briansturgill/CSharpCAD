namespace CSharpCAD;

internal static partial class Modifiers
{
    /*
     * All shapes (primitives or the results of operations) can be modified to correct issues, etc.
     * In all cases, these functions returns the results, and never changes the original geometry.
     * @module modeling/modifiers
     * @example
     * var { snap } = require('@jscad/modeling').modifiers
     */
    private static string getTag(Vec3 vertex) => $"{vertex}";

    public class Side
    {
        public readonly Vec3 vertex0;
        public readonly Vec3 vertex1;
        public readonly int polygonindex;
        public Side(Vec3 vertex0, Vec3 vertex1, int polygonindex)
        {
            this.vertex0 = vertex0;
            this.vertex1 = vertex1;
            this.polygonindex = polygonindex;
        }
    }

    private static string? addSide(Dictionary<string, List<Side>> sidemap, Dictionary<string, List<string>> vertextag2sidestart,
      Dictionary<string, List<string>> vertextag2sideend, Vec3 vertex0, Vec3 vertex1, int polygonindex)
    {
        var starttag = getTag(vertex0);
        var endtag = getTag(vertex1);
        //Debug.Assert(starttag == endtag);
        var newsidetag = $"{starttag}/{endtag}";
        var reversesidetag = $"{endtag}/{starttag}";
        if (sidemap.ContainsKey(reversesidetag))
        {
            // remove the opposing side from mappings
            deleteSide(sidemap, vertextag2sidestart, vertextag2sideend, vertex1, vertex0, null);
            return null;
        }
        // add the side to the mappings
        var newsideobj = new Side(vertex0, vertex1, polygonindex);
        if (!(sidemap.ContainsKey(newsidetag)))
        {
            sidemap[newsidetag] = new List<Side> { newsideobj };
        }
        else
        {
            sidemap[newsidetag].Add(newsideobj);
        }
        if (vertextag2sidestart.ContainsKey(starttag))
        {
            vertextag2sidestart[starttag].Add(newsidetag);
        }
        else
        {
            vertextag2sidestart[starttag] = new List<string> { newsidetag };
        }
        if (vertextag2sideend.ContainsKey(endtag))
        {
            vertextag2sideend[endtag].Add(newsidetag);
        }
        else
        {
            vertextag2sideend[endtag] = new List<string> { newsidetag };
        }
        return newsidetag;
    }

    public static void deleteSide(Dictionary<string, List<Side>> sidemap, Dictionary<string, List<string>> vertextag2sidestart,
      Dictionary<string, List<string>> vertextag2sideend, Vec3 vertex0, Vec3 vertex1, int? polygonindex)
    {
        var starttag = getTag(vertex0);

        var endtag = getTag(vertex1);

        var sidetag = $"{starttag}/{endtag}";
        //Debug.Assert(!sidemap.ContainsKey(sidetag));
        var idx = -1;

        var sideobjs = sidemap[sidetag];

        for (var i = 0; i < sideobjs.Count; i++)
        {
            var sideobj = sideobjs[i];
            var sidetag_ = getTag(sideobj.vertex0);

            if (sidetag_ != starttag) continue;

            sidetag_ = getTag(sideobj.vertex1);

            if (sidetag_ != endtag) continue;


            if (polygonindex is not null)
            {
                if (sideobj.polygonindex != polygonindex) continue;
            }
            idx = i;
            break;
        }
        //Debug.Assert(idx < 0);

        sideobjs.RemoveAt(sideobjs.Count - 1);

        if (sideobjs.Count == 0)
        {
            sidemap.Remove(sidetag);
        }

        // adjust start and end lists
        vertextag2sidestart[starttag].Remove(sidetag);
        if (vertextag2sidestart[starttag].Count == 0)
        {
            vertextag2sidestart.Remove(starttag);
        }

        vertextag2sideend[endtag].Remove(sidetag);
        if (vertextag2sideend[endtag].Count == 0)
        {
            vertextag2sideend.Remove(endtag);
        }
    }

    /*
      Suppose we have two polygons ACDB and EDGF:

       A-----B
       |     |
       |     E--F
       |     |  |
       C-----D--G

      Note that vertex E forms a T-junction on the side BD. In this case some STL slicers will complain
      that the solid is not watertight. This is because the watertightness check is done by checking if
      each side DE is matched by another side ED.

      This function will return a new solid with ACDB replaced by ACDEB

      Note that this can create polygons that are slightly non-convex (due to rounding errors). Therefore the result should
      not be used for further CSG operations!

      Note this function is meant to be used to preprocess geometries when triangulation is required, i.e. AMF, STL, etc.
      Do not use the results in other operations.
    */

    /*
     * Insert missing vertices for T junctions, which creates polygons that can be triangulated.
     * @param {Array} polygons - the original polygons which may or may not have T junctions
     * @return original polygons (if no T junctions found) or new polygons with updated vertices
     */
    public static Poly3[] insertTjunctions(Poly3[] polygons)
    {
        // STEP 1 : build a map of 'unmatched' sides from the polygons
        // i.e. side AB in one polygon does not have a matching side BA in another polygon
        var sidemap = new Dictionary<string, List<Side>>();
        for (var polygonindex = 0; polygonindex < polygons.Length; polygonindex++)
        {
            var polygon = polygons[polygonindex];
            var numvertices = polygon.Vertices.Length;
            if (numvertices >= 3)
            {
                var vertex = polygon.Vertices[0];
                var vertextag = getTag(vertex);
                for (var vertexindex = 0; vertexindex < numvertices; vertexindex++)
                {
                    var nextvertexindex = vertexindex + 1;
                    if (nextvertexindex == numvertices) nextvertexindex = 0;

                    var nextvertex = polygon.Vertices[nextvertexindex];
                    var nextvertextag = getTag(nextvertex);

                    var sidetag = $"{vertextag}/{nextvertextag}";
                    var reversesidetag = $"{nextvertextag}/{vertextag}";
                    if (sidemap.ContainsKey(reversesidetag))
                    {
                        // this side matches the same side in another polygon. Remove from sidemap
                        // FIXME is this check necessary? there should only be ONE(1) opposing side
                        // FIXME assert ?
                        var ar = sidemap[reversesidetag];
                        ar.RemoveAt(ar.Count - 1);
                        if (ar.Count == 0)
                        {
                            sidemap.Remove(reversesidetag);
                        }
                    }
                    else
                    {
                        var sideobj = new Side(vertex, nextvertex, polygonindex);
                        if (!(sidemap.ContainsKey(sidetag)))
                        {
                            sidemap[sidetag] = new List<Side> { sideobj };
                        }
                        else
                        {
                            sidemap[sidetag].Add(sideobj);
                        }
                    }
                    vertex = nextvertex;
                    vertextag = nextvertextag;
                }
            }
            else
            {
                Debug.Fail("Warning: invalid polygon found during insertTjunctions");
            }
        }

        if (sidemap.Count > 0)
        {
            // console.log('insertTjunctions',sidemap.size)
            // STEP 2 : create a list of starting sides and ending sides
            var vertextag2sidestart = new Dictionary<string, List<string>>();
            var vertextag2sideend = new Dictionary<string, List<string>>();
            var sidestocheck = new HashSet<string>();
            foreach (var (sidetag, sideobjs) in sidemap)
            {
                sidestocheck.Add(sidetag);
                foreach (var sideobj in sideobjs)
                {
                    var starttag = getTag(sideobj.vertex0);

                    var endtag = getTag(sideobj.vertex1);

                    if (vertextag2sidestart.ContainsKey(starttag))
                    {
                        vertextag2sidestart[starttag].Add(sidetag);
                    }
                    else
                    {
                        vertextag2sidestart[starttag] = new List<string> { sidetag };
                    }
                    if (vertextag2sideend.ContainsKey(endtag))
                    {
                        vertextag2sideend[endtag].Add(sidetag);
                    }
                    else
                    {
                        vertextag2sideend[endtag] = new List<string> { sidetag };
                    }
                }
            }

            // STEP 3 : if sidemap is not empty
            var newpolygons = polygons.ToArray(); // make a copy in order to replace polygons inline
            while (true)
            {
                if (sidemap.Count == 0) break;

                foreach (var sidetag in sidemap.Keys)
                {
                    sidestocheck.Add(sidetag);
                }

                var donesomething = false;
                while (true)
                {
                    var sidetags = sidestocheck.ToArray();
                    if (sidetags.Length == 0) break; // sidestocheck is empty, we're done!
                    var sidetagtocheck = sidetags[0];
                    var donewithside = true;
                    if (sidemap.ContainsKey(sidetagtocheck))
                    {
                        var sideobjs = sidemap[sidetagtocheck];
                        //Debug.Assert(sideobjs is null || sideobjs.Count == 0);
                        var sideobj = sideobjs[0];
                        for (var directionindex = 0; directionindex < 2; directionindex++)
                        {
                            var startvertex = (directionindex == 0) ? sideobj.vertex0 : sideobj.vertex1;
                            var endvertex = (directionindex == 0) ? sideobj.vertex1 : sideobj.vertex0;
                            var startvertextag = getTag(startvertex);
                            var endvertextag = getTag(endvertex);
                            var matchingsides = new List<string>();
                            if (directionindex == 0)
                            {
                                if (vertextag2sideend.ContainsKey(startvertextag))
                                {
                                    matchingsides = vertextag2sideend[startvertextag];
                                }
                            }
                            else
                            {
                                if (vertextag2sidestart.ContainsKey(startvertextag))
                                {
                                    matchingsides = vertextag2sidestart[startvertextag];
                                }
                            }
                            for (var matchingsideindex = 0; matchingsideindex < matchingsides.Count; matchingsideindex++)
                            {
                                var matchingsidetag = matchingsides[matchingsideindex];
                                var matchingside = sidemap[matchingsidetag][0];
                                var matchingsidestartvertex = (directionindex == 0) ? matchingside.vertex0 : matchingside.vertex1;
                                var matchingsideendvertex = (directionindex == 0) ? matchingside.vertex1 : matchingside.vertex0;
                                var matchingsidestartvertextag = getTag(matchingsidestartvertex);
                                var matchingsideendvertextag = getTag(matchingsideendvertex);
                                //Debug.Assert(matchingsideendvertextag != startvertextag);
                                if (matchingsidestartvertextag == endvertextag)
                                {
                                    // matchingside cancels sidetagtocheck
                                    deleteSide(sidemap, vertextag2sidestart, vertextag2sideend, startvertex, endvertex, null);
                                    deleteSide(sidemap, vertextag2sidestart, vertextag2sideend, endvertex, startvertex, null);
                                    donewithside = false;
                                    directionindex = 2; // skip reverse direction check
                                    donesomething = true;
                                    break;
                                }
                                else
                                {
                                    var startpos = startvertex;
                                    var endpos = endvertex;
                                    var checkpos = matchingsidestartvertex;
                                    var direction = checkpos.Subtract(startpos);
                                    // Now we need to check if endpos is on the line startpos-checkpos:
                                    var t = endpos.Subtract(startpos).Dot(direction) / direction.Dot(direction);
                                    if ((t > 0) && (t < 1))
                                    {
                                        var closestpoint = direction.Scale(t);
                                        closestpoint = closestpoint.Add(startpos);
                                        var distancesquared = closestpoint.SquaredDistance(endpos);
                                        if (distancesquared < (C.EPS * C.EPS))
                                        {
                                            // Yes it's a t-junction! We need to split matchingside in two:
                                            var polygonindex = matchingside.polygonindex;
                                            var polygon = newpolygons[polygonindex];
                                            // find the index of startvertextag in polygon:
                                            var insertionvertextag = getTag(matchingside.vertex1);
                                            var insertionvertextagindex = -1;
                                            for (var i = 0; i < polygon.Vertices.Length; i++)
                                            {
                                                if (getTag(polygon.Vertices[i]) == insertionvertextag)
                                                {
                                                    insertionvertextagindex = i;
                                                    break;
                                                }
                                            }
                                            //Debug.Assert(insertionvertextagindex < 0);
                                            // split the side by inserting the vertex:
                                            var newvertices = new List<Vec3>(polygon.Vertices.Length+1);
                                            newvertices.AddRange(polygon.Vertices.ToArray());
                                            newvertices.Insert(insertionvertextagindex, endvertex);
                                            var newpolygon = new Poly3(newvertices.ToArray(), polygon.Color);

                                            newpolygons[polygonindex] = newpolygon;

                                            // remove the original sides from our maps
                                            deleteSide(sidemap, vertextag2sidestart, vertextag2sideend, matchingside.vertex0, matchingside.vertex1, polygonindex);
                                            var newsidetag1 = addSide(sidemap, vertextag2sidestart, vertextag2sideend, matchingside.vertex0, endvertex, polygonindex);
                                            var newsidetag2 = addSide(sidemap, vertextag2sidestart, vertextag2sideend, endvertex, matchingside.vertex1, polygonindex);
                                            if (newsidetag1 is not null) sidestocheck.Add(newsidetag1);
                                            if (newsidetag2 is not null) sidestocheck.Add(newsidetag2);
                                            donewithside = false;
                                            directionindex = 2; // skip reverse direction check
                                            donesomething = true;
                                            break;
                                        } // if(distancesquared < 1e-10)
                                    } // if( (t > 0) && (t < 1) )
                                } // if(endingstidestartvertextag == endvertextag)
                            } // for matchingsideindex
                        } // for directionindex
                    } // if(sidetagtocheck in sidemap)
                    if (donewithside)
                    {
                        sidestocheck.Remove(sidetagtocheck);
                    }
                }
                if (!donesomething) break;
            }
            polygons = newpolygons;
        }
        sidemap.Clear();

        return polygons;
    }
}
