namespace CSharpCAD;

public static partial class Modifiers
{

    public class Edge
    {
        public Vec3 v1;
        public Vec3 v2;
        public Edge? next;
        public Edge? prev;
        public List<Poly3> polygons = new List<Poly3>();

        public Edge(Vec3 v1, Vec3 v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
        public double distance => this.v1.SquaredDistance(this.v2);
    };

    private static int findEdgeIndex(List<Edge> edges, Edge edge)
    {
        var ei = -1;
        var cnt = 0;
        foreach (var element in edges)
        {
            if (element.v1 == edge.v1 && element.v2 == edge.v2 ||
                element.v1 == edge.v2 && element.v2 == edge.v1)
            {
                ei = cnt;
                break;
            }
            cnt++;
        }
        return ei;
    }

    private static int findVertexIndex(Vec3[] vertices, Vec3 point)
    {
        var ei = -1;
        var cnt = 0;
        foreach (var vertex in vertices)
        {
            if (vertex.IsNearlyEqual(point))
            {
                ei = cnt;
                break;
            }
            cnt++;
        }
        return ei;
    }

    /*
     * Add a unique edge to the given list of edges.
     * Each edge has a list of associated polygons.
     * Edges with two polygons are complete, while edges with one polygon are open, i.e hole or t-junction..
     */
    public static void addEdge(List<Edge> edges, Edge edge, Poly3 polygon)
    {
        var ei = findEdgeIndex(edges, edge);
        if (ei >= 0)
        {
            edge = edges[ei];
            edge.polygons.Add(polygon);
        }
        else
        {
            edge.polygons.Add(polygon);
            edges.Add(edge);
        }
    }

    /*
     * Remove the edge from the given list of edges.
     */
    public static void removeEdge(List<Edge> edges, Edge edge)
    {
        edges.Remove(edge);
    }

    /*
     * Add all edges of the polygon to the given list of edges.
     */
    public static void addPolygon(List<Edge> edges, Poly3 polygon)
    {
        var vertices = polygon.Vertices;
        var nv = vertices.Length;

        var edge = new Edge(vertices[nv - 1], vertices[0]);
        addEdge(edges, edge, polygon);

        for (var i = 0; i < (nv - 1); i++)
        {
            edge = new Edge(vertices[i], vertices[i + 1]);
            addEdge(edges, edge, polygon);
        }
    }

    /*
     * Remove all polygons associated with the old edge from the given list of edges.
     */
    public static void removePolygons(List<Edge> edges, Edge oldedge)
    {
        // console.log('removePolygons',oldedge)
        var polygons = oldedge.polygons;
        foreach (var polygon in polygons)
        {
            var vertices = polygon.Vertices;
            var nv = vertices.Length;

            var edge = new Edge(vertices[nv - 1], vertices[0]);
            removeEdge(edges, edge);

            for (var i = 0; i < (nv - 1); i++)
            {
                edge = new Edge(vertices[i], vertices[i + 1]);
                removeEdge(edges, edge);
            }
        }
    }

    /*
     * Split the polygon, ensuring one polygon includes the open edge.
     */
    public static (Poly3, Poly3) splitPolygon(Edge openedge, Poly3 polygon, double eps)
    {
        // console.log('splitPolygon',openedge,polygon)
        var vertices = polygon.Vertices;
        var i = findVertexIndex(vertices, openedge.v1);
        var polygon1 = new Poly3(new Vec3[] { vertices[(i + 0) % 3], vertices[(i + 1) % 3], openedge.v2 });
        var polygon2 = new Poly3(new Vec3[] { openedge.v2, vertices[(i + 1) % 3], vertices[(i + 2) % 3] });
        if (polygon.Color is not null)
        {
            polygon1.Color = polygon.Color;
            polygon2.Color = polygon.Color;
        }
        // console.log('polygon1',polygon1)
        // console.log('polygon2',polygon2)
        return (polygon1, polygon2);
    }

    /// <summary>Determine the closest point on the given line to the given point.</summary>
    public static Vec3 closestPoint((Vec3, Vec3)line, Vec3 point)
    {
        var (lstart, lend) = line;
        var lpoint = lstart;
        var ldirection = lend.Subtract(lstart).Normalize();

        var a = point.Subtract(lpoint).Dot(ldirection);
        var b = ldirection.Dot(ldirection);
        var t = a / b;

        var closestpoint = ldirection.Scale(t);
        closestpoint = closestpoint.Add(lpoint);
        return closestpoint;
    }

