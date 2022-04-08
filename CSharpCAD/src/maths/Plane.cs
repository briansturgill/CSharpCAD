namespace CSharpCAD;

/// <summary>Represents a plane in 3D coordinate space as determined by a normal (perpendicular to the plane) and distance from 0,0,0.</summary>
public class Plane : IEquatable<Plane>
{
    /// <summary>Sides made of tuples of (Vec2, Vec2)</summary>
    public readonly Vec3 normal;
    public readonly double w;

    public Plane() { }

    /// <summary>Create a new plane from the given normal and origin values.</summary>
    public Plane(Vec3 normal, Vec3 origin)
    {
        this.normal = normal.Normalize();
        this.w = origin.Dot(this.normal);
    }

    /// <summary>Create a new plane from normal and w.</summary>
    public Plane(Vec3 normal, double w)
    {
        this.normal = normal;
        this.w = w;
    }

    /// <summary>Create a new plane by copying.</summary>
    public Plane(Plane p)
    {
        this.normal = p.normal;
        this.w = p.w;
    }

    /// <summary>Check if this geometry is equal to the given geometry.</summary>
    public bool Equals(Plane? gp)
    {
        if (gp is null) {
            return false;
        }
        return this.normal == gp.normal && this.w == gp.w;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public static bool operator ==(Plane a, Plane b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if this vector is not equal to the given vector.</summary>
    public static bool operator !=(Plane a, Plane b) => !(a == b);

    /// <summary>Standard C# override.</summary>
    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Plane v = (Plane)obj;
            return Equals(v);
        }
    }

    /// <summary>Standard C# override.</summary>
    public override int GetHashCode()
    {
        return normal.GetHashCode() ^ w.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
	public override string ToString()
    {
        return $"Plane(normal={normal},w={w})";
    }

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Plane p)
    {
        return this.normal.IsNearlyEqual(p.normal) && Math.Abs(this.w - p.w) < C.EPS;
    }

    /**
     * Flip the given plane.
     *
     * @param {plane} out - receiving plane
     * @param {plane} plane - plane to flip
     * @return {plane} out
     * @alias module:modeling/maths/plane.flip
     */
    public Plane Flip()
    {
        return new Plane(
            this.normal.Negate(),
            -this.w
        );
    }

    /**
     * Create a plane from the given 3 points.
     *
     * @param {plane} out - receiving plane
     * @param {Array} vertices - points on the plane
     * @returns {plane} out
     * @alias module:modeling/maths/plane.fromPoints
     */
    public static Plane From3Points(Vec3 a, Vec3 b, Vec3 c)
    {
        var normal = b.Subtract(a).Cross(c.Subtract(a)).Normalize(); // ba = ba x ca
        return new Plane(normal, normal.Dot(a));
    }

    /**
     * Create a plane from the given points.
     *
     * @param {plane} out - receiving plane
     * @param {Array} vertices - points on the plane
     * @returns {plane} out
     * @alias module:modeling/maths/plane.fromPoints
     */
    public static Plane FromPoints(Vec3[] vertices)
    {
        var len = vertices.Length;

        // Calculate normal vector for a single vertex
        // Inline to avoid allocations
        Vec3 vertexNormal(int index)
        {
            var a = vertices[index];
            var b = vertices[(index + 1) % len];
            var c = vertices[(index + 2) % len];
            var ba = b.Subtract(a); // ba = b - a
            return b.Subtract(a).Cross(c.Subtract(a)).Normalize(); // ba = ba x ca
        }

        var ret = new Vec3();
        if (len == 3)
        {
            // optimization for triangles, which are always coplanar
            ret = vertexNormal(0);
        }
        else
        {
            // sum of vertex normals
            for (var i = 0; i < len; i++)
            {
                ret = ret.Add(vertexNormal(i));
            }
            // renormalize normal vector
            ret = ret.Normalize();
        }
        return new Plane(ret, ret.Dot(vertices[0]));

    }

    /**
     * <summary>
     * Create a new plane from the given points like fromPoints,
     * but allow the vectors to be on one point or one line.
     * In such a case, a random plane through the given points is constructed.
     * </summary>
     */
    public static Plane FromPointsRandom(Vec3 a, Vec3 b, Vec3 c)
    {
        var ba = b.Subtract(a);
        var ca = c.Subtract(a);
        if (ba.Length() < C.EPS)
        {
            ba = ca.Orthogonal();
        }
        if (ca.Length() < C.EPS)
        {
            ca = ba.Orthogonal();
        }
        var normal = ba.Cross(ca);
        if (normal.Length() < C.EPS)
        {
            // this would mean that ba == ca.negated()
            ca = ba.Orthogonal();
            normal = ba.Cross(ca);
        }
        normal = normal.Normalize();
        var w = normal.Dot(a);
        return new Plane(normal, w);
    }

    /// <summary>Used by Mat4.</summary>
    public (double, double, double, double) Points => (this.normal.x, this.normal.y, this.normal.z, this.w);

    /**
     * Project the given point on to the this plane.
     *
     * @param {plane} plane - plane of reference
     * @param {vec3} point - point of reference
     * @return {vec3} projected point on plane
     * @alias module:modeling/maths/plane.projectionOfPoint
     */
    public Vec3 ProjectionOfPoint(Vec3 point)
    {
        var a = point.x * this.normal.x + point.y * this.normal.y + point.z * this.normal.z - this.w;
        return new Vec3(
          point.x - a * this.normal.x,
          point.y - a * this.normal.y,
          point.z - a * this.normal.z
        );
    }

    /// <summary>Calculate the distance to the given point.</summary>
    public double SignedDistanceToPoint(Vec3 gp) => this.normal.Dot(gp) - this.w;


    /// <summary>Transform the given plane using the given matrix</summary
    public Plane Transform(Mat4 matrix)
    {
        // get two vectors in the plane:
        var r = this.normal.Orthogonal();
        var u = this.normal.Cross(r);
        var v = this.normal.Cross(u);
        // get 3 points in the plane:
        var point1 = new Vec3(this.w).Multiply(this.normal);
        var point2 = point1.Add(u);
        var point3 = point1.Add(v);
        // transform the points:
        point1 = point1.Transform(matrix);
        point2 = point2.Transform(matrix);
        point3 = point3.Transform(matrix);
        // and create a new plane from the transformed points:
        var p = From3Points(point1, point2, point3);
        if (matrix.IsMirroring())
        {
            p = p.Flip();
        }
        return p;
    }

}