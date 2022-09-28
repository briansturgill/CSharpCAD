namespace CSharpCAD;

internal static partial class Modifiers
{
    private class Side
    {
        public readonly Vec3 vertex0;
        public readonly Vec3 vertex1;
        public readonly int polygonIndex;
        public Side(Vec3 vertex0, Vec3 vertex1, int polygonIndex)
        {
            this.vertex0 = vertex0;
            this.vertex1 = vertex1;
            this.polygonIndex = polygonIndex;
        }
    }

    private static (Vec3, Vec3)? addSide(Dictionary<(Vec3, Vec3), List<Side>> sideMap, Dictionary<Vec3, List<(Vec3, Vec3)>> vertexToSideStart,
      Dictionary<Vec3, List<(Vec3, Vec3)>> vertexToSideEnd, Vec3 start, Vec3 end, int polygonIndex)
    {
        Debug.Assert(start != end);
        var newSide = (start, end);
        var reverseSide = (end, start);
        if (sideMap.ContainsKey(reverseSide))
        {
            // remove the opposing side from mappings
            deleteSide(sideMap, vertexToSideStart, vertexToSideEnd, end, start, null);
            return null;
        }
        // add the side to the mappings
        var newSideObj = new Side(start, end, polygonIndex);
        if (!(sideMap.ContainsKey(newSide)))
        {
            sideMap[newSide] = new List<Side> { newSideObj };
        }
        else
        {
            sideMap[newSide].Add(newSideObj);
        }
        if (vertexToSideStart.ContainsKey(start))
        {
            vertexToSideStart[start].Add(newSide);
        }
        else
        {
            vertexToSideStart[start] = new List<(Vec3, Vec3)> { newSide };
        }
        if (vertexToSideEnd.ContainsKey(end))
        {
            vertexToSideEnd[end].Add(newSide);
        }
        else
        {
            vertexToSideEnd[end] = new List<(Vec3, Vec3)> { newSide };
        }
        return newSide;
    }

