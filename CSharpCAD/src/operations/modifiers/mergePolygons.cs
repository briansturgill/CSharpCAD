namespace CSharpCAD;

internal static partial class Modifiers
{
    // create a set of edges from the given polygon, and link the edges as well
    public static List<Edge> CreateEdges(Poly3 polygon)
    {
        var points = polygon.Vertices;
        var edges = new List<Edge>();
        for (var i = 0; i < points.Length; i++)
        {
            var j = (i + 1) % points.Length;
            var edge = new Edge(points[i], points[j]);
            edges.Add(edge);
        }
        // link the edges together
        for (var i = 0; i < edges.Count; i++)
        {
            var j = (i + 1) % points.Length;
            edges[i].next = edges[j];
            edges[j].prev = edges[i];
        }
        return edges;
    }

    public static void InsertEdge(Dictionary<string, Edge> edgeMap, Edge edge)
    {
        var key = $"{edge.v1}:{edge.v2}";
        edgeMap[key] = edge;
    }

    public static void DeleteEdge(Dictionary<string, Edge> edgeMap, Edge edge)
    {
        var key = $"{edge.v1}:{edge.v2}";
        edgeMap.Remove(key);
    }

    public static Edge? FindOppositeEdge(Dictionary<string, Edge> edgeMap, Edge edge)
    {
        Edge? ret = null;
        var key = $"{edge.v2}:{edge.v1}"; // NOTE: OPPOSITE OF INSERT KEY
        edgeMap.TryGetValue(key, out ret);
        return ret;
    }

    // calculate the two adjoining angles between the opposing edges
    public static (double, double) CalculateAnglesBetween(Edge current, Edge opposite, Vec3 normal)
    {
        if (current.prev is null || opposite.prev is null || current.next is null || opposite.next is null)
        {
            throw new NullReferenceException("Unexpected nulls");
        }
        var v0 = current.prev.v1;
        var v1 = current.prev.v2;
        Vec3 v2 = opposite.next.v2;
        var angle1 = CalculateAngle(v0, v1, v2, normal);

        v0 = opposite.prev.v1;
        v1 = opposite.prev.v2;
        v2 = current.next.v2;
        var angle2 = CalculateAngle(v0, v1, v2, normal);

        return (angle1, angle2);
    }

    public static double CalculateAngle(Vec3 prevpoint, Vec3 point, Vec3 nextpoint, Vec3 normal)
    {
        var d0 = point.Subtract(prevpoint);
        var d1 = nextpoint.Subtract(point);
        d0 = d0.Cross(d1);
        return d0.Dot(normal);
    }

    // create a polygon starting from the given edge (if possible)
    public static Poly3 CreatePolygonAnd(Edge edge)
    {
        var points = new List<Vec3>();
        while (edge.next is not null)
        {
            var next = edge.next;

            points.Add(edge.v1);

            edge.prev = null;
            edge.next = null;

            edge = next;
        }
        return new Poly3(points);
    }

