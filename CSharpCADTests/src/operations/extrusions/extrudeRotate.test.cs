namespace CSharpCADTests;

[TestFixture]
public class ExtrudeRotateTests
{
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
        Assert.AreEqual(pts.Count, 96);
    }


    [Test]
    public void TestExtrudeRotateAngle()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, 8), new Vec2(10, -8), new Vec2(26, -8), new Vec2(26, 8) });

        // test angle
        var geometry3 = ExtrudeRotate(geometry2, segments: 4, angle: 45);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        var exp = UnitTestData.ExtrudeRotateAngleExp;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        geometry3 = ExtrudeRotate(geometry2, segments: 4, angle: -250);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 28);

        geometry3 = ExtrudeRotate(geometry2, segments: 4, angle: 250);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 28);
    }

    [Test]
    public void TestExtrudeRotateStartAngle()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, 8), new Vec2(10, -8), new Vec2(26, -8), new Vec2(26, 8) });

        // test startAngle
        var geometry3 = ExtrudeRotate(geometry2, segments: 5, startAngle: 45);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        var exp = new List<Vec3>{
          new Vec3(7.0710678118654755, 7.071067811865475, 8),
          new Vec3(18.38477631085024, 18.384776310850235, 8),
          new Vec3(-11.803752993228215, 23.166169628897567, 8)
        };
        Assert.AreEqual(pts.Count, 40);
        Assert.IsTrue(Helpers.CompareListsNEVec3(pts[0], exp));

        geometry3 = ExtrudeRotate(geometry2, segments: 5, startAngle: -45);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        exp = new List<Vec3> {
          new Vec3(7.0710678118654755, -7.071067811865475, 8),
          new Vec3(18.38477631085024, -18.384776310850235, 8),
          new Vec3(23.166169628897567, 11.803752993228215, 8)
        };
        Assert.AreEqual(pts.Count, 40);
        Assert.IsTrue(Helpers.CompareListsNEVec3(pts[0], exp));
    }

    [Test]
    public void TestExtrudeRotateSegments()
    {
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(10, 8), new Vec2(10, -8), new Vec2(26, -8), new Vec2(26, 8) });

        // test segments
        var geometry3 = ExtrudeRotate(geometry2, segments: 4);
        Assert.DoesNotThrow(() => geometry3.Validate());
        var pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 32);

        geometry3 = ExtrudeRotate(geometry2, segments: 64);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 512);

        // test overlapping edges
        geometry2 = new Geom2(new List<Vec2> { new Vec2(0, 0), new Vec2(2, 1), new Vec2(1, 2), new Vec2(1, 3), new Vec2(3, 4), new Vec2(0, 5) });
        geometry3 = ExtrudeRotate(geometry2, segments: 8);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 64);

        // test overlapping edges that produce hollow shape
        geometry2 = new Geom2(new List<Vec2>{ new Vec2(30, 0), new Vec2(30, 60), new Vec2(0, 60), new Vec2(0, 50),
          new Vec2(10, 40), new Vec2(10, 30), new Vec2(0, 20), new Vec2(0, 10), new Vec2(10, 0)});
        geometry3 = ExtrudeRotate(geometry2, segments: 8);
        Assert.DoesNotThrow(() => geometry3.Validate());
        pts = geometry3.ToPoints();
        Assert.AreEqual(pts.Count, 80);
    }

    [Test]
    public void TestExtrudeRotateOverlap()
    {
        // overlap of Y axis; even number of + and - points
        var geometry = new Geom2(new List<Vec2> { new Vec2(-1, 8), new Vec2(-1, -8), new Vec2(7, -8), new Vec2(7, 8) });

        var obs = ExtrudeRotate(geometry, segments: 4, angle: 90);
        Assert.DoesNotThrow(() => geometry.Validate());
        var pts = obs.ToPoints();
        var exp = UnitTestData.ExtrudeRotateOverlapExp;
        Assert.AreEqual(pts.Count, exp.Count);
        //Helpers.PrintListOfLists("pts,exp", pts, exp);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        // overlap of Y axis; larger number of - points
        geometry = new Geom2(new List<Vec2> { new Vec2(-1, 8), new Vec2(-2, 4), new Vec2(-1, -8), new Vec2(7, -8), new Vec2(7, 8) });

        obs = ExtrudeRotate(geometry, segments: 8, angle: 90);
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = obs.ToPoints();
        exp = UnitTestData.ExtrudeRotateOverlapExp2;
        Assert.AreEqual(pts.Count, exp.Count);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }
}