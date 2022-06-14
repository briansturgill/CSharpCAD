namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> SnapGeom3Exp1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(-0.5, 0.5, -0.5),
        new Vec3(0.5, 0.5, -0.5),
        new Vec3(0.5, -0.5, -0.5),
        new Vec3(-0.5, -0.5, -0.5)
    }, 
      new List<Vec3> {
        new Vec3(-0.5, -0.5, 0.5),
        new Vec3(0.5, -0.5, 0.5),
        new Vec3(0.5, 0.5, 0.5),
        new Vec3(-0.5, 0.5, 0.5)
    }, 
      new List<Vec3> {
        new Vec3(-0.5, -0.5, -0.5),
        new Vec3(0.5, -0.5, -0.5),
        new Vec3(0.5, -0.5, 0.5),
        new Vec3(-0.5, -0.5, 0.5)
    }, 
      new List<Vec3> {
        new Vec3(0.5, -0.5, -0.5),
        new Vec3(0.5, 0.5, -0.5),
        new Vec3(0.5, 0.5, 0.5),
        new Vec3(0.5, -0.5, 0.5)
    }, 
      new List<Vec3> {
        new Vec3(0.5, 0.5, -0.5),
        new Vec3(-0.5, 0.5, -0.5),
        new Vec3(-0.5, 0.5, 0.5),
        new Vec3(0.5, 0.5, 0.5)
    }, 
      new List<Vec3> {
        new Vec3(-0.5, 0.5, -0.5),
        new Vec3(-0.5, -0.5, -0.5),
        new Vec3(-0.5, -0.5, 0.5),
        new Vec3(-0.5, 0.5, 0.5)
    }
  };
}
