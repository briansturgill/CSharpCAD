namespace CSharpCADTests;

[TestFixture]
public class ExtrudeLinearTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExtrudeLinearDefaults()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(5, 5), new Vec2(-5, 5), new Vec2(-5, -5), new Vec2(5, -5) });
        Assert.DoesNotThrow(() => geometry2.Validate());

        var geometry3 = ExtrudeLinear(geometry2);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeLinearDefExp", pts);
        var exp = UnitTestData.ExtrudeLinearDefExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestExtrudeLinearNoTwist()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(5, 5), new Vec2(-5, 5), new Vec2(-5, -5), new Vec2(5, -5) });
        Assert.DoesNotThrow(() => geometry2.Validate());

        var geometry3 = ExtrudeLinear(geometry2, height: 15);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeLinearNoTwistExp", pts);
        var exp = UnitTestData.ExtrudeLinearNoTwistExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        geometry3 = ExtrudeLinear(geometry2, height: -15);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeLinearNoTwistExp2", pts);
        exp = UnitTestData.ExtrudeLinearNoTwistExp2;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestExtrudeLinearTwist()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(5, 5), new Vec2(-5, 5), new Vec2(-5, -5), new Vec2(5, -5) });
        Assert.DoesNotThrow(() => geometry2.Validate());

        var geometry3 = ExtrudeLinear(geometry2, height: 15, twistAngle: Math.PI / -4);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        var exp = UnitTestData.ExtrudeLinearTwistExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        geometry3 = ExtrudeLinear(geometry2, height: 15, twistAngle: Math.PI / 2, twistSteps: 3);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        exp = UnitTestData.ExtrudeLinearTwistExp2;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        geometry3 = ExtrudeLinear(geometry2, height: 15, twistAngle: Math.PI / 2, twistSteps: 30);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 244);
    }

    [Test]
    public void TestExtrudeLinearHoles()
    {
        var geometry2 = new Geom2(UnitTestData.ExtrudeLinearHolesSides);
        var geometry3 = ExtrudeLinear(geometry2, height: 15);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        var exp = UnitTestData.ExtrudeLinearHolesExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
