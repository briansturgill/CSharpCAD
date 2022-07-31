namespace CSharpCADTests;

[TestFixture]
public class UnionTests
{
    static bool WriteTests = false;
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

        var geometry1 = Circle(radius: 2, segments: 8);
        Assert.DoesNotThrow(() => geometry1.Validate());

        // union of one object
        Geom2 result1 = Union(geometry1);
        Assert.DoesNotThrow(() => result1.Validate());
        var obs = result1.ToPoints();
        if(WriteTests) TestData.Make("UnionGeom2Exp1", obs);
        var exp = UnitTestData.UnionGeom2Exp1;
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // union of two non-overlapping objects
        Geom2 obj = Rectangle(size: (4, 4), center: (0,0));
        Geom2 geometry2 = Center(obj, relativeTo: new Vec2(10, 10));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = Union(geometry1, geometry2);
        Assert.DoesNotThrow(() => result2.Validate());
        obs = result2.ToPoints();
        if(WriteTests) TestData.Make("UnionGeom2Exp2", obs);
        exp = UnitTestData.UnionGeom2Exp2;
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // union of two partially overlapping objects
        var geometry3 = Rectangle(size: (18, 18), center: (0,0));
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = Union(geometry2, geometry3);
        Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        if(WriteTests) TestData.Make("UnionGeom2Exp3", obs);
        exp = UnitTestData.UnionGeom2Exp3;
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // union of two completely overlapping objects
        var result4 = Union(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        if(WriteTests) TestData.Make("UnionGeom2Exp4", obs);
        exp = UnitTestData.UnionGeom2Exp4;
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // union of unions of non-overlapping objects (BSP gap from #907)
        var circ = Circle(radius: 1, segments: 32);
        Assert.DoesNotThrow(() => circ.Validate());
        var result5 = Union(
          Union(
            Translate(new Vec2(17, 21), circ),
            Translate(new Vec2(7, 0), circ)
          ),
          Union(
            Translate(new Vec2(3, 21), circ),
            Translate(new Vec2(17, 21), circ)
          )
        );
        obs = result5.ToPoints();
        Assert.DoesNotThrow(() => result5.Validate());
        Assert.AreEqual(obs.Length, 96);
    }

    [Test]
    public void TestUnionGeom3()
    {
        var geometry1 = Sphere(radius: 2, segments: 8);
        Assert.DoesNotThrow(() => geometry1.Validate());

        // union of one object
        var result1 = Union(geometry1);
        Assert.DoesNotThrow(() => result1.Validate());
        var obs = result1.ToPoints();
        if(WriteTests) TestData.Make("UnionGeom3Exp1", obs);
        var exp = UnitTestData.UnionGeom3Exp1;
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // union of two non-overlapping objects
        var obj2 = Cuboid(size: (4, 4, 4), center: (0,0,0));
        var geometry2 = Center(obj2, relativeTo: new Vec3(10, 10, 10));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = Union(geometry1, geometry2);
        obs = result2.ToPoints();
        Assert.DoesNotThrow(() => result2.Validate());
        Assert.AreEqual(obs.Count, 38);

        // union of two partially overlapping objects
        var geometry3 = Cuboid(size: (18, 18, 18), center: (0,0,0));
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = Union(geometry2, geometry3);
        // LATER JSCAD Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        if(WriteTests) TestData.Make("UnionGeom3Exp2", obs);
        exp = UnitTestData.UnionGeom3Exp2;
        Assert.AreEqual(obs.Count, 18);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // union of two completely overlapping objects
        var result4 = Union(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        if(WriteTests) TestData.Make("UnionGeom3Exp3", obs);
        exp = UnitTestData.UnionGeom3Exp3;
        Assert.AreEqual(obs.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));
    }

    [Test]
    public void TestUnionGeom3WithRounding()
    {
        //'union of geom3 with rounding issues #137'
        var obj1 = Cuboid(size: (44, 26, 5), center: (0,0,0));
        var geometry1 = Center(obj1, relativeTo: new Vec3(0, 0, -1));
        Assert.DoesNotThrow(() => geometry1.Validate());
        var obj2 = Cuboid(size: (44, 26, 1.8), center: (0,0,0)); // introduce percision error
        var geometry2 = Center(obj2, relativeTo: new Vec3(0, 0, -4.400001)); // introduce percision error
        Assert.DoesNotThrow(() => geometry2.Validate());

        var obs = Union(geometry1, geometry2);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Count, 6); // number of polygons in union
    }
}