namespace CSharpCAD;

/// <summary>A vector of 3 coordinates.</summary>

public readonly struct Vec3 : IEquatable<Vec3>
{
    /// <summary>Coordinate.</summary>
    public readonly double x;
    /// <summary>Coordinate.</summary>
    public readonly double y;
    /// <summary>Coordinate.</summary>
    public readonly double z;

    /// <summary>Construct from 3 coordinates.</summary>
    /// <remarks>With no arguments, construct a zero vector.</remarks>
    public Vec3(double x, double y, double z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    /// <summary>Construct from the x and y of the Vec2 and the z argument.</summary>
    public Vec3(Vec2 v, double z)
    {
        this.x = v.x; this.y = v.y; this.z = z;
    }

    /// <summary>Construct from a scalar repeated 3 times.</summary>
    public Vec3(double scalar)
    {
        this.x = scalar; this.y = scalar; this.z = scalar;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public bool Equals(Vec3 gv)
    {
        return this.x == gv.x && this.y == gv.y && this.z == gv.z;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public static bool operator ==(Vec3 a, Vec3 b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if this vector is not equal to the given vector.</summary>
    public static bool operator !=(Vec3 a, Vec3 b) => !(a == b);

    /// <summary>Automatically convert a tuple of 3 doubles to a Vec3.</summary>
    public static implicit operator Vec3((double, double, double ) tuple) {
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
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
	public override string ToString() => $"Vec3({this.x},{this.y},{this.z})";

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Vec3 b)
    {
        if (double.IsNaN(this.x) || double.IsNaN(this.y) || double.IsNaN(this.z) ||
            double.IsNaN(b.x) || double.IsNaN(b.y) || double.IsNaN(b.z))
        {
            return false;
        }
        if (double.IsInfinity(this.x) || double.IsInfinity(this.y) || double.IsInfinity(this.z) ||
            double.IsInfinity(b.x) || double.IsInfinity(b.y) || double.IsInfinity(b.z))
        {
            return false;
        }
        if (Math.Abs(this.x - b.x) >= C.EPS ||
            Math.Abs(this.y - b.y) >= C.EPS ||
            Math.Abs(this.z - b.z) >= C.EPS)
        {
            return false;
        }
        return true;
    }

    /// <summary>Returns the hypotenuse of the three points, avoiding unnecessary underflow/overflow.</summary>
    public static double Hypot(double _x, double _y, double _z)
    {
        var x = _x < 0.0 ? -_x : _x;
        var y = _y < 0.0 ? -_y : _y;
        var z = _z < 0.0 ? -_z : _z;
        var m = x > y ? x : y;
        m = m > z ? m : z;
        if (m == 0.0)
        {
            return 0.0;
        }
        x /= m;
        y /= m;
        z /= m;
        return m * Math.Sqrt(x * x + y * y + z * z);
    }

    /// <summary>Returns the vector of the absolute value of this vector's coordinates.</summary>
    public Vec3 Abs()
    {
        return new Vec3(Math.Abs(this.x),
            Math.Abs(this.y),
            Math.Abs(this.z));
    }

    /// <summary>Returns the addition of this vector with the given vector.</summary>
    public Vec3 Add(Vec3 gv)
    {
        return new Vec3(this.x + gv.x,
            this.y + gv.y,
            this.z + gv.z);
    }

    /// <summary>Returns the angle between this and the given vector in radians.</summary>
    public double Angle(Vec3 gv)
    {
        var mag1 = Hypot(this.x, this.y, this.z);
        var mag2 = Hypot(gv.x, gv.y, gv.z);
        var mag = mag1 * mag2;
        double cosine = mag == 0.0 ? 0.0 : (this.Dot(gv) / mag);
        return Math.Acos(Math.Min(Math.Max(cosine, -1.0), 1.0));
    }

    /// <summary>Returns the cross product of this and the given vector.</summary>
    public Vec3 Cross(Vec3 gv)
    {
        var _x = this.y * gv.z - this.z * gv.y;
        var _y = this.z * gv.x - this.x * gv.z;
        var _z = this.x * gv.y - this.y * gv.x;
        return new Vec3(_x, _y, _z);
    }

    /// <summary>Returns the Euclidian distance between this and the given vector.</summary>
    public double Distance(Vec3 gv)
    {
        var _x = gv.x - this.x;
        var _y = gv.y - this.y;
        var _z = gv.z - this.z;
        return Hypot(_x, _y, _z);
    }

    /// <summary>Returns the vector containing the division of this vector by the given vector.</summary>
    public Vec3 Divide(Vec3 gv)
    {
        return new Vec3(this.x / gv.x,
            this.y / gv.y,
            this.z / gv.z);
    }

    /// <summary>Returns the dot product of this and the given vector.</summary>
    public double Dot(Vec3 gv) => this.x * gv.x + this.y * gv.y + this.z * gv.z;

    /// <summary>Returns the length of a vector.</summary>
    public double Length()
    {
        return Hypot(x, y, z);
    }

    /// <summary>Returns a linear interpolation between this and the given vector.</summary>
    public Vec3 Lerp(Vec3 gv, double interpolant)
    {
        return new Vec3(this.x + interpolant * (gv.x - this.x),
            this.y + interpolant * (gv.y - this.y),
            this.z + interpolant * (gv.z - this.z));
    }

    /// <summary>Returns the maximum coordinates of this and the given vector.</summary>
    public Vec3 Max(Vec3 gv)
    {
        return new Vec3(Math.Max(this.x, gv.x),
            Math.Max(this.y, gv.y),
            Math.Max(this.z, gv.z));
    }

    /// <summary>Returns the minimum coordinates of this and the given vector.</summary>
    public Vec3 Min(Vec3 gv)
    {
        return new Vec3(Math.Min(this.x, gv.x),
            Math.Min(this.y, gv.y),
            Math.Min(this.z, gv.z));
    }

    /// <summary>Returns the vector containing the multiplication of this vector by the given vector.</summary>
    public Vec3 Multiply(Vec3 gv)
    {
        return new Vec3(this.x * gv.x,
            this.y * gv.y,
            this.z * gv.z);
    }

    /// <summary>Returns the vector containing the negation of this vector.</summary>
    public Vec3 Negate()
    {
        return new Vec3(-this.x, -this.y, -this.z);
    }

    /// <summary>Returns the vector containing the normalization of this vector.</summary>
    public Vec3 Normalize()
    {
        var len = x * x + y * y + z * z;
        if (len > 0.0)
        {
            len = 1 / Math.Sqrt(len);
        }
        return new Vec3(this.x * len, this.y * len, this.z * len);
    }

    /// <summary>Create a vector orthogonal to this one.</summary>
    public Vec3 Orthogonal()
    {
        var bV = this.Abs();
        var _x = 0 + Convert.ToDouble((bV.x < bV.y) && (bV.x < bV.z));
        var _y = 0 + Convert.ToDouble((bV.y <= bV.x) && (bV.y < bV.z));
        var _z = 0 + Convert.ToDouble((bV.z <= bV.x) && (bV.z <= bV.y));

        return Cross(new Vec3(_x, _y, _z));
    }

    /// <summary>Rotate the this vector around the given origin, X axis only.</summary>
    public Vec3 RotateX(Vec3 origin, double radians)
    {
        // translate point to the origin
        var p = new Vec3(this.x - origin.x,
            this.y - origin.y,
            this.z - origin.z
        );

        // perform rotation
        var r = new Vec3(p.x,
            p.y * Math.Cos(radians) - p.z * Math.Sin(radians),
            p.y * Math.Sin(radians) + p.z * Math.Cos(radians)
        );

        // translate to correct position
        return new Vec3(r.x + origin.x,
            r.y + origin.y,
            r.z + origin.z
        );
    }

    /// <summary>Rotate the this vector around the given origin, Y axis only.</summary>
    public Vec3 RotateY(Vec3 origin, double radians)
    {

        // translate point to the origin
        var p = new Vec3(this.x - origin.x,
            this.y - origin.y,
            this.z - origin.z
        );

        // perform rotation
        var r = new Vec3(p.z * Math.Sin(radians) + p.x * Math.Cos(radians),
            p.y,
            p.z * Math.Cos(radians) - p.x * Math.Sin(radians)
        );

        // translate to correct position
        return new Vec3(r.x + origin.x,
            r.y + origin.y,
            r.z + origin.z
        );
    }

    /// <summary>Rotate the this vector around the given origin, Z axis only.</summary>
    public Vec3 RotateZ(Vec3 origin, double radians)
    {
        // Translate point to the origin
        var p = new Vec2(this.x - origin.x,
            this.y - origin.y
        );

        // perform rotation
        var r = new Vec2((p.x * Math.Cos(radians)) - (p.y * Math.Sin(radians)),
            (p.x * Math.Sin(radians)) + (p.y * Math.Cos(radians))
        );

        // translate to correct position
        return new Vec3(r.x + origin.x,
            r.y + origin.y,
            this.z
        );
    }

    /// <summary>Scales the coordinates of this vector by a scalar number.</summary>
    public Vec3 Scale(double amount)
    {
        return new Vec3(this.x * amount,
            this.y * amount,
            this.z * amount
        );
    }

    /// <summary>Snaps the coordinates of this vector to the given epsilon.</summary>
    public Vec3 Snap(double epsilon)
    {
        return new Vec3(Math.Round(this.x / epsilon) * epsilon + 0,
            Math.Round(this.y / epsilon) * epsilon + 0,
            Math.Round(this.z / epsilon) * epsilon + 0
        );
    }

    /// <summary>Calculates the squared distance between two vectors.</summary>
    public double SquaredDistance(Vec3 gv)
    {
        var x = gv.x - this.x;
        var y = gv.y - this.y;
        var z = gv.z - this.z;

        return x * x + y * y + z * z;
    }

    /// <summary>Calculates the squared length of the given vector.</summary>
    public double SquaredLength()
    {
        var x = this.x;
        var y = this.y;
        var z = this.z;

        return x * x + y * y + z * z;
    }

    /// <summary>Subtracts the coordinates of this vector and the given vector.</summary>
    public Vec3 Subtract(Vec3 gv)
    {
        return new Vec3(this.x - gv.x,
            this.y - gv.y,
            this.z - gv.z
        );
    }

    /// <summary>Transforms this vector using the given matrix.</summary>
    public Vec3 Transform(Mat4 gm)
    {
        var w = gm.D(3) * x + gm.D(7) * y + gm.D(11) * z + gm.D(15);
        if (w == 0.0)
        {
            w = 1.0;
        }
        return new Vec3(
            (gm.D(0) * x + gm.D(4) * y + gm.D(8) * z + gm.D(12)) / w,
            (gm.D(1) * x + gm.D(5) * y + gm.D(9) * z + gm.D(13)) / w,
            (gm.D(2) * x + gm.D(6) * y + gm.D(10) * z + gm.D(14)) / w);
    }
}