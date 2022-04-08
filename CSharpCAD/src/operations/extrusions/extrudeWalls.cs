namespace CSharpCAD;

public static partial class CSCAD
{

    // https://en.wikipedia.org/wiki/Greatest_common_divisor#Using_Euclid's_algorithm
    private static int gcd(int a, int b)
    {
        if (a == b) { return a; }
        if (a < b) { return gcd(b, a); }
        if (b == 1) { return 1; }
        if (b == 0) { return a; }
        return gcd(b, a % b);
    }

    private static int lcm(int a, int b) => (a * b) / gcd(a, b);

    // Return a set of edges that encloses the same area by splitting
    // the given edges to have newlength total edges.
    private static Slice.Edge[] repartitionEdges(int newlength, Slice.Edge[] edges)
    {
        // NOTE: This implementation splits each edge evenly.
        var multiple = newlength / edges.Length;
        if (multiple == 1)
        {
            return edges;
        }

        var divisor = new Vec3(multiple, multiple, multiple);

        var newEdges = new List<Slice.Edge>();
        foreach (var edge in edges)
        {
            var increment = edge.v1.Subtract(edge.v0);
            increment = increment.Divide(divisor);

            // repartition the edge
            var prev = edge.v0;
            for (var i = 1; i <= multiple; ++i)
            {
                var next = prev.Add(increment);
                newEdges.Add(new Slice.Edge(prev, next));
                prev = next;
            }
        }
        return newEdges.ToArray();
    }

    private static readonly double EPSAREA = (C.EPS * C.EPS / 2) * Math.Sin(Math.PI / 3);

    /*
     * Extrude (build) walls between the given slices.
     * Each wall consists of two triangles, which may be invalid if slices are overlapping.
     */
    internal static List<Poly3> ExtrudeWalls(Slice slice0, Slice slice1)
    {
        var edges0 = slice0.ToEdges();
        var edges1 = slice1.ToEdges();

        if (edges0.Length != edges1.Length)
        {
            // different shapes, so adjust one or both to the same number of edges
            var newlength = lcm(edges0.Length, edges1.Length);
            if (newlength != edges0.Length) edges0 = repartitionEdges(newlength, edges0);
            if (newlength != edges1.Length) edges1 = repartitionEdges(newlength, edges1);
        }

        var walls = new List<Poly3>();
        for (var i = 0; i < edges0.Length; i++)
        {
            var edge0 = edges0[i];
            var edge1 = edges1[i];

            var poly0 = new Poly3(new Vec3[] { edge0.v0, edge0.v1, edge1.v1 });
            var poly0area = poly0.Area();
            if (double.IsFinite(poly0area) && poly0area > EPSAREA) walls.Add(poly0);

            var poly1 = new Poly3(new Vec3[] { edge0.v0, edge1.v1, edge1.v0 });
            var poly1area = poly1.Area();
            if (double.IsFinite(poly1area) && poly1area > EPSAREA) walls.Add(poly1);
        }
        return walls;
    }

}
