namespace CSharpCADTests;

using static CSharpCAD.Modifiers;

[TestFixture]
public class ModifiersTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestGeneralize()
    {
        var geometry1 = Cuboid(size: (Math.PI, Math.PI / 2, Math.PI * 2), center: (0, 0, 0));

        // apply no modifications
        var result = generalize(geometry1);
        Assert.DoesNotThrow(() => result.Validate());
        var pts = result.ToPoints();
        if(WriteTests) TestData.Make("GeneralizeExp1", pts);
        var exp = UnitTestData.GeneralizeExp1;
        Assert.IsTrue(Helpers.CompareListOfLists(pts, exp));
        Assert.AreEqual(pts, exp);

#if LATER
        // apply snap only
        result = generalize(geometry1, snap: true);
        Assert.DoesNotThrow(() => result.Validate());
        pts = result.ToPoints();
        if(WriteTests) TestData.Make("GeneralizeExp2", pts);
        exp = UnitTestData.GeneralizeExp2;
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
#endif

        // apply triangulate only
        result = generalize(geometry1, triangulate: true);
        Assert.DoesNotThrow(() => result.Validate());
        pts = result.ToPoints();
        if(WriteTests) TestData.Make("GeneralizeExp3", pts);
        exp = UnitTestData.GeneralizeExp3;
        Assert.IsTrue(Helpers.CompareListOfLists(pts, exp));

        var geometry2 = result; // save the triangles for another test

        // apply simplify only (triangles => quads)
        result = generalize(geometry2, simplify: true);
        Assert.DoesNotThrow(() => result.Validate());
        pts = result.ToPoints();
        if(WriteTests) TestData.Make("GeneralizeExp4", pts);
        exp = UnitTestData.GeneralizeExp4;
        Assert.IsTrue(Helpers.CompareListOfLists(pts, exp));

        // apply repairs only (triangles)
        result = generalize(geometry2, repair: true);
        Assert.DoesNotThrow(() => result.Validate());
        pts = result.ToPoints();
        if(WriteTests) TestData.Make("GeneralizeExp5", pts);
        exp = UnitTestData.GeneralizeExp5;
        Assert.IsTrue(Helpers.CompareListOfLists(pts, exp));
    }

    [Test]
    public void TestGeneralizeNewCaseFixedErrorCBSFound()
    {
        var geometry1 = Union(
          Cuboid(size: (8, 8, 8), center: (0, 0, 0)),
          Cuboid(center: (0, 0, 4))
        );
        geometry1.ApplyTransforms();
        var result = generalize(geometry1, repair: true);
        // LATER JSCAD Assert.DoesNotThrow(() => result.Validate());
        var pts = result.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-4, -4, -4), new Vec3(-4, -4, 4), new Vec3(-4, 4, 4), new Vec3(-4, 4, -4)},
          new List<Vec3>{new Vec3(-4, -4, -4), new Vec3(-4, 4, -4), new Vec3(4, 4, -4), new Vec3(4, -4, -4)},
          new List<Vec3>{new Vec3(-4, -4, -4), new Vec3(4, -4, -4), new Vec3(4, -4, 4), new Vec3(-4, -4, 4)},
          new List<Vec3>{new Vec3(-4, 4, -4), new Vec3(-4, 4, 4), new Vec3(4, 4, 4), new Vec3(4, 4, -4)},
          new List<Vec3>{new Vec3(4, -4, -4), new Vec3(4, 4, -4), new Vec3(4, 4, 4), new Vec3(4, -4, 4)},
          new List<Vec3>{new Vec3(-4, -1, 4), new Vec3(-4, -4, 4), new Vec3(4, -4, 4), new Vec3(4, -1, 4)},
          new List<Vec3>{new Vec3(-4, 4, 4), new Vec3(-4, 1, 4), new Vec3(4, 1, 4), new Vec3(4, 4, 4)},
          new List<Vec3>{new Vec3(1, 1, 4), new Vec3(1, -1, 4), new Vec3(4, -1, 4), new Vec3(4, 1, 4)},
          new List<Vec3>{new Vec3(1, 1, 4), new Vec3(1, 1, 5), new Vec3(1, -1, 5), new Vec3(1, -1, 4)},
          new List<Vec3>{new Vec3(-4, 1, 4), new Vec3(-4, -1, 4), new Vec3(-1, -1, 4), new Vec3(-1, 1, 4)},
          new List<Vec3>{new Vec3(-1, -1, 4), new Vec3(-1, -1, 5), new Vec3(-1, 1, 5), new Vec3(-1, 1, 4)},
          new List<Vec3>{new Vec3(-1, -1, 5), new Vec3(1, -1, 5), new Vec3(1, 1, 5), new Vec3(-1, 1, 5)},
          new List<Vec3>{new Vec3(1, -1, 4), new Vec3(1, -1, 5), new Vec3(-1, -1, 5), new Vec3(-1, -1, 4)},
          new List<Vec3>{new Vec3(-1, 1, 4), new Vec3(-1, 1, 5), new Vec3(1, 1, 5), new Vec3(1, 1, 4)}
        };
        //LATER JSCAD - waiting for a fix.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestGeneralizeWithTjunctions()
    {
        var geometry1 = new Geom3(new List<List<Vec3>> {
            new List<Vec3> {new Vec3(-1, -1, -1), new Vec3(-1, -1, 1), new Vec3(-1, 1, 1), new Vec3(-1, 1, -1)},
            new List<Vec3> {new Vec3(1, -1, -1), new Vec3(1, 1, -1), new Vec3(1, 1, 1), new Vec3(1, -1, 1)},
            new List<Vec3> {new Vec3(-1, -1, -1), new Vec3(1, -1, -1), new Vec3(1, -1, 1), new Vec3(-1, -1, 1)},
            new List<Vec3> {new Vec3(-1, 1, -1), new Vec3(-1, 1, 1), new Vec3(1, 1, 1), new Vec3(1, 1, -1)},
            new List<Vec3> {new Vec3(-1, -1, -1), new Vec3(-1, 1, -1), new Vec3(1, 1, -1), new Vec3(1, -1, -1)},
            // T junctions
            new List<Vec3> {new Vec3(-1, -1, 1), new Vec3(0, -1, 1), new Vec3(0, 0, 1)},
            new List<Vec3> {new Vec3(-1, 0, 1), new Vec3(-1, -1, 1), new Vec3(0, 0, 1)},

            new List<Vec3> {new Vec3(0, -1, 1), new Vec3(1, -1, 1), new Vec3(0, 0, 1)},
            new List<Vec3> {new Vec3(1, -1, 1), new Vec3(1, 0, 1), new Vec3(0, 0, 1)},

            new List<Vec3> {new Vec3(1, 0, 1), new Vec3(1, 1, 1), new Vec3(0, 0, 1)},
            new List<Vec3> {new Vec3(1, 1, 1), new Vec3(0, 1, 1), new Vec3(0, 0, 1)},

            new List<Vec3> {new Vec3(0, 1, 1), new Vec3(-1, 1, 1), new Vec3(0, 0, 1)},
            new List<Vec3> {new Vec3(-1, 1, 1), new Vec3(-1, 0, 1), new Vec3(0, 0, 1)}
          });

        var result = generalize(geometry1, snap: true, triangulate: true);
        Assert.DoesNotThrow(() => result.Validate());
        var pts = result.ToPoints();
        var exp = new List<List<Vec3>> {
          new List<Vec3> {new Vec3(-1, 0, 0.2), new Vec3(-1, -1, -1), new Vec3(-1, -1, 1)},
          new List<Vec3> {new Vec3(-1, 0, 0.2), new Vec3(-1, -1, 1), new Vec3(-1, 0, 1)},
          new List<Vec3> {new Vec3(-1, 0, 0.2), new Vec3(-1, 0, 1), new Vec3(-1, 1, 1)},
          new List<Vec3> {new Vec3(-1, 0, 0.2), new Vec3(-1, 1, 1), new Vec3(-1, 1, -1)},
          new List<Vec3> {new Vec3(-1, 0, 0.2), new Vec3(-1, 1, -1), new Vec3(-1, -1, -1)},
          new List<Vec3> {new Vec3(1, 0, 0.2), new Vec3(1, -1, -1), new Vec3(1, 1, -1)},
          new List<Vec3> {new Vec3(1, 0, 0.2), new Vec3(1, 1, -1), new Vec3(1, 1, 1)},
          new List<Vec3> {new Vec3(1, 0, 0.2), new Vec3(1, 1, 1), new Vec3(1, 0, 1)},
          new List<Vec3> {new Vec3(1, 0, 0.2), new Vec3(1, 0, 1), new Vec3(1, -1, 1)},
          new List<Vec3> {new Vec3(1, 0, 0.2), new Vec3(1, -1, 1), new Vec3(1, -1, -1)},
          new List<Vec3> {new Vec3(0, -1, 0.2), new Vec3(-1, -1, -1), new Vec3(1, -1, -1)},
          new List<Vec3> {new Vec3(0, -1, 0.2), new Vec3(1, -1, -1), new Vec3(1, -1, 1)},
          new List<Vec3> {new Vec3(0, -1, 0.2), new Vec3(1, -1, 1), new Vec3(0, -1, 1)},
          new List<Vec3> {new Vec3(0, -1, 0.2), new Vec3(0, -1, 1), new Vec3(-1, -1, 1)},
          new List<Vec3> {new Vec3(0, -1, 0.2), new Vec3(-1, -1, 1), new Vec3(-1, -1, -1)},
          new List<Vec3> {new Vec3(0, 1, 0.2), new Vec3(-1, 1, -1), new Vec3(-1, 1, 1)},
          new List<Vec3> {new Vec3(0, 1, 0.2), new Vec3(-1, 1, 1), new Vec3(0, 1, 1)},
          new List<Vec3> {new Vec3(0, 1, 0.2), new Vec3(0, 1, 1), new Vec3(1, 1, 1)},
          new List<Vec3> {new Vec3(0, 1, 0.2), new Vec3(1, 1, 1), new Vec3(1, 1, -1)},
          new List<Vec3> {new Vec3(0, 1, 0.2), new Vec3(1, 1, -1), new Vec3(-1, 1, -1)},
          new List<Vec3> {new Vec3(-1, -1, -1), new Vec3(-1, 1, -1), new Vec3(1, 1, -1)},
          new List<Vec3> {new Vec3(-1, -1, -1), new Vec3(1, 1, -1), new Vec3(1, -1, -1)},
          new List<Vec3> {new Vec3(-1, -1, 1), new Vec3(0, -1, 1), new Vec3(0, 0, 1)},
          new List<Vec3> {new Vec3(-1, 0, 1), new Vec3(-1, -1, 1), new Vec3(0, 0, 1)},
          new List<Vec3> {new Vec3(0, -1, 1), new Vec3(1, -1, 1), new Vec3(0, 0, 1)},
          new List<Vec3> {new Vec3(1, -1, 1), new Vec3(1, 0, 1), new Vec3(0, 0, 1)},
          new List<Vec3> {new Vec3(1, 0, 1), new Vec3(1, 1, 1), new Vec3(0, 0, 1)},
          new List<Vec3> {new Vec3(1, 1, 1), new Vec3(0, 1, 1), new Vec3(0, 0, 1)},
          new List<Vec3> {new Vec3(0, 1, 1), new Vec3(-1, 1, 1), new Vec3(0, 0, 1)},
          new List<Vec3> {new Vec3(-1, 1, 1), new Vec3(-1, 0, 1), new Vec3(0, 0, 1)}
        };
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestinsertTjunctionsDirectly()
    {
        var geometry1 = new Geom3();
        var geometry2 = Cuboid(size: (2, 2, 2), center: (0, 0, 0));
        var geometry3 = new Geom3(
          new List<List<Vec3>> {
      new List<Vec3> { new Vec3(-1, -1, -1), new Vec3(-1, -1, 1), new Vec3(-1, 1, 1), new Vec3(-1, 1, -1)},
      new List<Vec3> { new Vec3(1, -1, -1), new Vec3(1, 1, -1), new Vec3(1, 1, 1), new Vec3(1, -1, 1)},
      new List<Vec3> { new Vec3(-1, -1, -1), new Vec3(1, -1, -1), new Vec3(1, -1, 1), new Vec3(-1, -1, 1)},
      new List<Vec3> { new Vec3(-1, 1, -1), new Vec3(-1, 1, 1), new Vec3(1, 1, 1), new Vec3(1, 1, -1)},
      new List<Vec3> { new Vec3(-1, -1, -1), new Vec3(-1, 1, -1), new Vec3(1, 1, -1), new Vec3(1, -1, -1)},
      // T junction
      new List<Vec3> { new Vec3(-1, -1, 1), new Vec3(1, -1, 1), new Vec3(1, 1, 1)},
      new List<Vec3> { new Vec3(1, 1, 1), new Vec3(-1, 1, 1), new Vec3(0, 0, 1)},
      new List<Vec3> { new Vec3(-1, 1, 1), new Vec3(-1, -1, 1), new Vec3(0, 0, 1)
    }
        });
        var geometry4 = new Geom3(
          new List<List<Vec3>> {
      new List<Vec3> {new Vec3(-1, -1, -1), new Vec3(-1, -1, 1), new Vec3(-1, 1, 1), new Vec3(-1, 1, -1)},
      new List<Vec3> {new Vec3(1, -1, -1), new Vec3(1, 1, -1), new Vec3(1, 1, 1), new Vec3(1, -1, 1)},
      new List<Vec3> {new Vec3(-1, -1, -1), new Vec3(1, -1, -1), new Vec3(1, -1, 1), new Vec3(-1, -1, 1)},
      new List<Vec3> {new Vec3(-1, 1, -1), new Vec3(-1, 1, 1), new Vec3(1, 1, 1), new Vec3(1, 1, -1)},
      new List<Vec3> {new Vec3(-1, -1, -1), new Vec3(-1, 1, -1), new Vec3(1, 1, -1), new Vec3(1, -1, -1)},
      // T junctions
      new List<Vec3> {new Vec3(-1, -1, 1), new Vec3(0, -1, 1), new Vec3(0, 0, 1)},
      new List<Vec3> {new Vec3(-1, 0, 1), new Vec3(-1, -1, 1), new Vec3(0, 0, 1)},

      new List<Vec3> {new Vec3(0, -1, 1), new Vec3(1, -1, 1), new Vec3(0, 0, 1)},
      new List<Vec3> {new Vec3(1, -1, 1), new Vec3(1, 0, 1), new Vec3(0, 0, 1)},

      new List<Vec3> {new Vec3(1, 0, 1), new Vec3(1, 1, 1), new Vec3(0, 0, 1)},
      new List<Vec3> {new Vec3(1, 1, 1), new Vec3(0, 1, 1), new Vec3(0, 0, 1)},

      new List<Vec3> {new Vec3(0, 1, 1), new Vec3(-1, 1, 1), new Vec3(0, 0, 1)},
      new List<Vec3> {new Vec3(-1, 1, 1), new Vec3(-1, 0, 1), new Vec3(0, 0, 1)}
          }
        );

        var result1 = insertTjunctions(geometry1.ToPolygons());
        Assert.AreSame(result1, geometry1.ToPolygons()); // no T junctions

        var result2 = insertTjunctions(geometry2.ToPolygons());
        Assert.AreSame(result2, geometry2.ToPolygons());// no T junctions;

        // NOTE: insertTjunctions does NOT split the polygon, it just adds a new point at each T

        var result3 = insertTjunctions(geometry3.ToPolygons());
        var exp = new Poly3[] {
    new Poly3(new Vec3[] {new Vec3(-1, -1, -1), new Vec3(-1, -1, 1), new Vec3(-1, 1, 1), new Vec3(-1, 1, -1)}),
    new Poly3(new Vec3[] {new Vec3(1, -1, -1), new Vec3(1, 1, -1), new Vec3(1, 1, 1), new Vec3(1, -1, 1)}),
    new Poly3(new Vec3[] {new Vec3(-1, -1, -1), new Vec3(1, -1, -1), new Vec3(1, -1, 1), new Vec3(-1, -1, 1)}),
    new Poly3(new Vec3[] {new Vec3(-1, 1, -1), new Vec3(-1, 1, 1), new Vec3(1, 1, 1), new Vec3(1, 1, -1)}),
    new Poly3(new Vec3[] {new Vec3(-1, -1, -1), new Vec3(-1, 1, -1), new Vec3(1, 1, -1), new Vec3(1, -1, -1)}),
    new Poly3(new Vec3[] {new Vec3(0, 0, 1), new Vec3(-1, -1, 1), new Vec3(1, -1, 1), new Vec3(1, 1, 1)}),
    new Poly3(new Vec3[] {new Vec3(1, 1, 1), new Vec3(-1, 1, 1), new Vec3(0, 0, 1)}),
    new Poly3(new Vec3[] {new Vec3(-1, 1, 1), new Vec3(-1, -1, 1), new Vec3(0, 0, 1)})
  };
        Assert.AreNotSame(result3, geometry3.ToPolygons());
        Assert.AreEqual(result3, exp);

        var result4 = insertTjunctions(geometry4.ToPolygons());
        exp = new Poly3[] {
    new Poly3(new Vec3[] {new Vec3(-1, -1, -1), new Vec3(-1, -1, 1), new Vec3(-1, 0, 1), new Vec3(-1, 1, 1), new Vec3(-1, 1, -1)}),
    new Poly3(new Vec3[] {new Vec3(1, -1, -1), new Vec3(1, 1, -1), new Vec3(1, 1, 1), new Vec3(1, 0, 1), new Vec3(1, -1, 1)}),
    new Poly3(new Vec3[] {new Vec3(-1, -1, -1), new Vec3(1, -1, -1), new Vec3(1, -1, 1), new Vec3(0, -1, 1), new Vec3(-1, -1, 1)}),
    new Poly3(new Vec3[] {new Vec3(-1, 1, -1), new Vec3(-1, 1, 1), new Vec3(0, 1, 1), new Vec3(1, 1, 1), new Vec3(1, 1, -1)}),
    new Poly3(new Vec3[] {new Vec3(-1, -1, -1), new Vec3(-1, 1, -1), new Vec3(1, 1, -1), new Vec3(1, -1, -1)}),
    new Poly3(new Vec3[] {new Vec3(-1, -1, 1), new Vec3(0, -1, 1), new Vec3(0, 0, 1)}),
    new Poly3(new Vec3[] {new Vec3(-1, 0, 1), new Vec3(-1, -1, 1), new Vec3(0, 0, 1)}),
    new Poly3(new Vec3[] {new Vec3(0, -1, 1), new Vec3(1, -1, 1), new Vec3(0, 0, 1)}),
    new Poly3(new Vec3[] {new Vec3(1, -1, 1), new Vec3(1, 0, 1), new Vec3(0, 0, 1)}),
    new Poly3(new Vec3[] {new Vec3(1, 0, 1), new Vec3(1, 1, 1), new Vec3(0, 0, 1)}),
    new Poly3(new Vec3[] {new Vec3(1, 1, 1), new Vec3(0, 1, 1), new Vec3(0, 0, 1)}),
    new Poly3(new Vec3[] {new Vec3(0, 1, 1), new Vec3(-1, 1, 1), new Vec3(0, 0, 1)}),
    new Poly3(new Vec3[] {new Vec3(-1, 1, 1), new Vec3(-1, 0, 1), new Vec3(0, 0, 1)})
  };
        Assert.AreNotSame(result4, geometry4);
        Assert.AreEqual(result4, exp);
    }

    [Test]
    public void TestSnapPolygons()
    {
        var polygons = new Geom3(new List<List<Vec3>> {
    // valid polygons
    new List<Vec3>{ new Vec3(0, 0, 0), new Vec3(0, 10, 0), new Vec3(0, 10, 10) }, // OK
    new List<Vec3>{ new Vec3(0, 0, 0), new Vec3(0, 10, 0), new Vec3(0, 10, 10), new Vec3(0, 0, 10) }, // OK
    // invalid polygons
    new List<Vec3>(0),
    new List<Vec3>{ new Vec3(0, 0, 0) },
    new List<Vec3>{ new Vec3(0, 0, 0), new Vec3(0, 10, 0) },
    // duplicated vertices
    new List<Vec3> {
      new Vec3(-24.445112000000115, 19.346837333333426, 46.47572533333356),
      new Vec3(-24.44446933333345, 19.346837333333426, 46.47508266666689),
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019),
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019)
    }, // OK
    new List<Vec3> {
      new Vec3(-24.445112000000115, 19.346837333333426, 46.47572533333356),
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019),
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019)
    },
    new List<Vec3> {
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019),
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019)
    },
    // duplicate vertices after snap
    new List<Vec3> {
      new Vec3(-24.445112000000115, 19.346837333333426, 46.47572533333356),
      new Vec3(-24.44446933333345, 19.346837333333426, 46.47508266666689),
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019),
      new Vec3(-23.70540266666678 - 0.00001234, 18.79864266666676 + 0.000001234, 39.56448800000019 + 0.00001234)
    }, // OK
    new List<Vec3> {
      new Vec3(-24.445112000000115, 19.346837333333426, 46.47572533333356),
      new Vec3(-23.70540266666678 - 0.00001234, 18.79864266666676 + 0.000001234, 39.56448800000019 + 0.00001234),
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019),
      new Vec3(-23.70540266666678 - 0.00001234, 18.79864266666676 + 0.000001234, 39.56448800000019 + 0.00001234)
    },
    new List<Vec3> {
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019),
      new Vec3(-23.70540266666678 - 0.00001234, 18.79864266666676 + 0.000001234, 39.56448800000019 + 0.00001234),
      new Vec3(-23.70540266666678, 18.79864266666676, 39.56448800000019),
      new Vec3(-23.70540266666678 - 0.00001234, 18.79864266666676 + 0.000001234, 39.56448800000019 + 0.00001234)
    },
    // inverted polygon
    new List<Vec3> {
      new Vec3(20.109133333333336, -4.894033333333335, -1.0001266666666668),
      new Vec3(20.021120000000003, -5.1802133333333344, -1.0001266666666668),
      new Vec3(20.020300000000002, -5.182946666666668, -1.0001266666666668),
      new Vec3(10.097753333333335, -5.182946666666668, -1.0001266666666668),
      new Vec3(10.287720000000002, -4.894033333333335, -1.0001266666666668)
    }
  }).ToPolygons();

        var results = snapPolygons(0.0001, polygons);
        Assert.AreEqual(results.Length, 5);

        var exp3 = new Poly3(new Vec3[] {
    new Vec3(-24.4451, 19.3468, 46.4757),
    new Vec3(-24.4445, 19.3468, 46.475100000000005),
    new Vec3(-23.7054, 18.7986, 39.5645)
  });

        Assert.AreEqual(results[3].Vertices, exp3.Vertices);
    }

