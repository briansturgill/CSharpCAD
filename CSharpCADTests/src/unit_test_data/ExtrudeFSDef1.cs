namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeFSDef1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(10, -10, 0),
        new Vec3(10, 10, 0),
        new Vec3(10, 10, 1)
    }, 
      new List<Vec3> {
        new Vec3(10, -10, 0),
        new Vec3(10, 10, 1),
        new Vec3(10, -10, 1)
    }, 
      new List<Vec3> {
        new Vec3(10, 10, 0),
        new Vec3(-10, 10, 0),
        new Vec3(-10, 10, 1)
    }, 
      new List<Vec3> {
        new Vec3(10, 10, 0),
        new Vec3(-10, 10, 1),
        new Vec3(10, 10, 1)
    }, 
      new List<Vec3> {
        new Vec3(-10, 10, 0),
        new Vec3(-10, -10, 0),
        new Vec3(-10, -10, 1)
    }, 
      new List<Vec3> {
        new Vec3(-10, 10, 0),
        new Vec3(-10, -10, 1),
        new Vec3(-10, 10, 1)
    }, 
      new List<Vec3> {
        new Vec3(-10, -10, 0),
        new Vec3(10, -10, 0),
        new Vec3(10, -10, 1)
    }, 
      new List<Vec3> {
        new Vec3(-10, -10, 0),
        new Vec3(10, -10, 1),
        new Vec3(-10, -10, 1)
    }, 
      new List<Vec3> {
        new Vec3(-10, -10, 1),
        new Vec3(10, -10, 1),
        new Vec3(10, 10, 1)
    }, 
      new List<Vec3> {
        new Vec3(10, 10, 1),
        new Vec3(-10, 10, 1),
        new Vec3(-10, -10, 1)
    }, 
      new List<Vec3> {
        new Vec3(10, 10, 0),
        new Vec3(10, -10, 0),
        new Vec3(-10, -10, 0)
    }, 
      new List<Vec3> {
        new Vec3(-10, -10, 0),
        new Vec3(-10, 10, 0),
        new Vec3(10, 10, 0)
    }
  };
}
