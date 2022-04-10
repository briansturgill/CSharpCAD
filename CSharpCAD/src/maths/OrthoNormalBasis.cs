namespace CSharpCAD;

/**
 * <summary>OrthoNormalBasis reprojects points on a 3D plane onto a 2D plane
 * or from a 2D plane back onto the 3D plane.</summary>
 */
public class OrthoNormalBasis
{
    Vec3 u;
    Vec3 v;
    Plane plane;
    Vec3 planeorigin;
    /// <summary>Construct an OrthoNormalBasis</summary>
    /// <param name="plane">The basis plane.</param>
    /// <param name="rv">Right hand vector.</param>
    public OrthoNormalBasis(Plane plane, Vec3? rv = null)
    {
        // choose an arbitrary right hand vector, making sure it is somewhat orthogonal to the plane normal:
        var rightvector = rv ?? plane.Normal.Orthogonal();
        this.v = plane.Normal.Cross(rightvector).Normalize();
        this.u = this.v.Cross(plane.Normal);
        this.plane = plane;
        this.planeorigin = plane.Normal.Scale(plane.W);
    }

    /// <summary>Get an orthonormal basis for the standard XYZ planes.</summary>
    /// <remarks>
    /// Parameters: the names of two 3D axes. The 2d x axis will map to the first given 3D axis, the 2d y
    /// axis will map to the second.
    /// Prepend the axis with a "-" to invert the direction of this axis.
    /// For example: OrthoNormalBasis.GetCartesian("-Y","Z")
    ///   will return an orthonormal basis where the 2d X axis maps to the 3D inverted Y axis, and
    ///   the 2d Y axis maps to the 3D Z axis.
    /// </remarks>
    public static OrthoNormalBasis GetCartesian(string xaxisid, string yaxisid)
    {
        var axisid = xaxisid + "/" + yaxisid;
        Vec3 planenormal, rightvector;
        if (axisid == "X/Y")
        {
            planenormal = new Vec3(0, 0, 1);
            rightvector = new Vec3(1, 0, 0);
        }
        else if (axisid == "Y/-X")
        {
            planenormal = new Vec3(0, 0, 1);
            rightvector = new Vec3(0, 1, 0);
        }
        else if (axisid == "-X/-Y")
        {
            planenormal = new Vec3(0, 0, 1);
            rightvector = new Vec3(-1, 0, 0);
        }
        else if (axisid == "-Y/X")
        {
            planenormal = new Vec3(0, 0, 1);
            rightvector = new Vec3(0, -1, 0);
        }
        else if (axisid == "-X/Y")
        {
            planenormal = new Vec3(0, 0, -1);
            rightvector = new Vec3(-1, 0, 0);
        }
        else if (axisid == "-Y/-X")
        {
            planenormal = new Vec3(0, 0, -1);
            rightvector = new Vec3(0, -1, 0);
        }
        else if (axisid == "X/-Y")
        {
            planenormal = new Vec3(0, 0, -1);
            rightvector = new Vec3(1, 0, 0);
        }
        else if (axisid == "Y/X")
        {
            planenormal = new Vec3(0, 0, -1);
            rightvector = new Vec3(0, 1, 0);
        }
        else if (axisid == "X/Z")
        {
            planenormal = new Vec3(0, -1, 0);
            rightvector = new Vec3(1, 0, 0);
        }
        else if (axisid == "Z/-X")
        {
            planenormal = new Vec3(0, -1, 0);
            rightvector = new Vec3(0, 0, 1);
        }
        else if (axisid == "-X/-Z")
        {
            planenormal = new Vec3(0, -1, 0);
            rightvector = new Vec3(-1, 0, 0);
        }
        else if (axisid == "-Z/X")
        {
            planenormal = new Vec3(0, -1, 0);
            rightvector = new Vec3(0, 0, -1);
        }
        else if (axisid == "-X/Z")
        {
            planenormal = new Vec3(0, 1, 0);
            rightvector = new Vec3(-1, 0, 0);
        }
        else if (axisid == "-Z/-X")
        {
            planenormal = new Vec3(0, 1, 0);
            rightvector = new Vec3(0, 0, -1);
        }
        else if (axisid == "X/-Z")
        {
            planenormal = new Vec3(0, 1, 0);
            rightvector = new Vec3(1, 0, 0);
        }
        else if (axisid == "Z/X")
        {
            planenormal = new Vec3(0, 1, 0);
            rightvector = new Vec3(0, 0, 1);
        }
        else if (axisid == "Y/Z")
        {
            planenormal = new Vec3(1, 0, 0);
            rightvector = new Vec3(0, 1, 0);
        }
        else if (axisid == "Z/-Y")
        {
            planenormal = new Vec3(1, 0, 0);
            rightvector = new Vec3(0, 0, 1);
        }
        else if (axisid == "-Y/-Z")
        {
            planenormal = new Vec3(1, 0, 0);
            rightvector = new Vec3(0, -1, 0);
        }
        else if (axisid == "-Z/Y")
        {
            planenormal = new Vec3(1, 0, 0);
            rightvector = new Vec3(0, 0, -1);
        }
        else if (axisid == "-Y/Z")
        {
            planenormal = new Vec3(-1, 0, 0);
            rightvector = new Vec3(0, -1, 0);
        }
        else if (axisid == "-Z/-Y")
        {
            planenormal = new Vec3(-1, 0, 0);
            rightvector = new Vec3(0, 0, -1);
        }
        else if (axisid == "Y/-Z")
        {
            planenormal = new Vec3(-1, 0, 0);
            rightvector = new Vec3(0, 1, 0);
        }
        else if (axisid == "Z/Y")
        {
            planenormal = new Vec3(-1, 0, 0);
            rightvector = new Vec3(0, 0, 1);
        }
        else
        {
            throw new ArgumentException("OrthoNormalBasis.GetCartesian: invalid combination of axis identifiers. Should pass two string arguments from [X,Y,Z,-X,-Y,-Z], being two different axes.");
        }
        return new OrthoNormalBasis(new Plane(planenormal, 0), rightvector);
    }

#if APPARENTLY_UNUSED
    // The z=0 plane, with the 3D x and y vectors mapped to the 2D x and y vector
    public static OrthoNormalBasis Z0Plane()
    {
        var plane = new Plane(new Vec3(0, 0, 1), 0);
        return new OrthoNormalBasis(plane, new Vec3(1, 0, 0));
    }
#endif

