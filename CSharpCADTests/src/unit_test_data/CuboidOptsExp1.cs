namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> CuboidOptsExp1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(0, 2, 4),
        new Vec3(3, 5, 4),
        new Vec3(6, 2, 4)
    }, 
      new List<Vec3> {
        new Vec3(0, 2, 4),
        new Vec3(6, 2, 4),
        new Vec3(6, 2, 10)
    }, 
      new List<Vec3> {
        new Vec3(0, 2, 4),
        new Vec3(6, 2, 10),
        new Vec3(0, 2, 10)
    }, 
      new List<Vec3> {
        new Vec3(0, 2, 10),
        new Vec3(6, 2, 10),
        new Vec3(3, 5, 10)
    }, 
      new List<Vec3> {
        new Vec3(6, 2, 4),
        new Vec3(3, 5, 4),
        new Vec3(6, 8, 4)
    }, 
      new List<Vec3> {
        new Vec3(6, 2, 4),
        new Vec3(6, 8, 4),
        new Vec3(6, 8, 10)
    }, 
      new List<Vec3> {
        new Vec3(6, 2, 4),
        new Vec3(6, 8, 10),
        new Vec3(6, 2, 10)
    }, 
      new List<Vec3> {
        new Vec3(6, 2, 10),
        new Vec3(6, 8, 10),
        new Vec3(3, 5, 10)
    }, 
      new List<Vec3> {
        new Vec3(6, 8, 4),
        new Vec3(3, 5, 4),
        new Vec3(0, 8, 4)
    }, 
      new List<Vec3> {
        new Vec3(6, 8, 4),
        new Vec3(0, 8, 4),
        new Vec3(0, 8, 10)
    }, 
      new List<Vec3> {
        new Vec3(6, 8, 4),
        new Vec3(0, 8, 10),
        new Vec3(6, 8, 10)
    }, 
      new List<Vec3> {
        new Vec3(6, 8, 10),
        new Vec3(0, 8, 10),
        new Vec3(3, 5, 10)
    }, 
      new List<Vec3> {
        new Vec3(0, 8, 4),
        new Vec3(3, 5, 4),
        new Vec3(0, 2, 4)
    }, 
      new List<Vec3> {
        new Vec3(0, 8, 4),
        new Vec3(0, 2, 4),
        new Vec3(0, 2, 10)
    }, 
      new List<Vec3> {
        new Vec3(0, 8, 4),
        new Vec3(0, 2, 10),
        new Vec3(0, 8, 10)
    }, 
      new List<Vec3> {
        new Vec3(0, 8, 10),
        new Vec3(0, 2, 10),
        new Vec3(3, 5, 10)
    }
  };
}
