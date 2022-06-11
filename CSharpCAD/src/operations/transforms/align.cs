namespace CSharpCAD;

public static partial class CSCAD
{

    private const int _C = 1, _X = 2, _N = 3, _U = 0;

    /// <summary>Alignment modes for the "Align" function.</summary>
    /// <remarks>
    /// A 3 character sequence specifiying the mode for X, Y, and Z axes.
    ///   C - Center
    ///   X - Maximum
    ///   N - Minimum
    ///   U - Unaligned
    /// </remarks>
    public enum AM
    {
#pragma warning disable CS1591
        CCC = _C << 4 | _C << 2 | _C,
        CCX = _C << 4 | _C << 2 | _X,
        CCN = _C << 4 | _C << 2 | _N,
        CCU = _C << 4 | _C << 2 | _U,
        CXC = _C << 4 | _X << 2 | _C,
        CXX = _C << 4 | _X << 2 | _X,
        CXN = _C << 4 | _X << 2 | _N,
        CXU = _C << 4 | _X << 2 | _U,
        CNC = _C << 4 | _N << 2 | _C,
        CNX = _C << 4 | _N << 2 | _X,
        CNN = _C << 4 | _N << 2 | _N,
        CNU = _C << 4 | _N << 2 | _U,
        CUC = _C << 4 | _U << 2 | _C,
        CUX = _C << 4 | _U << 2 | _X,
        CUN = _C << 4 | _U << 2 | _N,
        CUU = _C << 4 | _U << 2 | _U,
        XCC = _X << 4 | _C << 2 | _C,
        XCX = _X << 4 | _C << 2 | _X,
        XCN = _X << 4 | _C << 2 | _N,
        XCU = _X << 4 | _C << 2 | _U,
        XXC = _X << 4 | _X << 2 | _C,
        XXX = _X << 4 | _X << 2 | _X,
        XXN = _X << 4 | _X << 2 | _N,
        XXU = _X << 4 | _X << 2 | _U,
        XNC = _X << 4 | _N << 2 | _C,
        XNX = _X << 4 | _N << 2 | _X,
        XNN = _X << 4 | _N << 2 | _N,
        XNU = _X << 4 | _N << 2 | _U,
        XUC = _X << 4 | _U << 2 | _C,
        XUX = _X << 4 | _U << 2 | _X,
        XUN = _X << 4 | _U << 2 | _N,
        XUU = _X << 4 | _U << 2 | _U,
        NCC = _N << 4 | _C << 2 | _C,
        NCX = _N << 4 | _C << 2 | _X,
        NCN = _N << 4 | _C << 2 | _N,
        NCU = _N << 4 | _C << 2 | _U,
        NXC = _N << 4 | _X << 2 | _C,
        NXX = _N << 4 | _X << 2 | _X,
        NXN = _N << 4 | _X << 2 | _N,
        NXU = _N << 4 | _X << 2 | _U,
        NNC = _N << 4 | _N << 2 | _C,
        NNX = _N << 4 | _N << 2 | _X,
        NNN = _N << 4 | _N << 2 | _N,
        NNU = _N << 4 | _N << 2 | _U,
        NUC = _N << 4 | _U << 2 | _C,
        NUX = _N << 4 | _U << 2 | _X,
        NUN = _N << 4 | _U << 2 | _N,
        NUU = _N << 4 | _U << 2 | _U,
        UCC = _U << 4 | _C << 2 | _C,
        UCX = _U << 4 | _C << 2 | _X,
        UCN = _U << 4 | _C << 2 | _N,
        UCU = _U << 4 | _C << 2 | _U,
        UXC = _U << 4 | _X << 2 | _C,
        UXX = _U << 4 | _X << 2 | _X,
        UXN = _U << 4 | _X << 2 | _N,
        UXU = _U << 4 | _X << 2 | _U,
        UNC = _U << 4 | _N << 2 | _C,
        UNX = _U << 4 | _N << 2 | _X,
        UNN = _U << 4 | _N << 2 | _N,
        UNU = _U << 4 | _N << 2 | _U,
        UUC = _U << 4 | _U << 2 | _C,
        UUX = _U << 4 | _U << 2 | _X,
        UUN = _U << 4 | _U << 2 | _N,
        UUU = _U << 4 | _U << 2 | _U,
#pragma warning restore CS1591
    }

    private static (Vec3, Vec3) getBoundingBox(Geometry gobj)
    {
        if (gobj.Is2D)
        {
            var (min, max) = ((Geom2)gobj).BoundingBox();
            return (new Vec3(min, 0), new Vec3(max, 0));
        }
        return ((Geom3)gobj).BoundingBox();
    }

    internal static Geometry alignGeometry(Geometry gobj, AM modes, Vec3 relativeTo)
    {
        double v(Vec3 val, int i)
        {
            switch (i)
            {
                case 0:
                    return val.X;
                case 1:
                    return val.Y;
                case 2:
                    return val.Z;
                default:
                    throw new ArgumentException("Index out of bounds.");
            }
        }
        var (min, max) = getBoundingBox(gobj);
        var tr = new double[] { 0, 0, 0 };
        for (var i = 0; i < 3; i++)
        {
            var mode = (((int)modes) & 3) >> (2 - i);
            if (mode == _C)
            {
                tr[i] = v(relativeTo, i) - (v(min, i) + v(max, i)) / 2;
            }
            else if (mode == _X)
            {
                tr[i] = v(relativeTo, i) - v(max, i);
            }
            else if (mode == _N)
            {
                tr[i] = v(relativeTo, i) - v(min, i);
            }
        }

        return Translate((tr[0], tr[1], tr[2]), gobj);
    }

    /**
     * <summary>Align the boundaries of the given geometry using the given options.</summary>
     * <param name="gobj">The geometry object to be aligned.</param>
     * <param name="modes">A value from the AM enumeration, which consists of 3 letters (one for each of X, Y, Z): C - center, X - Max, N-Min, U-unaligned</param>
     * <param name="relativeTo" default="(0,0,0)">The point one each axis on which to align the geometry upon.</param>
     * <remarks>
     * C# syntax makes the porting of JSCAD's "align" difficult. We had to simplify.
     * 1) There is no "grouped" option... you can get the same effect by using Union on the geometries, then aligning the resulting object.
     * 2) The relativeTo option does not allow an X, Y, or Z coordinate to be null. You'll have to find the values from a bounding box yourself.
     * </remarks>
     * <example>
     * var alignedGeometry = Align(gobj, AM.NCU, relativeTo: (10, 0, 10]);
     * </example>
     */
    public static Geometry Align(Geometry gobj, AM modes = AM.CCN, Vec3? relativeTo = null)
    {
        Vec3 _relativeTo = relativeTo ?? new Vec3();

        return alignGeometry(gobj, modes, _relativeTo);
    }
}