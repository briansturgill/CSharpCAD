namespace CSharpCAD;

/// <summary>A vector of 3 coordinates.</summary>

public readonly struct Vec3 : IEquatable<Vec3>, IComparable
{
    /// <summary>Coordinate.</summary>
    public readonly double X;
    /// <summary>Coordinate.</summary>
    public readonly double Y;
    /// <summary>Coordinate.</summary>
    public readonly double Z;

    /// <summary>Construct from 3 coordinates.</summary>
    /// <remarks>With no arguments, construct a zero vector.</remarks>
    public Vec3(double x, double y, double z)
    {
        this.X = x; this.Y = y; this.Z = z;
    }

    /// <summary>Construct from the x and y of the Vec2 and the z argument.</summary>
    public Vec3(Vec2 v, double z)
    {
        this.X = v.X; this.Y = v.Y; this.Z = z;
    }

    /// <summary>Construct from a scalar repeated 3 times.</summary>
    public Vec3(double scalar)
    {
        this.X = scalar; this.Y = scalar; this.Z = scalar;
    }

    /// <summary>Compare this vector to another allowing IsNearlyEqual to mean equal.</summary>
    public int CompareTo(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return -1;
        }
        Vec3 gv = (Vec3)obj;
        if (this.IsNearlyEqual(gv))
        {
            return 0;
        }
        if (this.X < gv.X || this.Y < gv.Y || this.Z < gv.Z) return -1;
        return 1;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public bool Equals(Vec3 gv)
    {
        return this.X == gv.X && this.Y == gv.Y && this.Z == gv.Z;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public static bool operator ==(Vec3 a, Vec3 b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if this vector is not equal to the given vector.</summary>
    public static bool operator !=(Vec3 a, Vec3 b) => !(a == b);

    /// <summary>Automatically convert a tuple of 3 doubles to a Vec3.</summary>
    public static implicit operator Vec3((double, double, double) tuple)
    {
        var (x, y, z) = tuple;
        return new Vec3(x, y, z);
    }


    /// <summary>Standard C# override.</summary>
    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Vec3 v = (Vec3)obj;
            return Equals(v);
        }
    }

    /// <summary>Standard C# override.</summary>
    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
	public override string ToString() => $"Vec3({this.X},{this.Y},{this.Z})";

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Vec3 b)
    {
        if (double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z) ||
            double.IsNaN(b.X) || double.IsNaN(b.Y) || double.IsNaN(b.Z))
        {
            return false;
        }
        if (double.IsInfinity(this.X) || double.IsInfinity(this.Y) || double.IsInfinity(this.Z) ||
            double.IsInfinity(b.X) || double.IsInfinity(b.Y) || double.IsInfinity(b.Z))
        {
            return false;
        }
        if (Math.Abs(this.X - b.X) >= C.EPS ||
            Math.Abs(this.Y - b.Y) >= C.EPS ||
            Math.Abs(this.Z - b.Z) >= C.EPS)
        {
            return false;
        }
        return true;
    }

    /// <summary>Returns the hypotenuse of the three points. Intentionally optimizing for speed.</summary>
    public static double Hypot(double x, double y, double z)
    {
        return Math.Sqrt(x * x + y * y + z * z);
    }

    /// <summary>Returns the vector of the absolute value of this vector's coordinates.</summary>
    public Vec3 Abs()
    {
        return new Vec3(Math.Abs(this.X),
            Math.Abs(this.Y),
            Math.Abs(this.Z));
    }

    /// <summary>Returns the addition of this vector with the given vector.</summary>
    public Vec3 Add(Vec3 gv)
    {
        return new Vec3(this.X + gv.X,
            this.Y + gv.Y,
            this.Z + gv.Z);
    }

    /// <summary>Returns the angle between this and the given vector in radians.</summary>
    public double Angle(Vec3 gv)
    {
        var mag1 = Hypot(this.X, this.Y, this.Z);
        var mag2 = Hypot(gv.X, gv.Y, gv.Z);
        var mag = mag1 * mag2;
        double cosine = mag == 0.0 ? 0.0 : (this.Dot(gv) / mag);
        return Math.Acos(Math.Min(Math.Max(cosine, -1.0), 1.0));
    }

    /// <summary>Returns the cross product of this and the given vector.</summary>
    public Vec3 Cross(Vec3 gv)
    {
        var _x = this.Y * gv.Z - this.Z * gv.Y;
        var _y = this.Z * gv.X - this.X * gv.Z;
        var _z = this.X * gv.Y - this.Y * gv.X;
        return new Vec3(_x, _y, _z);
    }

    /// <summary>Returns the Euclidian distance between this and the given vector.</summary>
    public double Distance(Vec3 gv)
    {
        var _x = gv.X - this.X;
        var _y = gv.Y - this.Y;
        var _z = gv.Z - this.Z;
        return Hypot(_x, _y, _z);
    }

    /// <summary>Returns the vector containing the division of this vector by the given vector.</summary>
    public Vec3 Divide(Vec3 gv)
    {
        return new Vec3(this.X / gv.X,
            this.Y / gv.Y,
            this.Z / gv.Z);
    }

    /// <summary>Returns the dot product of this and the given vector.</summary>
    public double Dot(Vec3 gv) => this.X * gv.X + this.Y * gv.Y + this.Z * gv.Z;

    /// <summary>Returns the length of a vector.</summary>
    public double Length()
    {
        return Hypot(X, Y, Z);
    }

    /// <summary>Returns a linear interpolation between this and the given vector.</summary>
    public Vec3 Lerp(Vec3 gv, double interpolant)
    {
        return new Vec3(this.X + interpolant * (gv.X - this.X),
            this.Y + interpolant * (gv.Y - this.Y),
            this.Z + interpolant * (gv.Z - this.Z));
    }

    /// <summary>Returns the maximum coordinates of this and the given vector.</summary>
    public Vec3 Max(Vec3 gv)
    {
        return new Vec3(Math.Max(this.X, gv.X),
            Math.Max(this.Y, gv.Y),
            Math.Max(this.Z, gv.Z));
    }

    /// <summary>Returns the minimum coordinates of this and the given vector.</summary>
    public Vec3 Min(Vec3 gv)
    {
        return new Vec3(Math.Min(this.X, gv.X),
            Math.Min(this.Y, gv.Y),
            Math.Min(this.Z, gv.Z));
    }

    /// <summary>Returns the vector containing the multiplication of this vector by the given vector.</summary>
    public Vec3 Multiply(Vec3 gv)
    {
        return new Vec3(this.X * gv.X,
            this.Y * gv.Y,
            this.Z * gv.Z);
    }

    /// <summary>Returns the vector containing the negation of this vector.</summary>
    public Vec3 Negate()
    {
        return new Vec3(-this.X, -this.Y, -this.Z);
    }

    /// <summary>Returns the vector containing the normalization of this vector.</summary>
    public Vec3 Normalize()
    {
        var len = X * X + Y * Y + Z * Z;
        if (len > 0.0)
        {
            len = 1 / Math.Sqrt(len);
        }
        return new Vec3(this.X * len, this.Y * len, this.Z * len);
    }

    /// <summary>Create a vector orthogonal to this one.</summary>
    public Vec3 Orthogonal()
    {
        var bV = this.Abs();
        var _x = 0 + Convert.ToDouble((bV.X < bV.Y) && (bV.X < bV.Z));
        var _y = 0 + Convert.ToDouble((bV.Y <= bV.X) && (bV.Y < bV.Z));
        var _z = 0 + Convert.ToDouble((bV.Z <= bV.X) && (bV.Z <= bV.Y));

        return Cross(new Vec3(_x, _y, _z));
    }

    /// <summary>Rotate the this vector around the given origin, X axis only.</summary>
    public Vec3 RotateX(Vec3 origin, double radians)
    {
        // translate point to the origin
        var p = new Vec3(this.X - origin.X,
            this.Y - origin.Y,
            this.Z - origin.Z
        );

        // perform rotation
        var r = new Vec3(p.X,
            p.Y * CosR(radians) - p.Z * SinR(radians),
            p.Y * SinR(radians) + p.Z * CosR(radians)
        );

        // translate to correct position
        return new Vec3(r.X + origin.X,
            r.Y + origin.Y,
            r.Z + origin.Z
        );
    }

    /// <summary>Rotate the this vector around the given origin, Y axis only.</summary>
    public Vec3 RotateY(Vec3 origin, double radians)
    {

        // translate point to the origin
        var p = new Vec3(this.X - origin.X,
            this.Y - origin.Y,
            this.Z - origin.Z
        );

        // perform rotation
        var r = new Vec3(p.Z * SinR(radians) + p.X * CosR(radians),
            p.Y,
            p.Z * CosR(radians) - p.X * SinR(radians)
        );

        // translate to correct position
        return new Vec3(r.X + origin.X,
            r.Y + origin.Y,
            r.Z + origin.Z
        );
    }

    /// <summary>Rotate the this vector around the given origin, Z axis only.</summary>
    public Vec3 RotateZ(Vec3 origin, double radians)
    {
        // Translate point to the origin
        var p = new Vec2(this.X - origin.X,
            this.Y - origin.Y
        );

        // perform rotation
        var r = new Vec2((p.X * CosR(radians)) - (p.Y * SinR(radians)),
            (p.X * SinR(radians)) + (p.Y * CosR(radians))
        );

        // translate to correct position
        return new Vec3(r.X + origin.X,
            r.Y + origin.Y,
            this.Z
        );
    }

    /// <summary>Scales the coordinates of this vector by a scalar number.</summary>
    public Vec3 Scale(double amount)
    {
        return new Vec3(this.X * amount,
            this.Y * amount,
            this.Z * amount
        );
    }

    /// <summary>Snaps the coordinates of this vector to the given epsilon.</summary>
    public Vec3 Snap(double epsilon)
    {
        return new Vec3(Math.Round(this.X / epsilon) * epsilon + 0,
            Math.Round(this.Y / epsilon) * epsilon + 0,
            Math.Round(this.Z / epsilon) * epsilon + 0
        );
    }

    /// <summary>Calculates the squared distance between two vectors.</summary>
    public double SquaredDistance(Vec3 gv)
    {
        var x = gv.X - this.X;
        var y = gv.Y - this.Y;
        var z = gv.Z - this.Z;

        return x * x + y * y + z * z;
    }

    /// <summary>Calculates the squared length of the given vector.</summary>
    public double SquaredLength()
    {
        var x = this.X;
        var y = this.Y;
        var z = this.Z;

        return x * x + y * y + z * z;
    }

    /// <summary>Subtracts the coordinates of this vector and the given vector.</summary>
    public Vec3 Subtract(Vec3 gv)
    {
        return new Vec3(this.X - gv.X,
            this.Y - gv.Y,
            this.Z - gv.Z
        );
    }

    /// <summary>Transforms this vector using the given matrix.</summary>
    public Vec3 Transform(Mat4 gm)
    {
        var w = gm.D(3) * X + gm.D(7) * Y + gm.D(11) * Z + gm.D(15);
        if (w == 0.0)
        {
            w = 1.0;
        }
        return new Vec3(
            (gm.D(0) * X + gm.D(4) * Y + gm.D(8) * Z + gm.D(12)) / w,
            (gm.D(1) * X + gm.D(5) * Y + gm.D(9) * Z + gm.D(13)) / w,
            (gm.D(2) * X + gm.D(6) * Y + gm.D(10) * Z + gm.D(14)) / w);
    }
}