#if NOTUSED
    [Test]
    public void TestSnapGeom2()
    {
        var geometry1 = new Geom2();
        var geometry2 = Rectangle(size: (1, 1), center: (0, 0));
        var geometry3 = Rectangle(size: (1.3333333333333333333333, 1.3333333333333333333333), center: (0, 0));
        var geometry4 = Rectangle(size: (Math.PI * 1000, Math.PI * 1000), center: (0, 0));

        var results = new Geom2[4];
        results[0] = snap(geometry1);
        results[1] = snap(geometry2);
        results[2] = snap(geometry3);
        results[3] = snap(geometry4);

        var pts = results[0].ToPoints();
        var exp = new Vec2[] { };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        pts = results[1].ToPoints();
        exp = new Vec2[] { new Vec2(-0.5, -0.5), new Vec2(0.5, -0.5), new Vec2(0.5, 0.5), new Vec2(-0.5, 0.5) };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        pts = results[2].ToPoints();
        exp = new Vec2[] {
          new Vec2(-0.6666666666666666, -0.6666666666666666), new Vec2(0.6666666666666666, -0.6666666666666666),
          new Vec2(0.6666666666666666, 0.6666666666666666), new Vec2(-0.6666666666666666, 0.6666666666666666)
        };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        pts = results[3].ToPoints();
        exp = new Vec2[] {
          new Vec2(-1570.7963267948967, -1570.7963267948967), new Vec2(1570.7963267948967, -1570.7963267948967),
          new Vec2(1570.7963267948967, 1570.7963267948967), new Vec2(-1570.7963267948967, 1570.7963267948967)
        };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
    }
