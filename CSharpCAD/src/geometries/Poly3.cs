namespace CSharpCAD;

/// <summary>Represents a convex 3D polygon.</summary>
/// <remarks>The vertices used to initialize a polygon must be coplanar and form a convex shape.</remarks>
public class Poly3 : IEquatable<Poly3>
{
    private Vec3[] vertices;
    internal Vec3[] Vertices { get => vertices; }
    internal Color? Color;
    private CSharpCAD.Plane? _plane; // Cached Plane
    private (Vec3, Vec3)? boundingBox = null;
    private (Vec3, double)? boundingSphere = null;

    /// <summary>Creates a new 3D polygon with initial values.</summary>
    public Poly3(List<Vec3> points, Color? Color = null)
    {
        this.vertices = points.ToArray();
        this.Color = Color;
        this._plane = null;
    }

    // <summary>Internal constructor.</summary>
    internal Poly3(Vec3[] vertices, Color? Color = null)
    {
        this.vertices = vertices;
        this.Color = Color;
        this._plane = null;
    }

    /// <summary>Check if this polygon is equal to the given polygon.</summary>
    public bool Equals(Poly3? gp)
    {
        if (gp is null)
        {
            return false;
        }
        if (this.vertices.Length != gp.vertices.Length)
        {
            return false;
        }
        for (var i = 0; i < vertices.Length; i++)
        {
            if (this.vertices[i] != gp.vertices[i])
            {
                return false;
            }
        }
        return this.Color == gp.Color;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public static bool operator ==(Poly3 a, Poly3 b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if this vector is not equal to the given vector.</summary>
    public static bool operator !=(Poly3 a, Poly3 b) => !(a == b);

    /// <summary>Standard C# override.</summary>
    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Poly3 v = (Poly3)obj;
            return Equals(v);
        }
    }

    /// <summary>Standard C# override.</summary>
    public override int GetHashCode()
    {
        return vertices.GetHashCode() ^ Color.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
	public override string ToString()
    {
        var s = new StringBuilder();
        s.Append("Poly3(\n");
        foreach (var vertex in this.vertices)
        {
            s.Append($"{vertex}\n");
        }
        s.Append($"{this.Color}");
        return s.ToString();
    }

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Poly3 gp)
    {
        if (this.vertices.Length != gp.vertices.Length)
        {
            return false;
        }
        for (var i = 0; i < vertices.Length; i++)
        {
            if (!this.vertices[i].IsNearlyEqual(gp.vertices[i]))
            {
                return false;
            }
        }
        return this.Color == gp.Color;
    }

    /// <summary>Create a deep clone of this polygon.</summary>
    public Poly3 Clone()
    {
        var vcopy = new Vec3[this.vertices.Length];
        Array.Copy(vertices, vcopy, vertices.Length);
        return new Poly3(vcopy, this.Color);
    }


    /// <summary>Invert this polygon to face the opposite direction.</summary>
    public Poly3 Invert()
    {
        var vertices = new Vec3[this.vertices.Length];
        var len = vertices.Length;
        for (int i = 0; i < len; i++)
        {
            vertices[len - i - 1] = this.vertices[i];
        }

        return new Poly3(vertices, this.Color);
    }

    /// <summary>Check whether this polygon is convex.</summary>
    public bool IsConvex() => AreVerticesConvex(this.vertices);

    /// <summary>Check whether a set of vertices are convex.</summary>
    public static bool AreVerticesConvex(Vec3[] vertices)
    {
        var numvertices = vertices.Length;
        if (numvertices > 2)
        {
            // note: plane ~= normal point
            var normal = CSharpCAD.Plane.FromPoints(vertices).Normal;
            var prevprevpos = vertices[numvertices - 2];
            var prevpos = vertices[numvertices - 1];
            for (var i = 0; i < numvertices; i++)
            {
                var pos = vertices[i];
                if (!Poly3.IsConvexPoint(prevprevpos, prevpos, pos, normal))
                {
                    return false;
                }
                prevprevpos = prevpos;
                prevpos = pos;
            }
        }
        return true;
    }

    /// <summary>Calculate whether three points form a convex corner.</summary>
    public static bool IsConvexPoint(Vec3 prevpoint, Vec3 point, Vec3 nextpoint, Vec3 normal)
    {
        var crossproduct = point.Subtract(prevpoint).Cross(nextpoint.Subtract(point));
        var crossdotnormal = crossproduct.Dot(normal);
        return crossdotnormal >= 0;
    }

    /// <summary>Measure the area of this polygon.</summary>
    /// <remarks>@see 2000 softSurfer http://geomalgorithms.com</remarks>
    public double Area()
    {
        var n = this.vertices.Length;
        if (n < 3)
        {
            return 0; // degenerate polygon
        }
        var vertices = this.vertices;

        // calculate a normal vector
        var normal = Plane().Normal;

        // determine direction of projection
        var ax = Math.Abs(normal.X);
        var ay = Math.Abs(normal.Y);
        var az = Math.Abs(normal.Z);

        if (ax + ay + az == 0)
        {
            // normal does not exist
            return 0;
        }

        var coord = 3; // ignore Z coordinates
        if ((ax > ay) && (ax > az))
        {
            coord = 1; // ignore X coordinates
        }
        else
        if (ay > az)
        {
            coord = 2; // ignore Y coordinates
        }

        var area = 0.0;
        var h = 0;
        var i = 1;
        var j = 2;
        switch (coord)
        {
            case 1: // ignore X coordinates
                    // compute area of 2D projection
                for (i = 1; i < n; i++)
                {
                    h = i - 1;
                    j = (i + 1) % n;
                    area += (vertices[i].Y * (vertices[j].Z - vertices[h].Z));
                }
                area += (vertices[0].Y * (vertices[1].Z - vertices[n - 1].Z));
                // scale to get area
                area /= (2 * normal.X);
                break;

            case 2: // ignore Y coordinates
                    // compute area of 2D projection
                for (i = 1; i < n; i++)
                {
                    h = i - 1;
                    j = (i + 1) % n;
                    area += (vertices[i].Z * (vertices[j].X - vertices[h].X));
                }
                area += (vertices[0].Z * (vertices[1].X - vertices[n - 1].X));
                // scale to get area
                area /= (2 * normal.Y);
                break;

            case 3: // ignore Z coordinates
            default:
                // compute area of 2D projection
                for (i = 1; i < n; i++)
                {
                    h = i - 1;
                    j = (i + 1) % n;
                    area += (vertices[i].X * (vertices[j].Y - vertices[h].Y));
                }
                area += (vertices[0].X * (vertices[1].Y - vertices[n - 1].Y));
                // scale to get area
                area /= (2 * normal.Z);
                break;
        }
        return area;
    }

