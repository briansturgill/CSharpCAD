namespace CSharpCAD;

/// <summary>An advanced class, you need to understand "extrudeFromSlices.cs" before using.</summary>
public class Slice : IEquatable<Slice>
{
    internal List<Edge> edges;

    // Internal constructor.
    private Slice(List<Edge> edges)
    {
        this.edges = edges;
    }

    // Internal constructor.
    internal Slice(Geom2.Side[] sides)
    {
        edges = new List<Edge>(sides.Length);
        // create a list of edges from the sides
        for (var i = 0; i < sides.Length; i++)
        {
            var side = sides[i];
            edges.Add(new Edge(new Vec3(side.v0.X, side.v0.Y, 0), new Vec3(side.v1.X, side.v1.Y, 0)));
        }
    }
    /// <summary>Create a slice from the given points.</summary>
    public Slice(List<Vec2> points)
    {
        if (points.Count < 3) throw new ArgumentException("The given points must contain THREE or more points.");

        this.edges = new List <Edge>(points.Count);
        var prevpoint = points[points.Count - 1];
        for (int i = 0; i < points.Count; i++)
        {
            var point = points[i];
            this.edges.Add(new Edge(new Vec3(prevpoint.X, prevpoint.Y, 0), new Vec3(point.X, point.Y, 0)));
            prevpoint = point;
        }
    }

    ///
    public Slice(List<Vec3> points)
    {
        if (points.Count < 3) throw new ArgumentException("The given points must contain THREE or more points.");

        this.edges = new List<Edge>(points.Count);
        var prevpoint = points[points.Count - 1];
        for (int i = 0; i < points.Count; i++)
        {
            var point = points[i];
            this.edges.Add(new Edge(prevpoint, point));
            prevpoint = point;
        }
    }

    ///
    public bool Equals(Slice? gs)
    {
        if (gs is null)
        {
            return false;
        }
        var aedges = this.edges;
        var bedges = gs.edges;

        if (aedges.Count != bedges.Count)
        {
            return false;
        }

        for (var i = 0; i < aedges.Count; i++)
        {
            if (aedges[i] != bedges[i])
            {
                return false;
            }
        }
        return true;
    }

    ///
    public static bool operator ==(Slice a, Slice b)
    {
        return a.Equals(b);
    }

    ///
    public static bool operator !=(Slice a, Slice b) => !(a == b);

