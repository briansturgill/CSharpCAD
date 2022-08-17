namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> UnionGeom3Exp3 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(-9, -9, -9),
        new Vec3(0, 0, -9),
        new Vec3(9, -9, -9)
    }, 
      new List<Vec3> {
        new Vec3(-9, -9, -9),
        new Vec3(9, -9, -9),
        new Vec3(9, -9, 9)
    }, 
      new List<Vec3> {
        new Vec3(-9, -9, -9),
        new Vec3(9, -9, 9),
        new Vec3(-9, -9, 9)
    }, 
      new List<Vec3> {
        new Vec3(-9, -9, 9),
        new Vec3(9, -9, 9),
        new Vec3(0, 0, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, -9, -9),
        new Vec3(0, 0, -9),
        new Vec3(9, 9, -9)
    }, 
      new List<Vec3> {
        new Vec3(9, -9, -9),
        new Vec3(9, 9, -9),
        new Vec3(9, 9, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, -9, -9),
        new Vec3(9, 9, 9),
        new Vec3(9, -9, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, -9, 9),
        new Vec3(9, 9, 9),
        new Vec3(0, 0, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, -9),
        new Vec3(0, 0, -9),
        new Vec3(-9, 9, -9)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, -9),
        new Vec3(-9, 9, -9),
        new Vec3(-9, 9, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, -9),
        new Vec3(-9, 9, 9),
        new Vec3(9, 9, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, 9),
        new Vec3(-9, 9, 9),
        new Vec3(0, 0, 9)
    }, 
      new List<Vec3> {
        new Vec3(-9, 9, -9),
        new Vec3(0, 0, -9),
        new Vec3(-9, -9, -9)
    }, 
      new List<Vec3> {
        new Vec3(-9, 9, -9),
        new Vec3(-9, -9, -9),
        new Vec3(-9, -9, 9)
    }, 
      new List<Vec3> {
        new Vec3(-9, 9, -9),
        new Vec3(-9, -9, 9),
        new Vec3(-9, 9, 9)
    }, 
      new List<Vec3> {
        new Vec3(-9, 9, 9),
        new Vec3(-9, -9, 9),
        new Vec3(0, 0, 9)
    }
  };
}
