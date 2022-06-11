namespace CSharpCAD;

/// <summary>Represents a new 3D geometry composed of polygons.</summary>
public class Geom3 : Geometry
{
    ///
    internal Poly3[] polygons;
    private Mat4 transforms;
    private (Vec3, Vec3)? boundingBox;
    private bool needsTransform;
    ///
    public Mat4 Transforms { get => this.transforms; }
    ///
    public Color? Color;
    internal readonly bool IsRetesselated;

    /// <summary>Is this a 2D geometry object?</summary>
    public override bool Is2D => false;

    /// <summary>Is this a 3D geometry object?</summary>
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
        this.boundingBox = null;
        this.needsTransform = false;
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
        this.boundingBox = null;
        this.needsTransform = false;
    }

    /// <summary>Internal constructor. Public for testing use only.</summary>
    public Geom3(Poly3[] polygons, Mat4? transforms = null, Color? Color = null, bool isRetesselated = false, bool needsTransform = false)
    {
        this.polygons = polygons;
        this.transforms = transforms ?? new Mat4();
        this.Color = Color;
        this.IsRetesselated = isRetesselated;
        this.boundingBox = null;
        this.needsTransform = needsTransform;
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
        if (!this.needsTransform) return this;
        if (this.transforms.IsIdentity()) return this;

        // apply transforms to each polygon
        // const isMirror = mat4.isMirroring(geometry.transforms)
        // TBD if (isMirror) newvertices.reverse()
        for (var i = 0; i < polygons.Length; i++)
        {
            polygons[i] = polygons[i].Transform(this.transforms);
        }
        this.transforms = new Mat4();
        this.needsTransform = false;
        return this;
    }

    /// <summary>Measure the epsilon value for this geometry object.</summary>
    public double MeasureEpsilon()
    {
        var total = 0.0;
        var (min, max) = this.BoundingBox();
        total += max.X + max.Y + max.Z;
        total -= min.X + min.Y + min.Z;
        return C.EPS * total / 3; /*dimensions*/
    }

    /// <summary>Return a full clone of this geometry.</summary>
    public Geom3 Clone()
    {
        // There is no need to copy the transform matrix or Color as they are immutable.
        return new Geom3(this.polygons.ToArray(), this.transforms, this.Color, this.IsRetesselated, this.needsTransform);
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
        if (this.boundingBox is not null) return ((Vec3, Vec3))this.boundingBox;
        this.ApplyTransforms();
        if (this.polygons.Length == 0)
        {
            return (new Vec3(), new Vec3());
        }
        var v0 = polygons[0].Vertices[0];
        var min_x = v0.X;
        var min_y = v0.Y;
        var min_z = v0.Z;
        var max_x = min_x;
        var max_y = min_y;
        var max_z = min_z;

        foreach (var p in this.polygons)
        {
            //var (n, x) = p.BoundingBox();
            foreach (var v in p.Vertices)
            {
                if (v.X < min_x) min_x = v.X;
                if (v.Y < min_y) min_y = v.Y;
                if (v.Z < min_z) min_z = v.Z;
                if (v.X > max_x) max_x = v.X;
                if (v.Y > max_y) max_y = v.Y;
                if (v.Z > max_z) max_z = v.Z;
            }
        }
        var bb = (new Vec3(min_x, min_y, min_z), new Vec3(max_x, max_y, max_z));
        this.boundingBox = bb;
        return bb;
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


    /// <summary>Add a transformation to this geometry.</summary>
    /// <remarks>This is done in a lazy fashion, only affecting the internal transforms vector.</remarks>
    public Geom3 Transform(Mat4 matrix)
    {
        var transforms = matrix.Multiply(this.transforms);
        return new Geom3(this.polygons.ToArray(), transforms, Color, needsTransform: true);
    }

    /**
     * <summary>Determine if the given object is a valid 3D geometry.</summary>
     * <remarks>
     * Checks for valid data structure, convex polygon faces, and manifold edges.
     *
     * **If the geometry is not valid, an exception will be thrown with details of the geometry error.**
     * </remarks>
     */
    public override void Validate()
    {
        // check polygons
        foreach (var polygon in polygons)
        {
            polygon.Validate();
        }
        this.ValidateManifold();

        // check transforms
        this.transforms.Validate();

        // LATER: check for self-intersecting
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