namespace CSharpCAD;

/// <summary>A vector of 2 coordinates.</summary>

public readonly struct Vec2 : IEquatable<Vec2>
{
    /// <summary>Coordinate.</summary>
    public readonly double x;
    /// <summary>Coordinate.</summary>
    public readonly double y;

    /// <summary>Construct from 2 coordinates.</summary>
    /// <remarks>With no arguments, construct a zero vector.</remarks>
    public Vec2(double x, double y)
    {
        this.x = x; this.y = y;
    }

    public readonly static Vec2 ZERO = new Vec2(0, 0);

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public bool Equals(Vec2 gv)
    {
        return this.x == gv.x && this.y == gv.y;
    }

    /// <summary>Check if this vector is equal to the given vector.</summary>
    public static bool operator ==(Vec2 a, Vec2 b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if this vector is not equal to the given vector.</summary>
    public static bool operator !=(Vec2 a, Vec2 b) => !(a == b);

    /// <summary>Standard C# override.</summary>
    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Vec2 v = (Vec2)obj;
            return Equals(v);
        }
    }

    /// <summary>Standard C# override.</summary>
    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
    public override string ToString() => $"Vec2({this.x:F5},{this.y:F5})";

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Vec2 b)
    {
        if (double.IsNaN(this.x) || double.IsNaN(this.y) ||
            double.IsNaN(b.x) || double.IsNaN(b.y))
        {
            return false;
        }
        if (double.IsInfinity(this.x) || double.IsInfinity(this.y) ||
            double.IsInfinity(b.x) || double.IsInfinity(b.y))
        {
            return false;
        }
        if (Math.Abs(this.x - b.x) >= C.EPS ||
            Math.Abs(this.y - b.y) >= C.EPS)
        {
            return false;
        }
        return true;
    }

    /// <summary>Returns the hypotenuse of the three points, avoiding unnecessary underflow/overflow.</summary>
    public static double Hypot(double _x, double _y)
    {
        var x = _x < 0.0 ? -_x : _x;
        var y = _y < 0.0 ? -_y : _y;
        var m = x > y ? x : y;
        if (m == 0.0)
        {
            return 0.0;
        }
        x /= m;
        y /= m;
        return m * Math.Sqrt(x * x + y * y);
    }

    /// <summary>Calculates the absolute coordinates of the given vector.</summary>
    public Vec2 Abs()
    {
        return new Vec2(
          Math.Abs(this.x),
          Math.Abs(this.y)
        );
    }

    /// <summary>Adds the coordinates of this and the given vector.<summary>
    public Vec2 Add(Vec2 gv)
    {
        return new Vec2(
          this.x + gv.x,
          this.y + gv.y
        );
    }

    /// <summary>Calculate the angle of the given vector in degrees.</summary>
    public double AngleDegrees() => this.AngleRadians() * 57.29577951308232;

    /// <summary>Calculate the angle of the given vector in radians.</summary>
    public double AngleRadians() => Math.Atan2(this.y, this.x); // y=sin, x=cos

    /// <summary>Computes the cross product (3D) of this and the given vector.</summary>
    public Vec3 Cross(Vec2 gv)
    {
        return new Vec3(
          0,
          0,
          this.x * gv.y - this.y * gv.x
        );
    }

    /// <summary>Calculates the distance between this and the given vector.</summary>
    public double Distance(Vec2 gv)
    {
        var x = gv.x - this.x;
        var y = gv.y - this.y;
        return Hypot(x, y);
    }

    /// <summary>Divides the coordinates of this and the given vector.</summary>
    public Vec2 Divide(Vec2 gv)
    {
        return new Vec2(
          this.x / gv.x,
          this.y / gv.y
        );
    }

    /// <summmary>Calculates the dot product of two vectors.</summary>
    public double Dot(Vec2 gv) => this.x * gv.x + this.y * gv.y;


    /// <summary>Create a new vector in the direction of the given angle.</summary>
    public static Vec2 FromAngleDegrees(double degrees) => FromAngleRadians(Math.PI * degrees / 180);

    /// <summary>Create a new vector in the direction of the given angle.</summary>
    public static Vec2 FromAngleRadians(double radians)
    {
        return new Vec2(
          Math.Cos(radians),
          Math.Sin(radians)
        );
    }

    /// <summary>Create a vector from a single scalar value.</summary>
    public static Vec2 FromScalar(double scalar)
    {
        return new Vec2(
          scalar,
          scalar
        );
    }

    /// <summary>Calculates the length of this vector.</summary>
    public double Length() => Hypot(this.x, this.y);

    /// <summary>Performs a linear interpolation between this and the given vector.</summary>
    public Vec2 Lerp(Vec2 gv, double t)
    {
        var ax = this.x;
        var ay = this.y;
        return new Vec2(
          ax + t * (gv.x - ax),
          ay + t * (gv.y - ay)
        );
    }

    /// <summary>Returns the maximum coordinates of this and a given vector.
    public Vec2 Max(Vec2 gv)
    {
        return new Vec2(
            Math.Max(this.x, gv.x),
            Math.Max(this.y, gv.y));
    }

    /// <summary>Returns the minimum coordinates of this and a given vector.
    public Vec2 Min(Vec2 gv)
    {
        return new Vec2(
            Math.Min(this.x, gv.x),
            Math.Min(this.y, gv.y));
    }


    /// <summary>Multiplies the coordinates of two vectors (A*B).</summary>
    public Vec2 Multiply(Vec2 gv)
    {
        return new Vec2(
          this.x * gv.x,
          this.y * gv.y
        );
    }

    /// <summary>Negates the coordinates of this vector.</summary>
    public Vec2 Negate() { return new Vec2(-this.x, -this.y); }

    /// <summary>Normalize this vector.</summary>
    public Vec2 Normalize()
    {
        var len = x * x + y * y;
        if (len > 0)
        {
            len = 1 / Math.Sqrt(len);
        }
        return new Vec2(
          x * len,
          y * len
        );
    }


    /// <summary>Calculates the normal of the given vector.</summary>
    /// <remarks>The normal value is the given vector rotated 90 degress.</remarks>
    public Vec2 Normal() => this.Rotate(new Vec2(0, 0), Math.PI / 2);

    /// <summary>Rotates the given vector by the given angle.</summary>
    public Vec2 Rotate(Vec2 origin, double radians)
    {
        var x = this.x - origin.x;
        var y = this.y - origin.y;
        var c = Math.Cos(radians);
        var s = Math.Sin(radians);

        return new Vec2(
          x * c - y * s + origin.x,
          x * s + y * c + origin.y
        );
    }

    /// <summary>Scales the coordinates of the given vector.</summary>
    public Vec2 Scale(double amount)
    {
        return new Vec2(
          this.x * amount,
          this.y * amount
        );
    }

    /// <summary>Snaps the coordinates of the given vector to the given epsilon.</summary>
    public Vec2 Snap(double epsilon)
    {
        return new Vec2(
          Math.Round(this.x / epsilon) * epsilon + 0,
          Math.Round(this.y / epsilon) * epsilon + 0
        );
    }

    /// <summary>Calculates the squared distance between this and the given vector.</summary>
    public double SquaredDistance(Vec2 gv)
    {
        var x = gv.x - this.x;
        var y = gv.y - this.y;
        return x * x + y * y;
    }

    /// <summary>Calculates the squared length of the given vector.</summary>
    public double SquaredLength()
    {
        var x = this.x;
        var y = this.y;
        return x * x + y * y;
    }

    /// <summary>Subtracts the coordinates of this and the given vector.</summary>
    public Vec2 Subtract(Vec2 gv)
    {
        return new Vec2(
          this.x - gv.x,
          this.y - gv.y
        );
    }


    /// <summary>Transforms the given vector using the given matrix.</summary>
    public Vec2 Transform(Mat4 matrix)
    {
        return new Vec2(
          matrix.D(0) * this.x + matrix.D(4) * this.y + matrix.D(12),
          matrix.D(1) * this.x + matrix.D(5) * this.y + matrix.D(13)
        );
    }
}
