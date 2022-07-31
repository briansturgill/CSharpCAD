namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> ExtrudeRotateOverlapExp = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(0, 4.898587196589413E-16, 8),
        new Vec3(7, 4.898587196589413E-16, 8),
        new Vec3(-6.123233995736767E-17, 7, 8)
    }, 
      new List<Vec3> {
        new Vec3(7, -4.898587196589413E-16, -8),
        new Vec3(4.898587196589413E-16, -2.999519565323715E-32, -8),
        new Vec3(9.184850993605148E-16, 7, -8)
    }, 
      new List<Vec3> {
        new Vec3(7, 4.898587196589413E-16, 8),
        new Vec3(7, -4.898587196589413E-16, -8),
        new Vec3(9.184850993605148E-16, 7, -8)
    }, 
      new List<Vec3> {
        new Vec3(7, 4.898587196589413E-16, 8),
        new Vec3(9.184850993605148E-16, 7, -8),
        new Vec3(-6.123233995736767E-17, 7, 8)
    }, 
      new List<Vec3> {
        new Vec3(4.898587196589413E-16, -2.999519565323715E-32, -8),
        new Vec3(-4.898587196589413E-16, 2.999519565323715E-32, 8),
        new Vec3(-6.123233995736767E-17, 7, 8)
    }, 
      new List<Vec3> {
        new Vec3(-6.123233995736767E-17, 7, 8),
        new Vec3(9.184850993605148E-16, 7, -8),
        new Vec3(4.898587196589413E-16, -2.999519565323715E-32, -8)
    }, 
      new List<Vec3> {
        new Vec3(7, 4.898587196589413E-16, 8),
        new Vec3(0, 4.898587196589413E-16, 8),
        new Vec3(0, -4.898587196589413E-16, -8)
    }, 
      new List<Vec3> {
        new Vec3(0, -4.898587196589413E-16, -8),
        new Vec3(7, -4.898587196589413E-16, -8),
        new Vec3(7, 4.898587196589413E-16, 8)
    }
  };
}
