namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> UnionGeom3Exp2 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(8, 8, 12),
        new Vec3(12, 8, 12),
        new Vec3(10, 10, 12)
    }, 
      new List<Vec3> {
        new Vec3(12, 8, 8),
        new Vec3(10, 10, 8),
        new Vec3(12, 12, 8)
    }, 
      new List<Vec3> {
        new Vec3(12, 8, 8),
        new Vec3(12, 12, 8),
        new Vec3(12, 12, 12)
    }, 
      new List<Vec3> {
        new Vec3(12, 8, 8),
        new Vec3(12, 12, 12),
        new Vec3(12, 8, 12)
    }, 
      new List<Vec3> {
        new Vec3(12, 8, 12),
        new Vec3(12, 12, 12),
        new Vec3(10, 10, 12)
    }, 
      new List<Vec3> {
        new Vec3(12, 12, 8),
        new Vec3(10, 10, 8),
        new Vec3(8, 12, 8)
    }, 
      new List<Vec3> {
        new Vec3(12, 12, 8),
        new Vec3(8, 12, 8),
        new Vec3(8, 12, 12)
    }, 
      new List<Vec3> {
        new Vec3(12, 12, 8),
        new Vec3(8, 12, 12),
        new Vec3(12, 12, 12)
    }, 
      new List<Vec3> {
        new Vec3(12, 12, 12),
        new Vec3(8, 12, 12),
        new Vec3(10, 10, 12)
    }, 
      new List<Vec3> {
        new Vec3(8, 12, 8),
        new Vec3(8, 8, 12),
        new Vec3(8, 12, 12)
    }, 
      new List<Vec3> {
        new Vec3(8, 12, 12),
        new Vec3(8, 8, 12),
        new Vec3(10, 10, 12)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, 8),
        new Vec3(10, 10, 8),
        new Vec3(11, 9, 8)
    }, 
      new List<Vec3> {
        new Vec3(12, 8, 9),
        new Vec3(12, 8, 12),
        new Vec3(9, 8, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, 8, 9),
        new Vec3(12, 8, 12),
        new Vec3(8, 8, 12),
        new Vec3(8, 8, 9)
    }, 
      new List<Vec3> {
        new Vec3(8, 12, 8),
        new Vec3(10, 10, 8),
        new Vec3(9, 9, 8),
        new Vec3(8, 9, 8)
    }, 
      new List<Vec3> {
        new Vec3(8, 12, 8),
        new Vec3(8, 9, 8),
        new Vec3(8, 9, 11)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, 8),
        new Vec3(11, 9, 8),
        new Vec3(12, 8, 8),
        new Vec3(9, 8, 8)
    }, 
      new List<Vec3> {
        new Vec3(9, 8, 8),
        new Vec3(12, 8, 8),
        new Vec3(12, 8, 9),
        new Vec3(9, 8, 9)
    }, 
      new List<Vec3> {
        new Vec3(8, 8, 9),
        new Vec3(8, 8, 12),
        new Vec3(8, 9, 11),
        new Vec3(8, 9, 9)
    }, 
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
    }, 
      new List<Vec3> {
        new Vec3(9, -9, -9),
        new Vec3(9, 9, -9),
        new Vec3(9, 9, 8),
        new Vec3(9, 8, 8)
    }, 
      new List<Vec3> {
        new Vec3(9, -9, -9),
        new Vec3(9, 8, 8),
        new Vec3(9, -9, 8)
    }, 
      new List<Vec3> {
        new Vec3(8, 8, 9),
        new Vec3(0, 0, 9),
        new Vec3(8, -8, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, 9, -9),
        new Vec3(-8, 9, 8),
        new Vec3(9, 9, 8)
    }, 
      new List<Vec3> {
        new Vec3(8, 9, 9),
        new Vec3(-9, 9, 9),
        new Vec3(0, 0, 9),
        new Vec3(8, 8, 9)
    }, 
      new List<Vec3> {
        new Vec3(9, 8, 8),
        new Vec3(9, 8, 9),
        new Vec3(9, -9, 9),
        new Vec3(9, -9, 8)
    }, 
      new List<Vec3> {
        new Vec3(9, -9, 9),
        new Vec3(9, 8, 9),
        new Vec3(8, 8, 9),
        new Vec3(8, -8, 9)
    }, 
      new List<Vec3> {
        new Vec3(-8, 9, 8),
        new Vec3(-9, 9, 9),
        new Vec3(8, 9, 9),
        new Vec3(8, 9, 8)
    }
  };
}
