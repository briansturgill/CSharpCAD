namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeFSDef1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3((double)(10), (double)(-10), (double)(0)),
        new Vec3((double)(10), (double)(10), (double)(0)),
        new Vec3((double)(10), (double)(10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(10), (double)(-10), (double)(0)),
        new Vec3((double)(10), (double)(10), (double)(1)),
        new Vec3((double)(10), (double)(-10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(10), (double)(10), (double)(0)),
        new Vec3((double)(-10), (double)(10), (double)(0)),
        new Vec3((double)(-10), (double)(10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(10), (double)(10), (double)(0)),
        new Vec3((double)(-10), (double)(10), (double)(1)),
        new Vec3((double)(10), (double)(10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(-10), (double)(10), (double)(0)),
        new Vec3((double)(-10), (double)(-10), (double)(0)),
        new Vec3((double)(-10), (double)(-10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(-10), (double)(10), (double)(0)),
        new Vec3((double)(-10), (double)(-10), (double)(1)),
        new Vec3((double)(-10), (double)(10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(-10), (double)(-10), (double)(0)),
        new Vec3((double)(10), (double)(-10), (double)(0)),
        new Vec3((double)(10), (double)(-10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(-10), (double)(-10), (double)(0)),
        new Vec3((double)(10), (double)(-10), (double)(1)),
        new Vec3((double)(-10), (double)(-10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(-10), (double)(-10), (double)(1)),
        new Vec3((double)(10), (double)(-10), (double)(1)),
        new Vec3((double)(10), (double)(10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(10), (double)(10), (double)(1)),
        new Vec3((double)(-10), (double)(10), (double)(1)),
        new Vec3((double)(-10), (double)(-10), (double)(1))
    },
      new List<Vec3> {
        new Vec3((double)(10), (double)(10), (double)(0)),
        new Vec3((double)(10), (double)(-10), (double)(0)),
        new Vec3((double)(-10), (double)(-10), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-10), (double)(-10), (double)(0)),
        new Vec3((double)(-10), (double)(10), (double)(0)),
        new Vec3((double)(10), (double)(10), (double)(0))
    }
  };
}
