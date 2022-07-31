namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> CubeOptionsExp1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(3, 3, 3),
        new Vec3(6.5, 6.5, 3),
        new Vec3(10, 3, 3)
    }, 
      new List<Vec3> {
        new Vec3(3, 3, 3),
        new Vec3(10, 3, 3),
        new Vec3(10, 3, 10)
    }, 
      new List<Vec3> {
        new Vec3(3, 3, 3),
        new Vec3(10, 3, 10),
        new Vec3(3, 3, 10)
    }, 
      new List<Vec3> {
        new Vec3(3, 3, 10),
        new Vec3(10, 3, 10),
        new Vec3(6.5, 6.5, 10)
    }, 
      new List<Vec3> {
        new Vec3(10, 3, 3),
        new Vec3(6.5, 6.5, 3),
        new Vec3(10, 10, 3)
    }, 
      new List<Vec3> {
        new Vec3(10, 3, 3),
        new Vec3(10, 10, 3),
        new Vec3(10, 10, 10)
    }, 
      new List<Vec3> {
        new Vec3(10, 3, 3),
        new Vec3(10, 10, 10),
        new Vec3(10, 3, 10)
    }, 
      new List<Vec3> {
        new Vec3(10, 3, 10),
        new Vec3(10, 10, 10),
        new Vec3(6.5, 6.5, 10)
    }, 
      new List<Vec3> {
        new Vec3(10, 10, 3),
        new Vec3(6.5, 6.5, 3),
        new Vec3(3, 10, 3)
    }, 
      new List<Vec3> {
        new Vec3(10, 10, 3),
        new Vec3(3, 10, 3),
        new Vec3(3, 10, 10)
    }, 
      new List<Vec3> {
        new Vec3(10, 10, 3),
        new Vec3(3, 10, 10),
        new Vec3(10, 10, 10)
    }, 
      new List<Vec3> {
        new Vec3(10, 10, 10),
        new Vec3(3, 10, 10),
        new Vec3(6.5, 6.5, 10)
    }, 
      new List<Vec3> {
        new Vec3(3, 10, 3),
        new Vec3(6.5, 6.5, 3),
        new Vec3(3, 3, 3)
    }, 
      new List<Vec3> {
        new Vec3(3, 10, 3),
        new Vec3(3, 3, 3),
        new Vec3(3, 3, 10)
    }, 
      new List<Vec3> {
        new Vec3(3, 10, 3),
        new Vec3(3, 3, 10),
        new Vec3(3, 10, 10)
    }, 
      new List<Vec3> {
        new Vec3(3, 10, 10),
        new Vec3(3, 3, 10),
        new Vec3(6.5, 6.5, 10)
    }
  };
}
