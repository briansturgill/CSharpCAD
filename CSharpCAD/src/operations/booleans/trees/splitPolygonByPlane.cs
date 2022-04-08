namespace CSharpCAD;

public static partial class CSCAD
{
    // Returns object:
    // .type:
    //   0: coplanar-front
    //   1: coplanar-back
    //   2: front
    //   3: back
    //   4: spanning
    // In case the polygon is spanning, returns:
    // .front: a Polygon3 of the front part
    // .back: a Polygon3 of the back part
    private static (int, Poly3?, Poly3?) SplitPolygonByPlane(Plane splane, Poly3 polygon)
    {
        int result_type = -1;
        Poly3? result_front = null;
        Poly3? result_back = null;

        // cache in local lets (speedup):
        var vertices = polygon.Vertices;
        var numvertices = vertices.Length;
        var pplane = polygon.Plane();
        if (pplane == splane)
        {
            result_type = 0;
        }
        else
        {
            var hasfront = false;
            var hasback = false;
            var vertexIsBack = new List<bool>();
            var MINEPS = -C.EPS;
            for (var i = 0; i < numvertices; i++)
            {
                var t = splane.normal.Dot(vertices[i]) - splane.w;
                var isback = (t < MINEPS);
                vertexIsBack.Add(isback);
                if (t > C.EPS) hasfront = true;
                if (t < MINEPS) hasback = true;
            }
            if ((!hasfront) && (!hasback))
            {
                // all points coplanar
                var t = splane.normal.Dot(pplane.normal);
                result_type = (t >= 0) ? 0 : 1;
            }
            else if (!hasback)
            {
                result_type = 2;
            }
            else if (!hasfront)
            {
                result_type = 3;
            }
            else
            {
                // spanning
                result_type = 4;
                var frontvertices = new List<Vec3>();
                var backvertices = new List<Vec3>();
                var isback = vertexIsBack[0];
                for (var vertexindex = 0; vertexindex < numvertices; vertexindex++)
                {
                    var vertex = vertices[vertexindex];
                    var nextvertexindex = vertexindex + 1;
                    if (nextvertexindex >= numvertices) nextvertexindex = 0;
                    var nextisback = vertexIsBack[nextvertexindex];
                    if (isback == nextisback)
                    {
                        // line segment is on one side of the plane:
                        if (isback)
                        {
                            backvertices.Add(vertex);
                        }
                        else
                        {
                            frontvertices.Add(vertex);
                        }
                    }
                    else
                    {
                        // line segment intersects plane:
                        var nextpoint = vertices[nextvertexindex];
                        var intersectionpoint = SplitLineSegmentByPlane(splane, vertex, nextpoint);
                        if (isback)
                        {
                            backvertices.Add(vertex);
                            backvertices.Add(intersectionpoint);
                            frontvertices.Add(intersectionpoint);
                        }
                        else
                        {
                            frontvertices.Add(vertex);
                            frontvertices.Add(intersectionpoint);
                            backvertices.Add(intersectionpoint);
                        }
                    }
                    isback = nextisback;
                } // for vertexindex
                  // remove duplicate vertices:
                var EPS_SQUARED = C.EPS * C.EPS;
                if (backvertices.Count >= 3)
                {
                    var prevvertex = backvertices[backvertices.Count - 1];
                    for (var vertexindex = 0; vertexindex < backvertices.Count; vertexindex++)
                    {
                        var vertex = backvertices[vertexindex];
                        if (vertex.SquaredDistance(prevvertex) < EPS_SQUARED)
                        {
                            backvertices.RemoveAt(vertexindex);
                            vertexindex--;
                        }
                        prevvertex = vertex;
                    }
                }
                if (frontvertices.Count >= 3)
                {
                    var prevvertex = frontvertices[frontvertices.Count - 1];
                    for (var vertexindex = 0; vertexindex < frontvertices.Count; vertexindex++)
                    {
                        var vertex = frontvertices[vertexindex];
                        if (vertex.SquaredDistance(prevvertex) < EPS_SQUARED)
                        {
                            frontvertices.RemoveAt(vertexindex);
                            vertexindex--;
                        }
                        prevvertex = vertex;
                    }
                }
                if (frontvertices.Count >= 3)
                {
                    result_front = new Poly3(frontvertices, null);
                }
                if (backvertices.Count >= 3)
                {
                    result_back = new Poly3(backvertices, null);
                }
            }
        }
        return (result_type, result_front, result_back);
    }
}