    /// <summary>Measure the bounding box of this polygon.</summary>
    /// <returns>Tuple of (min, max)</returns>
    public (Vec3, Vec3) BoundingBox()
    {
        if (this.boundingBox is not null) return ((Vec3, Vec3))this.boundingBox;
        var vertices = this.vertices;
        var numvertices = vertices.Length;
        var min = numvertices == 0 ? new Vec3() : vertices[0];
        var max = min;
        for (var i = 1; i < numvertices; i++)
        {
            min = min.Min(vertices[i]);
            max = max.Max(vertices[i]);
        }

        var bb = (min, max);
        this.boundingBox = bb;
        return bb;
    }

    /// <summary>Measure the bounding sphere of the given polygon.</summary>
    /// <returns>Tuple of (center, radius)</returns>
    public (Vec3, double) BoundingSphere()
    {
        if (this.boundingSphere is not null) return ((Vec3, double))this.boundingSphere;
        var (box_0, box_1) = this.BoundingBox();
        var center = box_0;
        center = box_0.Add(box_1);
        center = center.Scale(0.5);
        var radius = center.Distance(box_1);
        var bs = (center, radius);
        this.boundingSphere = bs;
        return bs;
    }

    /**
     * <summary>Measure the signed volume of the given polygon, which must be convex.</summary>
     * <remarks>
     * The volume is that formed by the tetrahedon connected to the axis [0,0,0],
     * and will be positive or negative based on the rotation of the vertices.
     * @see http://chenlab.ece.cornell.edu/Publication/Cha/icip01_Cha.pdf
     * </remarks>
     */
    public double SignedVolume()
    {
        var signedVolume = 0.0;
        var vertices = this.vertices;
        // calculate based on triangluar polygons
        for (var i = 0; i < vertices.Length - 2; i++)
        {
            var cross = vertices[i + 1].Cross(vertices[i + 2]);
            signedVolume += vertices[0].Dot(cross);
        }
        signedVolume /= 6;
        return signedVolume;
    }

    /// <summary>The plane of this polygon.</summary>
    public Plane Plane()
    {
        if (_plane is null)
        {
            _plane = CSharpCAD.Plane.FromPoints(this.vertices);
        }
        return _plane;
    }

    /// <summary>Return the given polygon as a list of points.</summary>
    public List<Vec3> ToPoints() => this.vertices.ToList();

    /// <summary>Transform the given polygon using the given matrix.</summary>
    public Poly3 Transform(Mat4 matrix)
    {
        var vertices = new Vec3[this.vertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = this.vertices[i].Transform(matrix);
        }
        if (matrix.IsMirroring())
        {
            // reverse the order to preserve the orientation
            vertices = vertices.Reverse().ToArray();
        }
        return new Poly3(vertices, this.Color);
    }

    /**
     * <summary>Determine if this object is a valid polygon.</summary>
     * <remarks>
     * Checks for valid data structure, convex polygons, and duplicate points.
     *
     * **If the geometry is not valid, an exception will be thrown with details of the geometry error.**
     * </remarks>
     */
    public void Validate()
    {
        // check for empty polygon
        if (this.vertices.Length < 3)
        {
            throw new ValidationException($"Poly3 has less than three vertices: {this.vertices.Length}.");
        }
        // check area
        if (this.Area() <= 0)
        {
            throw new ValidationException("Poly3 area must be greater than zero.");
        }

        // check for duplicate points
        for (var i = 0; i < this.vertices.Length; i++)
        {
            var v0 = this.vertices[i];
            var v1 = this.vertices[(i + 1) % this.vertices.Length];
            if (v1 == v0)
            {
                throw new ValidationException($"Poly3 has duplicate vertex: {this.vertices[i]}.");
            }
        }

        // check convexity
        if (!this.IsConvex())
        {
            throw new ValidationException("Poly3 must be convex.");
        }

        // check for infinity, nan
        foreach (var vertex in this.vertices)
        {
            {
                if (!double.IsFinite(vertex.X) || !double.IsFinite(vertex.Y) || !double.IsFinite(vertex.Z))
                {
                    throw new ValidationException($"Poly3 has invalid vertex: {vertex}.");
                }
            }
        }
        // check that points are co-planar
        if (this.vertices.Length > 3)
        {
            var normal = this.Plane();
            foreach (var vertex in this.vertices)
            {
                var dist = Math.Abs(normal.SignedDistanceToPoint(vertex));
                if (dist > C.EPS)
                {
                    throw new ValidationException($"Poly3 must be coplanar: vertex {vertex} distance {dist}.");
                }
            }
        }
    }
}