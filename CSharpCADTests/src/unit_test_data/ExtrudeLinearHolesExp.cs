namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeLinearHolesExp = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(-5), (double)(-5), (double)(0)),
        new Vec3((double)(-5), (double)(-5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(-5), (double)(-5), (double)(15)),
        new Vec3((double)(-5), (double)(5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(-5), (double)(0)),
        new Vec3((double)(5), (double)(-5), (double)(0)),
        new Vec3((double)(5), (double)(-5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(-5), (double)(0)),
        new Vec3((double)(5), (double)(-5), (double)(15)),
        new Vec3((double)(-5), (double)(-5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(-5), (double)(0)),
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(5), (double)(5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(-5), (double)(0)),
        new Vec3((double)(5), (double)(5), (double)(15)),
        new Vec3((double)(5), (double)(-5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(-5), (double)(5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(-5), (double)(5), (double)(15)),
        new Vec3((double)(5), (double)(5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(-2), (double)(0)),
        new Vec3((double)(-2), (double)(2), (double)(0)),
        new Vec3((double)(-2), (double)(2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(-2), (double)(0)),
        new Vec3((double)(-2), (double)(2), (double)(15)),
        new Vec3((double)(-2), (double)(-2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(2), (double)(-2), (double)(0)),
        new Vec3((double)(-2), (double)(-2), (double)(0)),
        new Vec3((double)(-2), (double)(-2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(2), (double)(-2), (double)(0)),
        new Vec3((double)(-2), (double)(-2), (double)(15)),
        new Vec3((double)(2), (double)(-2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(2), (double)(2), (double)(0)),
        new Vec3((double)(2), (double)(-2), (double)(0)),
        new Vec3((double)(2), (double)(-2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(2), (double)(2), (double)(0)),
        new Vec3((double)(2), (double)(-2), (double)(15)),
        new Vec3((double)(2), (double)(2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(2), (double)(0)),
        new Vec3((double)(2), (double)(2), (double)(0)),
        new Vec3((double)(2), (double)(2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(2), (double)(0)),
        new Vec3((double)(2), (double)(2), (double)(15)),
        new Vec3((double)(-2), (double)(2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(-5), (double)(15)),
        new Vec3((double)(5), (double)(5), (double)(15)),
        new Vec3((double)(2), (double)(2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(2), (double)(15)),
        new Vec3((double)(2), (double)(2), (double)(15)),
        new Vec3((double)(5), (double)(5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(-5), (double)(15)),
        new Vec3((double)(2), (double)(2), (double)(15)),
        new Vec3((double)(2), (double)(-2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(2), (double)(15)),
        new Vec3((double)(5), (double)(5), (double)(15)),
        new Vec3((double)(-5), (double)(5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(-5), (double)(15)),
        new Vec3((double)(5), (double)(-5), (double)(15)),
        new Vec3((double)(2), (double)(-2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(-2), (double)(15)),
        new Vec3((double)(-2), (double)(2), (double)(15)),
        new Vec3((double)(-5), (double)(5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(-5), (double)(15)),
        new Vec3((double)(2), (double)(-2), (double)(15)),
        new Vec3((double)(-2), (double)(-2), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(-2), (double)(15)),
        new Vec3((double)(-5), (double)(5), (double)(15)),
        new Vec3((double)(-5), (double)(-5), (double)(15))
    },
      new List<Vec3> {
        new Vec3((double)(2), (double)(2), (double)(0)),
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(5), (double)(-5), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(2), (double)(2), (double)(0)),
        new Vec3((double)(-2), (double)(2), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(2), (double)(-2), (double)(0)),
        new Vec3((double)(2), (double)(2), (double)(0)),
        new Vec3((double)(5), (double)(-5), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(5), (double)(5), (double)(0)),
        new Vec3((double)(-2), (double)(2), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(2), (double)(-2), (double)(0)),
        new Vec3((double)(5), (double)(-5), (double)(0)),
        new Vec3((double)(-5), (double)(-5), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(-2), (double)(2), (double)(0)),
        new Vec3((double)(-2), (double)(-2), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(-2), (double)(0)),
        new Vec3((double)(2), (double)(-2), (double)(0)),
        new Vec3((double)(-5), (double)(-5), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-5), (double)(-5), (double)(0)),
        new Vec3((double)(-5), (double)(5), (double)(0)),
        new Vec3((double)(-2), (double)(-2), (double)(0))
    }
  };
}