    public static bool enclosedEdge(Edge openedge, Edge edge, double eps)
    {
        if (openedge.distance < edge.distance)
        {
            // only look for opposing edges
            if (openedge.v1 == edge.v2)
            {
                // only opposing open edges enclosed by the edge
                var distanceE0O0 = openedge.v1.SquaredDistance(edge.v1);
                var distanceE0O1 = openedge.v2.SquaredDistance(edge.v1);
                var distanceE1O0 = openedge.v1.SquaredDistance(edge.v2);
                var distanceE1O1 = openedge.v2.SquaredDistance(edge.v2);
                if (distanceE0O0 <= edge.distance && distanceE0O1 < edge.distance && distanceE1O0 < edge.distance && distanceE1O1 < edge.distance)
                {
                    /* CBS - Translation to C# NOTE
                        I think I found a bug here. They are using the seemingly defunct line3 class.
                        I pulled the logic out as I don't think I need the whole class.
                        Anyway, they have the order of arguments wrong. In both Line3 and Line2 clases the
                        "signature" is: closestPoint(line, point). They have them backwards.
                        Due to the vagaries of javascript (original authors are using nested points everywhere),
                        there was no error detected. One would think it would be caught by unit test, but apparently it isn't.

                        Original javascript reads:

                            const line3d = line3.fromPoints(edge[0], edge[1])
                            const closest0 = vec3.snap(vec3.create(), eps, line3.closestPoint(openedge[0], line3d))
                            const closest1 = vec3.snap(vec3.create(), eps, line3.closestPoint(openedge[1], line3d))
                            if (almostEquals(eps, closest0, openedge[0]) && almostEquals(eps, closest1, openedge[1])) {
                                return true
                            }

                    */
                    // only look for parallel open edges
                    var line3d = (edge.v1, edge.v2);
                    var closest0 = closestPoint(line3d, openedge.v1).Snap(eps);
                    var closest1 = closestPoint(line3d, openedge.v2).Snap(eps);
                    if (closest0.IsNearlyEqual(openedge.v1) && closest1.IsNearlyEqual(openedge.v2))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /*
     * Split the edge if possible from the list of open edges.
     * Return a list of new polygons, or null if not possible
     */
    public static (Poly3, Poly3)? splitEdge(List<Edge> openedges, Edge edge, double eps)
    {
        // console.log('splitEdge',edge)
        for (var i = 0; i < openedges.Count; i++)
        {
            var openedge = openedges[i];
            if (enclosedEdge(openedge, edge, eps))
            {
                // spit the polygon associated with the edge
                var polygon = edge.polygons[0];
                var newpolygons = splitPolygon(openedge, polygon, eps);
                return newpolygons;
            }
        }
        return null;
    }

    /*
     * Cull a list of open edges (see above) from the list of edges.
     */
    public static List<Edge> cullOpenEdges(List<Edge> edges)
    {
        var openedges = new List<Edge>();
        foreach (var edge in edges)
        {
            var polygons = edge.polygons;
            if (polygons.Count == 1)
            {
                // console.log('open edge: ',edge[0],'<-->',edge[1])
                openedges.Add(edge);
            }
        }
        // console.log('open edges:',openedges.length)
        // console.log('**********OPEN*********')
        // console.log(openedges)
        // console.log('**********OPEN*********')
        return openedges;
    }

    /*
     * Convert the list of edges into a list of polygons.
     */
    public static List<Poly3> edgesToPolygons(List<Edge> edges)
    {
        var polygons = new List<Poly3>();
        foreach (var edge in edges)
        {
            var visited = new HashSet<Poly3>();
            foreach (var polygon in edge.polygons)
            {
                if (visited.Contains(polygon)) continue;
                visited.Add(polygon);
                polygons.Add(polygon);
            }
        }
        return polygons;
    }

    /*
     * Convert the given list of polygons to a list of edges.
     */
    public static List<Edge> polygonsToEdges(Poly3[] polygons)
    {
        var edges = new List<Edge>(polygons.Length);
        foreach (var polygon in polygons)
        {
            addPolygon(edges, polygon);
        }
        return edges;
    }

}