    ///
    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Slice s = (Slice)obj;
            return Equals(s);
        }
    }

    ///
    public override int GetHashCode()
    {
        var hc = 0;
        foreach (var edge in edges)
        {
            hc ^= edge.GetHashCode();
        }
        return hc;
    }

    ///
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("Slice(\n");
        foreach (var edge in edges)
        {
            sb.Append($"{edge}\n");
        }
        sb.Append(")\n");
        return sb.ToString();
    }

    ///
    public bool IsNearlyEqual(Slice gs)
    {
        var aedges = this.edges;
        var bedges = gs.edges;

        if (aedges.Count != bedges.Count)
        {
            return false;
        }

        for (var i = 0; i < aedges.Count; i++)
        {
            var aedge = aedges[i];
            var bedge = bedges[i];
            var d = aedge.v0.SquaredDistance(bedge.v0);
            if (d >= C.EPSILON)
            {
                return false;
            }
        }
        return true;
    }



    /// <summary>Calculate the plane of the given slice.</summary>
    /// <remarks>NOTE: The slice (and all points) are assumed to be planar from the beginning.</remarks>
    public Plane CalculatePlane()
    {
        var edges = this.edges;
        if (edges.Count < 3) throw new ArgumentException("Slices must have 3 or more edges to calculate a plane.");

        // find the midpoint of the slice, which will lie on the plane by definition
        var midpoint = new Vec3();
        foreach (var edge in edges)
        {
            midpoint = edge.v0.Add(midpoint);
        }
        midpoint = midpoint.Scale(1 / edges.Count);

        // find the farthest edge from the midpoint, which will be on an outside edge
        Edge farthestEdge = new Edge();
        var distance = ((double)(0.0));
        foreach (var edge in edges)
        {
            if (edge.v0 != edge.v1) // Make sure it is not a self-edge.
            {
                var d = midpoint.SquaredDistance(edge.v0);
                if (d > distance)
                {
                    farthestEdge = edge;
                    distance = d;
                }
            }
        }
        // find the before edge
        var beforeEdge = edges.Find((Edge edge) => edge.v1 == farthestEdge.v0);
        if (beforeEdge is null)
        {
            throw new InvalidDataException("Faulty edge set.");
        }
        return Plane.From3Points(beforeEdge.v0, farthestEdge.v0, farthestEdge.v1);
    }

    /// <summary>Reverse the edges of the given slice.</summary>
    public Slice Reverse()
    {
        // reverse the edges
        var newedges = new List<Edge>(edges.Count);
        for (int i = 0; i < edges.Count; i++)
        {
            newedges.Add(new Edge(edges[i].v1, edges[i].v0));
        }
        return new Slice(newedges);
    }

    internal List<Edge> ToEdges() => edges;

    private Poly3 toPolygon3D(Vec3 vector, Edge edge)
    {
        return new Poly3(new Vec3[]{
          edge.v0.Subtract(vector),
          edge.v1.Subtract(vector),
          edge.v1.Add(vector),
          edge.v0.Add(vector)}
        );
    }

    /// <summary>Return a list of polygons which are enclosed by the slice.</summary>
    public Poly3[] ToPolygons()
    {
        var hierarchy = new Earcut.PolygonHierarchy(this);

        var polygons = new List<Poly3>();
        foreach (var (solid, holes) in hierarchy.roots)
        {
            // hole indices
            var index = solid.Count;
            var holesIndex = new List<int>();
            foreach (var hole in holes)
            {
                holesIndex.Add(index);
                index += hole.Count;
            }

            // compute earcut triangulation for each solid
            var len = solid.Count;
            foreach (var hole in holes)
            {
                len += hole.Count;
            }

            var vertices = new List<Vec2>(len);
            vertices.AddRange(solid);
            foreach (var hole in holes)
            {
                vertices.AddRange(hole);
            }

            var data_index = 0;
            var data = new double[len * 2];
            foreach (var v in solid)
            {
                data[data_index++] = v.X;
                data[data_index++] = v.Y;
            }
            foreach (var hole in holes)
            {
                foreach (var v in hole)
                {
                    data[data_index++] = v.X;
                    data[data_index++] = v.Y;
                }
            }

            // Get original 3D vertex by index
            var getVertex = (int i) => hierarchy.To3D(vertices[i]);
            var indices = Earcut.Triangulate(data, holesIndex);
            for (var i = 0; i < indices.Count; i += 3)
            {
                var v1 = getVertex(indices[i]);
                var v2 = getVertex(indices[i + 1]);
                var v3 = getVertex(indices[i + 2]);
                // Map back to original vertices
                polygons.Add(new Poly3(new List<Vec3> { v1, v2, v3 }));
            }
        }
        return polygons.ToArray();
    }

    /// <summary>Transform this slice using the given matrix.</summary>
    public Slice Transform(Mat4 matrix)
    {
        var newedges = new List<Edge>(edges.Count);
        for (var i = 0; i < edges.Count; i++)
        {
            var edge = edges[i];
            newedges.Add(new Edge(edge.v0.Transform(matrix), edge.v1.Transform(matrix)));
        }
        return new Slice(newedges);
    }

    internal class Edge : IEquatable<Edge>
    {
        public Vec3 v0;
        public Vec3 v1;

        public Edge()
        {
            this.v0 = new Vec3();
            this.v1 = new Vec3();
        }

        public Edge(Vec3 v0, Vec3 v1)
        {
            this.v0 = v0;
            this.v1 = v1;
        }

        public bool Equals(Edge? ge)
        {
            if (ge is null)
            {
                return false;
            }
            return this.v0 == ge.v0 && this.v1 == ge.v1;
        }

        public static bool operator ==(Edge a, Edge b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Edge a, Edge b) => !(a == b);

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Edge e = (Edge)obj;
                return Equals(e);
            }
        }

        public override int GetHashCode()
        {
            return v0.GetHashCode() ^ v1.GetHashCode();
        }

        public override string ToString() => $"Edge({this.v0},{this.v1})";

        public Edge Complement()
        {
            return new Edge(v1, v0);
        }
    }

    /// <summary>Mend gaps in a 2D slice to make it a closed polygon.</summary>
    public void RepairSlice()
    {
        if (edges.Count == 0) return;
        var edgeCount = new Dictionary<Vec3, int>(); // count of (in - out) edges
        foreach (var edge in edges)
        {
            if (!edgeCount.ContainsKey(edge.v0))
            {
                edgeCount[edge.v0] = 1;
            }
            else
            {
                edgeCount[edge.v0] = edgeCount[edge.v0] + 1; // in;
            }
            if (!edgeCount.ContainsKey(edge.v1))
            {
                edgeCount[edge.v1] = -1;
            }
            else
            {
                edgeCount[edge.v1] = edgeCount[edge.v1] - 1; // out
            }
        }
        // find vertices which are missing in or out edges
        var missingIn = new List<Vec3>(edgeCount.Count);
        var missingOut = new List<Vec3>(edgeCount.Count);
        foreach (var (v, count) in edgeCount)
        {
            if (count < 0) { missingIn.Add(v); }
            if (count > 0) { missingOut.Add(v); }
        }
        // pairwise distance of bad vertices
        foreach (var v1 in missingIn)
        {
            // find the closest vertex that is missing an out edge
            var bestDistance = double.PositiveInfinity;
            Vec3 bestReplacement = new Vec3(double.NaN, double.NaN, double.NaN);
            foreach (var v2 in missingOut)
            {
                var distance = Vec2.Hypot(v1.X - v2.X, v1.Y - v2.Y);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestReplacement = v2;
                }
            }
            Console.WriteLine($"repairSlice: repairing vertex gap {v1} to {bestReplacement} distance {bestDistance}");
            // merge broken vertices
            foreach (var edge in edges)
            {
                if (edge.v0 == v1) edge.v0 = bestReplacement;
                if (edge.v1 == v1) edge.v1 = bestReplacement;
            }
        }
        // Remove self-edges
        edges = edges.Where((Edge e) => e.v0 != e.v1).ToList();
    }
}