    private static void deleteSide(Dictionary<(Vec3, Vec3), List<Side>> sideMap, Dictionary<Vec3, List<(Vec3, Vec3)>> vertexToSideStart,
      Dictionary<Vec3, List<(Vec3, Vec3)>> vertexToSideEnd, Vec3 start, Vec3 end, int? polygonIndex)
    {
        var side = (start, end);
        Debug.Assert(sideMap.ContainsKey(side));
        var idx = -1;

        var sideObjs = sideMap[side];

        for (var i = 0; i < sideObjs.Count; i++)
        {
            var sideObj = sideObjs[i];
            var side_ = sideObj.vertex0;

            if (side_ != start) continue;

            side_ = sideObj.vertex1;

            if (side_ != end) continue;


            if (polygonIndex is not null)
            {
                if (sideObj.polygonIndex != polygonIndex) continue;
            }
            idx = i;
            break;
        }
        Debug.Assert(idx >= 0);

        sideObjs.RemoveAt(sideObjs.Count - 1);

        if (sideObjs.Count == 0)
        {
            sideMap.Remove(side);
        }

        // adjust start and end lists
        vertexToSideStart[start].Remove(side);
        if (vertexToSideStart[start].Count == 0)
        {
            vertexToSideStart.Remove(start);
        }

        vertexToSideEnd[end].Remove(side);
        if (vertexToSideEnd[end].Count == 0)
        {
            vertexToSideEnd.Remove(end);
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
    public static Poly3[] InsertTjunctions(Poly3[] polygons)
    {
        // STEP 1 : build a map of 'unmatched' sides from the polygons
        // i.e. side AB in one polygon does not have a matching side BA in another polygon
        var sideMap = new Dictionary<(Vec3, Vec3), List<Side>>();
        for (var polygonIndex = 0; polygonIndex < polygons.Length; polygonIndex++)
        {
            var polygon = polygons[polygonIndex];
            var numVertices = polygon.Vertices.Length;
            if (numVertices >= 3)
            {
                var vertex = polygon.Vertices[0];
                for (var vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
                {
                    var nextVertexIndex = vertexIndex + 1;
                    if (nextVertexIndex == numVertices) nextVertexIndex = 0;

                    var nextVertex = polygon.Vertices[nextVertexIndex];

                    var side = (vertex, nextVertex);
                    var reverseSide = (nextVertex, vertex);
                    if (sideMap.ContainsKey(reverseSide))
                    {
                        // this side matches the same side in another polygon. Remove from sideMap
                        // FIXME is this check necessary? there should only be ONE(1) opposing side
                        // FIXME assert ?
                        var ar = sideMap[reverseSide];
                        ar.RemoveAt(ar.Count - 1);
                        if (ar.Count == 0)
                        {
                            sideMap.Remove(reverseSide);
                        }
                        else
                        {
                            Debug.Fail("Should only be one reverseSide.");
                        }
                    }
                    else
                    {
                        var sideObj = new Side(vertex, nextVertex, polygonIndex);
                        if (!(sideMap.ContainsKey(side)))
                        {
                            sideMap[side] = new List<Side> { sideObj };
                        }
                        else
                        {
                            sideMap[side].Add(sideObj);
                        }
                    }
                    vertex = nextVertex;
                }
            }
            else
            {
                Debug.Fail("Warning: invalid polygon found during insertTjunctions");
            }
        }

        if (sideMap.Count > 0)
        {
            // STEP 2 : create a list of starting sides and ending sides
            var vertexToSideStart = new Dictionary<Vec3, List<(Vec3, Vec3)>>();
            var vertexToSideEnd = new Dictionary<Vec3, List<(Vec3, Vec3)>>();
            var sidesToCheck = new HashSet<(Vec3, Vec3)>();
            foreach (var (side, sideObjs) in sideMap)
            {
                sidesToCheck.Add(side);
                foreach (var sideObj in sideObjs)
                {
                    var start = sideObj.vertex0;

                    var end = sideObj.vertex1;

                    if (vertexToSideStart.ContainsKey(start))
                    {
                        vertexToSideStart[start].Add(side);
                    }
                    else
                    {
                        vertexToSideStart[start] = new List<(Vec3, Vec3)> { side };
                    }
                    if (vertexToSideEnd.ContainsKey(end))
                    {
                        vertexToSideEnd[end].Add(side);
                    }
                    else
                    {
                        vertexToSideEnd[end] = new List<(Vec3, Vec3)> { side };
                    }
                }
            }

            // STEP 3 : if sideMap is not empty
            var newPolygons = polygons.ToArray(); // make a copy in order to replace polygons inline
            while (true)
            {
                if (sideMap.Count == 0) break;

                foreach (var side in sideMap.Keys)
                {
                    sidesToCheck.Add(side);
                }

                var doneSomething = false;
                while (true)
                {
                    var sides = sidesToCheck.ToArray();
                    if (sides.Length == 0) break; // sidesToCheck is empty, we're done!
                    var sideToCheck = sides[0];
                    var doneWithSide = true;
                    if (sideMap.ContainsKey(sideToCheck))
                    {
                        var sideObjs = sideMap[sideToCheck];
                        Debug.Assert(sideObjs is not null && sideObjs.Count != 0);
                        var sideObj = sideObjs[0];
                        for (var directionIndex = 0; directionIndex < 2; directionIndex++)
                        {
                            var startVertex = (directionIndex == 0) ? sideObj.vertex0 : sideObj.vertex1;
                            var endVertex = (directionIndex == 0) ? sideObj.vertex1 : sideObj.vertex0;
                            var matchingSides = new List<(Vec3, Vec3)>();
                            if (directionIndex == 0)
                            {
                                if (vertexToSideEnd.ContainsKey(startVertex))
                                {
                                    matchingSides = vertexToSideEnd[startVertex];
                                }
                            }
                            else
                            {
                                if (vertexToSideStart.ContainsKey(startVertex))
                                {
                                    matchingSides = vertexToSideStart[startVertex];
                                }
                            }
                            for (var matchingSideIndex = 0; matchingSideIndex < matchingSides.Count; matchingSideIndex++)
                            {
                                var matchingSide = sideMap[matchingSides[matchingSideIndex]][0];
                                var matchingSideStartVertex = (directionIndex == 0) ? matchingSide.vertex0 : matchingSide.vertex1;
                                var matchingSideEndVertex = (directionIndex == 0) ? matchingSide.vertex1 : matchingSide.vertex0;
                                Debug.Assert(matchingSideEndVertex == startVertex);
                                if (matchingSideStartVertex == endVertex)
                                {
                                    // matchingSide cancels sideToCheck
                                    deleteSide(sideMap, vertexToSideStart, vertexToSideEnd, startVertex, endVertex, null);
                                    deleteSide(sideMap, vertexToSideStart, vertexToSideEnd, endVertex, startVertex, null);
                                    doneWithSide = false;
                                    directionIndex = 2; // skip reverse direction check
                                    doneSomething = true;
                                    break;
                                }
                                else
                                {
                                    var startpos = startVertex;
                                    var endpos = endVertex;
                                    var checkpos = matchingSideStartVertex;
                                    var direction = checkpos.Subtract(startpos);
                                    // Now we need to check if endpos is on the line startpos-checkpos:
                                    var t = endpos.Subtract(startpos).Dot(direction) / direction.Dot(direction);
                                    if ((t > 0) && (t < 1))
                                    {
                                        var closestPoint = direction.Scale(t);
                                        closestPoint = closestPoint.Add(startpos);
                                        var distancesquared = closestPoint.SquaredDistance(endpos);
                                        if (distancesquared < C.EPS * 0.1) // Was (C.EPS * C.EPS)
                                        {
                                            // Yes it's a t-junction! We need to split matchingSide in two:
                                            var polygonIndex = matchingSide.polygonIndex;
                                            var polygon = newPolygons[polygonIndex];
                                            // find the index of startVertextag in polygon:
                                            var insertionVertex = matchingSide.vertex1;
                                            var insertionVertexIndex = -1;
                                            for (var i = 0; i < polygon.Vertices.Length; i++)
                                            {
                                                if (polygon.Vertices[i] == insertionVertex)
                                                {
                                                    insertionVertexIndex = i;
                                                    break;
                                                }
                                            }
                                            Debug.Assert(insertionVertexIndex >= 0);
                                            // split the side by inserting the vertex:
                                            var newVertices = new List<Vec3>(polygon.Vertices.Length + 1);
                                            newVertices.AddRange(polygon.Vertices.ToArray());
                                            newVertices.Insert(insertionVertexIndex, endVertex);
                                            var newPolygon = new Poly3(newVertices.ToArray(), polygon.Color);

                                            newPolygons[polygonIndex] = newPolygon;

                                            // remove the original sides from our maps
                                            deleteSide(sideMap, vertexToSideStart, vertexToSideEnd, matchingSide.vertex0, matchingSide.vertex1, polygonIndex);
                                            var newSide1 = addSide(sideMap, vertexToSideStart, vertexToSideEnd, matchingSide.vertex0, endVertex, polygonIndex);
                                            var newSide2 = addSide(sideMap, vertexToSideStart, vertexToSideEnd, endVertex, matchingSide.vertex1, polygonIndex);
                                            if (newSide1 is not null) sidesToCheck.Add(((Vec3, Vec3))newSide1);
                                            if (newSide2 is not null) sidesToCheck.Add(((Vec3, Vec3))newSide2);
                                            doneWithSide = false;
                                            directionIndex = 2; // skip reverse direction check
                                            doneSomething = true;
                                            break;
                                        } // if(distancesquared < 1e-10)
                                    } // if( (t > 0) && (t < 1) )
                                } // if(endingstidestartVertex == endVertex)
                            } // for matchingSideIndex
                        } // for directionIndex
                    } // if(sideToCheck in sideMap)
                    if (doneWithSide)
                    {
                        sidesToCheck.Remove(sideToCheck);
                    }
                }
                if (!doneSomething) break;
            }
            polygons = newPolygons;
        }
        sideMap.Clear();

        return polygons;
    }
}
