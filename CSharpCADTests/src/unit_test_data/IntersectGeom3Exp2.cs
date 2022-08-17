namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> IntersectGeom3Exp2 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(9, 9, 8),
        new Vec3(9, 9, 9),
        new Vec3(9, 8, 8)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, 9),
        new Vec3(8, 9, 9),
        new Vec3(8, 8, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, 9),
        new Vec3(9, 8, 9),
        new Vec3(9, 8, 8)
    }, 
      new List<Vec3> {
        new Vec3(9, 8, 9),
        new Vec3(9, 9, 9),
        new Vec3(8, 8, 9)
    }, 
      new List<Vec3> {
        new Vec3(8, 9, 9),
        new Vec3(9, 9, 9),
        new Vec3(9, 9, 8),
        new Vec3(8, 9, 8)
    }, 
      new List<Vec3> {
        new Vec3(8, 8, 9),
        new Vec3(8, 8, 8),
        new Vec3(9, 8, 9)
    }, 
      new List<Vec3> {
        new Vec3(8, 9, 8),
        new Vec3(9, 9, 8),
        new Vec3(8, 8, 8)
    }, 
      new List<Vec3> {
        new Vec3(9, 8, 8),
        new Vec3(8, 8, 8),
        new Vec3(9, 9, 8)
    }, 
      new List<Vec3> {
        new Vec3(9, 8, 9),
        new Vec3(8, 8, 8),
        new Vec3(9, 8, 8)
    }, 
      new List<Vec3> {
        new Vec3(8, 9, 9),
        new Vec3(8, 9, 8),
        new Vec3(8, 8, 8),
        new Vec3(8, 8, 9)
    }
  };
}
