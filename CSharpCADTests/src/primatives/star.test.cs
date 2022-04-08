using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class StarTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestStarDefaults()
    {
        var geometry = Star(new Opts());
        Assert.DoesNotThrow(() => geometry.Validate());
        var pts = geometry.ToPoints();
        var exp = new Vec2[] {
          new Vec2(1, 0),
          new Vec2(0.30901699437494745, 0.2245139882897927),
          new Vec2(0.30901699437494745, 0.9510565162951535),
          new Vec2(-0.11803398874989482, 0.36327126400268045),
          new Vec2(-0.8090169943749473, 0.5877852522924732),
          new Vec2(-0.38196601125010515, 4.6777345306052316e-17),
          new Vec2(-0.8090169943749475, -0.587785252292473),
          new Vec2(-0.1180339887498949, -0.36327126400268045),
          new Vec2(0.30901699437494723, -0.9510565162951536),
          new Vec2(0.3090169943749474, -0.22451398828979277)
        };

        Assert.AreEqual(pts.Length, 10);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
    }

    [Test]
    public void TestStarOptions()
    {
        // test center
        var geometry = Star(new Opts { { "outerRadius", 5 }, { "center", (5, 5) } });
        Assert.DoesNotThrow(() => geometry.Validate());
        var pts = geometry.ToPoints();
        var exp = new Vec2[] {
          new Vec2(10, 5),
          new Vec2(6.545084971874737, 6.122569941448964),
          new Vec2(6.545084971874737, 9.755282581475768),
          new Vec2(4.4098300562505255, 6.816356320013402),
          new Vec2(0.9549150281252636, 7.938926261462367),
          new Vec2(3.0901699437494745, 5),
          new Vec2(0.9549150281252627, 2.061073738537635),
          new Vec2(4.4098300562505255, 3.1836436799865977),
          new Vec2(6.545084971874736, 0.2447174185242318),
          new Vec2(6.545084971874736, 3.8774300585510364)
        };

        Assert.AreEqual(pts.Length, 10);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test vertices
        geometry = Star(new Opts { { "outerRadius", 5 }, { "vertices", 8 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(5, 0),
          new Vec2(3.5355339059327378, 1.4644660940672625),
          new Vec2(3.5355339059327378, 3.5355339059327373),
          new Vec2(1.4644660940672627, 3.5355339059327378),
          new Vec2(3.061616997868383e-16, 5),
          new Vec2(-1.4644660940672622, 3.5355339059327378),
          new Vec2(-3.5355339059327373, 3.5355339059327378),
          new Vec2(-3.5355339059327378, 1.464466094067263),
          new Vec2(-5, 6.123233995736766e-16),
          new Vec2(-3.535533905932738, -1.464466094067262),
          new Vec2(-3.5355339059327386, -3.5355339059327373),
          new Vec2(-1.4644660940672647, -3.535533905932737),
          new Vec2(-9.184850993605148e-16, -5),
          new Vec2(1.4644660940672634, -3.5355339059327373),
          new Vec2(3.535533905932737, -3.5355339059327386),
          new Vec2(3.535533905932737, -1.464466094067265)
        };

        Assert.AreEqual(pts.Length, 16);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test density
        geometry = Star(new Opts { { "outerRadius", 5 }, { "vertices", 8 }, { "density", 3 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(5, 0),
          new Vec2(2.5, 1.0355339059327378),
          new Vec2(3.5355339059327378, 3.5355339059327373),
          new Vec2(1.0355339059327378, 2.5),
          new Vec2(3.061616997868383e-16, 5),
          new Vec2(-1.0355339059327375, 2.5),
          new Vec2(-3.5355339059327373, 3.5355339059327378),
          new Vec2(-2.5, 1.035533905932738),
          new Vec2(-5, 6.123233995736766e-16),
          new Vec2(-2.5000000000000004, -1.0355339059327373),
          new Vec2(-3.5355339059327386, -3.5355339059327373),
          new Vec2(-1.035533905932739, -2.4999999999999996),
          new Vec2(-9.184850993605148e-16, -5),
          new Vec2(1.0355339059327382, -2.4999999999999996),
          new Vec2(3.535533905932737, -3.5355339059327386),
          new Vec2(2.4999999999999996, -1.0355339059327393)
        };

        Assert.AreEqual(pts.Length, 16);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test innerRadius
        geometry = Star(new Opts { { "outerRadius", 5 }, { "vertices", 8 }, { "innerRadius", 1 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(5, 0),
          new Vec2(0.9238795325112867, 0.3826834323650898),
          new Vec2(3.5355339059327378, 3.5355339059327373),
          new Vec2(0.38268343236508984, 0.9238795325112867),
          new Vec2(3.061616997868383e-16, 5),
          new Vec2(-0.3826834323650897, 0.9238795325112867),
          new Vec2(-3.5355339059327373, 3.5355339059327378),
          new Vec2(-0.9238795325112867, 0.3826834323650899),
          new Vec2(-5, 6.123233995736766e-16),
          new Vec2(-0.9238795325112868, -0.38268343236508967),
          new Vec2(-3.5355339059327386, -3.5355339059327373),
          new Vec2(-0.38268343236509034, -0.9238795325112865),
          new Vec2(-9.184850993605148e-16, -5),
          new Vec2(0.38268343236509, -0.9238795325112866),
          new Vec2(3.535533905932737, -3.5355339059327386),
          new Vec2(0.9238795325112865, -0.3826834323650904)
        };

        Assert.AreEqual(pts.Length, 16);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));

        // test start angle
        geometry = Star(new Opts { { "outerRadius", 5 }, { "startAngle", (360 - 45) * 0.017453292519943295 } });
        Assert.DoesNotThrow(() => geometry.Validate());
        pts = geometry.ToPoints();
        exp = new Vec2[] {
          new Vec2(3.535533905932737, -3.5355339059327386),
          new Vec2(1.8863168790768001, -0.2987632431673055),
          new Vec2(4.45503262094184, 2.269952498697733),
          new Vec2(0.8670447016547834, 1.701671040210256),
          new Vec2(-0.7821723252011483, 4.938441702975689),
          new Vec2(-1.3504537836886306, 1.350453783688634),
          new Vec2(-4.9384417029756875, 0.7821723252011604),
          new Vec2(-1.701671040210257, -0.8670447016547808),
          new Vec2(-2.26995249869774, -4.455032620941836),
          new Vec2(0.2987632431673025, -1.8863168790768006)
        };

        Assert.AreEqual(pts.Length, 10);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
    }
}
