namespace CSharpCADTests;

[TestFixture]
public class HullTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestHullSingleGeom2()
    {
        var geometry = new Geom2();

        var obs = Hull(geometry);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 0);

        geometry = new Geom2(new Vec2[] { new Vec2(5, 5), new Vec2(-5, 5), new Vec2(-5, -5), new Vec2(5, -5) });
        Assert.DoesNotThrow(() => geometry.Validate());
        obs = Hull(geometry);
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
        obs = Hull(geometry);
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
        var obs = Hull(geometry1, geometry1);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 4);

        // one inside another
        obs = Hull(geometry1, geometry2);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 4);

        // one overlapping another
        obs = Hull(geometry1, geometry3);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();

        Assert.AreEqual(pts.Length, 8);

        obs = Hull(geometry2, geometry4);
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

        var obs = Hull(geometry1, geometry2);
        Assert.DoesNotThrow(() => obs.Validate());
        var pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 5);

        obs = Hull(geometry1, geometry3);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 5);

        obs = Hull(geometry2, geometry3);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 5);

        obs = Hull(geometry1, geometry2, geometry3);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 6);

        obs = Hull(geometry5, geometry4);
        Assert.DoesNotThrow(() => obs.Validate());
        pts = obs.ToPoints();
        Assert.AreEqual(pts.Length, 8);
    }
}
