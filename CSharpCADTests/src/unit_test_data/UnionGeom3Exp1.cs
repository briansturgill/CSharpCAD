namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> UnionGeom3Exp1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3(2, 0, 0),
        new Vec3(1.4142135623730951, -1.414213562373095, 0),
        new Vec3(1.0000000000000002, -1, -1.414213562373095),
        new Vec3(1.4142135623730951, 0, -1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(1.4142135623730951, 0, 1.414213562373095),
        new Vec3(1.0000000000000002, -1, 1.414213562373095),
        new Vec3(1.4142135623730951, -1.414213562373095, 0),
        new Vec3(2, 0, 0)
    }, 
      new List<Vec3> {
        new Vec3(1.4142135623730951, 0, -1.414213562373095),
        new Vec3(1.0000000000000002, -1, -1.414213562373095),
        new Vec3(0, 0, -2)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 2),
        new Vec3(1.0000000000000002, -1, 1.414213562373095),
        new Vec3(1.4142135623730951, 0, 1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(1.4142135623730951, -1.414213562373095, 0),
        new Vec3(0, -2, 0),
        new Vec3(0, -1.4142135623730951, -1.414213562373095),
        new Vec3(1.0000000000000002, -1, -1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(1.0000000000000002, -1, 1.414213562373095),
        new Vec3(0, -1.4142135623730951, 1.414213562373095),
        new Vec3(0, -2, 0),
        new Vec3(1.4142135623730951, -1.414213562373095, 0)
    }, 
      new List<Vec3> {
        new Vec3(1.0000000000000002, -1, -1.414213562373095),
        new Vec3(0, -1.4142135623730951, -1.414213562373095),
        new Vec3(0, 0, -2)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 2),
        new Vec3(0, -1.4142135623730951, 1.414213562373095),
        new Vec3(1.0000000000000002, -1, 1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(0, -2, 0),
        new Vec3(-1.414213562373095, -1.4142135623730951, 0),
        new Vec3(-1, -1.0000000000000002, -1.414213562373095),
        new Vec3(0, -1.4142135623730951, -1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(0, -1.4142135623730951, 1.414213562373095),
        new Vec3(-1, -1.0000000000000002, 1.414213562373095),
        new Vec3(-1.414213562373095, -1.4142135623730951, 0),
        new Vec3(0, -2, 0)
    }, 
      new List<Vec3> {
        new Vec3(0, -1.4142135623730951, -1.414213562373095),
        new Vec3(-1, -1.0000000000000002, -1.414213562373095),
        new Vec3(0, 0, -2)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 2),
        new Vec3(-1, -1.0000000000000002, 1.414213562373095),
        new Vec3(0, -1.4142135623730951, 1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(-1.414213562373095, -1.4142135623730951, 0),
        new Vec3(-2, 0, 0),
        new Vec3(-1.4142135623730951, 0, -1.414213562373095),
        new Vec3(-1, -1.0000000000000002, -1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(-1, -1.0000000000000002, 1.414213562373095),
        new Vec3(-1.4142135623730951, 0, 1.414213562373095),
        new Vec3(-2, 0, 0),
        new Vec3(-1.414213562373095, -1.4142135623730951, 0)
    }, 
      new List<Vec3> {
        new Vec3(-1, -1.0000000000000002, -1.414213562373095),
        new Vec3(-1.4142135623730951, 0, -1.414213562373095),
        new Vec3(0, 0, -2)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 2),
        new Vec3(-1.4142135623730951, 0, 1.414213562373095),
        new Vec3(-1, -1.0000000000000002, 1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(-2, 0, 0),
        new Vec3(-1.4142135623730954, 1.414213562373095, 0),
        new Vec3(-1.0000000000000002, 1, -1.414213562373095),
        new Vec3(-1.4142135623730951, 0, -1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(-1.4142135623730951, 0, 1.414213562373095),
        new Vec3(-1.0000000000000002, 1, 1.414213562373095),
        new Vec3(-1.4142135623730954, 1.414213562373095, 0),
        new Vec3(-2, 0, 0)
    }, 
      new List<Vec3> {
        new Vec3(-1.4142135623730951, 0, -1.414213562373095),
        new Vec3(-1.0000000000000002, 1, -1.414213562373095),
        new Vec3(0, 0, -2)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 2),
        new Vec3(-1.0000000000000002, 1, 1.414213562373095),
        new Vec3(-1.4142135623730951, 0, 1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(-1.4142135623730954, 1.414213562373095, 0),
        new Vec3(0, 2, 0),
        new Vec3(0, 1.4142135623730951, -1.414213562373095),
        new Vec3(-1.0000000000000002, 1, -1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(-1.0000000000000002, 1, 1.414213562373095),
        new Vec3(0, 1.4142135623730951, 1.414213562373095),
        new Vec3(0, 2, 0),
        new Vec3(-1.4142135623730954, 1.414213562373095, 0)
    }, 
      new List<Vec3> {
        new Vec3(-1.0000000000000002, 1, -1.414213562373095),
        new Vec3(0, 1.4142135623730951, -1.414213562373095),
        new Vec3(0, 0, -2)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 2),
        new Vec3(0, 1.4142135623730951, 1.414213562373095),
        new Vec3(-1.0000000000000002, 1, 1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(0, 2, 0),
        new Vec3(1.4142135623730947, 1.4142135623730954, 0),
        new Vec3(0.9999999999999998, 1.0000000000000002, -1.414213562373095),
        new Vec3(0, 1.4142135623730951, -1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(0, 1.4142135623730951, 1.414213562373095),
        new Vec3(0.9999999999999998, 1.0000000000000002, 1.414213562373095),
        new Vec3(1.4142135623730947, 1.4142135623730954, 0),
        new Vec3(0, 2, 0)
    }, 
      new List<Vec3> {
        new Vec3(0, 1.4142135623730951, -1.414213562373095),
        new Vec3(0.9999999999999998, 1.0000000000000002, -1.414213562373095),
        new Vec3(0, 0, -2)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 2),
        new Vec3(0.9999999999999998, 1.0000000000000002, 1.414213562373095),
        new Vec3(0, 1.4142135623730951, 1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(1.4142135623730947, 1.4142135623730954, 0),
        new Vec3(2, 0, 0),
        new Vec3(1.4142135623730951, 0, -1.414213562373095),
        new Vec3(0.9999999999999998, 1.0000000000000002, -1.414213562373095)
    }, 
      new List<Vec3> {
        new Vec3(0.9999999999999998, 1.0000000000000002, 1.414213562373095),
        new Vec3(1.4142135623730951, 0, 1.414213562373095),
        new Vec3(2, 0, 0),
        new Vec3(1.4142135623730947, 1.4142135623730954, 0)
    }, 
      new List<Vec3> {
        new Vec3(0.9999999999999998, 1.0000000000000002, -1.414213562373095),
        new Vec3(1.4142135623730951, 0, -1.414213562373095),
        new Vec3(0, 0, -2)
    }, 
      new List<Vec3> {
        new Vec3(0, 0, 2),
        new Vec3(1.4142135623730951, 0, 1.414213562373095),
        new Vec3(0.9999999999999998, 1.0000000000000002, 1.414213562373095)
    }
  };
}
