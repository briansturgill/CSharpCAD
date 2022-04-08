namespace CSharpCAD;

/// <summary>Represents a new 3D geometry composed of polygons.</summary>
public class Geom3 : Geometry
{
    /// <summary>List of polygons</summary>
    public Poly3[] Polygons { get => this.polygons; }
    private Poly3[] polygons;
    private Mat4 transforms;
    public Mat4 Transforms { get => this.transforms; }
    public Color? Color;
    public readonly bool IsRetesselated;

    public override bool Is2D => false;
    public override bool Is3D => true;

    /**
     * <summary>Construct a new 3D geometry from a list of points.</summary>
     * <remarks>
     * The list of points should contain sub-arrays, each defining a single polygon of points.
     * In addition, the points should follow the right-hand rule for rotation in order to
     * define an external facing polygon.
     * </remarks>
     */
    public Geom3(List<List<Vec3>> points)
    {
        var polygons = new Poly3[points.Count];
        for (var i = 0; i < points.Count; i++)
        {
            polygons[i] = new Poly3(points[i]);
        }
        this.polygons = polygons;
        this.Color = null;
        this.transforms = new Mat4();
        this.IsRetesselated = false;
        if (GlobalParams.CheckingEnabled)
        {
            this.Validate();
        }
    }

    /// <summary>Internal constructor. Public for testing use only.</summary>
    public Geom3()
    {
        this.polygons = new Poly3[0];
        this.transforms = new Mat4();
        this.Color = null;
        this.IsRetesselated = false;
    }

    /// <summary>Internal constructor. Public for testing use only.</summary>
    public Geom3(Poly3[] polygons, Mat4? transforms = null, Color? Color = null, bool isRetesselated = false)
    {
        this.polygons = polygons;
        this.transforms = transforms ?? new Mat4();
        this.Color = Color;
        this.IsRetesselated = isRetesselated;
        if (GlobalParams.CheckingEnabled)
        {
            this.Validate();
        }
    }


    /// <summary>Check if this geometry is equal to the given geometry.</summary>
    public bool Equals(Geom3 gg)
    {
        if (this.polygons.Length != gg.polygons.Length)
        {
            return false;
        }
        for (var i = 0; i < polygons.Length; i++)
        {
            if (this.polygons[i] != gg.polygons[i])
            {
                return false;
            }
        }
        if (this.transforms != gg.transforms)
        {
            return false;
        }
        return this.Color == gg.Color;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public static bool operator ==(Geom3 a, Geom3 b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if this vector is not equal to the given vector.</summary>
    public static bool operator !=(Geom3 a, Geom3 b) => !(a == b);

    /// <summary>Standard C# override.</summary>
    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Geom3 v = (Geom3)obj;
            return Equals(v);
        }
    }

    /// <summary>Standard C# override.</summary>
    public override int GetHashCode()
    {
        return polygons.GetHashCode() ^ transforms.GetHashCode() ^ Color.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
	public override string ToString()
    {
        var s = new StringBuilder();
        s.Append("Geom3(\n");
        foreach (var side in this.polygons)
        {
            s.Append($"{side}\n");
        }
        s.Append($"{this.transforms}\n");
        s.Append($"{this.Color}");
        return s.ToString();
    }

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Geom3 gg)
    {
        if (this.polygons.Length != gg.polygons.Length)
        {
            return false;
        }
        for (var i = 0; i < polygons.Length; i++)
        {
            if (!this.polygons[i].IsNearlyEqual(gg.polygons[i]))
            {
                return false;
            }
        }
        if (!this.transforms.IsNearlyEqual(gg.transforms))
        {
            return false;
        }
        return this.Color == gg.Color;
    }

    /// <summary>Apply the transforms of the given geometry.</summary>
    /// <remarks>NOTE: This function must be called BEFORE exposing any data. See toPolygons.</remarks>
    public Geom3 ApplyTransforms()
    {
        if (this.transforms.IsIdentity()) return this;

        // apply transforms to each polygon
        // const isMirror = mat4.isMirroring(geometry.transforms)
        // TBD if (isMirror) newvertices.reverse()
        for (var i = 0; i < polygons.Length; i++)
        {
            polygons[i] = polygons[i].Transform(this.transforms);
        }
        this.transforms = new Mat4();
        return this;
    }
    public double MeasureEpsilon()
    {
        var total = 0.0;
        var (min, max) = this.BoundingBox();
        total += max.x + max.y + max.z;
        total -= min.x + min.y + min.z;
        return C.EPS * total / 3; /*dimensions*/
    }

