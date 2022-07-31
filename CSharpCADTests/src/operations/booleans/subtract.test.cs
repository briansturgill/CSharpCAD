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
        var result1 = Subtract(geometry1);
        Assert.DoesNotThrow(() => result1.Validate());
        var obs = result1.ToPoints();
        if(WriteTests) TestData.Make("SubtractTransformExp1", obs);
        var exp = UnitTestData.SubtractTransformExp1;
        Assert.AreEqual(obs.Length, 8);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // subtract of two non-overlapping objects
        var geometry2 = Center(Rectangle(size: (4, 4), center: (0, 0)), relativeTo: new Vec2(10, 10));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = Subtract(geometry1, geometry2);
        Assert.DoesNotThrow(() => result2.Validate());
        obs = result2.ToPoints();
        if(WriteTests) TestData.Make("SubtractTransformExp2", obs);
        exp = UnitTestData.SubtractTransformExp2;
        Assert.AreEqual(obs.Length, 8);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // subtract of two partially overlapping objects
        var geometry3 = Rectangle(size: (18, 18), center: (0,0));
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = Subtract(geometry2, geometry3);
        Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        if(WriteTests) TestData.Make("SubtractOverLappingObjectsExp1", obs);
        exp = UnitTestData.SubtractOverLappingObjectsExp1;
        Assert.AreEqual(obs.Length, 6);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(obs, exp));

        // subtract of two completely overlapping objects
        var result4 = Subtract(geometry1, geometry3);
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
        var result1 = Subtract(geometry1);
        Assert.DoesNotThrow(() => result1.Validate());
        var obs = result1.ToPoints();
        if(WriteTests) TestData.Make("SubtractTransformGeom3Exp1", obs);
        var exp = UnitTestData.SubtractTransformGeom3Exp1;
        Assert.AreEqual(obs.Count, 32);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // subtract of two non-overlapping objects
        var geometry2 = Center(Cuboid(size: (4, 4, 4), center: (0, 0, 0)), relativeTo: (10, 10, 10));
        Assert.DoesNotThrow(() => geometry2.Validate());

        var result2 = Subtract(geometry1, geometry2);
        Assert.DoesNotThrow(() => result2.Validate());
        obs = result2.ToPoints();
        Assert.AreEqual(obs.Count, 32);

        // subtract of two partially overlapping objects
        var geometry3 = Cuboid(size: (18, 18, 18), center: (0, 0, 0));
        Assert.DoesNotThrow(() => geometry3.Validate());

        var result3 = Subtract(geometry2, geometry3);
        // LATER JSCAD Assert.DoesNotThrow(() => result3.Validate());
        obs = result3.ToPoints();
        if(WriteTests) TestData.Make("SubtractTransformGeom3Exp2", obs);
        exp = UnitTestData.SubtractTransformGeom3Exp2;
        Assert.AreEqual(obs.Count, 12);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(obs, exp));

        // subtract of two completely overlapping objects
        var result4 = Subtract(geometry1, geometry3);
        Assert.DoesNotThrow(() => result4.Validate());
        obs = result4.ToPoints();
        Assert.AreEqual(obs.Count, 0);
    }
}
