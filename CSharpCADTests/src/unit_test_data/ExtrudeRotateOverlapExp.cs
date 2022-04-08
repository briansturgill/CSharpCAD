namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeRotateOverlapExp = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3((double)(0), (double)(4.898587196589413e-16), (double)(8)),
        new Vec3((double)(7), (double)(4.898587196589413e-16), (double)(8)),
        new Vec3((double)(-6.123233995736767e-17), (double)(7), (double)(8))
    },
      new List<Vec3> {
        new Vec3((double)(7), (double)(-4.898587196589413e-16), (double)(-8)),
        new Vec3((double)(4.898587196589413e-16), (double)(-2.999519565323715e-32), (double)(-8)),
        new Vec3((double)(9.184850993605148e-16), (double)(7), (double)(-8))
    },
      new List<Vec3> {
        new Vec3((double)(7), (double)(4.898587196589413e-16), (double)(8)),
        new Vec3((double)(7), (double)(-4.898587196589413e-16), (double)(-8)),
        new Vec3((double)(9.184850993605148e-16), (double)(7), (double)(-8))
    },
      new List<Vec3> {
        new Vec3((double)(7), (double)(4.898587196589413e-16), (double)(8)),
        new Vec3((double)(9.184850993605148e-16), (double)(7), (double)(-8)),
        new Vec3((double)(-6.123233995736767e-17), (double)(7), (double)(8))
    },
      new List<Vec3> {
        new Vec3((double)(4.898587196589413e-16), (double)(-2.999519565323715e-32), (double)(-8)),
        new Vec3((double)(-4.898587196589413e-16), (double)(2.999519565323715e-32), (double)(8)),
        new Vec3((double)(-6.123233995736767e-17), (double)(7), (double)(8))
    },
      new List<Vec3> {
        new Vec3((double)(-6.123233995736767e-17), (double)(7), (double)(8)),
        new Vec3((double)(9.184850993605148e-16), (double)(7), (double)(-8)),
        new Vec3((double)(4.898587196589413e-16), (double)(-2.999519565323715e-32), (double)(-8))
    },
      new List<Vec3> {
        new Vec3((double)(7), (double)(4.898587196589413e-16), (double)(8)),
        new Vec3((double)(0), (double)(4.898587196589413e-16), (double)(8)),
        new Vec3((double)(0), (double)(-4.898587196589413e-16), (double)(-8))
    },
      new List<Vec3> {
        new Vec3((double)(0), (double)(-4.898587196589413e-16), (double)(-8)),
        new Vec3((double)(7), (double)(-4.898587196589413e-16), (double)(-8)),
        new Vec3((double)(7), (double)(4.898587196589413e-16), (double)(8))
    }
  };
}
