namespace CSharpCADTests;

[TestFixture]
public class IntersectTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestIntersetGeom2()
    {
        var geometry1 = Circle(radius: 2, segments: 8);
        Assert.DoesNotThrow(() => geometry1.Validate());

        // intersect of one object
        var result1 = (Geom2)Intersect(geometry1);
        Assert.DoesNotThrow(() => result1.Validate());
        var obs = result1.ToPoints();
        var exp = UnitTestData.IntersectGeom2Exp1;
        Assert.AreEqual(obs.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // intersect of two non-overlapping objects
        var geometry2 = (Geom2)Center(Rectangle(size: (4, 4), center: (0, 0)), relativeTo: new Vec3(10, 10, 0));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = (Geom2)Intersect(geometry1, geometry2);
        Assert.DoesNotThrow(() => result2.Validate());
        obs = result2.ToPoints();
        Assert.AreEqual(obs.Length, 0);

        // intersect of two partially overlapping objects
        var geometry3 = Rectangle(size: (18, 18), center: (0, 0));
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = (Geom2)Intersect(geometry2, geometry3);
        Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        exp = new Vec2[] {
    new Vec2(9, 9), new Vec2(8, 9), new Vec2(8, 8), new Vec2(9, 8)
  };
        Assert.AreEqual(obs.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // intersect of two completely overlapping objects
        var result4 = (Geom2)Intersect(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        exp = UnitTestData.IntersectGeom2Exp2;
        Assert.AreEqual(obs.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));
    }

    [Test]
    public void TestIntersetGeom3()
    {
        var geometry1 = Sphere(radius: 2, segments: 8);
        Assert.DoesNotThrow(() => geometry1.Validate());

        // intersect of one object
        var result1 = (Geom3)Intersect(geometry1);
        Assert.DoesNotThrow(() => result1.Validate());
        var obs = result1.ToPoints();
        var exp = UnitTestData.IntersectGeom3Exp1;
        Assert.AreEqual(obs.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // intersect of two non-overlapping objects
        var geometry2 = (Geom3)Center(Cuboid(size: (4, 4, 4), center: (0, 0, 0)), relativeTo: new Vec3(10, 10, 10));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = (Geom3)Intersect(geometry1, geometry2);
        Assert.DoesNotThrow(() => result2.Validate());
        obs = result2.ToPoints();
        Assert.AreEqual(obs.Count, 0);

        // intersect of two partially overlapping objects
        var geometry3 = Cuboid(size: (18, 18, 18), center: (0, 0, 0));
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = (Geom3)Intersect(geometry2, geometry3);
        Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();

        exp = UnitTestData.IntersectGeom3Exp2;

        Assert.AreEqual(obs.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // intersect of two completely overlapping objects
        var result4 = (Geom3)Intersect(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        Assert.AreEqual(obs.Count, 32);
    }

}