    ///
    public Mat4 GetProjectionMatrix()
    {
        return new Mat4(
          this.u.x, this.v.x, this.plane.Normal.x, 0,
          this.u.y, this.v.y, this.plane.Normal.y, 0,
          this.u.z, this.v.z, this.plane.Normal.z, 0,
          0, 0, -this.plane.W, 1
        );
    }

    ///
    public Mat4 GetInverseProjectionMatrix()
    {
        var p = this.plane.Normal.Scale(this.plane.W);
        return new Mat4(
            this.u.x, this.u.y, this.u.z, 0,
            this.v.x, this.v.y, this.v.z, 0,
            this.plane.Normal.x, this.plane.Normal.y, this.plane.Normal.z, 0,
            p.x, p.y, p.z, 1
          );
    }

    /// <summary>Use the ONB to translate a 3D point to 2D.</summary>
    public Vec2 To2D(Vec3 point)
    {
        return new Vec2(point.Dot(this.u), point.Dot(this.v));
    }

    /// <summary>Use the ONB to translate a 2D point to 3D.</summary>
    public Vec3 To3D(Vec2 point)
    {
        var v1 = this.u.Scale(point.x);
        var v2 = this.v.Scale(point.y);
        var v3 = v1.Add(this.planeorigin);
        var v4 = v2.Add(v3);
        return v4;
    }

#if APPARENTLY_UNUSED
    public Line2 Line3Dto2D(Line3 line3d)
    {
        var a = line3d.point;
        var b = line3d.direction.Add(a);
        var a2d = this.To2D(a);
        var b2d = this.To2D(b);
        return new Line2(a2d, b2d);
    }

    public Line3 line2Dto3D(Line2 line2d)
    {
        var a = line2d.origin();
        var b = line2d.direction().Add(a);
        var a3d = this.To3D(a);
        var b3d = this.To3D(b);
        return new Line3(a3d, b3d);
    }

    public OrthoNormalBasis Transform(Mat4 matrix4x4)
    {
        // todo: this may not work properly in case of mirroring
        var newplane = this.plane.Transform(matrix4x4);
        var rightpointTransformed = this.u.Transform(matrix4x4);
        var originTransformed = new Vec3(0, 0, 0).Transform(matrix4x4);
        var newrighthandvector = rightpointTransformed.Subtract(originTransformed);
        var newbasis = new OrthoNormalBasis(newplane, newrighthandvector);
        return newbasis;
    }
#endif
}