    /// <summary>Return a full clone of this geometry.</summary>
    public Geom3 Clone()
    {
        // There is no need to copy each polygon, transform matrix or Color as they are immutable.
        return new Geom3(this.polygons.ToArray(), this.transforms, this.Color);
    }

    /// <summary>Invert this geometry, transposing solid and empty space.</summary>
    public Geom3 Invert()
    {
        var polygons = ToPolygons();
        var len = polygons.Length;
        var newpolygons = new Poly3[len];
        for (var i = 0; i < len; i++)
        {
            newpolygons[i] = polygons[i].Invert();
        }
        return new Geom3(newpolygons, this.transforms, this.Color);
    }

    /// <summary>Return the (min, max) BoundingBox of this geometry.</summary>
    public (Vec3, Vec3) BoundingBox()
    {
        this.ApplyTransforms();
        var min = new Vec3();
        var max = new Vec3();
        if (this.polygons.Length > 0)
        {
            min = this.polygons[0].ToPoints()[0];
        }
        max = min;
        foreach (var p in this.polygons)
        {
            var (n, x) = p.BoundingBox();
            min = min.Min(n);
            max = max.Max(x);
        }
        return (min, max);
    }

    /// <summary>Return this geometry as a list of points, after applying transforms.</summary>
    public List<List<Vec3>> ToPoints()
    {
        var polygons = this.ToPolygons();
        var listofListofpoints = new List<List<Vec3>>(this.polygons.Length);
        foreach (var p in polygons)
        {
            listofListofpoints.Add(p.ToPoints());
        }
        return listofListofpoints;
    }

    /// <summary>Produces an array of polygons from this geometry, after applying transforms.</summary>
    /// <remarks>The returned array should not be modified as the polygons are shared with the geometry.</remarks>
    public Poly3[] ToPolygons() => this.ApplyTransforms().polygons;


    public Geom3 Transform(Mat4 matrix)
    {
        var transforms = matrix.Multiply(this.transforms);
        return new Geom3(this.polygons.ToArray(), transforms, Color);
    }

    /**
     * <summary>Determine if the given object is a valid 3D geometry.</summary>
     * <remarks>
     * Checks for valid data structure, convex polygon faces, and manifold edges.
     *
     * **If the geometry is not valid, an exception will be thrown with details of the geometry error.**
     * </remarks>
     */
    public void Validate()
    {
        // check polygons
        foreach (var polygon in polygons)
        {
            polygon.Validate();
        }
        this.ValidateManifold();

        // check transforms
        this.transforms.Validate();

        // TODO: check for self-intersecting
    }

    /*
     * Check manifold edge condition: Every edge is in exactly 2 faces
     */
    private void ValidateManifold()
    {
        // count of each edge
        var edgeCount = new Dictionary<(Vec3, Vec3), int>();
        foreach (var polygon in polygons)
        {
            var vertices = polygon.Vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                // sort for undirected edge
                var edge = (vertices[i], vertices[(i + 1) % vertices.Length]);
                var count = 0;
                if (edgeCount.ContainsKey(edge))
                {
                    count = edgeCount[edge];
                    edgeCount[edge] = count + 1;
                }
            }
        }
        // check that edges are always matched
        var nonManifold = new List<(Vec3, Vec3)>(0);
        foreach (var (edge, count) in edgeCount)
        {
            var (v0, v1) = edge;
            var complementEdge = (v1, v0);
            var complementCount = 0;
            if (edgeCount.ContainsKey(complementEdge))
            {
                complementCount = edgeCount[complementEdge];
            }
            if (count != complementCount)
            {
                nonManifold.Add(edge);
            }
        }
        if (nonManifold.Count > 0)
        {
            throw new ValidationException($"Non-manifold edge count: {nonManifold.Count}\n");
        }
    }
}