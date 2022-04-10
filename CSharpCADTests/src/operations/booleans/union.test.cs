namespace CSharpCADTests;

[TestFixture]
public class UnionTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestUnionGeom2()
    {
        // test('union: union of a path produces expected changes to points', (t) => {
        //   var geometry = path.fromPoints({}, [[0, 1, 0], [1, 0, 0]])
        //
        //   geometry = union({normal: [1, 0, 0]}, geometry)
        //   var obs = path.toPoints(geometry)
        //   var exp = []
        //
        //   t.deepEqual(obs, exp)
        // })

        var geometry1 = (Geom2)Circle(radius: 2, segments: 8);
        Assert.DoesNotThrow(() => geometry1.Validate());

        // union of one object
        Geom2 result1 = (Geom2)Union(geometry1);
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
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // union of two non-overlapping objects
        Geom2 obj = Rectangle(new Opts { { "size", (4, 4) } });
        Geom2 geometry2 = (Geom2)Center(obj, relativeTo: new Vec3(10, 10, 0));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = (Geom2)Union(geometry1, geometry2);
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
        new Vec2(8, 12),
        new Vec2(8, 8),
        new Vec2(12, 8),
        new Vec2(12, 12),
        new Vec2(1.4142000000000001, -1.4142000000000001)
      };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // union of two partially overlapping objects
        var geometry3 = Rectangle(new Opts { { "size", (18, 18) } });
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = (Geom2)Union(geometry2, geometry3);
        Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        exp = new Vec2[] {
          new Vec2(11.999973333333333, 11.999973333333333),
          new Vec2(7.999933333333333, 11.999973333333333),
          new Vec2(9.000053333333334, 7.999933333333333),
          new Vec2(-9.000053333333334, 9.000053333333334),
          new Vec2(-9.000053333333334, -9.000053333333334),
          new Vec2(9.000053333333334, -9.000053333333334),
          new Vec2(7.999933333333333, 9.000053333333334),
          new Vec2(11.999973333333333, 7.999933333333333)
        };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // union of two completely overlapping objects
        var result4 = (Geom2)Union(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        exp = new Vec2[] {
            new Vec2(-9.000046666666666, -9.000046666666666),
            new Vec2(9.000046666666666, -9.000046666666666),
            new Vec2(9.000046666666666, 9.000046666666666),
            new Vec2(-9.000046666666666, 9.000046666666666)
          };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // union of unions of non-overlapping objects (BSP gap from #907)
        var circ = Circle(radius: 1, segments: 32);
        Assert.DoesNotThrow(() => circ.Validate());
        var result5 = (Geom2)Union(
          Union(
            Translate(new Vec3(17, 21, 0), circ),
            Translate(new Vec3(7, 0, 0), circ)
          ),
          Union(
            Translate(new Vec3(3, 21, 0), circ),
            Translate(new Vec3(17, 21, 0), circ)
          )
        );
        obs = result5.ToPoints();
        // LATER JSCAD - The two assertions being worked on.
        // Assert.Throws(typeof(ValidationException), () => result5.Validate());
        // Assert.AreEqual(obs.Length, 112);
    }

    [Test]
    public void TestUnionGeom3()
    {
        var geometry1 = Sphere(new Opts { { "radius", 2 }, { "segments", 8 } });
        Assert.DoesNotThrow(() => geometry1.Validate());

        // union of one object
        var result1 = (Geom3)Union(geometry1);
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
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // union of two non-overlapping objects
        var obj2 = Cuboid(new Opts { { "size", (4, 4, 4) } });
        var geometry2 = (Geom3)Center(obj2, relativeTo: new Vec3(10, 10, 10));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = (Geom3)Union(geometry1, geometry2);
        obs = result2.ToPoints();
        Assert.DoesNotThrow(() => result2.Validate());
        Assert.AreEqual(obs.Count, 38);

        // union of two partially overlapping objects
        var geometry3 = Cuboid(new Opts { { "size", (18, 18, 18) } });
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = (Geom3)Union(geometry2, geometry3);
        Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        exp = new List<List<Vec3>> {
        new List<Vec3>{new Vec3(12, 8, 8), new Vec3(12, 12, 8), new Vec3(12, 12, 12), new Vec3(12, 8, 12)},
        new List<Vec3>{new Vec3(8, 12, 8), new Vec3(8, 12, 12), new Vec3(12, 12, 12), new Vec3(12, 12, 8)},
        new List<Vec3>{new Vec3(8, 8, 12), new Vec3(12, 8, 12), new Vec3(12, 12, 12), new Vec3(8, 12, 12)},
        new List<Vec3>{new Vec3(8, 12, 12), new Vec3(8, 12, 9), new Vec3(8, 8, 9), new Vec3(8, 8, 12)},
        new List<Vec3>{new Vec3(8, 12, 9), new Vec3(8, 12, 8), new Vec3(8, 9, 8), new Vec3(8, 9, 9)},
        new List<Vec3>{new Vec3(8, 8, 12), new Vec3(8, 8, 9), new Vec3(12, 8, 9), new Vec3(12, 8, 12)},
        new List<Vec3>{new Vec3(9, 8, 9), new Vec3(9, 8, 8), new Vec3(12, 8, 8), new Vec3(12, 8, 9)},
        new List<Vec3>{new Vec3(12, 12, 8), new Vec3(12, 9, 8), new Vec3(8, 9, 8), new Vec3(8, 12, 8)},
        new List<Vec3>{new Vec3(12, 9, 8), new Vec3(12, 8, 8), new Vec3(9, 8, 8), new Vec3(9, 9, 8)},
        new List<Vec3>{new Vec3(-9, -9, -9), new Vec3(-9, -9, 9), new Vec3(-9, 9, 9), new Vec3(-9, 9, -9)},
        new List<Vec3>{new Vec3(-9, -9, -9), new Vec3(9, -9, -9), new Vec3(9, -9, 9), new Vec3(-9, -9, 9)},
        new List<Vec3>{new Vec3(-9, -9, -9), new Vec3(-9, 9, -9), new Vec3(9, 9, -9), new Vec3(9, -9, -9)},
        new List<Vec3>{new Vec3(9, -9, 9), new Vec3(9, -9, 8), new Vec3(9, 8, 8), new Vec3(9, 8, 9)},
        new List<Vec3>{new Vec3(9, -9, 8), new Vec3(9, -9, -9), new Vec3(9, 9, -9), new Vec3(9, 9, 8)},
        new List<Vec3>{new Vec3(8, 9, 9), new Vec3(8, 9, 8), new Vec3(-9, 9, 8), new Vec3(-9, 9, 9)},
        new List<Vec3>{new Vec3(9, 9, 8), new Vec3(9, 9, -9), new Vec3(-9, 9, -9), new Vec3(-9, 9, 8)},
        new List<Vec3>{new Vec3(-9, 9, 9), new Vec3(-9, 8, 9), new Vec3(8, 8, 9), new Vec3(8, 9, 9)},
        new List<Vec3>{new Vec3(-9, 8, 9), new Vec3(-9, -9, 9), new Vec3(9, -9, 9), new Vec3(9, 8, 9)}
      };
        Assert.AreEqual(obs.Count, 18);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // union of two completely overlapping objects
        var result4 = (Geom3)Union(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        exp = new List<List<Vec3>> {
        new List<Vec3>{new Vec3(-9, -9, -9), new Vec3(-9, -9, 9), new Vec3(-9, 9, 9), new Vec3(-9, 9, -9)},
        new List<Vec3>{new Vec3(9, -9, -9), new Vec3(9, 9, -9), new Vec3(9, 9, 9), new Vec3(9, -9, 9)},
        new List<Vec3>{new Vec3(-9, -9, -9), new Vec3(9, -9, -9), new Vec3(9, -9, 9), new Vec3(-9, -9, 9)},
        new List<Vec3>{new Vec3(-9, 9, -9), new Vec3(-9, 9, 9), new Vec3(9, 9, 9), new Vec3(9, 9, -9)},
        new List<Vec3>{new Vec3(-9, -9, -9), new Vec3(-9, 9, -9), new Vec3(9, 9, -9), new Vec3(9, -9, -9)},
        new List<Vec3>{new Vec3(-9, -9, 9), new Vec3(9, -9, 9), new Vec3(9, 9, 9), new Vec3(-9, 9, 9)}
      };
        Assert.AreEqual(obs.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));
    }

    [Test]
    public void TestUnionGeom3WithRounding()
    {
        //'union of geom3 with rounding issues #137'
        var obj1 = Cuboid(new Opts { { "size", (44, 26, 5) } });
        var geometry1 = (Geom3)Center(obj1, relativeTo: new Vec3(0, 0, -1));
        Assert.DoesNotThrow(() => geometry1.Validate());
        var obj2 = Cuboid(new Opts { { "size", (44, 26, 1.8) } }); // introduce percision error
        var geometry2 = (Geom3)Center(obj2, relativeTo: new Vec3(0, 0, -4.400001)); // introduce percision error
        Assert.DoesNotThrow(() => geometry2.Validate());

        var obs = (Geom3)Union(geometry1, geometry2);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Count, 6); // number of polygons in union
    }

    [Test]
    public void TestUnionGeom2WithClosingIssues()
    {
        var c = new Geom2(new Geom2.Side[] {
        new Geom2.Side(new Vec2(-45.82118740347841168159, -16.85726810555620147625), new Vec2(-49.30331715865012398581, -14.68093629710870118288)),
        new Geom2.Side(new Vec2(-49.10586702080816223770, -15.27604177352110781385), new Vec2(-48.16645938811709015681, -15.86317173589183227023)),
        new Geom2.Side(new Vec2(-49.60419521731581937729, -14.89550781504266296906), new Vec2(-49.42407001323204696064, -15.67605088949303393520)),
        new Geom2.Side(new Vec2(-49.05727291218684626983, -15.48661638542171203881), new Vec2(-49.10586702080816223770, -15.27604177352110781385)),
        new Geom2.Side(new Vec2(-49.30706235399220815907, -15.81529674600091794900), new Vec2(-46.00505780290426827150, -17.21108547999804727624)),
        new Geom2.Side(new Vec2(-46.00505780290426827150, -17.21108547999804727624), new Vec2(-45.85939703723252591772, -17.21502856394236857795)),
        new Geom2.Side(new Vec2(-45.85939703723252591772, -17.21502856394236857795), new Vec2(-45.74972032664388166268, -17.11909303495791334626)),
        new Geom2.Side(new Vec2(-45.74972032664388166268, -17.11909303495791334626), new Vec2(-45.73424573227583067592, -16.97420292661295349035)),
        new Geom2.Side(new Vec2(-45.73424573227583067592, -16.97420292661295349035), new Vec2(-45.82118740347841168159, -16.85726810555620147625)),
        new Geom2.Side(new Vec2(-49.30331715865012398581, -14.68093629710870118288), new Vec2(-49.45428884427643367871, -14.65565769658912387285)),
        new Geom2.Side(new Vec2(-49.45428884427643367871, -14.65565769658912387285), new Vec2(-49.57891661679624917269, -14.74453612941635327616)),
        new Geom2.Side(new Vec2(-49.57891661679624917269, -14.74453612941635327616), new Vec2(-49.60419521731581937729, -14.89550781504266296906)),
        new Geom2.Side(new Vec2(-49.42407001323204696064, -15.67605088949303393520), new Vec2(-49.30706235399220815907, -15.81529674600091794900)),
        new Geom2.Side(new Vec2(-48.16645938811709015681, -15.86317173589183227023), new Vec2(-49.05727291218684626983, -15.48661638542171203881))
      });
        var d = new Geom2(new Geom2.Side[] {
        new Geom2.Side(new Vec2(-49.03431352173912216585, -15.58610714407888764299), new Vec2(-49.21443872582289458251, -14.80556406962851667686)),
        new Geom2.Side(new Vec2(-68.31614651314507113966, -3.10790373951434872879), new Vec2(-49.34036769611472550423, -15.79733157434056778357)),
        new Geom2.Side(new Vec2(-49.58572929483430868913, -14.97552686612213790340), new Vec2(-49.53755741140093959984, -15.18427183431472826669)),
        new Geom2.Side(new Vec2(-49.53755741140093959984, -15.18427183431472826669), new Vec2(-54.61235529924312714911, -11.79066769321313756791)),
        new Geom2.Side(new Vec2(-49.30227466841120076424, -14.68159232649114187552), new Vec2(-68.09792828135776687759, -2.77270756611528668145)),
        new Geom2.Side(new Vec2(-49.21443872582289458251, -14.80556406962851667686), new Vec2(-49.30227466841120076424, -14.68159232649114187552)),
        new Geom2.Side(new Vec2(-49.34036769611472550423, -15.79733157434056778357), new Vec2(-49.18823337756091262918, -15.82684012194931710837)),
        new Geom2.Side(new Vec2(-49.18823337756091262918, -15.82684012194931710837), new Vec2(-49.06069007212390431505, -15.73881563386780157998)),
        new Geom2.Side(new Vec2(-49.06069007212390431505, -15.73881563386780157998), new Vec2(-49.03431352173912216585, -15.58610714407888764299)),
        new Geom2.Side(new Vec2(-68.09792828135776687759, -2.77270756611528668145), new Vec2(-68.24753735887460948106, -2.74623350179570024920)),
        new Geom2.Side(new Vec2(-68.24753735887460948106, -2.74623350179570024920), new Vec2(-68.37258141465594007968, -2.83253376987636329432)),
        new Geom2.Side(new Vec2(-68.37258141465594007968, -2.83253376987636329432), new Vec2(-68.40089829889257089235, -2.98180502037078554167)),
        new Geom2.Side(new Vec2(-68.40089829889257089235, -2.98180502037078554167), new Vec2(-68.31614651314507113966, -3.10790373951434872879)),
        new Geom2.Side(new Vec2(-54.61235529924312714911, -11.79066769321313756791), new Vec2(-49.58572929483430868913, -14.97552686612213790340))
      });
        // geom2.toOutlines(c)
        // geom2.toOutlines(d)

        var obs = (Geom2)Union(c, d);
        Assert.DoesNotThrow(() => obs.Validate());
        // var outlines = geom2.toOutlines(obs)
        var pts = obs.ToPoints();
        var exp = new Vec2[] {
        new Vec2(-49.10585516965137, -15.276000175919414),
        new Vec2(-49.0573272145917, -15.486679335654257),
        new Vec2(-49.307011370463215, -15.815286644243773),
        new Vec2(-46.00502320253235, -17.211117609669667),
        new Vec2(-45.85943933735334, -17.215031154432545),
        new Vec2(-45.74972963250071, -17.119149307742074),
        new Vec2(-45.734205904941305, -16.974217700023555),
        new Vec2(-48.166473975068946, -15.86316234184296),
        new Vec2(-49.318621553259746, -15.801589237573706),
        new Vec2(-49.585786209072104, -14.975570389622606),
        new Vec2(-68.31614189569036, -3.1078763476921982),
        new Vec2(-49.53751915699663, -15.184292776976012),
        new Vec2(-68.09789654941396, -2.7727464644978874),
        new Vec2(-68.24752441084793, -2.7462648116024244),
        new Vec2(-68.37262739176788, -2.8324932478777995),
        new Vec2(-68.40093536555268, -2.98186020632758),
        new Vec2(-54.61234310251047, -11.79072766159384),
        new Vec2(-49.30335872868453, -14.680880468978017),
        new Vec2(-49.34040695243976, -15.797284338334542),
        new Vec2(-45.82121705016925, -16.857333163105647)
      };
        Assert.AreEqual(pts.Length, 20); // number of sides in union
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
    }
}