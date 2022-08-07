namespace CSharpCADTests;

[TestFixture]
public class ExtrudeRotateTests
{
    static bool WriteTests = false;

    private static Geom3 makeGeom3(Vec3[] points, Mat4? transforms = null)
    {
        var poly = new Poly3(points);
        var poly_array = new Poly3[] { poly };
        return new Geom3(poly_array, transforms);
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestExtrudeRotateDefaults()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, 8), new Vec2(10, -8), new Vec2(26, -8), new Vec2(26, 8) });

        var geometry3 = ExtrudeRotate(geometry2);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 256);
    }


    [Test]
    public void TestExtrudeRotateAngle()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, 8), new Vec2(10, -8), new Vec2(26, -8), new Vec2(26, 8) });
        Assert.DoesNotThrow(() => geometry2.Validate());

        // test angle
        var geometry3 = ExtrudeRotate(geometry2, segments: 5, angle: 45);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if(WriteTests) TestData.Make("ExtrudeRotateAngleExp", pts);
        var exp = UnitTestData.ExtrudeRotateAngleExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        Assert.Throws<ArgumentException>(() => ExtrudeRotate(geometry2, segments: 4, angle: -250));

        geometry3 = ExtrudeRotate(geometry2, segments: 5, angle: 250);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 32);
    }

    [Test]
    public void TestExtrudeRotateStartAngle()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, 8), new Vec2(10, -8), new Vec2(26, -8), new Vec2(26, 8) });

        // test startAngle
        var geometry3 = ExtrudeRotate(geometry2, segments: 5, startAngle: 45);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        if(WriteTests) TestData.Make("ExtrudeRotateStartAngleExp", pts);
        var exp = UnitTestData.ExtrudeRotateStartAngleExp;
        Assert.AreEqual(pts.Count, 40);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        geometry3 = ExtrudeRotate(geometry2, segments: 5, startAngle: 5);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        if(WriteTests) TestData.Make("ExtrudeRotateStartAngleExp2", pts);
        exp = UnitTestData.ExtrudeRotateStartAngleExp2;
        Assert.AreEqual(pts.Count, 40);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestExtrudeRotateSegments()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, 8), new Vec2(10, -8), new Vec2(26, -8), new Vec2(26, 8) });
        Assert.DoesNotThrow(() => geometry2.Validate());
        // test segments
        var geometry3 = ExtrudeRotate(geometry2, segments: 5);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 40);

        geometry3 = ExtrudeRotate(geometry2, segments: 64);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 512);

        // test overlapping edges
        geometry2 = new Geom2(new List<Vec2> { new Vec2(0, 0), new Vec2(2, 1), new Vec2(1, 2), new Vec2(1, 3), new Vec2(3, 4), new Vec2(0, 5) });
        Assert.DoesNotThrow(() => geometry2.Validate());
        geometry3 = ExtrudeRotate(geometry2, segments: 8);
        // JSCAD Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 96);

        // test overlapping edges that produce hollow shape
        geometry2 = new Geom2(new List<Vec2>{ new Vec2(30, 0), new Vec2(30, 60), new Vec2(0, 60), new Vec2(0, 50),
          new Vec2(10, 40), new Vec2(10, 30), new Vec2(0, 20), new Vec2(0, 10), new Vec2(10, 0)});
        geometry3 = ExtrudeRotate(geometry2, segments: 8);
        // LATER JSCAD Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 144);
    }

    [Test]
    public void TestExtrudeRotateOverlap()
    {
        // overlap of Y axis; even number of + and - points
        var geometry = new Geom2(new List<Vec2> { new Vec2(-1, 8), new Vec2(-1, -8), new Vec2(7, -8), new Vec2(7, 8) });

        var obs = ExtrudeRotate(geometry, segments: 5, angle: 90);
        Assert.DoesNotThrow(() => geometry.Validate());
        var pts = obs.ToPoints();
        if(WriteTests) TestData.Make("ExtrudeRotateOverlapExp", pts);
        var exp = UnitTestData.ExtrudeRotateOverlapExp;
        Assert.AreEqual(pts.Count, exp.Count);
        //Helpers.PrintListOfLists("pts,exp", pts, exp);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // overlap of Y axis; larger number of - points
        geometry = new Geom2(new List<Vec2> { new Vec2(-1, 8), new Vec2(-2, 4), new Vec2(-1, -8), new Vec2(7, -8), new Vec2(7, 8) });

        obs = ExtrudeRotate(geometry, segments: 8, angle: 90);
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = obs.ToPoints();
        if(WriteTests) TestData.Make("ExtrudeRotateOverlapExp2", pts);
        exp = UnitTestData.ExtrudeRotateOverlapExp2;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}