namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> HullMultipleGeom3Exp2 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(3.5, 3.5, 6.5),
        new Vec3(6.5, 3.5, 6.5),
        new Vec3(6.5, 6.5, 6.5),
        new Vec3(3.5, 6.5, 6.5)
    }, 
      new List<Vec3> {
        new Vec3(6.5, 6.5, 3.5),
        new Vec3(1, 1, -1),
        new Vec3(-1, 1, -1),
        new Vec3(3.5, 6.5, 3.5)
    }, 
      new List<Vec3> {
        new Vec3(6.5, 6.5, 3.5),
        new Vec3(3.5, 6.5, 3.5),
        new Vec3(3.5, 6.5, 6.5),
        new Vec3(6.5, 6.5, 6.5)
    }, 
      new List<Vec3> {
        new Vec3(6.5, 6.5, 3.5),
        new Vec3(6.5, 6.5, 6.5),
        new Vec3(6.5, 3.5, 6.5),
        new Vec3(6.5, 3.5, 3.5)
    }, 
      new List<Vec3> {
        new Vec3(1, -1, -1),
        new Vec3(-1, -1, -1),
        new Vec3(-1, 1, -1),
        new Vec3(1, 1, -1)
    }, 
      new List<Vec3> {
        new Vec3(1, -1, -1),
        new Vec3(1, 1, -1),
        new Vec3(6.5, 6.5, 3.5),
        new Vec3(6.5, 3.5, 3.5)
    }, 
      new List<Vec3> {
        new Vec3(1, -1, -1),
        new Vec3(6.5, 3.5, 3.5),
        new Vec3(6.5, 3.5, 6.5),
        new Vec3(1, -1, 1)
    }, 
      new List<Vec3> {
        new Vec3(-1, -1, 1),
        new Vec3(-1, -1, -1),
        new Vec3(1, -1, -1),
        new Vec3(1, -1, 1)
    }, 
      new List<Vec3> {
        new Vec3(-1, -1, 1),
        new Vec3(1, -1, 1),
        new Vec3(6.5, 3.5, 6.5),
        new Vec3(3.5, 3.5, 6.5)
    }, 
      new List<Vec3> {
        new Vec3(-1, 1, 1),
        new Vec3(-1, 1, -1),
        new Vec3(-1, -1, -1),
        new Vec3(-1, -1, 1)
    }, 
      new List<Vec3> {
        new Vec3(-1, 1, 1),
        new Vec3(-1, -1, 1),
        new Vec3(3.5, 3.5, 6.5),
        new Vec3(3.5, 6.5, 6.5)
    }, 
      new List<Vec3> {
        new Vec3(-1, 1, 1),
        new Vec3(3.5, 6.5, 6.5),
        new Vec3(3.5, 6.5, 3.5),
        new Vec3(-1, 1, -1)
    }
  };
}
