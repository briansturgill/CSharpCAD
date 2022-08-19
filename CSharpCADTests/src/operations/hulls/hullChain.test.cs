namespace CSharpCADTests;

[TestFixture]
public class HullChainTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestHullChainTwoGeom2()
    {
        var geometry1 = new Geom2(new List<Vec2> { new Vec2(6, 6), new Vec2(3, 6), new Vec2(3, 3), new Vec2(6, 3) });
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(-6, -6), new Vec2(-9, -6), new Vec2(-9, -9), new Vec2(-6, -9) });
        Assert.DoesNotThrow(() => geometry1.Validate());
        Assert.DoesNotThrow(() => geometry2.Validate());

        // same
        var obs = HullChain(geometry1, geometry1);
        var pts = obs.ToPoints();

        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Length, 4);

        // different
        obs = HullChain(geometry1, geometry2);
        pts = obs.ToPoints();

        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Length, 6);
    }

    [Test]
    public void TestHullChainThreeGeom2()
    {
        var geometry1 = new Geom2(new List<Vec2> { new Vec2(6, 6), new Vec2(3, 6), new Vec2(3, 3), new Vec2(6, 3) });
        var geometry2 = new Geom2(new List<Vec2> { new Vec2(-6, -6), new Vec2(-9, -6), new Vec2(-9, -9), new Vec2(-6, -9) });
        var geometry3 = new Geom2(new List<Vec2> { new Vec2(-6, 6), new Vec2(-3, 6), new Vec2(-3, 9), new Vec2(-6, 9) });
        Assert.DoesNotThrow(() => geometry1.Validate());
        Assert.DoesNotThrow(() => geometry2.Validate());
        Assert.DoesNotThrow(() => geometry3.Validate());

        // open
        var obs = HullChain(geometry1, geometry2, geometry3);
        var pts = obs.ToPoints();

        // the sides change based on the bestplane chosen in trees/Node.js
        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Length, 10);

        // closed
        obs = HullChain(geometry1, geometry2, geometry3, geometry1);
        pts = obs.ToPoints();

        // the sides change based on the bestplane chosen in trees/Node.js
        Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Length, 10);
    }

    [Test]
    public void TestHullChainThreeGeom3()
    {
        var geometry1 = new Geom3(new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-1, -1, -1), new Vec3(-1, -1, 1), new Vec3(-1, 1, 1), new Vec3(-1, 1, -1)},
          new List<Vec3>{new Vec3(1, -1, -1), new Vec3(1, 1, -1), new Vec3(1, 1, 1), new Vec3(1, -1, 1)},
          new List<Vec3>{new Vec3(-1, -1, -1), new Vec3(1, -1, -1), new Vec3(1, -1, 1), new Vec3(-1, -1, 1)},
          new List<Vec3>{new Vec3(-1, 1, -1), new Vec3(-1, 1, 1), new Vec3(1, 1, 1), new Vec3(1, 1, -1)},
          new List<Vec3>{new Vec3(-1, -1, -1), new Vec3(-1, 1, -1), new Vec3(1, 1, -1), new Vec3(1, -1, -1)},
          new List<Vec3>{new Vec3(-1, -1, 1), new Vec3(1, -1, 1), new Vec3(1, 1, 1), new Vec3(-1, 1, 1)}
        });
        var geometry2 = new Geom3(new List<List<Vec3>> {
            new List<Vec3>{new Vec3(3.5, 3.5, 3.5), new Vec3(3.5, 3.5, 6.5), new Vec3(3.5, 6.5, 6.5), new Vec3(3.5, 6.5, 3.5)},
            new List<Vec3>{new Vec3(6.5, 3.5, 3.5), new Vec3(6.5, 6.5, 3.5), new Vec3(6.5, 6.5, 6.5), new Vec3(6.5, 3.5, 6.5)},
            new List<Vec3>{new Vec3(3.5, 3.5, 3.5), new Vec3(6.5, 3.5, 3.5), new Vec3(6.5, 3.5, 6.5), new Vec3(3.5, 3.5, 6.5)},
            new List<Vec3>{new Vec3(3.5, 6.5, 3.5), new Vec3(3.5, 6.5, 6.5), new Vec3(6.5, 6.5, 6.5), new Vec3(6.5, 6.5, 3.5)},
            new List<Vec3>{new Vec3(3.5, 3.5, 3.5), new Vec3(3.5, 6.5, 3.5), new Vec3(6.5, 6.5, 3.5), new Vec3(6.5, 3.5, 3.5)},
            new List<Vec3>{new Vec3(3.5, 3.5, 6.5), new Vec3(6.5, 3.5, 6.5), new Vec3(6.5, 6.5, 6.5), new Vec3(3.5, 6.5, 6.5)}
          });
        var geometry3 = new Geom3(new List<List<Vec3>> {
          new List<Vec3>{new Vec3(-4.5, 1.5, -4.5), new Vec3(-4.5, 1.5, -1.5), new Vec3(-4.5, 4.5, -1.5), new Vec3(-4.5, 4.5, -4.5)},
          new List<Vec3>{new Vec3(-1.5, 1.5, -4.5), new Vec3(-1.5, 4.5, -4.5), new Vec3(-1.5, 4.5, -1.5), new Vec3(-1.5, 1.5, -1.5)},
          new List<Vec3>{new Vec3(-4.5, 1.5, -4.5), new Vec3(-1.5, 1.5, -4.5), new Vec3(-1.5, 1.5, -1.5), new Vec3(-4.5, 1.5, -1.5)},
          new List<Vec3>{new Vec3(-4.5, 4.5, -4.5), new Vec3(-4.5, 4.5, -1.5), new Vec3(-1.5, 4.5, -1.5), new Vec3(-1.5, 4.5, -4.5)},
          new List<Vec3>{new Vec3(-4.5, 1.5, -4.5), new Vec3(-4.5, 4.5, -4.5), new Vec3(-1.5, 4.5, -4.5), new Vec3(-1.5, 1.5, -4.5)},
          new List<Vec3>{new Vec3(-4.5, 1.5, -1.5), new Vec3(-1.5, 1.5, -1.5), new Vec3(-1.5, 4.5, -1.5), new Vec3(-4.5, 4.5, -1.5)}
        });
        Assert.DoesNotThrow(() => geometry1.Validate());
        Assert.DoesNotThrow(() => geometry2.Validate());
        Assert.DoesNotThrow(() => geometry3.Validate());

        // open
        var obs = HullChain(geometry1, geometry2, geometry3);
        var pts = obs.ToPoints();
        // LATER JSCAD Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Count, 62);

        // closed
        obs = HullChain(geometry1, geometry2, geometry3, geometry1);
        pts = obs.ToPoints();
        // LATER JSCAD Assert.DoesNotThrow(() => obs.Validate());
        Assert.AreEqual(pts.Count, 122);
    }
}
