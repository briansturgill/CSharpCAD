namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeSimpleDefExp = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(5, -5, 0),
        new Vec3(-5, -5, 0),
        new Vec3(-5, 5, 0),
        new Vec3(5, 5, 0)
    }, 
      new List<Vec3> {
        new Vec3(5, 5, 1),
        new Vec3(-5, 5, 1),
        new Vec3(-5, -5, 1),
        new Vec3(5, -5, 1)
    }, 
      new List<Vec3> {
        new Vec3(5, 5, 0),
        new Vec3(-5, 5, 0),
        new Vec3(-5, 5, 1),
        new Vec3(5, 5, 1)
    }, 
      new List<Vec3> {
        new Vec3(-5, 5, 0),
        new Vec3(-5, -5, 0),
        new Vec3(-5, -5, 1),
        new Vec3(-5, 5, 1)
    }, 
      new List<Vec3> {
        new Vec3(-5, -5, 0),
        new Vec3(5, -5, 0),
        new Vec3(5, -5, 1),
        new Vec3(-5, -5, 1)
    }, 
      new List<Vec3> {
        new Vec3(5, -5, 0),
        new Vec3(5, 5, 0),
        new Vec3(5, 5, 1),
        new Vec3(5, -5, 1)
    }
  };
}
