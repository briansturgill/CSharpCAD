namespace CSharpCADTests;

using NUnit.Framework;

public static partial class UnitTestData {
  public static List<List<Vec3>> IntersectGeom3Exp1 = new List<List<Vec3>> {
      new List<Vec3> {
        new Vec3((double)(2), (double)(0), (double)(0)),
        new Vec3((double)(1.4142135623730951), (double)(-1.414213562373095), (double)(0)),
        new Vec3((double)(1.0000000000000002), (double)(-1), (double)(-1.414213562373095)),
        new Vec3((double)(1.4142135623730951), (double)(0), (double)(-1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(1.4142135623730951), (double)(0), (double)(1.414213562373095)),
        new Vec3((double)(1.0000000000000002), (double)(-1), (double)(1.414213562373095)),
        new Vec3((double)(1.4142135623730951), (double)(-1.414213562373095), (double)(0)),
        new Vec3((double)(2), (double)(0), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(1.4142135623730951), (double)(0), (double)(-1.414213562373095)),
        new Vec3((double)(1.0000000000000002), (double)(-1), (double)(-1.414213562373095)),
        new Vec3((double)(1.2246467991473532e-16), (double)(0), (double)(-2))
    },
      new List<Vec3> {
        new Vec3((double)(1.2246467991473532e-16), (double)(0), (double)(2)),
        new Vec3((double)(1.0000000000000002), (double)(-1), (double)(1.414213562373095)),
        new Vec3((double)(1.4142135623730951), (double)(0), (double)(1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(1.4142135623730951), (double)(-1.414213562373095), (double)(0)),
        new Vec3((double)(1.2246467991473532e-16), (double)(-2), (double)(0)),
        new Vec3((double)(8.659560562354934e-17), (double)(-1.4142135623730951), (double)(-1.414213562373095)),
        new Vec3((double)(1.0000000000000002), (double)(-1), (double)(-1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(1.0000000000000002), (double)(-1), (double)(1.414213562373095)),
        new Vec3((double)(8.659560562354934e-17), (double)(-1.4142135623730951), (double)(1.414213562373095)),
        new Vec3((double)(1.2246467991473532e-16), (double)(-2), (double)(0)),
        new Vec3((double)(1.4142135623730951), (double)(-1.414213562373095), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(1.0000000000000002), (double)(-1), (double)(-1.414213562373095)),
        new Vec3((double)(8.659560562354934e-17), (double)(-1.4142135623730951), (double)(-1.414213562373095)),
        new Vec3((double)(8.659560562354934e-17), (double)(-8.659560562354932e-17), (double)(-2))
    },
      new List<Vec3> {
        new Vec3((double)(8.659560562354934e-17), (double)(-8.659560562354932e-17), (double)(2)),
        new Vec3((double)(8.659560562354934e-17), (double)(-1.4142135623730951), (double)(1.414213562373095)),
        new Vec3((double)(1.0000000000000002), (double)(-1), (double)(1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(1.2246467991473532e-16), (double)(-2), (double)(0)),
        new Vec3((double)(-1.414213562373095), (double)(-1.4142135623730951), (double)(0)),
        new Vec3((double)(-1), (double)(-1.0000000000000002), (double)(-1.414213562373095)),
        new Vec3((double)(8.659560562354934e-17), (double)(-1.4142135623730951), (double)(-1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(8.659560562354934e-17), (double)(-1.4142135623730951), (double)(1.414213562373095)),
        new Vec3((double)(-1), (double)(-1.0000000000000002), (double)(1.414213562373095)),
        new Vec3((double)(-1.414213562373095), (double)(-1.4142135623730951), (double)(0)),
        new Vec3((double)(1.2246467991473532e-16), (double)(-2), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(8.659560562354934e-17), (double)(-1.4142135623730951), (double)(-1.414213562373095)),
        new Vec3((double)(-1), (double)(-1.0000000000000002), (double)(-1.414213562373095)),
        new Vec3((double)(7.498798913309288e-33), (double)(-1.2246467991473532e-16), (double)(-2))
    },
      new List<Vec3> {
        new Vec3((double)(7.498798913309288e-33), (double)(-1.2246467991473532e-16), (double)(2)),
        new Vec3((double)(-1), (double)(-1.0000000000000002), (double)(1.414213562373095)),
        new Vec3((double)(8.659560562354934e-17), (double)(-1.4142135623730951), (double)(1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(-1.414213562373095), (double)(-1.4142135623730951), (double)(0)),
        new Vec3((double)(-2), (double)(-2.4492935982947064e-16), (double)(0)),
        new Vec3((double)(-1.4142135623730951), (double)(-1.7319121124709868e-16), (double)(-1.414213562373095)),
        new Vec3((double)(-1), (double)(-1.0000000000000002), (double)(-1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(-1), (double)(-1.0000000000000002), (double)(1.414213562373095)),
        new Vec3((double)(-1.4142135623730951), (double)(-1.7319121124709868e-16), (double)(1.414213562373095)),
        new Vec3((double)(-2), (double)(-2.4492935982947064e-16), (double)(0)),
        new Vec3((double)(-1.414213562373095), (double)(-1.4142135623730951), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-1), (double)(-1.0000000000000002), (double)(-1.414213562373095)),
        new Vec3((double)(-1.4142135623730951), (double)(-1.7319121124709868e-16), (double)(-1.414213562373095)),
        new Vec3((double)(-8.659560562354932e-17), (double)(-8.659560562354934e-17), (double)(-2))
    },
      new List<Vec3> {
        new Vec3((double)(-8.659560562354932e-17), (double)(-8.659560562354934e-17), (double)(2)),
        new Vec3((double)(-1.4142135623730951), (double)(-1.7319121124709868e-16), (double)(1.414213562373095)),
        new Vec3((double)(-1), (double)(-1.0000000000000002), (double)(1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(-2), (double)(-2.4492935982947064e-16), (double)(0)),
        new Vec3((double)(-1.4142135623730954), (double)(1.414213562373095), (double)(0)),
        new Vec3((double)(-1.0000000000000002), (double)(1), (double)(-1.414213562373095)),
        new Vec3((double)(-1.4142135623730951), (double)(-1.7319121124709868e-16), (double)(-1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(-1.4142135623730951), (double)(-1.7319121124709868e-16), (double)(1.414213562373095)),
        new Vec3((double)(-1.0000000000000002), (double)(1), (double)(1.414213562373095)),
        new Vec3((double)(-1.4142135623730954), (double)(1.414213562373095), (double)(0)),
        new Vec3((double)(-2), (double)(-2.4492935982947064e-16), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-1.4142135623730951), (double)(-1.7319121124709868e-16), (double)(-1.414213562373095)),
        new Vec3((double)(-1.0000000000000002), (double)(1), (double)(-1.414213562373095)),
        new Vec3((double)(-1.2246467991473532e-16), (double)(-1.4997597826618576e-32), (double)(-2))
    },
      new List<Vec3> {
        new Vec3((double)(-1.2246467991473532e-16), (double)(-1.4997597826618576e-32), (double)(2)),
        new Vec3((double)(-1.0000000000000002), (double)(1), (double)(1.414213562373095)),
        new Vec3((double)(-1.4142135623730951), (double)(-1.7319121124709868e-16), (double)(1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(-1.4142135623730954), (double)(1.414213562373095), (double)(0)),
        new Vec3((double)(-3.6739403974420594e-16), (double)(2), (double)(0)),
        new Vec3((double)(-2.59786816870648e-16), (double)(1.4142135623730951), (double)(-1.414213562373095)),
        new Vec3((double)(-1.0000000000000002), (double)(1), (double)(-1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(-1.0000000000000002), (double)(1), (double)(1.414213562373095)),
        new Vec3((double)(-2.59786816870648e-16), (double)(1.4142135623730951), (double)(1.414213562373095)),
        new Vec3((double)(-3.6739403974420594e-16), (double)(2), (double)(0)),
        new Vec3((double)(-1.4142135623730954), (double)(1.414213562373095), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-1.0000000000000002), (double)(1), (double)(-1.414213562373095)),
        new Vec3((double)(-2.59786816870648e-16), (double)(1.4142135623730951), (double)(-1.414213562373095)),
        new Vec3((double)(-8.659560562354935e-17), (double)(8.659560562354932e-17), (double)(-2))
    },
      new List<Vec3> {
        new Vec3((double)(-8.659560562354935e-17), (double)(8.659560562354932e-17), (double)(2)),
        new Vec3((double)(-2.59786816870648e-16), (double)(1.4142135623730951), (double)(1.414213562373095)),
        new Vec3((double)(-1.0000000000000002), (double)(1), (double)(1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(-3.6739403974420594e-16), (double)(2), (double)(0)),
        new Vec3((double)(1.4142135623730947), (double)(1.4142135623730954), (double)(0)),
        new Vec3((double)(0.9999999999999998), (double)(1.0000000000000002), (double)(-1.414213562373095)),
        new Vec3((double)(-2.59786816870648e-16), (double)(1.4142135623730951), (double)(-1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(-2.59786816870648e-16), (double)(1.4142135623730951), (double)(1.414213562373095)),
        new Vec3((double)(0.9999999999999998), (double)(1.0000000000000002), (double)(1.414213562373095)),
        new Vec3((double)(1.4142135623730947), (double)(1.4142135623730954), (double)(0)),
        new Vec3((double)(-3.6739403974420594e-16), (double)(2), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(-2.59786816870648e-16), (double)(1.4142135623730951), (double)(-1.414213562373095)),
        new Vec3((double)(0.9999999999999998), (double)(1.0000000000000002), (double)(-1.414213562373095)),
        new Vec3((double)(-2.2496396739927864e-32), (double)(1.2246467991473532e-16), (double)(-2))
    },
      new List<Vec3> {
        new Vec3((double)(-2.2496396739927864e-32), (double)(1.2246467991473532e-16), (double)(2)),
        new Vec3((double)(0.9999999999999998), (double)(1.0000000000000002), (double)(1.414213562373095)),
        new Vec3((double)(-2.59786816870648e-16), (double)(1.4142135623730951), (double)(1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(1.4142135623730947), (double)(1.4142135623730954), (double)(0)),
        new Vec3((double)(2), (double)(4.898587196589413e-16), (double)(0)),
        new Vec3((double)(1.4142135623730951), (double)(3.4638242249419736e-16), (double)(-1.414213562373095)),
        new Vec3((double)(0.9999999999999998), (double)(1.0000000000000002), (double)(-1.414213562373095))
    },
      new List<Vec3> {
        new Vec3((double)(0.9999999999999998), (double)(1.0000000000000002), (double)(1.414213562373095)),
        new Vec3((double)(1.4142135623730951), (double)(3.4638242249419736e-16), (double)(1.414213562373095)),
        new Vec3((double)(2), (double)(4.898587196589413e-16), (double)(0)),
        new Vec3((double)(1.4142135623730947), (double)(1.4142135623730954), (double)(0))
    },
      new List<Vec3> {
        new Vec3((double)(0.9999999999999998), (double)(1.0000000000000002), (double)(-1.414213562373095)),
        new Vec3((double)(1.4142135623730951), (double)(3.4638242249419736e-16), (double)(-1.414213562373095)),
        new Vec3((double)(8.65956056235493e-17), (double)(8.659560562354935e-17), (double)(-2))
    },
      new List<Vec3> {
        new Vec3((double)(8.65956056235493e-17), (double)(8.659560562354935e-17), (double)(2)),
        new Vec3((double)(1.4142135623730951), (double)(3.4638242249419736e-16), (double)(1.414213562373095)),
        new Vec3((double)(0.9999999999999998), (double)(1.0000000000000002), (double)(1.414213562373095))
    }
  };
}
