namespace CSharpCAD;

/// <summary>Represents a plane in 3D coordinate space as determined by a normal (perpendicular to the plane) and distance from 0,0,0.</summary>
public class Plane : IEquatable<Plane>
{
    /// <summary>Vector normal to the plane.</summary>
    public readonly Vec3 Normal;

    /// <summary>Mysterious entity, akin to + C in integrals.</summary>
    /// <remarks>
    ///   More seriously, W seems to just be any point in the plane, not colinear with normal.
    ///   We could call it NonColinearPoint, but that would make things too easy.
    /// </remarks>
    public readonly double W;

    ///<summary>Construct an empty Plane.</summary>
    public Plane() { }

    /// <summary>Create a new plane from the given normal and origin values.</summary>
    public Plane(Vec3 normal, Vec3 origin)
    {
        this.Normal = normal.Normalize();
        this.W = origin.Dot(this.Normal);
    }

    /// <summary>Create a new plane from normal and w.</summary>
    public Plane(Vec3 normal, double w)
    {
        this.Normal = normal;
        this.W = w;
    }

    /// <summary>Create a new plane by copying.</summary>
    public Plane(Plane p)
    {
        this.Normal = p.Normal;
        this.W = p.W;
    }

    /// <summary>Check if this geometry is equal to the given geometry.</summary>
    public bool Equals(Plane? gp)
    {
        if (gp is null) {
            return false;
        }
        return this.Normal == gp.Normal && this.W == gp.W;
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
        return Normal.GetHashCode() ^ W.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
	public override string ToString()
    {
        return $"Plane(normal={Normal},w={W})";
    }

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Plane p)
    {
        return this.Normal.IsNearlyEqual(p.Normal) && Math.Abs(this.W - p.W) < C.EPS;
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
            this.Normal.Negate(),
            -this.W
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
        var a = point.X * this.Normal.X + point.Y * this.Normal.Y + point.Z * this.Normal.Z - this.W;
        return new Vec3(
          point.X - a * this.Normal.X,
          point.Y - a * this.Normal.Y,
          point.Z - a * this.Normal.Z
        );
    }

    /// <summary>Calculate the distance to the given point.</summary>
    public double SignedDistanceToPoint(Vec3 gp) => this.Normal.Dot(gp) - this.W;


    /// <summary>Transform the given plane using the given matrix</summary>
    public Plane Transform(Mat4 matrix)
    {
        // get two vectors in the plane:
        var r = this.Normal.Orthogonal();
        var u = this.Normal.Cross(r);
        var v = this.Normal.Cross(u);
        // get 3 points in the plane:
        var point1 = new Vec3(this.W).Multiply(this.Normal);
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