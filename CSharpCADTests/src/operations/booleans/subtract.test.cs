namespace CSharpCADTests;

[TestFixture]
public class SubtractTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestTransformGeom2()
    {
        var geometry1 = Circle(radius: 2, segments: 8);
        Assert.DoesNotThrow(() => geometry1.Validate());

        // subtract of one object
        var result1 = (Geom2)Subtract(geometry1);
        Assert.DoesNotThrow(() => result1.Validate());
        var obs = result1.ToPoints();
        var exp = new Vec2[] {
          new Vec2(2, 0),
          new Vec2(1.4142000000000001, 1.4142000000000001),
          new Vec2(0, 2),
          new Vec2(-1.4142000000000001, 1.4142000000000001),
          new Vec2(-2, 0),
          new Vec2(-1.4142000000000001, -1.4142000000000001),
          new Vec2(0, -2),
          new Vec2(1.4142000000000001, -1.4142000000000001)
        };
        Assert.AreEqual(obs.Length, 8);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // subtract of two non-overlapping objects
        var geometry2 = (Geom2)Center(Rectangle(size: (4, 4), center: (0, 0)), relativeTo: new Vec3(10, 10, 0));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = (Geom2)Subtract(geometry1, geometry2);
        Assert.DoesNotThrow(() => result2.Validate());
        obs = result2.ToPoints();
        exp = new Vec2[] {
          new Vec2(2, 0),
          new Vec2(1.4142000000000001, 1.4142000000000001),
          new Vec2(0, 2),
          new Vec2(-1.4142000000000001, 1.4142000000000001),
          new Vec2(-2, 0),
          new Vec2(-1.4142000000000001, -1.4142000000000001),
          new Vec2(0, -2),
          new Vec2(1.4142000000000001, -1.4142000000000001)
        };
        Assert.AreEqual(obs.Length, 8);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // subtract of two partially overlapping objects
        var geometry3 = Rectangle(size: (18, 18), center: (0,0));
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = (Geom2)Subtract(geometry2, geometry3);
        Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        exp = new Vec2[] {
          new Vec2(12, 12), new Vec2(9, 9), new Vec2(8, 9), new Vec2(8, 12), new Vec2(9, 8), new Vec2(12, 8)
        };
        Assert.AreEqual(obs.Length, 6);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // subtract of two completely overlapping objects
        var result4 = (Geom2)Subtract(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        exp = new Vec2[0];
        Assert.AreEqual(obs.Length, 0);
        Assert.AreEqual(obs, exp);
    }

    [Test]
    public void TestTransformGeom3()
    {
        var geometry1 = Sphere(radius: 2, segments: 8);
        Assert.DoesNotThrow(() => geometry1.Validate());

        // subtract of one object
        var result1 = (Geom3)Subtract(geometry1);
        Assert.DoesNotThrow(() => result1.Validate());
        var obs = result1.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(2, 0, 0), new Vec3(1.4142135623730951, -1.414213562373095, 0),
            new Vec3(1.0000000000000002, -1, -1.414213562373095), new Vec3(1.4142135623730951, 0, -1.414213562373095)},
          new List<Vec3>{new Vec3(1.4142135623730951, 0, 1.414213562373095), new Vec3(1.0000000000000002, -1, 1.414213562373095),
            new Vec3(1.4142135623730951, -1.414213562373095, 0), new Vec3(2, 0, 0)},
          new List<Vec3>{new Vec3(1.4142135623730951, 0, -1.414213562373095), new Vec3(1.0000000000000002, -1, -1.414213562373095), new Vec3(1.2246467991473532e-16, 0, -2)},
          new List<Vec3>{new Vec3(1.2246467991473532e-16, 0, 2), new Vec3(1.0000000000000002, -1, 1.414213562373095), new Vec3(1.4142135623730951, 0, 1.414213562373095)},
          new List<Vec3>{new Vec3(1.4142135623730951, -1.414213562373095, 0), new Vec3(1.2246467991473532e-16, -2, 0),
            new Vec3(8.659560562354934e-17, -1.4142135623730951, -1.414213562373095), new Vec3(1.0000000000000002, -1, -1.414213562373095)},
          new List<Vec3>{new Vec3(1.0000000000000002, -1, 1.414213562373095), new Vec3(8.659560562354934e-17, -1.4142135623730951, 1.414213562373095),
            new Vec3(1.2246467991473532e-16, -2, 0), new Vec3(1.4142135623730951, -1.414213562373095, 0)},
          new List<Vec3>{new Vec3(1.0000000000000002, -1, -1.414213562373095), new Vec3(8.659560562354934e-17, -1.4142135623730951, -1.414213562373095), new Vec3(8.659560562354934e-17, -8.659560562354932e-17, -2)},
          new List<Vec3>{new Vec3(8.659560562354934e-17, -8.659560562354932e-17, 2), new Vec3(8.659560562354934e-17, -1.4142135623730951, 1.414213562373095), new Vec3(1.0000000000000002, -1, 1.414213562373095)},
          new List<Vec3>{new Vec3(1.2246467991473532e-16, -2, 0), new Vec3(-1.414213562373095, -1.4142135623730951, 0),
            new Vec3(-1, -1.0000000000000002, -1.414213562373095), new Vec3(8.659560562354934e-17, -1.4142135623730951, -1.414213562373095)},
          new List<Vec3>{new Vec3(8.659560562354934e-17, -1.4142135623730951, 1.414213562373095), new Vec3(-1, -1.0000000000000002, 1.414213562373095),
            new Vec3(-1.414213562373095, -1.4142135623730951, 0), new Vec3(1.2246467991473532e-16, -2, 0)},
          new List<Vec3>{new Vec3(8.659560562354934e-17, -1.4142135623730951, -1.414213562373095), new Vec3(-1, -1.0000000000000002, -1.414213562373095), new Vec3(7.498798913309288e-33, -1.2246467991473532e-16, -2)},
          new List<Vec3>{new Vec3(7.498798913309288e-33, -1.2246467991473532e-16, 2), new Vec3(-1, -1.0000000000000002, 1.414213562373095), new Vec3(8.659560562354934e-17, -1.4142135623730951, 1.414213562373095)},
          new List<Vec3>{new Vec3(-1.414213562373095, -1.4142135623730951, 0), new Vec3(-2, -2.4492935982947064e-16, 0),
            new Vec3(-1.4142135623730951, -1.7319121124709868e-16, -1.414213562373095), new Vec3(-1, -1.0000000000000002, -1.414213562373095)},
          new List<Vec3>{new Vec3(-1, -1.0000000000000002, 1.414213562373095), new Vec3(-1.4142135623730951, -1.7319121124709868e-16, 1.414213562373095),
            new Vec3(-2, -2.4492935982947064e-16, 0), new Vec3(-1.414213562373095, -1.4142135623730951, 0)},
          new List<Vec3>{new Vec3(-1, -1.0000000000000002, -1.414213562373095), new Vec3(-1.4142135623730951, -1.7319121124709868e-16, -1.414213562373095), new Vec3(-8.659560562354932e-17, -8.659560562354934e-17, -2)},
          new List<Vec3>{new Vec3(-8.659560562354932e-17, -8.659560562354934e-17, 2), new Vec3(-1.4142135623730951, -1.7319121124709868e-16, 1.414213562373095), new Vec3(-1, -1.0000000000000002, 1.414213562373095)},
          new List<Vec3>{new Vec3(-2, -2.4492935982947064e-16, 0), new Vec3(-1.4142135623730954, 1.414213562373095, 0),
            new Vec3(-1.0000000000000002, 1, -1.414213562373095), new Vec3(-1.4142135623730951, -1.7319121124709868e-16, -1.414213562373095)},
          new List<Vec3>{new Vec3(-1.4142135623730951, -1.7319121124709868e-16, 1.414213562373095), new Vec3(-1.0000000000000002, 1, 1.414213562373095),
            new Vec3(-1.4142135623730954, 1.414213562373095, 0), new Vec3(-2, -2.4492935982947064e-16, 0)},
          new List<Vec3>{new Vec3(-1.4142135623730951, -1.7319121124709868e-16, -1.414213562373095), new Vec3(-1.0000000000000002, 1, -1.414213562373095), new Vec3(-1.2246467991473532e-16, -1.4997597826618576e-32, -2)},
          new List<Vec3>{new Vec3(-1.2246467991473532e-16, -1.4997597826618576e-32, 2), new Vec3(-1.0000000000000002, 1, 1.414213562373095), new Vec3(-1.4142135623730951, -1.7319121124709868e-16, 1.414213562373095)},
          new List<Vec3>{new Vec3(-1.4142135623730954, 1.414213562373095, 0), new Vec3(-3.6739403974420594e-16, 2, 0),
            new Vec3(-2.59786816870648e-16, 1.4142135623730951, -1.414213562373095), new Vec3(-1.0000000000000002, 1, -1.414213562373095)},
          new List<Vec3>{new Vec3(-1.0000000000000002, 1, 1.414213562373095), new Vec3(-2.59786816870648e-16, 1.4142135623730951, 1.414213562373095),
            new Vec3(-3.6739403974420594e-16, 2, 0), new Vec3(-1.4142135623730954, 1.414213562373095, 0)},
          new List<Vec3>{new Vec3(-1.0000000000000002, 1, -1.414213562373095), new Vec3(-2.59786816870648e-16, 1.4142135623730951, -1.414213562373095), new Vec3(-8.659560562354935e-17, 8.659560562354932e-17, -2)},
          new List<Vec3>{new Vec3(-8.659560562354935e-17, 8.659560562354932e-17, 2), new Vec3(-2.59786816870648e-16, 1.4142135623730951, 1.414213562373095), new Vec3(-1.0000000000000002, 1, 1.414213562373095)},
          new List<Vec3>{new Vec3(-3.6739403974420594e-16, 2, 0), new Vec3(1.4142135623730947, 1.4142135623730954, 0),
            new Vec3(0.9999999999999998, 1.0000000000000002, -1.414213562373095), new Vec3(-2.59786816870648e-16, 1.4142135623730951, -1.414213562373095)},
          new List<Vec3>{new Vec3(-2.59786816870648e-16, 1.4142135623730951, 1.414213562373095), new Vec3(0.9999999999999998, 1.0000000000000002, 1.414213562373095),
            new Vec3(1.4142135623730947, 1.4142135623730954, 0), new Vec3(-3.6739403974420594e-16, 2, 0)},
          new List<Vec3>{new Vec3(-2.59786816870648e-16, 1.4142135623730951, -1.414213562373095), new Vec3(0.9999999999999998, 1.0000000000000002, -1.414213562373095), new Vec3(-2.2496396739927864e-32, 1.2246467991473532e-16, -2)},
          new List<Vec3>{new Vec3(-2.2496396739927864e-32, 1.2246467991473532e-16, 2), new Vec3(0.9999999999999998, 1.0000000000000002, 1.414213562373095), new Vec3(-2.59786816870648e-16, 1.4142135623730951, 1.414213562373095)},
          new List<Vec3>{new Vec3(1.4142135623730947, 1.4142135623730954, 0), new Vec3(2, 4.898587196589413e-16, 0),
            new Vec3(1.4142135623730951, 3.4638242249419736e-16, -1.414213562373095), new Vec3(0.9999999999999998, 1.0000000000000002, -1.414213562373095)},
          new List<Vec3>{new Vec3(0.9999999999999998, 1.0000000000000002, 1.414213562373095), new Vec3(1.4142135623730951, 3.4638242249419736e-16, 1.414213562373095),
            new Vec3(2, 4.898587196589413e-16, 0), new Vec3(1.4142135623730947, 1.4142135623730954, 0)},
          new List<Vec3>{new Vec3(0.9999999999999998, 1.0000000000000002, -1.414213562373095), new Vec3(1.4142135623730951, 3.4638242249419736e-16, -1.414213562373095), new Vec3(8.65956056235493e-17, 8.659560562354935e-17, -2)},
          new List<Vec3>{new Vec3(8.65956056235493e-17, 8.659560562354935e-17, 2), new Vec3(1.4142135623730951, 3.4638242249419736e-16, 1.414213562373095), new Vec3(0.9999999999999998, 1.0000000000000002, 1.414213562373095)}
        };
        Assert.AreEqual(obs.Count, 32);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // subtract of two non-overlapping objects
        var geometry2 = (Geom3)Center(Cuboid(size: (4, 4, 4), center: (0, 0, 0)), relativeTo: (10, 10, 10));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = (Geom3)Subtract(geometry1, geometry2);
        Assert.DoesNotThrow(() => result2.Validate());
        obs = result2.ToPoints();
        Assert.AreEqual(obs.Count, 32);

        // subtract of two partially overlapping objects
        var geometry3 = Cuboid(size: (18, 18, 18), center: (0, 0, 0));
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = (Geom3)Subtract(geometry2, geometry3);
        Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(12, 8, 8), new Vec3(12, 12, 8), new Vec3(12, 12, 12), new Vec3(12, 8, 12)},
          new List<Vec3>{new Vec3(8, 12, 8), new Vec3(8, 12, 12), new Vec3(12, 12, 12), new Vec3(12, 12, 8)},
          new List<Vec3>{new Vec3(8, 8, 12), new Vec3(12, 8, 12), new Vec3(12, 12, 12), new Vec3(8, 12, 12)},
          new List<Vec3>{new Vec3(9, 8, 8), new Vec3(9, 8, 9), new Vec3(9, 9, 9), new Vec3(9, 9, 8)},
          new List<Vec3>{new Vec3(8, 9, 8), new Vec3(9, 9, 8), new Vec3(9, 9, 9), new Vec3(8, 9, 9)},
          new List<Vec3>{new Vec3(8, 8, 9), new Vec3(8, 9, 9), new Vec3(9, 9, 9), new Vec3(9, 8, 9)},
          new List<Vec3>{new Vec3(8, 12, 12), new Vec3(8, 12, 9), new Vec3(8, 8, 9), new Vec3(8, 8, 12)},
          new List<Vec3>{new Vec3(8, 12, 9), new Vec3(8, 12, 8), new Vec3(8, 9, 8), new Vec3(8, 9, 9)},
          new List<Vec3>{new Vec3(8, 8, 12), new Vec3(8, 8, 9), new Vec3(12, 8, 9), new Vec3(12, 8, 12)},
          new List<Vec3>{new Vec3(9, 8, 9), new Vec3(9, 8, 8), new Vec3(12, 8, 8), new Vec3(12, 8, 9)},
          new List<Vec3>{new Vec3(12, 12, 8), new Vec3(12, 9, 8), new Vec3(8, 9, 8), new Vec3(8, 12, 8)},
          new List<Vec3>{new Vec3(12, 9, 8), new Vec3(12, 8, 8), new Vec3(9, 8, 8), new Vec3(9, 9, 8)}
        };
        Assert.AreEqual(obs.Count, 12);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // subtract of two completely overlapping objects
        var result4 = (Geom3)Subtract(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        Assert.AreEqual(obs.Count, 0);
    }
}
