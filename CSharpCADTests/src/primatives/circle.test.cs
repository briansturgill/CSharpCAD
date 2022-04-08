using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class CircleTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCircleDefaults()
    {
        var geometry = Circle(new Opts { { "radius", 2 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        var pts = ((Geom2)geometry).ToPoints();

        Assert.IsTrue(pts.Length == 32);
    }


    [Test]
    public void TestCircleOptions()
    {
        // test center
        var geometry = Circle(new Opts { { "radius", 3.5 }, { "center", (6.5, 6.5) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        var pts = ((Geom2)geometry).ToPoints();
        var exp = new Vec2[] {
          new Vec2(10, 6.5),
          new Vec2(9.932748481411306, 7.182816127056449),
          new Vec2(9.733578363789503, 7.8393920132778145),
          new Vec2(9.410143643058909, 8.444495815568608),
          new Vec2(8.974873734152917, 8.974873734152915),
          new Vec2(8.444495815568608, 9.410143643058909),
          new Vec2(7.8393920132778145, 9.733578363789503),
          new Vec2(7.182816127056449, 9.932748481411306),
          new Vec2(6.5, 10),
          new Vec2(5.817183872943551, 9.932748481411306),
          new Vec2(5.1606079867221855, 9.733578363789503),
          new Vec2(4.555504184431394, 9.410143643058909),
          new Vec2(4.025126265847084, 8.974873734152917),
          new Vec2(3.589856356941091, 8.444495815568608),
          new Vec2(3.2664216362104965, 7.8393920132778145),
          new Vec2(3.0672515185886935, 7.18281612705645),
          new Vec2(3, 6.5),
          new Vec2(3.0672515185886935, 5.81718387294355),
          new Vec2(3.266421636210496, 5.160607986722186),
          new Vec2(3.589856356941091, 4.555504184431394),
          new Vec2(4.025126265847083, 4.025126265847084),
          new Vec2(4.555504184431392, 3.5898563569410915),
          new Vec2(5.160607986722184, 3.266421636210497),
          new Vec2(5.817183872943549, 3.067251518588694),
          new Vec2(6.499999999999999, 3),
          new Vec2(7.182816127056449, 3.0672515185886935),
          new Vec2(7.8393920132778145, 3.266421636210497),
          new Vec2(8.444495815568606, 3.589856356941091),
          new Vec2(8.974873734152915, 4.025126265847083),
          new Vec2(9.410143643058909, 4.555504184431392),
          new Vec2(9.733578363789503, 5.160607986722184),
          new Vec2(9.932748481411306, 5.817183872943549)
        };

        Assert.AreEqual(pts.Length, 32);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test radius
        geometry = Circle(new Opts { { "radius", 3.5 }, { "segments", 16 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = ((Geom2)geometry).ToPoints();
        exp = new Vec2[] {
          new Vec2(3.5, 0),
          new Vec2(3.2335783637895035, 1.3393920132778143),
          new Vec2(2.4748737341529163, 2.474873734152916),
          new Vec2(1.3393920132778145, 3.2335783637895035),
          new Vec2(2.143131898507868e-16, 3.5),
          new Vec2(-1.339392013277814, 3.2335783637895035),
          new Vec2(-2.474873734152916, 2.4748737341529163),
          new Vec2(-3.2335783637895035, 1.3393920132778145),
          new Vec2(-3.5, 4.286263797015736e-16),
          new Vec2(-3.233578363789504, -1.3393920132778139),
          new Vec2(-2.474873734152917, -2.474873734152916),
          new Vec2(-1.339392013277816, -3.233578363789503),
          new Vec2(-6.429395695523604e-16, -3.5),
          new Vec2(1.339392013277815, -3.233578363789503),
          new Vec2(2.474873734152916, -2.474873734152917),
          new Vec2(3.233578363789503, -1.3393920132778163)
        };

        Assert.AreEqual(pts.Length, 16);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test startAngle
        geometry = Circle(new Opts { { "radius", 3.5 }, { "startAngle", Math.PI / 2 }, { "segments", 16 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = ((Geom2)geometry).ToPoints();
        exp = new Vec2[] {
          new Vec2(2.143131898507868e-16, 3.5),
          new Vec2(-1.339392013277814, 3.2335783637895035),
          new Vec2(-2.474873734152916, 2.4748737341529163),
          new Vec2(-3.2335783637895035, 1.3393920132778145),
          new Vec2(-3.5, 4.286263797015736e-16),
          new Vec2(-3.233578363789504, -1.3393920132778139),
          new Vec2(-2.474873734152917, -2.474873734152916),
          new Vec2(-1.339392013277816, -3.233578363789503),
          new Vec2(-6.429395695523604e-16, -3.5),
          new Vec2(1.339392013277815, -3.233578363789503),
          new Vec2(2.474873734152916, -2.474873734152917),
          new Vec2(3.233578363789503, -1.3393920132778163),
          new Vec2(3.5, -8.572527594031472e-16),
          new Vec2(0, 0)
        };

        Assert.AreEqual(pts.Length, 14);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test endAngle
        geometry = Circle(new Opts { { "radius", 3.5 }, { "endAngle", Math.PI / 2 }, { "segments", 16 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = ((Geom2)geometry).ToPoints();
        exp = new Vec2[] {
          new Vec2(3.5, 0),
          new Vec2(3.2335783637895035, 1.3393920132778143),
          new Vec2(2.4748737341529163, 2.474873734152916),
          new Vec2(1.3393920132778145, 3.2335783637895035),
          new Vec2(2.143131898507868e-16, 3.5),
          new Vec2(0, 0)
        };

        Assert.AreEqual(pts.Length, 6);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test segments
        geometry = Circle(new Opts { { "radius", 3.5 }, { "segments", 5 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = ((Geom2)geometry).ToPoints();
        exp = new Vec2[] {
          new Vec2(3.5, 0),
          new Vec2(1.081559480312316, 3.3286978070330373),
          new Vec2(-2.8315594803123156, 2.0572483830236563),
          new Vec2(-2.831559480312316, -2.0572483830236554),
          new Vec2(1.0815594803123152, -3.3286978070330377)
        };

        Assert.AreEqual(pts.Length, 5);
        Assert.True(Helpers.CompareArraysNEVec2(pts, exp));
    }
}
