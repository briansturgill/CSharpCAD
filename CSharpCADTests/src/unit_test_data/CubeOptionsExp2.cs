namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> CubeOptionsExp2 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(-3.5, 3.5, -3.5),
        new Vec3(3.5, 3.5, -3.5),
        new Vec3(3.5, -3.5, -3.5),
        new Vec3(-3.5, -3.5, -3.5)
    }, 
      new List<Vec3> {
        new Vec3(-3.5, -3.5, 3.5),
        new Vec3(3.5, -3.5, 3.5),
        new Vec3(3.5, 3.5, 3.5),
        new Vec3(-3.5, 3.5, 3.5)
    }, 
      new List<Vec3> {
        new Vec3(-3.5, -3.5, -3.5),
        new Vec3(3.5, -3.5, -3.5),
        new Vec3(3.5, -3.5, 3.5),
        new Vec3(-3.5, -3.5, 3.5)
    }, 
      new List<Vec3> {
        new Vec3(3.5, -3.5, -3.5),
        new Vec3(3.5, 3.5, -3.5),
        new Vec3(3.5, 3.5, 3.5),
        new Vec3(3.5, -3.5, 3.5)
    }, 
      new List<Vec3> {
        new Vec3(3.5, 3.5, -3.5),
        new Vec3(-3.5, 3.5, -3.5),
        new Vec3(-3.5, 3.5, 3.5),
        new Vec3(3.5, 3.5, 3.5)
    }, 
      new List<Vec3> {
        new Vec3(-3.5, 3.5, -3.5),
        new Vec3(-3.5, -3.5, -3.5),
        new Vec3(-3.5, -3.5, 3.5),
        new Vec3(-3.5, 3.5, 3.5)
    }
  };
}