    /*
     * Merge COPLANAR polygons that share common edges.
     * @param {poly3[]} sourcepolygons - list of polygons
     * @returns {poly3[]} new set of polygons
     */
    public static List<Poly3> MergeCoplanarPolygons(double epsilon, List<Poly3> sourcepolygons)
    {
        if (sourcepolygons.Count < 2) return sourcepolygons;

        var normal = sourcepolygons[0].Plane();
        var polygons = new List<Poly3>();
        polygons.AddRange(sourcepolygons);
        Debug.Assert(!Object.ReferenceEquals(polygons, sourcepolygons));
        var edgeListMap = new Dictionary<string, Edge>();

        while (polygons.Count > 0)
        { // NOTE: the length of polygons WILL change
            var polygon = polygons[0];
            polygons.RemoveAt(0);
            var edges = CreateEdges(polygon);
            for (var i = 0; i < edges.Count; i++)
            {
                var current = edges[i];
                var opposite = FindOppositeEdge(edgeListMap, current);
                if (opposite is not null && opposite.prev is not null && opposite.next is not null
                    && current.prev is not null && current.next is not null)
                {
                    var (angle_0, angle_1) = CalculateAnglesBetween(current, opposite, normal.Normal);
                    if (angle_0 >= 0 && angle_1 >= 0)
                    {
                        var edge1 = opposite.next;
                        var edge2 = current.next;
                        // adjust the edges, linking together opposing polygons
                        current.prev.next = opposite.next;
                        current.next.prev = opposite.prev;

                        opposite.prev.next = current.next;
                        opposite.next.prev = current.prev;

                        // remove the opposing edges
                        current.next = null;
                        current.prev = null;

                        DeleteEdge(edgeListMap, opposite);

                        opposite.next = null;
                        opposite.prev = null;

                        void mergeEdges(Dictionary<string, Edge> list, Edge e1, Edge e2)
                        {
                            var newedge = new Edge(e2.v1, e1.v2);
                            newedge.next = e1.next;
                            newedge.prev = e2.prev;
                            // link in newedge
#nullable disable
                            e2.prev.next = newedge;
                            e1.next.prev = newedge;
#nullable enable
                            // remove old edges
                            DeleteEdge(list, e1);
                            e1.next = null;
                            e1.prev = null;

                            DeleteEdge(list, e2);
                            e2.next = null;
                            e2.prev = null;
                        }

#nullable disable
                        if (angle_0 == 0.0)
                        {
                            mergeEdges(edgeListMap, edge1, edge1.prev);
                        }
                        if (angle_1 == 0.0)
                        {
                            mergeEdges(edgeListMap, edge2, edge2.prev);
                        }
#nullable enable
                    }
                }
                else
                {
                    if (current.next is not null) InsertEdge(edgeListMap, current);
                }
            }
        }

        // build a set of polygons from the remaining edges
        var destpolygons = new List<Poly3>();

        foreach (var (tag, edge) in edgeListMap)
        {
            var polygon = CreatePolygonAnd(edge);
            if (polygon.Vertices.Length > 0) destpolygons.Add(polygon);
        }

        return destpolygons;
    }

    // Normals are directional vectors with component values from 0 to 1.0, requiring specialized comparision
    // This EPS is derived from a serieas of tests to determine the optimal precision for comparing coplanar polygons,
    // as provided by the sphere primitive at high segmentation
    // This EPS is for 64 bit Number values
    public const double NEPS = 1e-13;

    // Compare two normals (unit vectors) for equality.
    public static bool AboutEqualNormals(Vec3 a, Vec3 b) => (Math.Abs(a.x - b.x) <= NEPS && Math.Abs(a.y - b.y) <= NEPS && Math.Abs(a.z - b.z) <= NEPS);

    public static bool Coplanar(Plane plane1, Plane plane2)
    {
        // expect the same distance from the origin, within tolerance
        if (Math.Abs(plane1.W - plane2.W) < 0.00000015)
        {
            return AboutEqualNormals(plane1.Normal, plane2.Normal);
        }
        return false;
    }

    public static Poly3[] mergePolygons(double epsilon, Poly3[] polygons)
    {
        var polygonsPerPlane = new Dictionary<Plane, List<Poly3>>(); // elements: [plane, [poly3...]]
        foreach (var polygon in polygons)
        {
            Plane? coplanar = null;
            foreach (var p in polygonsPerPlane.Keys)
            {
                if (Coplanar(p, polygon.Plane()))
                {
                    coplanar = p;
                    break;
                }
            }
            if (coplanar is not null)
            {
                polygonsPerPlane[(Plane)coplanar].Add(polygon);
            }
            else
            {
                polygonsPerPlane.Add(polygon.Plane(), new List<Poly3> { polygon});
            }
        }

        var destpolygons = new List<Poly3>();
        foreach (var polyList in polygonsPerPlane.Values)
        {
            var retesselayedpolygons = MergeCoplanarPolygons(epsilon, polyList);
            destpolygons.AddRange(retesselayedpolygons);
        }
        return destpolygons.ToArray();
    }
}