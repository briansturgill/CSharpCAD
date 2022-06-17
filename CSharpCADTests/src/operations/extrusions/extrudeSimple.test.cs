namespace CSharpCADTests;

[TestFixture]
public class ExtrudeSimpleTests
{
    static bool WriteTests = true;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExtrudeSimpleDefaults()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(5, 5), new Vec2(-5, 5), new Vec2(-5, -5), new Vec2(5, -5) });
        Assert.DoesNotThrow(() => geometry2.Validate());

        var geometry3 = ExtrudeSimple(geometry2);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeSimpleDefExp", pts);
        var exp = UnitTestData.ExtrudeSimpleDefExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestExtrudeSimpleOpts()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(5, 5), new Vec2(-5, 5), new Vec2(-5, -5), new Vec2(5, -5) });
        Assert.DoesNotThrow(() => geometry2.Validate());

        var geometry3 = ExtrudeSimple(geometry2, height: 15);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeSimpleOptsExp", pts);
        var exp = UnitTestData.ExtrudeSimpleOptsExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        geometry3 = ExtrudeSimple(geometry2, height: -15);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeSimpleOptsExp2", pts);
        exp = UnitTestData.ExtrudeSimpleOptsExp2;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestExtrudeSimpleHoles()
    {
        var geometry2 = Subtract(Circle(radius: 10), Circle(radius: 6));
        Assert.DoesNotThrow(() => geometry2.Validate());
        Assert.Throws<ArgumentException>(() => ExtrudeSimple(geometry2, height: 15));
    }
}
