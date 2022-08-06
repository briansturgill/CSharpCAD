namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeLinearNoTwistExp2 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(5, 5, -15),
        new Vec3(-5, 5, -15),
        new Vec3(-5, 5, 0)
    }, 
      new List<Vec3> {
        new Vec3(5, 5, -15),
        new Vec3(-5, 5, 0),
        new Vec3(5, 5, 0)
    }, 
      new List<Vec3> {
        new Vec3(5, 5, -15),
        new Vec3(0, 0, -15),
        new Vec3(-5, 5, -15)
    }, 
      new List<Vec3> {
        new Vec3(5, 5, 0),
        new Vec3(-5, 5, 0),
        new Vec3(0, 0, 0)
    }, 
      new List<Vec3> {
        new Vec3(-5, 5, -15),
        new Vec3(-5, -5, -15),
        new Vec3(-5, -5, 0)
    }, 
      new List<Vec3> {
        new Vec3(-5, 5, -15),
        new Vec3(-5, -5, 0),
        new Vec3(-5, 5, 0)
    }, 
      new List<Vec3> {
        new Vec3(-5, 5, -15),
        new Vec3(0, 0, -15),
        new Vec3(-5, -5, -15)
    }, 
      new List<Vec3> {
        new Vec3(-5, 5, 0),
        new Vec3(-5, -5, 0),
        new Vec3(0, 0, 0)
    }, 
      new List<Vec3> {
        new Vec3(-5, -5, -15),
        new Vec3(5, -5, -15),
        new Vec3(5, -5, 0)
    }, 
      new List<Vec3> {
        new Vec3(-5, -5, -15),
        new Vec3(5, -5, 0),
        new Vec3(-5, -5, 0)
    }, 
      new List<Vec3> {
        new Vec3(-5, -5, -15),
        new Vec3(0, 0, -15),
        new Vec3(5, -5, -15)
    }, 
      new List<Vec3> {
        new Vec3(-5, -5, 0),
        new Vec3(5, -5, 0),
        new Vec3(0, 0, 0)
    }, 
      new List<Vec3> {
        new Vec3(5, -5, -15),
        new Vec3(5, 5, -15),
        new Vec3(5, 5, 0)
    }, 
      new List<Vec3> {
        new Vec3(5, -5, -15),
        new Vec3(5, 5, 0),
        new Vec3(5, -5, 0)
    }, 
      new List<Vec3> {
        new Vec3(5, -5, -15),
        new Vec3(0, 0, -15),
        new Vec3(5, 5, -15)
    }, 
      new List<Vec3> {
        new Vec3(5, -5, 0),
        new Vec3(5, 5, 0),
        new Vec3(0, 0, 0)
    }
  };
}
