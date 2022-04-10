namespace CSharpCAD;

/// <summary>Constants.</summary>
public static class C
{
    // Seems not to be used, but helps explain EPS below.
    // <summary>The resolution of space, currently one hundred nanometers (1/EPS).</summary>
    //public const double spaticalResolution = 1e5;

    /// <summary>Epsilon used during determination of near zero distances (1/spatialResolution)</summary>
    public const double EPS = 1e-5;

    /// <summary>Like Number.EPSILON in Javascript.</summary>
    public const double EPSILON = 2.220446049250313e-16; // double 2**-52
    // If we ever need one for single precision: = 1.1920928955078125e-7f; // single 2**-23


    // Normals are directional vectors with component values from 0 to 1.0, requiring specialized comparision
    // This EPS is derived from a serieas of tests to determine the optimal precision for comparing
    // coplanar polygons, as provided by the sphere primitive at high segmentation.
    // This EPS is for 64 bit Number values
    /// <summary>Normal vectors require a special EPS.</summary>
    public const double NEPS = 1e-13;
}