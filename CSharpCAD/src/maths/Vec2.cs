namespace CSharpCAD;

/// <summary>A vector of 2 coordinates.</summary>

public readonly struct Vec2 : IEquatable<Vec2>
{
    /// <summary>Coordinate.</summary>
    public readonly double X;
    /// <summary>Coordinate.</summary>
    public readonly double Y;

    /// <summary>Construct from 2 coordinates.</summary>
    /// <remarks>With no arguments, construct a zero vector.</remarks>
    public Vec2(double x, double y)
    {
        this.X = x; this.Y = y;
    }

    ///
    public bool Equals(Vec2 gv)
    {
        return this.X == gv.X && this.Y == gv.Y;
    }

    ///
    public static bool operator ==(Vec2 a, Vec2 b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if this vector is not equal to the given vector.</summary>
    public static bool operator !=(Vec2 a, Vec2 b) => !(a == b);

    /// <summary>Automatically convert a tuple of 2 doubles to a Vec2.</summary>
    public static implicit operator Vec2((double, double) tuple)
    {
        var (x, y) = tuple;
        return new Vec2(x, y);
    }

    ///
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
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
    public override string ToString() => $"Vec2({this.X:F5},{this.Y:F5})";

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Vec2 b)
    {
        if (double.IsNaN(this.X) || double.IsNaN(this.Y) ||
            double.IsNaN(b.X) || double.IsNaN(b.Y))
        {
            return false;
        }
        if (double.IsInfinity(this.X) || double.IsInfinity(this.Y) ||
            double.IsInfinity(b.X) || double.IsInfinity(b.Y))
        {
            return false;
        }
        if (Math.Abs(this.X - b.X) >= C.EPS ||
            Math.Abs(this.Y - b.Y) >= C.EPS)
        {
            return false;
        }
        return true;
    }

    /// <summary>Returns the hypotenuse of the two points. Intentionally optimizing for speed.</summary>
    public static double Hypot(double x, double y)
    {
        return Math.Sqrt(x * x + y * y);
    }

    /// <summary>Calculates the absolute coordinates of the given vector.</summary>
    public Vec2 Abs()
    {
        return new Vec2(
          Math.Abs(this.X),
          Math.Abs(this.Y)
        );
    }

    /// <summary>Adds the coordinates of this and the given vector.</summary>
    public Vec2 Add(Vec2 gv)
    {
        return new Vec2(
          this.X + gv.X,
          this.Y + gv.Y
        );
    }

    /// <summary>Calculate the angle of the given vector in degrees.</summary>
    public double AngleDegrees() => this.AngleRadians() * 57.29577951308232;

    /// <summary>Calculate the angle of the given vector in radians.</summary>
    public double AngleRadians() => Math.Atan2(this.Y, this.X); // y=sin, x=cos

    /// <summary>Computes the cross product (3D) of this and the given vector.</summary>
    public Vec3 Cross(Vec2 gv)
    {
        return new Vec3(
          0,
          0,
          this.X * gv.Y - this.Y * gv.X
        );
    }

    /// <summary>Calculates the distance between this and the given vector.</summary>
    public double Distance(Vec2 gv)
    {
        var x = gv.X - this.X;
        var y = gv.Y - this.Y;
        return Hypot(x, y);
    }

    /// <summary>Divides the coordinates of this and the given vector.</summary>
    public Vec2 Divide(Vec2 gv)
    {
        return new Vec2(
          this.X / gv.X,
          this.Y / gv.Y
        );
    }

    /// <summary>Calculates the dot product of two vectors.</summary>
    public double Dot(Vec2 gv) => this.X * gv.X + this.Y * gv.Y;


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
    public double Length() => Hypot(this.X, this.Y);

    /// <summary>Performs a linear interpolation between this and the given vector.</summary>
    public Vec2 Lerp(Vec2 gv, double t)
    {
        var ax = this.X;
        var ay = this.Y;
        return new Vec2(
          ax + t * (gv.X - ax),
          ay + t * (gv.Y - ay)
        );
    }

    /// <summary>Returns the maximum coordinates of this and a given vector.</summary>
    public Vec2 Max(Vec2 gv)
    {
        return new Vec2(
            Math.Max(this.X, gv.X),
            Math.Max(this.Y, gv.Y));
    }

    /// <summary>Returns the minimum coordinates of this and a given vector.</summary>
    public Vec2 Min(Vec2 gv)
    {
        return new Vec2(
            Math.Min(this.X, gv.X),
            Math.Min(this.Y, gv.Y));
    }


    /// <summary>Multiplies the coordinates of two vectors (A*B).</summary>
    public Vec2 Multiply(Vec2 gv)
    {
        return new Vec2(
          this.X * gv.X,
          this.Y * gv.Y
        );
    }

    /// <summary>Negates the coordinates of this vector.</summary>
    public Vec2 Negate() { return new Vec2(-this.X, -this.Y); }

    /// <summary>Normalize this vector.</summary>
    public Vec2 Normalize()
    {
        var len = X * X + Y * Y;
        if (len > 0)
        {
            len = 1 / Math.Sqrt(len);
        }
        return new Vec2(
          X * len,
          Y * len
        );
    }


    /// <summary>Calculates the normal of the given vector.</summary>
    /// <remarks>The normal value is the given vector rotated 90 degress.</remarks>
    public Vec2 Normal() => this.Rotate(new Vec2(0, 0), Math.PI / 2);

    /// <summary>Rotates the given vector by the given angle.</summary>
    public Vec2 Rotate(Vec2 origin, double radians)
    {
        var x = this.X - origin.X;
        var y = this.Y - origin.Y;
        var c = Math.Cos(radians);
        var s = Math.Sin(radians);

        return new Vec2(
          x * c - y * s + origin.X,
          x * s + y * c + origin.Y
        );
    }

    /// <summary>Scales the coordinates of the given vector.</summary>
    public Vec2 Scale(double amount)
    {
        return new Vec2(
          this.X * amount,
          this.Y * amount
        );
    }

    /// <summary>Snaps the coordinates of the given vector to the given epsilon.</summary>
    public Vec2 Snap(double epsilon)
    {
        return new Vec2(
          Math.Round(this.X / epsilon) * epsilon + 0,
          Math.Round(this.Y / epsilon) * epsilon + 0
        );
    }

    /// <summary>Calculates the squared distance between this and the given vector.</summary>
    public double SquaredDistance(Vec2 gv)
    {
        var x = gv.X - this.X;
        var y = gv.Y - this.Y;
        return x * x + y * y;
    }

    /// <summary>Calculates the squared length of the given vector.</summary>
    public double SquaredLength()
    {
        var x = this.X;
        var y = this.Y;
        return x * x + y * y;
    }

    /// <summary>Subtracts the coordinates of this and the given vector.</summary>
    public Vec2 Subtract(Vec2 gv)
    {
        return new Vec2(
          this.X - gv.X,
          this.Y - gv.Y
        );
    }


    /// <summary>Transforms the given vector using the given matrix.</summary>
    public Vec2 Transform(Mat4 matrix)
    {
        return new Vec2(
          matrix.D(0) * this.X + matrix.D(4) * this.Y + matrix.D(12),
          matrix.D(1) * this.X + matrix.D(5) * this.Y + matrix.D(13)
        );
    }
}