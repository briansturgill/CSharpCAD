namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> CylinderOptsExp1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(1.236067977499789, -3.8042260651806146, 0),
        new Vec3(-3.2360679774997902, -2.351141009169892, 0),
        new Vec3(-3.2360679774997894, 2.351141009169893, 0),
        new Vec3(1.2360679774997898, 3.804226065180614, 0),
        new Vec3(4, 0, 0)
    }, 
      new List<Vec3> {
        new Vec3(4, 0, 10),
        new Vec3(1.2360679774997898, 3.804226065180614, 10),
        new Vec3(-3.2360679774997894, 2.351141009169893, 10),
        new Vec3(-3.2360679774997902, -2.351141009169892, 10),
        new Vec3(1.236067977499789, -3.8042260651806146, 10)
    }, 
      new List<Vec3> {
        new Vec3(4, 0, 0),
        new Vec3(1.2360679774997898, 3.804226065180614, 0),
        new Vec3(1.2360679774997898, 3.804226065180614, 10),
        new Vec3(4, 0, 10)
    }, 
      new List<Vec3> {
        new Vec3(1.2360679774997898, 3.804226065180614, 0),
        new Vec3(-3.2360679774997894, 2.351141009169893, 0),
        new Vec3(-3.2360679774997894, 2.351141009169893, 10),
        new Vec3(1.2360679774997898, 3.804226065180614, 10)
    }, 
      new List<Vec3> {
        new Vec3(-3.2360679774997894, 2.351141009169893, 0),
        new Vec3(-3.2360679774997902, -2.351141009169892, 0),
        new Vec3(-3.2360679774997902, -2.351141009169892, 10),
        new Vec3(-3.2360679774997894, 2.351141009169893, 10)
    }, 
      new List<Vec3> {
        new Vec3(-3.2360679774997902, -2.351141009169892, 0),
        new Vec3(1.236067977499789, -3.8042260651806146, 0),
        new Vec3(1.236067977499789, -3.8042260651806146, 10),
        new Vec3(-3.2360679774997902, -2.351141009169892, 10)
    }, 
      new List<Vec3> {
        new Vec3(1.236067977499789, -3.8042260651806146, 0),
        new Vec3(4, 0, 0),
        new Vec3(4, 0, 10),
        new Vec3(1.236067977499789, -3.8042260651806146, 10)
    }
  };
}
