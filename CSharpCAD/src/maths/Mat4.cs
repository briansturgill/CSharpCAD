namespace CSharpCAD;

/// <summary>A 4x4 matrix.</summary>

public class Mat4 : IEquatable<Mat4>
{
    // <summary>Data.</summary>
    private readonly double[] d;

    /// <summary>Readonly access to data array for this matrix.</summary>
    public double D(int index) { return d[index]; }

    /// <summary>Construct an identity matrix.</summary>
    public Mat4() : this(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1) {}

    /// <summary>Construct from 16 values.</summary>
    public Mat4(double d0, double d1, double d2, double d3,
      double d4, double d5, double d6, double d7,
      double d8, double d9, double d10, double d11,
      double d12, double d13, double d14, double d15)
    {
        this.d = new double[4*4]{d0,d1,d2,d3,d4,d5,d6,d7,d8,d9,d10,d11,d12,d13,d14,d15};
    }

    /// <summary>Construct a copy of a Mat4.</summary>
    public Mat4(Mat4 m)
    {
        this.d = new double[4*4];
        m.d.CopyTo(this.d, 0);
    }

    /// <summary>Check if this matrix is equal to the given matrix.</summary>
    public bool Equals(Mat4? gm)
    {
        if (gm is null) {
            return false;
        }
        return this.d.SequenceEqual<double>(gm.d);
    }

    /// <summary>Check if the two matricies are equal.</summary>
    public static bool operator ==(Mat4 a, Mat4 b)
    {
        return a.Equals(b);
    }

    /// <summary>Check if the two matricies are not equal.</summary>
    public static bool operator !=(Mat4 a, Mat4 b) => !(a == b);

