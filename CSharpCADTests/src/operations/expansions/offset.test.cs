using NUnit.Framework;

using CSharpCAD;

namespace CSharpCADTests;

[TestFixture]
public class OffsetTests
{
    static bool WriteTests = false;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestOffsetOptionsOffsettingSimpleGeom2()
    {
        var geometry = new Geom2(new Vec2[] {
            new Vec2(-5, -5),
            new Vec2(5, -5),
            new Vec2(5, 5),
            new Vec2(3, 5),
            new Vec2(3, 0),
            new Vec2(-3, 0),
            new Vec2(-3, 5),
            new Vec2(-5, 5)
          });
        Assert.DoesNotThrow(() => (geometry).Validate());

        // empty
        var empty = new Geom2();
        var obs = Offset(empty, delta: 1);
        var pts = obs.ToPoints();
        var exp = new Vec2[] { };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());

        // expand +
        obs = Offset(geometry, delta: 1, corners: Corners.Round, segments:0);
        pts = obs.ToPoints();
        exp = new Vec2[] {
          new Vec2(-5, -6),
          new Vec2(5, -6),
          new Vec2(6, -5),
          new Vec2(6, 5),
          new Vec2(5, 6),
          new Vec2(3, 6),
          new Vec2(2, 5),
          new Vec2(2, 1),
          new Vec2(-2, 1),
          new Vec2(-2, 5),
          new Vec2(-3, 6),
          new Vec2(-5, 6),
          new Vec2(-6, 5),
          new Vec2(-6, -5)
        };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());

        // contract -
        obs = Offset(geometry, delta: -0.5, corners: Corners.Round, segments:0);
        pts = obs.ToPoints();
        exp = new Vec2[] {
          new Vec2(-4.5, -4.5),
          new Vec2(4.5, -4.5),
          new Vec2(4.5, 4.5),
          new Vec2(3.5, 4.5),
          new Vec2(3.5, -3.0616171314629196e-17),
          new Vec2(3, -0.5),
          new Vec2(-3, -0.5),
          new Vec2(-3.5, 3.0616171314629196e-17),
          new Vec2(-3.5, 4.5),
          new Vec2(-4.5, 4.5)
      };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());

        // segments 1 - sharp points at corner
        obs = Offset(geometry, delta: 1, corners: Corners.Edge);
        pts = obs.ToPoints();
        exp = new Vec2[] {
          new Vec2(6, -6),
          new Vec2(6, 6),
          new Vec2(2, 6),
          new Vec2(2, 1),
          new Vec2(-2, 1),
          new Vec2(-1.9999999999999996, 6),
          new Vec2(-6, 6),
          new Vec2(-6, -6)
        };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());

        // segments 16 - rounded corners
        obs = Offset(geometry, delta: -0.5, corners: Corners.Round);
        pts = obs.ToPoints();
        exp = new Vec2[] {
          new Vec2(-4.5, -4.5),
          new Vec2(4.5, -4.5),
          new Vec2(4.5, 4.5),
          new Vec2(3.5, 4.5),
          new Vec2(3.5, -3.061616997868383e-17),
          new Vec2(3.4619397662556435, -0.19134171618254492),
          new Vec2(3.353553390593274, -0.3535533905932738),
          new Vec2(3.191341716182545, -0.46193976625564337),
          new Vec2(3, -0.5),
          new Vec2(-3, -0.5),
          new Vec2(-3.191341716182545, -0.46193976625564337),
          new Vec2(-3.353553390593274, -0.3535533905932738),
          new Vec2(-3.4619397662556435, -0.19134171618254495),
          new Vec2(-3.5, 3.061616997868383e-17),
          new Vec2(-3.5, 4.5),
          new Vec2(-4.5, 4.5)
        };
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());
    }

    [Test]
    public void TestOffsetOptionsOffsettingComplexGeom2()
    {
        var geometry = new Geom2(new Geom2.Side[] {
          new Geom2.Side(new Vec2(-75, 75), new Vec2(-75, -75)),
          new Geom2.Side(new Vec2(-75, -75), new Vec2(75, -75)),
          new Geom2.Side(new Vec2(75, -75), new Vec2(75, 75)),
          new Geom2.Side(new Vec2(-40, 75), new Vec2(-75, 75)),
          new Geom2.Side(new Vec2(75, 75), new Vec2(40, 75)),
          new Geom2.Side(new Vec2(40, 75), new Vec2(40, 0)),
          new Geom2.Side(new Vec2(40, 0), new Vec2(-40, 0)),
          new Geom2.Side(new Vec2(-40, 0), new Vec2(-40, 75)),
          new Geom2.Side(new Vec2(15, -10), new Vec2(15, -40)),
          new Geom2.Side(new Vec2(-15, -10), new Vec2(15, -10)),
          new Geom2.Side(new Vec2(-15, -40), new Vec2(-15, -10)),
          new Geom2.Side(new Vec2(-8, -40), new Vec2(-15, -40)),
          new Geom2.Side(new Vec2(15, -40), new Vec2(8, -40)),
          new Geom2.Side(new Vec2(-8, -25), new Vec2(-8, -40)),
          new Geom2.Side(new Vec2(8, -25), new Vec2(-8, -25)),
          new Geom2.Side(new Vec2(8, -40), new Vec2(8, -25)),
          new Geom2.Side(new Vec2(-2, -15), new Vec2(-2, -19)),
          new Geom2.Side(new Vec2(-2, -19), new Vec2(2, -19)),
          new Geom2.Side(new Vec2(2, -19), new Vec2(2, -15)),
          new Geom2.Side(new Vec2(2, -15), new Vec2(-2, -15))
        });
        Assert.DoesNotThrow(() => (geometry).Validate());

        // expand +
        var obs = Offset(geometry, delta: 2, corners: Corners.Edge);
        var pts = obs.ToPoints();
        if(WriteTests) TestData.Make("OffsetPlusComplexExp1", pts);
        var exp = UnitTestData.OffsetPlusComplexExp1;
        Assert.AreEqual(pts.Length, 20);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());
    }

    [Test]
    public void TestOffsetOptionsOffsettingRoundGeom2()
    {
        var geometry = new Geom2(new Vec2[] {
            new Vec2(10.00000, 0.00000),
            new Vec2(9.23880, 3.82683),
            new Vec2(7.07107, 7.07107),
            new Vec2(3.82683, 9.23880),
            new Vec2(0.00000, 10.00000),
            new Vec2(-3.82683, 9.23880),
            new Vec2(-7.07107, 7.07107),
            new Vec2(-9.23880, 3.82683),
            new Vec2(-10.00000, 0.00000),
            new Vec2(-9.23880, -3.82683),
            new Vec2(-7.07107, -7.07107),
            new Vec2(-3.82683, -9.23880),
            new Vec2(-0.00000, -10.00000),
            new Vec2(3.82683, -9.23880),
            new Vec2(7.07107, -7.07107),
            new Vec2(9.23880, -3.82683)
        });
        Assert.DoesNotThrow(() => (geometry).Validate());

        var obs = Offset(geometry, delta: -0.5, corners: Corners.Round);
        var pts = obs.ToPoints();
        var exp = new Vec2[] {
          new Vec2(9.490204518135641, 0),
          new Vec2(8.767810140100096, 3.6317399864658007),
          new Vec2(6.710590060510285, 6.7105900605102855),
          new Vec2(3.6317399864658024, 8.767810140100096),
          new Vec2(-4.440892098500626e-16, 9.490204518135641),
          new Vec2(-3.6317399864658007, 8.767810140100096),
          new Vec2(-6.7105900605102855, 6.710590060510285),
          new Vec2(-8.767810140100096, 3.6317399864658024),
          new Vec2(-9.490204518135641, -4.440892098500626e-16),
          new Vec2(-8.767810140100096, -3.6317399864658007),
          new Vec2(-6.710590060510285, -6.7105900605102855),
          new Vec2(-3.6317399864658024, -8.767810140100096),
          new Vec2(4.440892098500626e-16, -9.490204518135641),
          new Vec2(3.6317399864658007, -8.767810140100096),
          new Vec2(6.7105900605102855, -6.710590060510285),
          new Vec2(8.767810140100096, -3.6317399864658024)
        };
        Assert.AreEqual(pts.Length, exp.Length);
        Assert.IsTrue(Helpers.CompareArraysNEVec2(pts, exp));
        Assert.DoesNotThrow(() => (obs).Validate());
    }
}