namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> IntersectGeom3Exp2 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3((double)(9), (double)(9), (double)(8)),
        new Vec3((double)(9), (double)(9), (double)(9)),
        new Vec3((double)(9), (double)(8), (double)(9)),
        new Vec3((double)(9), (double)(8), (double)(8))
    },
      new List<Vec3> {
        new Vec3((double)(8), (double)(9), (double)(9)),
        new Vec3((double)(9), (double)(9), (double)(9)),
        new Vec3((double)(9), (double)(9), (double)(8)),
        new Vec3((double)(8), (double)(9), (double)(8))
    },
      new List<Vec3> {
        new Vec3((double)(9), (double)(8), (double)(9)),
        new Vec3((double)(9), (double)(9), (double)(9)),
        new Vec3((double)(8), (double)(9), (double)(9)),
        new Vec3((double)(8), (double)(8), (double)(9))
    },
      new List<Vec3> {
        new Vec3((double)(8), (double)(9), (double)(9)),
        new Vec3((double)(8), (double)(9), (double)(8)),
        new Vec3((double)(8), (double)(8), (double)(8)),
        new Vec3((double)(8), (double)(8), (double)(9))
    },
      new List<Vec3> {
        new Vec3((double)(8), (double)(8), (double)(9)),
        new Vec3((double)(8), (double)(8), (double)(8)),
        new Vec3((double)(9), (double)(8), (double)(8)),
        new Vec3((double)(9), (double)(8), (double)(9))
    },
      new List<Vec3> {
        new Vec3((double)(9), (double)(8), (double)(8)),
        new Vec3((double)(8), (double)(8), (double)(8)),
        new Vec3((double)(8), (double)(9), (double)(8)),
        new Vec3((double)(9), (double)(9), (double)(8))
    }
  };
}
