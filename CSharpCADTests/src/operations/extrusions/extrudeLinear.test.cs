namespace CSharpCADTests;

[TestFixture]
public class ExtrudeLinearTests
{
    static bool WriteTests = false;
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

        var geometry3 = ExtrudeTwist(geometry2, height: 15, twistAngle: -45);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeLinearTwistExp", pts);
        var exp = UnitTestData.ExtrudeLinearTwistExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        geometry3 = ExtrudeTwist(geometry2, height: 15, twistAngle: 90, twistSteps: 3);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeLinearTwistExp2", pts);
        exp = UnitTestData.ExtrudeLinearTwistExp2;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        geometry3 = ExtrudeTwist(geometry2, height: 15, twistAngle: 90, twistSteps: 30);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 244);
    }

    [Test]
    public void TestExtrudeLinearHoles()
    {
        var nrtree = new Geom2.NRTree();
        nrtree.Insert(new Vec2[] {
            new Vec2(-5, -5),
            new Vec2(5, -5),
            new Vec2(5, 5),
            new Vec2(-5, 5)
        });
        nrtree.Insert(new Vec2[] {
            new Vec2(-2, 2),
            new Vec2(2, 2),
            new Vec2(2, -2),
            new Vec2(-2, -2)
        });

        //var geometry2 = new Geom2(UnitTestData.ExtrudeLinearHolesSides);
        var geometry2 = new Geom2(nrtree);
        Assert.DoesNotThrow(() => geometry2.Validate());
        var geometry3 = ExtrudeLinear(geometry2, height: 15);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeLinearHolesExp", pts);
        var exp = UnitTestData.ExtrudeLinearHolesExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}