    /// <summary>Standard C# override.</summary>
    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Mat4 m = (Mat4)obj;
            return Equals(m);
        }
    }

    /// <summary>Standard C# override.</summary>
    public override int GetHashCode()
    {
        return this.d[0].GetHashCode() ^
          this.d[1].GetHashCode() ^
          this.d[2].GetHashCode() ^
          this.d[3].GetHashCode() ^
          this.d[4].GetHashCode() ^
          this.d[5].GetHashCode() ^
          this.d[6].GetHashCode() ^
          this.d[7].GetHashCode() ^
          this.d[8].GetHashCode() ^
          this.d[9].GetHashCode() ^
          this.d[10].GetHashCode() ^
          this.d[11].GetHashCode() ^
          this.d[12].GetHashCode() ^
          this.d[13].GetHashCode() ^
          this.d[14].GetHashCode() ^
          this.d[15].GetHashCode();
    }

    /// <summary>Standard C# override.</summary>
    public override string ToString()
    {
        string ret = "";
        foreach (var i in d)
        {
            if (ret != "")
            {
                ret += ",";
            }
            ret += i.ToString();
        }
        return ret;
    }

    /// <summary>Used mostly for testing.</summary>
    public bool IsNearlyEqual(Mat4 b)
    {
        for (var i = 0; i < this.d.Length; i++)
        {
            if (double.IsNaN(this.d[i]) || double.IsNaN(b.d[i]))
            {
                return false;
            }
            if (double.IsInfinity(this.d[i]) || double.IsInfinity(b.d[i]))
            {
                return false;
            }
            var v = Math.Abs(this.d[i] - b.d[i]);
            if (v >= C.EPS)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Returns the addition of this matrix with the given matrix.</summary>
    public Mat4 Add(Mat4 gm)
    {
        return new Mat4(
            this.d[0] + gm.d[0],
            this.d[1] + gm.d[1],
            this.d[2] + gm.d[2],
            this.d[3] + gm.d[3],
            this.d[4] + gm.d[4],
            this.d[5] + gm.d[5],
            this.d[6] + gm.d[6],
            this.d[7] + gm.d[7],
            this.d[8] + gm.d[8],
            this.d[9] + gm.d[9],
            this.d[10] + gm.d[10],
            this.d[11] + gm.d[11],
            this.d[12] + gm.d[12],
            this.d[13] + gm.d[13],
            this.d[14] + gm.d[14],
            this.d[15] + gm.d[15]);
    }

    /// <summary>Return a full clone of this matrix.</summary>
    public Mat4 Clone() {
        return new Mat4(this);
    }

    /**
      * <summary>Creates a matrix from a given angle around a given axis.</summary>
      *
      * <remarks>
      * This is equivalent to (but much faster than):
      *
      *     mat4.identity(dest)
      *     mat4.rotate(dest, dest, rad, axis)
      *
      * </remarks>
      *
      * <example>
      * var matrix = FromRotation(Math.PI / 2, new Vec3(0, 0, 3));
      * </example>
      **/
    public static Mat4 FromRotation(double rad, Vec3 axis)
    {
        var x = axis.x;
        var y = axis.y;
        var z = axis.z;
        var len = axis.Length();

        if (Math.Abs(len) < C.EPS)
        {
            // axis is 0,0,0 or almost
            return new Mat4();
        }

        len = 1 / len;
        x *= len;
        y *= len;
        z *= len;

        var s = Math.Sin(rad);
        var c = Math.Cos(rad);
        var t = 1 - c;

        // Perform rotation-specific matrix multiplication
        return new Mat4(
            x * x * t + c,
            y * x * t + z * s,
            z * x * t - y * s,
            0,
            x * y * t - z * s,
            y * y * t + c,
            z * y * t + x * s,
            0,
            x * z * t + y * s,
            y * z * t - x * s,
            z * z * t + c,
            0,
            0,
            0,
            0,
            1);
    }

    /**
      * <summary>Creates a matrix from a vector scaling.</summary>
      *
      * <remarks>
      * This is equivalent to (but much faster than):
      *
      *     mat4.identity(dest)
      *     mat4.scale(dest, dest, vec)
      * </remarks>
      *
      * <example>
      * var matrix = FromScaling(new Vec3(1, 2, 0.5));
      * </example>
      **/
    public static Mat4 FromScaling(Vec3 v)
    {
        return new Mat4(
          v.x,
          0,
          0,
          0,
          0,
          v.y,
          0,
          0,
          0,
          0,
          v.z,
          0,
          0,
          0,
          0,
          1);
    }

    /**
      * <summary>Creates a matrix from the given Tait–Bryan angles.</summary>
      *
      * <param name="yaw">Z rotation in radians.</param>
      * <param name="pitch">Y rotation in radians.</param>
      * <param name="roll">X rotation in radians.</param>
      *
      * <remarks>
      * Tait-Bryan Euler angle convention using active, intrinsic rotations around the axes in the order z-y-x.
      * @see https://en.wikipedia.org/wiki/Euler_angles
      * </remarks>
      *
      * <example>
      * var matrix = FromTaitBryanRotation(Math.PI/2, 0, Math.PI);
      * </example>
      **/
    public static Mat4 FromTaitBryanRotation(double yaw, double pitch, double roll)
    {
        // precompute sines and cosines of Euler angles
        var sy = Math.Sin(yaw);
        var cy = Math.Cos(yaw);
        var sp = Math.Sin(pitch);
        var cp = Math.Cos(pitch);
        var sr = Math.Sin(roll);
        var cr = Math.Cos(roll);

        // create and populate rotation matrix
        // left-hand-rule rotation
        // var els = [
        //  cp*cy, sr*sp*cy - cr*sy, sr*sy + cr*sp*cy, 0,
        //  cp*sy, cr*cy + sr*sp*sy, cr*sp*sy - sr*cy, 0,
        //  -sp, sr*cp, cr*cp, 0,
        //  0, 0, 0, 1
        // ]
        // right-hand-rule rotation
        return new Mat4(
          cp * cy, cp * sy, -sp, 0,
          sr * sp * cy - cr * sy, cr * cy + sr * sp * sy, sr * cp, 0,
          sr * sy + cr * sp * cy, cr * sp * sy - sr * cy, cr * cp, 0,
          0, 0, 0, 1
        );
    }

    /**
      * <summary>Creates a matrix from a vector translation.</summary>
      *
      * <param name="vec">Offset (vector) of translation.</param>
      *
      * <remarks>
      * This is equivalent to (but much faster than):
      *
      *     mat4.identity(dest)
      *     mat4.translate(dest, dest, vec)
      * </remarks>
      *
      * <example>
      * var matrix = FromTranslation(new Vec3(1, 2, 3));
      * </example>
      **/
    public static Mat4 FromTranslation(Vec3 vec)
    {
        return new Mat4(
          1,
          0,
          0,
          0,
          0,
          1,
          0,
          0,
          0,
          0,
          1,
          0,
          vec.x,
          vec.y,
          vec.z,
          1);
    }

    /**
      * <summary>Creates a matrix from a vector scaling.</summary>
      *
      * <remarks>
      * Each vector must be a directional vector with a length greater then zero.
      * @see https://gist.github.com/kevinmoran/b45980723e53edeb8a5a43c49f134724
      * </remarks>
      *
      * <example>
      * var matrix = FromTranslation(new Vec3(1, 2, 3));
      * </example>
      **/
    public static Mat4 FromVectorRotation(Vec3 source, Vec3 target)
    {
        var sourceNormal = source.Normalize();
        var targetNormal = target.Normalize();

        var axis = targetNormal.Cross(sourceNormal);
        var cosA = targetNormal.Dot(sourceNormal);
        if (cosA == -1.0)
        {
            return Mat4.FromRotation(Math.PI, sourceNormal.Orthogonal());
        }
        var k = 1 / (1 + cosA);

        return new Mat4(
          (axis.x * axis.x * k) + cosA,
          (axis.y * axis.x * k) - axis.z,
          (axis.z * axis.x * k) + axis.y,
          0,

          (axis.x * axis.y * k) + axis.z,
          (axis.y * axis.y * k) + cosA,
          (axis.z * axis.y * k) - axis.x,
          0,

          (axis.x * axis.z * k) - axis.y,
          (axis.y * axis.z * k) + axis.x,
          (axis.z * axis.z * k) + cosA,
          0,

          0,
          0,
          0,
          1);
    }

    /**
      * <summary>Creates a matrix from the given angle around the X axis.</summary>
      *
      * <remarks>
      * This is equivalent to (but much faster than):
      *
      *     mat4.identity(dest)
      *     mat4.rotateX(dest, dest, radians)
      * </remarks>
      *
      * <example>
      * var matrix = FromXRotation(Math.PI / 2));
      * </example>
      **/
    public static Mat4 FromXRotation(double radians)
    {
        var s = Math.Sin(radians);
        var c = Math.Cos(radians);
        return new Mat4(
            1,
            0,
            0,
            0,
            0,
            c,
            s,
            0,
            0,
            -s,
            c,
            0,
            0,
            0,
            0,
            1);
    }

    /**
      * <summary>Creates a matrix from the given angle around the Y axis.</summary>
      *
      * <remarks>
      * This is equivalent to (but much faster than):
      *
      *     mat4.identity(dest)
      *     mat4.rotateY(dest, dest, radians)
      * </remarks>
      *
      * <example>
      * var matrix = FromYRotation(Math.PI / 2));
      * </example>
      **/
    public static Mat4 FromYRotation(double radians)
    {
        var s = Math.Sin(radians);
        var c = Math.Cos(radians);

        return new Mat4(
          c,
          0,
          -s,
          0,
          0,
          1,
          0,
          0,
          s,
          0,
          c,
          0,
          0,
          0,
          0,
          1);
    }

    /**
      * <summary>Creates a matrix from the given angle around the Z axis.</summary>
      *
      * <remarks>
      * This is equivalent to (but much faster than):
      *
      *     mat4.identity(dest)
      *     mat4.rotateZ(dest, dest, radians)
      * </remarks>
      *
      * <example>
      * var matrix = FromZRotation(Math.PI / 2));
      * </example>
      **/
    public static Mat4 FromZRotation(double radians)
    {
        var s = Math.Sin(radians);
        var c = Math.Cos(radians);

        return new Mat4(
          c, s, 0, 0,
          -s, c, 0, 0,
          0, 0, 1, 0,
          0, 0, 0, 1
        );
    }

    /// <summary>Is this matrix an identity matrix?</summary>
    public bool IsIdentity() => (
      d[0] == 1 && d[1] == 0 && d[2] == 0 && d[3] == 0 &&
      d[4] == 0 && d[5] == 1 && d[6] == 0 && d[7] == 0 &&
      d[8] == 0 && d[9] == 0 && d[10] == 1 && d[11] == 0 &&
      d[12] == 0 && d[13] == 0 && d[14] == 0 && d[15] == 1
    );

    /// <summary>Returns an inverted copy of this matrix.</summary>
    public Mat4 Invert()
    {
        var a00 = this.d[0];
        var a01 = this.d[1];
        var a02 = this.d[2];
        var a03 = this.d[3];
        var a10 = this.d[4];
        var a11 = this.d[5];
        var a12 = this.d[6];
        var a13 = this.d[7];
        var a20 = this.d[8];
        var a21 = this.d[9];
        var a22 = this.d[10];
        var a23 = this.d[11];
        var a30 = this.d[12];
        var a31 = this.d[13];
        var a32 = this.d[14];
        var a33 = this.d[15];

        var b00 = a00 * a11 - a01 * a10;
        var b01 = a00 * a12 - a02 * a10;
        var b02 = a00 * a13 - a03 * a10;
        var b03 = a01 * a12 - a02 * a11;
        var b04 = a01 * a13 - a03 * a11;
        var b05 = a02 * a13 - a03 * a12;
        var b06 = a20 * a31 - a21 * a30;
        var b07 = a20 * a32 - a22 * a30;
        var b08 = a20 * a33 - a23 * a30;
        var b09 = a21 * a32 - a22 * a31;
        var b10 = a21 * a33 - a23 * a31;
        var b11 = a22 * a33 - a23 * a32;

        // Calculate the determinant
        var det = b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;

        if (det == 0.0)
        {
            throw new ArgumentException("Attempt to invert a matrix with determinate of zero.");
        }
        det = 1.0 / det;

        return new Mat4(
          (a11 * b11 - a12 * b10 + a13 * b09) * det,
          (a02 * b10 - a01 * b11 - a03 * b09) * det,
          (a31 * b05 - a32 * b04 + a33 * b03) * det,
          (a22 * b04 - a21 * b05 - a23 * b03) * det,
          (a12 * b08 - a10 * b11 - a13 * b07) * det,
          (a00 * b11 - a02 * b08 + a03 * b07) * det,
          (a32 * b02 - a30 * b05 - a33 * b01) * det,
          (a20 * b05 - a22 * b02 + a23 * b01) * det,
          (a10 * b10 - a11 * b08 + a13 * b06) * det,
          (a01 * b08 - a00 * b10 - a03 * b06) * det,
          (a30 * b04 - a31 * b02 + a33 * b00) * det,
          (a21 * b02 - a20 * b04 - a23 * b00) * det,
          (a11 * b07 - a10 * b09 - a12 * b06) * det,
          (a00 * b09 - a01 * b07 + a02 * b06) * det,
          (a31 * b01 - a30 * b03 - a32 * b00) * det,
          (a20 * b03 - a21 * b01 + a22 * b00) * det);
    }

    /// <summary>Determine whether this matrix is a mirroring transformation.</summary>
    public bool IsMirroring()
    {
        var u = new Vec3(this.d[0], this.d[4], this.d[8]);
        var v = new Vec3(this.d[1], this.d[5], this.d[9]);
        var w = new Vec3(this.d[2], this.d[6], this.d[10]);

        // for a true orthogonal, non-mirrored base, u.cross(v) == w
        // If they have an opposite direction then we are mirroring
        var mirrorvalue = u.Cross(v).Dot(w);
        var ismirror = mirrorvalue < 0;
        return ismirror;
    }

    /// <summary>Determine whether this matrix is only translate and/or scale.</summary>
    public bool IsOnlyTransformScale() => (
      IsNearZero(this.d[1]) && IsNearZero(this.d[2]) && IsNearZero(this.d[3]) &&
      IsNearZero(this.d[4]) && IsNearZero(this.d[6]) && IsNearZero(this.d[7]) &&
      IsNearZero(this.d[8]) && IsNearZero(this.d[9]) && IsNearZero(this.d[11]) &&
      this.d[15] == 1
    );

    private bool IsNearZero(double num) => Math.Abs(num) < C.EPSILON;

    /// <summary>Multiply the input matrix by a Vector2.</summary>
    public Vec2 LeftMultiplyVec2(Vec2 v)
    {
        var v0 = v.x;
        var v1 = v.y;
        var v2 = 0;
        var v3 = 1;
        var x = v0 * this.d[0] + v1 * this.d[4] + v2 * this.d[8] + v3 * this.d[12];
        var y = v0 * this.d[1] + v1 * this.d[5] + v2 * this.d[9] + v3 * this.d[13];
        var w = v0 * this.d[3] + v1 * this.d[7] + v2 * this.d[11] + v3 * this.d[15];

        // scale such that fourth element becomes 1:
        if (w != 1)
        {
            var invw = 1.0 / w;
            x *= invw;
            y *= invw;
        }
        return new Vec2(x, y);
    }


    /// <summary>Multiply the input matrix by a Vector3.</summary>
    public Vec3 LeftMultiplyVec3(Vec3 v)
    {
        var v0 = v.x;
        var v1 = v.y;
        var v2 = v.z;
        var v3 = 1;
        var x = v0 * this.d[0] + v1 * this.d[4] + v2 * this.d[8] + v3 * this.d[12];
        var y = v0 * this.d[1] + v1 * this.d[5] + v2 * this.d[9] + v3 * this.d[13];
        var z = v0 * this.d[2] + v1 * this.d[6] + v2 * this.d[10] + v3 * this.d[14];
        var w = v0 * this.d[3] + v1 * this.d[7] + v2 * this.d[11] + v3 * this.d[15];

        // scale such that fourth element becomes 1:
        if (w != 1)
        {
            var invw = 1.0 / w;
            x *= invw;
            y *= invw;
            z *= invw;
        }
        return new Vec3(x, y, z);
    }

    /// <summary>Create a matrix for mirroring about the given plane.</summary>
    public static Mat4 MirrorByPlane(Plane plane)
    {
        var (nx, ny, nz, w) = plane.Points;

        return new Mat4(
          (1.0 - 2.0 * nx * nx),
          (-2.0 * ny * nx),
          (-2.0 * nz * nx),
          0,
          (-2.0 * nx * ny),
          (1.0 - 2.0 * ny * ny),
          (-2.0 * nz * ny),
          0,
          (-2.0 * nx * nz),
          (-2.0 * ny * nz),
          (1.0 - 2.0 * nz * nz),
          0,
          (2.0 * nx * w),
          (2.0 * ny * w),
          (2.0 * nz * w),
          1
        );
    }

    /// <summary>Create an affine matrix for mirroring into an arbitrary plane.</summary>
    public Mat4 Mirror(Vec3 vector)
    {
        var x = vector.x;
        var y = vector.y;
        var z = vector.z;

        return new Mat4(
          this.d[0] * x,
          this.d[1] * x,
          this.d[2] * x,
          this.d[3] * x,
          this.d[4] * y,
          this.d[5] * y,
          this.d[6] * y,
          this.d[7] * y,
          this.d[8] * z,
          this.d[9] * z,
          this.d[10] * z,
          this.d[11] * z,
          this.d[12],
          this.d[13],
          this.d[14],
          this.d[15]
        );
    }

    /// <summary>Multiplies this by the given matrix.</summary>
    public Mat4 Multiply(Mat4 gm)
    {
        var ret = new Mat4();
        var a00 = this.d[0];
        var a01 = this.d[1];
        var a02 = this.d[2];
        var a03 = this.d[3];
        var a10 = this.d[4];
        var a11 = this.d[5];
        var a12 = this.d[6];
        var a13 = this.d[7];
        var a20 = this.d[8];
        var a21 = this.d[9];
        var a22 = this.d[10];
        var a23 = this.d[11];
        var a30 = this.d[12];
        var a31 = this.d[13];
        var a32 = this.d[14];
        var a33 = this.d[15];

        // Cache only the current line of the second matrix
        var b0 = gm.d[0];
        var b1 = gm.d[1];
        var b2 = gm.d[2];
        var b3 = gm.d[3];
        ret.d[0] = b0 * a00 + b1 * a10 + b2 * a20 + b3 * a30;
        ret.d[1] = b0 * a01 + b1 * a11 + b2 * a21 + b3 * a31;
        ret.d[2] = b0 * a02 + b1 * a12 + b2 * a22 + b3 * a32;
        ret.d[3] = b0 * a03 + b1 * a13 + b2 * a23 + b3 * a33;

        b0 = gm.d[4];
        b1 = gm.d[5];
        b2 = gm.d[6];
        b3 = gm.d[7];
        ret.d[4] = b0 * a00 + b1 * a10 + b2 * a20 + b3 * a30;
        ret.d[5] = b0 * a01 + b1 * a11 + b2 * a21 + b3 * a31;
        ret.d[6] = b0 * a02 + b1 * a12 + b2 * a22 + b3 * a32;
        ret.d[7] = b0 * a03 + b1 * a13 + b2 * a23 + b3 * a33;

        b0 = gm.d[8];
        b1 = gm.d[9];
        b2 = gm.d[10];
        b3 = gm.d[11];
        ret.d[8] = b0 * a00 + b1 * a10 + b2 * a20 + b3 * a30;
        ret.d[9] = b0 * a01 + b1 * a11 + b2 * a21 + b3 * a31;
        ret.d[10] = b0 * a02 + b1 * a12 + b2 * a22 + b3 * a32;
        ret.d[11] = b0 * a03 + b1 * a13 + b2 * a23 + b3 * a33;

        b0 = gm.d[12];
        b1 = gm.d[13];
        b2 = gm.d[14];
        b3 = gm.d[15];
        ret.d[12] = b0 * a00 + b1 * a10 + b2 * a20 + b3 * a30;
        ret.d[13] = b0 * a01 + b1 * a11 + b2 * a21 + b3 * a31;
        ret.d[14] = b0 * a02 + b1 * a12 + b2 * a22 + b3 * a32;
        ret.d[15] = b0 * a03 + b1 * a13 + b2 * a23 + b3 * a33;
        return ret;
    }

    /// <summary>Multiply a 2D vector by a matrix.</summary>
    public Vec2 RightMultiplyVec2(Vec2 vector)
    {
        var v0 = vector.x;
        var v1 = vector.y;
        var v2 = 0;
        var v3 = 1;
        var x = v0 * this.d[0] + v1 * this.d[1] + v2 * this.d[2] + v3 * this.d[3];
        var y = v0 * this.d[4] + v1 * this.d[5] + v2 * this.d[6] + v3 * this.d[7];
        var w = v0 * this.d[12] + v1 * this.d[13] + v2 * this.d[14] + v3 * this.d[15];

        // scale such that fourth element becomes 1:
        if (w != 1)
        {
            var invw = 1.0 / w;
            x *= invw;
            y *= invw;
        }
        return new Vec2(x, y);
    }

    /// <summary>Multiply a 3D vector by a matrix</summary>
    public Vec3 RightMultiplyVec3(Vec3 vector)
    {
        var v0 = vector.x;
        var v1 = vector.y;
        var v2 = vector.z;
        var v3 = 1;
        var x = v0 * this.d[0] + v1 * this.d[1] + v2 * this.d[2] + v3 * this.d[3];
        var y = v0 * this.d[4] + v1 * this.d[5] + v2 * this.d[6] + v3 * this.d[7];
        var z = v0 * this.d[8] + v1 * this.d[9] + v2 * this.d[10] + v3 * this.d[11];
        var w = v0 * this.d[12] + v1 * this.d[13] + v2 * this.d[14] + v3 * this.d[15];

        // scale such that fourth element becomes 1:
        if (w != 1)
        {
            var invw = 1.0 / w;
            x *= invw;
            y *= invw;
            z *= invw;
        }
        return new Vec3(x, y, z);
    }

    /// <summary>Rotates this matrix by the given angle about the given axis.</summary>
    public Mat4 Rotate(double radians, Vec3 axis)
    {
        var x = axis.x;
        var y = axis.y;
        var z = axis.z;
        var len = axis.Length();
        var ret = new Mat4();

        if (Math.Abs(len) < 0.000001)
        {
            // axis is 0,0,0 or almost
            return new Mat4(this);
        }

        len = 1 / len;
        x *= len;
        y *= len;
        z *= len;

        var s = Math.Sin(radians);
        var c = Math.Cos(radians);
        var t = 1 - c;


        var a00 = this.d[0];
        var a01 = this.d[1];
        var a02 = this.d[2];
        var a03 = this.d[3];
        var a10 = this.d[4];
        var a11 = this.d[5];
        var a12 = this.d[6];
        var a13 = this.d[7];
        var a20 = this.d[8];
        var a21 = this.d[9];
        var a22 = this.d[10];
        var a23 = this.d[11];

        // Construct the elements of the rotation this.d
        var b00 = x * x * t + c;
        var b01 = y * x * t + z * s;
        var b02 = z * x * t - y * s;
        var b10 = x * y * t - z * s;
        var b11 = y * y * t + c;
        var b12 = z * y * t + x * s;
        var b20 = x * z * t + y * s;
        var b21 = y * z * t - x * s;
        var b22 = z * z * t + c;

        // Perform rotation-specific matrix multiplication
        ret.d[0] = a00 * b00 + a10 * b01 + a20 * b02;
        ret.d[1] = a01 * b00 + a11 * b01 + a21 * b02;
        ret.d[2] = a02 * b00 + a12 * b01 + a22 * b02;
        ret.d[3] = a03 * b00 + a13 * b01 + a23 * b02;
        ret.d[4] = a00 * b10 + a10 * b11 + a20 * b12;
        ret.d[5] = a01 * b10 + a11 * b11 + a21 * b12;
        ret.d[6] = a02 * b10 + a12 * b11 + a22 * b12;
        ret.d[7] = a03 * b10 + a13 * b11 + a23 * b12;
        ret.d[8] = a00 * b20 + a10 * b21 + a20 * b22;
        ret.d[9] = a01 * b20 + a11 * b21 + a21 * b22;
        ret.d[10] = a02 * b20 + a12 * b21 + a22 * b22;
        ret.d[11] = a03 * b20 + a13 * b21 + a23 * b22;

        ret.d[12] = this.d[12];
        ret.d[13] = this.d[13];
        ret.d[14] = this.d[14];
        ret.d[15] = this.d[15];

        return ret;
    }

    /// <summary>Rotates a matrix by the given angle around the X axis.</summary>
    public Mat4 RotateX(double radians)
    {
        var s = Math.Sin(radians);
        var c = Math.Cos(radians);
        var ret = new Mat4();

        var a10 = this.d[4];
        var a11 = this.d[5];
        var a12 = this.d[6];
        var a13 = this.d[7];
        var a20 = this.d[8];
        var a21 = this.d[9];
        var a22 = this.d[10];
        var a23 = this.d[11];

        // Copy the unchanged rows.
        ret.d[0] = this.d[0];
        ret.d[1] = this.d[1];
        ret.d[2] = this.d[2];
        ret.d[3] = this.d[3];
        ret.d[12] = this.d[12];
        ret.d[13] = this.d[13];
        ret.d[14] = this.d[14];
        ret.d[15] = this.d[15];

        // Perform axis-specific this.d multiplication
        ret.d[4] = a10 * c + a20 * s;
        ret.d[5] = a11 * c + a21 * s;
        ret.d[6] = a12 * c + a22 * s;
        ret.d[7] = a13 * c + a23 * s;
        ret.d[8] = a20 * c - a10 * s;
        ret.d[9] = a21 * c - a11 * s;
        ret.d[10] = a22 * c - a12 * s;
        ret.d[11] = a23 * c - a13 * s;

        return ret;
    }

    /// <summary>Rotates this matrix by the given angle around the Y axis.</summary>
    public Mat4 RotateY(double radians)
    {
        var s = Math.Sin(radians);
        var c = Math.Cos(radians);
        var ret = new Mat4();

        var a00 = this.d[0];
        var a01 = this.d[1];
        var a02 = this.d[2];
        var a03 = this.d[3];
        var a20 = this.d[8];
        var a21 = this.d[9];
        var a22 = this.d[10];
        var a23 = this.d[11];

        // Copy the unchanged rows.
        ret.d[4] = this.d[4];
        ret.d[5] = this.d[5];
        ret.d[6] = this.d[6];
        ret.d[7] = this.d[7];
        ret.d[12] = this.d[12];
        ret.d[13] = this.d[13];
        ret.d[14] = this.d[14];
        ret.d[15] = this.d[15];

        // Perform axis-specific matrix multiplication
        ret.d[0] = a00 * c - a20 * s;
        ret.d[1] = a01 * c - a21 * s;
        ret.d[2] = a02 * c - a22 * s;
        ret.d[3] = a03 * c - a23 * s;
        ret.d[8] = a00 * s + a20 * c;
        ret.d[9] = a01 * s + a21 * c;
        ret.d[10] = a02 * s + a22 * c;
        ret.d[11] = a03 * s + a23 * c;
        return ret;
    }

    /// <summary>Rotates this matrix by the given angle around the Z axis.</summary>
    public Mat4 RotateZ(double radians)
    {
        var s = Math.Sin(radians);
        var c = Math.Cos(radians);
        var ret = new Mat4();

        var a00 = this.d[0];
        var a01 = this.d[1];
        var a02 = this.d[2];
        var a03 = this.d[3];
        var a10 = this.d[4];
        var a11 = this.d[5];
        var a12 = this.d[6];
        var a13 = this.d[7];

        // Copy the unchanged last row.
        ret.d[8] = this.d[8];
        ret.d[9] = this.d[9];
        ret.d[10] = this.d[10];
        ret.d[11] = this.d[11];
        ret.d[12] = this.d[12];
        ret.d[13] = this.d[13];
        ret.d[14] = this.d[14];
        ret.d[15] = this.d[15];

        // Perform axis-specific matrix multiplication
        ret.d[0] = a00 * c + a10 * s;
        ret.d[1] = a01 * c + a11 * s;
        ret.d[2] = a02 * c + a12 * s;
        ret.d[3] = a03 * c + a13 * s;
        ret.d[4] = a10 * c - a00 * s;
        ret.d[5] = a11 * c - a01 * s;
        ret.d[6] = a12 * c - a02 * s;
        ret.d[7] = a13 * c - a03 * s;

        return ret;
    }

    /// <summary>Scales the matrix by the given dimensions Vec3.</summary>
    public Mat4 Scale(Vec3 dimensions)
    {
        var x = dimensions.x;
        var y = dimensions.y;
        var z = dimensions.z;
        var ret = new Mat4();

        ret.d[0] = this.d[0] * x;
        ret.d[1] = this.d[1] * x;
        ret.d[2] = this.d[2] * x;
        ret.d[3] = this.d[3] * x;
        ret.d[4] = this.d[4] * y;
        ret.d[5] = this.d[5] * y;
        ret.d[6] = this.d[6] * y;
        ret.d[7] = this.d[7] * y;
        ret.d[8] = this.d[8] * z;
        ret.d[9] = this.d[9] * z;
        ret.d[10] = this.d[10] * z;
        ret.d[11] = this.d[11] * z;
        ret.d[12] = this.d[12];
        ret.d[13] = this.d[13];
        ret.d[14] = this.d[14];
        ret.d[15] = this.d[15];

        return ret;
    }

    /// <summary>Subtracts this matrix from a given matrix</summary>
    public Mat4 Subtract(Mat4 gm)
    {
        return new Mat4(
          this.d[0] - gm.d[0],
          this.d[1] - gm.d[1],
          this.d[2] - gm.d[2],
          this.d[3] - gm.d[3],
          this.d[4] - gm.d[4],
          this.d[5] - gm.d[5],
          this.d[6] - gm.d[6],
          this.d[7] - gm.d[7],
          this.d[8] - gm.d[8],
          this.d[9] - gm.d[9],
          this.d[10] - gm.d[10],
          this.d[11] - gm.d[11],
          this.d[12] - gm.d[12],
          this.d[13] - gm.d[13],
          this.d[14] - gm.d[14],
          this.d[15] - gm.d[15]
        );
    }
    /// <summary>Translate the matrix by the given offset vector.</summary>
    public Mat4 Translate(Vec3 offsets)
    {
        var x = offsets.x;
        var y = offsets.y;
        var z = offsets.z;
        var ret = new Mat4();
        double a00;
        double a01;
        double a02;
        double a03;
        double a10;
        double a11;
        double a12;
        double a13;
        double a20;
        double a21;
        double a22;
        double a23;

        a00 = this.d[0]; a01 = this.d[1]; a02 = this.d[2]; a03 = this.d[3];
        a10 = this.d[4]; a11 = this.d[5]; a12 = this.d[6]; a13 = this.d[7];
        a20 = this.d[8]; a21 = this.d[9]; a22 = this.d[10]; a23 = this.d[11];

        ret.d[0] = a00; ret.d[1] = a01; ret.d[2] = a02; ret.d[3] = a03;
        ret.d[4] = a10; ret.d[5] = a11; ret.d[6] = a12; ret.d[7] = a13;
        ret.d[8] = a20; ret.d[9] = a21; ret.d[10] = a22; ret.d[11] = a23;

        ret.d[12] = a00 * x + a10 * y + a20 * z + this.d[12];
        ret.d[13] = a01 * x + a11 * y + a21 * z + this.d[13];
        ret.d[14] = a02 * x + a12 * y + a22 * z + this.d[14];
        ret.d[15] = a03 * x + a13 * y + a23 * z + this.d[15];

        return ret;
    }

    /// <summary>Check if matrix is valid.</summary>
    public void Validate() {
        if (!this.d.All(double.IsFinite))
        {
            throw new ValidationException($"Invalid Mat4: {this}");
        }
    }
}