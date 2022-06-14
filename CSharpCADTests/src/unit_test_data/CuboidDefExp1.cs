namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> CuboidDefExp1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(-1, 1, -1),
        new Vec3(1, 1, -1),
        new Vec3(1, -1, -1),
        new Vec3(-1, -1, -1)
    }, 
      new List<Vec3> {
        new Vec3(-1, -1, 1),
        new Vec3(1, -1, 1),
        new Vec3(1, 1, 1),
        new Vec3(-1, 1, 1)
    }, 
      new List<Vec3> {
        new Vec3(-1, -1, -1),
        new Vec3(1, -1, -1),
        new Vec3(1, -1, 1),
        new Vec3(-1, -1, 1)
    }, 
      new List<Vec3> {
        new Vec3(1, -1, -1),
        new Vec3(1, 1, -1),
        new Vec3(1, 1, 1),
        new Vec3(1, -1, 1)
    }, 
      new List<Vec3> {
        new Vec3(1, 1, -1),
        new Vec3(-1, 1, -1),
        new Vec3(-1, 1, 1),
        new Vec3(1, 1, 1)
    }, 
      new List<Vec3> {
        new Vec3(-1, 1, -1),
        new Vec3(-1, -1, -1),
        new Vec3(-1, -1, 1),
        new Vec3(-1, 1, 1)
    }
  };
}
