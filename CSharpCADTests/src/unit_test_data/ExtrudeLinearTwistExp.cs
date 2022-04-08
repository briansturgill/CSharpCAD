namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeLinearTwistExp = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3((double)(5), (double)(-5), (double)(0)),
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(7.0710678118654755), (double)(4.440892098500626e-16), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(-5), (double)(0)),
        new Vec3((double)(7.0710678118654755), (double)(4.440892098500626e-16), (double)(15)),
        new Vec3((double)(4.440892098500626e-16), (double)(-7.0710678118654755), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(-4.440892098500626e-16), (double)(7.0710678118654755), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(-4.440892098500626e-16), (double)(7.0710678118654755), (double)(15)),
        new Vec3((double)(7.0710678118654755), (double)(4.440892098500626e-16), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(-5), (double)(-5), (double)(0)),
        new Vec3((double)(-7.0710678118654755), (double)(-4.440892098500626e-16), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(-7.0710678118654755), (double)(-4.440892098500626e-16), (double)(15)),
        new Vec3((double)(-4.440892098500626e-16), (double)(7.0710678118654755), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(-5), (double)(0)),
        new Vec3((double)(5), (double)(-5), (double)(0)),
        new Vec3((double)(4.440892098500626e-16), (double)(-7.0710678118654755), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(-5), (double)(0)),
        new Vec3((double)(4.440892098500626e-16), (double)(-7.0710678118654755), (double)(15)),
        new Vec3((double)(-7.0710678118654755), (double)(-4.440892098500626e-16), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-7.0710678118654755), (double)(-4.440892098500626e-16), (double)(15)),
        new Vec3((double)(4.440892098500626e-16), (double)(-7.0710678118654755), (double)(15)),
        new Vec3((double)(7.0710678118654755), (double)(4.440892098500626e-16), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(7.0710678118654755), (double)(4.440892098500626e-16), (double)(15)),
        new Vec3((double)(-4.440892098500626e-16), (double)(7.0710678118654755), (double)(15)),
        new Vec3((double)(-7.0710678118654755), (double)(-4.440892098500626e-16), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(5), (double)(-5), (double)(0)),
        new Vec3((double)(-5), (double)(-5), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(-5), (double)(0)),
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(5), (double)(5), (double)(0))
    }
  };
}
