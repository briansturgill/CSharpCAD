namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeLinearTwistExp = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(0, 0, 0),
        new Vec3(-5, 5, 0),
        new Vec3(5, 5, 0)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 0),
        new Vec3(-5, -5, 0),
        new Vec3(-5, 5, 0)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 0),
        new Vec3(5, -5, 0),
        new Vec3(-5, -5, 0)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 0),
        new Vec3(5, 5, 0),
        new Vec3(5, -5, 0)
    }, 
      new List<Vec3> {
        new Vec3(5, 5, 0),
        new Vec3(-5, 5, 0),
        new Vec3(-4.440892098500626E-16, 7.0710678118654755, 15)
    }, 
      new List<Vec3> {
        new Vec3(5, 5, 0),
        new Vec3(-4.440892098500626E-16, 7.0710678118654755, 15),
        new Vec3(7.0710678118654755, 4.440892098500626E-16, 15)
    }, 
      new List<Vec3> {
        new Vec3(-5, 5, 0),
        new Vec3(-5, -5, 0),
        new Vec3(-7.0710678118654755, -4.440892098500626E-16, 15)
    }, 
      new List<Vec3> {
        new Vec3(-5, 5, 0),
        new Vec3(-7.0710678118654755, -4.440892098500626E-16, 15),
        new Vec3(-4.440892098500626E-16, 7.0710678118654755, 15)
    }, 
      new List<Vec3> {
        new Vec3(-5, -5, 0),
        new Vec3(5, -5, 0),
        new Vec3(4.440892098500626E-16, -7.0710678118654755, 15)
    }, 
      new List<Vec3> {
        new Vec3(-5, -5, 0),
        new Vec3(4.440892098500626E-16, -7.0710678118654755, 15),
        new Vec3(-7.0710678118654755, -4.440892098500626E-16, 15)
    }, 
      new List<Vec3> {
        new Vec3(5, -5, 0),
        new Vec3(5, 5, 0),
        new Vec3(7.0710678118654755, 4.440892098500626E-16, 15)
    }, 
      new List<Vec3> {
        new Vec3(5, -5, 0),
        new Vec3(7.0710678118654755, 4.440892098500626E-16, 15),
        new Vec3(4.440892098500626E-16, -7.0710678118654755, 15)
    }, 
      new List<Vec3> {
        new Vec3(7.0710678118654755, 4.440892098500626E-16, 15),
        new Vec3(-4.440892098500626E-16, 7.0710678118654755, 15),
        new Vec3(1.1102230246251565E-16, 0, 15)
    }, 
      new List<Vec3> {
        new Vec3(-4.440892098500626E-16, 7.0710678118654755, 15),
        new Vec3(-7.0710678118654755, -4.440892098500626E-16, 15),
        new Vec3(1.1102230246251565E-16, 0, 15)
    }, 
      new List<Vec3> {
        new Vec3(-7.0710678118654755, -4.440892098500626E-16, 15),
        new Vec3(4.440892098500626E-16, -7.0710678118654755, 15),
        new Vec3(1.1102230246251565E-16, 0, 15)
    }, 
      new List<Vec3> {
        new Vec3(4.440892098500626E-16, -7.0710678118654755, 15),
        new Vec3(7.0710678118654755, 4.440892098500626E-16, 15),
        new Vec3(1.1102230246251565E-16, 0, 15)
    }
  };
}
