namespace CSharpCADTests;

[TestFixture]
public class ExtrudeSimpleBetweenTests
{
    static bool WriteTests = true;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExtrudeSimpleBetweenDefaults()
    {
        var geometry1 = Circle(radius: 20);
        var geometry2 = Circle(radius: 10);
        Assert.DoesNotThrow(() => geometry1.Validate());
        Assert.DoesNotThrow(() => geometry2.Validate());

        var geometry3 = ExtrudeSimpleBetween(geometry1, geometry2, height: 5);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeSimpleBetweenDefExp", pts);
        var exp = UnitTestData.ExtrudeSimpleBetweenDefExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestExtrudeSimpleBetweenOpts()
    {
        var geometry1 = Circle(radius: 20);
        var geometry2 = Circle(radius: 10);
        Assert.DoesNotThrow(() => geometry1.Validate());
        Assert.DoesNotThrow(() => geometry2.Validate());

        var geometry3 = ExtrudeSimpleBetween(geometry1, geometry2, height: 15, center_z: 23);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if (WriteTests) TestData.Make("ExtrudeSimpleBetweenOptsExp", pts);
        var exp = UnitTestData.ExtrudeSimpleBetweenOptsExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestExtrudeSimpleBetweenHoles()
    {
        var geometry1 = Circle(radius: 20);
        var geometry2 = Subtract(Circle(radius: 10), Circle(radius: 6));
        Assert.DoesNotThrow(() => geometry1.Validate());
        Assert.DoesNotThrow(() => geometry2.Validate());
        Assert.Throws<ArgumentException>(() => ExtrudeSimpleBetween(geometry1, geometry2, height: 15));
        Assert.Throws<ArgumentException>(() => ExtrudeSimpleBetween(geometry2, geometry1, height: 15));
    }
}