#endif

    [Test]
    public void TestSnapGeom3()
    {
        var geometry1 = new Geom3();
        var geometry2 = Cuboid(size: (1, 1, 1), center: (0, 0, 0));
        var geometry3 = Cuboid(size: (1.3333333333333333333333, 1.3333333333333333333333, 1.3333333333333333333333), center: (0, 0, 0));
        var geometry4 = Cuboid(size: (Math.PI * 1000, Math.PI * 1000, Math.PI * 1000), center: (0, 0, 0));

        var results = new Geom3[4];
        results[0] = snap(geometry1);
        results[1] = snap(geometry2);
        results[2] = snap(geometry3);
        results[3] = snap(geometry4);

        var pts = results[0].ToPoints();
        var exp = new List<List<Vec3>>(0);
        Assert.IsTrue(Helpers.CompareListOfLists(pts, exp));

        pts = results[1].ToPoints();
        if(WriteTests) TestData.Make("SnapGeom3Exp1", pts);
        exp = UnitTestData.SnapGeom3Exp1;
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        pts = results[2].ToPoints();
        if(WriteTests) TestData.Make("SnapGeom3Exp2", pts);
        exp = UnitTestData.SnapGeom3Exp2;
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        pts = results[3].ToPoints();
        if(WriteTests) TestData.Make("SnapGeom3Exp3", pts);
        exp = UnitTestData.SnapGeom3Exp3;
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestclosestPoint()
    {
        var line1 = (new Vec3(), new Vec3(0, 0, 1)); // line follows X axis
        var x1 = closestPoint(line1, new Vec3(0, 0, 0));
        Assert.IsTrue(x1.IsNearlyEqual(new Vec3(0, 0, 0)));
        var x2 = closestPoint(line1, new Vec3(0, 1, 0));
        Assert.IsTrue(x2.IsNearlyEqual(new Vec3(0, 0, 0)));
        var x3 = closestPoint(line1, new Vec3(6, 0, 0));
        Assert.IsTrue(x3.IsNearlyEqual(new Vec3(0, 0, 0))); // rounding errors

        var line2 = (new Vec3(-5, -5, -5), new Vec3(5, 5, 5));
        var x4 = closestPoint(line2, new Vec3(0, 0, 0));
        Assert.IsTrue(x4.IsNearlyEqual(new Vec3(0.000000000000, 0.000000000000, 0.00000000000)));
        var x5 = closestPoint(line2, new Vec3(1, 0, 0));
        Assert.IsTrue(x5.IsNearlyEqual(new Vec3(0.3333333333333339, 0.3333333333333339, 0.3333333333333339)));
        var x6 = closestPoint(line2, new Vec3(2, 0, 0));
        Assert.IsTrue(x6.IsNearlyEqual(new Vec3(0.6666666666666661, 0.6666666666666661, 0.6666666666666661)));
        var x7 = closestPoint(line2, new Vec3(3, 0, 0));
        Assert.IsTrue(x7.IsNearlyEqual(new Vec3(1, 1, 1)));
        var x8 = closestPoint(line2, new Vec3(4, 0, 0));
        Assert.IsTrue(x8.IsNearlyEqual(new Vec3(1.3333333333333348, 1.3333333333333348, 1.3333333333333348)));
        var x9 = closestPoint(line2, new Vec3(5, 0, 0));
        Assert.IsTrue(x9.IsNearlyEqual(new Vec3(1.666666666666667, 1.666666666666667, 1.666666666666667)));
        var x10 = closestPoint(line2, new Vec3(50, 0, 0));
        Assert.IsTrue(x10.IsNearlyEqual(new Vec3(16.666666666666668, 16.666666666666668, 16.666666666666668)));

        var ya = closestPoint(line2, new Vec3(-5, -5, -5));
        Assert.IsTrue(ya.IsNearlyEqual(new Vec3(-5, -5, -5)));
        var yb = closestPoint(line2, new Vec3(5, 5, 5));
        Assert.IsTrue(yb.IsNearlyEqual(new Vec3(5, 5, 5)));
    }
}
