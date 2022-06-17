namespace CSharpCADTests;

[TestFixture]
public class SubtractTests
{
    static bool WriteTests = false;

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
        if(WriteTests) TestData.Make("SubtractTransformGeom3Exp1", obs);
        var exp = UnitTestData.SubtractTransformGeom3Exp1;
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
        if(WriteTests) TestData.Make("SubtractTransformGeom3Exp2", obs);
        exp = UnitTestData.SubtractTransformGeom3Exp2;
        Assert.AreEqual(obs.Count, 12);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // subtract of two completely overlapping objects
        var result4 = (Geom3)Subtract(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        Assert.AreEqual(obs.Count, 0);
    }
}