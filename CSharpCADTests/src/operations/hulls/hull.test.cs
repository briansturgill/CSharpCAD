namespace CSharpCADTests;

[TestFixture]
public class HullTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestHullSingleGeom2()
    {
        var geometry = new Geom2();

        var obs = (Geom2)Hull(geometry);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 0);

        geometry = new Geom2(new Vec2[] { new Vec2(5, 5), new Vec2(-5, 5), new Vec2(-5, -5), new Vec2(5, -5) });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = (Geom2)Hull(geometry);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 4);

        // convex C shape
        geometry = new Geom2(new Vec2[]{
          new Vec2(5.00000, 8.66025),
          new Vec2(-5.00000, 8.66025),
          new Vec2(-10.00000, 0.00000),
          new Vec2(-5.00000, -8.66025),
          new Vec2(5.00000, -8.66025),
          new Vec2(6.00000, -6.92820),
          new Vec2(-2.00000, -6.92820),
          new Vec2(-6.00000, 0.00000),
          new Vec2(-2.00000, 6.92820),
          new Vec2(6.00000, 6.92820)
        });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = (Geom2)Hull(geometry);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 7);
    }


    [Test]
    public void TestHullMultipleOverlappingGeom2()
    {
        var geometry1 = new Geom2(new Vec2[] { new Vec2(5, 5), new Vec2(-5, 5), new Vec2(-5, -5), new Vec2(5, -5) });
        var geometry2 = new Geom2(new Vec2[] { new Vec2(3, 3), new Vec2(-3, 3), new Vec2(-3, -3), new Vec2(3, -3) });
        var geometry3 = new Geom2(new Vec2[] { new Vec2(6, 3), new Vec2(-6, 3), new Vec2(-6, -3), new Vec2(6, -3) });

        // convex C shape
        var geometry4 = new Geom2(new Vec2[]{
          new Vec2(5.00000, 8.66025),
          new Vec2(-5.00000, 8.66025),
          new Vec2(-10.00000, 0.00000),
          new Vec2(-5.00000, -8.66025),
          new Vec2(5.00000, -8.66025),
          new Vec2(6.00000, -6.92820),
          new Vec2(-2.00000, -6.92820),
          new Vec2(-6.00000, 0.00000),
          new Vec2(-2.00000, 6.92820),
          new Vec2(6.00000, 6.92820)
        });
        Assert.DoesNotThrow(() => geometry1.Validate());
        Assert.DoesNotThrow(() => geometry2.Validate());
        Assert.DoesNotThrow(() => geometry3.Validate());
        Assert.DoesNotThrow(() => geometry4.Validate());

        // same
        var obs = (Geom2)Hull(geometry1, geometry1);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 4);

        // one inside another
        obs = (Geom2)Hull(geometry1, geometry2);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 4);

        // one overlapping another
        obs = (Geom2)Hull(geometry1, geometry3);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 8);

        obs = (Geom2)Hull(geometry2, geometry4);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 7);
    }


    [Test]
    public void TestHullMultipleVariousGeom2()
    {
        var geometry1 = new Geom2(new Vec2[] { new Vec2(6, 6), new Vec2(0, 6), new Vec2(0, 0), new Vec2(6, 0) });
        var geometry2 = new Geom2(new Vec2[] { new Vec2(6, 3), new Vec2(-6, 3), new Vec2(-6, -3), new Vec2(6, -3) });
        var geometry3 = new Geom2(new Vec2[] { new Vec2(-10, -10), new Vec2(0, -20), new Vec2(10, -10) });

        // convex C shape
        var geometry4 = new Geom2(new Vec2[]{
            new Vec2(5.00000, 8.66025),
            new Vec2(-5.00000, 8.66025),
            new Vec2(-10.00000, 0.00000),
            new Vec2(-5.00000, -8.66025),
            new Vec2(5.00000, -8.66025),
            new Vec2(6.00000, -6.92820),
            new Vec2(-2.00000, -6.92820),
            new Vec2(-6.00000, 0.00000),
            new Vec2(-2.00000, 6.92820),
            new Vec2(6.00000, 6.92820)
          });
        var geometry5 = new Geom2(new Vec2[] { new Vec2(-17, -17), new Vec2(-23, -17), new Vec2(-23, -23), new Vec2(-17, -23) });

        var obs = (Geom2)Hull(geometry1, geometry2);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 5);

        obs = (Geom2)Hull(geometry1, geometry3);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 5);

        obs = (Geom2)Hull(geometry2, geometry3);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 5);

        obs = (Geom2)Hull(geometry1, geometry2, geometry3);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 6);

        obs = (Geom2)Hull(geometry5, geometry4);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 8);
    }

    [Test]
    public void TestHullSingleGeom3()
    {
        var geometry = new Geom3();

        var obs = (Geom3)Hull(geometry);
        var pts = obs.ToPoints();

        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Count, 0);

        geometry = Sphere(radius: 2, segments: 8);
        Assert.DoesNotThrow(() => geometry.Validate());

        obs = (Geom3)Hull(geometry);
        pts = obs.ToPoints();

        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Count, 32);
    }

    [Test]
    public void TestHullMultipleGeom3()
    {
        var geometry1 = Cuboid(size: (2, 2, 2), center: (0, 0, 0));
        Assert.DoesNotThrow(() => geometry1.Validate());

        var obs = (Geom3)Hull(geometry1, geometry1); // same
        var pts = obs.ToPoints();
        if (WriteTests) TestData.Make("HullMultipleGeom3Exp1", pts);
        var exp = UnitTestData.HullMultipleGeom3Exp1;

        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Count, 6);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));

        var geometry2 = Center(Cuboid(size: (3, 3, 3), center: (0, 0, 0)), relativeTo: (5, 5, 5));

        obs = (Geom3)Hull(geometry1, geometry2);
        pts = obs.ToPoints();
        if (WriteTests) TestData.Make("HullMultipleGeom3Exp2", pts);
        exp = UnitTestData.HullMultipleGeom3Exp2;

        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Count, 12);
        Assert.IsTrue(Helpers.CompareListOfListsNEVec3(pts, exp));
    }

    [Test]
    public void TestHullMultipleOverLappingGeom3()
    {
        var geometry1 = Cuboid(size: (2, 2, 2));
        geometry1 = Ellipsoid(radius: (2, 2, 2), segments: 12);
        var geometry2 = (Geom3)Center(Ellipsoid(radius: (3, 3, 3), segments: 12), relativeTo: (3, -3, 3));
        var geometry3 = (Geom3)Center(Ellipsoid(radius: (3, 3, 3), segments: 12), relativeTo: (-3, -3, -3));
        Assert.DoesNotThrow(() => geometry1.Validate());
        Assert.DoesNotThrow(() => geometry2.Validate());
        Assert.DoesNotThrow(() => geometry3.Validate());

        var obs = (Geom3)Hull(geometry1, geometry2, geometry3);
        var pts = obs.ToPoints();

        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Count, 92);
    }
}
