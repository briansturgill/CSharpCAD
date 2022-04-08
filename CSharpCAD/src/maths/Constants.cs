namespace CSharpCAD;

/// <summary>Constants.</summary>
public static class C
{
    /// <summary>The resolution of space, currently one hundred nanometers (1/EPS).</summary>
    public const double spaticalResolution = 1e5;
    /// <summary>Epsilon used during determination of near zero distances (1/spatialResolution)</summary>
    public const double EPS = 1e-5;

    /// <summary>Like Number.EPSILON in Javascript.</summary>
    public const double EPSILON = 2.220446049250313e-16; // double 2**-52
    //public const double EPSILON = 1.1920928955078125e-7f; // single 2**-23


    // Normals are directional vectors with component values from 0 to 1.0, requiring specialized comparision
    // This EPS is derived from a serieas of tests to determine the optimal precision for comparing coplanar polygons,
    // as provided by the sphere primitive at high segmentation
    // This EPS is for 64 bit Number values
    public const double NEPS = 1e-13;

    // Educated guess here, the values 1e-5 and 1e-6 are commonly used to avoid precision errors.
    // See https://en.wikipedia.org/wiki/IEEE_754-1985.
    // public const double NEPS = 1e-6;// single
}